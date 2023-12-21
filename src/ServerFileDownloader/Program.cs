using CommandLine;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerFileDownloader.CdnClient;
using ServerFileDownloader.Features.VerifyFile;

namespace ServerFileDownloader;

public static class Program
{
    public static Task Main(string[] args)
    {
        CommandLineOptions parsedOptions = null!;
        
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(opts => parsedOptions = opts)
            .WithNotParsed( _ => Environment.Exit(1));
        
        var host = CreateHostBuilder(args, parsedOptions).Build();
        return host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args, CommandLineOptions parsedOptions)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddLogging(cfg =>
                {
                    cfg.AddFilter("Microsoft", LogLevel.Warning);
                });
                
                services
                    .AddSingleton<CommandLineOptions>(parsedOptions)
                    .AddSingleton<HttpClient>(provider => new HttpClient { BaseAddress = new Uri(Constants.BaseUrl) })
                    .AddTransient<ICdnClient, CdnClient.CdnClient>()
                    .AddTransient<IFileVerifier, FileVerifier>()
                    .AddHostedService<ApplicationService>();


                services.AddValidatorsFromAssemblyContaining<ApplicationService>();
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationService>());
            });
        
        return hostBuilder;
    }
}