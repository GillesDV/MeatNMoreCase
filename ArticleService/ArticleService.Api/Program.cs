using ArticleService.Api.Configuration;
using ArticleService.Application.Interfaces;
using ArticleService.Domain.Entity;
using ArticleService.Infrastructure;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);
const string ArticleCorsPolicy = "ArticleCorsPolicy";

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
builder.Services.AddCors(options =>
{
    options.AddPolicy(ArticleCorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>()
            ?? [];

        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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
app.UseCors(ArticleCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

var articles = app.MapGroup("/articles")
    .WithTags("articles")
    .RequireAuthorization();

articles.MapGet("/", async (IArticleService articleService) =>
{
    return Results.Ok(await articleService.GetAll());
})
    .WithName("GetArticles")
    .WithSummary("Get all articles")
    .WithDescription("Returns every article with its identifier, description, and unit of measure.")
    .Produces<IReadOnlyCollection<Article>>(StatusCodes.Status200OK);

articles.MapGet("/{articleId:int}", async (int articleId, IArticleService articleService) =>
{
    var article = await articleService.GetById(articleId);

    return article is null
        ? Results.NotFound()
        : Results.Ok(article);
})
.WithName("GetArticleById")
.WithSummary("Get an article by ID")
.WithDescription("Returns the article that matches the supplied article ID.")
.Produces<Article>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

articles.MapPost("/", async (Article article, IArticleService articleService) =>
{
    var createdArticle = await articleService.Create(article);

    return Results.Created($"/articles/{createdArticle.ArticleId}", createdArticle);
})
.WithName("CreateArticle")
.WithSummary("Create an article")
.WithDescription("Creates a new article and publishes the article-created event used by dependent services.")
.Accepts<Article>("application/json")
.Produces<Article>(StatusCodes.Status201Created);

articles.MapPut("/{articleId:int}", async (int articleId, Article article, IArticleService articleService) =>
{
    return await articleService.Update(articleId, article)
        ? Results.NoContent()
        : Results.NotFound();
})
.WithName("UpdateArticle")
.WithSummary("Update an article")
.WithDescription("Updates the description and unit of measure for the article with the supplied ID.")
.Accepts<Article>("application/json")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
