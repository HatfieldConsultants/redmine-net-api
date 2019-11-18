using System.Collections.Generic;
using System.Net;

namespace Redmine.Api
{
    internal sealed class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode, string body, Dictionary<string, string> headers, string contentType)
        {
            ContentType = contentType;
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// The response status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The content type of the response.
        /// </summary>
        public string ContentType { get; }
    }
}