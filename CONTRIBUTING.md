# Contributing to Ezra

Thanks for your interest in contributing. This guide explains how to propose and land a change.

By participating you agree to abide by the [Code of Conduct](CODE_OF_CONDUCT.md).

## Reporting bugs and requesting features

Search [existing issues](https://github.com/quinntynebrown/ezra/issues) first, then open a new one. For a bug, include the command you ran, what you expected, what happened, your OS and `dotnet --version`. For security issues, do **not** open a public issue; see [SECURITY.md](SECURITY.md).

For anything larger than a small fix, open an issue and agree on the approach before writing code.

## Development setup

Prerequisite: the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
git clone https://github.com/<your-fork>/ezra.git
cd ezra
dotnet build
dotnet test
```

Warnings are treated as errors.

## Project rules

Read [AGENTS.md](AGENTS.md) before changing code. In short:

- **One tool, one project.** Each tool is its own `Ezra.<Tool>` project. Dependencies point one way: `Ezra` → `Ezra.<Tool>` → `Ezra.Abstractions`. Tools never reference each other; they share capabilities through interfaces in `Ezra.Abstractions`.
- **Keep the host thin.** Startup, global options, error handling and exit codes only.
- **Keep changes small.** Implement what the change asks for and nothing more. No speculative features or unrelated refactoring.
- **Requirements come first.** Behavior is specified in [docs/specs](docs/specs).

## Tests

Production behavior is developed with acceptance test-driven development:

1. Write the Given-When-Then acceptance criteria for one small slice.
2. Write the acceptance test in `tests/Ezra.AcceptanceTests` with a `Traces to: L2-xxx` header, and confirm it fails for the expected reason.
3. Write the minimum production code to make it pass, then refactor with tests green.

Tests prove behavior. Do not add tests that assert code structure, layout or naming.

## Submitting a pull request

1. Fork the repository and create a branch from `main`.
2. Make your change in small, focused commits with clear messages.
3. Run `dotnet build -c Release` and `dotnet test -c Release`; both must pass.
4. Open a pull request describing what changed and why, and link the related issue.

CI builds, tests and packs on Linux, Windows and macOS. A maintainer will review your pull request; please respond to feedback and keep the branch up to date.

## License

By contributing, you agree that your contributions are licensed under the [MIT License](LICENSE).
