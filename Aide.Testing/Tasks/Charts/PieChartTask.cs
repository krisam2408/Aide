using Aide.Charts;
using Aide.Charts.DataTransfer;
using Aide.Color;
using TerminalWrapper;

namespace Aide.Testing.Tasks.Charts;

internal class PieChartTask : MainTask
{
    private readonly string m_output;

    public override string TaskName => "Pie Chart Task";

    public PieChartTask(string output)
    {
        m_output = $"{output}charts/pieChart.jpg"; 
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        SingleDataset data = GetData();

        ChartOptions<SingleDataset> config = new(m_output, data)
        {
            Title = "Colors",
            BackgroundColor = new ColorRatio(0.5, 0.5, 0.5)
        };

        PieChart chart = new(config);

        await chart.DrawChart();
    }

    private static SingleDataset GetData()
    {
        string[] labels = ["Red", "Blue", "Green", "Orange"];
        ColorHSB[] colors = [
            new(0, 80, 100),
            new(240, 80, 100),
            new(120, 80, 100),
            new(30, 80, 100)
        ];

        SingleDataset dataset = new(labels, colors);

        dataset.AddData([30, 50, 70, 90]);

        return dataset;
    }
}
