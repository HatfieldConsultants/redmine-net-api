using System;
using System.Net;
using System.Text;
using System.Threading;
using Redmine.Api.Exceptions;
using Redmine.Api.Extensions;
using Redmine.Api.Internals.Serialization;
using Redmine.Api.Types;
#if !(NET20)
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#endif


#if !(NET20 || NET40)
namespace Redmine.Api
{
    internal sealed class WebClient : IWebClient
    {
        private readonly RedmineConnectionSettings connectionSettings;
        private readonly IRedmineSerializer serializer;

        private readonly HttpClient httpClient;
        private readonly bool disposeHandler;

        /// <summary>
        /// 
        /// </summary>
        public WebClient(RedmineConnectionSettings connectionSettings, IRedmineSerializer serializer)
        {
            this.connectionSettings = connectionSettings;
            this.serializer = serializer;

            if (connectionSettings.HttpClient != null)
            {
                httpClient = connectionSettings.HttpClient;
                disposeHandler = false;
            }
            else
            {
                disposeHandler = true;
                
//                ServicePointManager.FindServicePoint(endpoint)  
//                    .ConnectionLeaseTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
//                
//                ServicePointManager.DnsRefreshTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
//                
                
                httpClient = new HttpClient(createHttpClientHandler());
                httpClient.Timeout = connectionSettings.Timeout.GetValueOrDefault(TimeSpan.FromSeconds(30));

                if (connectionSettings.ProtocolVersion != null)
                {
                    httpClient.DefaultRequestHeaders.Add("Version", connectionSettings.ProtocolVersion.ToString());
                }

                if (!connectionSettings.Referer.IsNullOrWhiteSpace())
                {
                    httpClient.DefaultRequestHeaders.Add("Referrer", connectionSettings.Referer);
                }

                if (!connectionSettings.UserAgent.IsNullOrWhiteSpace())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", connectionSettings.UserAgent);
                }
            }
            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(connectionSettings.ContentType));

