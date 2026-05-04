FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["StockService/StockService/StockService.Api.csproj", "StockService/StockService/"]
COPY ["StockService/StockService.Application/StockService.Application.csproj", "StockService/StockService.Application/"]
COPY ["StockService/StockService.Domain/StockService.Domain.csproj", "StockService/StockService.Domain/"]
COPY ["StockService/StockService.Infrastructure/StockService.Infrastructure.csproj", "StockService/StockService.Infrastructure/"]
RUN dotnet restore "StockService/StockService/StockService.Api.csproj"
COPY . .
RUN dotnet publish "StockService/StockService/StockService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockService.Api.dll"]
