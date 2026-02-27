# Test Case Specification — TC003

## 1. Document Control
- Case ID: TC003
- Title: Add To Cart From Products Creates Cart Line
- Version: 1.0
- Status: Candidate
- Owner: QA / Automation
- Last Updated: 2026-02-27

## 2. Classification
- Type: Smoke
- Suite: ProductsSmokeSuite
- Priority: High
- Linked Risks: R-004 (Cart state inconsistencies)
- Related Catalog Entry: `test-case-catalog.md` (TC003)

## 3. Objective
Validate that adding a product from the products listing creates a corresponding cart line and starts a valid shopping session.

## 4. Preconditions
1. Web, API, and database services are up and healthy.
2. Deterministic baseline reset is available and successful.
3. Test session starts with a clean browser context (no reused guest cart state).
4. Product `HOODIE-CLASSIC` exists and is in stock.

## 5. Test Data
- Product SKU: `HOODIE-CLASSIC`
- Initial Quantity: `1`
- Base URL: `http://localhost:8080`
- API Reset Endpoint: `POST /internal/test/v1/reset`

## 6. Procedure (Steps)
### Step 1 — Reset baseline data
- Action: Call test-control reset endpoint.
- Expected Result: Reset completes successfully (no error response).

### Step 2 — Open products page
- Action: Navigate to `/products`.
- Expected Result: Products page is displayed and product cards are visible.

### Step 3 — Add product to cart
- Action: Click add-to-cart action for `HOODIE-CLASSIC` from product listing.
- Expected Result: User is redirected to cart view (or cart state updates visibly, based on UX contract).

### Step 4 — Validate cart line exists
- Action: Verify cart contains line item for `HOODIE-CLASSIC`.
- Expected Result: Cart line is present with quantity `1`.

### Step 5 — Validate totals are coherent
- Action: Read subtotal/total values in cart.
- Expected Result: Totals are non-empty, numeric, and consistent with one unit of selected item under baseline pricing.

## 7. Expected Results (Final)
1. A cart line for `HOODIE-CLASSIC` is created after add-to-cart.
2. Quantity is initialized correctly to `1`.
3. Cart reflects a valid, non-empty purchasing state.
4. No functional error is shown to the user.

## 8. Postconditions
1. Test evidence (logs, status) is persisted.
2. Browser session may be closed or reset per suite policy.
3. No cross-test dependency is introduced.

## 9. Finally / Cleanup
- Ensure browser context is disposed or isolated for next case.
- If shared environment was modified beyond baseline, reset state.

## 10. Execution Evidence Requirements
Required evidence:
1. Test runner result entry linked to `TC003`.
2. Pass/fail outcome with failure reason when applicable.
3. Execution reference (run ID or equivalent trace identifier).
4. Available diagnostic artifact reference when failure occurs.

## 11. Pass / Fail Criteria
- Pass: All expected results are met with no assertion failures.
- Fail: Any expected result is not met, including missing cart line, wrong quantity, navigation failure, or runtime exception.

## 12. Automation Decision Fields
- Current Automation State: Candidate
- Decision Date: 2026-02-27
- Decision By: QA / Automation
- Decision Reason: Initial catalog registration
- Target Release: TBD
