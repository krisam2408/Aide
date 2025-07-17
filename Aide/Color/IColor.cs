namespace Aide.Color;

public interface IColor
{
    byte Alpha { get; set; }
    float Opacity { get; set; }

    string Hexcode();
    T ToColor<T>() where T : IColor;
    byte[] RGBChannels();
    byte[] ARGBChannels();
}
