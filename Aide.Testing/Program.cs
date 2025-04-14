using Aide.Testing.Tasks;
using System.Runtime.Versioning;
using TerminalWrapper;
using TerminalWrapper.Console;

namespace Aide.Testing;

[SupportedOSPlatform("windows")]
public sealed class Program
{
    private const string m_output = "../../../output/";

    public static async Task Main()
    {
        List<MainTask> tasks = 
        [
            new ExampleTask(m_output),
            new FunctionGraphicTask(m_output)
        ];

        Terminal terminal = ConsoleTerminal.CreateTerminal(tasks);

        await terminal.RunAsync();
    }
}
