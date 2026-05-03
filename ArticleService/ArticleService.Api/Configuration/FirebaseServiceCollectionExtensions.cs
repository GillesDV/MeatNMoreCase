using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ArticleService.Api.Configuration;

public static class FirebaseServiceCollectionExtensions
{
    public static IServiceCollection AddFirebaseAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var firebaseOptions = configuration
            .GetRequiredSection(FirebaseOptions.SectionName)
            .Get<FirebaseOptions>()
            ?? throw new InvalidOperationException($"Configure the {FirebaseOptions.SectionName} section.");

        if (string.IsNullOrWhiteSpace(firebaseOptions.ProjectId))
        {
            throw new InvalidOperationException("Configure Firebase:ProjectId with your Firebase project id.");
        }

        InitializeFirebaseAdmin(firebaseOptions, environment);

        var firebaseAuthority = $"https://securetoken.google.com/{firebaseOptions.ProjectId}";

        services
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

        services.AddAuthorization();

        return services;
    }

    private static void InitializeFirebaseAdmin(FirebaseOptions firebaseOptions, IHostEnvironment environment)
    {
        if (FirebaseApp.DefaultInstance is not null || string.IsNullOrWhiteSpace(firebaseOptions.ServiceAccountKeyPath))
        {
            return;
        }

        var serviceAccountKeyPath = Path.IsPathRooted(firebaseOptions.ServiceAccountKeyPath)
            ? firebaseOptions.ServiceAccountKeyPath
            : Path.Combine(environment.ContentRootPath, firebaseOptions.ServiceAccountKeyPath);

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
}
