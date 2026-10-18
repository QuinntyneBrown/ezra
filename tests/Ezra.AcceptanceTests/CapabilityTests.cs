// Acceptance Test
// Traces to: L2-006
// Description: One module uses another module's capability through a shared interface, in-process.

using System.CommandLine;
using Ezra.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ezra.AcceptanceTests;

public sealed class CapabilityTests
{
    [Fact]
    public async Task Consumer_calls_provider_through_the_capability_interface()
    {
        // Given module A uses a capability that module B implements
        IEzraModule[] modules = [new ConsumerModule(), new ProviderModule()];

        // When A's command runs
        var result = await InProcess.RunAsync(modules, "consume");

        // Then B's implementation is invoked in-process
        Assert.Equal(0, result.ExitCode);
        Assert.Equal($"greeting from provider in process {Environment.ProcessId}", result.StdOut.Trim());
    }

    [Fact]
    public async Task Missing_provider_is_reported_by_capability_name()
    {
        // Given module B is not registered
        IEzraModule[] modules = [new ConsumerModule()];

        // When A's command that needs B runs
        var result = await InProcess.RunAsync(modules, "consume");

        // Then the error names the missing capability and the exit code is 1
        Assert.Equal(1, result.ExitCode);
        Assert.Contains(nameof(IGreeting), result.StdErr);
    }

    // Stands in for a capability interface published in Ezra.Abstractions.
    public interface IGreeting
    {
        string Greet();
    }

    private sealed class ConsumerModule : IEzraModule
    {
        public void ConfigureServices(IServiceCollection services) { }

        public Command CreateCommand(IServiceProvider services)
        {
            var command = new Command("consume");
            command.SetAction(result =>
                result.InvocationConfiguration.Output.WriteLine(services.GetRequiredService<IGreeting>().Greet()));
            return command;
        }
    }

    private sealed class ProviderModule : IEzraModule
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddSingleton<IGreeting, ProviderGreeting>();

        public Command CreateCommand(IServiceProvider services) => new("provide");
    }

    private sealed class ProviderGreeting : IGreeting
    {
        public string Greet() => $"greeting from provider in process {Environment.ProcessId}";
    }
}
