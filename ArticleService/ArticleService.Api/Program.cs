using ArticleService.Api.Configuration;
using ArticleService.Application;
using ArticleService.Domain.Entity;
using ArticleService.Infrastructure;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNServiceBus(_ =>
{
    var endpointConfiguration = new EndpointConfiguration("ArticleService");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport(new LearningTransport());
    endpointConfiguration.EnableInstallers();

    return endpointConfiguration;
});

// Add services to the container.

builder.Services.AddApiSwagger();
builder.Services.AddFirebaseAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddArticleInfrastructure(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeArticleDatabaseAsync();

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

var articles = app.MapGroup("/articles")
    .WithTags("articles")
    .RequireAuthorization();

articles.MapGet("/", async (IArticleService articleService) =>
{
    return Results.Ok(await articleService.GetAll());
});

articles.MapGet("/{articleId:int}", async (int articleId, IArticleService articleService) =>
{
    var article = await articleService.GetById(articleId);

    return article is null
        ? Results.NotFound()
        : Results.Ok(article);
});

articles.MapPost("/", async (Article article, IArticleService articleService) =>
{
    var createdArticle = await articleService.Create(article);

    return Results.Created($"/articles/{createdArticle.ArticleId}", createdArticle);
});

articles.MapPut("/{articleId:int}", async (int articleId, Article article, IArticleService articleService) =>
{
    return await articleService.Update(articleId, article)
        ? Results.NoContent()
        : Results.NotFound();
});

articles.MapDelete("/{articleId:int}", async (int articleId, IArticleService articleService) =>
{
    return await articleService.Delete(articleId)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
