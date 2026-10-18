# Ezra

[![CI](https://github.com/quinntynebrown/ezra/actions/workflows/ci.yml/badge.svg)](https://github.com/quinntynebrown/ezra/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Ezra is a cross-platform command-line tool for .NET, installed as a .NET tool (`ezra`). It hosts many commands with nested subcommands, in the style of `dotnet ef migrations add`.

> **Status:** early development (version 0.1.0). Commands and options may change.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Build and install from source

```sh
git clone https://github.com/quinntynebrown/ezra.git
cd ezra
dotnet build
dotnet pack src/Ezra -c Release -o artifacts
dotnet tool install --global --add-source ./artifacts Ezra
```

### Usage

```sh
ezra --help
ezra <tool> <subcommand> [options]
```

Use `--verbose` or `--quiet` to control output. Every command and subcommand documents its own arguments and options through `--help`.

## How it works

The `Ezra` project is a thin host that builds one dependency-injection container from every tool module and routes the parsed command to its handler. Each tool is its own `Ezra.<Tool>` project that implements `IEzraModule`, registers its services and contributes one top-level command.

```
src/
├── Ezra/               # root host, packed as the `ezra` tool
├── Ezra.Abstractions/  # module contract and cross-tool service interfaces
└── Ezra.<Tool>/        # one project per tool
tests/
└── Ezra.AcceptanceTests/
```

See [AGENTS.md](AGENTS.md) for the architecture rules.

## Documentation

- [Requirements (L1)](docs/specs/L1.md) and [detailed requirements (L2)](docs/specs/L2.md)
- [Detailed designs](docs/detailed-designs)
- [Estimates and roadmap](docs/estimates/README.md)

## Contributing

Contributions are welcome. Please read [CONTRIBUTING.md](CONTRIBUTING.md) and our [Code of Conduct](CODE_OF_CONDUCT.md) first. To report a vulnerability, see [SECURITY.md](SECURITY.md).

## License

Ezra is licensed under the [MIT License](LICENSE).
