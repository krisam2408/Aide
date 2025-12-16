using Aide.Charts.DataTransfer;
using SkiaSharp;

namespace Aide.Charts;

public sealed class PieChart : BaseChart<SingleDataset>
{
    public PieChart(ChartOptions<SingleDataset> context) : base(context) { }

    public async Task DrawChart()
    {
        ChartBounds bounds = new(Context);

        using SKBitmap bmp = new(bounds.SquareRect, bounds.SquareRect);
        using SKCanvas canvas = new(bmp);

        canvas.Clear(ToSkiaColor(Context.BackgroundColor));

        float total = 0;

        foreach(double value in Context.Data.Values)
            total += (float)value;

        float startAngle = 0;

        using SKPaint paint = new();
        paint.IsAntialias = true;

        using SKPaint textPaint = new();
        textPaint.IsAntialias = true;
        textPaint.Color = SKColors.Black;

        using SKFont font = new(SKTypeface.Default);
        font.Size = 16;

        for(int i = 0; i < Context.Data.Count; i++)
        {
            float sweepAngle = (float)(Context.Data.Values[i] / total * 360f);
            paint.Color = ToSkiaColor(Context.Data.Colors[i]);
            canvas.DrawArc(new SKRect(bounds.GraphStart, bounds.GraphTop, bounds.SquareRect - Context.RightMargin, bounds.SquareRect - Context.BottomMargin), startAngle, sweepAngle, true, paint);

            float midAngle = startAngle + sweepAngle * 0.5f;
            float x = (float)(bounds.SquareRect * 0.5f + System.Math.Cos(midAngle * System.Math.PI / 180) * 100f);
            float y = (float)(bounds.SquareRect * 0.5f + System.Math.Sin(midAngle * System.Math.PI / 180) * 100f);
            canvas.DrawText(Context.Data.Labels[i], x, y, SKTextAlign.Center, font, textPaint);

            startAngle += sweepAngle;
        }

        await SaveChart(bmp, Context.FilePath);
    }

    protected override void DrawLabels(int i, ChartBounds bounds, SKFont font, SKPaint paint, SKCanvas canvas) { }
}
