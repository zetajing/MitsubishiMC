using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MitsubishiMC.McProtocol;

namespace MitsubishiMC
{
    partial class MainForm : Form
    {
        private McProtocolTcp _plc;
        private CancellationTokenSource _cts;

        public MainForm()
        {
            InitializeComponent();

            // 绑定事件
            this._btnConnect.Click += new EventHandler(this.OnConnect);
            this._btnDisconnect.Click += new EventHandler(this.OnDisconnect);
            this._btnWordRead.Click += new EventHandler(this.OnWordRead);
            this._btnWordWrite.Click += new EventHandler(this.OnWordWrite);
            this._btnWordBatchRead.Click += new EventHandler(this.OnWordBatchRead);
            this._btnBitRead.Click += new EventHandler(this.OnBitRead);
            this._btnBitWrite.Click += new EventHandler(this.OnBitWrite);
            this._btnBitBatchRead.Click += new EventHandler(this.OnBitBatchRead);
            this._btnRandRead.Click += new EventHandler(this.OnRandomRead);
            this._btnRandWrite.Click += new EventHandler(this.OnRandomWrite);
            this.Load += new EventHandler(this.Form_Load);

            // 设计时不创建 PLC 客户端
            if (!DesignMode)
            {
                _plc = new McProtocolTcp("192.168.1.73", 6000);
                UpdateConnectionUI(false);
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // 窗体加载后的初始化（设计时不会执行）
        }

        // ═══════════════════════════════════════════════════
        // 事件：连接 / 断开
        // ═══════════════════════════════════════════════════
        private async void OnConnect(object sender, EventArgs e)
        {
            SetControlsEnabled(false);
            try
            {
                string ip = _txtIp.Text.Trim();
                int port = int.Parse(_txtPort.Text.Trim());

                if (_plc != null)
                {
                    _plc.Dispose();
                }
                _plc = new McProtocolTcp(ip, port);

                if (_cts != null)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                }
                _cts = new CancellationTokenSource();

                Log("正在连接 {0}:{1} ...", ip, port);
                await _plc.ConnectAsync(_cts.Token);
                Log("✓ 连接成功");
                UpdateConnectionUI(true);
            }
            catch (Exception ex)
            {
                Log("✗ 连接失败: {0}", ex.Message);
                MessageBox.Show(string.Format("连接失败:\n{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void OnDisconnect(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }
            if (_plc != null)
            {
                _plc.Close();
            }
            Log("已断开连接");
            UpdateConnectionUI(false);
        }

        // ═══════════════════════════════════════════════════
        // 事件：字元件
        // ═══════════════════════════════════════════════════
        private async void OnWordRead(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var dev = ParseDevice(_cbWordDevice.Text);
                int addr = (int)_numWordAddr.Value;
                ushort val = await _plc.ReadWordAsync(dev, addr);
                _txtWordValue.Text = val.ToString();
                Log("读取 {0}{1} = {2}", dev, addr, val);
            });
        }

        private async void OnWordWrite(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var dev = ParseDevice(_cbWordDevice.Text);
                int addr = (int)_numWordAddr.Value;
                ushort val = ushort.Parse(_txtWordValue.Text.Trim());
                await _plc.WriteWordAsync(dev, addr, val);

                ushort verify = await _plc.ReadWordAsync(dev, addr);
                Log("写入 {0}{1} = {2}  ✓ (回读={3})", dev, addr, val, verify);
            });
        }

