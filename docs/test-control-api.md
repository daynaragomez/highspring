# Test-Control API (Selenium)

This document describes the minimal test-control endpoints exposed by `Highspring.Api` for deterministic Selenium setup.

## Base URL

- Local Docker: `http://localhost:8081`
- Base path: `/internal/test/v1`

## Availability

- Endpoints are mapped only in `Development` and `Test` environments.
- These endpoints are intended for automation setup/teardown, not production clients.

## 1) Reset to baseline

Resets transactional data and reloads baseline seed from:
- `src/Highspring.Infrastructure/Seed/Baseline/products.json`
- `src/Highspring.Infrastructure/Seed/Baseline/coupons.json`
- `src/Highspring.Infrastructure/Seed/Baseline/tax-component-rates.json`

**Request**

```http
POST /internal/test/v1/reset
```

**cURL**

```bash
curl -i -X POST http://localhost:8081/internal/test/v1/reset
```

**Response**
- `204 No Content`

---

## 2) Set product stock

Sets stock quantity for a product SKU.

**Request**

```http
POST /internal/test/v1/products/{sku}/stock
Content-Type: application/json

{
  "quantity": 1
}
```

**cURL**

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"quantity":1}' \
  http://localhost:8081/internal/test/v1/products/HOODIE-CLASSIC/stock
```

**Responses**
- `204 No Content`
- `400 Bad Request` (e.g., negative quantity)
- `404 Not Found` (unknown SKU)

---

## 3) Upsert cart items for guest session

Replaces cart items for a guest session. Repeated SKUs are normalized and summed.

**Request**

```http
POST /internal/test/v1/carts/{guestSessionId}/items
Content-Type: application/json

{
  "items": [
    { "sku": "HOODIE-CLASSIC", "quantity": 1 },
    { "sku": "SNEAKER-URBAN", "quantity": 2 }
  ]
}
```

**cURL**

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"items":[{"sku":"HOODIE-CLASSIC","quantity":1},{"sku":"SNEAKER-URBAN","quantity":2}]}' \
  http://localhost:8081/internal/test/v1/carts/guest-a/items
```

**Response 200 example**

```json
{
  "id": "68f92c47-cb1f-458d-8d62-b21b96056460",
  "guestSessionId": "guest-a",
  "items": [
    { "sku": "HOODIE-CLASSIC", "name": "Classic Hoodie", "unitPrice": 49.00, "quantity": 1 },
    { "sku": "SNEAKER-URBAN", "name": "Urban Sneakers", "unitPrice": 89.00, "quantity": 2 }
  ]
}
```

**Error responses**
- `400 Bad Request`
- `404 Not Found` (unknown SKU)

---

## 4) Apply or remove cart coupon

Stores coupon state for a guest session.

- Apply: send a valid `couponCode`
- Remove: send `null` or empty string

**Request**

```http
POST /internal/test/v1/carts/{guestSessionId}/coupon
Content-Type: application/json

{
  "couponCode": "SAVE10"
}
```

**Apply cURL**

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"couponCode":"SAVE10"}' \
  http://localhost:8081/internal/test/v1/carts/guest-a/coupon
```

**Remove cURL**

```bash
curl -i \
  -H "Content-Type: application/json" \
  -d '{"couponCode":null}' \
  http://localhost:8081/internal/test/v1/carts/guest-a/coupon
```

**Responses**
- `204 No Content`
- `400 Bad Request`
- `404 Not Found` (coupon not found or inactive)

---

## 5) Get order snapshot

Retrieves immutable order pricing/tax snapshot for assertions.

**Request**

```http
GET /internal/test/v1/orders/{orderId}
```

**cURL**

```bash
curl -i http://localhost:8081/internal/test/v1/orders/<order-id>
```

**Response 200 example**

```json
{
  "orderId": "11111111-2222-3333-4444-555555555555",
  "guestSessionId": "guest-a",
  "subtotal": 49.00,
  "discountTotal": 4.90,
  "taxTotal": 7.16,
  "grandTotal": 51.26,
  "items": [
    {
      "productSku": "HOODIE-CLASSIC",
      "productName": "Classic Hoodie",
      "unitPrice": 49.00,
      "quantity": 1,
      "lineTotal": 49.00
    }
  ],
  "taxLines": [
    { "taxCode": "GST", "rate": 0.05, "taxableBase": 44.10, "taxAmount": 2.21 },
    { "taxCode": "QST", "rate": 0.09975, "taxableBase": 44.10, "taxAmount": 4.95 }
  ],
  "placedAtUtc": "2026-02-25T18:00:00Z"
}
```

**Response**
- `404 Not Found` if order does not exist

---

## Suggested Selenium setup flow

1. `POST /reset`
2. Optional scenario setup:
   - `POST /products/{sku}/stock`
   - `POST /carts/{guestSessionId}/items`
   - `POST /carts/{guestSessionId}/coupon`
3. Execute UI flow in Selenium
4. Assert totals and tax lines via `GET /orders/{orderId}`
