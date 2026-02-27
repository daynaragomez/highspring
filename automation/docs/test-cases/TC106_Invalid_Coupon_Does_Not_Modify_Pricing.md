# Test Case Specification — TC106

## 1. Document Control
- Case ID: TC106
- Title: Invalid Coupon Does Not Modify Pricing
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: PricingE2ESuite
- Priority: Medium
- Linked Risks: R-005
- Related Catalog Entry: `test-case-catalog.md` (TC106)

## 3. Objective
Validate that an invalid coupon is rejected and does not alter cart pricing.

## 4. Preconditions
1. Baseline reset is successful.
2. Cart contains at least one item.

## 5. Test Data
- Invalid coupon code: `INVALIDCODE`

## 6. Procedure (Steps)
### Step 1 — Capture baseline pricing
- Action: Read subtotal/tax/total before coupon attempt.
- Expected Result: Baseline values captured.

### Step 2 — Apply invalid coupon
- Action: Submit invalid coupon code.
- Expected Result: Coupon is rejected.

### Step 3 — Compare pricing
- Action: Re-read pricing values.
- Expected Result: Pricing remains unchanged.

## 7. Expected Results (Final)
1. Invalid coupon does not create discount.
2. Pricing values are unchanged from baseline.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- None beyond normal session handling.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC106`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Invalid coupon is rejected and pricing is unchanged.
- Fail: Invalid coupon changes pricing or is incorrectly accepted.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
