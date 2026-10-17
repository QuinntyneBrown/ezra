# Ezra

Ezra is a cross-platform .NET CLI, installed as a .NET tool (`ezra`). It hosts many commands with nested subcommands, in the style of `dotnet ef migrations add`. Requirements live in `docs/specs/L1.md` and `docs/specs/L2.md`.

## Folder structure (MANDATORY)

```
ezra/
├── docs/
│   └── specs/                      # L1.md, L2.md
├── src/
│   ├── Ezra/                       # root host: the packed .NET tool (ToolCommandName = ezra)
│   │   ├── Program.cs              # builds the host, registers every module, runs the parser
│   │   └── Ezra.csproj             # references each Ezra.<Tool> project so they ship in one package
│   ├── Ezra.Abstractions/          # the module contract and cross-tool service interfaces only
│   └── Ezra.<Tool>/                # one project per tool, e.g. Ezra.Scaffold
│       ├── <Tool>Module.cs         # implements IEzraModule: registers services, contributes the root command
│       ├── Commands/               # one file per command or subcommand
│       ├── Services/               # the tool's logic, behind interfaces
│       └── Templates/              # embedded resources (e.g. the AGENTS.md template)
├── tests/
│   └── Ezra.AcceptanceTests/       # acceptance tests with a "Traces to: L2-xxx" header
├── Ezra.slnx
└── .agents/ .claude/
```

*   **One tool, one project:** every tool is its own `Ezra.<Tool>` project. Never put one tool's commands or logic in another tool's project or in the root host.
*   **Dependencies point one way:** `Ezra` → `Ezra.<Tool>` → `Ezra.Abstractions`. A tool never references another tool or the root host. `Ezra.Abstractions` references nothing in the solution.
*   **The root host is thin:** it holds startup, global options (`--verbose`, `--quiet`), error handling and exit codes only. No command logic lives there.

## How tools are defined and orchestrated

*   **The module contract:** `Ezra.Abstractions` defines `IEzraModule`. A module does two things: it registers its services with the dependency-injection container, and it returns its one top-level command (for example `scaffold`). Registering commands must do no I/O, so startup stays fast however many tools exist.
*   **Defining subcommands:** a tool's top-level command is a `System.CommandLine` `Command`. Each subcommand is its own class in `Commands/` and is added as a child of its parent command, to any depth (`ezra <tool> <sub> <sub>`). Each command declares its arguments, options, descriptions, defaults and allowed values, so help and validation come from the declaration. A handler stays thin: it parses input and calls a service from `Services/`.
*   **Registering a tool:** add a project reference from `Ezra` to the new `Ezra.<Tool>`, then add one line for its module in `Program.cs`. Adding a tool must not change any other tool. If two modules claim the same top-level command name, startup fails.
*   **Orchestration across tools:** the root host builds one service container from every module and routes the parsed command to its handler. When one tool needs another tool's capability, the providing tool implements an interface published in `Ezra.Abstractions`, and the calling tool depends on that interface only. Calls happen in-process through dependency injection. Never reference another tool's implementation, and never shell out to `ezra`.

## Scope, Simplicity, and Zero-Waste Engineering (CRITICAL)

This is the absolute highest-priority rule in this repository. AI agents tend to over-engineer, hallucinate future needs, and balloon codebase complexity. This kills project velocity and comprehensibility. You must resist this impulse. Deliver radically simple solutions that fulfill the exact prompt—nothing more.

### 1. Absolute Scope Lock
*   **Deliver Only the Explicit Request:** Implement exactly what is asked for in the immediate task. Do not invent features, add unrequested states, create predictive API endpoints, or spin up new UI screens/components unless explicitly ordered.
*   **Zero Future-Proofing:** Do not write code for "imagined future use cases." Write code strictly for the present moment. If the user didn't ask for it today, it does not exist.
*   **Ignore Unsent Requirements:** Documents, specs, or mocks may contain broader context or future phases. Do not implement them early. Only implement what is requested in the active prompt.

### 2. Radical Simplicity & Proportionality
*   **Proportional Diffs:** The size and footprint of your code change must match the size of the request. A small feature or fix must result in a small, clean, readable diff.
*   **Path of Least Resistance:** Always choose the simplest, most direct code path that solves the problem. Do not introduce new abstractions, architectural layers, design patterns, wrappers, or external libraries unless it is physically impossible to solve the issue without them.
*   **No Unsolicited Refactoring:** Do not "improve," rewrite, or clean up code adjacent to your task unless it is strictly broken and blocking your implementation. Leave surrounding code alone.

### 3. Hard Stop & Query Clause
*   **When in Doubt, Ask:** If a task seems to logically require expanding the scope, introduces a major jump in complexity, or conflicts with the existing architecture, **STOP IMMEDIATELY**. Do not guess. Present the trade-offs to the user and await explicit confirmation before writing a single line of code.

## Incremental Implementation and ATDD - mandatory

Implement production behavior with acceptance test-driven development (ATDD). Plan small, reviewable slices, then complete one slice at a time: write Given-When-Then acceptance criteria, write the acceptance test, and run it to prove it fails for the expected reason BEFORE writing production code. Implement only what satisfies that slice, refactor with tests green, and run the relevant regression checks. Do not move to the next slice until those checks pass. No bulk implementation, no tests added afterward, and no weakening tests to manufacture a pass.

### Never write architecture tests

Never add a test that asserts the shape of the codebase rather than its behavior: no structure, layout, or naming tests; no banned-API scans; no tests that parse specifications. Those constraints belong to the compiler, the formatter, and review. A test suite exists to prove behavior.
