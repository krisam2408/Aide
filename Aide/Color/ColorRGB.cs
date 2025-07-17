using System.Drawing;

namespace Aide.Color;

public sealed class ColorRGB : IColor
{
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public byte Alpha { get; set; }

    public float Opacity 
    {
        get => Alpha / 255f;
        set 
        {
            float v = value.ClampToOne();
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

    public T ToColor<T>() where T : IColor
    {
        throw new NotImplementedException();
    }
}
