# Test Case Specification — TC004

## 1. Document Control
- Case ID: TC004
- Title: Cart Quantity Update Reflects In Line And Total
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: Smoke
- Suite: CartSmokeSuite
- Priority: High
- Linked Risks: R-004 (Cart state inconsistencies)
- Related Catalog Entry: `test-case-catalog.md` (TC004)

## 3. Objective
Validate that changing quantity in cart updates both line quantity and pricing totals coherently.

## 4. Preconditions
1. Services are healthy.
2. Baseline reset is successful.
3. Cart contains at least one line item.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`
- Quantity transition: `1 -> 2`

## 6. Procedure (Steps)
### Step 1 — Seed cart state
- Action: Add product to cart from products page.
- Expected Result: Cart contains one line with quantity `1`.

### Step 2 — Update quantity
- Action: Set quantity input to `2` and update.
- Expected Result: Cart line shows quantity `2`.

### Step 3 — Validate totals
- Action: Read summary values after update.
- Expected Result: Totals are recalculated and coherent with new quantity.

## 7. Expected Results (Final)
1. Quantity update is applied successfully.
2. Summary reflects updated cart state.
3. No inconsistency between line quantity and totals.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Reset state if required by suite isolation policy.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC004`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Updated quantity and totals are correct.
- Fail: Quantity does not update or totals are inconsistent.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
