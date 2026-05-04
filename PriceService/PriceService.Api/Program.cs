using PriceService.Api.Configuration;
using PriceService.Application;
using PriceService.Domain.Entity;
using PriceService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiSwagger();
builder.Services.AddFirebaseAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddPriceInfrastructure(builder.Configuration);

var app = builder.Build();

await app.Services.InitializePriceDatabaseAsync();

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

var articlePrices = app.MapGroup("/article-prices")
    .WithTags("article-prices")
    .RequireAuthorization();

articlePrices.MapGet("/", (IPriceService priceService) =>
{
    return Results.Ok(priceService.GetAll());
});

articlePrices.MapGet("/{articleId:int}", (int articleId, IPriceService priceService) =>
{
    var articlePrice = priceService.GetByArticleId(articleId);

    return articlePrice is null
        ? Results.NotFound()
        : Results.Ok(articlePrice);
});

articlePrices.MapPost("/", (ArticlePrice articlePrice, IPriceService priceService) =>
{
    var createdArticlePrice = priceService.Create(articlePrice);

    return Results.Created($"/article-prices/{createdArticlePrice.ArticleId}", createdArticlePrice);
});

articlePrices.MapPut("/{articleId:int}", (int articleId, ArticlePrice articlePrice, IPriceService priceService) =>
{
    return priceService.Update(articleId, articlePrice)
        ? Results.NoContent()
        : Results.NotFound();
});

articlePrices.MapDelete("/{articleId:int}", (int articleId, IPriceService priceService) =>
{
    return priceService.Delete(articleId)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
