using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

namespace Redmine.Net.Api.Types;

/// <summary>
/// 
/// </summary>
public interface IRedmineApiRequest
{
    /// <summary>
    /// 
    /// </summary>
    object Body { get; set; }
    /// <summary>
    /// 
    /// </summary>
    Dictionary<string, string> Headers { get; }
    /// <summary>
    /// 
    /// </summary>
    HttpMethod Method { get; set; }
    /// <summary>
    /// 
    /// </summary>
    NameValueCollection Parameters { get; set; }
    /// <summary>
    /// 
    /// </summary>
    Uri BaseAddress { get; set; }
    /// <summary>
    /// 
    /// </summary>
    string Endpoint { get; set; }
    /// <summary>
    /// 
    /// </summary>
    TimeSpan Timeout { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    string ContentType { get; set; }
}