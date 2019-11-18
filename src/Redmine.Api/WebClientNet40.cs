/*
Copyright 2011 - 2019 Adrian Popescu.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/



#if NET40
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Redmine.Api.Exceptions;
using Redmine.Api.Extensions;
using Redmine.Api.Internals.Serialization;


namespace Redmine.Api
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class WebClient : IWebClient
    {
        private readonly RedmineConnectionSettings connectionSettings;
        private readonly IRedmineSerializer serializer;

        /// <summary>
        /// 
        /// </summary>
        public WebClient(RedmineConnectionSettings connectionSettings,IRedmineSerializer serializer)
        {
            this.connectionSettings = connectionSettings;
            this.serializer = serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string Get(string uri)
        {
            try
            {
                return GetResponse(InitWebRequest(HttpVerbs.GET, uri));
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string Create<T>(string uri, T data) where T: class
        {
            var request = InitWebRequest(HttpVerbs.POST, uri);

            var payload = serializer.Serialize(data);

            if (!payload.IsNullOrWhiteSpace())
            {
                try
                {
                    WriteToRequestStream(request, payload);
                }
                catch (Exception exception)
                {
                    HandleException(exception);
                }
            }
            
            return GetResponse(request);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string Update<T>(string uri, T data) where T: class
        {
            var request = InitWebRequest(HttpVerbs.PUT, uri);
           
            var payload = serializer.Serialize(data);

            if (!payload.IsNullOrWhiteSpace())
            {
                try
                {
                    WriteToRequestStream(request, payload);
                }
                catch (Exception exception)
                {
                    HandleException(exception);
                }
            }
           
            return GetResponse(request);
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string Patch<T>(string uri, T data) where T: class
        {
            var request = InitWebRequest(HttpVerbs.PATCH, uri);
            
            var payload = serializer.Serialize(data);

            if (!payload.IsNullOrWhiteSpace())
            {
                try
                {
                    WriteToRequestStream(request, payload);
                }
                catch (Exception exception)
                {
                    HandleException(exception);
                }
            }
            
            return GetResponse(request);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string Delete<T>(string uri)
        {
            var request = InitWebRequest(HttpVerbs.POST, uri);

            return GetResponse(request);
        }

        public Task<string> GetAsync(string uri, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> Create<T>(string uri, T data, CancellationToken cancellationToken = default) where T : class
        {
           
            
            throw new RedmineException("");
        }

        public Task<string> Update<T>(string uri, T data, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<string> Patch<T>(string uri, T data, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<string> Delete(string uri, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> Delete<T>(string uri, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
        
        
        private HttpWebRequest InitWebRequest(string method, string requestUri)
        {
            var uri = new Uri(requestUri);

            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = method;
            
            request.Timeout = connectionSettings.Timeout.GetValueOrDefault(TimeSpan.FromSeconds(30)).Seconds;
            request.AllowAutoRedirect = connectionSettings.AllowAutoRedirect;
            
            request.AllowWriteStreamBuffering = connectionSettings.AllowWriteStreamBuffering;
            request.AutomaticDecompression = connectionSettings.AutomaticDecompression;
            request.ClientCertificates = connectionSettings.ClientCertificates;
            request.ContentType = connectionSettings.ContentType;
            
            request.Credentials = connectionSettings.Credentials;
            request.KeepAlive = connectionSettings.KeepAlive;
            request.MaximumAutomaticRedirections = connectionSettings.MaximumAutomaticRedirections;
            request.MediaType = connectionSettings.MediaType;
            request.Pipelined = connectionSettings.Pipelined;
            request.PreAuthenticate = connectionSettings.PreAuthenticate;
            request.ProtocolVersion = connectionSettings.ProtocolVersion;
            request.ReadWriteTimeout = connectionSettings.ReadWriteTimeout;
            request.Referer = connectionSettings.Referer;
            
            #if !(NET20 || NET40)
                request.AllowReadStreamBuffering = connectionSettings.AllowReadStreamBuffering;
                request.ContinueTimeout = connectionSettings.ContinueTimeout;
                request.ServerCertificateValidationCallback = connectionSettings.ServerCertificateValidationCallback;
            #else
                ServicePointManager.ServerCertificateValidationCallback = connectionSettings.ServerCertificateValidationCallback;
            #endif
            
            request.UseDefaultCredentials = connectionSettings.UseDefaultCredentials;
            request.UserAgent = connectionSettings.UserAgent;
                
            if (connectionSettings.UseProxy)
            {
                if (connectionSettings.WebProxy != null)
                {
                    request.Proxy = connectionSettings.WebProxy;
                }
            }

            if (connectionSettings.UseCookies)
            {
                #if !(NET20 || NET40)
                if (request.SupportsCookieContainer)
                #endif
                {
                    request.CookieContainer = connectionSettings.CookieContainer;
                }
            }

            //TODO: ...
            if (connectionSettings.UseDefaultCredentials)
            {
               // var cache = new CredentialCache { { new Uri(connectionSettings.Host), "Basic", new NetworkCredential(connectionSettings.UserName, connectionSettings.Password) } };

//                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{connectionSettings.UserName}:{connectionSettings.Password}"));
//                var basicAuthorization = $"Basic {token}";
            }

            request.CachePolicy = connectionSettings.DefaultCachePolicy;
            return request;
        }

        private static string GetResponse(HttpWebRequest request)
        {
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        #if NET20
        private static string GetResponseAsync(HttpWebRequest request)
        {
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        #elif NET40
        
        private static  string GetResponseAsync(HttpWebRequest request)
        {
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    return  streamReader.ReadToEnd();
                }
            }
        }
        
        #else
         private static async Task<string> GetResponseAsync(HttpWebRequest request)
        {
            var response = await request.GetResponseAsync();
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }
        #endif

        
        private static void WriteToRequestStream(HttpWebRequest request, string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        
        #if NET20
        
        private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
    {
        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
        
        // End the operation
        using (Stream postStream = request.EndGetRequestStream(asynchronousResult))
        {
            Console.WriteLine("Please enter the input data to be posted:");
            string postData = "testdata";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            postStream.Write(byteArray, 0, postData.Length);
        }

        // Start the asynchronous operation to get the response
        request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
    }

    private static void GetResponseCallback(IAsyncResult asynchronousResult)
    {
        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

        // End the operation
        using (HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(asynchronousResult))
        {
            using (Stream streamResponse = response.GetResponseStream())
            {
                using (StreamReader streamRead = new StreamReader(streamResponse))
                {
                    string responseString = streamRead.ReadToEnd();
                    Console.WriteLine(responseString);
                }
            }
        }
        //TODO: success callback
    }
    
    private static void WriteToRequestStreamAsync(HttpWebRequest request, string payload)
    {
        var bytes = Encoding.UTF8.GetBytes(payload);
        request.ContentLength = bytes.Length;
        request.BeginGetRequestStream(GetRequestStreamCallback,request);
    }
    
        
#elif NET40
#else
        private static async Task WriteToRequestStreamAsync(HttpWebRequest request, string payload, CancellationToken cancellationToken = default)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = bytes.Length;
            using (var stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            }
        }
#endif
        
        
        private void HandleException(Exception exception)
        {
            if (!(exception is WebException))
            {
                throw new RedmineException(exception.Message, exception);
            }

            var webException = (WebException) exception;
            var status = webException.Status;
            if (webException.Response != null)
            {
                var response = (HttpWebResponse) webException.Response;
                var responseCode = (int) response.StatusCode;
                
                throw new RedmineException($"");
            }

            throw new RedmineException();
        }
        
         public void Dispose()
        {
            
        }
    }
}
#endif