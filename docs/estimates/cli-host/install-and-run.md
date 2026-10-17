# Estimate — install-and-run

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/cli-host/install-and-run/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `Ezra.csproj` tool packaging and `Directory.Build.props` | 2 |
| `TextFile` helper with line-ending and encoding rules | 2 |
| `PackagingTests`: pack, global-path install, local-manifest install, update, `--version` | 6 |
| `ci.yml` three-OS matrix with test and pack | 4 |
| **Total** | **14** |

## Minimal scope

The minimal scope packs the tool and runs the acceptance tests on Linux only. It keeps `TextFile` and the `--version` test. It satisfies L2-001 and defers the Windows and macOS legs of L2-002.

Minimal scope total: **8 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| Local-manifest and update install tests; keep global install only | L2-001 criteria 2 and 3 are verified by hand | 3 |
| Windows and macOS CI legs | Platform regressions are found after release | 2 |
