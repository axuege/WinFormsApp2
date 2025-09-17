##  DesktopBeatLight for .NET 8
## ��ĿĿ¼�ṹ˵��
 ��������ܹ������ֲ㣨UI�������Ĳ㣨Core������Ƶ�㣨Audio��
# �������������Ŀ¼
mkdir DesktopBeatLight
# �����Ŀ¼
cd DesktopBeatLight

# ������������ļ�
dotnet new sln -n DesktopBeatLight

# 1. ���� src Ŀ¼
mkdir src
cd src

# 2. ���� UI ��Ŀ��WinForms��
dotnet new winforms -n DesktopBeatLight.UI
# Ϊ UI ��Ŀ�������ļ���
mkdir DesktopBeatLight.UI\Controls
mkdir DesktopBeatLight.UI\Helpers
mkdir DesktopBeatLight.UI\Properties

# 3. ���� Core ��Ŀ����⣩
dotnet new classlib -n DesktopBeatLight.Core
# Ϊ Core ��Ŀ�������ļ���
mkdir DesktopBeatLight.Core\Abstractions
mkdir DesktopBeatLight.Core\Models
mkdir DesktopBeatLight.Core\Implementations
mkdir DesktopBeatLight.Core\Implementations\ThemeRenderers
mkdir DesktopBeatLight.Core\Helpers
mkdir DesktopBeatLight.Core\Constants

# 4. ���� Audio ��Ŀ����⣩
dotnet new classlib -n DesktopBeatLight.Audio
# Ϊ Audio ��Ŀ�������ļ���
mkdir DesktopBeatLight.Audio\Abstractions
mkdir DesktopBeatLight.Audio\Implementations
mkdir DesktopBeatLight.Audio\Helpers
mkdir DesktopBeatLight.Audio\Models

# �ص���Ŀ¼
cd ..

# 1. ���� tests Ŀ¼
mkdir tests
cd tests

# 2. ���� Core ������Ŀ
dotnet new xunit -n DesktopBeatLight.Core.Tests
# ��Ӷ� Core ��Ŀ�����ã���ȷ��·����ȷ���ص� src ȡ Core ��Ŀ��
dotnet add DesktopBeatLight.Core.Tests\DesktopBeatLight.Core.Tests.csproj reference ..\src\DesktopBeatLight.Core\DesktopBeatLight.Core.csproj

# 3. ���� Audio ������Ŀ
dotnet new xunit -n DesktopBeatLight.Audio.Tests
# ��Ӷ� Audio ��Ŀ������
dotnet add DesktopBeatLight.Audio.Tests\DesktopBeatLight.Audio.Tests.csproj reference ..\src\DesktopBeatLight.Audio\DesktopBeatLight.Audio.csproj

# �ص���Ŀ¼
cd ..

# ������Ŀ��Project����ӵ����������DesktopBeatLight.sln����
# ��� src �µ���Ŀ
dotnet sln add src\DesktopBeatLight.UI\DesktopBeatLight.UI.csproj
dotnet sln add src\DesktopBeatLight.Core\DesktopBeatLight.Core.csproj
dotnet sln add src\DesktopBeatLight.Audio\DesktopBeatLight.Audio.csproj

# ��� tests �µ���Ŀ
dotnet sln add tests\DesktopBeatLight.Core.Tests\DesktopBeatLight.Core.Tests.csproj
dotnet sln add tests\DesktopBeatLight.Audio.Tests\DesktopBeatLight.Audio.Tests.csproj


## DesktopBeatLight.sln                // ��������ļ�
���� .github/
��  ���� workflows/
��     ���� ci.yml                     // GitHub Actions �Զ�������/������ˮ��
���� src/
��  ���� DesktopBeatLight.UI/          // ���ֲ㣨WinForms ����Ӧ�ã�
��  ���� DesktopBeatLight.Core/        // ���Ĳ㣨ҵ���߼���ʵ�塢�ӿڣ�
��  ���� DesktopBeatLight.Audio/       // ��Ƶ�㣨��Ƶ���񡢾�����⣩
���� tests/                           // ���Բ㣨��Ԫ���ԡ����ܲ��ԣ�
��  ���� DesktopBeatLight.Core.Tests/
��  ���� DesktopBeatLight.Audio.Tests/
���� publish.bat                      // һ�� NativeAOT �����ű�
���� README.md                        // ��Ŀ˵���ĵ�

