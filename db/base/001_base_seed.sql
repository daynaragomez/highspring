INSERT INTO "Products" ("Id", "Name", "Price", "Stock", "CreatedUtc")
VALUES
    (1, 'Mechanical Keyboard', 99.99, 12, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (2, 'Gaming Mouse', 49.99, 25, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (3, 'USB-C Hub', 39.99, 40, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (4, '4K Monitor', 399.99, 8, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (5, 'Noise Cancelling Headphones', 219.00, 15, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (6, 'Portable SSD', 129.50, 30, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (7, 'Smartphone Stand', 24.00, 60, TIMESTAMPTZ '2024-01-01 00:00:00Z'),
    (8, 'Wireless Charger', 45.00, 35, TIMESTAMPTZ '2024-01-01 00:00:00Z')
ON CONFLICT ("Id") DO UPDATE
SET
    "Name" = EXCLUDED."Name",
    "Price" = EXCLUDED."Price",
    "Stock" = EXCLUDED."Stock",
    "CreatedUtc" = EXCLUDED."CreatedUtc";
