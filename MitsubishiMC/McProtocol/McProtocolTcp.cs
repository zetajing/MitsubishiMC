using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MitsubishiMC.McProtocol
{
    /// <summary>
    /// MC 协议 TCP 通信客户端（3E 帧 · net472 兼容版，同步 + 异步）
    /// </summary>
    public sealed class McProtocolTcp : IDisposable
    {
        private readonly string _host;
        private readonly int _port;
        private readonly int _sendTimeout;
        private readonly int _receiveTimeout;

        private TcpClient _tcp;
        private NetworkStream _stream;
        private readonly object _lock = new object();
        private readonly SemaphoreSlim _asyncLock = new SemaphoreSlim(1, 1);

        /// <summary>是否已连接</summary>
        public bool Connected
        {
            get
            {
                var tcp = _tcp;
                return tcp != null && tcp.Connected;
            }
        }

        public McProtocolTcp(string host, int port = 5000,
                             int sendTimeout = 3000, int receiveTimeout = 5000)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException(nameof(host));
            _host = host;
            _port = port;
            _sendTimeout = sendTimeout;
            _receiveTimeout = receiveTimeout;
        }

        // ═══════════════════════════════════════════════════
        // 连接 / 断开
        // ═══════════════════════════════════════════════════
        public void Connect()
        {
            Close();
            _tcp = new TcpClient();
            _tcp.SendTimeout = _sendTimeout;
            _tcp.ReceiveTimeout = _receiveTimeout;
            _tcp.Connect(_host, _port);
            _stream = _tcp.GetStream();
        }

        public async Task ConnectAsync(CancellationToken ct = default)
        {
            Close();
            _tcp = new TcpClient();
            _tcp.SendTimeout = _sendTimeout;
            _tcp.ReceiveTimeout = _receiveTimeout;
            // net472 的 ConnectAsync 没有 CancellationToken 重载
            await _tcp.ConnectAsync(_host, _port).ConfigureAwait(false);
            _stream = _tcp.GetStream();
        }

        public void Close()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
            if (_tcp != null)
            {
                _tcp.Dispose();
                _tcp = null;
            }
        }

        public void Dispose() => Close();

        // ═══════════════════════════════════════════════════
        // 批量读取字（同步）
        // ═══════════════════════════════════════════════════
        public ushort[] ReadWords(McDeviceType device, int startNo, ushort count)
            => ReadWords(new McAddress(device, startNo), count);

        public ushort[] ReadWords(McAddress start, ushort count)
        {
            if (count == 0) return Array.Empty<ushort>();
            if (count > 960) ThrowOutOfRange(nameof(count), "单次读取最多 960 字");

            var request = McFrame3E.BuildReadWordsRequest(start, count);
            var response = SendReceive(request);
            var data = ParseAndCheck(response, count * 2);

            var result = new ushort[count];
            for (int i = 0; i < count; i++)
                result[i] = McFrame3E.ReadU16LE(data, i * 2);
            return result;
        }

        public ushort ReadWord(McDeviceType device, int number)
            => ReadWords(device, number, 1)[0];

        // ═══════════════════════════════════════════════════
        // 批量写入字（同步）
        // ═══════════════════════════════════════════════════
        public void WriteWords(McDeviceType device, int startNo, ushort[] values)
            => WriteWords(new McAddress(device, startNo), values);

        public void WriteWords(McAddress start, ushort[] values)
        {
            if (values.Length == 0) return;
            if (values.Length > 960) ThrowOutOfRange(nameof(values), "单次写入最多 960 字");

            var request = McFrame3E.BuildWriteWordsRequest(start, values);
            var response = SendReceive(request);
            ParseAndCheck(response, 0);
        }

        public void WriteWord(McDeviceType device, int number, ushort value)
            => WriteWords(device, number, new ushort[] { value });

        // ═══════════════════════════════════════════════════
        // 批量读取位（同步）
        // ═══════════════════════════════════════════════════
        public bool[] ReadBits(McDeviceType device, int startNo, ushort count)
            => ReadBits(new McAddress(device, startNo), count);

        public bool[] ReadBits(McAddress start, ushort count)
        {
            if (count == 0) return Array.Empty<bool>();
            if (count > 3584) ThrowOutOfRange(nameof(count), "单次位读取最多 3584 点");

            var request = McFrame3E.BuildReadBitsRequest(start, count);
            var response = SendReceive(request);

            int byteLen = (count + 7) / 8;
            var data = ParseAndCheck(response, byteLen);

            var result = new bool[count];
            for (int i = 0; i < count; i++)
                result[i] = ((data[i / 8] >> (i % 8)) & 1) == 1;
            return result;
        }

        public bool ReadBit(McDeviceType device, int number)
            => ReadBits(device, number, 1)[0];

        // ═══════════════════════════════════════════════════
        // 批量写入位（同步）
        // ═══════════════════════════════════════════════════
        public void WriteBits(McDeviceType device, int startNo, bool[] values)
            => WriteBits(new McAddress(device, startNo), values);

        public void WriteBits(McAddress start, bool[] values)
        {
            if (values.Length == 0) return;
            if (values.Length > 3584) ThrowOutOfRange(nameof(values), "单次位写入最多 3584 点");

            var request = McFrame3E.BuildWriteBitsRequest(start, values);
            var response = SendReceive(request);
            ParseAndCheck(response, 0);
        }

        public void WriteBit(McDeviceType device, int number, bool value)
            => WriteBits(device, number, new bool[] { value });

        // ═══════════════════════════════════════════════════
        // 随机读写（同步）
        // ═══════════════════════════════════════════════════
        public ushort[] RandomReadWords(params McAddress[] addresses)
        {
            int count = addresses.Length;
            if (count == 0) return Array.Empty<ushort>();
            if (count > 192) ThrowOutOfRange(nameof(addresses), "随机读取最多 192 点");

            var request = McFrame3E.BuildRandomReadWordsRequest(addresses);
            var response = SendReceive(request);
            var data = ParseAndCheck(response, count * 2);

            var result = new ushort[count];
            for (int i = 0; i < count; i++)
                result[i] = McFrame3E.ReadU16LE(data, i * 2);
            return result;
        }

        public void RandomWriteWords(params McAddressValue[] items)
        {
            if (items.Length == 0) return;
            if (items.Length > 192) ThrowOutOfRange(nameof(items), "随机写入最多 192 点");

            var request = McFrame3E.BuildRandomWriteWordsRequest(items);
            var response = SendReceive(request);
            ParseAndCheck(response, 0);
        }

        // ═══════════════════════════════════════════════════
        // ═══ 异步版本 ════════════════════════════════════
        // ═══════════════════════════════════════════════════

        public async Task<ushort[]> ReadWordsAsync(McDeviceType device, int startNo,
                                                    ushort count, CancellationToken ct = default)
            => await ReadWordsAsync(new McAddress(device, startNo), count, ct).ConfigureAwait(false);

        public async Task<ushort[]> ReadWordsAsync(McAddress start, ushort count,
                                                    CancellationToken ct = default)
        {
            if (count == 0) return Array.Empty<ushort>();
            if (count > 960) ThrowOutOfRange(nameof(count), "单次读取最多 960 字");

            var request = McFrame3E.BuildReadWordsRequest(start, count);
            var response = await SendReceiveAsync(request, ct).ConfigureAwait(false);
            var data = ParseAndCheck(response, count * 2);

            var result = new ushort[count];
            for (int i = 0; i < count; i++)
                result[i] = McFrame3E.ReadU16LE(data, i * 2);
            return result;
        }

        public async Task<ushort> ReadWordAsync(McDeviceType device, int number,
                                                 CancellationToken ct = default)
        {
            var vals = await ReadWordsAsync(device, number, 1, ct).ConfigureAwait(false);
            return vals[0];
        }

        public async Task WriteWordsAsync(McDeviceType device, int startNo,
                                           ushort[] values, CancellationToken ct = default)
            => await WriteWordsAsync(new McAddress(device, startNo), values, ct).ConfigureAwait(false);

        public async Task WriteWordsAsync(McAddress start, ushort[] values,
                                           CancellationToken ct = default)
        {
            if (values.Length == 0) return;
            if (values.Length > 960) ThrowOutOfRange(nameof(values), "单次写入最多 960 字");

            var request = McFrame3E.BuildWriteWordsRequest(start, values);
            var response = await SendReceiveAsync(request, ct).ConfigureAwait(false);
            ParseAndCheck(response, 0);
        }

        public async Task WriteWordAsync(McDeviceType device, int number,
                                          ushort value, CancellationToken ct = default)
            => await WriteWordsAsync(device, number, new ushort[] { value }, ct).ConfigureAwait(false);

        // ── 位 异步 ────────────────────────────────────────

        public async Task<bool[]> ReadBitsAsync(McDeviceType device, int startNo,
                                                 ushort count, CancellationToken ct = default)
            => await ReadBitsAsync(new McAddress(device, startNo), count, ct).ConfigureAwait(false);

        public async Task<bool[]> ReadBitsAsync(McAddress start, ushort count,
                                                 CancellationToken ct = default)
        {
            if (count == 0) return Array.Empty<bool>();
            if (count > 3584) ThrowOutOfRange(nameof(count), "单次位读取最多 3584 点");

            var request = McFrame3E.BuildReadBitsRequest(start, count);
            var response = await SendReceiveAsync(request, ct).ConfigureAwait(false);

            int byteLen = (count + 7) / 8;
            var data = ParseAndCheck(response, byteLen);

            var result = new bool[count];
            for (int i = 0; i < count; i++)
                result[i] = ((data[i / 8] >> (i % 8)) & 1) == 1;
            return result;
        }

        public async Task<bool> ReadBitAsync(McDeviceType device, int number,
                                              CancellationToken ct = default)
        {
            var vals = await ReadBitsAsync(device, number, 1, ct).ConfigureAwait(false);
            return vals[0];
        }

        public async Task WriteBitsAsync(McDeviceType device, int startNo,
                                          bool[] values, CancellationToken ct = default)
            => await WriteBitsAsync(new McAddress(device, startNo), values, ct).ConfigureAwait(false);

        public async Task WriteBitsAsync(McAddress start, bool[] values,
                                          CancellationToken ct = default)
        {
            if (values.Length == 0) return;
            if (values.Length > 3584) ThrowOutOfRange(nameof(values), "单次位写入最多 3584 点");

            var request = McFrame3E.BuildWriteBitsRequest(start, values);
            var response = await SendReceiveAsync(request, ct).ConfigureAwait(false);
            ParseAndCheck(response, 0);
        }

        public async Task WriteBitAsync(McDeviceType device, int number,
                                         bool value, CancellationToken ct = default)
            => await WriteBitsAsync(device, number, new bool[] { value }, ct).ConfigureAwait(false);

        // ── 随机读写 异步 ──────────────────────────────────

        public async Task<ushort[]> RandomReadWordsAsync(McAddress[] addresses,
                                                          CancellationToken ct = default)
        {
            int count = addresses.Length;
            if (count == 0) return Array.Empty<ushort>();
            if (count > 192) ThrowOutOfRange(nameof(addresses), "随机读取最多 192 点");

            var request = McFrame3E.BuildRandomReadWordsRequest(addresses);
            var response = await SendReceiveAsync(request, ct).ConfigureAwait(false);
            var data = ParseAndCheck(response, count * 2);

            var result = new ushort[count];
            for (int i = 0; i < count; i++)
                result[i] = McFrame3E.ReadU16LE(data, i * 2);
            return result;
        }

        public async Task RandomWriteWordsAsync(McAddressValue[] items,
                                                 CancellationToken ct = default)
        {
            if (items.Length == 0) return;
            if (items.Length > 192) ThrowOutOfRange(nameof(items), "随机写入最多 192 点");

            var request = McFrame3E.BuildRandomWriteWordsRequest(items);
            var response = await SendReceiveAsync(request, ct).ConfigureAwait(false);
            ParseAndCheck(response, 0);
        }

        // ──────── 私有：同步收发 ────────────────────────────
        private byte[] SendReceive(byte[] request)
        {
            lock (_lock)
            {
                EnsureConnected();
                _stream.Write(request, 0, request.Length);
                _stream.Flush();
                return ReceiveFullResponse();
            }
        }

        private byte[] ReceiveFullResponse()
        {
            byte[] headerBuf = ReadExactly(11);
            ushort respDataLen = McFrame3E.ReadU16LE(headerBuf, 7);
            byte[] dataBuf = ReadExactly(respDataLen);

            var full = new byte[headerBuf.Length + dataBuf.Length];
            Buffer.BlockCopy(headerBuf, 0, full, 0, headerBuf.Length);
            Buffer.BlockCopy(dataBuf, 0, full, headerBuf.Length, dataBuf.Length);
            return full;
        }

        private byte[] ReadExactly(int count)
        {
            var buffer = new byte[count];
            int offset = 0;
            while (offset < count)
            {
                int read = _stream.Read(buffer, offset, count - offset);
                if (read == 0)
                    throw new McProtocolException("PLC 连接意外断开");
                offset += read;
            }
            return buffer;
        }

        // ──────── 私有：异步收发 ────────────────────────────
        private async Task<byte[]> SendReceiveAsync(byte[] request, CancellationToken ct)
        {
            await _asyncLock.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                EnsureConnected();
                await _stream.WriteAsync(request, 0, request.Length, ct).ConfigureAwait(false);
                await _stream.FlushAsync(ct).ConfigureAwait(false);
                return await ReceiveFullResponseAsync(ct).ConfigureAwait(false);
            }
            finally
            {
                _asyncLock.Release();
            }
        }

        private async Task<byte[]> ReceiveFullResponseAsync(CancellationToken ct)
        {
            byte[] headerBuf = await ReadExactlyAsync(11, ct).ConfigureAwait(false);
            ushort respDataLen = McFrame3E.ReadU16LE(headerBuf, 7);
            byte[] dataBuf = await ReadExactlyAsync(respDataLen, ct).ConfigureAwait(false);

            var full = new byte[headerBuf.Length + dataBuf.Length];
            Buffer.BlockCopy(headerBuf, 0, full, 0, headerBuf.Length);
            Buffer.BlockCopy(dataBuf, 0, full, headerBuf.Length, dataBuf.Length);
            return full;
        }

        private async Task<byte[]> ReadExactlyAsync(int count, CancellationToken ct)
        {
            var buffer = new byte[count];
            int offset = 0;
            var stream = _stream;
            while (offset < count)
            {
                int read = await stream.ReadAsync(buffer, offset, count - offset, ct)
                                         .ConfigureAwait(false);
                if (read == 0)
                    throw new McProtocolException("PLC 连接意外断开");
                offset += read;
            }
            return buffer;
        }

        // ──────── 私有：通用 ────────────────────────────────
        private void EnsureConnected()
        {
            if (_tcp == null || !_tcp.Connected || _stream == null)
                throw new InvalidOperationException("未连接到 PLC，请先调用 Connect() 或 ConnectAsync()");
        }

        private static byte[] ParseAndCheck(byte[] response, int expectedDataLen)
        {
            McFrame3E.ParseResponse(response, out bool ok, out ushort endCode, out byte[] data);
            if (!ok)
                throw new McProtocolException($"操作失败", endCode);
            if (expectedDataLen > 0 && (data == null || data.Length < expectedDataLen))
                throw new McProtocolException(
                    $"响应数据不足: 期望 {expectedDataLen} 字节，实际 {(data?.Length ?? 0)} 字节");
            return data;
        }

        private static void ThrowOutOfRange(string paramName, string message)
            => throw new ArgumentOutOfRangeException(paramName, message);
    }

    /// <summary>
    /// MC 协议通信异常
    /// </summary>
    public sealed class McProtocolException : Exception
    {
        public ushort EndCode { get; }

        public McProtocolException(string message, ushort endCode)
            : base($"{message} (结束代码: 0x{endCode:X4})")
        {
            EndCode = endCode;
        }

        public McProtocolException(string message) : base(message)
        {
            EndCode = 0xFFFF;
        }
    }
}
