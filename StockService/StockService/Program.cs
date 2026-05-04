using StockService.Api.Configuration;
using StockService.Application;
using StockService.Domain.Entity;
using StockService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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

stockInfos.MapGet("/", (IStockService stockService) =>
{
    return Results.Ok(stockService.GetAll());
});

stockInfos.MapGet("/{articleId:int}", (int articleId, IStockService stockService) =>
{
    var stockInfo = stockService.GetByArticleId(articleId);

    return stockInfo is null
        ? Results.NotFound()
        : Results.Ok(stockInfo);
});

stockInfos.MapPost("/", (StockInfo stockInfo, IStockService stockService) =>
{
    var createdStockInfo = stockService.Create(stockInfo);

    return Results.Created($"/stock-info/{createdStockInfo.ArticleId}", createdStockInfo);
});

stockInfos.MapPut("/{articleId:int}", (int articleId, StockInfo stockInfo, IStockService stockService) =>
{
    return stockService.Update(articleId, stockInfo)
        ? Results.NoContent()
        : Results.NotFound();
});

stockInfos.MapDelete("/{articleId:int}", (int articleId, IStockService stockService) =>
{
    return stockService.Delete(articleId)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
