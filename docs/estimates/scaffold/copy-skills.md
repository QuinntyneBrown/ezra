# Estimate — copy-skills

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/scaffold/copy-skills/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `SkillsSourceLocator` with per-OS default and override | 1 |
| `SkillCopier` discovery, recursive copy to both destinations, cancellation | 4 |
| Symbolic-link detection and warnings | 2 |
| `SafePath` and `UnsafePathException` mapping | 1 |
| Acceptance tests: nested skills, non-skill folders, missing source, override, links, path escape, timing | 6 |
| **Total** | **14** |

## Minimal scope

The minimal scope copies skills from the default or overridden source into both destinations and keeps `SafePath`. It satisfies L2-012 and L2-015 criteria 2 and 3, and defers symbolic-link skipping (L2-015 criterion 1) and the timing test (L2-017 criterion 2).

Minimal scope total: **10 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| Symbolic-link skipping | A linked file or folder in a skill is copied or fails the run | 3 |
| Timing acceptance test | L2-017 criterion 2 is verified by hand | 1 |
