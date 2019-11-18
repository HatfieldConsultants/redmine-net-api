#if NET40
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Redmine.Api
{
    internal sealed partial class HttpRequester : IHttpRequesterAsync
    {
        #region Implementation of IHttpRequesterAsync

        public Task<IHttpResponse> GetAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            var requestMessage = CreateHttpWebRequest(request);

            return  requestMessage.GetResponseAsync().ContinueWith(GetResult, cancellationToken);
        }

        public Task<IHttpResponse> CreateAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            var requestMessage = CreateHttpWebRequest(request);

            return  requestMessage.GetResponseAsync().ContinueWith(GetResult, cancellationToken);
        }

        public Task<IHttpResponse> PatchAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            var requestMessage = CreateHttpWebRequest(request);

            return  requestMessage.GetResponseAsync().ContinueWith(GetResult, cancellationToken);
        }

        public Task<IHttpResponse> UpdateAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            var requestMessage = CreateHttpWebRequest(request);

            return  requestMessage.GetResponseAsync().ContinueWith(GetResult, cancellationToken);
        }

        public Task<IHttpResponse> DeleteAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            var requestMessage = CreateHttpWebRequest(request);

            return  requestMessage.GetResponseAsync().ContinueWith(GetResult, cancellationToken);
        }

        #endregion

        private static IHttpResponse GetResult(Task<WebResponse> task)
        {
           var asyncState= (IHttpRequest) task.AsyncState;
           return CreateHttpResponse((HttpWebResponse)task.Result);
        }
    }
}
#endif