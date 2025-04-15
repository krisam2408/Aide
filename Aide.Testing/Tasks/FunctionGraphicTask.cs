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
    private const int m_margin = 96;
    private const int m_size = 512;
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

            using (Bitmap bmp = new(m_size, m_size))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(m_background);

                DrawLines(g);

                for(int i = 0; i < data.Length; i++)
                    DrawDots(g, i, data[i]);

                int index = (int)function;
                string id = index
                    .ToString()
                    .PadLeft(2, '0');

                bmp.Save($"{m_output}{id}_{function}.png");
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
        g.FillRectangle(brush, m_margin, m_margin, 4, m_size - m_margin*2);
        g.FillRectangle(brush, m_margin, m_size - m_margin, m_size - m_margin * 2, 4);
    }

    private void DrawDots(Graphics g, int index, double data) 
    {
        Brush brush = new SolidBrush(m_dots);
        int x = m_margin + Math.Round(index*0.01f * (m_size-m_margin*2f));
        int y = Math.Round((float)(m_size -m_margin-(data*(m_size -m_margin*2))));
        g.FillEllipse(brush, x, y, 4, 4);
    }
        
}
