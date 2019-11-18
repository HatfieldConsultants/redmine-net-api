using System;
using System.Collections.Generic;

namespace Redmine.Api
{
    internal sealed class HttpRequest : IHttpRequest
    {
        public HttpRequest(string uri, string method,Dictionary<string, string> headers, TimeSpan timeout, string contentType) : this(uri, method, headers, (string)null, timeout, contentType)
        {
        }

        public HttpRequest(string uri, string method,Dictionary<string, string> headers, string payload, TimeSpan timeout, string contentType)
        {
            Payload = payload;
            Headers = headers;
            Timeout = timeout;
            ContentType = contentType;
            Method = method;
            Uri = uri;
        }
        
        public HttpRequest(string uri, string method,Dictionary<string, string> headers, byte[] payload, TimeSpan timeout, string contentType)
        {
            PayloadBytes = payload;
            Headers = headers;
            Timeout = timeout;
            ContentType = contentType;
            Method = method;
            Uri = uri;
        }

        public string Payload { get; }
        
        public byte[] PayloadBytes { get; }

        public string Uri { get; }
        public Dictionary<string, string> Headers { get; }
        public TimeSpan Timeout { get; }
        public string ContentType { get; }
        public string Method { get; }
        public Version Version { get; set; }

        public static HttpRequest Get(string uri, Dictionary<string, string> headers, string contentType)
        {
            return new HttpRequest(uri, HttpVerbs.GET, headers, TimeSpan.MinValue, contentType);
        }
        
        public static HttpRequest Post(string uri, Dictionary<string, string> headers, string payload, string contentType)
        {
            return new HttpRequest(uri, HttpVerbs.POST, headers, payload,TimeSpan.MinValue, contentType);
        }
        
        public static HttpRequest Put(string uri, Dictionary<string, string> headers, string payload, string contentType)
        {
            return new HttpRequest(uri, HttpVerbs.PATCH, headers, payload,TimeSpan.MinValue, contentType);
        }
        
        public static HttpRequest Patch(string uri, Dictionary<string, string> headers, string payload, string contentType)
        {
            return new HttpRequest(uri, HttpVerbs.PATCH, headers, payload,TimeSpan.MinValue, contentType);
        }
        
        public static HttpRequest Delete(string uri, Dictionary<string, string> headers, string contentType)
        {
            return new HttpRequest(uri, HttpVerbs.DELETE, headers, TimeSpan.MinValue, contentType);
        }
    }
}