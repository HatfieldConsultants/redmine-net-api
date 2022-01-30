using Redmine.Net.Api.Old.Internals;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
internal static class CredentialsExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    public static string GetToken(this IRedmineApiCredentials credentials)
    { 
        Ensure.ArgumentNotNull(credentials, nameof(credentials));

        return credentials.Password;
    }
}