DesktopBeatLight.UI/
���� DesktopBeatLight.UI.csproj       // ��Ŀ���ã����� Core/Audio �㡢���� AOT��
���� Program.cs                       // Ӧ����ڣ�����ע��ע�ᡢ���������ڣ�
���� MainForm.cs                      // �����ڣ����ر߿��ö����󶨵ƴ��ؼ���
���� MainForm.Designer.cs             // ��������������루�Զ����ɣ�
���� MainForm.resx                    // ��������Դ��ͼ�ꡢ�ַ�����
���� Controls/                        // �Զ���ؼ��ļ���
��  ���� LightStripControl.cs          // ���Ŀؼ�������ײ��ƴ�����Ƶ��������Ⱦ��
��  ���� LightStripControl.Designer.cs // �ƴ��ؼ����������ѡ���ֶ�д���ֿ�ʡ�ԣ�
��  ���� ThemeComboBox.cs              // ����ѡ�������򣨰� Core �������б�
���� Helpers/                         // UI ��������
��  ���� DpiHelper.cs                  // DPI ���乤�ߣ�����ؼ����ű�����
��  ���� GraphicsHelper.cs             // ��ͼ������Բ�Ǿ��Ρ����仭�ʷ�װ��
���� Properties/                      // Ӧ������
   ���� AssemblyInfo.cs               // ������Ϣ���汾�����ߣ�
   ���� Resources.resx                // ȫ����Դ��ͼ�ꡢ����ͼƬ��
   ���� Settings.settings             // Ӧ�����ã���ѡ���細��λ�ü��䣩

 ##  DesktopBeatLight.Core/
���� DesktopBeatLight.Core.csproj     // ��Ŀ���ã����� MathNet.Numerics��System.Text.Json��
���� Abstractions/                    // ���Ľӿڣ����塰��ʲô��������ʵ�֣�
��  ���� IAudioAnalyzer.cs             // FFT ��Ƶ�����ӿڣ�������Ƶ�����Ƶ�ף�
��  ���� IThemeRenderer.cs             // ������Ⱦ�ӿڣ�����Ƶ�ס����Ƶƴ���
��  ���� IConfigStorage.cs             // ���ô洢�ӿڣ�����/�����������ã�
���� Models/                          // ҵ��ʵ�壨�������壬��ҵ���߼���
��  ���� SpectrumData.cs               // Ƶ������ʵ�壨Ƶ���������顢Ƶ��������
��  ���� ThemeConfig.cs                // ��������ʵ�壨��ǰ���⡢�ƴ��߶ȡ��������أ�
��  ���� ThemeType.cs                  // ö�٣��������ͣ�Fire/Ocean/Neon��
���� Implementations/                 // �ӿ�ʵ�֣����塰��ô�����������������⣩
��  ���� FftAudioAnalyzer.cs           // FFT ����ʵ�֣����� MathNet.Numerics��
��  ���� ThemeRenderers/               // ������Ⱦʵ�֣���������ࣩ
��  ��  ���� FireThemeRenderer.cs       // ����������Ⱦ������ȡ��ƽ��䣩
��  ��  ���� OceanThemeRenderer.cs      // ����������Ⱦ����������̽��䣩
��  ��  ���� NeonThemeRenderer.cs       // �޺�������Ⱦ���ϡ��ۡ�����ɫ��
��  ���� JsonConfigStorage.cs          // ���ô洢ʵ�֣����� System.Text.Json Դ���ɣ�
���� Helpers/                         // ���ĸ�������
��  ���� FftHelper.cs                  // FFT ������Ƶ��������һ����ƽ������
��  ���� JsonSerializerContext.cs      // JSON Դ�����������ģ�AOT �Ѻã��޷��䣩
���� Constants/                       // ȫ�ֳ���
   ���� FftConstants.cs               // FFT ���������ڴ�С=1024��Ƶ����=64��
   ���� ThemeConstants.cs             // ���ⳣ����Ĭ�ϵƴ��߶�=8px��������ֵ=-40dB��

##   DesktopBeatLight.Audio/
���� DesktopBeatLight.Audio.csproj    // ��Ŀ���ã����� NAudio��Core �� Models��
���� Abstractions/                    // ��Ƶ�ӿڣ����� NAudio ������
��  ���� IAudioCapture.cs              // ��Ƶ����ӿڣ�����/ֹͣ�������¼��������¼���
���� Implementations/                 // ��Ƶʵ�֣���װ NAudio��
��  ���� WasapiAudioCapture.cs         // ���� NAudio �Ļ��ز���ץȡ�����������
���� Helpers/                         // ��Ƶ��������
��  ���� AudioFormatHelper.cs          // ��Ƶ��ʽת�����ֽڡ����㡢����������������
��  ���� SilenceDetector.cs            // ������⣨RMS �������㣬��ֵ=-40dB��
���� Models/                          // ��Ƶʵ�壨����Ƶ��ʹ�ã�����㣩
   ���� AudioCaptureConfig.cs         // ��Ƶ�������ã�������=44100Hz����������С=1024��


