namespace DesktopBeatLight.Core.SinalProcessing;

public static class FftConstants
{
    // 1. FFT�㷨�������ã�Ӱ��������Ⱥ����ܣ�
    /// <summary>
    /// FFT����Ĳ�����������������2���ݣ���256��512��1024��
    /// ѡ��1024����˷������ȣ��ܸ���20Hz-20kHz��Ƶ��Χ�������ܣ���CPUռ�ã�
    /// </summary>
    public const int fftSize = 1024; //FFT��С

    /// <summary>
    /// ��Ƶ�����ʣ���WASAPI����Ĳ����ʱ���һ�£�Ĭ��44100Hz����Ƶ��׼�����ʣ�
    /// �������޸�WASAPI�����ʣ���ͬ�����´�ֵ
    /// </summary>
    public const int sampleRate = 44100; //������
    /// <summary>
    /// FFT�������Ч���ݳ��ȣ�FFT����Գƣ���ǰ�����Ч��
    /// ���㹫ʽ��FftSize / 2��1024/2=512����Ч�㣩
    /// </summary>
    public static readonly int fftResultLength = fftSize / 2;

    // 2. Ƶ�״������ã�Ӱ��ƴ���ȾЧ����
    /// <summary>
    /// ����������ƴ���Ƶ��Ƶ�����������������Ŀ�������
    /// ѡ��64��ƽ���Ӿ�ϸ�ڣ�����̫ϡ�裩����Ⱦ���ܣ�����̫�ܼ���
    /// ��Ӧ֮ǰFftAnalyzer��DownsampleTo64Points����
    /// </summary>
    public const int TargetSpectrumPointCount = 64;

    /// <summary>
    /// Ƶ�׷��ȷŴ�ϵ���������ƴ����εĸ߶������ȣ�
    /// 5���Ŵ󣺱���ԭʼƵ��ֵ��С���µƴ������Ա仯����Ӧ���������heightRatio���㣩
    /// �����õƴ�̫��/̫�ߣ��ɵ�����ֵ����3f��7f��
    /// </summary>
    public const float SpectrumAmplificationFactor = 5f;

    /// <summary>
    /// Ƶ�׷��ȵ�������ƣ����⼫��ֵ���µƴ��������ڷ�Χ��
    /// 1f��Ӧ100%���ڸ߶ȣ�����Ⱦʱ��Math.Min(..., 1f)���ʹ��
    /// </summary>
    public const float MaxSpectrumAmplitude = 1f;

    // ����ģ�����ã��뾲����⡢��Ⱦ��ʱ��������
    /// <summary>
    /// ��Ⱦ֡�ʶ�Ӧ��֡��������룩
    /// 50FPS = 1000ms / 50 = 20ms����֮ǰPeriodicTimer��20ms���һ��
    /// </summary>
    public const int RenderFrameIntervalMs = 20;

    /// <summary>
    /// ������������֡��ֵ������������֡����
    /// �����߼���2�� / 20msÿ֡ = 100֡����֮ǰSilenceDetector��100֡�ж�һ��
    /// </summary>
    public const int SilenceTriggerFrameCount = 100;

    /// <summary>
    /// DPI����Ļ�׼ֵ��WindowsĬ��DPIΪ96��
    /// ����DpiHelper����ؼ����ű�������ScaleByDpi������
    /// </summary>
    public const int BaseDpi = 96;
    
}