using System;

namespace MitsubishiMC.McProtocol
{
    /// <summary>
    /// MC 协议 3E 帧构建与解析（Ethernet 二进制模式 · net472 兼容版）
    /// </summary>
    internal static class McFrame3E
    {
        // ─── 子报头 ─────────────────────────────────────────
        private const ushort SubheaderRequest = 0x5000;

        // ─── 固定头部字段 ───────────────────────────────────
        private const byte NetworkNo = 0x00;
        private const byte PcNo = 0xFF;
        private const ushort DestIoNo = 0x03FF;
        private const byte DestStationNo = 0x00;

        // ─── 默认超时 ───────────────────────────────────────
        private const ushort DefaultTimer = 0x000A; // 10 × 250ms = 2.5s

        // ─── 命令代码 ───────────────────────────────────────
        private const ushort CmdBatchRead   = 0x0401;
        private const ushort CmdBatchWrite  = 0x1401;
        private const ushort CmdRandomRead  = 0x0403;
        private const ushort CmdRandomWrite = 0x1402;

        // ─── 子命令 ─────────────────────────────────────────
        private const ushort SubcmdWord = 0x0000;
        private const ushort SubcmdBit  = 0x0001;

        // ═══════════════════════════════════════════════════
        // 1. 批量读取字
        // ═══════════════════════════════════════════════════
        public static byte[] BuildReadWordsRequest(McAddress start, ushort count)
        {
            const int appLen = 11;
            byte[] header = BuildHeader(appLen);
            var packet = new byte[header.Length + appLen];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            int pos = header.Length;

            WriteU16LE(packet, ref pos, CmdBatchRead);
            WriteU16LE(packet, ref pos, SubcmdWord);
            packet[pos++] = McDeviceCode.GetCode(start.Device);
            WriteI32LE3(packet, ref pos, start.Number);
            WriteU16LE(packet, ref pos, count);
            return packet;
        }

        // ═══════════════════════════════════════════════════
        // 2. 批量写入字
        // ═══════════════════════════════════════════════════
        public static byte[] BuildWriteWordsRequest(McAddress start, ushort[] values)
        {
            int count = values.Length;
            const int fixedPart = 11;
            int appLen = fixedPart + count * 2;
            byte[] header = BuildHeader(appLen);
            var packet = new byte[header.Length + appLen];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            int pos = header.Length;

            WriteU16LE(packet, ref pos, CmdBatchWrite);
            WriteU16LE(packet, ref pos, SubcmdWord);
            packet[pos++] = McDeviceCode.GetCode(start.Device);
            WriteI32LE3(packet, ref pos, start.Number);
            WriteU16LE(packet, ref pos, (ushort)count);

            foreach (ushort v in values)
                WriteU16LE(packet, ref pos, v);
            return packet;
        }

        // ═══════════════════════════════════════════════════
        // 3. 批量读取位
        // ═══════════════════════════════════════════════════
        public static byte[] BuildReadBitsRequest(McAddress start, ushort count)
        {
            const int appLen = 11;
            byte[] header = BuildHeader(appLen);
            var packet = new byte[header.Length + appLen];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            int pos = header.Length;

            WriteU16LE(packet, ref pos, CmdBatchRead);
            WriteU16LE(packet, ref pos, SubcmdBit);
            packet[pos++] = McDeviceCode.GetCode(start.Device);
            WriteI32LE3(packet, ref pos, start.Number);
            WriteU16LE(packet, ref pos, count);
            return packet;
        }

        // ═══════════════════════════════════════════════════
        // 4. 批量写入位
        // ═══════════════════════════════════════════════════
        public static byte[] BuildWriteBitsRequest(McAddress start, bool[] values)
        {
            int count = values.Length;
            int byteLen = (count + 7) / 8;
            const int fixedPart = 11;
            int appLen = fixedPart + byteLen;
            byte[] header = BuildHeader(appLen);
            var packet = new byte[header.Length + appLen];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            int pos = header.Length;

            WriteU16LE(packet, ref pos, CmdBatchWrite);
            WriteU16LE(packet, ref pos, SubcmdBit);
            packet[pos++] = McDeviceCode.GetCode(start.Device);
            WriteI32LE3(packet, ref pos, start.Number);
            WriteU16LE(packet, ref pos, (ushort)count);

            // bool[] 打包为字节 (LSB = 第1位)
            for (int i = 0; i < byteLen; i++)
            {
                byte b = 0;
                for (int j = 0; j < 8; j++)
                {
                    int idx = i * 8 + j;
                    if (idx < count && values[idx])
                        b |= (byte)(1 << j);
                }
                packet[pos++] = b;
            }
            return packet;
        }

