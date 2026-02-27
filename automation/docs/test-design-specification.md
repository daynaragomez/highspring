# Highspring Test Design Specification

## 1. Document Control
- Document: Test Design Specification
- Product: Highspring
- Related document: `master-test-plan.md`
- Owner: QA / Automation
- Status: Draft v1

## 2. Purpose
Define the complete automation test design that implements the Master Test Plan.

This specification defines:
- test architecture,
- suite model,
- test case governance model,
- case classification,
- traceability and execution metadata.

This document is intentionally independent from current code layout and represents the desired target design.

## 3. Design Principles
1. **Business-first coverage**: prioritize critical user and pricing behavior.
2. **Deterministic execution**: scenarios start from known state.
3. **Separation of concerns**: orchestration, case behavior, and shared UI actions are separated.
4. **Traceability**: every test case has unique ID and clear requirement mapping.
5. **Fast feedback + depth**: Smoke for rapid confidence, E2E for deep validation.

## 4. Coverage Dimensions
The design covers behavior across these dimensions:
- Navigation and discoverability
- Cart state mutation
- Discount and pricing rules
- Tax calculation behavior
- Checkout workflow
- Confirmation and post-order state
- Data integrity cross-checks (UI vs persisted snapshot)
- Negative and edge conditions (invalid coupon, stock constraints)

## 5. Test Architecture Model
Hierarchy:
1. **Type**: `Smoke` or `E2E`
2. **Suite**: business domain grouping
3. **Case**: single executable scenario

Execution contract:
- Each suite contains one or more related cases.
- Each case has one unique ID.
- Case classification is fixed in this design unless explicitly revised.
- Case automation status is managed by QA/Automation approval (Candidate, Approved, Deferred).

## 6. Suite Catalog
### 6.1 Smoke Suites
1. `HomeSmokeSuite`
2. `ProductsSmokeSuite`
3. `CartSmokeSuite`
4. `CheckoutSmokeSuite`

### 6.2 E2E Suites
1. `CartE2ESuite`
2. `PricingE2ESuite`
3. `CheckoutE2ESuite`
4. `OrderSnapshotE2ESuite`

## 7. Test Case Catalog
Authoritative source: `test-case-catalog.md`

This specification governs the model; the catalog document stores case inventory and lifecycle status.

## 8. Traceability Matrix (Behavior → Cases)
- Add to cart: `TC003`, `TC009`, `TC101`
- Update quantity: `TC004`, `TC102`
- Remove item: `TC005`, `TC103`
- Apply coupon discount: `TC104`, `TC105`, `TC106`
- Validate tax calculation: `TC104`, `TC105`, `TC107`, `TC113`
- Complete checkout: `TC006`, `TC108`, `TC109`, `TC111`
- Validate confirmation totals: `TC007`, `TC108`, `TC109`, `TC112`
- Cart cleared post-checkout: `TC110`
- Product discovery/navigation: `TC001`, `TC002`, `TC008`

## 9. Case Metadata Standard
Each case must declare:
- Case ID
- Type (`Smoke|E2E`)
- Suite name
- Priority (`High|Medium|Low`)
- Linked Risk ID(s) from `master-test-plan.md` Section 5.1
- Determinism level (`Deterministic` expected by default)
- Requirement mapping / behavior mapping

Priority must follow the risk rule from `master-test-plan.md` Section 5.2.

## 9.1 Risk-to-Case Traceability Matrix
- **R-001 Incorrect pricing totals** → `TC007`, `TC104`, `TC105`, `TC107`, `TC108`, `TC109`, `TC112`
- **R-002 Incorrect tax calculation logic** → `TC104`, `TC105`, `TC107`, `TC113`
- **R-003 Checkout completion failures** → `TC006`, `TC108`, `TC109`, `TC111`
- **R-004 Cart state inconsistencies** → `TC003`, `TC004`, `TC005`, `TC009`, `TC101`, `TC102`, `TC103`, `TC110`
- **R-005 Coupon apply/remove inconsistencies** → `TC104`, `TC105`, `TC106`
- **R-006 UI synchronization issues** → `TC001`, `TC002`, `TC006`, `TC007`, `TC108`, `TC109`
- **R-007 Cosmetic UI defects** → Not in automation-critical scope for current catalog

## 10. Preconditions and Data Baselines
Baseline preconditions for most cases:
1. Application services healthy.
2. Deterministic baseline reset successful.
3. Session isolation enabled for scenario execution.

Case-level preconditions (example categories):
- stock overrides,
- preloaded cart state,
- coupon state.

## 11. Expected Observability at Case Level
Each case execution must provide:
- Case start and end events,
- Step-level progress events,
- Assertion-level outcomes,
- Correlation IDs for run and case,
- Failure diagnostics with context.

## 12. Execution Policy
### 12.1 Candidate Catalog
The case catalog in this document is the candidate universe and does not imply mandatory automation of every case.

### 12.2 Mandatory Release Fast Gate
Run the **approved automated subset** of Smoke cases that are High risk and High priority.

### 12.3 Scheduled Deep Validation
Run the **approved automated subset** of E2E cases based on risk coverage goals and release needs.

### 12.4 Targeted Diagnostic Runs
Run selected suites/cases by priority, risk, and investigation needs.

## 12.5 Automation Selection Workflow
1. Catalog case is created with `Status=Candidate`.
2. Initial triage sets automation intent based on risk and priority.
3. Test Case Specification is authored.
4. QA/Automation review finalizes state: `Approved` or `Deferred`.
5. Any state change must be recorded in `automation-decision-log.md`.

Required decision log fields:
- Case ID
- From Status
- To Status
- Decision Date
- Decision By
- Decision Reason
- Target Release

## 13. Design Constraints
- Keep tests independent and order-agnostic.
- Keep scenario setup explicit inside each case design.

## 14. Deliverables from This Specification
1. Test Case Specification documents for each catalog case.
2. Automation implementation for the approved subset of catalog cases.
3. Execution reports linked to case IDs.
4. Persistent diagnostic logs linked to case executions.
5. Decision history in `automation-decision-log.md`.

## 15. Change Management
Any modification to:
- suite list,
- case IDs,
- case classification,
- priority,
requires updating this specification and re-approval by QA/Automation owner.
