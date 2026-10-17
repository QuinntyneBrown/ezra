using System.Diagnostics;

namespace Ezra.AcceptanceTests;

public sealed record CliResult(int ExitCode, string StdOut, string StdErr);

public static class Cli
{
    public static string RepoRoot { get; } = FindRepoRoot();

    public static CliResult Run(string fileName, string arguments, string workingDirectory)
    {
        var start = new ProcessStartInfo(fileName, arguments)
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        using var process = Process.Start(start)!;
        var stdout = process.StandardOutput.ReadToEndAsync();
        var stderr = process.StandardError.ReadToEndAsync();
        process.WaitForExit();
        return new CliResult(process.ExitCode, stdout.Result, stderr.Result);
    }

    public static CliResult Dotnet(string arguments, string workingDirectory)
    {
        var result = Run("dotnet", arguments, workingDirectory);
        Assert.True(result.ExitCode == 0, $"dotnet {arguments} failed:\n{result.StdOut}\n{result.StdErr}");
        return result;
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "Ezra.slnx")))
            dir = dir.Parent;
        return dir?.FullName ?? throw new InvalidOperationException("Ezra.slnx not found above the test output folder.");
    }
}
