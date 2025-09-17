# DesktopBeatLight for .NET 8

![demo](demo.gif)  
*实时音频频谱桌面灯带 —— 让声音看得见*

## 项目概述
DesktopBeatLight 是一款基于 .NET 8 开发的 Windows 桌面音频可视化工具，通过捕获系统音频、进行 FFT 分析，在桌面边缘生成实时响应的动态灯带效果。项目以"轻量高效"为核心设计理念，采用 NativeAOT 编译技术，实现零依赖、高性能的用户体验。

### 核心特性
- ⚡ **50 FPS 原生渲染**：精准 20ms 间隔定时，确保流畅视觉体验
- 📦 **极致轻量化**：单文件可执行程序（≈420KB），无外部依赖
- 🎨 **3 种动态主题**：Fire（火焰）/ Ocean（海洋）/ Neon（霓虹），支持实时切换
- 🖥️ **全 DPI 适配**：完美支持 100%/150%/200% 缩放比例
- 🔌 **即插即用**：无需安装、无需管理员权限、USB 设备自动识别

## 需求说明

### 功能需求
1. **音频捕获**
   - 实时抓取系统输出音频（支持扬声器、耳机等所有输出设备）
   - 实现 RMS 音量检测（≤-40dB 判定为静音）
   - 静音持续 2 秒后自动暂停灯带渲染

2. **频谱分析**
   - 基于 FFT 算法将音频转换为频谱数据
   - 优化频域分辨率，平衡性能与精度
   - 实现频谱平滑过渡，避免视觉闪烁

3. **视觉渲染**
   - 在桌面底部边缘绘制动态灯带（高度自适应，默认 8px）
   - 支持 3 种主题切换（切换响应时间 ≤100ms）
   - 实现 50 FPS 稳定渲染，30 秒内掉帧 ≤12 次

4. **系统适配**
   - 自动适配不同屏幕分辨率与 DPI 缩放
   - 支持多显示器环境（仅主显示器生效）
   - 窗口置顶但不拦截鼠标事件

### 非功能需求
1. **性能指标**
   - 冷启动时间 ≤350ms
   - 内存占用 ≤30MB
   - CPU 使用率 ≤5%（常规负载）
   - 单文件体积 ≤500KB

2. **兼容性**
   - 支持 Windows 10 1903+ / Windows 11 所有版本
   - 兼容常见音频设备（内置声卡、USB 音频、虚拟声卡）
   - 适配 1080p/2K/4K 分辨率

3. **用户体验**
   - 零配置启动（无需手动设置）
   - 提供简洁右键菜单（主题切换/退出）
   - 无弹窗广告，无后台进程残留

## 快速开始

### 直接使用
1. 从 [Releases](https://github.com/yourname/DesktopBeatLight/releases) 下载 `DesktopBeatLight_v1.0.0.zip`
2. 解压压缩包，双击 `DesktopBeatLight.exe` 运行
3. 播放音乐/视频，观察桌面底部的动态灯带效果

> 提示：右键点击灯带可切换主题或退出程序

### 从源码构建
#### 环境依赖
- .NET 8 SDK ≥ 8.0.204
- Visual Studio 2022 17.9+
  - 安装工作负载：`.NET 桌面开发`
  - 安装组件：`NativeAOT 支持的 MSBuild 组件`

#### 一键构建git clone https://github.com/yourname/DesktopBeatLight.git
cd DesktopBeatLight
./publish.bat  # 输出路径：Release\AOT\DesktopBeatLight.exe
> 首次构建约需 30 秒，生成文件大小约 420KB

## 架构设计

### 三层架构（无循环依赖）┌─ DesktopBeatLight.UI      # 表现层
│  ├─ 主窗口与灯带控件
│  ├─ 主题渲染实现
│  └─ 用户交互逻辑
│
├─ DesktopBeatLight.Core    # 核心层
│  ├─ FFT 音频分析引擎
│  ├─ 主题数据模型(DTO)
│  └─ 配置序列化(Json源生成)
│
└─ DesktopBeatLight.Audio   # 音频层
   ├─ NAudio 封装(环回捕获)
   ├─ 静音检测算法
   └─ 音频缓冲区管理
### 关键技术实现
| 功能 | .NET 8 实现方案 |
|------|----------------|
| 50fps 定时 | `PeriodicTimer` 异步循环（精确 20ms 间隔） |
| 单文件瘦身 | NativeAOT + `TrimMode=full` + 不变全球化 |
| 配置持久化 | `System.Text.Json` 源生成器（零反射） |
| DPI 适配 | `Application.SetHighDpiMode(SystemAware)` + `DeviceDpi` 缩放 |
| 静音检测 | RMS 计算 + 100 帧连续判断（2 秒） |
| 体积控制 | CI 流程中集成 `dotnet size` 检查（硬限制 500KB） |

## 开发指南

### 代码规范
1. 遵循 C# 11 语法规范，优先使用值类型（`struct`）和 `readonly` 修饰
2. 跨层通信使用 `ReadOnlySpan<T>` 避免内存分配
3. UI 线程与工作线程严格分离（使用 `Invoke` 切换上下文）
4. 所有序列化操作必须使用源生成器（禁用反射）

### 核心模块开发要点

#### 1. 音频捕获模块// 核心代码示例（AudioCapture.cs）
using NAudio.Wave;

public class AudioCapture : IDisposable
{
    private WasapiLoopbackCapture _capture;
    private readonly SilenceDetector _silenceDetector = new(-40); // -40dB 阈值
    
    public event Action<ReadOnlySpan<float>> AudioDataAvailable;
    
    public void Start()
    {
        _capture = new WasapiLoopbackCapture();
        _capture.DataAvailable += OnDataAvailable;
        _capture.StartRecording();
    }
    
    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        // 转换为单声道浮点数据
        var floatData = ConvertToFloat(e.Buffer, e.BytesRecorded);
        
        // 检测静音状态
        _silenceDetector.Process(floatData);
        
        // 触发数据可用事件
        AudioDataAvailable?.Invoke(floatData);
    }
    
    // 其他实现...
}
#### 2. FFT 分析模块// 核心代码示例（FftAnalyzer.cs）
public class FftAnalyzer
{
    private readonly Complex[] _fftBuffer;
    private readonly FastFourierTransform _fft;
    private readonly int _fftSize = 1024; // 2的幂次，优化性能
    
