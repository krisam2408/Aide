using System.Drawing;
using System.Runtime.Versioning;
using TerminalWrapper;

namespace Aide.Testing.Tasks;

[SupportedOSPlatform("windows")]
internal class FunctionGraphicTask : MainTask
{
    public override string TaskName => "Try Graphics";

    private enum Functions
    {
        lerp,
        inSine, outSine, inOutSine,
        inQuad, outQuad, inOutQuad,
        inCubic, outCubic, inOutCubic,
        inQuart, outQuart, inOutQuart,
        inQuint, outQuint, inOutQuint,
        inExpo, outExpo, inOutExpo,
        inCirc, outCirc, inOutCirc,
        inBack, outBack, inOutBack,
        inElastic, outElastic, inOutElastic,
        inBounce, outBounce, inOutBounce,
    }

    private readonly string m_output;
    private readonly int m_margin = 16;
    private readonly System.Drawing.Color m_background = System.Drawing.Color.FromArgb(24, 24, 24);
    private readonly System.Drawing.Color m_lines = System.Drawing.Color.FromArgb(198, 198, 198);
    private readonly System.Drawing.Color m_dots = System.Drawing.Color.FromArgb(198, 50, 198);

    public FunctionGraphicTask(string output)
    {
        m_output = output;
    }

    public override Task ExecuteAsync(CancellationToken cancelToken)
    {
        foreach (Functions function in Enum.GetValues(typeof(Functions)))
        {
            double[] data = PrepData(function);

            using (Bitmap bmp = new(255, 255))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(m_background);

                DrawLines(g);

                foreach(double d in data)
                    DrawDots(g, d);

                bmp.Save($"{m_output}{function}.png");
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
                Functions.inSine => Easings.InSine(t),
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
                _ => Easings.Lerp(0, 1, t)
            };
            data.Add(d);
        }

        return data.ToArray();
    }

    private void DrawLines(Graphics g)
    {
        Brush brush = new SolidBrush(m_lines);
        g.FillRectangle(brush, m_margin, m_margin, 4, 255 - m_margin*2);
        g.FillRectangle(brush, m_margin, 255 - m_margin, 255 - m_margin * 2, 4);
    }

    private void DrawDots(Graphics g, double data) { }
        
}
