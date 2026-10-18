using Ezra.Abstractions;

namespace Ezra.AcceptanceTests;

// Runs the Ezra host in-process with the given modules.
public static class InProcess
{
    public static async Task<CliResult> RunAsync(IEzraModule[] modules, params string[] args)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = await new EzraApp(modules).RunAsync(args, output, error);
        return new CliResult(exitCode, output.ToString(), error.ToString());
    }
}
