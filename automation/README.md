# Automation Conventions

This folder contains Selenium UI automation for DagoShopFlow using C# + xUnit.

## Current Architecture

- `Config/` → environment and runtime test settings
- `Core/` → driver lifecycle, waits, file logger provider
- `Api/` → test-control client for deterministic setup/assertions
- `Selectors/` → centralized `data-testid` constants
- `Pages/` → page object model (UI interactions only)
- `Tests/BaseTestCase.cs` → shared case helpers (scoped logging, common assertions)
- `Tests/BaseCaseSuiteTest.cs` → shared suite orchestration (`RunCase`)
- `Tests/Smoke/Cases` → one smoke test case per file (`TCxxx_...`)
- `Tests/Smoke/Suites` → orchestration-only smoke suites
- `Tests/E2E/Cases` → one e2e test case per file (`TCxxx_...`)
- `Tests/E2E/Suites` → orchestration-only e2e suites

## Test Organization Rules

- One automated case = one case class file (`TCxxx_...`).
- Suite methods orchestrate case lifecycle only:
  - `Preconditions`
  - `Step1..StepN`
  - `ValidateExpectedResults`
  - `ApplyPostconditions`
  - `ApplyFinallyCleanup`
- Business assertions live in case classes, not in suite infrastructure.
- Page classes should not contain business assertions.

## Trait Model (Filtering)

The active trait model is:
- `[Trait("Type", "Smoke")]` or `[Trait("Type", "E2E")]`
- `[Trait("Suite", "<domain>")]`
- `[Trait("Case", "TCxxx")]`

`Category` traits are deprecated in this repo.

## Logging & Evidence

- Each case execution receives:
  - `RunId`
  - `CaseExecutionId`
- Logs are persisted to:
  - `automation/TestResults/logs/`
- Test execution is configured to run sequentially at assembly level to avoid shared reset collisions.

## Local Execution

```bash
# Smoke only
dotnet test automation/Highspring.Automation.csproj --filter "Type=Smoke"

# E2E only
dotnet test automation/Highspring.Automation.csproj --filter "Type=E2E"

# Single case
dotnet test automation/Highspring.Automation.csproj --filter "Case=TC006"

# All tests
dotnet test automation/Highspring.Automation.csproj

# Headed mode (visible browser)
HIGHSPRING_HEADLESS=false dotnet test automation/Highspring.Automation.csproj --filter "Type=Smoke"
```

## CI Cadence

Workflow: `.github/workflows/automation-tests.yml`

- Push (`demo-app/**`, `automation/**`) → Smoke
- Pull Request (`demo-app/**`, `automation/**`) → Smoke + E2E
- Nightly schedule → E2E
- Manual dispatch → `all`, `smoke`, or `e2e`

## PR Checklist

- [ ] New/changed selectors are centralized in `Selectors/TestIds.cs`.
- [ ] Page interactions are implemented in `Pages/*` (not directly in suites).
- [ ] New automation case has a dedicated `TCxxx_...` case file.
- [ ] Suite method is orchestration-only and tagged with `Type`, `Suite`, `Case`.
- [ ] No `Thread.Sleep` added; explicit waits are used.
- [ ] Local verification done with relevant filters:
  - `dotnet test ... --filter "Type=Smoke"`
  - `dotnet test ... --filter "Type=E2E"` (if flow/business logic changed)
- [ ] Decision log and case documentation are updated when scope/status changes.
