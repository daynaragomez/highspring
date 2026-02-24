# DemoShop

Minimal ASP.NET Core 8 Razor Pages app packaged for containerized E2E testing demos.

## Prerequisites
- Docker (with Docker Compose v2)

## Run locally
```bash
docker compose up --build
```
This builds the web image, starts the ASP.NET Core service on port 8080, and boots a companion PostgreSQL container.

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
