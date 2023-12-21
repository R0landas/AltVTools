using MediatR;

namespace ServerFileDownloader.Features.FileDownload.Data;

internal record DownloadDataFilesCommand(string Channel) : IRequest;