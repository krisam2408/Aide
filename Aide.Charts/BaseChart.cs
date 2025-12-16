using Aide.Charts.DataTransfer;
using Aide.Color;
using SkiaSharp;

namespace Aide.Charts;

public abstract class BaseChart<T> where T : BaseDataset
{
    private readonly ChartOptions<T> m_context;
    protected ChartOptions<T> Context => m_context;

    protected BaseChart(ChartOptions<T> context)
    {
        context.Validate();
        m_context = context;
    }

    public abstract Task DrawChart();

    protected void DrawLegends(SKCanvas canvas, out ChartBounds bounds)
    {
        canvas.Clear(ToSkiaColor(Context.BackgroundColor));

        using SKPaint blackPaint = new();
        blackPaint.Color = SKColors.Black;
        blackPaint.StrokeWidth = 3;
        blackPaint.IsAntialias = true;
        blackPaint.StrokeCap = SKStrokeCap.Square;

        bounds = new(Context);

        canvas.DrawLine(bounds.GraphStart, bounds.GraphBottom, bounds.GraphEnd, bounds.GraphBottom, blackPaint);
        canvas.DrawLine(bounds.GraphStart, bounds.GraphTop, bounds.GraphStart, bounds.GraphBottom, blackPaint);


        using SKFont font = new(SKTypeface.Default);

        int numTicks = 5;
        float tickSpacing = 100f / numTicks;

        for (int i = 0; i <= numTicks; i++)
        {
            float yValue = i * tickSpacing;
            float yPos = bounds.GraphBottom - (yValue * bounds.ScaleY);

            string valueText = yValue.ToString();
            font.MeasureText(valueText, out SKRect textBounds);

            canvas.DrawText(valueText, bounds.GraphStart - 15, yPos - textBounds.MidY, SKTextAlign.Right, font, blackPaint);
            canvas.DrawLine(bounds.GraphStart - 5, yPos, bounds.GraphStart, yPos, blackPaint);
        }

        for (int i = 0; i < Context.Data.Count; i++)
            DrawLabels(i, bounds, font, blackPaint, canvas);

        font.Size = 24;

        if (!string.IsNullOrWhiteSpace(Context.Title))
        {
            font.MeasureText(Context.Title, out SKRect textBounds);

            canvas.DrawText(Context.Title, Context.Width * 0.5f, Context.TopMargin * 0.5f - textBounds.MidY, SKTextAlign.Center, font, blackPaint);
        }

        if (!string.IsNullOrWhiteSpace(Context.XLabel))
        {
            font.MeasureText(Context.XLabel, out SKRect textBounds);

            canvas.DrawText(Context.XLabel, Context.Width * 0.5f, Context.Height - Context.BottomMargin * 0.5f - textBounds.MidY, SKTextAlign.Center, font, blackPaint);
        }

        if (!string.IsNullOrEmpty(Context.YLabel))
        {
            canvas.Save();
            canvas.Translate(Context.LeftMargin * 0.5f, Context.Height * 0.5f);
            canvas.RotateDegrees(-90);

            canvas.DrawText(Context.YLabel, 0, 0, SKTextAlign.Center, font, blackPaint);

            canvas.Restore();
        }
    }

    protected abstract void DrawLabels(int i, ChartBounds bounds, SKFont font, SKPaint paint, SKCanvas canvas);

    protected async Task SaveChart(SKBitmap bmp, string filepath)
    {
        SKEncodedImageFormat setFormat()
        {
            if(m_context.AllowTransparency)
                return SKEncodedImageFormat.Png;
            return SKEncodedImageFormat.Jpeg;
        }

        using SKImage img = SKImage.FromBitmap(bmp);
        using SKData data = img.Encode(setFormat(), 100);

        byte[] buffer = data.ToArray();
        await File.WriteAllBytesAsync(filepath, buffer);
    }

    protected static SKColor ToSkiaColor(IColor color)
    {
        byte[] argb = color.ARGBChannels();
        SKColor sk = new(argb[1], argb[2], argb[3], argb[0]);
        return sk;
    }
}
