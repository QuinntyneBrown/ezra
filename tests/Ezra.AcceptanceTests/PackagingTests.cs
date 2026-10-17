// Acceptance Test
// Traces to: L2-001
// Description: Ezra installs as a .NET tool and prints its version.

namespace Ezra.AcceptanceTests;

public sealed class PackagingTests(EzraPackage package) : IClassFixture<EzraPackage>, IDisposable
{
    private readonly string work = Directory.CreateTempSubdirectory("ezra-work-").FullName;

    [Fact]
    public void Installed_tool_prints_its_version()
    {
        // Given the Ezra package
        var version = package.NewVersion();
        package.Pack(version);

        // When it is installed as a tool and `ezra --version` runs
        var toolPath = Path.Combine(work, "tools");
        Cli.Dotnet($"tool install Ezra --tool-path \"{toolPath}\" --add-source \"{package.Feed}\" --version {version}", work);
        var result = Cli.Run(Path.Combine(toolPath, "ezra"), "--version", work);

        // Then the package version is printed and the exit code is 0
        Assert.Equal(0, result.ExitCode);
        Assert.Equal(version, result.StdOut.Trim());
    }

    public void Dispose() => Directory.Delete(work, recursive: true);
}
