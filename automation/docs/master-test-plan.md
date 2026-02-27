# Highspring Master Test Plan

## 1. Document Control
- Document: Master Test Plan
- Product: Highspring (Web + API + PostgreSQL)
- Scope: End-to-end user behavior validation through automated UI testing
- Owner: QA / Automation
- Status: Draft v1

## 2. Purpose
Define the overall quality strategy for validating critical ecommerce behavior in Highspring.

This plan establishes:
- what is tested,
- why it is tested,
- how quality risk is managed,
- when test execution is considered sufficient for release confidence.

## 3. Test Objectives
1. Detect critical regressions quickly after changes.
2. Validate core business behavior for cart, discount, tax, and checkout.
3. Ensure deterministic and repeatable execution across local and CI environments.
4. Provide clear pass/fail evidence for release decisions.

## 4. Test Scope
### In Scope
- User-facing flows:
  - Product discovery and add-to-cart
  - Cart updates/removals
  - Coupon application
  - Tax and total validation
  - Checkout completion
  - Confirmation and post-checkout cart state
- Integration validation between UI behavior and persisted order results through test-control endpoints where applicable.

### Out of Scope
- Performance/load testing
- Security/penetration testing
- Accessibility certification
- Cross-browser matrix expansion beyond current automation baseline

## 5. Quality Risks and Priorities
### 5.1 Risk Register
- **R-001 (High)**: Incorrect pricing totals (subtotal, discount, tax, grand total)
- **R-002 (High)**: Incorrect tax calculation logic (including GST/QST behavior)
- **R-003 (High)**: Checkout completion failures
- **R-004 (High)**: Cart state inconsistencies after mutation or checkout
- **R-005 (Medium)**: Coupon application/removal inconsistencies
- **R-006 (Medium)**: Intermittent UI synchronization issues
- **R-007 (Low)**: Cosmetic UI defects not affecting critical flows

### 5.2 Priority Assignment Rule
Test case priority is derived from linked risk(s) and business criticality:
- **High**: linked to any High risk (`R-001` to `R-004`) and/or release-critical behavior.
- **Medium**: linked only to Medium risks (`R-005`, `R-006`) without release-critical impact.
- **Low**: linked only to Low risk (`R-007`) and non-critical behavior.

If a case links to multiple risks, the highest linked risk defines the case priority.

## 6. Test Levels and Coverage Model
### Smoke Coverage
Purpose: fast confidence gate for release-critical behavior.

Characteristics:
- Short runtime
- Minimal scenario depth
- Highest business criticality

### E2E Coverage
Purpose: deeper validation of business behavior and system flow integrity.

Characteristics:
- Broader scenario depth
- Richer validation and cross-checks
- May include slower, multi-step workflows

## 7. Test Environment Strategy
- Primary environment: Docker-based local/CI stack
  - Web UI: `http://localhost:8080`
  - API: `http://localhost:8081`
  - Database: PostgreSQL
- Test-control API is used in non-production contexts for deterministic setup and assertions.
- Environment must be healthy before execution (`/health` endpoints).

## 8. Test Data Strategy
- Baseline deterministic seed is the default starting point.
- Each scenario must define required data preconditions explicitly.
- Test data setup/reset is controlled to avoid cross-test coupling.
- No scenario relies on residual state from previous test execution.

## 9. Test Design Governance
- Every automated scenario must originate from an approved test case definition.
- Each case must include:
  - Preconditions
  - Steps
  - Expected results
  - Postconditions
  - Priority and classification (Smoke or E2E)
  - Linked Risk ID(s) from Section 5.1
- Traceability must be maintained from requirement/behavior to test case to execution result.

## 10. Entry and Exit Criteria
### Entry Criteria
- Required services are running and healthy.
- Deterministic baseline reset is available.
- Latest test definitions are approved.

### Exit Criteria (Smoke)
- All Smoke tests pass.
- No unresolved high-severity defects in critical paths.

### Exit Criteria (E2E)
- All mandatory E2E tests pass.
- No unresolved high-severity defects in business-critical workflows.
- Failures are triaged with root-cause status.

## 11. Defect Management and Triage
- Failed tests are triaged by severity and reproducibility.
- Defect severity guidance:
  - Sev1: blocks checkout or critical transaction correctness
  - Sev2: major business-rule mismatch with workaround
  - Sev3: non-critical functional issue
- Flaky tests are tracked separately from product defects and must have remediation actions.

## 12. Observability and Evidence
- Execution evidence includes:
  - Test run summary (pass/fail)
  - Per-test diagnostic logs
  - Correlation IDs linking run and case execution
- Logs are retained under `automation/TestResults/logs/` for investigation.

## 13. Execution Cadence
- Change validation: run Smoke on relevant changes.
- Pull request validation: run Smoke + selected/required E2E.
- Scheduled regression: run E2E suite on cadence (for example nightly).
- Manual targeted runs: by suite/case during debugging.

## 14. Roles and Responsibilities
- QA/Automation:
  - Own test design, case quality, automation behavior, and triage support.
- Developers:
  - Support root-cause analysis and fixes for failing critical behaviors.
- Release owner:
  - Makes release decision based on defined entry/exit criteria and risk status.

## 15. Deliverables
1. Master Test Plan (this document)
2. Test Design Specification (suite and case catalog)
3. Test Case Specifications (detailed case definitions)
4. Automated execution reports and logs
5. Defect triage records for failed runs

## 16. Assumptions and Constraints
- Deterministic test-control endpoints are available in test environments.
- Stable selectors are maintained for UI automation.
- Execution time and infrastructure capacity may constrain full E2E frequency.

## 17. Approval
This plan becomes active after QA and engineering sign-off.
Any change to scope, quality gates, or exit criteria requires update and re-approval.
