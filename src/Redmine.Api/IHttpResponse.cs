using System.Collections.Generic;
using System.Net;

namespace Redmine.Api
{
    internal interface IHttpResponse
    {
        /// <summary>
        /// 
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Information about the API.
        /// </summary>
        Dictionary<string, string> Headers { get; }

        /// <summary>
        /// The response status code.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The content type of the response.
        /// </summary>
        string ContentType { get; }
    }
}