using MediatR;

namespace ServerFileDownloader.Features.FileDownload.Module;

internal record DownloadModuleFilesCommand(string Channel, string Platform, string ModuleName) : IRequest;