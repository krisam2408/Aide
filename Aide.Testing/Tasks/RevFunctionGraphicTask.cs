using System.Drawing;
using System.Runtime.Versioning;
using TerminalWrapper;

namespace Aide.Testing.Tasks;

[SupportedOSPlatform("windows")]
internal class RevFunctionGraphicTask : MainTask
{
    public override string TaskName => "Try Graphics 288-0";

    private readonly string m_output;
    private const string m_folder = "03";
    private const int m_xMargin = 32;
    private const int m_yMargin = 112;
    private const int m_size = 512;
    private const int m_startValue = 288;
    private const int m_endValue = 0;
    private readonly System.Drawing.Color m_background = System.Drawing.Color.FromArgb(24, 24, 24);
    private readonly System.Drawing.Color m_lines = System.Drawing.Color.FromArgb(198, 198, 198);
    private readonly System.Drawing.Color m_dots = System.Drawing.Color.FromArgb(198, 50, 198);

    public RevFunctionGraphicTask(string output)
    {
        m_output = output;
    }

    public override Task ExecuteAsync(CancellationToken cancelToken)
    {
        if (!Directory.Exists($"{m_output}{m_folder}"))
            Directory.CreateDirectory($"{m_output}{m_folder}");

        int maxFunctions = 2;

        for (int i = 0; i < maxFunctions; i++)
        {
            Functions function = (Functions)i;
            double[] data = PrepData(function);

            using (Bitmap bmp = new(m_size, m_size))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(m_background);

                DrawLines(g);

                for(int e = 0; e < data.Length; e++)
                    DrawDots(g, e, data[e]);

                int index = (int)function;
                string id = index
                    .ToString()
                    .PadLeft(2, '0');

                bmp.Save($"{m_output}{m_folder}/{id}_{function}.png");
            }
        }

        return Task.CompletedTask;
    }

    private double[] PrepData(Functions function)
    {
        List<double> data = [];
        for (double t = 0; t <= 1; t+=0.01)
        {
            double d = function switch
            { 
                Functions.inSine => Easings.InSine(m_startValue, m_endValue, t),
                Functions.outSine => Easings.OutSine(t),
                Functions.inOutSine => Easings.InOutSine(t),
                Functions.inQuad => Easings.InQuad(t),
                Functions.outQuad => Easings.OutQuad(t),
                Functions.inOutQuad => Easings.InOutQuad(t),
                Functions.inCubic => Easings.InCubic(t),
                Functions.outCubic => Easings.OutCubic(t),
                Functions.inOutCubic => Easings.InOutCubic(t),
                Functions.inQuart => Easings.InQuart(t),
                Functions.outQuart => Easings.OutQuart(t),
                Functions.inOutQuart => Easings.InOutQuart(t),
                Functions.inQuint => Easings.InQuint(t),
                Functions.outQuint => Easings.OutQuint(t),
                Functions.inOutQuint => Easings.InOutQuint(t),
                Functions.inExpo => Easings.InExpo(t),
                Functions.outExpo => Easings.OutExpo(t),
                Functions.inOutExpo => Easings.InOutExpo(t),
                Functions.inCirc => Easings.InCirc(t),
                Functions.outCirc => Easings.OutCirc(t),
                Functions.inOutCirc => Easings.InOutCirc(t),
                Functions.inBack => Easings.InBack(t),
                Functions.outBack => Easings.OutBack(t),
                Functions.inOutBack => Easings.InOutBack(t),
                Functions.inElastic => Easings.InElastic(t),
                Functions.outElastic => Easings.OutElastic(t),
                Functions.inOutElastic => Easings.InOutElastic(t),
                Functions.inBounce => Easings.InBounce(t),
                Functions.outBounce => Easings.OutBounce(t),
                Functions.inOutBounce => Easings.InOutBounce(t),
                _ => Easings.Lerp(m_startValue, m_endValue, t)
            };
            data.Add(d);
        }

        return data.ToArray();
    }

    private void DrawLines(Graphics g)
    {
        Brush brush = new SolidBrush(m_lines);
        g.FillRectangle(brush, m_xMargin, m_yMargin, 4, m_size - m_yMargin * 2);
        g.FillRectangle(brush, m_xMargin, m_size - m_yMargin, m_size - m_xMargin * 2, 4);
    }

    private void DrawDots(Graphics g, int index, double data) 
    {
        Brush brush = new SolidBrush(m_dots);
        int x = m_xMargin + Math.Round(index * 0.01f * (m_size - m_xMargin * 2f));
        int y = Math.Round((float)(m_size - m_yMargin - data));
        g.FillEllipse(brush, x, y, 4, 4);
    }
        
}
