using Aide.Color;

namespace Aide.Charts.DataTransfer;

public abstract class ChartOptions
{
    private readonly string m_filepath;
    public string FilePath => m_filepath;

    public int Width { get; set; } = 900;
    public int Height { get; set; } = 600;

    public float LeftMargin { get; set; } = 100;
    public float TopMargin { get; set; } = 100;
    public float RightMargin { get; set; } = 100;
    public float BottomMargin { get; set; } = 100;

    public float XMargin => LeftMargin + RightMargin;
    public float YMargin => TopMargin + BottomMargin;

    public string Title { get; set; } = "";
    public string XLabel { get; set; } = "";
    public string YLabel { get; set; } = "";

    public IColor BackgroundColor { get; set; } = new ColorRatio(1, 1, 1);
    public IColor FontColor { get; set; } = new ColorRatio(0.15, 0.15, 0.15);
    
    public bool AllowTransparency { get; private set; } = false;

    public abstract int DataCount { get; }

    protected ChartOptions(string filepath)
    {
        m_filepath = filepath;
    }

    internal virtual void Validate()
    {
        if ((Width - XMargin) <= 0)
            throw new ArgumentOutOfRangeException(nameof(Width), "Width needs to be larger");

        if ((Height - YMargin) <= 0)
            throw new ArgumentOutOfRangeException(nameof(Height), "Height needs to be larger");

        if (BackgroundColor.Opacity < 1)
            AllowTransparency = true;
    }
}

public sealed class ChartOptions<T> : ChartOptions where T : BaseDataset
{
    public T Data { get; init; }
    public override int DataCount => Data.Count;

    public ChartOptions(string filepath, T data) : base(filepath)
    {
        Data = data;
    }

    internal override void Validate()
    {
        base.Validate();

        Data.Validate();
    }
}