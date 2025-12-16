using Aide.Testing.Tasks.Charts;
using Aide.Testing.Tasks.Easings;
using TerminalWrapper;
using TerminalWrapper.Console;

const string m_output = "../../../output/";
CancellationToken token = new();

List<MainTask> tasks = 
[
    new LinearChartTask(m_output),
    new BarChartTask(m_output),
    new PieChartTask(m_output),
];

foreach (MainTask task in tasks)
    await task.ExecuteAsync(token);

