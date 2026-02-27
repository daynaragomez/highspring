# Automation Conventions

This folder contains Selenium UI automation for Highspring using C# + xUnit.

## Architecture

- `Config/` → environment and test settings
- `Core/` → driver lifecycle, waits, base test class
- `Selectors/` → centralized `data-testid` constants
- `Pages/` → page object model (UI interactions only)
- `Flows/` → reusable business journeys across pages
- `Api/` → test-control API client for deterministic setup
- `Tests/Smoke` → fast deploy gate scenarios
- `Tests/E2E` → business-flow validations

## Naming Conventions

- File name matches class name.
- Test classes end with `Tests`.
- Test methods use `Action_Condition_ExpectedResult` style.
- Category traits:
  - `[Trait("Category", "Smoke")]`
  - `[Trait("Category", "E2E")]`

## Test Writing Rules

- Keep UI selectors in `Selectors/TestIds.cs`.
- Keep page interactions in `Pages/*` and avoid business assertions there.
- Keep reusable journey steps in `Flows/*`.
- Keep test assertions inside `Tests/*`.
- Avoid `Thread.Sleep`; use explicit waits from `Core/Waits.cs`.

## Local Execution

```bash
# Smoke only
dotnet test automation/Highspring.Automation.csproj --filter "Category=Smoke"

# E2E only
dotnet test automation/Highspring.Automation.csproj --filter "Category=E2E"

# All tests
dotnet test automation/Highspring.Automation.csproj

# Headed mode (visible browser)
HIGHSPRING_HEADLESS=false dotnet test automation/Highspring.Automation.csproj --filter "Category=Smoke"
```

## CI Cadence

Workflow: `.github/workflows/automation-tests.yml`

- Push (`demo-app/**`, `automation/**`) → Smoke
- Pull Request (`demo-app/**`, `automation/**`) → Smoke + E2E
- Nightly schedule → E2E
- Manual dispatch → `all`, `smoke`, or `e2e`

## PR Checklist

Use this checklist for any automation PR:

- [ ] New/changed selectors are centralized in `Selectors/TestIds.cs`.
- [ ] Page interactions are implemented in `Pages/*` (not directly in tests).
- [ ] Reusable user journeys are placed in `Flows/*` when needed.
- [ ] Test methods are tagged with the correct category (`Smoke` or `E2E`).
- [ ] No `Thread.Sleep` added; explicit waits are used.
- [ ] Local verification done with relevant filters:
  - `dotnet test ... --filter "Category=Smoke"`
  - `dotnet test ... --filter "Category=E2E"` (if flow/business logic changed)
- [ ] Assertions validate business outcome, not only navigation/UI presence.
