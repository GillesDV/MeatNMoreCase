# Meat & More Case

Inventory demo with three ASP.NET Core minimal APIs, an Angular 19 portal, SQL Server, Firebase Authentication, and NServiceBus Learning Transport.

## Prerequisites

- .NET 10 SDK
- Node.js and npm
- Docker Desktop
- A Firebase project with Authentication enabled
- Google sign-in enabled in Firebase Authentication
- `localhost` and `127.0.0.1` added to Firebase Authentication authorized domains

## Local Configuration

Create `.env` from `.env.example`:

```powershell
Copy-Item .env.example .env
```

Development values currently used in this repo:

```env
ask for / see mail for super secret values 🚀 
```

## Recommended Run Path

Run SQL Server in Docker, then run the APIs locally. This matches the checked-in development URLs used by PriceService.

1. Start SQL Server:

```powershell
docker compose up -d sqlserver
```

2. Run ArticleService:

```powershell
dotnet run --project ArticleService\ArticleService.Api\ArticleService.Api.csproj --launch-profile https
```

ArticleService URLs:

```text
https://localhost:7231
http://localhost:5227
Swagger: https://localhost:7231/swagger
```

3. Run StockService:

```powershell
dotnet run --project StockService\StockService\StockService.Api.csproj --launch-profile https
```

StockService URLs:

```text
https://localhost:7218
http://localhost:5097
Swagger: https://localhost:7218/swagger
```

4. Run PriceService:

```powershell
dotnet run --project PriceService\PriceService.Api\PriceService.Api.csproj --launch-profile https
```

PriceService URLs:

```text
https://localhost:7005
http://localhost:5204
Swagger: https://localhost:7005/swagger
```

5. Run the Angular portal:

```powershell
cd InventoryPortal
npm install
npm start -- --host 127.0.0.1 --port 4200
```

Angular URL:

```text
http://127.0.0.1:4200
```

## Docker Commands

Start only SQL Server:

```powershell
docker compose up -d sqlserver
```

Set up Docker DB:

```powershell
docker compose --env-file .env up --build
```

## Firebase Notes

The Angular Firebase config lives in:

```text
InventoryPortal/src/environments/environment.ts
```

For local login, make sure Firebase Authentication has Google enabled and allows:

```text
localhost
127.0.0.1
```

## Notes And Tradeoffs

- Maybe extract the Minimal Api endpoints from `Program.cs` into another file / structure as well? Especially if more controllers might be added in the future
- Shared Firebase/auth setup is duplicated across services and could become a shared package.
- Try to use vertical slice architecture 
- Instead of using in-memory nservicebus, use Azure Service bus / RabbitMQ; for decent persistence & scalability.
- add some logging & error handling for not-so-happy flows
- See about adding caching
- Angular is on version 19 because the assignment specified Angular 19.x. Could be bumped to 20.X (aka the latest LTS at the time of writing)
