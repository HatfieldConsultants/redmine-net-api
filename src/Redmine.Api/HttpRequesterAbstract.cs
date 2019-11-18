using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
#if!(NET20)
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
#endif
using System.Text;
using Redmine.Api.Extensions;

namespace Redmine.Api
{
    internal abstract class HttpRequesterAbstract : IHttpRequester, IDisposable
    {

        private readonly RedmineConnectionSettings _connectionSettings;
#if !NET20
        protected readonly HttpClient httpClient;
        private readonly bool disposeHandler;
#endif
        private readonly string basicAuthorization;

        /// <summary>
        /// 
        /// </summary>


        protected HttpRequesterAbstract(RedmineConnectionSettings connectionSettings)
        {
            this._connectionSettings = connectionSettings;


            var token = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{connectionSettings.UserName}:{connectionSettings.Password}"));
            basicAuthorization = $"Basic {token}";

            InitializeServicePointManager(connectionSettings);

#if !NET20
           
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

                httpClient = new HttpClient(createHttpClientHandler())
                {
                    Timeout = connectionSettings.Timeout.GetValueOrDefault(TimeSpan.FromSeconds(30))
                };

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
                    throw new ArgumentNullException(nameof(connectionSettings.ApiKey), "Api key cannot be null or empty");
                }
            }
            else
            {
                token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{connectionSettings.UserName}:{connectionSettings.Password}"));
                basicAuthorization = $"Basic {token}";
                httpClient.DefaultRequestHeaders.Add("Authorization", basicAuthorization);
            }
#endif

        }
        
