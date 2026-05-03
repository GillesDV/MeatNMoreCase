using ArticleService.Api;
using ArticleService.Application;
using ArticleService.Domain.Entity;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var firebaseOptions = builder.Configuration
    .GetRequiredSection(FirebaseOptions.SectionName)
    .Get<FirebaseOptions>()
    ?? throw new InvalidOperationException($"Configure the {FirebaseOptions.SectionName} section.");

if (string.IsNullOrWhiteSpace(firebaseOptions.ProjectId))
{
    throw new InvalidOperationException("Configure Firebase:ProjectId with your Firebase project id.");
}

if (!string.IsNullOrWhiteSpace(firebaseOptions.ServiceAccountKeyPath))
{
    var serviceAccountKeyPath = Path.IsPathRooted(firebaseOptions.ServiceAccountKeyPath)
        ? firebaseOptions.ServiceAccountKeyPath
        : Path.Combine(builder.Environment.ContentRootPath, firebaseOptions.ServiceAccountKeyPath);

    if (!File.Exists(serviceAccountKeyPath))
    {
        throw new FileNotFoundException("The configured Firebase service account key file was not found.", serviceAccountKeyPath);
    }

    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(serviceAccountKeyPath),
        ProjectId = firebaseOptions.ProjectId
    });
}

var firebaseAuthority = $"https://securetoken.google.com/{firebaseOptions.ProjectId}";

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Firebase ID token. Use: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(document =>
    {
        var requirement = new OpenApiSecurityRequirement();
        requirement.Add(new OpenApiSecuritySchemeReference("Bearer", document, null), []);

        return requirement;
    });
});
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = firebaseAuthority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = firebaseAuthority,
            ValidateAudience = true,
            ValidAudience = firebaseOptions.ProjectId,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IArticleService, ArticleService.Application.ArticleService>();

var app = builder.Build();

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
