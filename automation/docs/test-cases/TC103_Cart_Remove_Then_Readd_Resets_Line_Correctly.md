# Test Case Specification — TC103

## 1. Document Control
- Case ID: TC103
- Title: Cart Remove Then Readd Resets Line Correctly
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: CartE2ESuite
- Priority: High
- Linked Risks: R-004
- Related Catalog Entry: `test-case-catalog.md` (TC103)

## 3. Objective
Validate cart state after remove and re-add sequence for same SKU.

## 4. Preconditions
1. Baseline reset is successful.
2. SKU is in stock.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`

## 6. Procedure (Steps)
### Step 1 — Add item
- Action: Add SKU to cart.
- Expected Result: Line exists with quantity `1`.

### Step 2 — Remove item
- Action: Remove cart line.
- Expected Result: Cart becomes empty.

### Step 3 — Re-add item
- Action: Add same SKU again.
- Expected Result: Line exists with quantity reset to `1`.

## 7. Expected Results (Final)
1. Remove operation fully clears previous line state.
2. Re-add starts clean at default quantity.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Reset state if suite isolation requires.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC103`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Re-added line quantity resets correctly.
- Fail: Old state leaks into re-added line.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
