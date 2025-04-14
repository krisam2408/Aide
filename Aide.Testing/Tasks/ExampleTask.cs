using System.Drawing;
using System.Runtime.Versioning;
using TerminalWrapper;

namespace Aide.Testing.Tasks;

[SupportedOSPlatform("windows")]
internal sealed class ExampleTask : MainTask
{
    public override string TaskName => "Graphic Example Task";

    private readonly string m_output;

    public ExampleTask(string output)
    {
        m_output = output;
    }

    public override Task ExecuteAsync(CancellationToken cancelToken)
    {
        // Sample data
        int[] data = { 10, 20, 30, 40, 50 };

        // Image dimensions
        int width = 500;
        int height = 300;

        using (Bitmap bitmap = new(width, height)) // Create a new bitmap
        using (Graphics g = Graphics.FromImage(bitmap)) // Create graphics object
        {
            // Clear the background
            g.Clear(System.Drawing.Color.White);

            // Define bar properties
            int barWidth = 40;
            int barSpacing = 10;
            int maxBarHeight = height - 20; // Leave some space for margins

            // Find the maximum value in the data
            int maxValue = 0;
            foreach (int value in data)
            {
                if (value > maxValue)
                    maxValue = value;
            }

            // Draw bars
            for (int i = 0; i < data.Length; i++)
            {
                int barHeight = (int)((data[i] / (float)maxValue) * maxBarHeight);
                int x = i * (barWidth + barSpacing) + barSpacing;
                int y = height - barHeight - 10; // Leave some space at the bottom

                g.FillRectangle(Brushes.Blue, x, y, barWidth, barHeight);
            }

            // Save the image
            bitmap.Save($"{m_output}barchart.png");

            return Task.CompletedTask;
        }       
    }
}