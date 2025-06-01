using Aide.ColorExtraction.Tasks;
using TerminalWrapper;
using TerminalWrapper.Console;

const string OutputPath = "../../../output/";

MainTask[] tasks = [
    new ExtractHSBColorsTask(OutputPath)
];

ConsoleSettings settings = new()
{
    SeparatorLength = 80,
    PauseAfterExit = false,
};

Terminal terminal = ConsoleTerminal.CreateTerminal(tasks, settings);

await terminal.RunAsync();