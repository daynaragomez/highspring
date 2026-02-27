# Highspring — QA Challenge Submission

[![Automation Tests (main)](https://github.com/daynaragomez/highspring/actions/workflows/automation-tests.yml/badge.svg?branch=main)](https://github.com/daynaragomez/highspring/actions/workflows/automation-tests.yml?query=branch%3Amain)

This repository is a **test-first QA challenge submission** for the ecommerce cart + checkout journey.

In this submission you will find:
- End-to-end test design artifacts with case prioritization and rationale.
- Implemented automation for the required cart/checkout scenarios.
- Reproducible execution output via trait-based runs (`Type`, `Suite`, `Case`) and persisted logs.

 ---

 ## Challenge Deliverables (Mapped)

 ### 1) Test Design Document

 Core design artifacts:
 - [automation/docs/master-test-plan.md](automation/docs/master-test-plan.md)
 - [automation/docs/test-design-specification.md](automation/docs/test-design-specification.md)
 - [automation/docs/test-case-catalog.md](automation/docs/test-case-catalog.md)
 - [automation/docs/automation-decision-log.md](automation/docs/automation-decision-log.md)

 Detailed case specifications:
 - [automation/docs/test-cases](automation/docs/test-cases)

 These documents cover:
 - Candidate/approved/deferred lifecycle.
 - Automation prioritization rationale (risk, business value, release impact).
 - Per-case structure: preconditions, steps, expected results, postconditions, cleanup.

 ### 2) Automation Code

 Framework:
 - .NET + Selenium + xUnit

 Automation project:
 - [automation/Highspring.Automation.csproj](automation/Highspring.Automation.csproj)

 Architecture implemented:
 - Suite orchestration base: [automation/Tests/BaseCaseSuiteTest.cs](automation/Tests/BaseCaseSuiteTest.cs)
 - Case behavior base: [automation/Tests/BaseTestCase.cs](automation/Tests/BaseTestCase.cs)
 - One case per file (`TCxxx_...`) under `Smoke/Cases` and `E2E/Cases`
 - Suite-level execution intent under `Smoke/Suites` and `E2E/Suites`
 - Traits for filtering:
  - `Type=Smoke|E2E`
  - `Suite=<domain>`
  - `Case=<TC-ID>`

 ### 3) Test Execution Output

 Evidence sources:
 - Console test summaries from `dotnet test`
 - Per-case persisted logs under:
  - `automation/TestResults/logs/`

 ### 4) Optional Bonus

 Included:
 - CI workflow with smoke/e2e cadence:
  - [.github/workflows/automation-tests.yml](.github/workflows/automation-tests.yml)

 Not yet included:
 - Data-driven permutations across multiple products/quantities at scale (can be added next).

 ---

 ## Requirement Coverage Matrix

 Required challenge behaviors and current automated coverage:

 1. **Adding a product to the cart**
   - Case: `TC003`
   - Suite: [automation/Tests/Smoke/Suites/ProductsSuiteSmoke.cs](automation/Tests/Smoke/Suites/ProductsSuiteSmoke.cs)
   - Implementation: [automation/Tests/Smoke/Cases/TC003_Add_To_Cart_From_Products_Creates_Cart_Line.cs](automation/Tests/Smoke/Cases/TC003_Add_To_Cart_From_Products_Creates_Cart_Line.cs)

 2. **Removing a product from the cart**
   - Case: `TC005`
   - Suite: [automation/Tests/Smoke/Suites/CartSuiteSmoke.cs](automation/Tests/Smoke/Suites/CartSuiteSmoke.cs)
   - Implementation: [automation/Tests/Smoke/Cases/TC005_Remove_Cart_Line_Shows_Empty_State.cs](automation/Tests/Smoke/Cases/TC005_Remove_Cart_Line_Shows_Empty_State.cs)

 3. **Updating product quantity**
   - Case: `TC004`
   - Suite: [automation/Tests/Smoke/Suites/CartSuiteSmoke.cs](automation/Tests/Smoke/Suites/CartSuiteSmoke.cs)
   - Implementation: [automation/Tests/Smoke/Cases/TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total.cs](automation/Tests/Smoke/Cases/TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total.cs)

 4. **Proceeding to checkout with valid details**
   - Case: `TC006`
   - Suite: [automation/Tests/Smoke/Suites/CheckoutSuiteSmoke.cs](automation/Tests/Smoke/Suites/CheckoutSuiteSmoke.cs)
   - Implementation: [automation/Tests/Smoke/Cases/TC006_Checkout_Submission_Redirects_To_Confirmation.cs](automation/Tests/Smoke/Cases/TC006_Checkout_Submission_Redirects_To_Confirmation.cs)

 5. **Attempting checkout with missing/invalid details**
   - Case: `TC010`
   - Suite: [automation/Tests/Smoke/Suites/CheckoutSuiteSmoke.cs](automation/Tests/Smoke/Suites/CheckoutSuiteSmoke.cs)
   - Implementation: [automation/Tests/Smoke/Cases/TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation.cs](automation/Tests/Smoke/Cases/TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation.cs)

 Note: “invalid payment” is not represented in the current mock UI/domain model; the invalid-checkout requirement is covered via invalid/missing required checkout details.

 ---

 ## How to Run (Reviewer Quick Path)

 ### Start app stack

 ```bash
 docker compose up -d --build
 ```

 Endpoints:
 - Web: `http://localhost:8080`
 - API: `http://localhost:8081`

 ### Build automation

 ```bash
 dotnet build automation/Highspring.Automation.csproj
 ```

 ### Run by execution type

 ```bash
 # Smoke gate
 dotnet test automation/Highspring.Automation.csproj --filter "Type=Smoke"

 # E2E regression
 dotnet test automation/Highspring.Automation.csproj --filter "Type=E2E"
 ```

 ### Run a single case

 ```bash
 dotnet test automation/Highspring.Automation.csproj --filter "Case=TC006"
 ```

 ### Headed (non-headless) run

 ```bash
 HIGHSPRING_HEADLESS=false dotnet test automation/Highspring.Automation.csproj --filter "Type=Smoke"
 ```

 ---

 ## Test Stability Notes

 - Tests are configured to run sequentially at assembly level to avoid shared-state reset collisions.
 - Baseline reset endpoint used by tests:
  - `POST /internal/test/v1/reset`
 - Stable selector strategy uses `data-testid` values from the web app.

 ---

 ## Reviewer Navigation

If you review only three things, start here:
- [automation/docs/automation-decision-log.md](automation/docs/automation-decision-log.md)
- [automation/Tests/Smoke/Suites](automation/Tests/Smoke/Suites)
- [automation/Tests/E2E/Suites](automation/Tests/E2E/Suites)
