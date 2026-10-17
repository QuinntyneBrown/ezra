# Estimate — scaffold-project

## Basis

Estimated for an intermediate C#/.NET developer who knows `dotnet` tooling and dependency injection but has not used `System.CommandLine` 2.0 and does not know this codebase. Hours cover implementation, xUnit acceptance tests and review fixes. [Detailed design](../../detailed-designs/scaffold/scaffold-project/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `ScaffoldModule`, `ScaffoldCommand` options and `ScaffoldOptions` binding | 3 |
| `ScaffoldCommandHandler` orchestration and summary output | 2 |
| `ProjectNameValidator` with reserved device names | 2 |
| `TargetFolderGuard` and `--force` semantics | 2 |
| `ScaffoldTransaction` rollback on failure and cancellation | 3 |
| Acceptance tests: happy path, `--output`, invalid backend, names, non-empty folder, `--force`, rollback | 7 |
| **Total** | **19** |

## Minimal scope

The minimal scope ships the command, name validation and the non-empty-folder refusal without `--force`. It satisfies L2-009, L2-013 and L2-014 criteria 1 and 3, and defers `--force` and the rollback in L2-016.

Minimal scope total: **12 hours**.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| `--force` | Re-scaffolding an existing folder requires deleting it first | 2 |
| `ScaffoldTransaction` rollback | A failed run can leave a partial project | 4 |
