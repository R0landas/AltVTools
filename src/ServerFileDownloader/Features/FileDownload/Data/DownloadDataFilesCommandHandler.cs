using MediatR;
using Microsoft.Extensions.Logging;
using ServerFileDownloader.CdnClient;
using ServerFileDownloader.Features.VerifyFile;

namespace ServerFileDownloader.Features.FileDownload.Data;

internal sealed class DownloadDataFilesCommandHandler(
    ILogger<DownloadDataFilesCommandHandler> logger,
    IFileVerifier verifier,
    ICdnClient cdn)
    : IRequestHandler<DownloadDataFilesCommand>
{
    public async Task Handle(DownloadDataFilesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Verifying and Downloading data files...");
        
        var moduleManifest = await cdn.FetchDataUpdateFile();

        if (moduleManifest is null)
        {
            logger.LogError("Failed to fetch server update manifest");
            return;
        }

        var filesToDownload = moduleManifest.HashList.Where(x => verifier.ShouldFileBeDownloaded(x.Key, x.Value))
            .Select(x => x.Key);

        foreach (var file in filesToDownload)
        {
            await cdn.DownloadFile($"/data/{request.Channel}/{file}", file);
        }
    }
}