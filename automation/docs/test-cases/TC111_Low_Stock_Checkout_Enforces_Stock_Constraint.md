# Test Case Specification — TC111

## 1. Document Control
- Case ID: TC111
- Title: Low Stock Checkout Enforces Stock Constraint
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: CheckoutE2ESuite
- Priority: High
- Linked Risks: R-003
- Related Catalog Entry: `test-case-catalog.md` (TC111)

## 3. Objective
Validate stock constraint enforcement during checkout when requested quantity exceeds available stock.

## 4. Preconditions
1. Baseline reset is successful.
2. Test-control endpoint can set stock quantity.
3. Cart can be prepared with quantity above stock threshold.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`
- Stock quantity: `1`
- Requested quantity: `2`

## 6. Procedure (Steps)
### Step 1 — Set low stock
- Action: Set product stock to constrained value.
- Expected Result: Stock update succeeds.

### Step 2 — Prepare over-limit cart
- Action: Attempt checkout with quantity above stock.
- Expected Result: Checkout submission is blocked or fails gracefully.

### Step 3 — Validate constraint behavior
- Action: Validate user-facing error/blocked completion.
- Expected Result: Order is not placed beyond stock.

## 7. Expected Results (Final)
1. Stock constraints are enforced.
2. Checkout does not complete invalid quantity order.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Restore/reset stock state for isolation.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC111`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Over-limit checkout is rejected and no invalid order is created.
- Fail: Checkout succeeds despite stock violation.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
