using Redmine.Net.Api.Types;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
internal sealed class ApiKeyAuthenticator : IAuthenticationHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="credentials"></param>
    public void Authenticate(IRedmineApiRequest request, IRedmineApiCredentials credentials)
    {
        var token = credentials.GetToken();
        
        if (token != null)
        {
            request.Headers["X-Redmine-API-Key"] = token;
        }
    }
}