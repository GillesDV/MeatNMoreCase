FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["PriceService/PriceService.Api/PriceService.Api.csproj", "PriceService/PriceService.Api/"]
COPY ["PriceService/PriceService.Application/PriceService.Application.csproj", "PriceService/PriceService.Application/"]
COPY ["PriceService/PriceService.Domain/PriceService.Domain.csproj", "PriceService/PriceService.Domain/"]
COPY ["PriceService/PriceService.Infrastructure/PriceService.Infrastructure.csproj", "PriceService/PriceService.Infrastructure/"]
RUN dotnet restore "PriceService/PriceService.Api/PriceService.Api.csproj"
COPY . .
RUN dotnet publish "PriceService/PriceService.Api/PriceService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PriceService.Api.dll"]
