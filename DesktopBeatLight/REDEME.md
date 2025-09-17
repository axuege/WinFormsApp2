##  DesktopBeatLight for .NET 8
## 项目目录结构说明
 创建三层架构：表现层（UI）、核心层（Core）、音频层（Audio）
# 创建解决方案根目录
mkdir DesktopBeatLight
# 进入根目录
cd DesktopBeatLight

# 创建解决方案文件
dotnet new sln -n DesktopBeatLight

# 1. 创建 src 目录
mkdir src
cd src

# 2. 创建 UI 项目（WinForms）
dotnet new winforms -n DesktopBeatLight.UI
# 为 UI 项目创建子文件夹
mkdir DesktopBeatLight.UI\Controls
mkdir DesktopBeatLight.UI\Helpers
mkdir DesktopBeatLight.UI\Properties

# 3. 创建 Core 项目（类库）
dotnet new classlib -n DesktopBeatLight.Core
# 为 Core 项目创建子文件夹
mkdir DesktopBeatLight.Core\Abstractions
mkdir DesktopBeatLight.Core\Models
mkdir DesktopBeatLight.Core\Implementations
mkdir DesktopBeatLight.Core\Implementations\ThemeRenderers
mkdir DesktopBeatLight.Core\Helpers
mkdir DesktopBeatLight.Core\Constants

# 4. 创建 Audio 项目（类库）
dotnet new classlib -n DesktopBeatLight.Audio
# 为 Audio 项目创建子文件夹
mkdir DesktopBeatLight.Audio\Abstractions
mkdir DesktopBeatLight.Audio\Implementations
mkdir DesktopBeatLight.Audio\Helpers
mkdir DesktopBeatLight.Audio\Models

# 回到根目录
cd ..

# 1. 创建 tests 目录
mkdir tests
cd tests

# 2. 创建 Core 测试项目
dotnet new xunit -n DesktopBeatLight.Core.Tests
# 添加对 Core 项目的引用（需确保路径正确，回到 src 取 Core 项目）
dotnet add DesktopBeatLight.Core.Tests\DesktopBeatLight.Core.Tests.csproj reference ..\src\DesktopBeatLight.Core\DesktopBeatLight.Core.csproj

# 3. 创建 Audio 测试项目
dotnet new xunit -n DesktopBeatLight.Audio.Tests
# 添加对 Audio 项目的引用
dotnet add DesktopBeatLight.Audio.Tests\DesktopBeatLight.Audio.Tests.csproj reference ..\src\DesktopBeatLight.Audio\DesktopBeatLight.Audio.csproj

# 回到根目录
cd ..

# 将子项目（Project）添加到解决方案（DesktopBeatLight.sln）中
# 添加 src 下的项目
dotnet sln add src\DesktopBeatLight.UI\DesktopBeatLight.UI.csproj
dotnet sln add src\DesktopBeatLight.Core\DesktopBeatLight.Core.csproj
dotnet sln add src\DesktopBeatLight.Audio\DesktopBeatLight.Audio.csproj

# 添加 tests 下的项目
dotnet sln add tests\DesktopBeatLight.Core.Tests\DesktopBeatLight.Core.Tests.csproj
dotnet sln add tests\DesktopBeatLight.Audio.Tests\DesktopBeatLight.Audio.Tests.csproj


## DesktopBeatLight.sln                // 解决方案文件
├─ .github/
│  └─ workflows/
│     └─ ci.yml                     // GitHub Actions 自动化构建/测试流水线
├─ src/
│  ├─ DesktopBeatLight.UI/          // 表现层（WinForms 桌面应用）
│  ├─ DesktopBeatLight.Core/        // 核心层（业务逻辑、实体、接口）
│  └─ DesktopBeatLight.Audio/       // 音频层（音频捕获、静音检测）
├─ tests/                           // 测试层（单元测试、性能测试）
│  ├─ DesktopBeatLight.Core.Tests/
│  └─ DesktopBeatLight.Audio.Tests/
├─ publish.bat                      // 一键 NativeAOT 发布脚本
└─ README.md                        // 项目说明文档

