using System.Collections.Generic;

namespace Redmine.Net.Api.Extensions;

/// <summary>
/// 
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="userAgent"></param>
    public static void SetUserAgent(this Dictionary<string, string> dictionary, string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return;
        }
        
        dictionary ??= new Dictionary<string, string>();
        dictionary["User-Agent"] = userAgent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="username"></param>
    public static void SetImpersonateUser(this Dictionary<string, string> dictionary, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return;
        }
        
        dictionary ??= new Dictionary<string, string>();
        dictionary["X-Redmine-Switch-User"] = username;
    }
}