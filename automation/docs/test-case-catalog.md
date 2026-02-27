# Highspring Test Case Catalog

## 1. Purpose
Provide the single source of truth for all candidate test cases, their classification, risk links, and priorities.

This catalog is governed by `test-design-specification.md` and `master-test-plan.md`.

## 2. Case Lifecycle Status
Each case must carry one lifecycle status:
- `Candidate`
- `Approved`
- `Deferred`

Default status on creation: `Candidate`.

## 3. Smoke Cases

### Suite: HomeSmokeSuite
- **TC001_Home_Page_Loads_And_Health_Indicators_Are_Visible**
  - Goal: validate app shell loads and home page is reachable.
  - Priority: Medium
  - Type: Smoke
  - Risks: `R-006`
  - Status: Candidate

- **TC008_Home_Navigation_To_Products_Is_Available**
  - Goal: validate critical navigation path from home to product discovery.
  - Priority: Medium
  - Type: Smoke
  - Risks: `R-006`
  - Status: Candidate

- **TC009_Home_Featured_Product_Add_To_Cart_Starts_Shopping_Flow**
  - Goal: validate shopping flow can start from home entry point.
  - Priority: High
  - Type: Smoke
  - Risks: `R-004`
  - Status: Candidate

### Suite: ProductsSmokeSuite
- **TC002_Products_Page_Loads_And_Product_Cards_Are_Visible**
  - Goal: validate product listing availability.
  - Priority: Medium
  - Type: Smoke
  - Risks: `R-006`
  - Status: Candidate

- **TC003_Add_To_Cart_From_Products_Creates_Cart_Line**
  - Goal: validate minimum purchase path from listing.
  - Priority: High
  - Type: Smoke
  - Risks: `R-004`
  - Status: Candidate

### Suite: CartSmokeSuite
- **TC004_Cart_Quantity_Update_Reflects_In_Line_And_Total**
  - Goal: validate quantity mutation reliability.
  - Priority: High
  - Type: Smoke
  - Risks: `R-004`
  - Status: Candidate

- **TC005_Remove_Cart_Line_Shows_Empty_State**
  - Goal: validate remove flow and cart empty behavior.
  - Priority: High
  - Type: Smoke
  - Risks: `R-004`
  - Status: Candidate

### Suite: CheckoutSmokeSuite
- **TC006_Checkout_Submission_Redirects_To_Confirmation**
  - Goal: validate checkout route continuity and completion.
  - Priority: High
  - Type: Smoke
  - Risks: `R-003`, `R-006`
  - Status: Candidate

- **TC007_Confirmation_Displays_Order_Id_And_Key_Totals**
  - Goal: validate order confirmation essentials.
  - Priority: High
  - Type: Smoke
  - Risks: `R-001`, `R-006`
  - Status: Candidate

- **TC010_Checkout_Invalid_Details_Blocks_Submission_And_Shows_Validation**
  - Goal: validate checkout rejects missing/invalid required details and keeps user on checkout page.
  - Priority: High
  - Type: Smoke
  - Risks: `R-003`, `R-006`
  - Status: Approved

## 4. E2E Cases

### Suite: CartE2ESuite
- **TC101_Cart_Repeated_Additions_Maintain_Stable_Quantities**
  - Goal: verify cart quantity consistency across repeated operations.
  - Priority: High
  - Type: E2E
  - Risks: `R-004`
  - Status: Candidate

- **TC102_Cart_Update_Sequence_Handles_Multiple_Transitions**
  - Goal: validate update sequence (increase/decrease) correctness.
  - Priority: High
  - Type: E2E
  - Risks: `R-004`
  - Status: Candidate

- **TC103_Cart_Remove_Then_Readd_Resets_Line_Correctly**
  - Goal: verify state reset behavior after remove/re-add.
  - Priority: High
  - Type: E2E
  - Risks: `R-004`
  - Status: Candidate

### Suite: PricingE2ESuite
- **TC104_Save10_Discount_Applies_Before_Tax_For_CA_QC**
  - Goal: enforce discount-before-tax rule for percent coupon.
  - Priority: High
  - Type: E2E
  - Risks: `R-001`, `R-002`, `R-005`
  - Status: Candidate

- **TC105_Take5_Discount_Applies_Before_Tax_For_CA_QC**
  - Goal: enforce discount-before-tax rule for fixed coupon.
  - Priority: High
  - Type: E2E
  - Risks: `R-001`, `R-002`, `R-005`
  - Status: Candidate

- **TC106_Invalid_Coupon_Does_Not_Modify_Pricing**
  - Goal: ensure invalid discount attempts are safely rejected.
  - Priority: Medium
  - Type: E2E
  - Risks: `R-005`
  - Status: Candidate

- **TC107_Pricing_Rounding_Is_Currency_Safe_Across_Stages**
  - Goal: validate rounding behavior through subtotal/discount/tax/total.
  - Priority: High
  - Type: E2E
  - Risks: `R-001`, `R-002`
  - Status: Candidate

### Suite: CheckoutE2ESuite
- **TC108_Checkout_With_Save10_Produces_Expected_UI_Totals**
  - Goal: validate complete checkout totals on UI with SAVE10.
  - Priority: High
  - Type: E2E
  - Risks: `R-001`, `R-003`, `R-006`
  - Status: Candidate

- **TC109_Checkout_Without_Coupon_Produces_Expected_UI_Totals**
  - Goal: validate baseline checkout totals without discount.
  - Priority: High
  - Type: E2E
  - Risks: `R-001`, `R-003`, `R-006`
  - Status: Candidate

- **TC110_Checkout_Clears_Cart_Post_Order**
  - Goal: confirm post-order cart cleanup behavior.
  - Priority: High
  - Type: E2E
  - Risks: `R-004`
  - Status: Candidate

- **TC111_Low_Stock_Checkout_Enforces_Stock_Constraint**
  - Goal: validate stock protection behavior at checkout boundary.
  - Priority: High
  - Type: E2E
  - Risks: `R-003`
  - Status: Candidate

### Suite: OrderSnapshotE2ESuite
- **TC112_Order_Snapshot_Matches_Confirmation_Totals**
  - Goal: verify persisted order totals equal confirmation UI totals.
  - Priority: High
  - Type: E2E
  - Risks: `R-001`
  - Status: Candidate

- **TC113_Order_Snapshot_Contains_Expected_Tax_Lines_GST_QST**
  - Goal: verify persisted tax-line integrity for CA/QC.
  - Priority: High
  - Type: E2E
  - Risks: `R-002`
  - Status: Candidate
