using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MitsubishiMC
{
    partial class MainForm
    {
        private IContainer components = null;

        private TabControl _tabControl;
        private TabPage _tabConnect;
        private TabPage _tabWord;
        private TabPage _tabBit;
        private TabPage _tabRandom;
        private TabPage _tabLog;
        private TextBox _txtIp;
        private TextBox _txtPort;
        private Button _btnConnect;
        private Button _btnDisconnect;
        private Label _lblStatus;
        private Label _lblIp;
        private Label _lblPort;
        private Label _lblStatusTitle;
        private Label _lblTip0;
        private ComboBox _cbWordDevice;
        private NumericUpDown _numWordAddr;
        private NumericUpDown _numWordCount;
        private TextBox _txtWordValue;
        private Button _btnWordRead;
        private Button _btnWordWrite;
        private Button _btnWordBatchRead;
        private ListBox _lbWordResult;
        private Label _lblWordType;
        private Label _lblWordAddr;
        private Label _lblWordCount;
        private Label _lblWordVal;
        private ComboBox _cbBitDevice;
        private NumericUpDown _numBitAddr;
        private NumericUpDown _numBitCount;
        private Button _btnBitRead;
        private Button _btnBitWrite;
        private Button _btnBitBatchRead;
        private ListBox _lbBitResult;
        private Label _lblBitType;
        private Label _lblBitAddr;
        private TextBox _txtRandAddrs;
        private Button _btnRandRead;
        private Button _btnRandWrite;
        private Label _lblRandTip;
        private RichTextBox _rtbLog;
        private StatusBar _statusBar;
        private StatusBarPanel _statusPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabConnect = new System.Windows.Forms.TabPage();
            this._lblIp = new System.Windows.Forms.Label();
            this._txtIp = new System.Windows.Forms.TextBox();
            this._lblPort = new System.Windows.Forms.Label();
            this._txtPort = new System.Windows.Forms.TextBox();
            this._btnConnect = new System.Windows.Forms.Button();
            this._btnDisconnect = new System.Windows.Forms.Button();
            this._lblStatusTitle = new System.Windows.Forms.Label();
            this._lblStatus = new System.Windows.Forms.Label();
            this._lblTip0 = new System.Windows.Forms.Label();
            this._tabWord = new System.Windows.Forms.TabPage();
            this._lblWordType = new System.Windows.Forms.Label();
            this._cbWordDevice = new System.Windows.Forms.ComboBox();
            this._lblWordAddr = new System.Windows.Forms.Label();
            this._numWordAddr = new System.Windows.Forms.NumericUpDown();
            this._lblWordCount = new System.Windows.Forms.Label();
            this._numWordCount = new System.Windows.Forms.NumericUpDown();
            this._lblWordVal = new System.Windows.Forms.Label();
            this._txtWordValue = new System.Windows.Forms.TextBox();
            this._btnWordRead = new System.Windows.Forms.Button();
            this._btnWordWrite = new System.Windows.Forms.Button();
            this._btnWordBatchRead = new System.Windows.Forms.Button();
            this._lbWordResult = new System.Windows.Forms.ListBox();
            this._tabBit = new System.Windows.Forms.TabPage();
            this._lblBitType = new System.Windows.Forms.Label();
            this._cbBitDevice = new System.Windows.Forms.ComboBox();
            this._lblBitAddr = new System.Windows.Forms.Label();
            this._numBitAddr = new System.Windows.Forms.NumericUpDown();
            this._numBitCount = new System.Windows.Forms.NumericUpDown();
            this._btnBitRead = new System.Windows.Forms.Button();
            this._btnBitWrite = new System.Windows.Forms.Button();
            this._btnBitBatchRead = new System.Windows.Forms.Button();
            this._lbBitResult = new System.Windows.Forms.ListBox();
            this._tabRandom = new System.Windows.Forms.TabPage();
            this._lblRandTip = new System.Windows.Forms.Label();
            this._txtRandAddrs = new System.Windows.Forms.TextBox();
            this._btnRandRead = new System.Windows.Forms.Button();
            this._btnRandWrite = new System.Windows.Forms.Button();
            this._tabLog = new System.Windows.Forms.TabPage();
            this._rtbLog = new System.Windows.Forms.RichTextBox();
            this._statusBar = new System.Windows.Forms.StatusBar();
            this._statusPanel = new System.Windows.Forms.StatusBarPanel();
            this._tabControl.SuspendLayout();
            this._tabConnect.SuspendLayout();
            this._tabWord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numWordAddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numWordCount)).BeginInit();
            this._tabBit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numBitAddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numBitCount)).BeginInit();
            this._tabRandom.SuspendLayout();
            this._tabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._statusPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabConnect);
            this._tabControl.Controls.Add(this._tabWord);
            this._tabControl.Controls.Add(this._tabBit);
            this._tabControl.Controls.Add(this._tabRandom);
            this._tabControl.Controls.Add(this._tabLog);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(894, 540);
            this._tabControl.TabIndex = 0;
            // 
            // _tabConnect
            // 
            this._tabConnect.Controls.Add(this._lblIp);
            this._tabConnect.Controls.Add(this._txtIp);
            this._tabConnect.Controls.Add(this._lblPort);
            this._tabConnect.Controls.Add(this._txtPort);
            this._tabConnect.Controls.Add(this._btnConnect);
            this._tabConnect.Controls.Add(this._btnDisconnect);
            this._tabConnect.Controls.Add(this._lblStatusTitle);
            this._tabConnect.Controls.Add(this._lblStatus);
            this._tabConnect.Controls.Add(this._lblTip0);
            this._tabConnect.Location = new System.Drawing.Point(4, 33);
            this._tabConnect.Name = "_tabConnect";
            this._tabConnect.Padding = new System.Windows.Forms.Padding(3);
            this._tabConnect.Size = new System.Drawing.Size(886, 503);
            this._tabConnect.TabIndex = 0;
            this._tabConnect.Text = "连接";
            this._tabConnect.UseVisualStyleBackColor = true;
            // 
            // _lblIp
            // 
            this._lblIp.AutoSize = true;
            this._lblIp.Location = new System.Drawing.Point(20, 20);
            this._lblIp.Name = "_lblIp";
            this._lblIp.Size = new System.Drawing.Size(30, 24);
            this._lblIp.TabIndex = 100;
            this._lblIp.Text = "IP:";
            // 
            // _txtIp
            // 
            this._txtIp.Location = new System.Drawing.Point(60, 17);
            this._txtIp.Name = "_txtIp";
            this._txtIp.Size = new System.Drawing.Size(160, 30);
            this._txtIp.TabIndex = 1;
            this._txtIp.Text = "192.168.1.73";
            // 
            // _lblPort
            // 
            this._lblPort.AutoSize = true;
            this._lblPort.Location = new System.Drawing.Point(240, 20);
            this._lblPort.Name = "_lblPort";
            this._lblPort.Size = new System.Drawing.Size(50, 24);
            this._lblPort.TabIndex = 101;
            this._lblPort.Text = "端口:";
            // 
            // _txtPort
            // 
            this._txtPort.Location = new System.Drawing.Point(290, 17);
            this._txtPort.Name = "_txtPort";
            this._txtPort.Size = new System.Drawing.Size(80, 30);
            this._txtPort.TabIndex = 2;
            this._txtPort.Text = "6000";
            // 
            // _btnConnect
            // 
            this._btnConnect.Location = new System.Drawing.Point(20, 55);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(100, 30);
            this._btnConnect.TabIndex = 3;
            this._btnConnect.Text = "连接";
            this._btnConnect.UseVisualStyleBackColor = true;
            // 
            // _btnDisconnect
            // 
            this._btnDisconnect.Location = new System.Drawing.Point(130, 55);
            this._btnDisconnect.Name = "_btnDisconnect";
            this._btnDisconnect.Size = new System.Drawing.Size(100, 30);
            this._btnDisconnect.TabIndex = 4;
            this._btnDisconnect.Text = "断开";
            this._btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // _lblStatusTitle
            // 
            this._lblStatusTitle.AutoSize = true;
            this._lblStatusTitle.Location = new System.Drawing.Point(20, 100);
            this._lblStatusTitle.Name = "_lblStatusTitle";
            this._lblStatusTitle.Size = new System.Drawing.Size(50, 24);
            this._lblStatusTitle.TabIndex = 102;
            this._lblStatusTitle.Text = "状态:";
            // 
            // _lblStatus
            // 
            this._lblStatus.AutoSize = true;
            this._lblStatus.ForeColor = System.Drawing.Color.Gray;
            this._lblStatus.Location = new System.Drawing.Point(60, 100);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(64, 24);
            this._lblStatus.TabIndex = 5;
            this._lblStatus.Text = "未连接";
            // 
            // _lblTip0
            // 
            this._lblTip0.AutoSize = true;
            this._lblTip0.ForeColor = System.Drawing.Color.Gray;
            this._lblTip0.Location = new System.Drawing.Point(20, 140);
            this._lblTip0.Name = "_lblTip0";
            this._lblTip0.Size = new System.Drawing.Size(374, 24);
            this._lblTip0.TabIndex = 103;
            this._lblTip0.Text = "先连接 PLC 再操作。默认端口 5000~5009。";
            // 
            // _tabWord
            // 
            this._tabWord.Controls.Add(this._lblWordType);
            this._tabWord.Controls.Add(this._cbWordDevice);
            this._tabWord.Controls.Add(this._lblWordAddr);
            this._tabWord.Controls.Add(this._numWordAddr);
            this._tabWord.Controls.Add(this._lblWordCount);
            this._tabWord.Controls.Add(this._numWordCount);
            this._tabWord.Controls.Add(this._lblWordVal);
            this._tabWord.Controls.Add(this._txtWordValue);
            this._tabWord.Controls.Add(this._btnWordRead);
            this._tabWord.Controls.Add(this._btnWordWrite);
            this._tabWord.Controls.Add(this._btnWordBatchRead);
            this._tabWord.Controls.Add(this._lbWordResult);
            this._tabWord.Location = new System.Drawing.Point(4, 33);
            this._tabWord.Name = "_tabWord";
            this._tabWord.Padding = new System.Windows.Forms.Padding(3);
            this._tabWord.Size = new System.Drawing.Size(796, 524);
            this._tabWord.TabIndex = 1;
            this._tabWord.Text = "字元件";
            this._tabWord.UseVisualStyleBackColor = true;
            // 
            // _lblWordType
            // 
            this._lblWordType.AutoSize = true;
            this._lblWordType.Location = new System.Drawing.Point(20, 20);
            this._lblWordType.Name = "_lblWordType";
            this._lblWordType.Size = new System.Drawing.Size(50, 24);
            this._lblWordType.TabIndex = 200;
            this._lblWordType.Text = "类型:";
            // 
            // _cbWordDevice
            // 
            this._cbWordDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbWordDevice.Items.AddRange(new object[] {
            "D",
            "W",
            "R",
            "SD",
            "ZR",
            "M",
            "L",
            "TN",
            "CN"});
            this._cbWordDevice.Location = new System.Drawing.Point(65, 17);
            this._cbWordDevice.Name = "_cbWordDevice";
            this._cbWordDevice.Size = new System.Drawing.Size(60, 32);
            this._cbWordDevice.TabIndex = 10;
            // 
            // _lblWordAddr
            // 
            this._lblWordAddr.AutoSize = true;
            this._lblWordAddr.Location = new System.Drawing.Point(140, 20);
            this._lblWordAddr.Name = "_lblWordAddr";
            this._lblWordAddr.Size = new System.Drawing.Size(50, 24);
            this._lblWordAddr.TabIndex = 201;
            this._lblWordAddr.Text = "地址:";
            // 
            // _numWordAddr
            // 
            this._numWordAddr.Location = new System.Drawing.Point(185, 17);
            this._numWordAddr.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numWordAddr.Name = "_numWordAddr";
            this._numWordAddr.Size = new System.Drawing.Size(80, 30);
            this._numWordAddr.TabIndex = 11;
            this._numWordAddr.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _lblWordCount
            // 
            this._lblWordCount.AutoSize = true;
            this._lblWordCount.Location = new System.Drawing.Point(280, 20);
            this._lblWordCount.Name = "_lblWordCount";
            this._lblWordCount.Size = new System.Drawing.Size(50, 24);
            this._lblWordCount.TabIndex = 202;
            this._lblWordCount.Text = "数量:";
            // 
            // _numWordCount
            // 
            this._numWordCount.Location = new System.Drawing.Point(325, 17);
            this._numWordCount.Maximum = new decimal(new int[] {
            960,
            0,
            0,
            0});
            this._numWordCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numWordCount.Name = "_numWordCount";
            this._numWordCount.Size = new System.Drawing.Size(60, 30);
            this._numWordCount.TabIndex = 12;
            this._numWordCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _lblWordVal
            // 
            this._lblWordVal.AutoSize = true;
            this._lblWordVal.Location = new System.Drawing.Point(400, 20);
            this._lblWordVal.Name = "_lblWordVal";
            this._lblWordVal.Size = new System.Drawing.Size(32, 24);
            this._lblWordVal.TabIndex = 203;
            this._lblWordVal.Text = "值:";
            // 
            // _txtWordValue
            // 
            this._txtWordValue.Location = new System.Drawing.Point(425, 17);
            this._txtWordValue.Name = "_txtWordValue";
            this._txtWordValue.Size = new System.Drawing.Size(100, 30);
            this._txtWordValue.TabIndex = 13;
            // 
            // _btnWordRead
            // 
            this._btnWordRead.Location = new System.Drawing.Point(20, 55);
            this._btnWordRead.Name = "_btnWordRead";
            this._btnWordRead.Size = new System.Drawing.Size(90, 28);
            this._btnWordRead.TabIndex = 14;
            this._btnWordRead.Text = "读取";
            this._btnWordRead.UseVisualStyleBackColor = true;
            // 
            // _btnWordWrite
            // 
            this._btnWordWrite.Location = new System.Drawing.Point(120, 55);
            this._btnWordWrite.Name = "_btnWordWrite";
            this._btnWordWrite.Size = new System.Drawing.Size(90, 28);
            this._btnWordWrite.TabIndex = 15;
            this._btnWordWrite.Text = "写入";
            this._btnWordWrite.UseVisualStyleBackColor = true;
            // 
            // _btnWordBatchRead
            // 
            this._btnWordBatchRead.Location = new System.Drawing.Point(220, 55);
            this._btnWordBatchRead.Name = "_btnWordBatchRead";
            this._btnWordBatchRead.Size = new System.Drawing.Size(150, 28);
            this._btnWordBatchRead.TabIndex = 16;
            this._btnWordBatchRead.Text = "批量读取 (按数量)";
            this._btnWordBatchRead.UseVisualStyleBackColor = true;
            // 
            // _lbWordResult
            // 
            this._lbWordResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbWordResult.FormattingEnabled = true;
            this._lbWordResult.ItemHeight = 22;
            this._lbWordResult.Location = new System.Drawing.Point(20, 95);
            this._lbWordResult.Name = "_lbWordResult";
            this._lbWordResult.Size = new System.Drawing.Size(760, 334);
            this._lbWordResult.TabIndex = 17;
            // 
            // _tabBit
            // 
            this._tabBit.Controls.Add(this._lblBitType);
            this._tabBit.Controls.Add(this._cbBitDevice);
            this._tabBit.Controls.Add(this._lblBitAddr);
            this._tabBit.Controls.Add(this._numBitAddr);
            this._tabBit.Controls.Add(this._numBitCount);
            this._tabBit.Controls.Add(this._btnBitRead);
            this._tabBit.Controls.Add(this._btnBitWrite);
            this._tabBit.Controls.Add(this._btnBitBatchRead);
            this._tabBit.Controls.Add(this._lbBitResult);
            this._tabBit.Location = new System.Drawing.Point(4, 33);
            this._tabBit.Name = "_tabBit";
            this._tabBit.Padding = new System.Windows.Forms.Padding(3);
            this._tabBit.Size = new System.Drawing.Size(796, 524);
            this._tabBit.TabIndex = 2;
            this._tabBit.Text = "位元件";
            this._tabBit.UseVisualStyleBackColor = true;
            // 
            // _lblBitType
            // 
            this._lblBitType.AutoSize = true;
            this._lblBitType.Location = new System.Drawing.Point(20, 20);
            this._lblBitType.Name = "_lblBitType";
            this._lblBitType.Size = new System.Drawing.Size(50, 24);
            this._lblBitType.TabIndex = 300;
            this._lblBitType.Text = "类型:";
            // 
            // _cbBitDevice
            // 
            this._cbBitDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbBitDevice.Items.AddRange(new object[] {
            "M",
            "X",
            "Y",
            "L"});
            this._cbBitDevice.Location = new System.Drawing.Point(65, 17);
            this._cbBitDevice.Name = "_cbBitDevice";
            this._cbBitDevice.Size = new System.Drawing.Size(60, 32);
            this._cbBitDevice.TabIndex = 20;
            // 
            // _lblBitAddr
            // 
            this._lblBitAddr.AutoSize = true;
            this._lblBitAddr.Location = new System.Drawing.Point(140, 20);
            this._lblBitAddr.Name = "_lblBitAddr";
            this._lblBitAddr.Size = new System.Drawing.Size(50, 24);
            this._lblBitAddr.TabIndex = 301;
            this._lblBitAddr.Text = "地址:";
            // 
            // _numBitAddr
            // 
            this._numBitAddr.Location = new System.Drawing.Point(185, 17);
            this._numBitAddr.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numBitAddr.Name = "_numBitAddr";
            this._numBitAddr.Size = new System.Drawing.Size(80, 30);
            this._numBitAddr.TabIndex = 21;
            // 
            // _numBitCount
            // 
            this._numBitCount.Location = new System.Drawing.Point(325, 17);
            this._numBitCount.Maximum = new decimal(new int[] {
            3584,
            0,
            0,
            0});
            this._numBitCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numBitCount.Name = "_numBitCount";
            this._numBitCount.Size = new System.Drawing.Size(60, 30);
            this._numBitCount.TabIndex = 22;
            this._numBitCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _btnBitRead
            // 
            this._btnBitRead.Location = new System.Drawing.Point(20, 55);
            this._btnBitRead.Name = "_btnBitRead";
            this._btnBitRead.Size = new System.Drawing.Size(90, 28);
            this._btnBitRead.TabIndex = 23;
            this._btnBitRead.Text = "读取位";
            this._btnBitRead.UseVisualStyleBackColor = true;
            // 
            // _btnBitWrite
            // 
            this._btnBitWrite.Location = new System.Drawing.Point(120, 55);
            this._btnBitWrite.Name = "_btnBitWrite";
            this._btnBitWrite.Size = new System.Drawing.Size(150, 28);
            this._btnBitWrite.TabIndex = 24;
            this._btnBitWrite.Text = "写入 ON/OFF（翻转）";
            this._btnBitWrite.UseVisualStyleBackColor = true;
            // 
            // _btnBitBatchRead
            // 
            this._btnBitBatchRead.Location = new System.Drawing.Point(280, 55);
            this._btnBitBatchRead.Name = "_btnBitBatchRead";
            this._btnBitBatchRead.Size = new System.Drawing.Size(130, 28);
            this._btnBitBatchRead.TabIndex = 25;
            this._btnBitBatchRead.Text = "批量读取 (16点)";
            this._btnBitBatchRead.UseVisualStyleBackColor = true;
            // 
            // _lbBitResult
            // 
            this._lbBitResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbBitResult.FormattingEnabled = true;
            this._lbBitResult.ItemHeight = 22;
            this._lbBitResult.Location = new System.Drawing.Point(20, 95);
            this._lbBitResult.Name = "_lbBitResult";
            this._lbBitResult.Size = new System.Drawing.Size(760, 334);
            this._lbBitResult.TabIndex = 26;
            // 
            // _tabRandom
            // 
            this._tabRandom.Controls.Add(this._lblRandTip);
            this._tabRandom.Controls.Add(this._txtRandAddrs);
            this._tabRandom.Controls.Add(this._btnRandRead);
            this._tabRandom.Controls.Add(this._btnRandWrite);
            this._tabRandom.Location = new System.Drawing.Point(4, 33);
            this._tabRandom.Name = "_tabRandom";
            this._tabRandom.Padding = new System.Windows.Forms.Padding(3);
            this._tabRandom.Size = new System.Drawing.Size(796, 524);
            this._tabRandom.TabIndex = 3;
            this._tabRandom.Text = "随机读写";
            this._tabRandom.UseVisualStyleBackColor = true;
            // 
            // _lblRandTip
            // 
            this._lblRandTip.AutoSize = true;
            this._lblRandTip.ForeColor = System.Drawing.Color.Gray;
            this._lblRandTip.Location = new System.Drawing.Point(20, 15);
            this._lblRandTip.Name = "_lblRandTip";
            this._lblRandTip.Size = new System.Drawing.Size(387, 24);
            this._lblRandTip.TabIndex = 400;
            this._lblRandTip.Text = "每行一个地址如 D100。写入格式 D100=1234";
            // 
            // _txtRandAddrs
            // 
            this._txtRandAddrs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtRandAddrs.Location = new System.Drawing.Point(20, 55);
            this._txtRandAddrs.Multiline = true;
            this._txtRandAddrs.Name = "_txtRandAddrs";
            this._txtRandAddrs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtRandAddrs.Size = new System.Drawing.Size(350, 250);
            this._txtRandAddrs.TabIndex = 31;
            this._txtRandAddrs.Text = "D100\r\nD200\r\nD300\r\nW10";
            // 
            // _btnRandRead
            // 
            this._btnRandRead.Location = new System.Drawing.Point(400, 55);
            this._btnRandRead.Name = "_btnRandRead";
            this._btnRandRead.Size = new System.Drawing.Size(120, 30);
            this._btnRandRead.TabIndex = 32;
            this._btnRandRead.Text = "随机读取 →";
            this._btnRandRead.UseVisualStyleBackColor = true;
            // 
            // _btnRandWrite
            // 
            this._btnRandWrite.Location = new System.Drawing.Point(400, 95);
            this._btnRandWrite.Name = "_btnRandWrite";
            this._btnRandWrite.Size = new System.Drawing.Size(120, 30);
            this._btnRandWrite.TabIndex = 33;
            this._btnRandWrite.Text = "随机写入 →";
            this._btnRandWrite.UseVisualStyleBackColor = true;
            // 
            // _tabLog
            // 
            this._tabLog.Controls.Add(this._rtbLog);
            this._tabLog.Location = new System.Drawing.Point(4, 33);
            this._tabLog.Name = "_tabLog";
            this._tabLog.Padding = new System.Windows.Forms.Padding(3);
            this._tabLog.Size = new System.Drawing.Size(796, 524);
            this._tabLog.TabIndex = 4;
            this._tabLog.Text = "日志";
            this._tabLog.UseVisualStyleBackColor = true;
            // 
            // _rtbLog
            // 
            this._rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtbLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._rtbLog.ForeColor = System.Drawing.Color.LightGreen;
            this._rtbLog.Location = new System.Drawing.Point(3, 3);
            this._rtbLog.Name = "_rtbLog";
            this._rtbLog.ReadOnly = true;
            this._rtbLog.Size = new System.Drawing.Size(790, 518);
            this._rtbLog.TabIndex = 40;
            this._rtbLog.Text = "";
            this._rtbLog.WordWrap = false;
            // 
            // _statusBar
            // 
            this._statusBar.Location = new System.Drawing.Point(0, 540);
            this._statusBar.Name = "_statusBar";
            this._statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this._statusPanel});
            this._statusBar.ShowPanels = true;
            this._statusBar.Size = new System.Drawing.Size(894, 22);
            this._statusBar.TabIndex = 50;
            // 
            // _statusPanel
            // 
            this._statusPanel.Name = "_statusPanel";
            this._statusPanel.Text = "就绪";
            this._statusPanel.Width = 600;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 562);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._statusBar);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MC 协议通信工具 — 三菱 PLC (3E 帧)";
            this._tabControl.ResumeLayout(false);
            this._tabConnect.ResumeLayout(false);
            this._tabConnect.PerformLayout();
            this._tabWord.ResumeLayout(false);
            this._tabWord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numWordAddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numWordCount)).EndInit();
            this._tabBit.ResumeLayout(false);
            this._tabBit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numBitAddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numBitCount)).EndInit();
            this._tabRandom.ResumeLayout(false);
            this._tabRandom.PerformLayout();
            this._tabLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._statusPanel)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
