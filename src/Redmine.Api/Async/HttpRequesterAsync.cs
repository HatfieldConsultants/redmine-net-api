

#if !(NET20 || NET40)
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Redmine.Api.Extensions;

namespace Redmine.Api
{
    internal partial class HttpRequester : IHttpRequesterAsync
    {
        #region Implementation of IHttpRequesterAsync

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IHttpResponse> GetAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            return await SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IHttpResponse> CreateAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            return await SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IHttpResponse> PatchAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            return await SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IHttpResponse> UpdateAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            return await SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IHttpResponse> DeleteAsync(IHttpRequest request, CancellationToken cancellationToken = default)
        {
            return await SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        private async Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            using (var requestMessage = CreateHttpRequestMessage(request))
            {
                CancellationTokenSource timeoutCancellationTokenSource = null;
                CancellationTokenSource linkedCancellationToken= null;
                var cancellationTokenForRequest = cancellationToken;

                if (request.Timeout != TimeSpan.Zero)
                {
                    timeoutCancellationTokenSource = new CancellationTokenSource(request.Timeout);
                    linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token);

                    cancellationTokenForRequest = linkedCancellationToken.Token;
                }

                try
                {
                    using (var responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationTokenForRequest).ConfigureAwait(false))
                    {
                        return await CreateHttpResponseAsync(responseMessage).ConfigureAwait(false);
                    }
                }
                finally
                {
                    timeoutCancellationTokenSource?.Dispose();
                    linkedCancellationToken?.Dispose();
                }
            }
        }


        private static async Task<IHttpResponse> CreateHttpResponseAsync(HttpResponseMessage responseMessage)
        {
            string body = null;
            string contentType = null;

            if (responseMessage.Content != null)
            {
                contentType = GetContentMediaType(responseMessage.Content);
                using(var stream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    body = await stream.ReadStringAsync().ConfigureAwait(false);
                }
            }

            return new HttpResponse(responseMessage.StatusCode, body, responseMessage.Headers.ToDictionary(h => h.Key, h => h.Value.First()), contentType);
        }

       
    }
}
#endif