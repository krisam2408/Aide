namespace Aide.Color;

public interface IColor
{
    float Alpha { get; set; }
    int Opacity { get; set; }

    string Hexcode();
    T ToColor<T>();
    int[] Channels();
}
