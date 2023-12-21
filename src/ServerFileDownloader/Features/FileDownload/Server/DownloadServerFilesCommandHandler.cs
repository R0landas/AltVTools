using MediatR;
using Microsoft.Extensions.Logging;
using ServerFileDownloader.CdnClient;
using ServerFileDownloader.Features.VerifyFile;

namespace ServerFileDownloader.Features.FileDownload.Server;

internal sealed class DownloadServerFilesCommandHandler(
    ILogger<DownloadServerFilesCommandHandler> logger,
    IFileVerifier fileVerifier,
    ICdnClient cdn)
    : IRequestHandler<DownloadServerFilesCommand>
{
    public async Task Handle(DownloadServerFilesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Verifying and Downloading server files...");
        var moduleManifest = await cdn.FetchServerUpdateFile();

        if (moduleManifest is null)
        {
            logger.LogError("Failed to download server update manifest");
            return;
        }

        var filesToDownload = moduleManifest.HashList.Where(x => fileVerifier.ShouldFileBeDownloaded(x.Key, x.Value))
            .Select(x => x.Key);

        foreach (var file in filesToDownload)
        {
            await cdn.DownloadFile($"/server/{request.Channel}/{request.Platform}/{file}", file);
        }
    }
}