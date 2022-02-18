using System.Collections.Generic;
using System.Net;

namespace Redmine.Net.Api.Types;

/// <summary>
/// 
/// </summary>
public interface IRedmineApiResponse
{
    /// <summary>
    /// 
    /// </summary>
    public object Body { get;}
    /// <summary>
    /// Information about the API.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; }
        
    /// <summary>
    /// The response status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    /// <summary>
    /// The content type of the response.
    /// </summary>
    public string ContentType { get; }
}