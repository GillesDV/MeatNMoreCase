using PriceService.Api.Configuration;
using PriceService.Application;
using PriceService.Application.DTO;
using PriceService.Infrastructure;
using NServiceBus;

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
    IPriceService priceService) =>
{
    var articlePrice = await priceService.GetByArticleId(articleId);

    return articlePrice is null
        ? Results.NotFound()
        : Results.Ok(articlePrice);
});

articlePrices.MapPut("/{articleId:int}", async (int articleId, UpdateArticlePriceDto articlePrice, IPriceService priceService) =>
{
    return await priceService.Update(articleId, articlePrice)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
