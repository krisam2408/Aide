using Aide.Color;
using SkiaSharp;

namespace Aide.ColorExtraction.DataTransfer;

public struct HSBComparisonValue
{
    public int Hue { get; set; }
    public int Saturation { get; set; }
    public int Brightness { get; set; }
    public string AdobeHex { get; set; }
    public int AdobeRed { get; set; }
    public int AdobeGreen { get; set; }
    public int AdobeBlue { get; set; }
    public string AideHex { get; set; }
    public int AideRed { get; set; }
    public int AideGreen { get; set; }
    public int AideBlue { get; set; }
    public readonly string DeltaRed => Delta(AideRed, AdobeRed);
    public readonly string DeltaGreen => Delta(AideGreen, AdobeGreen);
    public readonly string DeltaBlue => Delta(AideBlue, AdobeBlue);

    public HSBComparisonValue(HSBExtractionValue value)
    {
        HexCode hex = new(value.Hex);

        ColorHSB aideColor = new(value.Hue, value.Saturation, value.Brightness);
        HexCode aideHex = new(aideColor.Hexcode());

        Hue = value.Hue;
        Saturation = value.Saturation;
        Brightness = value.Brightness;
        AdobeHex = value.Hex;
        AdobeRed = hex.Red;
        AdobeGreen = hex.Green;
        AdobeBlue = hex.Blue;
        AideHex = aideHex.Code;
        AideRed = aideHex.Red;
        AideGreen = aideHex.Green;
        AideBlue = aideHex.Blue;
    }

    private static string Delta(int adobe, int aide)
    {
        int result = adobe - aide;
        if(result < 1)
            return result.ToString();
        return $"+{result}";
    }
}
