FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Highspring.Api/Highspring.Api.csproj src/Highspring.Api/
COPY src/Highspring.Application/Highspring.Application.csproj src/Highspring.Application/
COPY src/Highspring.Infrastructure/Highspring.Infrastructure.csproj src/Highspring.Infrastructure/
RUN dotnet restore src/Highspring.Api/Highspring.Api.csproj

COPY . .
RUN dotnet publish src/Highspring.Api/Highspring.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Highspring.Api.dll"]
