using System.Security.Cryptography;

namespace ServerFileDownloader.Features.VerifyFile;

public class FileVerifier : IFileVerifier
{
    public bool ShouldFileBeDownloaded(string path, string expectedFileHash)
    {
        if (!File.Exists(path))
        {
            return true;
        }

        using var fs = File.OpenRead(path);
        var hash = SHA1.HashData(fs);

        return !expectedFileHash.Equals(Convert.ToHexString(hash), StringComparison.InvariantCultureIgnoreCase);
    }
}