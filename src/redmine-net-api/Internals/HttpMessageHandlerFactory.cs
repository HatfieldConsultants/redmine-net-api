using System.Net;
using System.Net.Http;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
public sealed class HttpMessageHandlerFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static HttpMessageHandler CreateDefault()
    {
        return CreateDefault(null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="proxy"></param>
    /// <returns></returns>
    public static HttpMessageHandler CreateDefault(IWebProxy proxy)
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false
        };

        if (handler.SupportsAutomaticDecompression)
        {
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }

        if (handler.SupportsProxy && proxy != null)
        {
            handler.UseProxy = true;
            handler.Proxy = proxy;
        }

        return handler;
    }
}