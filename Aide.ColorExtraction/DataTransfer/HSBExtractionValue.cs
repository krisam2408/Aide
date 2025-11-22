namespace Aide.ColorExtraction.DataTransfer;

public struct HSBExtractionValue
{
    public int Hue { get; set; }
    public int Saturation { get; set; }
    public int Brightness { get; set; }
    public string Hex { get; set; }

    public static HSBExtractionValue operator ++(HSBExtractionValue a)
    {
        a.Hue++;

        if(a.Hue == 360)
        {
            a.Hue = 0;
            a.Saturation--;
        }

        if(a.Saturation < 0)
        {
            a.Saturation = 100;
            a.Brightness--;
        }

        if (a.Brightness < 0)
            a.Brightness = 0;

        return a;
    }

    public readonly string ToTerminal() => $"{Hex} -> ({Hue} - {Saturation} - {Brightness})";
}
