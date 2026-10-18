// Acceptance Test
// Traces to: L2-005
// Description: Tool modules contribute commands and services to the Ezra host.

using System.CommandLine;
using Ezra.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ezra.AcceptanceTests;

public sealed class ModuleRegistrationTests
{
    [Fact]
    public async Task Registered_module_command_appears_in_help_and_runs()
    {
        // Given a module that contributes the command `greet`
        IEzraModule[] modules = [new GreetModule()];

        // When the user asks for help, then runs the command
        var help = await InProcess.RunAsync(modules, "--help");
        var run = await InProcess.RunAsync(modules, "greet");

        // Then the command is listed and its handler runs
        Assert.Contains("greet", help.StdOut);
        Assert.Equal(0, run.ExitCode);
        Assert.Equal("hello", run.StdOut.Trim());
    }

    private sealed class GreetModule : IEzraModule
    {
        public void ConfigureServices(IServiceCollection services) { }

        public Command CreateCommand(IServiceProvider services)
        {
            var command = new Command("greet", "Says hello.");
            command.SetAction(result => result.InvocationConfiguration.Output.WriteLine("hello"));
            return command;
        }
    }
}
