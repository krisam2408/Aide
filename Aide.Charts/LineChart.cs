using Aide.Charts.DataTransfer;
using SkiaSharp;

namespace Aide.Charts;

public sealed class LineChart : BaseChart<MultipleDataset>
{
    public LineChart(ChartOptions<MultipleDataset> context) : base(context) { }

    public async Task DrawChart()
    {
        using SKBitmap bmp = new(Context.Width, Context.Height);
        using SKCanvas canvas = new(bmp);

        DrawLegends(canvas, out ChartBounds bounds);

        int linesLen = Context.Data.Values.Count;
        for(int i = 0; i < linesLen; i++) 
        {
            using SKPaint linePaint = new();
            linePaint.Color = ToSkiaColor(Context.Data.Colors[i]);
            linePaint.StrokeWidth = 4;
            linePaint.IsAntialias = true;
            linePaint.StrokeCap = SKStrokeCap.Round;

            using SKPaint blackPaint = new();
            blackPaint.Color = SKColors.Black;
            blackPaint.StrokeWidth = 2;
            blackPaint.IsAntialias = true;

            for (int j = 0; j < Context.Data.Count - 1; j++)
            {
                float x1 = Context.LeftMargin + j * bounds.StepX;
                float y1 = bounds.GraphBottom - (float)Context.Data.Values[i][j] * bounds.ScaleY;
                float x2 = Context.LeftMargin + (j + 1) * bounds.StepX;
                float y2 = bounds.GraphBottom - (float)Context.Data.Values[i][j + 1] * bounds.ScaleY;

                canvas.DrawLine(x1, y1, x2, y2, linePaint);
                canvas.DrawCircle(x1, y1, 8, blackPaint);
                canvas.DrawCircle(x1, y1, 6, linePaint);

                if(j == Context.Data.Count - 2)
                {
                    canvas.DrawCircle(x2, y2, 8, blackPaint);
                    canvas.DrawCircle(x2, y2, 6, linePaint);
                }
            }
        }

        await SaveChart(bmp, Context.FilePath);
    }

    protected override void DrawLabels(int i, ChartBounds bounds, SKFont font, SKPaint paint, SKCanvas canvas)
    {
        float x = bounds.GraphStart + i * bounds.StepX;
        float y = bounds.GraphBottom + 20;

        string label = Context.Data.Labels[i];
        font.MeasureText(label, out SKRect textBounds);

        canvas.DrawText(label, x - textBounds.MidX, y, SKTextAlign.Left, font, paint);
    }
}
