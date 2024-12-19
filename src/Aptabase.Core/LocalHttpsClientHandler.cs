namespace Aptabase.Core;

public class LocalHttpsClientHandler : HttpClientHandler
{
    public LocalHttpsClientHandler()
    {
        // Disable SSL verification
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
    }
}