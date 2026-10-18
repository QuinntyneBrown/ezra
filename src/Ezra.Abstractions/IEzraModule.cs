using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace Ezra.Abstractions;

// A tool module: one Ezra.<Tool> project contributes its services and one top-level command.
public interface IEzraModule
{
    // Registers the module's services. Must do no I/O.
    void ConfigureServices(IServiceCollection services);

    // Builds the module's command subtree. Actions resolve their services from `services`
    // only when invoked, so building the tree does no work beyond allocating commands.
    Command CreateCommand(IServiceProvider services);
}
