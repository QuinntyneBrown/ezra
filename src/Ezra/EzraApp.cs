using System.CommandLine;
using Ezra.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ezra;

// Composes every tool module into one service container and one command tree, then runs the command.
public sealed class EzraApp(IReadOnlyList<IEzraModule> modules)
{
    public async Task<int> RunAsync(string[] args, TextWriter output, TextWriter error, CancellationToken ct = default)
    {
        var services = new ServiceCollection();
        foreach (var module in modules)
            module.ConfigureServices(services);
        await using var provider = services.BuildServiceProvider();

        var root = new RootCommand("Ezra: a modular .NET command-line tool.");
        foreach (var module in modules)
            root.Subcommands.Add(module.CreateCommand(provider));

        var config = new InvocationConfiguration { Output = output, Error = error };
        return await root.Parse(args).InvokeAsync(config, ct);
    }
}
