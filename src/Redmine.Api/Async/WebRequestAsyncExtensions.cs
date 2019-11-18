#if NET40
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Redmine.Api
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebRequestAsyncExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task<Stream> GetRequestStreamAsync(this WebRequest request, object state = null)
        {
            return Task.Factory.FromAsync<Stream>(
                request.BeginGetRequestStream, request.EndGetRequestStream, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task<WebResponse> GetResponseAsync(this WebRequest request, object state = null)
        {
            return Task.Factory.FromAsync<WebResponse>(
                request.BeginGetResponse, request.EndGetResponse, state);
        }
    }
}
#endif