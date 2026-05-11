# Meat & More Case

Inventory demo with an Angular frontend and 3 ASP.NET Core minimal APIs, SQL Server, Firebase Authentication, and NServiceBus Learning Transport.

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

3. Run StockService:

```powershell
dotnet run --project StockService\StockService\StockService.Api.csproj --launch-profile https
```

4. Run PriceService:

```powershell
dotnet run --project PriceService\PriceService.Api\PriceService.Api.csproj --launch-profile https
```

5. Run the Angular portal:

```powershell
cd InventoryPortal
npm install
npm start -- --host 127.0.0.1 --port 4200
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

# Notes And Tradeoffs

- Maybe extract the Minimal Api endpoints from `Program.cs` into another file / structure as well? Especially if more controllers might be added in the future
- Shared Firebase/auth setup is duplicated across services and could become a shared package.
- Try to use vertical slice architecture 
- Instead of using in-memory nservicebus, use Azure Service bus / RabbitMQ; for decent persistence & scalability.
- add some logging & error handling for not-so-happy flows
- See about adding caching
- Angular is on version 19 because the assignment specified Angular 19.x. Could be bumped to 20.X (aka the latest LTS at the time of writing)

# Original assignment TLDR

- Use `docker-compose` to set up the local infrastructure
  - For example databases and optionally RabbitMQ
- All communication between systems must be secured using `OAuth 2.0`
- Use `NServiceBus`
  - Free choice between:
    - `LearningTransport`
    - `RabbitMQ`
- Services may exist within a single solution containing multiple startup projects
- The application must:
  - be available through Git
  - contain the necessary tests
  - expose endpoints through tools such as Swagger
  - include clear local setup instructions

## Frontend (Angular)

Use:
- `Angular 19`
- `Angular Material`

Functionality:
- Screen for creating an article:
  - `articleId`
  - `description`
  - `unit` (`kg` or `pcs`)

Authentication & authorization:
- Login through `OpenID Connect`
- Backend authorization via `OAuth 2.0`
- Free choice of supporting package (e.g. `angular-auth-oidc-client`)

---

## ArticleService

Functionality:
- Secured endpoint for creating articles
- Stores:
  - `articleId`
  - `description`
  - `unit`

Messaging:
- Publishes an event through the service bus whenever an article is created
- Other services react to this event:
  - `StockService` creates initial stock:
    - stock = `0`
    - location = `central warehouse`
  - `PriceService` creates an initial base price:
    - price = `0`

---

## StockService

Manages:
- `articleId`
- `stock`
- `location`
  - `central warehouse`
  - `secondary warehouse`

Endpoints:
- Secured endpoint for retrieving stock information
- Endpoint for updating stock

---

## PriceService

Manages:
- `articleId`
- `basePrice`

Endpoints:
- Secured endpoint for retrieving price information
- Secured endpoint for updating price information

Price calculation:
- Support for tiered discounts
  - From `10kg` → `10%`
  - From `20kg` → `20%`
- Additional stock-based discount
  - From stock `100kg` → additional `10%` discount