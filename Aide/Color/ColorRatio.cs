namespace Aide.Color;

public sealed class ColorRatio : IColor
{
    public double Red 
    { 
        get => field; 
        set => field = value.Clamp(); 
    }

    public double Green 
    { 
        get => field; 
        set => field = value.Clamp(); 
    }

    public double Blue 
    { 
        get => field; 
        set => field = value.Clamp(); 
    }

    public double Opacity 
    { 
        get => field; 
        set => field = value.Clamp(); 
    }

    public byte Alpha 
    {
        get 
        {
            double op = Opacity * 255;
            return (byte)op.Clamp(0, 255);
        }
        set => Opacity = value / 255.0;
    }

    public ColorRatio(double red, double green, double blue, double opacity)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Opacity = opacity;
    }

    public ColorRatio(double red, double green, double blue) : this(red, green, blue, 1) { }

    public ColorRatio() : this(1f, 1f, 1f) { }

    private static byte ToByte(double value) => (byte)(value * 255)
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
        ColorRGB rgb = (ColorRGB)this;
        return rgb.Hexcode();
    }

    public static explicit operator ColorRatio(ColorRGB color)
    {
        return new()
        {
            Red = color.Red / 255.0,
            Green = color.Green / 255.0,
            Blue = color.Blue / 255.0,
            Alpha = color.Alpha
        };
    }

    public static explicit operator ColorRatio(ColorHSB color)
    {
        double[] channels = [ 0, 0, 0 ];

        int sectorPos = color.Hue / 120;
        int spectrum = color.Hue - sectorPos * 120;

        double[] x = 
        [
            1,
            spectrum / 120.0
        ];

        int[] channelIndexes = HandleChannelIndexes(sectorPos, x);

        if (x[1] > 0.5)
            x[1] = 1 - x[1];

        channels[channelIndexes[0]] = x[0];
        channels[channelIndexes[1]] = x[1] * 2.0;

        double brgCoef = color.Brightness * 0.01;
        double satCoef = (100.0 - color.Saturation) * 0.01;

        for (int i = 0; i < 3; i++)
            channels[i] += satCoef;

        //double brgCoef = color.Brightness / 100.0;

        //channel[0] = channel[0] * brgCoef;
        //channel[1] = channel[1] * brgCoef;
        //channel[2] = channel[2] * brgCoef;

        //int brgByte = 255 * color.Brightness / 100;
        //int satByte = 255 * (100 - color.Saturation) / 100;
        
        //if (satByte > brgByte) 
        //    satByte = brgByte;

        //for (byte i = 0; i < 3; i++)
        //{
        //    if (channel[i] < satByte) channel[i] = satByte;
        //}

        return new()
        {
            Red = channels[0],
            Green = channels[1],
            Blue = channels[2],
            Opacity = color.Opacity
        };
    }

    private static int[] HandleChannelIndexes(int sectorPos, double[] x)
    {
        int channel(int value)
        {
            if(value > 2)
                return 0;
                
            if(value < 0)
                return 2;

            return value;
        }

        int main = sectorPos;

        if (x[1] > 0.5)
            main = channel(main + 1);

        int direction()
        {
            if (x[1] > 0.5)
                return channel(main - 1);
            return channel(main + 1);
        }

        int next = direction();

        return [ main, next ];
    }
}
