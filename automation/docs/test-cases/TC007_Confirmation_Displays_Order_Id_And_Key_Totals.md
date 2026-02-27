# Test Case Specification — TC007

## 1. Document Control
- Case ID: TC007
- Title: Confirmation Displays Order Id And Key Totals
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: Smoke
- Suite: CheckoutSmokeSuite
- Priority: High
- Linked Risks: R-001 (Incorrect pricing totals), R-006 (UI synchronization issues)
- Related Catalog Entry: `test-case-catalog.md` (TC007)

## 3. Objective
Validate that confirmation page shows a non-empty order identifier and key pricing totals.

## 4. Preconditions
1. Successful checkout has been submitted.
2. Confirmation route is reached.

## 5. Test Data
- Checkout generated order context

## 6. Procedure (Steps)
### Step 1 — Open confirmation state
- Action: Reach confirmation page from checkout.
- Expected Result: Confirmation page loads.

### Step 2 — Validate order ID
- Action: Read order confirmation id.
- Expected Result: Non-empty ID is visible.

### Step 3 — Validate key totals
- Action: Validate subtotal, discount, tax, total fields are present and numeric.
- Expected Result: All key totals are displayed.

## 7. Expected Results (Final)
1. Order identifier is visible and non-empty.
2. Key totals are present and coherent.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- None beyond standard session handling.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC007`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Order ID and key totals are visible and valid.
- Fail: Missing order ID, missing totals, or malformed values.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
