using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using MetroFramework.Forms;
using libdebug;
using TreyarchCompiler;
using TreyarchCompiler.Utilities;

namespace PS4BO3GSCInjector
{
    public partial class MainWindow : MetroForm
    {
        private PS4DBG ps4;
        private Process attachedProcess;
        private Enums.GameVersion selectedGameVersion = Enums.GameVersion.OneThreeThree;
        private readonly Dictionary<Enums.GameVersion, Tuple<ulong, int, int>> injectedScripts =
            new Dictionary<Enums.GameVersion, Tuple<ulong, int, int>>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            var ps4Ip = Properties.Settings.Default.ps4ip;
            var ps4Port = Properties.Settings.Default.ps4Port;
            ps4IpTextBox.Text = ps4Ip;
            ps4PortTextBox.Text = string.IsNullOrEmpty(ps4Port) ? "9090" : ps4Port;

            if (gameVersionComboBox.SelectedIndex < 0)
                gameVersionComboBox.SelectedIndex = 0;

            ApplyModernStyles();
        }

        private void ApplyModernStyles()
        {
            // Update Title & Author Badge
            this.Text = "PS4 BO3 GSC Injector";
            metroLabel3.Text = "Created by CGhxst";
            metroLabel3.BringToFront();

            // Style Accent Buttons
            StyleButton(connectPS4Button, Color.FromArgb(139, 92, 246), Color.FromArgb(124, 58, 237));
            StyleButton(attachBo3Button, Color.FromArgb(79, 70, 229), Color.FromArgb(67, 56, 202));
            StyleButton(compileGscProjectButton, Color.FromArgb(99, 102, 241), Color.FromArgb(79, 70, 229));
            StyleButton(injectGscButton, Color.FromArgb(16, 185, 129), Color.FromArgb(5, 150, 105));
            StyleButton(browseGscFolderButton, Color.FromArgb(45, 45, 60), Color.FromArgb(60, 60, 80));
            StyleButton(browseOutputPathButton, Color.FromArgb(45, 45, 60), Color.FromArgb(60, 60, 80));
            StyleButton(browseCompiledGscFileButton, Color.FromArgb(45, 45, 60), Color.FromArgb(60, 60, 80));
        }

