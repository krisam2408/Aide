using Aide.Color;
using TerminalWrapper;

namespace Aide.ColorExtraction.Tasks;

public sealed class DebugColorHSBCreationTask : MainTask
{
    public override string TaskName => $"Debug Color HSB Creation ({m_hue}, {m_saturation}, {m_brightness})";

    public readonly int m_hue;
    public readonly int m_brightness;
    public readonly int m_saturation;

    public DebugColorHSBCreationTask(int hue, int brightness, int saturation) : base()
    {
        m_hue = hue;
        m_brightness = brightness;
        m_saturation = saturation;
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        ColorHSB color = new(m_hue, m_saturation, m_brightness);
        HexCode code = new(color.Hexcode());

        await Terminal.WriteAsync($"Resulting color: {code.Code}");
    }
}
