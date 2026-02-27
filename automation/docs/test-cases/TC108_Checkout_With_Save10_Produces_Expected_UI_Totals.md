# Test Case Specification — TC108

## 1. Document Control
- Case ID: TC108
- Title: Checkout With Save10 Produces Expected UI Totals
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: CheckoutE2ESuite
- Priority: High
- Linked Risks: R-001, R-003, R-006
- Related Catalog Entry: `test-case-catalog.md` (TC108)

## 3. Objective
Validate checkout totals on UI when SAVE10 is applied.

## 4. Preconditions
1. Baseline reset is successful.
2. Cart contains target SKU.
3. SAVE10 is valid.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`
- Coupon: `SAVE10`

## 6. Procedure (Steps)
### Step 1 — Build cart with coupon
- Action: Add SKU and apply SAVE10 in cart.
- Expected Result: Updated summary values are shown.

### Step 2 — Continue to checkout
- Action: Navigate to checkout page.
- Expected Result: Checkout summary matches cart summary.

### Step 3 — Submit checkout
- Action: Fill required fields and place order.
- Expected Result: Redirect to confirmation.

### Step 4 — Validate confirmation totals
- Action: Validate totals on confirmation UI.
- Expected Result: Values match expected deterministic scenario.

## 7. Expected Results (Final)
1. SAVE10 totals are consistent cart → checkout → confirmation.
2. Checkout completes successfully.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Ensure session cleanup per policy.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC108`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Expected totals remain consistent through full checkout flow.
- Fail: Totals mismatch or checkout flow breaks.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
