namespace Aide.Color;

public sealed class ColorHSB : IColor
{
    public int Hue
    {
        get => field;
        set => field = value.Clamp(0, 359);
    }

    public int Saturation
    {
        get => field;
        set => field = value.Clamp(0, 100);
    }

    public int Brightness
    {
        get => field;
        set => field = value.Clamp(0, 100);
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
            return (byte)op
                .Clamp(0, 255);
        }
        set => Opacity = value / 255.0;
    }

    public ColorHSB(int hue, int saturation, int brightness, double opacity)
    {
        Hue = hue;
        Saturation = saturation;
        Brightness = brightness;
        Opacity = opacity;
    }

    public ColorHSB(int hue, int saturation, int brightness) : this(hue, saturation, brightness, 1) {}

    public byte[] RGBChannels()
    {
        ColorRGB rgb = (ColorRGB)this;
        return rgb.RGBChannels();
    }

    public byte[] ARGBChannels()
    {
        ColorRGB rgb = (ColorRGB)this;
        return rgb.ARGBChannels();
    }

    public string Hexcode()
    {
        ColorRGB rgb = (ColorRGB)this;
        return rgb.Hexcode();
    }

    public static explicit operator ColorHSB(ColorRGB color)
    {
        ColorRGBRatio rgb = (ColorRGBRatio)color;
        return (ColorHSB)rgb;
    }

    public static explicit operator ColorHSB(ColorRGBRatio color)
    {
        throw new NotImplementedException();
        //double[] channels = { color.Red, color.Green, color.Blue };
        //int[] priority = { 0, 0, 0 };
        //double channelMax = 0;
        //double channelMin = 255;

        //for (int i = 0; i < channels.Length; i++)
        //{
        //    if (channels[i] > channelMax)
        //    {
        //        channelMax = channels[i];
        //        priority[0] = i;
        //    }
        //    if (channels[i] < channelMin)
        //    {
        //        channelMin = channels[i];
        //        priority[2] = i;
        //    }
        //}

        //for (int i = 0; i < priority.Length; i++)
        //    if (i != priority[0] && i != priority[2])
        //    {
        //        priority[1] = i;
        //        break;
        //    }

        //double relation = 0;
        //if (channels[priority[0]] > 0)
        //    relation = (double)channels[priority[1]] / (double)channels[priority[0]];

        //int rangePole = 1;
        //if (priority[0] == 0 && priority[1] == 2)
        //{
        //    rangePole = -1;
        //    priority[0] = 3;
        //}

        //double brightnessFormula = (double)channelMax / 255.0;
        //double saturationFormula = 100.0 - ((double)channelMin / 255.0);
        //double hueFormula = (double)priority[0] * 120.0 + (double)relation * 60.0 * (double)rangePole;

        //int brightness = Math.Round(brightnessFormula);
        //int saturation = Math.Round(saturationFormula);
        //int hue = Math.Round(hueFormula);

        //return new ColorHSB(hue, saturation, brightness, color.Opacity);
    }
}
