using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Media;
using libdebug;
using Microsoft.Win32;
using PS4GSCInjector.GameProfiles;
using TreyarchCompiler.Utilities;
using Forms = System.Windows.Forms;
using Ps4Process = libdebug.Process;

namespace PS4GSCInjector
{
    public partial class MainWindow : Window
    {
        private static readonly Brush MutedBrush = new SolidColorBrush(Color.FromRgb(164, 164, 186));
        private static readonly Brush SuccessBrush = new SolidColorBrush(Color.FromRgb(74, 222, 128));
        private static readonly Brush WarningBrush = new SolidColorBrush(Color.FromRgb(250, 204, 21));
        private static readonly Brush DangerBrush = new SolidColorBrush(Color.FromRgb(248, 113, 113));

        private PS4DBG ps4;
        private Ps4Process attachedProcess;
        private GameTargetOption selectedTarget;
        private readonly Dictionary<string, InjectedScriptAllocation> injectedScripts =
            new Dictionary<string, InjectedScriptAllocation>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowServices.UseImmersiveDarkMode(this);

            var ps4Ip = Properties.Settings.Default.ps4ip;
            var ps4Port = Properties.Settings.Default.ps4Port;
            Ps4IpTextBox.Text = ps4Ip;
            Ps4PortTextBox.Text = string.IsNullOrEmpty(ps4Port) ? "9090" : ps4Port;
            PayloadTextBlock.Text = "Payload: ps4debug v1.1.19";
            SetConnectionStatus("Not attached", MutedBrush);

            LoadGameTargets();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectDebugger();
        }

        private void LoadGameTargets()
        {
            GameListBox.Items.Clear();

            foreach (var profile in GameProfileRegistry.All)
            {
                GameListBox.Items.Add(profile);
            }

            if (GameListBox.Items.Count > 0)
            {
                GameListBox.SelectedIndex = 0;
            }
        }

        private void LoadGameVersions()
        {
            var profile = GameListBox.SelectedItem as IGscGameProfile;
            GameVersionComboBox.Items.Clear();

            if (profile == null)
            {
                return;
            }

            foreach (var version in profile.Versions)
            {
                GameVersionComboBox.Items.Add(version);
            }

            if (GameVersionComboBox.Items.Count > 0)
            {
                GameVersionComboBox.SelectedIndex = 0;
            }
        }

        private void UpdateSelectedTarget()
        {
            var profile = GameListBox.SelectedItem as IGscGameProfile;
            var version = GameVersionComboBox.SelectedItem as GameVersionProfile;
            if (profile == null || version == null)
            {
                return;
            }

            var previousProfileId = selectedTarget?.Profile.Id;
            selectedTarget = new GameTargetOption(profile, version);

            if (previousProfileId != null &&
                !string.Equals(previousProfileId, selectedTarget.Profile.Id, StringComparison.OrdinalIgnoreCase) &&
                (ps4 != null || attachedProcess != null))
            {
                DisconnectDebugger();
                SetConnectionStatus("Not attached", MutedBrush);
            }

            AttachGameButton.Content = selectedTarget.Profile.AttachButtonText;
            CompilerTitleTextBlock.Text = selectedTarget.Profile.DisplayName + " GSC Compiler";
            InjectorTitleTextBlock.Text = selectedTarget.Profile.DisplayName + " GSC Injector";

            CompileGscProjectButton.IsEnabled = selectedTarget.Profile.CanCompile;
            GscProjectFolderTextBox.IsEnabled = selectedTarget.Profile.CanCompile;
            CompiledGscFileOutputTextBox.IsEnabled = selectedTarget.Profile.CanCompile;
            BrowseGscFolderButton.IsEnabled = selectedTarget.Profile.CanCompile;
            BrowseOutputPathButton.IsEnabled = selectedTarget.Profile.CanCompile;
            CompilerPanel.Visibility = selectedTarget.Profile.CanCompile ? Visibility.Visible : Visibility.Collapsed;

            UpdateTargetGuidance();
        }

        private void UpdateTargetGuidance()
        {
            if (selectedTarget == null)
            {
                return;
            }

            if (string.Equals(selectedTarget.Profile.Id, "bo4", StringComparison.OrdinalIgnoreCase))
            {
                TargetHelpTextBlock.Text = "T8 compiler and automatic " + selectedTarget.Version.DisplayName + " script hook.";
                CompilerHelpTextBlock.Text = "Compile a Black Ops 4 GSC project into a T8 .gscc script.";
                InjectorHelpTextBlock.Text = "Load " + selectedTarget.Version.DisplayName + ", attach, then inject a compiled T8 script.";
                return;
            }

            TargetHelpTextBlock.Text = "T7 compiler and direct script injection for " + selectedTarget.Version.DisplayName + ".";
            CompilerHelpTextBlock.Text = "Compile a Black Ops 3 GSC project into a T7 .gscc script.";
            InjectorHelpTextBlock.Text = "Attach in-game, then inject a compiled T7 script.";
        }

