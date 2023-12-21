namespace ServerFileDownloader.Features.VerifyFile;

public interface IFileVerifier
{
    bool ShouldFileBeDownloaded(string path, string expectedFileHash);
}