        private async void OnWordBatchRead(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var dev = ParseDevice(_cbWordDevice.Text);
                int start = (int)_numWordAddr.Value;
                ushort count = Math.Min((ushort)(int)_numWordCount.Value, (ushort)20);

                ushort[] vals = await _plc.ReadWordsAsync(dev, start, count);
                _lbWordResult.BeginUpdate();
                _lbWordResult.Items.Clear();
                for (int i = 0; i < vals.Length; i++)
                    _lbWordResult.Items.Add(string.Format("{0}{1,4} = {2,6}", dev, start + i, vals[i]));
                _lbWordResult.EndUpdate();

                Log("批量读取 {0}{1}~{2} 共 {3} 点", dev, start, start + count - 1, count);
            });
        }

        // ═══════════════════════════════════════════════════
        // 事件：位元件
        // ═══════════════════════════════════════════════════
        private async void OnBitRead(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var dev = ParseDevice(_cbBitDevice.Text);
                int addr = (int)_numBitAddr.Value;
                bool val = await _plc.ReadBitAsync(dev, addr);
                Log("读取 {0}{1} = {2}", dev, addr, val ? "ON" : "OFF");
                MessageBox.Show(string.Format("{0}{1} = {2}", dev, addr, val ? "ON" : "OFF"), "位状态");
            });
        }

        private async void OnBitWrite(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var dev = ParseDevice(_cbBitDevice.Text);
                int addr = (int)_numBitAddr.Value;

                bool cur = await _plc.ReadBitAsync(dev, addr);
                bool newVal = !cur;
                await _plc.WriteBitAsync(dev, addr, newVal);

                bool verify = await _plc.ReadBitAsync(dev, addr);
                Log("写入 {0}{1} = {2}  ✓ (回读={3})", dev, addr,
                    newVal ? "ON" : "OFF", verify ? "ON" : "OFF");
            });
        }

        private async void OnBitBatchRead(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var dev = ParseDevice(_cbBitDevice.Text);
                int start = (int)_numBitAddr.Value;
                ushort count = 16;

                bool[] vals = await _plc.ReadBitsAsync(dev, start, count);
                _lbBitResult.BeginUpdate();
                _lbBitResult.Items.Clear();
                for (int i = 0; i < vals.Length; i++)
                    _lbBitResult.Items.Add(string.Format("{0}{1,3} = {2}", dev, start + i, vals[i] ? "ON " : "OFF"));
                _lbBitResult.EndUpdate();

                Log("批量读取 {0}{1}~{2} 共 {3} 点", dev, start, start + count - 1, count);
            });
        }

        // ═══════════════════════════════════════════════════
        // 事件：随机读写
        // ═══════════════════════════════════════════════════
        private async void OnRandomRead(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var addrs = ParseAddressLines(_txtRandAddrs.Text);
                if (addrs.Length == 0) return;

                ushort[] vals = await _plc.RandomReadWordsAsync(addrs);
                _lbWordResult.BeginUpdate();
                _lbWordResult.Items.Clear();
                for (int i = 0; i < vals.Length; i++)
                    _lbWordResult.Items.Add(string.Format("{0} = {1}", addrs[i], vals[i]));
                _lbWordResult.EndUpdate();

                Log("随机读取 {0} 点 ✓", vals.Length);
            });
        }

        private async void OnRandomWrite(object sender, EventArgs e)
        {
            await SafeCall(async () =>
            {
                var items = ParseValueLines(_txtRandAddrs.Text);
                if (items.Length == 0) return;

                await _plc.RandomWriteWordsAsync(items);

                var addrs = new McAddress[items.Length];
                for (int i = 0; i < items.Length; i++)
                    addrs[i] = items[i].Address;
                ushort[] verify = await _plc.RandomReadWordsAsync(addrs);

                _lbWordResult.BeginUpdate();
                _lbWordResult.Items.Clear();
                for (int i = 0; i < verify.Length; i++)
                    _lbWordResult.Items.Add(string.Format("{0} = {1} ✓", addrs[i], verify[i]));
                _lbWordResult.EndUpdate();

                Log("随机写入 {0} 点 ✓", items.Length);
            });
        }

        // ═══════════════════════════════════════════════════
        // 辅助方法
        // ═══════════════════════════════════════════════════
        private McDeviceType ParseDevice(string text)
        {
            return (McDeviceType)Enum.Parse(typeof(McDeviceType), text.Trim());
        }

        private McAddress[] ParseAddressLines(string text)
        {
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new System.Collections.Generic.List<McAddress>();
            foreach (var line in lines)
            {
                var s = line.Trim();
                if (string.IsNullOrEmpty(s)) continue;
                list.Add(ParseAddress(s));
            }
            return list.ToArray();
        }

        private McAddress ParseAddress(string s)
        {
            int i = 0;
            while (i < s.Length && char.IsLetter(s[i])) i++;
            string devStr = s.Substring(0, i);
            int num = int.Parse(s.Substring(i));
            return new McAddress(ParseDevice(devStr), num);
        }

        private McAddressValue[] ParseValueLines(string text)
        {
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new System.Collections.Generic.List<McAddressValue>();
            foreach (var line in lines)
            {
                var s = line.Trim();
                if (string.IsNullOrEmpty(s)) continue;

                string[] parts = s.Split('=', ',');
                if (parts.Length != 2) continue;

                var addr = ParseAddress(parts[0].Trim());
                ushort val = ushort.Parse(parts[1].Trim());
                list.Add(new McAddressValue(addr, val));
            }
            return list.ToArray();
        }

        private async Task SafeCall(Func<Task> action)
        {
            if (_plc == null || !_plc.Connected)
            {
                MessageBox.Show("请先连接 PLC", "提示");
                _tabControl.SelectedIndex = 0;
                return;
            }
            SetControlsEnabled(false);
            try
            {
                await action();
            }
            catch (McProtocolException ex)
            {
                Log("✗ MC 协议错误: {0}", ex.Message);
                MessageBox.Show(ex.Message, "MC 协议错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log("✗ 错误: {0}", ex.Message);
                MessageBox.Show(ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void Log(string format, params object[] args)
        {
            if (_rtbLog == null || _rtbLog.IsDisposed) return;
            string msg = string.Format(format, args);
            string line = string.Format("{0:HH:mm:ss.fff}  {1}", DateTime.Now, msg);
            _rtbLog.AppendText(line + "\n");
            _rtbLog.ScrollToCaret();
        }

        private void UpdateConnectionUI(bool connected)
        {
            _lblStatus.Text = connected ? "已连接" : "未连接";
            _lblStatus.ForeColor = connected ? Color.Green : Color.Gray;
            _btnConnect.Enabled = !connected;
            _btnDisconnect.Enabled = connected;
            _btnWordRead.Enabled = connected;
            _btnWordWrite.Enabled = connected;
            _btnWordBatchRead.Enabled = connected;
            _btnBitRead.Enabled = connected;
            _btnBitWrite.Enabled = connected;
            _btnBitBatchRead.Enabled = connected;
            _btnRandRead.Enabled = connected;
            _btnRandWrite.Enabled = connected;
            _statusPanel.Text = connected ? "已连接" : "就绪";
        }

        private void SetControlsEnabled(bool enabled)
        {
            _btnConnect.Enabled = enabled && (_plc != null && !_plc.Connected);
            _btnDisconnect.Enabled = enabled && (_plc != null && _plc.Connected);
            if (enabled && _plc != null)
                UpdateConnectionUI(_plc.Connected);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
            if (_plc != null)
            {
                _plc.Dispose();
            }
            base.OnFormClosed(e);
        }
    }
}
