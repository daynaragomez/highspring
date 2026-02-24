FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY DemoShop.sln ./
COPY src/DemoShop.Web/DemoShop.Web.csproj src/DemoShop.Web/
RUN dotnet restore DemoShop.sln
COPY . .
WORKDIR /src/src/DemoShop.Web
RUN dotnet publish DemoShop.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DemoShop.Web.dll"]
