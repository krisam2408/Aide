namespace Aide.Color;

public sealed class ColorRGB : IColor
{
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public byte Alpha { get; set; }

    public double Opacity 
    {
        get => Alpha / 255f;
        set 
        {
            double v = value.Clamp();
            v *= 255;
            Alpha = (byte)v
                .Round()
                .Clamp(0,255);
        }
    }

    public ColorRGB(byte red, byte green, byte blue, byte alpha)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public ColorRGB(byte red, byte green, byte blue, float opacity) : this(red, green, blue, (byte)(opacity * 255)) { }

    public ColorRGB(byte red, byte green, byte blue) : this(red, green, blue, 255) { }

    public ColorRGB() : this(255, 255, 255) { }

    public byte[] RGBChannels() => 
    [
        Red,
        Green,
        Blue
    ];

    public byte[] ARGBChannels() =>
    [
        Alpha,
        Red,
        Green,
        Blue
    ];

    public string Hexcode() => $"#{Red:X2}{Green:X2}{Blue:X2}";

    public static explicit operator ColorRGB(ColorRGBRatio color)
    {
        return new()
        {
            Red = (byte)Math.Floor(color.Red * 255),
            Green = (byte)Math.Floor(color.Green * 255),
            Blue = (byte)Math.Floor(color.Blue * 255),
            Alpha = color.Alpha
        };
    }

    public static explicit operator ColorRGB(ColorHSB color)
    {
        ColorRGBRatio rgb = (ColorRGBRatio)color;
        return (ColorRGB)rgb;
    }
}
