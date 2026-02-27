# Test Case Specification — TC112

## 1. Document Control
- Case ID: TC112
- Title: Order Snapshot Matches Confirmation Totals
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: OrderSnapshotE2ESuite
- Priority: High
- Linked Risks: R-001
- Related Catalog Entry: `test-case-catalog.md` (TC112)

## 3. Objective
Validate persisted order snapshot totals match UI confirmation totals.

## 4. Preconditions
1. Successful checkout has produced an order ID.
2. Test-control order snapshot endpoint is reachable.

## 5. Test Data
- Order ID from confirmation page

## 6. Procedure (Steps)
### Step 1 — Complete checkout and capture order ID
- Action: Place order and read confirmation order ID.
- Expected Result: Non-empty order ID captured.

### Step 2 — Query order snapshot API
- Action: Request `/internal/test/v1/orders/{orderId}`.
- Expected Result: Snapshot response returned.

### Step 3 — Compare totals
- Action: Compare snapshot totals to confirmation UI totals.
- Expected Result: Subtotal/discount/tax/grand total match.

## 7. Expected Results (Final)
1. Persisted totals equal UI totals.
2. No mismatch between displayed and stored pricing values.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Standard cleanup only.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC112`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: Snapshot totals exactly match confirmation totals.
- Fail: Any total field mismatch.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