    public FftAnalyzer()
    {
        _fftBuffer = new Complex[_fftSize];
        _fft = new FastFourierTransform();
    }
    
    public Span<float> Analyze(ReadOnlySpan<float> audioData)
    {
        // 填充FFT缓冲区
        audioData.CopyTo(_fftBuffer.AsSpan().Slice(0, audioData.Length));
        
        // 执行FFT
        _fft.Transform(_fftBuffer, FourierTransform.Direction.Forward);
        
        // 计算频谱 magnitude
        return ComputeMagnitudes(_fftBuffer);
    }
    
    // 其他实现...
}
#### 3. 灯带渲染模块// 核心代码示例（LightStripControl.cs）
public class LightStripControl : Control
{
    private readonly ThemeRenderer _themeRenderer;
    private float[] _spectrumData = Array.Empty<float>();
    
    public LightStripControl()
    {
        DoubleBuffered = true; // 启用双缓冲防闪烁
        _themeRenderer = new ThemeRenderer(Theme.Fire);
    }
    
    public void UpdateSpectrum(float[] data)
    {
        _spectrumData = data;
        Invalidate(); // 触发重绘
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        // 根据当前主题渲染灯带
        _themeRenderer.Render(e.Graphics, ClientRectangle, _spectrumData);
    }
    
    // 其他实现...
}
## 测试验证

### 自动化测试用例（GitHub Actions）
| 测试编号 | 测试内容 | 验收标准 |
|----------|----------|----------|
| TC-F1 | 帧率稳定性 | 240fps摄像头录制30秒，掉帧≤12次 |
| TC-F2 | 音频响应 | 浏览器播放YouTube，灯带1秒内响应 |
| TC-F3 | 主题切换 | 切换耗时≤100ms，无明显卡顿 |
| TC-F4 | 静音检测 | 静音后2.0±0.2秒内灯带渐暗 |
| TC-F5 | 体积控制 | 生成的exe文件≤500KB |
| TC-NF0 | 启动速度 | 冷启动时间≤350ms |
| TC-NF1 | DPI适配 | 200%缩放时控件偏移≤2px |

### 手动测试清单
1. 在不同Windows版本（Win10 1903+/Win11）验证功能完整性
2. 测试各类音频设备（内置声卡、USB耳机、虚拟声卡）
3. 验证高分辨率（4K）和多显示器环境下的显示效果
4. 连续运行24小时，检查内存泄漏和稳定性

## 项目结构/.github/workflows/ci.yml     # CI流水线（构建+测试）
/src
  /DesktopBeatLight.UI        # WinForms主程序
  /DesktopBeatLight.Core      # 核心逻辑
  /DesktopBeatLight.Audio     # 音频处理
/tests                        # 测试脚本与基准数据
publish.bat                   # NativeAOT发布脚本
## 扩展 roadmap
- #15 屏幕边缘跑马灯（全屏覆盖模式）
- #16 BPM检测+键盘RGB同步
- #17 支持GIF导出（录制灯带效果）
- #20 跨平台支jie持（macOS/Linux via .NET MAUI）

## 许可证
[MIT](LICENSE) © 2025 axuege
    