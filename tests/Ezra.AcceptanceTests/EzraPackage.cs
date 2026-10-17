namespace Ezra.AcceptanceTests;

// Packs src/Ezra into a throwaway local feed, once per version.
public sealed class EzraPackage : IDisposable
{
    private readonly HashSet<string> packed = [];

    public string Feed { get; } = Directory.CreateTempSubdirectory("ezra-feed-").FullName;

    private readonly string runId = Guid.NewGuid().ToString("N");
    private int count;

    // Versions increase within a run; the run id keeps them out of the NuGet cache of earlier runs.
    public string NewVersion() => $"0.0.{Interlocked.Increment(ref count)}-test.{runId}";

    public void Pack(string version)
    {
        lock (packed)
        {
            if (!packed.Add(version)) return;
            var project = Path.Combine(Cli.RepoRoot, "src", "Ezra", "Ezra.csproj");
            Cli.Dotnet($"pack \"{project}\" -c Release -o \"{Feed}\" -p:Version={version}", Cli.RepoRoot);
        }
    }

    public void Dispose() => Directory.Delete(Feed, recursive: true);
}
