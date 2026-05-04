namespace PriceService.Api;

public sealed class FirebaseOptions
{
    public const string SectionName = "Firebase";

    public string ProjectId { get; init; } = string.Empty;

    public string ServiceAccountKeyPath { get; init; } = string.Empty;
}
