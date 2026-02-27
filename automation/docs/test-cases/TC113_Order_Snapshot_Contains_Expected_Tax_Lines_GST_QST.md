# Test Case Specification — TC113

## 1. Document Control
- Case ID: TC113
- Title: Order Snapshot Contains Expected Tax Lines GST QST
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: E2E
- Suite: OrderSnapshotE2ESuite
- Priority: High
- Linked Risks: R-002
- Related Catalog Entry: `test-case-catalog.md` (TC113)

## 3. Objective
Validate persisted order snapshot includes expected tax line structure for GST and QST.

## 4. Preconditions
1. Successful checkout in CA/QC context.
2. Snapshot endpoint reachable.

## 5. Test Data
- Order ID from completed checkout.
- Expected tax codes: `GST`, `QST`.

## 6. Procedure (Steps)
### Step 1 — Complete checkout
- Action: Place order and capture order ID.
- Expected Result: Confirmation with valid order ID.

### Step 2 — Request snapshot
- Action: Query order snapshot endpoint.
- Expected Result: Snapshot payload returned.

### Step 3 — Validate tax lines
- Action: Inspect tax lines collection.
- Expected Result: Includes GST and QST with valid rates/taxable base/tax amounts.

## 7. Expected Results (Final)
1. Snapshot includes expected tax lines.
2. Tax line structure and values are valid and coherent.

## 8. Postconditions
1. Evidence is recorded.

## 9. Finally / Cleanup
- Standard cleanup only.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC113`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: GST and QST lines are present and valid in snapshot.
- Fail: Missing tax code, malformed line, or invalid tax values.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
