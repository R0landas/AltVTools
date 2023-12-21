using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using ServerFileDownloader.CdnClient.Models;

namespace ServerFileDownloader.CdnClient;

public class CdnClient(HttpClient client, CommandLineOptions options, ILogger<CdnClient> logger) : ICdnClient
{
    public Task<UpdateManifest?> FetchModuleUpdateFile(string moduleName)
    {
        return client.GetFromJsonAsync<UpdateManifest>($"{moduleName}/{options.Channel}/{options.Platform}/update.json");
    }

    public Task<UpdateManifest?> FetchServerUpdateFile()
    {
        return client.GetFromJsonAsync<UpdateManifest>($"server/{options.Channel}/{options.Platform}/update.json");
    }

    public Task<UpdateManifest?> FetchDataUpdateFile()
    {
        return client.GetFromJsonAsync<UpdateManifest>($"data/{options.Channel}/update.json");
    }

    public async Task DownloadFile(string url, string path)
    {
        var fileUrl = $"{client.BaseAddress}/{url}";
        logger.LogInformation("Downloading file {FilePath} from {FileUrl}", Path.GetFileName(path), fileUrl);
        var directoryName = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        
        var response = await client.GetStreamAsync(url);
            await using var fileStream = File.OpenWrite(path);
        await response.CopyToAsync(fileStream);
    }
}