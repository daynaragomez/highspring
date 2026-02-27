# Test Case Specification — TC110

## 1. Document Control
- Case ID: TC110
- Title: Checkout Clears Cart Post Order
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: CheckoutE2ESuite
- Priority: High
- Linked Risks: R-004
- Related Catalog Entry: `test-case-catalog.md` (TC110)

## 3. Objective
Validate that cart is empty after a successful order placement.

## 4. Preconditions
1. Baseline reset is successful.
2. Checkout can be completed successfully.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`

## 6. Procedure (Steps)
### Step 1 — Complete checkout
- Action: Add item and place order.
- Expected Result: Confirmation is displayed.

### Step 2 — Open cart after order
- Action: Navigate to `/cart`.
- Expected Result: Cart opens successfully.

### Step 3 — Validate empty cart
- Action: Check empty state.
- Expected Result: Cart has no active line items.

## 7. Expected Results (Final)
1. Successful checkout clears cart state.
2. User sees empty cart after order.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Standard session cleanup.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC110`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Cart is empty after successful order.
- Fail: Cart retains items post-checkout.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
