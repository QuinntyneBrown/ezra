# Estimate — route-commands

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/cli-host/route-commands/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `EzraApp` tree assembly, parse and invoke | 4 |
| Root no-argument listing of top-level commands | 1 |
| `ParseErrorReporter` and `CommandSuggester` | 3 |
| Acceptance tests for nested routing, unknown command, help, defaults and allowed values | 5 |
| **Total** | **13** |

## Minimal scope

The minimal scope routes commands and relies on the default `System.CommandLine` error and help output. It satisfies L2-003 criteria 1 and 2 and L2-004, and defers the closest-match suggestion in L2-003 criterion 3.

Minimal scope total: **8 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| `CommandSuggester`; report the unknown token only | No 'did you mean' hint | 3 |
