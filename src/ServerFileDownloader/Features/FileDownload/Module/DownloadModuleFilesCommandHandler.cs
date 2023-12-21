using MediatR;
using Microsoft.Extensions.Logging;
using ServerFileDownloader.CdnClient;
using ServerFileDownloader.Features.VerifyFile;

namespace ServerFileDownloader.Features.FileDownload.Module;

internal sealed class DownloadModuleFilesCommandHandler(ILogger<DownloadModuleFilesCommand> logger, ICdnClient cdn, IFileVerifier fileVerifier)
    : IRequestHandler<DownloadModuleFilesCommand>
{
    public async Task Handle(DownloadModuleFilesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Verifying and Downloading files for module '{ModuleName}'", request.ModuleName);
        
        var moduleManifest = await cdn.FetchModuleUpdateFile(request.ModuleName);

        if (moduleManifest is null)
        {
            logger.LogError("Failed to fetch update manifest for module {ModuleName}", request.ModuleName);
            return;
        }

        var filesToDownload = moduleManifest.HashList.Where(x => fileVerifier.ShouldFileBeDownloaded(x.Key, x.Value))
            .Select(x => x.Key);

        foreach (var file in filesToDownload)
        {
            await cdn.DownloadFile($"/{request.ModuleName}/{request.Channel}/{request.Platform}/{file}", file);
        }
    }
}