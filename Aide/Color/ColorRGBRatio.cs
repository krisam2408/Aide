namespace Aide.Color;

public sealed class ColorRGBRatio : IColor
{
    private float m_red;
    public float Red { get => m_red; set => m_red = value.ClampToOne(); }

    private float m_green;
    public float Green { get => m_green; set => m_green = value.ClampToOne(); }

    private float m_blue;
    public float Blue { get => m_blue; set => m_blue = value.ClampToOne(); }

    private float m_opacity;
    public float Opacity { get => m_opacity; set => m_opacity = value.ClampToOne(); }

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

    public ColorRGBRatio(float red, float green, float blue, float opacity)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Opacity = opacity;
    }

    public ColorRGBRatio(float red, float green, float blue) : this(red, green, blue, 1f) { }

    public ColorRGBRatio() : this(1f, 1f, 1f) { }

    private static byte ToByte(float value) => (byte)(value * 255)
        .Round()
        .Clamp(0,255);

    public byte[] RGBChannels() =>
    [
        ToByte(Red),
        ToByte(Green),
        ToByte(Blue)
    ];

    public byte[] ARGBChannels() =>
    [
        ToByte(Alpha),
        ToByte(Red),
        ToByte(Green),
        ToByte(Blue)
    ];

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
