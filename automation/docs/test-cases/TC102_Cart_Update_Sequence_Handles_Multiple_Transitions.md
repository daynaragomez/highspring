# Test Case Specification — TC102

## 1. Document Control
- Case ID: TC102
- Title: Cart Update Sequence Handles Multiple Transitions
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: CartE2ESuite
- Priority: High
- Linked Risks: R-004
- Related Catalog Entry: `test-case-catalog.md` (TC102)

## 3. Objective
Validate quantity transitions (increase/decrease) across multiple updates remain correct.

## 4. Preconditions
1. Baseline reset is successful.
2. Cart contains target item.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`
- Sequence: `1 -> 5 -> 2`

## 6. Procedure (Steps)
### Step 1 — Prepare initial cart item
- Action: Add SKU to cart.
- Expected Result: Quantity starts at `1`.

### Step 2 — Increase quantity
- Action: Update quantity to `5`.
- Expected Result: Cart reflects quantity `5`.

### Step 3 — Decrease quantity
- Action: Update quantity to `2`.
- Expected Result: Cart reflects quantity `2`.

### Step 4 — Validate total coherence
- Action: Verify summary values align to final quantity.
- Expected Result: Totals correspond to quantity `2`.

## 7. Expected Results (Final)
1. Update transitions are applied in order.
2. Final quantity and totals are coherent.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Reset cart/session if needed.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC102`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: All transitions and final totals are correct.
- Fail: Transition ignored, wrong final quantity, or totals mismatch.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
