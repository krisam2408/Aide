namespace Aide.Charts.DataTransfer;

public struct ChartBounds
{
    public float GraphWidth { get; set; } = 0f;
    public float GraphHeight { get; set; } = 0f;
    public float GraphStart { get; set; } = 0f;
    public float GraphEnd { get; set; } = 0f;
    public float GraphTop { get; set; } = 0f;
    public float GraphBottom { get; set; } = 0f;

    public float StepX { get; set; } = 0f;
    public float BarWidth { get; set; } = 0f;
    public float ScaleY { get; set; } = 0f;

    public int SquareRect { get; init; }

    public ChartBounds() { }

    public ChartBounds(ChartOptions options)
    {
        GraphWidth = options.Width - options.XMargin;
        GraphHeight = options.Height - options.YMargin;
        GraphStart = options.LeftMargin;
        GraphEnd = options.Width - options.RightMargin;
        GraphTop = options.TopMargin;
        GraphBottom = options.Height - options.BottomMargin;

        StepX = GraphWidth / (options.DataCount - 1);
        BarWidth = GraphWidth / options.DataCount * 0.7f;
        ScaleY = GraphHeight * 0.01f;

        SquareRect = System.Math.Max(options.Width, options.Height);
    }
}
