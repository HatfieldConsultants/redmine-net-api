using System;
using System.Collections.Generic;

namespace Redmine.Api
{
    internal interface IHttpRequest
    {
        string Payload { get; }
        byte[] PayloadBytes { get; }
        string Uri { get; }

        Dictionary<string, string> Headers { get; }

        TimeSpan Timeout { get; }
        string ContentType { get; }
        string Method { get; }
        Version Version { get; set; }
    }
}