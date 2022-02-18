using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

namespace Redmine.Net.Api.Types;

/// <summary>
/// </summary>
public class RedmineApiRequest : IRedmineApiRequest
{
    /// <inheritdoc />
    public object Body { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string> Headers { get; set; } = new();

    /// <inheritdoc />
    public HttpMethod Method { get; set; }

    /// <inheritdoc />
    public NameValueCollection Parameters { get; set; } = new();

    /// <inheritdoc />
    public Uri BaseAddress { get; set; }

    /// <inheritdoc />
    public string Endpoint { get; set; }

    /// <inheritdoc />

    public TimeSpan Timeout { get; set; }

    /// <inheritdoc />
    public string ContentType { get; set; }
}