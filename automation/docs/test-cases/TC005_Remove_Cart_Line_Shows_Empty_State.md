# Test Case Specification — TC005

## 1. Document Control
- Case ID: TC005
- Title: Remove Cart Line Shows Empty State
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: Smoke
- Suite: CartSmokeSuite
- Priority: High
- Linked Risks: R-004 (Cart state inconsistencies)
- Related Catalog Entry: `test-case-catalog.md` (TC005)

## 3. Objective
Validate that removing the only cart line transitions cart to empty state.

## 4. Preconditions
1. Services are healthy.
2. Baseline reset is successful.
3. Cart has one line item.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`

## 6. Procedure (Steps)
### Step 1 — Prepare one-line cart
- Action: Add one item to cart.
- Expected Result: Cart shows one line.

### Step 2 — Remove line
- Action: Trigger remove action for the line item.
- Expected Result: Line is removed.

### Step 3 — Validate empty state
- Action: Verify empty cart message/state.
- Expected Result: Cart shows empty state.

## 7. Expected Results (Final)
1. Item removal works.
2. Cart transitions to empty state correctly.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Ensure clean state for next case.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC005`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Cart line removed and empty state shown.
- Fail: Line remains or empty state is incorrect.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
