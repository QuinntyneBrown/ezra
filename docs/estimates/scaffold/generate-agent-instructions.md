# Estimate — generate-agent-instructions

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/scaffold/generate-agent-instructions/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `Templates/AGENTS.md` embedded template with standard rule sections | 1 |
| `TemplateLoader` | 1 |
| `AgentInstructionsGenerator` with token replacement and both writes | 2 |
| Acceptance tests for heading, tech stack, standard sections and `CLAUDE.md` content | 2 |
| **Total** | **6** |

## Minimal scope

The full design is already the minimal scope; it satisfies L2-010 and L2-011.

Minimal scope total: **6 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| None | Every item is required by L2-010 or L2-011 | 0 |
