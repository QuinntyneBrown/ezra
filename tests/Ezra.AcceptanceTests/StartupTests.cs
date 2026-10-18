// Acceptance Test
// Traces to: L2-017
// Description: Startup does not build module services, and `ezra --help` is fast.

using System.CommandLine;
using System.Diagnostics;
using Ezra.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ezra.AcceptanceTests;

// Runs alone: timing under parallel pack-and-install tests measures contention, not startup.
[CollectionDefinition(nameof(StartupTests), DisableParallelization = true)]
public sealed class StartupTimingCollection;

[Collection(nameof(StartupTests))]
public sealed class StartupTests
{
    [Fact]
    public async Task Handler_services_are_built_only_when_their_command_runs()
    {
        // Given a module whose handler service records when it is built
        var created = new List<string>();
        IEzraModule[] modules = [new RecordingModule("first", created), new RecordingModule("second", created)];

        // When help is shown and then one command runs
        await InProcess.RunAsync(modules, "--help");
        await InProcess.RunAsync(modules, "first");

        // Then only the service of the command that ran was built
        Assert.Equal(["first"], created);
    }

    [Fact]
    public void Help_median_is_under_300_ms()
    {
        // Given the built Ezra host
        var ezra = Path.Combine(AppContext.BaseDirectory, "Ezra.dll");
        Cli.Run("dotnet", $"\"{ezra}\" --help", AppContext.BaseDirectory); // warm-up

        // When `ezra --help` runs 10 times
        var times = Enumerable.Range(0, 10).Select(_ =>
        {
            var watch = Stopwatch.StartNew();
            var result = Cli.Run("dotnet", $"\"{ezra}\" --help", AppContext.BaseDirectory);
            Assert.Equal(0, result.ExitCode);
            return watch.ElapsedMilliseconds;
        }).Order().ToList();

        // Then the median wall-clock time is under 300 ms
        Assert.True(times[5] < 300, $"Median was {times[5]} ms ({string.Join(", ", times)}).");
    }

    private sealed class RecordingModule(string name, List<string> created) : IEzraModule
    {
        public void ConfigureServices(IServiceCollection services) =>
            services.AddKeyedTransient(name, (_, _) => { created.Add(name); return new object(); });

        public Command CreateCommand(IServiceProvider services)
        {
            var command = new Command(name);
            command.SetAction(_ => services.GetRequiredKeyedService<object>(name));
            return command;
        }
    }
}
