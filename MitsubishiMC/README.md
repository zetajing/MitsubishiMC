# MitsubishiMC — 三菱 MC 协议通信工具

基于 **MC 协议 3E 帧（Ethernet）** 的 C# Windows 桌面应用，用于与三菱 MELSEC iQ-R / Q / L / FX 系列 PLC 进行通信。

## 功能一览

- **字元件读写** — D / W / R / ZR / M / L / TN / CN
- **位元件读写** — M / X / Y / L
- **批量读写** — 单次最多 960 字 / 3584 位
- **随机读写** — 非连续地址 1 次操作（最多 192 点）
- **同步 + 异步** — 所有操作均提供同步方法与 `async/await` 版本
- **线程安全** — 同步锁 + 异步信号量分离

## 项目结构

```
MitsubishiMC/
├── Program.cs                  # [STAThread] 程序入口
├── MainForm.cs                 # WinForms 业务逻辑（事件处理、PLC 通信）
├── MainForm.Designer.cs        # WinForms 设计器文件（控件布局）
├── MainForm.resx               # 窗体资源
├── MitsubishiMC.csproj         # .NET Framework 4.7.2 项目
└── McProtocol/                 # MC 协议核心库
    ├── McDevice.cs             # 设备类型枚举 & 地址值类型
    ├── McFrame3E.cs            # 3E 帧构建与解析（6 种命令）
    └── McProtocolTcp.cs        # TCP 通信主类（同步 + 异步）
```

## 快速开始

### 环境要求

- Visual Studio 2019+（或 `msbuild`）
- .NET Framework 4.7.2（Windows 10+ 自带）
- 一台三菱 PLC（MELSEC iQ-R / Q / L / FX 系列）

### 编译运行

```bash
# 用 Visual Studio
双击 MitsubishiMC.sln → 按 F5

# 或用 msbuild 命令行
msbuild MitsubishiMC.sln
.\MitsubishiMC\bin\Debug\MitsubishiMC.exe
```

### 界面操作

1. **连接 Tab** → 输入 PLC 的 IP 地址和端口号 → 点"连接"
2. **字元件 Tab** → 选类型（D/W/R…）、输入地址 → 读取/写入
3. **位元件 Tab** → 选类型（M/X/Y…）、输入地址 → 读取/翻转写入
4. **随机读写 Tab** → 每行一个地址 → 随机读取/随机写入
5. **日志 Tab** → 所有操作记录（深色终端风格）

## API 参考

### 初始化

```csharp
// 创建客户端
var plc = new McProtocolTcp("192.168.1.73", 5000);
plc.Connect();                // 同步
await plc.ConnectAsync();     // 异步

// 使用完毕后释放
plc.Close();
plc.Dispose();
```

### 字操作

```csharp
// 单字读取
ushort val = plc.ReadWord(McDeviceType.D, 100);
ushort val = await plc.ReadWordAsync(McDeviceType.D, 100);

// 批量读取
ushort[] batch = plc.ReadWords(McDeviceType.D, 100, 5);

// 单字写入
plc.WriteWord(McDeviceType.D, 100, 1234);

// 批量写入
plc.WriteWords(McDeviceType.D, 100, new ushort[] { 10, 20, 30 });
```

### 位操作

```csharp
// 读取单个位
bool m0 = plc.ReadBit(McDeviceType.M, 0);
bool m0 = await plc.ReadBitAsync(McDeviceType.M, 0);

// 批量读取位
bool[] bits = plc.ReadBits(McDeviceType.M, 0, 8);

// 写入单个位
plc.WriteBit(McDeviceType.M, 0, true);

// 批量写入位
plc.WriteBits(McDeviceType.M, 10, new bool[] { true, false, true });
```

### 随机操作

```csharp
// 随机读取（非连续地址）
ushort[] vals = plc.RandomReadWords(
    new McAddress(McDeviceType.D, 100),
    new McAddress(McDeviceType.D, 200),
    new McAddress(McDeviceType.W, 10)
);

// 随机写入
plc.RandomWriteWords(
    new McAddressValue(new McAddress(McDeviceType.D, 500), 999),
    new McAddressValue(new McAddress(McDeviceType.D, 501), 888)
);
```

## 3E 帧协议说明

### 报文结构（请求）

| 偏移 | 长度 | 说明 |
|------|------|------|
| 0 | 2 | 子报头 `0x5000` |
| 2 | 1 | 网络号 `0x00` |
| 3 | 1 | PC 号 `0xFF` |
| 4 | 2 | 目标模块 I/O No. `0x03FF` |
| 6 | 1 | 目标站号 `0x00` |
| 7 | 2 | 请求数据长度 |
| 9 | 2 | 超时时间（默认 10×250ms） |
| 11 | 2 | 命令（`0x0401` 读取 / `0x1401` 写入） |
| 13 | 2 | 子命令（`0x0000` 字 / `0x0001` 位） |
| 15+ | | 地址 & 数据 |

### 命令代码

| 命令 | 功能 | 最大点数 |
|------|------|---------|
| `0x0401` | 批量读取（字/位） | 960 字 / 3584 位 |
| `0x1401` | 批量写入（字/位） | 960 字 / 3584 位 |
| `0x0403` | 随机读取（字） | 192 点 |
| `0x1402` | 随机写入（字） | 192 点 |

### 设备代码

| 设备 | 代码 | 说明 |
|------|------|------|
| D | `0xA8` | 数据寄存器 |
| W | `0xB4` | 链接寄存器 |
| R | `0xAF` | 文件寄存器 |
| M | `0x90` | 内部继电器 |
| X | `0x9C` | 输入继电器 |
| Y | `0x9D` | 输出继电器 |
| L | `0x92` | 锁存继电器 |
| TN | `0xC0` | 定时器当前值 |
| CN | `0xC5` | 计数器当前值 |

## 技术要点

- **.NET Framework 4.7.2** 兼容，无外部 NuGet 依赖
- **小端字节序**，使用手动 `WriteU16LE` / `ReadU16LE` 方法
- **TCP 粘包处理** — `ReadExactly` 确保完整帧接收
- **线程安全** — 同步操作使用 `lock`，异步使用 `SemaphoreSlim`
- **异常处理** — `McProtocolException` 携带 PLC 返回的结束代码

## 许可证

MIT License