        // ═══════════════════════════════════════════════════
        // 5. 随机读取字
        // ═══════════════════════════════════════════════════
        public static byte[] BuildRandomReadWordsRequest(McAddress[] addresses)
        {
            int count = addresses.Length;
            int appLen = 2 + count * 6;
            byte[] header = BuildHeader(appLen);
            var packet = new byte[header.Length + appLen];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            int pos = header.Length;

            WriteU16LE(packet, ref pos, CmdRandomRead);
            WriteU16LE(packet, ref pos, SubcmdWord);
            WriteU16LE(packet, ref pos, (ushort)count);

            foreach (var addr in addresses)
            {
                packet[pos++] = McDeviceCode.GetCode(addr.Device);
                WriteI32LE3(packet, ref pos, addr.Number);
                pos += 2; // 预留 2 字节
            }
            return packet;
        }

        // ═══════════════════════════════════════════════════
        // 6. 随机写入字
        // ═══════════════════════════════════════════════════
        public static byte[] BuildRandomWriteWordsRequest(McAddressValue[] items)
        {
            int count = items.Length;
            int appLen = 2 + count * 8;
            byte[] header = BuildHeader(appLen);
            var packet = new byte[header.Length + appLen];
            Buffer.BlockCopy(header, 0, packet, 0, header.Length);
            int pos = header.Length;

            WriteU16LE(packet, ref pos, CmdRandomWrite);
            WriteU16LE(packet, ref pos, SubcmdWord);
            WriteU16LE(packet, ref pos, (ushort)count);

            foreach (var item in items)
            {
                packet[pos++] = McDeviceCode.GetCode(item.Address.Device);
                WriteI32LE3(packet, ref pos, item.Address.Number);
                pos += 2; // 预留 2 字节
                WriteU16LE(packet, ref pos, item.Value);
            }
            return packet;
        }

        // ═══════════════════════════════════════════════════
        // 7. 解析响应
        // ═══════════════════════════════════════════════════
        public static void ParseResponse(byte[] response, out bool success,
                                         out ushort endCode, out byte[] data)
        {
            success = false;
            endCode = 0xFFFF;
            data = null;

            if (response == null || response.Length < 11) return;

            int pos = 7; // 子报头(2) + 网络号(1) + PC号(1) + I/O No.(2) + 站号(1)

            if (pos + 2 > response.Length) return;
            ushort respDataLen = ReadU16LE(response, pos); pos += 2;

            if (pos + 2 > response.Length) return;
            endCode = ReadU16LE(response, pos); pos += 2;

            if (endCode != 0x0000) return;

            int dataLen = respDataLen - 2;
            if (dataLen < 0) return;

            data = new byte[dataLen];
            Buffer.BlockCopy(response, pos, data, 0, dataLen);
            success = true;
        }

        // ──────── 私有：构建公共头部 ────────────────────────
        private static byte[] BuildHeader(int appDataLength)
        {
            var header = new byte[9];
            int pos = 0;

            WriteU16LE(header, ref pos, SubheaderRequest);
            header[pos++] = NetworkNo;
            header[pos++] = PcNo;
            WriteU16LE(header, ref pos, DestIoNo);
            header[pos++] = DestStationNo;
            WriteU16LE(header, ref pos, (ushort)(appDataLength + 2));
            WriteU16LE(header, ref pos, DefaultTimer);
            return header;
        }

        // ──────── 字节序辅助方法 ────────────────────────────
        private static void WriteU16LE(byte[] buf, ref int pos, ushort val)
        {
            buf[pos++] = (byte)val;
            buf[pos++] = (byte)(val >> 8);
        }

        private static void WriteI32LE3(byte[] buf, ref int pos, int val)
        {
            buf[pos++] = (byte)val;
            buf[pos++] = (byte)(val >> 8);
            buf[pos++] = (byte)(val >> 16);
            // 只写 3 字节，高位丢弃
        }

        internal static ushort ReadU16LE(byte[] buf, int pos)
        {
            return (ushort)(buf[pos] | (buf[pos + 1] << 8));
        }
    }

    /// <summary>
    /// 用于随机写入的辅助类型（地址 + 值）
    /// </summary>
    public readonly struct McAddressValue
    {
        public McAddress Address { get; }
        public ushort Value { get; }

        public McAddressValue(McAddress address, ushort value)
        {
            Address = address;
            Value = value;
        }
    }
}
