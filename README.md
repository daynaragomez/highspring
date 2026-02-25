# Highspring

[![Automation Smoke (main)](https://github.com/daynaragomez/highspring/actions/workflows/automation-smoke.yml/badge.svg?branch=main)](https://github.com/daynaragomez/highspring/actions/workflows/automation-smoke.yml?query=branch%3Amain)

Automation-ready .NET ecommerce demo with:
- `Highspring.Web` (Razor Pages UI)
- `Highspring.Api` (test-control + health endpoints)
- `PostgreSQL` (via Docker Compose)

This README is focused on **building reliable Selenium automation** for cart, discount, tax, and checkout flows.

## Quick Start (Docker)

```bash
docker compose up -d --build
```

Services:
- Web UI: `http://localhost:8080`
- API: `http://localhost:8081`
- PostgreSQL: `localhost:5432`

Health checks:
- Web: `GET http://localhost:8080/health`
- API: `GET http://localhost:8081/health`

Reset test data to deterministic baseline:

```bash
curl -i -X POST http://localhost:8081/internal/test/v1/reset
```

---

## What To Automate

Core user-visible behaviors:
1. Add item to cart
2. Update cart quantity
3. Remove item from cart
4. Apply coupon discount
5. Validate tax calculation
6. Complete checkout
7. Validate order confirmation totals
8. Validate cart is cleared after checkout

Business expectations:
- Cart quantity changes are stable across repeated operations.
- Discount is applied before tax.
- Tax is computed from taxable base.
- Checkout decrements stock and clears cart.

---

## Navigation Map (UI)

Base URL: `http://localhost:8080`

- Home: `/`
- Product list: `/products`
- Product details: `/products/{sku}`
- Cart: `/cart`
- Checkout: `/checkout`
- Checkout confirmation: `/checkout/confirmation/{orderId}`

Typical flow for Selenium:
1. Open `/products`
2. Add product(s)
3. Open `/cart`
4. Update quantity / remove / apply coupon
5. Open `/checkout`
6. Submit checkout form
7. Assert redirect to `/checkout/confirmation/{orderId}`
8. Assert order totals and post-checkout empty cart

---

## Stable Selectors (`data-testid`)

### Home / Product Discovery
- `page-home`
- `product-card-{SKU}`
- `add-to-cart-{SKU}`

### Product List & Details
- `page-products`
- `page-product-details`
- `product-card-{SKU}`
- `add-to-cart-{SKU}`

### Cart
- `page-cart`
- `cart-line-{SKU}`
- `qty-input-{SKU}`
- `apply-discount-input`
- `apply-discount-button`
- `subtotal-value`
- `discount-total`
- `tax-total`
- `order-total`

### Checkout
- `page-checkout`
- `checkout-submit`
- `subtotal-value`
- `discount-total`
- `tax-total`
- `order-total`

### Confirmation
- `page-checkout-confirmation`
- `order-confirmation-id`
- `subtotal-value`
- `discount-total`
- `tax-total`
- `order-total`

---

## Baseline Seed Data (Deterministic)

### Products
- `HOODIE-CLASSIC` — 49.00
- `SNEAKER-URBAN` — 89.00
- `BACKPACK-TRAVEL` — 64.00

### Coupons
- `SAVE10` — 10% off
- `TAKE5` — fixed 5.00 off

### Taxes (CA/QC)
- `GST` = 5%
- `QST` = 9.975%

---

## Pricing Rules for Assertions

- `subtotal = sum(unitPrice * quantity)`
- `discount = coupon(subtotal)`
- `taxableBase = max(0, subtotal - discount)`
- `taxTotal = sum(taxLines over taxableBase)`
- `grandTotal = taxableBase + taxTotal`

Rounding behavior uses currency-safe rounding at each pricing stage.

Example (Hoodie 49.00 with `SAVE10` in CA/QC):
- Subtotal = 49.00
- Discount = 4.90
- Taxable base = 44.10
- GST = 2.21
- QST = 4.40
- Tax total = 6.61
- Grand total = 50.71

---

## Test-Control API (for Selenium Setup / Assertions)

Base URL: `http://localhost:8081/internal/test/v1`

> Available only in `Development` and `Test` environments.

### 1) Reset baseline
`POST /reset`

```bash
curl -i -X POST http://localhost:8081/internal/test/v1/reset
```

### 2) Set product stock
`POST /products/{sku}/stock`

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"quantity":1}' \
  http://localhost:8081/internal/test/v1/products/HOODIE-CLASSIC/stock
```

### 3) Upsert cart items by guest session
`POST /carts/{guestSessionId}/items`

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"items":[{"sku":"HOODIE-CLASSIC","quantity":1}]}' \
  http://localhost:8081/internal/test/v1/carts/guest-a/items
```

### 4) Apply/remove coupon by guest session
`POST /carts/{guestSessionId}/coupon`

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"couponCode":"SAVE10"}' \
  http://localhost:8081/internal/test/v1/carts/guest-a/coupon
```

Remove coupon:

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"couponCode":null}' \
  http://localhost:8081/internal/test/v1/carts/guest-a/coupon
```

### 5) Read order snapshot (strong assertion source)
`GET /orders/{orderId}`

```bash
curl -i http://localhost:8081/internal/test/v1/orders/<order-id>
```

Use this endpoint to validate:
- persisted subtotal / discount / tax / grand total
- tax lines (`GST`, `QST`, rates, taxable base)
- purchased items and quantities

---

## Selenium Implementation Notes

### Guest session behavior
The app uses cookie `highspring_guest_session` to identify guest carts.
- Reuse browser context to keep same cart.
- New browser profile/session creates a new cart.

### Form posting and anti-forgery
Razor Pages forms include anti-forgery tokens.
- In Selenium, normal click/submit handles this automatically.
- For direct HTTP automation, preserve cookies and include `__RequestVerificationToken`.

### Recommended waiting strategy
- Wait for URL change after submits (`/cart`, `/checkout`, confirmation route).
- Wait for key test IDs before asserting values.
- Avoid brittle CSS structure selectors; prefer `data-testid`.

---

## Suggested End-to-End Selenium Scenarios

### Scenario A: Core Cart Mutations
1. Reset baseline via API
2. Go to `/products`
3. Add `HOODIE-CLASSIC` three times
4. Assert cart quantity is `3`
5. Update quantity to `5`, then `2`
6. Remove line item
7. Assert empty cart message
8. Re-add same SKU and assert quantity is `1`

### Scenario B: Discount + Tax
1. Add `HOODIE-CLASSIC`
2. Apply `SAVE10`
3. Assert:
   - `discount-total = 4.90`
   - `tax-total = 6.61`
   - `order-total = 50.71`

### Scenario C: Checkout + Snapshot Validation
1. Add item(s), optionally apply coupon
2. Complete checkout form at `/checkout`
3. Capture `orderId` from confirmation page (`order-confirmation-id`)
4. Call `GET /internal/test/v1/orders/{orderId}`
5. Assert API totals/tax lines match UI confirmation
6. Open `/cart` and assert cart is empty

---

## Troubleshooting

- Reset database state:

```bash
docker compose down -v --remove-orphans
docker compose up -d --build
curl -i -X POST http://localhost:8081/internal/test/v1/reset
```

- Check container logs:

```bash
docker compose logs web --tail=200
docker compose logs api --tail=200
```

- If tests are flaky, verify:
  - test starts with `/internal/test/v1/reset`
  - Selenium session is not reused unexpectedly across tests
  - assertions use `data-testid` and explicit waits
