using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
public static class HttpHeadersExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpHeaders"></param>
    /// <returns></returns>
    public static IDictionary<string, string> ToDictionary(this HttpHeaders httpHeaders)
    {
        return httpHeaders.ToDictionary(x => x.Key, x => string.Join(",", x.Value));
    }
}