using Aide.ColorExtraction.DataTransfer;
using CsvHelper;
using SkiaSharp;
using System.Globalization;
using TerminalWrapper;

namespace Aide.ColorExtraction.Tasks;

public sealed class CompareHSBColorTask : MainTask
{
    private readonly CultureInfo Culture = new("en-US");
    private readonly string m_baseDirectory;
    private const string m_adobeFilenamePattern = "adobe_hsb_b*.csv";

    public override string TaskName => "Compare HSB Color";

    public CompareHSBColorTask(string path)
    {
        m_baseDirectory = path;
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        int brightness = await SetValue("Brightness", 1);

        int saturation = await SetValue("Saturation");

        HSBExtractionValue[] records = await GetColorRecords(brightness, saturation);
        HSBComparisonValue[] comparedRecords = Compare(records);
        
        await WriteCsvComparison(comparedRecords, brightness, saturation);
        await DrawImageComparison(comparedRecords, brightness, saturation);

        await WriteFinalResult(comparedRecords);
    }

    private async Task<int> SetValue(string valueName, int minValue = 0)
    {
        int value = -1;

        do
        {
            await Terminal.WriteAsync($"Write a {valueName} value [{minValue}-100]:");
            string? input = await Terminal.ReadAsync();

            if (!int.TryParse(input, out value))
            {
                await Terminal.WriteAsync("Invalid input");
                continue;
            }

            if (value < minValue && value > 100)
            {
                await Terminal.WriteAsync("Invalid input");
                continue;
            }

        } while (value < minValue && value > 100);

        await Terminal.WriteAsync($"{valueName} {value} selected");

        return value;
    }

    private async Task<HSBExtractionValue[]> GetColorRecords(int brightness, int saturation)
    {
        await Terminal.WriteAsync("Getting Adobe results");

        string paddedIndex = brightness
            .ToString()
            .PadLeft(3, '0');

        string inputPath = $"{m_baseDirectory}adobe_hsb/{m_adobeFilenamePattern.Replace("*", paddedIndex)}";

        using StreamReader sr = new(inputPath);
        using CsvReader csv = new(sr, Culture);
        HSBExtractionValue[] records = csv
            .GetRecords<HSBExtractionValue>()
            .Where(c => c.Saturation == saturation)
            .OrderByDescending(c => c.Brightness)
            .ThenByDescending(c => c.Saturation)
            .ThenBy(c => c.Hue)
            .ToArray();

        await Terminal.WriteAsync($"{records.Length} records found");
        return records;
    }

    private static HSBComparisonValue[] Compare(HSBExtractionValue[] records)
    {
        HSBComparisonValue[] result = records
            .Select(r => new HSBComparisonValue(r))
            .ToArray();

        return result;
    }

    private async Task WriteCsvComparison(HSBComparisonValue[] records, int brightness, int saturation)
    {
        await Terminal.WriteAsync("Writing CSV Results");

        string paddedBrg = brightness.ToString().PadLeft(3, '0');
        string paddedSat = saturation.ToString().PadLeft(3, '0');

        string outputPath = $"{m_baseDirectory}hsb_comparison/csv/comparison_b{paddedBrg}_s{paddedSat}.csv";

        using StreamWriter sw = new(outputPath);
        using CsvWriter csv = new(sw, Culture);
        csv.WriteRecords(records);
    }

    private async Task DrawImageComparison(HSBComparisonValue[] records, int brightness, int saturation)
    {
        await Terminal.WriteAsync("Drawing Image Results");

        string paddedBrg = brightness.ToString().PadLeft(3, '0');
        string paddedSat = saturation.ToString().PadLeft(3, '0');

        string outputPath = $"{m_baseDirectory}hsb_comparison/images/comparison_b{paddedBrg}_s{paddedSat}.png";

        using (SKBitmap bmp = new(200, 36000))
        using (SKCanvas g = new(bmp))
        {
            foreach (HSBComparisonValue record in records)
            {
                DrawRectangles(g, record);
            }

            using(SKImage image = SKImage.FromBitmap(bmp))
            using(SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
            using(Stream fs = File.OpenWrite(outputPath))
            {
                data.SaveTo(fs);
            }
        }

    }

    private static void DrawRectangles(SKCanvas g, HSBComparisonValue record)
    {
        SKColor adobeColor = SKColor.Parse(record.AdobeHex);
        SKPaint adobeBrush = new() { Color = adobeColor };

        SKColor aideColor = SKColor.Parse(record.AideHex);   
        SKPaint aideBrush = new() { Color = aideColor, };

        SKPaint textColor = new() { Color = SKColors.Black };
        SKFont textFont = new(SKTypeface.Default, 36);

        float height = record.Hue * 100f;

        g.DrawRect(0f, height, 100f, 100f, adobeBrush);
        g.DrawRect(100f, height, 100f, 100f, aideBrush);

        g.DrawText(record.Hue.ToString(), 25f, height + 50f, textFont, textColor);
    }

    private async Task WriteFinalResult(HSBComparisonValue[] records)
    {
        await Terminal.SeparatorAsync();
        await Terminal.WriteAsync("Checking Result Differences");

        Dictionary<string, int> recount = new();

        void addOrUpdate(string value)
        {
            if(recount.ContainsKey(value))
            {
                recount[value]++;
                return;
            }

            recount.Add(value, 1);
        }

        int total = 0;
        foreach (HSBComparisonValue value in records)
        {
            total += 3;
            if(value.DeltaRed != "0")
                addOrUpdate(value.DeltaRed);
            
            if(value.DeltaGreen != "0")
                addOrUpdate(value.DeltaGreen);
            
            if(value.DeltaBlue != "0")
                addOrUpdate(value.DeltaBlue);
        }

        if(recount.Count == 0)
        {
            await Terminal.WriteAsync("No Differences found!");
            return;
        }

        int count = 0;
        await Terminal.WriteAsync("Following Differences found!");
        foreach(KeyValuePair<string, int> kvp in recount)
        {
            count += kvp.Value;
            await Terminal.WriteAsync($"{kvp.Key} --> {kvp.Value}");
        }

        await Terminal.SeparatorAsync();
        await Terminal.WriteAsync($"{count} / {total} errors!");
    }
}
