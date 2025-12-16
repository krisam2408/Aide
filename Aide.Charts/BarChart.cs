using Aide.Charts.DataTransfer;
using SkiaSharp;

namespace Aide.Charts;

public sealed class BarChart : BaseChart<SingleDataset>
{
    public BarChart(ChartOptions<SingleDataset> context) : base(context) { }

    public override async Task DrawChart()
    {
        using SKBitmap bmp = new(Context.Width, Context.Height);
        SKCanvas canvas = new(bmp);

        DrawLegends(canvas, out ChartBounds bounds);

        using SKPaint rectPaint = new();
        rectPaint.Color = ToSkiaColor(Context.Data.Colors[0]);
        rectPaint.IsAntialias = true;

        for (int i = 0; i < Context.DataCount; i++)
        {
            float left = bounds.GraphStart + i * (bounds.GraphWidth / Context.DataCount) + bounds.BarWidth * 0.15f;
            float top = bounds.GraphBottom - (float)Context.Data.Values[i] * bounds.ScaleY;
            float right = left + bounds.BarWidth;
            float bottom = bounds.GraphBottom - 1.7f;

            canvas.DrawRect(new SKRect(left, top, right, bottom), rectPaint);
        }

        await SaveChart(bmp, Context.FilePath);
    }

    protected override void DrawLabels(int i, ChartBounds bounds, SKFont font, SKPaint paint, SKCanvas canvas)
    {
        float left = bounds.GraphStart + i * (bounds.GraphWidth / Context.DataCount) + bounds.BarWidth * 0.5f;
        float bottom = bounds.GraphBottom + 20;

        canvas.DrawText(Context.Data.Labels[i], left, bottom, font, paint);
    }
}
