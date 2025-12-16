using Aide.Charts;
using Aide.Charts.DataTransfer;
using Aide.Color;
using TerminalWrapper;

namespace Aide.Testing.Tasks.Charts;

public sealed class BarChartTask : MainTask
{
    private readonly string m_output;

    public override string TaskName => "Bar Chart Task";

    public BarChartTask(string output)
    {
        m_output = $"{output}charts/barChart.jpg"; 
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        SingleDataset data = GetData();

        ChartOptions<SingleDataset> config = new(m_output, data)
        {
            Title ="Value per Categories",
            XLabel="Categories",
            YLabel="Values",
            BackgroundColor=new ColorRatio(0.5,0.5,0.5)
        };

        BarChart chart = new(config);

        await chart.DrawChart();
    }

    private static SingleDataset GetData()
    {
        string[] labels = ["A", "B", "C", "D", "E"];

        SingleDataset dataset = new(labels);

        dataset.AddData([30, 70, 50, 90, 40]);

        return dataset;
    }
}
