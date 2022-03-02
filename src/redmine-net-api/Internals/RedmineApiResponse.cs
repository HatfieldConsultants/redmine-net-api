using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using Redmine.Net.Api.Old.Internals;

namespace Redmine.Net.Api.Types;

/// <summary>
/// 
/// </summary>
public sealed class RedmineApiResponse: IRedmineApiResponse
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="body"></param>
    /// <param name="headers"></param>
    /// <param name="contentType"></param>
    public RedmineApiResponse(HttpStatusCode statusCode, object body, IDictionary<string, string> headers, string contentType)
    {
        StatusCode = statusCode;
        Body = body;
        Headers = new ReadOnlyDictionary<string, string>(headers);
        ContentType = contentType;
    }

    /// <summary>
    /// Raw response body. Typically a string, but when requesting images, it will be a byte array.
    /// </summary>
    public object Body { get; }
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