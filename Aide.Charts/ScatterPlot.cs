using Aide.Charts.DataTransfer;
using SkiaSharp;

namespace Aide.Charts;

public sealed class ScatterPlot : BaseChart<VectorDataset>
{
    public ScatterPlot(ChartOptions<VectorDataset> context) : base(context)
    {
    }

    public override void DrawChart()
    {
        throw new NotImplementedException();
    }

    protected override void DrawLabels(int i, ChartBounds bounds, SKFont font, SKPaint paint, SKCanvas canvas)
    {
        throw new NotImplementedException();
    }
}