        private void SetConnectionStatus(string text, Brush brush)
        {
            ConnectionStatusTextBlock.Text = text;
            ConnectionStatusTextBlock.Foreground = brush;
        }

        private static string GetPayloadFile()
        {
            var exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location ?? AppDomain.CurrentDomain.BaseDirectory);
            return Path.Combine(exeDirectory, "Payloads", "ps4debug.bin");
        }

        private void DisconnectDebugger()
        {
            var debugger = ps4;
            ps4 = null;
            attachedProcess = null;
            injectedScripts.Clear();

            if (debugger == null)
            {
                return;
            }

            try
            {
                debugger.Disconnect();
            }
            catch
            {
            }
        }

        private void SendPayloadButton_Click(object sender, RoutedEventArgs e)
        {
            var savedIp = Properties.Settings.Default.ps4ip;
            var savedPort = Properties.Settings.Default.ps4Port;
            var currentIp = Ps4IpTextBox.Text.Trim();
            var currentPort = Ps4PortTextBox.Text.Trim();

            if (!ConnectionSettings.TryParsePayloadEndpoint(currentIp, currentPort, out var endpoint))
            {
                SetConnectionStatus("Invalid endpoint", DangerBrush);
                ShowError("Enter an IPv4 address and a port from 1 to 65535.", "Invalid Connection Settings");
                return;
            }

            if (string.IsNullOrEmpty(savedIp))
            {
                if (AskYesNo("Would you like to save your PS4 IP and port?", "Save Settings?"))
                {
                    SaveEndpoint(currentIp, currentPort);
                }
            }
            else if (savedIp != currentIp || savedPort != currentPort)
            {
                if (AskYesNo("The IP or port is different from stored settings. Would you like to update the stored settings?", "Update Settings?"))
                {
                    SaveEndpoint(currentIp, currentPort);
                }
            }

            try
            {
                var payloadPath = GetPayloadFile();
                if (!File.Exists(payloadPath))
                {
                    SetConnectionStatus("Payload missing", DangerBrush);
                    ShowError("Payload file not found at:" + Environment.NewLine + payloadPath, "Payload Missing");
                    return;
                }

                using (var payloadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    payloadSocket.ReceiveTimeout = 3000;
                    payloadSocket.SendTimeout = 3000;
                    payloadSocket.Connect(endpoint);
                    payloadSocket.SendFile(payloadPath);
                }

                SetConnectionStatus("Payload sent", WarningBrush);
            }
            catch (Exception ex)
            {
                SetConnectionStatus("Payload failed", DangerBrush);
                ShowError("Failed to send payload:" + Environment.NewLine + ex.Message, "Payload Injection Error");
            }
        }

        private static void SaveEndpoint(string ip, string port)
        {
            Properties.Settings.Default.ps4ip = ip;
            Properties.Settings.Default.ps4Port = port;
            Properties.Settings.Default.Save();
        }

        private void AttachGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTarget == null)
            {
                ShowError("Select a supported game target first.", "No Target Selected");
                return;
            }

            var currentIp = Ps4IpTextBox.Text.Trim();
            if (string.IsNullOrEmpty(currentIp))
            {
                ShowError("Please enter a valid PS4 IP address.", "Invalid IP");
                return;
            }

            DisconnectDebugger();

            try
            {
                ps4 = new PS4DBG(currentIp);
                ps4.Connect();
            }
            catch (Exception ex)
            {
                DisconnectDebugger();
                SetConnectionStatus("Connection failed", DangerBrush);
                ShowError("Failed to connect to ps4debug:" + Environment.NewLine + ex.Message, "Connection Failed");
                return;
            }

            if (!ps4.IsConnected)
            {
                DisconnectDebugger();
                SetConnectionStatus("Connection failed", DangerBrush);
                return;
            }

            Ps4Process proc = null;
            try
            {
                var procList = ps4.GetProcessList();
                if (procList?.processes != null)
                {
                    foreach (Ps4Process process in procList.processes)
                    {
                        if (process.name == selectedTarget.Profile.ProcessName)
                        {
                            proc = process;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisconnectDebugger();
                SetConnectionStatus("Process list failed", DangerBrush);
                ShowError("Error fetching process list:" + Environment.NewLine + ex.Message, "Process Error");
                return;
            }

            if (proc == null)
            {
                DisconnectDebugger();
                SetConnectionStatus("Game not found", DangerBrush);
                ShowError(selectedTarget.Profile.ProcessName + " process not found on PS4. Make sure " + selectedTarget.Profile.DisplayName + " is running.", "Process Not Found");
                return;
            }

            attachedProcess = proc;
            SetConnectionStatus("Attached", SuccessBrush);

            try
            {
                ps4.Notify(222, "Connected to PS4 GSC Injector!");
            }
            catch
            {
            }
        }

        private void BrowseGscFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    GscProjectFolderTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void BrowseOutputPathButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Compiled GSC Files (*.gscc)|*.gscc",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog(this) == true)
            {
                CompiledGscFileOutputTextBox.Text = dialog.FileName;
            }
        }

        private void CompileGscProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GscProjectFolderTextBox.Text) || string.IsNullOrWhiteSpace(CompiledGscFileOutputTextBox.Text))
            {
                ShowError("Please select a GSC project folder and an output location for the compiled .gscc file.", "Fill Out All Fields");
                return;
            }

            if (!Directory.Exists(GscProjectFolderTextBox.Text))
            {
                ShowError("The specified GSC project directory does not exist.", "Directory Not Found");
                return;
            }

            if (selectedTarget == null)
            {
                ShowError("Select a supported game target before compiling.", "No Target Selected");
                return;
            }

            if (!selectedTarget.Profile.CanCompile)
            {
                MessageBox.Show(this, selectedTarget.Profile.CompilerUnavailableMessage, "Compiler Not Available", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var conditionalSymbols = LoadConditionalSymbols(GscProjectFolderTextBox.Text);
            string source;
            var sourceTokens = new List<SourceTokenDef>();

            if (!TryReadProjectSource(GscProjectFolderTextBox.Text, sourceTokens, out source))
            {
                return;
            }

            var ppc = new ConditionalBlocks();
            foreach (var symbol in selectedTarget.Profile.ConditionalSymbols)
            {
                if (!conditionalSymbols.Exists(existingSymbol => string.Equals(existingSymbol, symbol, StringComparison.OrdinalIgnoreCase)))
                {
                    conditionalSymbols.Add(symbol);
                }
            }
            ppc.LoadConditionalTokens(conditionalSymbols);

            try
            {
                source = ppc.ParseSource(source);
            }
            catch (CBSyntaxException error)
            {
                ShowPreprocessorError(error, sourceTokens);
                return;
            }

            CompiledCode code;
            try
            {
                code = selectedTarget.Profile.Compile(source);
            }
            catch (Exception ex)
            {
                ShowError("Unhandled compiler error: " + ex.Message, "Compiler Error");
                return;
            }

            if (!string.IsNullOrEmpty(code.Error))
            {
                ShowError("There was an error compiling your GSC Project:" + Environment.NewLine + code.Error, "Compiler Error");
                return;
            }

            try
            {
                string outputDir = Path.GetDirectoryName(CompiledGscFileOutputTextBox.Text);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                File.WriteAllBytes(CompiledGscFileOutputTextBox.Text, code.CompiledScript);
                MessageBox.Show(this, "Your compiled GSC file has been exported to:" + Environment.NewLine + CompiledGscFileOutputTextBox.Text, "Compile Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("Failed to write compiled output file:" + Environment.NewLine + ex.Message, "File Write Error");
            }
        }

        private static List<string> LoadConditionalSymbols(string projectFolder)
        {
            var conditionalSymbols = new List<string>();
            string projectConfigFile = Path.Combine(projectFolder, "gsc.conf");
            string configFileToRead = File.Exists(projectConfigFile) ? projectConfigFile : (File.Exists("gsc.conf") ? "gsc.conf" : null);

            if (configFileToRead == null)
            {
                return conditionalSymbols;
            }

            foreach (string line in File.ReadAllLines(configFileToRead))
            {
                if (line.Trim().StartsWith("#"))
                {
                    continue;
                }

                var split = line.Trim().Split('=');
                if (split.Length < 2)
                {
                    continue;
                }

                if (string.Equals(split[0].Trim(), "symbols", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (string token in split[1].Trim().Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            conditionalSymbols.Add(token.Trim());
                        }
                    }
                }
            }

            return conditionalSymbols;
        }

        private bool TryReadProjectSource(string projectFolder, List<SourceTokenDef> sourceTokens, out string source)
        {
            source = "";
            var sb = new StringBuilder();
            int currentLineCount = 0;
            int currentCharCount = 0;

            var gscFiles = new List<string>(Directory.EnumerateFiles(projectFolder, "*.gsc", SearchOption.AllDirectories));
            gscFiles.Sort(StringComparer.OrdinalIgnoreCase);

            if (gscFiles.Count == 0)
            {
                MessageBox.Show(this, "No .gsc script files were found in the selected project folder.", "No Scripts Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string projectRoot = projectFolder.TrimEnd('\\', '/');
            foreach (string file in gscFiles)
            {
                var currentSource = new SourceTokenDef();
                string relativePath = file.Length > projectRoot.Length ? file.Substring(projectRoot.Length).TrimStart('\\', '/').Replace("\\", "/") : Path.GetFileName(file);
                currentSource.FilePath = relativePath;
                currentSource.LineStart = currentLineCount;
                currentSource.CharStart = currentCharCount;

                foreach (var line in File.ReadAllLines(file))
                {
                    currentSource.LineMappings[currentLineCount] = (currentCharCount, currentCharCount + line.Length + 1);
                    sb.Append(line);
                    sb.Append("\n");
                    currentLineCount += 1;
                    currentCharCount += line.Length + 1;
                }

                currentSource.LineEnd = currentLineCount;
                currentSource.CharEnd = currentCharCount;
                sourceTokens.Add(currentSource);
                sb.Append("\n");
            }

            source = sb.ToString();
            return true;
        }

        private void ShowPreprocessorError(CBSyntaxException error, List<SourceTokenDef> sourceTokens)
        {
            int errorCharPos = error.ErrorPosition;
            int numLineBreaks = 0;

            foreach (var sourceToken in sourceTokens)
            {
                if (errorCharPos >= sourceToken.CharStart && errorCharPos <= sourceToken.CharEnd)
                {
                    errorCharPos -= numLineBreaks;
                    foreach (var line in sourceToken.LineMappings)
                    {
                        var constraints = line.Value;
                        if (errorCharPos >= constraints.CStart && errorCharPos <= constraints.CEnd)
                        {
                            ShowError("There was an error compiling your GSC Project:" + Environment.NewLine +
                                      error.Message + " in scripts/" + sourceToken.FilePath +
                                      " at line " + (line.Key - sourceToken.LineStart + 1) +
                                      ", position " + (errorCharPos - constraints.CStart),
                                      "Compiler Error");
                            return;
                        }
                    }
                }

                numLineBreaks++;
            }

            ShowError("Preprocessor Syntax Error: " + error.Message, "Compiler Error");
        }

        private void BrowseCompiledGscFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Compiled GSC Files (*.gscc)|*.gscc"
            };

            if (dialog.ShowDialog(this) == true)
            {
                CompiledGscFileTextBox.Text = dialog.FileName;
            }
        }

        private void InjectGscButton_Click(object sender, RoutedEventArgs e)
        {
            if (ps4 == null || !ps4.IsConnected || attachedProcess == null)
            {
                var targetName = selectedTarget?.Profile.DisplayName ?? "a supported game";
                ShowError("Make sure to connect to your PS4 and attach to " + targetName + " first.", "Not Connected");
                return;
            }

            if (string.IsNullOrWhiteSpace(CompiledGscFileTextBox.Text))
            {
                ShowError("Please select a compiled GSC file to inject (.gscc).", "Select GSCC File");
                return;
            }

            byte[] buffer;
            try
            {
                buffer = File.ReadAllBytes(CompiledGscFileTextBox.Text);
            }
            catch (Exception ex)
            {
                ShowError("Could not read compiled GSC file:" + Environment.NewLine + ex.Message, "File Read Error");
                return;
            }

            if (!selectedTarget.Profile.IsValidCompiledScript(buffer))
            {
                ShowError("Selected file is not a valid " + selectedTarget.Profile.CompiledScriptDescription + ".", "Invalid Script");
                return;
            }

            try
            {
                selectedTarget.Profile.InjectCompiledScript(ps4, attachedProcess, selectedTarget.Version, buffer, injectedScripts);

                try
                {
                    ps4.Notify(222, "GSC Script injected!");
                }
                catch
                {
                }

                MessageBox.Show(this, "GSC script successfully injected into " + selectedTarget.Profile.DisplayName + " process.", "Injection Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError("Memory injection failed:" + Environment.NewLine + ex.Message, "Injection Error");
            }
        }

        private void GameListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadGameVersions();
        }

        private void GameVersionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateSelectedTarget();
        }

        private bool AskYesNo(string message, string title)
        {
            return MessageBox.Show(this, message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private void ShowError(string message, string title)
        {
            MessageBox.Show(this, message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
