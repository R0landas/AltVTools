using ServerFileDownloader.CdnClient.Models;

namespace ServerFileDownloader.CdnClient;

public interface ICdnClient
{
    Task<UpdateManifest?> FetchModuleUpdateFile(string moduleName);
    Task<UpdateManifest?> FetchServerUpdateFile();
    Task<UpdateManifest?> FetchDataUpdateFile();

    Task DownloadFile(string url, string path);
}