#if !NET20
        private HttpClientHandler createHttpClientHandler()
        {
            var request = new HttpClientHandler
            {
                AllowAutoRedirect = _connectionSettings.AllowAutoRedirect
            };


            if (request.SupportsAutomaticDecompression)
            {
                request.AutomaticDecompression = _connectionSettings.AutomaticDecompression;
            }

#if !(NET20 || NET40)

            request.ClientCertificateOptions = _connectionSettings.ClientCertificateOptions;

#endif
            request.Credentials = _connectionSettings.Credentials;

            if (_connectionSettings.MaximumAutomaticRedirections > 0)
            {

                request.MaxAutomaticRedirections = _connectionSettings.MaximumAutomaticRedirections;

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

            request.PreAuthenticate = _connectionSettings.PreAuthenticate;

            request.UseDefaultCredentials = _connectionSettings.UseDefaultCredentials;

#if !(NET20 || NET40 || NET45 || NET451 || NET452)
            
            request.SslProtocols = connectionSettings.SslProtocols;
            request.CheckCertificateRevocationList = connectionSettings.CheckCertificateRevocationList;
            request.ServerCertificateCustomValidationCallback = connectionSettings.ServerCertificateCustomValidationCallback;
                
#endif
            if (_connectionSettings.UseProxy)
            {
                if (_connectionSettings.WebProxy != null)
                {
                    request.Proxy = _connectionSettings.WebProxy;
                }
            }

            if (_connectionSettings.UseCookies)
            {
                request.CookieContainer = _connectionSettings.CookieContainer;
            }

            return request;
        }
#endif

        private void InitializeServicePointManager(RedmineConnectionSettings connectionSettings)
        {
            ServicePointManager.ServerCertificateValidationCallback = connectionSettings.ServerCertificateValidationCallback;
            ServicePointManager.CheckCertificateRevocationList = connectionSettings.CheckCertificateRevocationList;
            ServicePointManager.SecurityProtocol = connectionSettings.SecurityProtocolType;
            ServicePointManager.DefaultConnectionLimit = 50;
            ServicePointManager.DnsRefreshTimeout = 1;


            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.EnableDnsRoundRobin = true;

#if (NET45 || NET451 || NET452)
            if ((int)ServicePointManager.SecurityProtocol != 0)
            {
                // Add Tls1.2 to the existing enabled protocols
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
#endif

#if !(NET20 || NET40 || NET45 || NET451 || NET452)
            ServicePointManager.ReusePort = true;
#endif
        }

        public void Dispose()
        {

        }

        public IHttpResponse Get(IHttpRequest request)
        {
            var wr = CreateHttpWebRequest(request);
            return GetResponse(wr);
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            var wr = CreateHttpWebRequest(request);

            WriteToRequestStream(wr, request.Payload);

            return GetResponse(wr);
        }

        public IHttpResponse Patch(IHttpRequest request)
        {
            var wr = CreateHttpWebRequest(request);

            WriteToRequestStream(wr, request.Payload);

            return GetResponse(wr);
        }

        public IHttpResponse Update(IHttpRequest request)
        {
            var wr = CreateHttpWebRequest(request);

            WriteToRequestStream(wr, request.Payload);

            return GetResponse(wr);
        }

        public IHttpResponse Delete(IHttpRequest request)
        {
            var wr = CreateHttpWebRequest(request);
            return GetResponse(wr);
        }


        protected HttpWebRequest CreateHttpWebRequest(IHttpRequest apiRequest)
        {
            try
            {
                var uri = new Uri(apiRequest.Uri);

                var request = (HttpWebRequest) WebRequest.Create(uri);

                request.Method = apiRequest.Method;
                request.ContentType = apiRequest.ContentType;

                //  request.Timeout = (int)_connectionSettings.Timeout.GetValueOrDefault(TimeSpan.FromSeconds(90)).TotalSeconds;
                request.AllowAutoRedirect = _connectionSettings.AllowAutoRedirect;
                request.AllowWriteStreamBuffering = _connectionSettings.AllowWriteStreamBuffering;
                request.AutomaticDecompression = _connectionSettings.AutomaticDecompression;

                if (_connectionSettings.ClientCertificates != null)
                {
                    request.ClientCertificates = _connectionSettings.ClientCertificates;
                }

                request.ContentType = _connectionSettings.ContentType;

                request.KeepAlive = _connectionSettings.KeepAlive;

                if (_connectionSettings.MaximumAutomaticRedirections > 0)
                {
                    request.MaximumAutomaticRedirections = _connectionSettings.MaximumAutomaticRedirections;
                }

                request.Pipelined = _connectionSettings.Pipelined;
                request.PreAuthenticate = _connectionSettings.PreAuthenticate;

                if (_connectionSettings.ProtocolVersion != null)
                {
                    request.ProtocolVersion = _connectionSettings.ProtocolVersion;
                }

                if (_connectionSettings.ReadWriteTimeout > 0)
                {
                    request.ReadWriteTimeout = _connectionSettings.ReadWriteTimeout;
                }

                request.Referer = _connectionSettings.Referer;

                if (_connectionSettings.UseApiKey)
                {
                    if (!_connectionSettings.ApiKey.IsNullOrWhiteSpace())
                    {
                        request.Headers.Add("X-Redmine-API-Key", _connectionSettings.ApiKey);
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(_connectionSettings.ApiKey), "Api key cannot be null or empty");
                    }
                }
                else
                {
                    //TODO: check if username & passwd not null or empty
                    request.Headers.Add("Authorization", basicAuthorization);
                }

#if !(NET20 || NET40)
            request.AllowReadStreamBuffering = _connectionSettings.AllowReadStreamBuffering;
            //  request.ContinueTimeout = connectionSettings.ContinueTimeout;
            request.ServerCertificateValidationCallback = _connectionSettings.ServerCertificateValidationCallback;
#endif

                request.UserAgent = _connectionSettings.UserAgent;

                if (_connectionSettings.UseProxy)
                {
                    if (_connectionSettings.WebProxy != null)
                    {
                        var webProxy = _connectionSettings.WebProxy;
                        webProxy.Credentials = _connectionSettings.Credentials ?? CredentialCache.DefaultNetworkCredentials;
                        request.Proxy = webProxy;
                    }
                }

                if (_connectionSettings.UseCookies)
                {
#if !(NET20 || NET40)
                if (request.SupportsCookieContainer)
#endif
                    {
                        request.CookieContainer = _connectionSettings.CookieContainer;
                    }
                }

                request.UseDefaultCredentials = _connectionSettings.UseDefaultCredentials;
                if (!_connectionSettings.UseApiKey && !_connectionSettings.UseDefaultCredentials)
                {

                    request.Credentials = _connectionSettings.Credentials;
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                request.CachePolicy = _connectionSettings.DefaultCachePolicy ?? HttpWebRequest.DefaultCachePolicy;

                return request;
            }
            catch (Exception)
            {
                Exception ex = new Exception(apiRequest.Uri);
                
                throw ex;
            }
        }

        private static IHttpResponse GetResponse(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                return CreateHttpResponse(response);
            }
        }

        protected static IHttpResponse CreateHttpResponse(HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                {
                    return new HttpResponse(response.StatusCode, null, GetResponseHeaders(response.Headers), response.ContentType);
                }

                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    var body = streamReader.ReadToEnd();
                    return new HttpResponse(response.StatusCode, body, GetResponseHeaders (response.Headers), response.ContentType);
                }
            }
        }

        protected static Dictionary<string, string> GetResponseHeaders(WebHeaderCollection webHeaderCollection)
        {
            Dictionary<string,string> responseHeaders = new Dictionary<string, string>();
            foreach (string key in webHeaderCollection.AllKeys)
            {
                responseHeaders.Add(key, webHeaderCollection[key]);
            }

            return responseHeaders;
        }
        
        protected static void WriteToRequestStream(HttpWebRequest request, string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        

#if !NET20
        protected static string GetContentMediaType(HttpContent httpContent)
        {
            return httpContent.Headers?.ContentType?.MediaType;
        }

        protected static HttpRequestMessage CreateHttpRequestMessage(IHttpRequest request)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(request.Method),
                RequestUri = new Uri(request.Uri)
            };

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            if (request.Version != null)
            {
                requestMessage.Version = request.Version;
            }

            if (!request.Payload.IsNullOrWhiteSpace())
            {
                requestMessage.Content = new StringContent(request.Payload, Encoding.UTF8, request.ContentType);
            }

            return requestMessage;
        }
#endif
    }
}