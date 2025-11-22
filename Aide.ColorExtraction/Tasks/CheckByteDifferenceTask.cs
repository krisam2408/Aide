using Aide.Color;
using SkiaSharp;
using TerminalWrapper;

namespace Aide.ColorExtraction.Tasks;

public sealed class CheckByteDifferenceTask : MainTask
{
    public override string TaskName => "Check Byte Difference";

    private readonly string m_baseDirectory;

    public CheckByteDifferenceTask(string path)
    {
        m_baseDirectory = path; 
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        int len = 40;
        List<ColorRGB> colors = [];
        for (byte i = 0; i < len; i++)
            colors.Add(new(255, i, 0));

        await Terminal.WriteAsync("Drawing Image Results");

        using (SKBitmap bmp = new(200, len * 100))
        using (SKCanvas g = new(bmp))
        {
            foreach (ColorRGB color in colors)
            {
                DrawRectangles(g, color);
            }

            using (SKImage image = SKImage.FromBitmap(bmp))
            using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (Stream fs = File.OpenWrite($"{m_baseDirectory}colorByteDiff.png"))
            {
                data.SaveTo(fs);
            }
        }
    }

    private static void DrawRectangles(SKCanvas g, ColorRGB color)
    {
        SKColor initColor = SKColor.Parse("#FF0000");
        SKPaint initBrush = new() { Color = initColor };

        SKColor currColor = SKColor.Parse(color.Hexcode());
        SKPaint currBrush = new() { Color = currColor, };

        SKPaint textColor = new() { Color = SKColors.Black };
        SKFont textFont = new(SKTypeface.Default, 36);

        float height = color.Green * 100f;

        g.DrawRect(0f, height, 100f, 100f, initBrush);
        g.DrawRect(100f, height, 100f, 100f, currBrush);

        g.DrawText(color.Green.ToString(), 25f, height + 50f, textFont, textColor);
    }
}
