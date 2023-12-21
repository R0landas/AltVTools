namespace ServerFileDownloader.CdnClient.Models;

public record UpdateManifest
{
    public int LatestBuildNumber { get; init; }
    public string Version { get; init; } = string.Empty;
    public string SdkVersion { get; init; } = string.Empty;
    public Dictionary<string, string> HashList { get; init; } = new();
}