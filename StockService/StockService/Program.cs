using StockService.Api.Configuration;
using StockService.Application;
using StockService.Application.DTO;
using StockService.Infrastructure;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNServiceBus(_ =>
{
    var endpointConfiguration = new EndpointConfiguration("StockService");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport(new LearningTransport());
    endpointConfiguration.EnableInstallers();

    return endpointConfiguration;
});

// Add services to the container.
builder.Services.AddApiSwagger();
builder.Services.AddFirebaseAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddStockInfrastructure(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeStockDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var stockInfos = app.MapGroup("/stock-info")
    .WithTags("stock-info")
    .RequireAuthorization();

stockInfos.MapGet("/{articleId:int}", async (int articleId, IStockService stockService) =>
{
    var stockInfo = await stockService.GetByArticleId(articleId);

    return stockInfo is null
        ? Results.NotFound()
        : Results.Ok(stockInfo);
});

stockInfos.MapPut("/{articleId:int}", async (int articleId, UpdateStockItemDto stockInfo, IStockService stockService) =>
{
    return await stockService.Update(articleId, stockInfo)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
