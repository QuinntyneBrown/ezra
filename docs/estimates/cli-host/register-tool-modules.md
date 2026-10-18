# Estimate — register-tool-modules

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/cli-host/register-tool-modules/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `IEzraModule` in `Ezra.Abstractions` | 1 |
| `EzraApp` composition of services and commands, duplicate-name check | 4 |
| Acceptance tests with test-only modules: registration, duplicate names, capability present and missing | 5 |
| Startup timing test for `ezra --help` median under 300 ms | 2 |
| **Total** | **12** |

## Minimal scope

The minimal scope implements the module contract, explicit registration and the duplicate-name check. It satisfies L2-005 and L2-017, and defers L2-006 until the first cross-module capability is needed.

Minimal scope total: **9 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| Startup timing test | L2-017 criterion 1 is verified by hand | 2 |
