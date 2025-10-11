using System.Drawing;
using System;

namespace DesktopBeatLight.Core.Models;

public class ThemeRenderParams
{
    public Graphics Graphics { get; set; }
    public Rectangle Bounds { get; set; }
    public float[] SpectrumData { get; set; }
    public ThemeConfig ThemeConfig { get; set; }=null!;
    public bool IsMuted { get; set; }
    public DateTime CurrentTime { get; set; }
}