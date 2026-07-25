
namespace PS4BO3GSCInjector
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.styleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.ps4IpTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.portLabel = new MetroFramework.Controls.MetroLabel();
            this.ps4PortTextBox = new MetroFramework.Controls.MetroTextBox();
            this.supportedFwLabel = new MetroFramework.Controls.MetroLabel();
            this.gameLabel = new MetroFramework.Controls.MetroLabel();
            this.gameVersionComboBox = new MetroFramework.Controls.MetroComboBox();
            this.connectPS4Button = new System.Windows.Forms.Button();
            this.attachBo3Button = new System.Windows.Forms.Button();
            this.staticStatusLabel = new MetroFramework.Controls.MetroLabel();
            this.connectionStatusLabel = new MetroFramework.Controls.MetroLabel();
            this.compilerGroupBox = new System.Windows.Forms.GroupBox();
            this.browseOutputPathButton = new System.Windows.Forms.Button();
            this.browseGscFolderButton = new System.Windows.Forms.Button();
            this.compileGscProjectButton = new System.Windows.Forms.Button();
            this.compiledGscOutputLabel = new MetroFramework.Controls.MetroLabel();
            this.compiledGscFileOutputTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.gscProjectFolderTextBox = new MetroFramework.Controls.MetroTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.injectGscButton = new System.Windows.Forms.Button();
            this.browseCompiledGscFileButton = new System.Windows.Forms.Button();
            this.compiledGscFileLabel = new MetroFramework.Controls.MetroLabel();
            this.compiledGscFileTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.styleManager)).BeginInit();
            this.connectionGroupBox.SuspendLayout();
            this.compilerGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager
            // 
            this.styleManager.Owner = this;
            this.styleManager.Style = MetroFramework.MetroColorStyle.Purple;
            this.styleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.connectionGroupBox.Controls.Add(this.supportedFwLabel);
            this.connectionGroupBox.Controls.Add(this.connectionStatusLabel);
            this.connectionGroupBox.Controls.Add(this.staticStatusLabel);
            this.connectionGroupBox.Controls.Add(this.attachBo3Button);
            this.connectionGroupBox.Controls.Add(this.connectPS4Button);
            this.connectionGroupBox.Controls.Add(this.gameVersionComboBox);
            this.connectionGroupBox.Controls.Add(this.gameLabel);
            this.connectionGroupBox.Controls.Add(this.ps4PortTextBox);
            this.connectionGroupBox.Controls.Add(this.portLabel);
            this.connectionGroupBox.Controls.Add(this.ps4IpTextBox);
            this.connectionGroupBox.Controls.Add(this.metroLabel2);
            this.connectionGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(65)))), ((int)(((byte)(153)))));
            this.connectionGroupBox.Location = new System.Drawing.Point(24, 60);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(632, 95);
            this.connectionGroupBox.TabIndex = 0;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Target & Connection Settings";
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.AutoSize = true;
            this.connectionStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.connectionStatusLabel.Location = new System.Drawing.Point(345, 60);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(99, 19);
            this.connectionStatusLabel.TabIndex = 11;
            this.connectionStatusLabel.Text = "Not Connected";
            this.connectionStatusLabel.UseCustomForeColor = true;
            // 
            // staticStatusLabel
            // 
            this.staticStatusLabel.AutoSize = true;
            this.staticStatusLabel.Location = new System.Drawing.Point(295, 60);
            this.staticStatusLabel.Name = "staticStatusLabel";
            this.staticStatusLabel.Size = new System.Drawing.Size(46, 19);
            this.staticStatusLabel.TabIndex = 10;
            this.staticStatusLabel.Text = "Status:";
            this.staticStatusLabel.UseStyleColors = true;
            // 
            // attachBo3Button
            // 
            this.attachBo3Button.BackColor = System.Drawing.Color.Black;
            this.attachBo3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.attachBo3Button.Location = new System.Drawing.Point(145, 56);
            this.attachBo3Button.Name = "attachBo3Button";
            this.attachBo3Button.Size = new System.Drawing.Size(120, 26);
            this.attachBo3Button.TabIndex = 9;
            this.attachBo3Button.Text = "Attach BO3";
            this.attachBo3Button.UseVisualStyleBackColor = false;
            this.attachBo3Button.Click += new System.EventHandler(this.attachBo3Button_Click);
            // 
            // connectPS4Button
            // 
            this.connectPS4Button.BackColor = System.Drawing.Color.Black;
            this.connectPS4Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connectPS4Button.Location = new System.Drawing.Point(15, 56);
            this.connectPS4Button.Name = "connectPS4Button";
            this.connectPS4Button.Size = new System.Drawing.Size(120, 26);
            this.connectPS4Button.TabIndex = 8;
            this.connectPS4Button.Text = "Connect PS4";
            this.connectPS4Button.UseVisualStyleBackColor = false;
            this.connectPS4Button.Click += new System.EventHandler(this.connectPS4Button_Click);
            // 
            // gameVersionComboBox
            // 
            this.gameVersionComboBox.FormattingEnabled = true;
            this.gameVersionComboBox.ItemHeight = 23;
            this.gameVersionComboBox.Items.AddRange(new object[] {
            "1.33",
            "1.26"});
            this.gameVersionComboBox.Location = new System.Drawing.Point(350, 18);
            this.gameVersionComboBox.Name = "gameVersionComboBox";
            this.gameVersionComboBox.Size = new System.Drawing.Size(88, 29);
            this.gameVersionComboBox.TabIndex = 7;
            this.gameVersionComboBox.UseSelectable = true;
            this.gameVersionComboBox.UseStyleColors = true;
            this.gameVersionComboBox.SelectedIndexChanged += new System.EventHandler(this.gameVersionComboBox_SelectedIndexChanged);
            // 
            // gameLabel
            // 
            this.gameLabel.AutoSize = true;
            this.gameLabel.Location = new System.Drawing.Point(300, 23);
            this.gameLabel.Name = "gameLabel";
            this.gameLabel.Size = new System.Drawing.Size(47, 19);
            this.gameLabel.TabIndex = 6;
            this.gameLabel.Text = "Game:";
            this.gameLabel.UseStyleColors = true;
            // 
            // supportedFwLabel
            // 
            this.supportedFwLabel.AutoSize = true;
            this.supportedFwLabel.FontSize = MetroFramework.MetroLabelSize.Small;
            this.supportedFwLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(160)))));
            this.supportedFwLabel.Location = new System.Drawing.Point(450, 25);
            this.supportedFwLabel.Name = "supportedFwLabel";
            this.supportedFwLabel.Size = new System.Drawing.Size(168, 15);
            this.supportedFwLabel.TabIndex = 12;
            this.supportedFwLabel.Text = "ps4debug v1.1.19";
            this.supportedFwLabel.UseCustomForeColor = true;
            // 
            // ps4PortTextBox
            // 
            this.ps4PortTextBox.CustomButton.Image = null;
            this.ps4PortTextBox.CustomButton.Location = new System.Drawing.Point(33, 1);
            this.ps4PortTextBox.CustomButton.Name = "";
            this.ps4PortTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.ps4PortTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.ps4PortTextBox.CustomButton.TabIndex = 1;
            this.ps4PortTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ps4PortTextBox.CustomButton.UseSelectable = true;
            this.ps4PortTextBox.CustomButton.Visible = false;
            this.ps4PortTextBox.Lines = new string[0];
            this.ps4PortTextBox.Location = new System.Drawing.Point(225, 21);
            this.ps4PortTextBox.MaxLength = 32767;
            this.ps4PortTextBox.Name = "ps4PortTextBox";
            this.ps4PortTextBox.PasswordChar = '\0';
            this.ps4PortTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ps4PortTextBox.SelectedText = "";
            this.ps4PortTextBox.SelectionLength = 0;
            this.ps4PortTextBox.SelectionStart = 0;
            this.ps4PortTextBox.ShortcutsEnabled = true;
            this.ps4PortTextBox.Size = new System.Drawing.Size(55, 23);
            this.ps4PortTextBox.TabIndex = 3;
            this.ps4PortTextBox.UseSelectable = true;
            this.ps4PortTextBox.UseStyleColors = true;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(185, 23);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(37, 19);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            this.portLabel.UseStyleColors = true;
            // 
            // ps4IpTextBox
            // 
            this.ps4IpTextBox.CustomButton.Image = null;
            this.ps4IpTextBox.CustomButton.Location = new System.Drawing.Point(113, 1);
            this.ps4IpTextBox.CustomButton.Name = "";
            this.ps4IpTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.ps4IpTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.ps4IpTextBox.CustomButton.TabIndex = 1;
            this.ps4IpTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ps4IpTextBox.CustomButton.UseSelectable = true;
            this.ps4IpTextBox.CustomButton.Visible = false;
            this.ps4IpTextBox.Lines = new string[0];
            this.ps4IpTextBox.Location = new System.Drawing.Point(42, 21);
            this.ps4IpTextBox.MaxLength = 32767;
            this.ps4IpTextBox.Name = "ps4IpTextBox";
            this.ps4IpTextBox.PasswordChar = '\0';
            this.ps4IpTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ps4IpTextBox.SelectedText = "";
            this.ps4IpTextBox.SelectionLength = 0;
            this.ps4IpTextBox.SelectionStart = 0;
            this.ps4IpTextBox.ShortcutsEnabled = true;
            this.ps4IpTextBox.Size = new System.Drawing.Size(135, 23);
            this.ps4IpTextBox.TabIndex = 1;
            this.ps4IpTextBox.UseSelectable = true;
            this.ps4IpTextBox.UseStyleColors = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(15, 23);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(23, 19);
            this.metroLabel2.TabIndex = 0;
            this.metroLabel2.Text = "IP:";
            this.metroLabel2.UseStyleColors = true;
            // 
            // compilerGroupBox
            // 
            this.compilerGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.compilerGroupBox.Controls.Add(this.compileGscProjectButton);
            this.compilerGroupBox.Controls.Add(this.browseOutputPathButton);
            this.compilerGroupBox.Controls.Add(this.compiledGscFileOutputTextBox);
            this.compilerGroupBox.Controls.Add(this.compiledGscOutputLabel);
            this.compilerGroupBox.Controls.Add(this.browseGscFolderButton);
            this.compilerGroupBox.Controls.Add(this.gscProjectFolderTextBox);
            this.compilerGroupBox.Controls.Add(this.metroLabel1);
            this.compilerGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(65)))), ((int)(((byte)(153)))));
            this.compilerGroupBox.Location = new System.Drawing.Point(24, 165);
            this.compilerGroupBox.Name = "compilerGroupBox";
            this.compilerGroupBox.Size = new System.Drawing.Size(632, 125);
            this.compilerGroupBox.TabIndex = 1;
            this.compilerGroupBox.TabStop = false;
            this.compilerGroupBox.Text = "GSC Compiler";
            // 
            // compileGscProjectButton
            // 
            this.compileGscProjectButton.BackColor = System.Drawing.Color.Black;
            this.compileGscProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.compileGscProjectButton.Location = new System.Drawing.Point(216, 86);
            this.compileGscProjectButton.Name = "compileGscProjectButton";
            this.compileGscProjectButton.Size = new System.Drawing.Size(200, 28);
            this.compileGscProjectButton.TabIndex = 6;
            this.compileGscProjectButton.Text = "⚡ Compile GSC Project";
            this.compileGscProjectButton.UseVisualStyleBackColor = false;
            this.compileGscProjectButton.Click += new System.EventHandler(this.compileGscProjectButton_Click);
            // 
            // browseOutputPathButton
            // 
            this.browseOutputPathButton.BackColor = System.Drawing.Color.Black;
            this.browseOutputPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseOutputPathButton.Location = new System.Drawing.Point(550, 52);
            this.browseOutputPathButton.Name = "browseOutputPathButton";
            this.browseOutputPathButton.Size = new System.Drawing.Size(68, 23);
            this.browseOutputPathButton.TabIndex = 5;
            this.browseOutputPathButton.Text = "Browse";
            this.browseOutputPathButton.UseVisualStyleBackColor = false;
            this.browseOutputPathButton.Click += new System.EventHandler(this.browseOutputPathButton_Click);
            // 
            // compiledGscFileOutputTextBox
            // 
            this.compiledGscFileOutputTextBox.CustomButton.Image = null;
            this.compiledGscFileOutputTextBox.CustomButton.Location = new System.Drawing.Point(403, 1);
            this.compiledGscFileOutputTextBox.CustomButton.Name = "";
            this.compiledGscFileOutputTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.compiledGscFileOutputTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.compiledGscFileOutputTextBox.CustomButton.TabIndex = 1;
            this.compiledGscFileOutputTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.compiledGscFileOutputTextBox.CustomButton.UseSelectable = true;
            this.compiledGscFileOutputTextBox.CustomButton.Visible = false;
            this.compiledGscFileOutputTextBox.Lines = new string[0];
            this.compiledGscFileOutputTextBox.Location = new System.Drawing.Point(115, 52);
            this.compiledGscFileOutputTextBox.MaxLength = 32767;
            this.compiledGscFileOutputTextBox.Name = "compiledGscFileOutputTextBox";
            this.compiledGscFileOutputTextBox.PasswordChar = '\0';
            this.compiledGscFileOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.compiledGscFileOutputTextBox.SelectedText = "";
            this.compiledGscFileOutputTextBox.SelectionLength = 0;
            this.compiledGscFileOutputTextBox.SelectionStart = 0;
            this.compiledGscFileOutputTextBox.ShortcutsEnabled = true;
            this.compiledGscFileOutputTextBox.Size = new System.Drawing.Size(425, 23);
            this.compiledGscFileOutputTextBox.TabIndex = 4;
            this.compiledGscFileOutputTextBox.UseSelectable = true;
            this.compiledGscFileOutputTextBox.UseStyleColors = true;
            // 
            // compiledGscOutputLabel
            // 
            this.compiledGscOutputLabel.AutoSize = true;
            this.compiledGscOutputLabel.Location = new System.Drawing.Point(15, 54);
            this.compiledGscOutputLabel.Name = "compiledGscOutputLabel";
            this.compiledGscOutputLabel.Size = new System.Drawing.Size(83, 19);
            this.compiledGscOutputLabel.TabIndex = 3;
            this.compiledGscOutputLabel.Text = "Output Path:";
            this.compiledGscOutputLabel.UseStyleColors = true;
            // 
            // browseGscFolderButton
            // 
            this.browseGscFolderButton.BackColor = System.Drawing.Color.Black;
            this.browseGscFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseGscFolderButton.Location = new System.Drawing.Point(550, 22);
            this.browseGscFolderButton.Name = "browseGscFolderButton";
            this.browseGscFolderButton.Size = new System.Drawing.Size(68, 23);
            this.browseGscFolderButton.TabIndex = 2;
            this.browseGscFolderButton.Text = "Browse";
            this.browseGscFolderButton.UseVisualStyleBackColor = false;
            this.browseGscFolderButton.Click += new System.EventHandler(this.browseGscFolderButton_Click);
            // 
            // gscProjectFolderTextBox
            // 
            this.gscProjectFolderTextBox.CustomButton.Image = null;
            this.gscProjectFolderTextBox.CustomButton.Location = new System.Drawing.Point(403, 1);
            this.gscProjectFolderTextBox.CustomButton.Name = "";
            this.gscProjectFolderTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.gscProjectFolderTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.gscProjectFolderTextBox.CustomButton.TabIndex = 1;
            this.gscProjectFolderTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.gscProjectFolderTextBox.CustomButton.UseSelectable = true;
            this.gscProjectFolderTextBox.CustomButton.Visible = false;
            this.gscProjectFolderTextBox.Lines = new string[0];
            this.gscProjectFolderTextBox.Location = new System.Drawing.Point(115, 22);
            this.gscProjectFolderTextBox.MaxLength = 32767;
            this.gscProjectFolderTextBox.Name = "gscProjectFolderTextBox";
            this.gscProjectFolderTextBox.PasswordChar = '\0';
            this.gscProjectFolderTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gscProjectFolderTextBox.SelectedText = "";
            this.gscProjectFolderTextBox.SelectionLength = 0;
            this.gscProjectFolderTextBox.SelectionStart = 0;
            this.gscProjectFolderTextBox.ShortcutsEnabled = true;
            this.gscProjectFolderTextBox.Size = new System.Drawing.Size(425, 23);
            this.gscProjectFolderTextBox.TabIndex = 1;
            this.gscProjectFolderTextBox.UseSelectable = true;
            this.gscProjectFolderTextBox.UseStyleColors = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(15, 24);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(81, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "GSC Project:";
            this.metroLabel1.UseStyleColors = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.injectGscButton);
            this.groupBox1.Controls.Add(this.browseCompiledGscFileButton);
            this.groupBox1.Controls.Add(this.compiledGscFileTextBox);
            this.groupBox1.Controls.Add(this.compiledGscFileLabel);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(65)))), ((int)(((byte)(153)))));
            this.groupBox1.Location = new System.Drawing.Point(24, 300);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 95);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GSC Injector";
            // 
            // injectGscButton
            // 
            this.injectGscButton.BackColor = System.Drawing.Color.Black;
            this.injectGscButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.injectGscButton.Location = new System.Drawing.Point(216, 56);
            this.injectGscButton.Name = "injectGscButton";
            this.injectGscButton.Size = new System.Drawing.Size(200, 28);
            this.injectGscButton.TabIndex = 3;
            this.injectGscButton.Text = "💉 Inject GSC Payload";
            this.injectGscButton.UseVisualStyleBackColor = false;
            this.injectGscButton.Click += new System.EventHandler(this.injectGscButton_Click);
            // 
            // browseCompiledGscFileButton
            // 
            this.browseCompiledGscFileButton.BackColor = System.Drawing.Color.Black;
            this.browseCompiledGscFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseCompiledGscFileButton.Location = new System.Drawing.Point(550, 22);
            this.browseCompiledGscFileButton.Name = "browseCompiledGscFileButton";
            this.browseCompiledGscFileButton.Size = new System.Drawing.Size(68, 23);
            this.browseCompiledGscFileButton.TabIndex = 2;
            this.browseCompiledGscFileButton.Text = "Browse";
            this.browseCompiledGscFileButton.UseVisualStyleBackColor = false;
            this.browseCompiledGscFileButton.Click += new System.EventHandler(this.browseCompiledGscFileButton_Click);
            // 
            // compiledGscFileTextBox
            // 
            this.compiledGscFileTextBox.CustomButton.Image = null;
            this.compiledGscFileTextBox.CustomButton.Location = new System.Drawing.Point(403, 1);
            this.compiledGscFileTextBox.CustomButton.Name = "";
            this.compiledGscFileTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.compiledGscFileTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.compiledGscFileTextBox.CustomButton.TabIndex = 1;
            this.compiledGscFileTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.compiledGscFileTextBox.CustomButton.UseSelectable = true;
            this.compiledGscFileTextBox.CustomButton.Visible = false;
            this.compiledGscFileTextBox.Lines = new string[0];
            this.compiledGscFileTextBox.Location = new System.Drawing.Point(115, 22);
            this.compiledGscFileTextBox.MaxLength = 32767;
            this.compiledGscFileTextBox.Name = "compiledGscFileTextBox";
            this.compiledGscFileTextBox.PasswordChar = '\0';
            this.compiledGscFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.compiledGscFileTextBox.SelectedText = "";
            this.compiledGscFileTextBox.SelectionLength = 0;
            this.compiledGscFileTextBox.SelectionStart = 0;
            this.compiledGscFileTextBox.ShortcutsEnabled = true;
            this.compiledGscFileTextBox.Size = new System.Drawing.Size(425, 23);
            this.compiledGscFileTextBox.TabIndex = 1;
            this.compiledGscFileTextBox.UseSelectable = true;
            this.compiledGscFileTextBox.UseStyleColors = true;
            // 
            // compiledGscFileLabel
            // 
            this.compiledGscFileLabel.AutoSize = true;
            this.compiledGscFileLabel.Location = new System.Drawing.Point(15, 24);
            this.compiledGscFileLabel.Name = "compiledGscFileLabel";
            this.compiledGscFileLabel.Size = new System.Drawing.Size(95, 19);
            this.compiledGscFileLabel.TabIndex = 0;
            this.compiledGscFileLabel.Text = "Compiled GSC:";
            this.compiledGscFileLabel.UseStyleColors = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel3.Location = new System.Drawing.Point(550, 408);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(106, 19);
            this.metroLabel3.TabIndex = 3;
            this.metroLabel3.Text = "Created by CGhxst";
            this.metroLabel3.UseStyleColors = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 440);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.compilerGroupBox);
            this.Controls.Add(this.connectionGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Purple;
            this.Text = "PS4 BO3 GSC Injector";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.styleManager)).EndInit();
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.compilerGroupBox.ResumeLayout(false);
            this.compilerGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MetroFramework.Components.MetroStyleManager styleManager;
        private System.Windows.Forms.GroupBox connectionGroupBox;
        private MetroFramework.Controls.MetroTextBox ps4IpTextBox;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel portLabel;
        private MetroFramework.Controls.MetroTextBox ps4PortTextBox;
        private MetroFramework.Controls.MetroLabel gameLabel;
        private MetroFramework.Controls.MetroComboBox gameVersionComboBox;
        private System.Windows.Forms.Button connectPS4Button;
        private System.Windows.Forms.Button attachBo3Button;
        private MetroFramework.Controls.MetroLabel staticStatusLabel;
        private MetroFramework.Controls.MetroLabel connectionStatusLabel;
        private System.Windows.Forms.GroupBox compilerGroupBox;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox gscProjectFolderTextBox;
        private System.Windows.Forms.Button browseGscFolderButton;
        private MetroFramework.Controls.MetroLabel compiledGscOutputLabel;
        private MetroFramework.Controls.MetroTextBox compiledGscFileOutputTextBox;
        private System.Windows.Forms.Button browseOutputPathButton;
        private System.Windows.Forms.Button compileGscProjectButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetroFramework.Controls.MetroLabel compiledGscFileLabel;
        private MetroFramework.Controls.MetroTextBox compiledGscFileTextBox;
        private System.Windows.Forms.Button browseCompiledGscFileButton;
        private System.Windows.Forms.Button injectGscButton;
        private MetroFramework.Controls.MetroLabel supportedFwLabel;
        private MetroFramework.Controls.MetroLabel metroLabel3;
    }
}