DesktopBeatLight.UI/
├─ DesktopBeatLight.UI.csproj       // 项目配置（引用 Core/Audio 层、启用 AOT）
├─ Program.cs                       // 应用入口（依赖注入注册、启动主窗口）
├─ MainForm.cs                      // 主窗口（隐藏边框、置顶、绑定灯带控件）
├─ MainForm.Designer.cs             // 主窗口设计器代码（自动生成）
├─ MainForm.resx                    // 主窗口资源（图标、字符串）
├─ Controls/                        // 自定义控件文件夹
│  ├─ LightStripControl.cs          // 核心控件：桌面底部灯带（绑定频谱数据渲染）
│  ├─ LightStripControl.Designer.cs // 灯带控件设计器（可选，手动写布局可省略）
│  └─ ThemeComboBox.cs              // 主题选择下拉框（绑定 Core 层主题列表）
├─ Helpers/                         // UI 辅助工具
│  ├─ DpiHelper.cs                  // DPI 适配工具（计算控件缩放比例）
│  └─ GraphicsHelper.cs             // 绘图辅助（圆角矩形、渐变画笔封装）
└─ Properties/                      // 应用属性
   ├─ AssemblyInfo.cs               // 程序集信息（版本、作者）
   ├─ Resources.resx                // 全局资源（图标、主题图片）
   └─ Settings.settings             // 应用设置（可选，如窗口位置记忆）

 ##  DesktopBeatLight.Core/
├─ DesktopBeatLight.Core.csproj     // 项目配置（引用 MathNet.Numerics、System.Text.Json）
├─ Abstractions/                    // 核心接口（定义“做什么”，解耦实现）
│  ├─ IAudioAnalyzer.cs             // FFT 音频分析接口（输入音频→输出频谱）
│  ├─ IThemeRenderer.cs             // 主题渲染接口（输入频谱→绘制灯带）
│  └─ IConfigStorage.cs             // 配置存储接口（加载/保存主题配置）
├─ Models/                          // 业务实体（数据载体，无业务逻辑）
│  ├─ SpectrumData.cs               // 频谱数据实体（频段能量数组、频段数量）
│  ├─ ThemeConfig.cs                // 主题配置实体（当前主题、灯带高度、静音开关）
│  └─ ThemeType.cs                  // 枚举：主题类型（Fire/Ocean/Neon）
├─ Implementations/                 // 接口实现（定义“怎么做”，依赖第三方库）
│  ├─ FftAudioAnalyzer.cs           // FFT 分析实现（基于 MathNet.Numerics）
│  ├─ ThemeRenderers/               // 主题渲染实现（按主题分类）
│  │  ├─ FireThemeRenderer.cs       // 火焰主题渲染（红→橙→黄渐变）
│  │  ├─ OceanThemeRenderer.cs      // 海洋主题渲染（蓝→青→绿渐变）
│  │  └─ NeonThemeRenderer.cs       // 霓虹主题渲染（紫→粉→青亮色）
│  └─ JsonConfigStorage.cs          // 配置存储实现（基于 System.Text.Json 源生成）
├─ Helpers/                         // 核心辅助工具
│  ├─ FftHelper.cs                  // FFT 辅助（频段能量归一化、平滑处理）
│  └─ JsonSerializerContext.cs      // JSON 源生成器上下文（AOT 友好，无反射）
└─ Constants/                       // 全局常量
   ├─ FftConstants.cs               // FFT 常量（窗口大小=1024、频段数=64）
   └─ ThemeConstants.cs             // 主题常量（默认灯带高度=8px、静音阈值=-40dB）

##   DesktopBeatLight.Audio/
├─ DesktopBeatLight.Audio.csproj    // 项目配置（引用 NAudio、Core 层 Models）
├─ Abstractions/                    // 音频接口（解耦 NAudio 依赖）
│  └─ IAudioCapture.cs              // 音频捕获接口（启动/停止、数据事件、静音事件）
├─ Implementations/                 // 音频实现（封装 NAudio）
│  └─ WasapiAudioCapture.cs         // 基于 NAudio 的环回捕获（抓取扬声器输出）
├─ Helpers/                         // 音频辅助工具
│  ├─ AudioFormatHelper.cs          // 音频格式转换（字节→浮点、多声道→单声道）
│  └─ SilenceDetector.cs            // 静音检测（RMS 能量计算，阈值=-40dB）
└─ Models/                          // 音频实体（仅音频层使用，不跨层）
   └─ AudioCaptureConfig.cs         // 音频捕获配置（采样率=44100Hz、缓冲区大小=1024）