            if (connectionSettings.UseApiKey)
            {
                if (!connectionSettings.ApiKey.IsNullOrWhiteSpace())
                {
                    httpClient.DefaultRequestHeaders.Add(RedmineKeys.KEY, connectionSettings.ApiKey);
                }
                else
                {
                    throw new ArgumentNullException(nameof(connectionSettings.ApiKey),"Api key cannot be null or empty");
                }
            }
            else
            { 
                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{connectionSettings.UserName}:{connectionSettings.Password}"));
                var basicAuthorization = $"Basic {token}";
                httpClient.DefaultRequestHeaders.Add("Authorization", basicAuthorization);
            }
        }

        private HttpClientHandler createHttpClientHandler()
        {
            var request = new HttpClientHandler();
            request.AllowAutoRedirect = connectionSettings.AllowAutoRedirect;


            if (request.SupportsAutomaticDecompression)
            {
                request.AutomaticDecompression = connectionSettings.AutomaticDecompression;
            }

#if !(NET20 || NET40)
            
            request.ClientCertificateOptions = connectionSettings.ClientCertificateOptions;
            
#endif
            request.Credentials = connectionSettings.Credentials;

            if (connectionSettings.MaximumAutomaticRedirections > 0)
            {
#if(NET20 || NET40)
                request.MaximumAutomaticRedirections = connectionSettings.MaximumAutomaticRedirections;
#else

                request.MaxAutomaticRedirections = connectionSettings.MaximumAutomaticRedirections;
#endif
            }
            //  request.AllowWriteStreamBuffering = connectionSettings.AllowWriteStreamBuffering;
            //  request.KeepAlive = connectionSettings.KeepAlive;
            //  request.MediaType = connectionSettings.MediaType;
            //  request.Pipelined = connectionSettings.Pipelined;
            //  request.ProtocolVersion = connectionSettings.ProtocolVersion;
            //  request.ReadWriteTimeout = connectionSettings.ReadWriteTimeout;
            //  request.Referer = connectionSettings.Referer;
            //  request.AllowReadStreamBuffering = connectionSettings.AllowReadStreamBuffering;
            //  request.ContinueTimeout = connectionSettings.ContinueTimeout;
            //  request.ServerCertificateValidationCallback = connectionSettings.ServerCertificateValidationCallback;
            //  request.UserAgent = connectionSettings.UserAgent;

            request.PreAuthenticate = connectionSettings.PreAuthenticate;

            request.UseDefaultCredentials = connectionSettings.UseDefaultCredentials;
            
#if!(NET20 || NET40 || NET45 || NET451 || NET452)
            
            request.SslProtocols = connectionSettings.SslProtocols;
            request.CheckCertificateRevocationList = connectionSettings.CheckCertificateRevocationList;
            request.ServerCertificateCustomValidationCallback = connectionSettings.ServerCertificateCustomValidationCallback;
            #else
            
            
            
#endif
            if (connectionSettings.UseProxy)
            {
                if (connectionSettings.WebProxy != null)
                {
                    request.Proxy = connectionSettings.WebProxy;
                }
            }

            if (connectionSettings.UseCookies)
            {
                request.CookieContainer = connectionSettings.CookieContainer;
            }
            
            
           

            return request;
        }

        public string Get(string uri)
        {
            throw new System.NotImplementedException();
        }

        public string Create<T>(string uri, T data) where T : class
        {
            throw new System.NotImplementedException();
        }

        public string Update<T>(string uri, T data) where T : class
        {
            throw new System.NotImplementedException();
        }

        public string Patch<T>(string uri, T data) where T : class
        {
            throw new System.NotImplementedException();
        }

        public string Delete<T>(string uri)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetAsync(string uri, CancellationToken cancellationToken = default)
        {
            var request = InitHttpRequestMessage(uri, HttpVerbs.GET);
            var response = await httpClient.SendAsync(request,cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            }
            
            throw new RedmineException("");
        }

        public async Task<string> Create<T>(string uri, T data, CancellationToken cancellationToken = default) where T : class
        {
            var payload = new StringContent(serializer.Serialize(data), Encoding.UTF8);
            var request = InitHttpRequestMessage(uri, HttpVerbs.POST);
            request.Content = payload;
            
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            }
            
            throw new RedmineException("");
        }

        public async Task<string> Update<T>(string uri, T data, CancellationToken cancellationToken = default) where T : class
        {
            var payload = new StringContent(serializer.Serialize(data), Encoding.UTF8);
            var request = InitHttpRequestMessage(uri, HttpVerbs.PUT);
            request.Content = payload;
            
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            }
            
            throw new RedmineException("");
        }

        public async  Task<string> Patch<T>(string uri, T data, CancellationToken cancellationToken = default) where T : class
        {
            var payload = new StringContent(serializer.Serialize(data), Encoding.UTF8);
            var request = InitHttpRequestMessage(uri, HttpVerbs.PATCH);
            request.Content = payload;
            
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            }
            
            throw new RedmineException("");
        }

        public async Task<string> Delete(string uri, CancellationToken cancellationToken = default)
        {
            var request = InitHttpRequestMessage(uri, HttpVerbs.DELETE);
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            }
            
            throw new RedmineException("");
        }
        
       
        

        public async Task<byte[]> DownloadAsync(string uri, CancellationToken cancellationToken = default) 
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
            
            throw new RedmineException("");
        }

        public async Task<Upload> UploadAsync(string uri, byte[] data, CancellationToken cancellationToken = default)
        {
            var payload = new StringContent(serializer.Serialize(data), Encoding.UTF8);
            var request = InitHttpRequestMessage(uri, HttpVerbs.POST);
            request.Content = payload;
            
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            
            throw new RedmineException("");
        }

      
        private HttpRequestMessage InitHttpRequestMessage(string requestUri, string method)
        {
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method)
            };

            request.RequestUri = new Uri(requestUri);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(connectionSettings.ContentType);
            
            return request;
        }

        public void Dispose()
        {
            if (disposeHandler)
            {
                httpClient?.Dispose();
            }
        }
    }
    
    
    
}
#endif