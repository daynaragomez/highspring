# Test Case Specification — TC107

## 1. Document Control
- Case ID: TC107
- Title: Pricing Rounding Is Currency Safe Across Stages
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: PricingE2ESuite
- Priority: High
- Linked Risks: R-001, R-002
- Related Catalog Entry: `test-case-catalog.md` (TC107)

## 3. Objective
Validate rounding consistency across subtotal, discount, tax, and total stages.

## 4. Preconditions
1. Baseline reset is successful.
2. Deterministic pricing inputs are used.

## 5. Test Data
- Deterministic SKU/coupon combinations for rounded values.

## 6. Procedure (Steps)
### Step 1 — Execute pricing scenario
- Action: Add configured items and apply coupon if required.
- Expected Result: Pricing summary is produced.

### Step 2 — Validate stage values
- Action: Compare each stage value against expected rounded values.
- Expected Result: Each stage matches currency-safe rounding expectation.

## 7. Expected Results (Final)
1. Rounding behavior is deterministic and consistent.
2. No stage introduces rounding drift.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Reset state if needed.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC107`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: All stage values align with expected rounding rules.
- Fail: Any stage value differs from expected currency-safe value.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
