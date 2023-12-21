using CommandLine;

namespace ServerFileDownloader;

public record CommandLineOptions
{
    [Option("platform", Required = true)] 
    public string Platform { get; set; } = null!;

    [Option("channel", Required = true)] 
    public string Channel { get; set; } = null!;
    
    [Option("server", Required = false)]
    public bool DownloadServerFiles { get; set; }
    
    [Option("data", Required = false)]
    public bool DownloadDataFiles { get; set; }

    [Option('m', "modules", Required = false)]
    public IEnumerable<string> ModulesToDownload { get; set; }
}