namespace Aide.Color;

public interface IColor
{
    byte Alpha { get; set; }
    double Opacity { get; set; }

    string Hexcode();
    byte[] RGBChannels();
    byte[] ARGBChannels();
}
