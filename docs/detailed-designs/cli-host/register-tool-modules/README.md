# Register tool modules

## Overview

Ezra stays maintainable at hundreds of commands by splitting them into *tool modules*.

- **tool module** — .NET class library named `Ezra.<Tool>` that contributes one top-level command and its services through the module contract
- **module contract** — `IEzraModule` interface in `Ezra.Abstractions` that every tool module implements
- **capability** — service interface published in `Ezra.Abstractions` that one module implements and another module consumes

The host composes modules; modules never see each other. Adding a tool is two edits in the host: a project reference and one registration line. This feature defines that contract, how the host builds one dependency-injection container from all modules, how one module calls another through a capability, and why startup cost stays flat as modules are added (L2-017).

## Description

- **`IEzraModule`** (`Ezra.Abstractions`) — the module contract.
  - `string Name` — the module's top-level command name.
  - `void ConfigureServices(IServiceCollection services)` — registers the module's handlers and services. It shall perform no I/O.
  - `Command CreateCommand(IServiceProvider services)` — builds the command subtree. Each action resolves its handler from `services` only when invoked, so building the tree does no work beyond allocating command objects (L2-017).
- **`ICommandHandler<TOptions>`** (`Ezra.Abstractions`) — `Task<int> HandleAsync(TOptions options, CancellationToken ct)`. A command action binds its `ParseResult` to `TOptions`, resolves the handler and calls it.
- **`Program.Modules`** (`src/Ezra/Program.cs`) — the explicit list of modules, for example `new ScaffoldModule()`. Registration is explicit, not reflection-based, so startup does no assembly scanning.
- **`ModuleRegistry`** (`src/Ezra`) — validates the module list. If two modules share a `Name`, it throws `DuplicateCommandException` naming both module types and the command; the host reports it and returns exit code `1` (L2-005 criterion 3).
- **`EzraApp`** — calls `ConfigureServices` on every module, adds host services (`IReporter`), builds one `ServiceProvider`, then calls `CreateCommand` on every module and adds each result to the root.
- **`ServiceProviderExtensions.GetCapability<T>()`** (`Ezra.Abstractions`) — resolves capability `T`. If no module registered `T`, it throws `MissingCapabilityException(typeof(T))`, which the host reports as `Capability 'T' is not available` with exit code `1` (L2-006 criterion 2).
- **Dependency rule** — each `Ezra.<Tool>.csproj` references only `Ezra.Abstractions` and external packages. Review enforces the rule; no test asserts project structure.
- **Capability example** — none exists yet. The first capability is introduced by the first tool that needs one.

## Requirements

The feature realizes the following level-2 (L2) requirements. Each L2 requirement refines a level-1 (L1) requirement, cited by identifier.

| L2 ID | Refines (L1) | Requirement |
|-------|--------------|-------------|
| `L2-005` | `L1-003` | Each tool shall be its own project and assembly named `Ezra.<Tool>` that implements one shared module contract defined in an `Ezra.Abstractions` project. Modules shall depend only on `Ezra.Abstractions` (and their own dependencies), never on each other or on the root host. |
| `L2-006` | `L1-003` | A command in one module shall be able to invoke a capability of another module only through a service interface published in `Ezra.Abstractions` (or a module-owned contracts package), never by referencing another module's implementation or by spawning `ezra` as a child process. |
| `L2-017` | `L1-007` | Commands shall be built so that startup cost does not grow with the number of registered commands: modules shall register commands without performing I/O or network calls, and handlers shall resolve their services only when invoked. |

## Diagrams

### System context

The developer sees one `ezra` command; the module split is internal to Ezra.

![C4 system context for register tool modules](diagrams/c4-context.png)

### Containers

The host references each tool module; tool modules reference only `Ezra.Abstractions`.

![C4 container view for register tool modules](diagrams/c4-container.png)

### Components

`EzraApp` reads the module list, `ModuleRegistry` validates it, and one service container serves every module's handlers.

![C4 component view for register tool modules](diagrams/c4-component.png)

### Class structure

Modules implement `IEzraModule`; handlers implement `ICommandHandler<TOptions>`; a consumer module depends on a capability interface, never on the providing module.

![Class diagram for register tool modules](diagrams/class-structure.png)

### Behaviour — compose modules at startup

The host validates names, gathers every module's services into one container, then builds the command tree without resolving any handler (L2-005, L2-017).

![Sequence diagram for compose modules at startup](diagrams/sequence-startup.png)

### Behaviour — call another module's capability

A consumer handler resolves a capability interface through the shared container; a missing provider yields exit code `1` (L2-006).

![Sequence diagram for call another module's capability](diagrams/sequence-capability.png)
