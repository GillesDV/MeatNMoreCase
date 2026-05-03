using ArticleService.Api.Configuration;
using ArticleService.Application;
using ArticleService.Domain.Entity;
using ArticleService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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

articles.MapGet("/", (IArticleService articleService) =>
{
    return Results.Ok(articleService.GetAll());
});

articles.MapGet("/{articleId:int}", (int articleId, IArticleService articleService) =>
{
    var article = articleService.GetById(articleId);

    return article is null
        ? Results.NotFound()
        : Results.Ok(article);
});

articles.MapPost("/", (Article article, IArticleService articleService) =>
{
    var createdArticle = articleService.Create(article);

    return Results.Created($"/articles/{createdArticle.ArticleId}", createdArticle);
});

articles.MapPut("/{articleId:int}", (int articleId, Article article, IArticleService articleService) =>
{
    return articleService.Update(articleId, article)
        ? Results.NoContent()
        : Results.NotFound();
});

articles.MapDelete("/{articleId:int}", (int articleId, IArticleService articleService) =>
{
    return articleService.Delete(articleId)
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
