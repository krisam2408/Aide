namespace Aide.Color;

public sealed class ColorHSB : IColor
{
    private int m_hue;
    public int Hue
    {
        get => m_hue;
        set => m_hue = value.Clamp(0, 359);
    }

    private int m_saturation;
    public int Saturation
    {
        get => m_saturation;
        set => m_saturation = value.Clamp(0, 100);
    }

    private int m_brightness;
    public int Brightness
    {
        get => m_brightness;
        set => m_brightness = value.Clamp(0, 100);
    }

    private float m_opacity;
    public float Opacity 
    { 
        get => m_opacity; 
        set => m_opacity = value.ClampToOne(); 
    }

    public byte Alpha
    {
        get
        {
            float op = m_opacity * 255;
            return (byte)op
                .Clamp(0, 255);
        }
        set => m_opacity = value / 255f;
    }

    public byte[] RGBChannels()
    {
        ColorRGB rgb = ToColor<ColorRGB>();
        return rgb.RGBChannels();
    }

    public byte[] ARGBChannels()
    {
        ColorRGB rgb = ToColor<ColorRGB>();
        return rgb.ARGBChannels();
    }

    public string Hexcode()
    {
        ColorRGB rgb = ToColor<ColorRGB>();
        return rgb.Hexcode();
    }

    public T ToColor<T>() where T : IColor
    {
        throw new NotImplementedException();
    }
}
