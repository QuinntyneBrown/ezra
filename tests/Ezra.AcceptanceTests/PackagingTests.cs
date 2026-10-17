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

    [Fact]
    public void Local_tool_runs_through_dotnet()
    {
        // Given the Ezra package and a folder with a tool manifest
        var version = package.NewVersion();
        package.Pack(version);
        Cli.Dotnet("new tool-manifest", work);

        // When it is installed locally and `dotnet ezra --version` runs
        Cli.Dotnet($"tool install Ezra --local --add-source \"{package.Feed}\" --version {version}", work);
        var result = Cli.Run("dotnet", "ezra --version", work);

        // Then Ezra runs and prints its version
        Assert.Equal(0, result.ExitCode);
        Assert.Equal(version, result.StdOut.Trim());
    }

    [Fact]
    public void Updated_tool_reports_the_new_version()
    {
        // Given an older version is installed
        var older = package.NewVersion();
        var newer = package.NewVersion();
        package.Pack(older);
        package.Pack(newer);
        var toolPath = Path.Combine(work, "tools");
        Cli.Dotnet($"tool install Ezra --tool-path \"{toolPath}\" --add-source \"{package.Feed}\" --version {older}", work);

        // When the tool is updated
        Cli.Dotnet($"tool update Ezra --tool-path \"{toolPath}\" --add-source \"{package.Feed}\" --version {newer}", work);
        var result = Cli.Run(Path.Combine(toolPath, "ezra"), "--version", work);

        // Then `ezra --version` reports the new version
        Assert.Equal(newer, result.StdOut.Trim());
    }

    public void Dispose() => Directory.Delete(work, recursive: true);
}
