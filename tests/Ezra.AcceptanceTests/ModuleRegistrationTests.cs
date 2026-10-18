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

    [Fact]
    public async Task Each_module_resolves_its_own_services()
    {
        // Given two modules that each register a service their handler uses
        IEzraModule[] modules = [new ServiceModule("first", "one"), new ServiceModule("second", "two")];

        // When each module's command runs
        var first = await InProcess.RunAsync(modules, "first");
        var second = await InProcess.RunAsync(modules, "second");

        // Then each handler gets its own module's service
        Assert.Equal("one", first.StdOut.Trim());
        Assert.Equal("two", second.StdOut.Trim());
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

    private sealed class ServiceModule(string name, string text) : IEzraModule
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddKeyedSingleton(name, new Message(text));

        public Command CreateCommand(IServiceProvider services)
        {
            var command = new Command(name);
            command.SetAction(result =>
                result.InvocationConfiguration.Output.WriteLine(services.GetRequiredKeyedService<Message>(name).Text));
            return command;
        }
    }

    private sealed record Message(string Text);
}
