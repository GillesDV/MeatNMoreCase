using ArticleService.Application;
using ArticleService.Domain.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IArticleService, ArticleService.Application.ArticleService>(); //TODO  make it Scoped, once a DB is used

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var articles = app.MapGroup("/articles")
    .WithTags("articles");

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
