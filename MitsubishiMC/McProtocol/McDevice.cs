using System;

namespace MitsubishiMC.McProtocol
{
    /// <summary>
    /// 三菱 PLC 软元件类型（字访问用）
    /// </summary>
    public enum McDeviceType
    {
        D, W, R, SD, Z, ZR, JnW,
        M, X, Y, L,
        TN, SS, CN,
    }

    /// <summary>
    /// 软元件地址描述（值类型）
    /// </summary>
    public readonly struct McAddress : IEquatable<McAddress>
    {
        public McDeviceType Device { get; }
        public int Number { get; }

        public McAddress(McDeviceType device, int number)
        {
            Device = device;
            Number = number;
        }

        public override string ToString() => $"{Device}{Number}";

        public bool Equals(McAddress other)
            => Device == other.Device && Number == other.Number;

        public override bool Equals(object obj)
            => obj is McAddress other && Equals(other);

        public override int GetHashCode()
            => ((int)Device * 397) ^ Number;

        public static bool operator ==(McAddress left, McAddress right)
            => left.Equals(right);

        public static bool operator !=(McAddress left, McAddress right)
            => !left.Equals(right);
    }

    /// <summary>
    /// 设备编码映射表（字访问命令 0x0401 / 0x1401 用）
    /// </summary>
    internal static class McDeviceCode
    {
        public static byte GetCode(McDeviceType type)
        {
            switch (type)
            {
                case McDeviceType.D:  return 0xA8;
                case McDeviceType.W:  return 0xB4;
                case McDeviceType.R:  return 0xAF;
                case McDeviceType.SD: return 0xA9;
                case McDeviceType.Z:  return 0xCC;
                case McDeviceType.ZR: return 0xB0;
                case McDeviceType.JnW: return 0xB5;
                case McDeviceType.M:  return 0x90;
                case McDeviceType.X:  return 0x9C;
                case McDeviceType.Y:  return 0x9D;
                case McDeviceType.L:  return 0x92;
                case McDeviceType.TN: return 0xC0;
                case McDeviceType.SS: return 0xC1;
                case McDeviceType.CN: return 0xC5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type),
                        $"不支持的设备类型: {type}");
            }
        }
    }
}
