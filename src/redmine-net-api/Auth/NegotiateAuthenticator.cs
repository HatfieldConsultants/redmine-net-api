using System;
using System.Net;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
internal sealed class NegotiateAuthenticator : IAuthenticationHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="credentials"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Authenticate(IRedmineApiRequest request, IRedmineApiCredentials credentials)
    {
        var credentialsCache = new CredentialCache();
        var nc = new NetworkCredential(credentials.Login, credentials.Password);
        credentialsCache.Add(request.BaseAddress, "Negotiate", nc);

        // var handler = new HttpClientHandler() { Credentials = credentialsCache, PreAuthenticate = true };
        // var httpClient = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 10) };

        
        throw new System.NotImplementedException();
    }
}