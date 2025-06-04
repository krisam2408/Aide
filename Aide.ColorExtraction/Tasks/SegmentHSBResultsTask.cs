using Aide.ColorExtraction.DataTransfer;
using CsvHelper;
using System.Globalization;
using TerminalWrapper;

namespace Aide.ColorExtraction.Tasks;

public sealed class SegmentHSBResultsTask : MainTask
{
    private const string Locale = "en-US";
    private readonly string m_inputPath;
    private readonly string m_outputPath;

    public override string TaskName => "Segment HSB Results";

    private readonly List<HSBValue> m_results = [];

    public SegmentHSBResultsTask(string path)
    {
        m_inputPath = $"{path}adobe_hsb.csv";
        m_outputPath = $"{path}hsb/adobe_hsb_b*.csv";
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        await Terminal.WriteAsync($"Checking {m_inputPath}...");

        if (!File.Exists(m_inputPath))
        {
            await Terminal.WriteAsync("No data to segment found...");
            return;
        }

        m_results.Clear();

        using(StreamReader sr = new(m_inputPath))
        using(CsvReader csvR = new(sr, CultureInfo.GetCultureInfo(Locale)))
        {
            HSBValue[] records = csvR
                .GetRecords<HSBValue>()
                .OrderByDescending(c => c.Brightness)
                .ThenByDescending(c => c.Saturation)
                .ThenBy(c => c.Hue)
                .ToArray();

            m_results.AddRange(records);
        }

        int[] distinctBrightness = m_results
            .DistinctBy(c => c.Brightness)
            .Select(c => c.Brightness)
            .ToArray();

        foreach (int i in distinctBrightness)
        {
            HSBValue[] segment = m_results
                .Where(c => c.Brightness == i)
                .OrderByDescending(c => c.Brightness)
                .ThenByDescending(c => c.Saturation)
                .ThenBy(c => c.Hue)
                .ToArray();

            string brgId = i.ToString()
                .PadLeft(3, '0');

            string path = m_outputPath
                .Replace("*", brgId);

            using (StreamWriter sw = new(path))
            using (CsvWriter csvW = new(sw, CultureInfo.GetCultureInfo(Locale)))
            {
                csvW.WriteRecords(segment);
            };
        }
    }
}
