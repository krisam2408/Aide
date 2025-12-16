using Aide.Charts;
using Aide.Charts.DataTransfer;
using Aide.Color;
using TerminalWrapper;

namespace Aide.Testing.Tasks.Charts;

internal class LinearChartTask : MainTask
{
    private readonly string m_output;

    public override string TaskName => "Linear Chart Task";

    public LinearChartTask(string output)
    {
        m_output = $"{output}charts/linearChart.jpg"; 
    }

    public override async Task ExecuteAsync(CancellationToken cancelToken)
    {

        MultipleDataset data = GetData();

        ChartOptions<MultipleDataset> config = new(m_output, data)
        {
            Title = "Values per Month",
            XLabel = "Months",
            YLabel = "Values",
            BackgroundColor = new ColorRatio(0.5, 0.5, 0.5)
        };

        LineChart chart = new(config);

        await chart.DrawChart();
    }

    private static MultipleDataset GetData() 
    {
        string[] labels = [ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul" ];

        MultipleDataset dataset = new(labels);

        dataset.AddLineData([ 10, 50, 30, 70, 90, 40, 60 ]);
        dataset.AddLineData([ 20, 30, 80, 60, 40, 50, 60 ]);

        return dataset;
    } 
}
