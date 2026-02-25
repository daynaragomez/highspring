# Highspring Implementation Plan

## Model placement
- Highspring.Application owns domain models and business rules: Product, Cart, CartItem, Coupon, TaxComponentRate, Order, OrderItem, OrderTaxLine, plus Cart/Discount/Tax/Checkout services.
- Highspring.Infrastructure owns persistence concerns only: EF Core entities/mappings, DbContext, repositories, migrations, and seed file loading.
- Highspring.Web and Highspring.Api are thin delivery layers (UI + endpoints) and must not contain business logic.

## Seed policy (single baseline only)
- Use one canonical baseline seed only.
- Baseline seed files must live only at: src/Highspring.Infrastructure/Seed/Baseline/*.json
- No scenario-specific seed files are allowed anywhere in the repo.
- Test scenarios must be created through /test/* endpoints, not by adding new seed files.

## Startup ownership rule
- Startup initialization ownership is in Highspring.Web Program startup pipeline.
- Only baseline setup is allowed at startup (environment-gated for Development/Test).
- Scenario mutations are never done at startup; they are done only via /test/* endpoints during tests.

## Required tax behavior
- Tax engine must support dual tax components for Quebec: GST and QST.
- Calculation returns per-component breakdown and summed tax total.
- Checkout persists immutable order tax lines (including GST and QST when applicable).

## Required pages
- /
- /products
- /products/{sku}
- /cart
- /checkout
- /checkout/confirmation/{orderId}

## Required testids
- page-home
- page-products
- page-product-details
- page-cart
- page-checkout
- page-checkout-confirmation
- product-card-{sku}
- add-to-cart-{sku}
- cart-line-{sku}
- qty-input-{sku}
- apply-discount-input
- apply-discount-button
- subtotal-value
- discount-total
- tax-total
- order-total
- checkout-submit
- order-confirmation-id

## Phased implementation
1. Foundation: create Application/Infrastructure/Api projects, wire DI, EF Core + PostgreSQL, migrations, health endpoint.
2. Core domain: cart, discounts, inventory concurrency, checkout transaction, GST+QST tax calculator.
3. Test control: implement /test/reset, /test/seed, /test/products/{sku}/stock, /test/carts/{guestSessionId}, /test/orders/{id}; restrict to Development/Test.
4. UI: implement required Razor pages with stable testids and deterministic flows.
5. Validation: integration tests for totals/tax, concurrency tests, Selenium readiness checklist.

## Acceptance criteria
- Deterministic Selenium flows pass for add/remove cart, discount apply, GST+QST verification, checkout confirmation, and out-of-stock race handling.
- All pricing, discount, and tax totals come from shared Application logic only.
- Baseline seed is loaded from src/Highspring.Infrastructure/Seed/Baseline/*.json and scenario setup is done only through /test/*.
- No scenario seed files exist in the codebase.
