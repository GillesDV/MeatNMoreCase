FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ArticleService/ArticleService.Api/ArticleService.Api.csproj", "ArticleService/ArticleService.Api/"]
COPY ["ArticleService/ArticleService.Application/ArticleService.Application.csproj", "ArticleService/ArticleService.Application/"]
COPY ["ArticleService/ArticleService.Domain/ArticleService.Domain.csproj", "ArticleService/ArticleService.Domain/"]
COPY ["ArticleService/ArticleService.Infrastructure/ArticleService.Infrastructure.csproj", "ArticleService/ArticleService.Infrastructure/"]
RUN dotnet restore "ArticleService/ArticleService.Api/ArticleService.Api.csproj"
COPY . .
RUN dotnet publish "ArticleService/ArticleService.Api/ArticleService.Api.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ArticleService.Api.dll"]