        private void StyleButton(Button btn, Color normalBg, Color hoverBg)
        {
            if (btn == null) return;
            btn.BackColor = normalBg;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", btn.Font.Size >= 11F ? 10.5F : 9F, FontStyle.Bold);

            btn.MouseEnter += (s, e) => btn.BackColor = hoverBg;
            btn.MouseLeave += (s, e) => btn.BackColor = normalBg;
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
                return;

            try
            {
                debugger.Disconnect();
            }
            catch (Exception)
            {
                // Cleanup is best effort when the console has already disconnected.
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DisconnectDebugger();
            base.OnFormClosed(e);
        }

        private void connectPS4Button_Click(object sender, EventArgs e)
        {
            var savedIp = Properties.Settings.Default.ps4ip;
            var savedPort = Properties.Settings.Default.ps4Port;
            var currentIp = ps4IpTextBox.Text.Trim();
            var currentPort = ps4PortTextBox.Text.Trim();

            if (!ConnectionSettings.TryParsePayloadEndpoint(currentIp, currentPort, out var endpoint))
            {
                connectionStatusLabel.Text = "Invalid IP or Port";
                connectionStatusLabel.ForeColor = Color.Red;
                MetroFramework.MetroMessageBox.Show(this, "Enter an IPv4 address and a port from 1 to 65535.", "Invalid Connection Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(savedIp))
            {
                if (MetroFramework.MetroMessageBox.Show(this, "Would you like to save your PS4 IP & Port?", "Save Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Properties.Settings.Default.ps4ip = currentIp;
                    Properties.Settings.Default.ps4Port = currentPort;
                    Properties.Settings.Default.Save();
                }
            }
            else if (savedIp != currentIp || savedPort != currentPort)
            {
                if (MetroFramework.MetroMessageBox.Show(this, "The IP or Port you entered is different from stored settings. Would you like to update the stored settings?", "Update Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Properties.Settings.Default.ps4ip = currentIp;
                    Properties.Settings.Default.ps4Port = currentPort;
                    Properties.Settings.Default.Save();
                }
            }

            try
            {
                var payloadPath = GetPayloadFile();
                if (!File.Exists(payloadPath))
                {
                    connectionStatusLabel.Text = "Payload File Missing";
                    connectionStatusLabel.ForeColor = Color.Red;
                    MetroFramework.MetroMessageBox.Show(this, $"Payload file not found at:\n{payloadPath}", "Payload Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var payloadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    payloadSocket.ReceiveTimeout = 3000;
                    payloadSocket.SendTimeout = 3000;
                    payloadSocket.Connect(endpoint);
                    payloadSocket.SendFile(payloadPath);
                }

                connectionStatusLabel.Text = "Payload Injected";
                connectionStatusLabel.ForeColor = Color.YellowGreen;
            }
            catch (Exception ex)
            {
                connectionStatusLabel.Text = "Injection Failed";
                connectionStatusLabel.ForeColor = Color.Red;
                MetroFramework.MetroMessageBox.Show(this, $"Failed to send payload:\n{ex.Message}", "Payload Injection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void attachBo3Button_Click(object sender, EventArgs e)
        {
            var currentIp = ps4IpTextBox.Text.Trim();
            if (string.IsNullOrEmpty(currentIp))
            {
                MetroFramework.MetroMessageBox.Show(this, "Please enter a valid PS4 IP address.", "Invalid IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                connectionStatusLabel.Text = "Connection Failed";
                connectionStatusLabel.ForeColor = Color.Red;
                MetroFramework.MetroMessageBox.Show(this, $"Failed to connect to ps4debug:\n{ex.Message}", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ps4.IsConnected)
            {
                DisconnectDebugger();
                connectionStatusLabel.Text = "Connection Failed";
                connectionStatusLabel.ForeColor = Color.Red;
                return;
            }

            Process proc = null;
            try
            {
                var procList = ps4.GetProcessList();
                if (procList?.processes != null)
                {
                    foreach (libdebug.Process process in procList.processes)
                    {
                        if (process.name == "eboot.bin")
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
                connectionStatusLabel.Text = "Process List Error";
                connectionStatusLabel.ForeColor = Color.Red;
                MetroFramework.MetroMessageBox.Show(this, $"Error fetching process list:\n{ex.Message}", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (proc == null)
            {
                DisconnectDebugger();
                connectionStatusLabel.Text = "Process Not Found";
                connectionStatusLabel.ForeColor = Color.Red;
                MetroFramework.MetroMessageBox.Show(this, "eboot.bin process not found on PS4. Make sure Black Ops 3 is running.", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            attachedProcess = proc;

            connectionStatusLabel.Text = "Connected + Attached";
            connectionStatusLabel.ForeColor = Color.Green;

            try
            {
                ps4.Notify(222, "Connected to PS4 BO3 GSC Injector!");
            }
            catch { }
        }

        private void browseGscFolderButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    gscProjectFolderTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void browseOutputPathButton_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Compiled GSC Files (*.gscc)|*.gscc";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    compiledGscFileOutputTextBox.Text = saveFileDialog.FileName;
                }
            }
        }

        private void compileGscProjectButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(gscProjectFolderTextBox.Text) || string.IsNullOrWhiteSpace(compiledGscFileOutputTextBox.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Please select a GSC project folder and an output location for the compiled .gscc file.", "Fill Out All Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(gscProjectFolderTextBox.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "The specified GSC project directory does not exist.", "Directory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> conditionalSymbols = new List<string>();

            string projectConfigFile = Path.Combine(gscProjectFolderTextBox.Text, "gsc.conf");
            string configFileToRead = File.Exists(projectConfigFile) ? projectConfigFile : (File.Exists("gsc.conf") ? "gsc.conf" : null);

            if (configFileToRead != null)
            {
                foreach (string line in File.ReadAllLines(configFileToRead))
                {
                    if (line.Trim().StartsWith("#")) continue;
                    var split = line.Trim().Split('=');
                    if (split.Length < 2) continue;
                    switch (split[0].ToLower().Trim())
                    {
                        case "symbols":
                            foreach (string token in split[1].Trim().Split(','))
                            {
                                if (!string.IsNullOrWhiteSpace(token))
                                    conditionalSymbols.Add(token.Trim());
                            }
                            break;
                    }
                }
            }

            string source = "";
            CompiledCode code;
            List<SourceTokenDef> sourceTokens = new List<SourceTokenDef>();
            StringBuilder sb = new StringBuilder();
            int currentLineCount = 0;
            int currentCharCount = 0;

            var gscFiles = new List<string>(Directory.EnumerateFiles(gscProjectFolderTextBox.Text, "*.gsc", SearchOption.AllDirectories));
            gscFiles.Sort(StringComparer.OrdinalIgnoreCase);

            if (gscFiles.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "No .gsc script files were found in the selected project folder.", "No Scripts Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string projectRoot = gscProjectFolderTextBox.Text.TrimEnd('\\', '/');

            foreach (string file in gscFiles)
            {
                var CurrentSource = new SourceTokenDef();
                string relativePath = file.Length > projectRoot.Length ? file.Substring(projectRoot.Length).TrimStart('\\', '/').Replace("\\", "/") : Path.GetFileName(file);
                CurrentSource.FilePath = relativePath;
                CurrentSource.LineStart = currentLineCount;
                CurrentSource.CharStart = currentCharCount;
                foreach (var line in File.ReadAllLines(file))
                {
                    CurrentSource.LineMappings[currentLineCount] = (currentCharCount, currentCharCount + line.Length + 1);
                    sb.Append(line);
                    sb.Append("\n");
                    currentLineCount += 1;
                    currentCharCount += line.Length + 1;
                }
                CurrentSource.LineEnd = currentLineCount;
                CurrentSource.CharEnd = currentCharCount;
                sourceTokens.Add(CurrentSource);
                sb.Append("\n");
            }
            source = sb.ToString();

            var ppc = new ConditionalBlocks();
            if (!conditionalSymbols.Exists(symbol => string.Equals(symbol, "BO3", StringComparison.OrdinalIgnoreCase)))
                conditionalSymbols.Add("BO3");
            ppc.LoadConditionalTokens(conditionalSymbols);

            try
            {
                source = ppc.ParseSource(source);
            }
            catch (CBSyntaxException error)
            {
                int errorCharPos = error.ErrorPosition;
                int numLineBreaks = 0;
                foreach (var stok in sourceTokens)
                {
                    if (errorCharPos >= stok.CharStart && errorCharPos <= stok.CharEnd)
                    {
                        errorCharPos -= numLineBreaks;
                        foreach (var line in stok.LineMappings)
                        {
                            var constraints = line.Value;
                            if (errorCharPos >= constraints.CStart && errorCharPos <= constraints.CEnd)
                            {
                                MetroFramework.MetroMessageBox.Show(this, $"There was an error compiling your GSC Project:\n{error.Message} in scripts/{stok.FilePath} at line {line.Key - stok.LineStart + 1}, position {errorCharPos - constraints.CStart}", "Compiler Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    numLineBreaks++;
                }
                MetroFramework.MetroMessageBox.Show(this, $"Preprocessor Syntax Error: {error.Message}", "Compiler Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                code = Compiler.Compile(false, source);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, $"Unhandled compiler error: {ex.Message}", "Compiler Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (code.Error != null && code.Error.Length > 0)
            {
                MetroFramework.MetroMessageBox.Show(this, $"There was an error compiling your GSC Project:\n{code.Error}", "Compiler Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string outputDir = Path.GetDirectoryName(compiledGscFileOutputTextBox.Text);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                File.WriteAllBytes(compiledGscFileOutputTextBox.Text, code.CompiledScript);
                MetroFramework.MetroMessageBox.Show(this, $"Your compiled GSC file has been exported to:\n{compiledGscFileOutputTextBox.Text}\nEnjoy! :)", "Compile Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, $"Failed to write compiled output file:\n{ex.Message}", "File Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void browseCompiledGscFileButton_Click(object sender, EventArgs e)
        {
            using (var fd = new OpenFileDialog())
            {
                fd.Filter = "Compiled GSC Files (*.gscc)|*.gscc";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    compiledGscFileTextBox.Text = fd.FileName;
                }
            }
        }

        private void injectGscButton_Click(object sender, EventArgs e)
        {
            if (ps4 == null || !ps4.IsConnected || attachedProcess == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "Make sure to connect to your PS4 and attach to Black Ops 3 first.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(compiledGscFileTextBox.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Please select a compiled GSC file to inject (.gscc)", "Select GSCC File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] buffer;
            try
            {
                buffer = File.ReadAllBytes(compiledGscFileTextBox.Text);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, $"Could not read compiled GSC file:\n{ex.Message}", "File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!CompiledScriptValidator.IsValid(buffer))
            {
                MetroFramework.MetroMessageBox.Show(this, "Selected file is not a valid compiled Black Ops 3 GSC script.", "Invalid Script", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ulong newGscFileAddress = 0;
            var pointerUpdated = false;
            try
            {
                ulong dupGscAddress = (ulong)selectedGameVersion;
                var filePointerAddress = ps4.ReadMemory<ulong>(attachedProcess.pid, dupGscAddress + 0x10);

                if (filePointerAddress == 0)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Failed to locate target script pointer in game memory. Ensure you are in a map.", "Memory Injection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int checksum = ps4.ReadMemory<int>(attachedProcess.pid, filePointerAddress + 0x8);
                BitConverter.GetBytes(checksum).CopyTo(buffer, 0x8);

                newGscFileAddress = ps4.AllocateMemory(attachedProcess.pid, buffer.Length);
                ps4.WriteMemory(attachedProcess.pid, newGscFileAddress, buffer);
                ps4.WriteMemory(attachedProcess.pid, dupGscAddress + 0x10, newGscFileAddress);
                pointerUpdated = true;

                if (injectedScripts.TryGetValue(selectedGameVersion, out var previousAllocation) &&
                    previousAllocation.Item3 == attachedProcess.pid)
                {
                    try
                    {
                        ps4.FreeMemory(attachedProcess.pid, previousAllocation.Item1, previousAllocation.Item2);
                    }
                    catch (Exception)
                    {
                        // The new script is already active; failure to free the old allocation is non-fatal.
                    }
                }

                injectedScripts[selectedGameVersion] = Tuple.Create(newGscFileAddress, buffer.Length, attachedProcess.pid);

                try
                {
                    ps4.Notify(222, "GSC Script injected!");
                }
                catch { }

                MetroFramework.MetroMessageBox.Show(this, "GSC script successfully injected into Black Ops 3 process!", "Injection Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (!pointerUpdated && newGscFileAddress != 0)
                {
                    try
                    {
                        ps4.FreeMemory(attachedProcess.pid, newGscFileAddress, buffer.Length);
                    }
                    catch (Exception)
                    {
                        // Preserve the original injection error.
                    }
                }

                MetroFramework.MetroMessageBox.Show(this, $"Memory injection failed:\n{ex.Message}", "Injection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gameVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gameVersionComboBox.SelectedIndex == 1)
                selectedGameVersion = Enums.GameVersion.OneTwoSix;
            else
                selectedGameVersion = Enums.GameVersion.OneThreeThree;
        }
    }
}
