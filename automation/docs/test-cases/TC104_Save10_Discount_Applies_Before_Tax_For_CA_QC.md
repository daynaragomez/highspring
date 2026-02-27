# Test Case Specification — TC104

## 1. Document Control
- Case ID: TC104
- Title: Save10 Discount Applies Before Tax For CA QC
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: PricingE2ESuite
- Priority: High
- Linked Risks: R-001, R-002, R-005
- Related Catalog Entry: `test-case-catalog.md` (TC104)

## 3. Objective
Verify SAVE10 percent discount is applied before GST/QST tax calculation for CA/QC context.

## 4. Preconditions
1. Baseline reset is successful.
2. Target SKU and coupon `SAVE10` are active.

## 5. Test Data
- SKU: `HOODIE-CLASSIC` (49.00)
- Coupon: `SAVE10`
- Region: `CA/QC`

## 6. Procedure (Steps)
### Step 1 — Add item and apply coupon
- Action: Add SKU then apply `SAVE10`.
- Expected Result: Coupon accepted and pricing recalculates.

### Step 2 — Validate pricing values
- Action: Validate subtotal, discount, tax, total.
- Expected Result: Values match expected baseline for rule discount-before-tax.

### Step 3 — Validate tax basis
- Action: Confirm tax reflects discounted taxable base.
- Expected Result: Tax is computed after discount application.

## 7. Expected Results (Final)
1. Discount applied correctly.
2. Tax computed from discounted base.
3. Grand total matches expected deterministic values.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Reset state if required.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC104`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Pricing values match expected discount-before-tax behavior.
- Fail: Discount/tax ordering or totals are incorrect.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
