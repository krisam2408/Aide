using Aide.Color;

namespace Aide.Charts.DataTransfer;

public abstract class BaseDataset
{
    public string[] Labels { get; set; }
    public IColor[] Colors { get; set; } = [
        new ColorHSB(0, 80, 100),
        new ColorHSB(30, 80, 100),
        new ColorHSB(60, 80, 100),
        new ColorHSB(90, 80, 100),
        new ColorHSB(120, 80, 100),
        new ColorHSB(150, 80, 100),
        new ColorHSB(180, 80, 100),
        new ColorHSB(210, 80, 100),
        new ColorHSB(240, 80, 100),
        new ColorHSB(270, 80, 100),
        new ColorHSB(300, 80, 100),
        new ColorHSB(330, 80, 100),
    ];

    public int Count => Labels.Length;

    protected BaseDataset(string[] labels, IColor[] colors)
    {
        Labels = labels;
        Colors = colors;
    }

    public BaseDataset(string[] labels)
    {
        Labels = labels; 
    }

    internal abstract void Validate();
}
