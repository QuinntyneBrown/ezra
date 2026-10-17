# Estimate — register-tool-modules

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/cli-host/register-tool-modules/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `IEzraModule`, `ICommandHandler<TOptions>` in `Ezra.Abstractions` | 2 |
| `EzraApp` composition of services and commands | 3 |
| `ModuleRegistry` duplicate-name check | 1 |
| `GetCapability<T>` and `MissingCapabilityException` mapping | 2 |
| Acceptance tests with test-only modules: registration, duplicate names, capability present and missing | 5 |
| Startup timing test for `ezra --help` median under 300 ms | 2 |
| **Total** | **15** |

## Minimal scope

The minimal scope implements the module contract, explicit registration and the duplicate-name check. It satisfies L2-005 and L2-017, and defers L2-006 until the first cross-module capability is needed.

Minimal scope total: **9 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| `GetCapability<T>` and its error mapping | A missing capability surfaces as a raw DI exception with exit code 2 | 4 |
| Startup timing test | L2-017 criterion 1 is verified by hand | 2 |
