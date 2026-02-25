# DemoShop

Minimal ASP.NET Core 8 Razor Pages app packaged for containerized E2E testing demos.

## Prerequisites
- Docker (with Docker Compose v2)

## Run locally
```bash
docker compose up --build
```
This builds the web image, starts the ASP.NET Core service on port 8080, and boots a companion PostgreSQL container.

### Apply EF Core migrations

Run the migrations from a disposable .NET SDK container (no local SDK setup required):

```bash
docker run --rm \
    -v "$PWD":/src \
    -w /src/src/DemoShop.Web \
    --network highspring_demo \
    mcr.microsoft.com/dotnet/sdk:8.0 \
    bash -lc "dotnet restore && dotnet tool install --global dotnet-ef && export PATH=\"/root/.dotnet:/root/.dotnet/tools:$PATH\" && dotnet ef database update"
```

This command connects to the running Postgres container and creates the schema/seed data before you browse the site.

## Endpoints
- http://localhost:8080 – Welcome page used to verify the container starts
- http://localhost:8080/health – Simple health probe returning HTTP 200

## Environment configuration
The web container reads `ConnectionStrings__Default` (already defined in `docker-compose.yml`) so the application is ready to connect to PostgreSQL when needed.

## Project layout
```
DemoShop.sln
├── docker-compose.yml
├── README.md
└── src
    └── DemoShop.Web
        ├── Dockerfile
        ├── Pages
        ├── wwwroot
        └── appsettings.json
```
