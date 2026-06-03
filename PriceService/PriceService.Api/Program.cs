using PriceService.Api.Configuration;
using PriceService.Application.DTO;
using PriceService.Infrastructure;
using NServiceBus;
using PriceService.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNServiceBus(_ =>
{
    var endpointConfiguration = new EndpointConfiguration("PriceService");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport(new LearningTransport());
    endpointConfiguration.EnableInstallers();

    return endpointConfiguration;
});

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

articlePrices.MapGet("/{articleId:int}", async (
    int articleId,
    int? quantityOrdered,
    IPriceService priceService) =>
{
    var articlePrice = await priceService.GetByArticleId(articleId, quantityOrdered);

    return articlePrice is null
        ? Results.NotFound()
        : Results.Ok(articlePrice);
})
.WithName("GetArticlePrice")
.WithSummary("Get an article price")
.WithDescription("Returns the calculated price for an article. When quantityOrdered is provided, the applicable quantity reduction is included in the total.")
.Produces<ArticlePriceDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

articlePrices.MapGet("/{articleId:int}/breakdown", async (
    int articleId,
    IPriceService priceService) =>
{
    var articlePriceBreakdown = await priceService.GetPriceBreakdownByArticleId(articleId);

    return articlePriceBreakdown is null
        ? Results.NotFound()
        : Results.Ok(articlePriceBreakdown);
})
.WithName("GetArticlePriceBreakdown")
.WithSummary("Get article price tiers")
.WithDescription("Returns the default unit price and all quantity-based price tiers for the article unit.")
.Produces<ArticlePriceBreakdownDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

articlePrices.MapPut("/{articleId:int}", async (int articleId, UpdateArticlePriceDto articlePrice, IPriceService priceService) =>
{
    return await priceService.Update(articleId, articlePrice)
        ? Results.NoContent()
        : Results.NotFound();
})
.WithName("UpdateArticlePrice")
.WithSummary("Update an article base price")
.WithDescription("Updates the base price in euros used to calculate totals and quantity-based price tiers for an article.")
.Accepts<UpdateArticlePriceDto>("application/json")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
