# Estimate — report-results

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/cli-host/report-results/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `ExitCodes`, `IReporter`, exception types in `Ezra.Abstractions` | 1 |
| `ConsoleReporter` with verbosity, stream split and `NO_COLOR`/redirect detection | 3 |
| Global `--verbose` / `--quiet` options and mutual-exclusion validator | 2 |
| `ExceptionMapper` including Ctrl+C handling | 2 |
| Acceptance tests for each exit code, quiet, verbose, redirected output and `NO_COLOR` | 5 |
| **Total** | **13** |

## Minimal scope

The minimal scope ships the exit codes, `ExceptionMapper` and an uncoloured `ConsoleReporter`. It satisfies L2-007 and L2-008 criteria 1, 3 and 4, and defers `--verbose` step logging (L2-008 criterion 2).

Minimal scope total: **10 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| Colour output; always plain text | No coloured warnings or errors | 1 |
| `--verbose` step logging | No per-step diagnostics | 2 |
