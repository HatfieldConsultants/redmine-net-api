using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api;

internal class BasicAuthenticator : IAuthenticationHandler
{
    ///<summary>
    ///Authenticate a request using the basic access authentication scheme
    ///</summary>
    ///<param name="request">The request to authenticate</param>
    ///<param name="credentials">The credentials to attach to the request</param>
    ///<remarks>
    ///</remarks>
    public void Authenticate(IRedmineApiRequest request, IRedmineApiCredentials credentials)
    {
        Debug.Assert(credentials.Password != null, "Password is null");

        var header = string.Format(
            CultureInfo.InvariantCulture,
            "Basic {0}",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(
                string.Format(CultureInfo.InvariantCulture, "{0}:{1}", credentials.Login, credentials.Password))));

        request.Headers["Authorization"] = header;
    }
}