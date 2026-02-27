# Test Case Specification — TC109

## 1. Document Control
- Case ID: TC109
- Title: Checkout Without Coupon Produces Expected UI Totals
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: CheckoutE2ESuite
- Priority: High
- Linked Risks: R-001, R-003, R-006
- Related Catalog Entry: `test-case-catalog.md` (TC109)

## 3. Objective
Validate baseline checkout totals on UI without any coupon applied.

## 4. Preconditions
1. Baseline reset is successful.
2. Cart contains test item(s).

## 5. Test Data
- SKU: `HOODIE-CLASSIC`
- Coupon: none

## 6. Procedure (Steps)
### Step 1 — Build cart without coupon
- Action: Add item(s) to cart only.
- Expected Result: Cart summary is shown with no discount.

### Step 2 — Proceed to checkout and submit
- Action: Fill checkout form and submit.
- Expected Result: Redirect to confirmation route.

### Step 3 — Validate totals
- Action: Validate subtotal/discount/tax/total values.
- Expected Result: Values match baseline deterministic expectations.

## 7. Expected Results (Final)
1. Checkout succeeds without coupon.
2. Totals are correct and consistent across flow.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Cleanup session/state per suite policy.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC109`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Baseline totals are correct through checkout.
- Fail: Totals mismatch or checkout cannot complete.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
