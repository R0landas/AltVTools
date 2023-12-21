using MediatR;

namespace ServerFileDownloader.Features.FileDownload.Server;

internal record DownloadServerFilesCommand(string Channel, string Platform) : IRequest;