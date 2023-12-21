using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerFileDownloader.Features.FileDownload.Data;
using ServerFileDownloader.Features.FileDownload.Module;
using ServerFileDownloader.Features.FileDownload.Server;
using ServerFileDownloader.Validators;

namespace ServerFileDownloader;

public class ApplicationService(ILogger<ApplicationService> logger, CommandLineOptions options, CommandLineOptionsValidator validator, ISender mediator, IHostApplicationLifetime applicationLifetime) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(options, cancellationToken);
        if (!validationResult.IsValid)
        {
            foreach (var validationFailure in validationResult.Errors)
            {
                logger.LogError("Validation error {ValidationMessage}", validationFailure.ErrorMessage);
            }
            applicationLifetime.StopApplication();
            return;
        }
        
        if (options.DownloadServerFiles)
        {
            await mediator.Send(new DownloadServerFilesCommand(options.Channel, options.Platform), cancellationToken);
        }
        
        if (options.DownloadDataFiles)
        {
            await mediator.Send(new DownloadDataFilesCommand(options.Channel), cancellationToken);
        }

        foreach (var moduleName in options.ModulesToDownload)
        {
            await mediator.Send(new DownloadModuleFilesCommand(options.Channel, options.Platform, moduleName), cancellationToken);
        }
        
        logger.LogInformation("File download completed");
        
        applicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}