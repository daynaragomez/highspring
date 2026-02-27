# Test Case Specification — TC105

## 1. Document Control
- Case ID: TC105
- Title: Take5 Discount Applies Before Tax For CA QC
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: PricingE2ESuite
- Priority: High
- Linked Risks: R-001, R-002, R-005
- Related Catalog Entry: `test-case-catalog.md` (TC105)

## 3. Objective
Verify TAKE5 fixed discount is applied before GST/QST calculations in CA/QC context.

## 4. Preconditions
1. Baseline reset is successful.
2. `TAKE5` coupon is active.

## 5. Test Data
- SKU: `HOODIE-CLASSIC`
- Coupon: `TAKE5`

## 6. Procedure (Steps)
### Step 1 — Add item and apply TAKE5
- Action: Add item and apply fixed coupon.
- Expected Result: Discount appears in summary.

### Step 2 — Validate totals
- Action: Validate subtotal, discount, tax, total.
- Expected Result: Values follow discount-before-tax rule.

## 7. Expected Results (Final)
1. Fixed discount is applied correctly.
2. Tax is computed from discounted base.
3. Total is deterministic and consistent.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Reset state if required by suite policy.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC105`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: All summary values match expected fixed-discount behavior.
- Fail: Discount or tax calculations are incorrect.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
