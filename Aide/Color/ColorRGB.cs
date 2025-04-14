namespace Aide.Color;

public class ColorRGB : IColor
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

    public byte[] Channels() => 
    [
        Red,
        Green,
        Blue,
        Alpha
    ];

    public string Hexcode()
    {
        throw new NotImplementedException();
    }

    public T ToColor<T>() where T : IColor
    {
        throw new NotImplementedException();
    }
}
