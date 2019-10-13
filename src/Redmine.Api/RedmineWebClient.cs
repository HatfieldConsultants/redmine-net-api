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

using System;
using System.IO;
using System.Net;
using System.Text;
using Redmine.Api.Exceptions;
using Redmine.Api.Extensions;
using Redmine.Api.Internals.Serialization;
using Redmine.Api.Types;

namespace Redmine.Api
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Net.WebClient" />
    public class RedmineWebClient : System.Net.WebClient
    {
        private const string UA = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:8.0) Gecko/20100101 Firefox/8.0";

        /// <summary>
        /// 
        /// </summary>
        public RedmineWebClient()
        {
            UserAgent = UA;
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [use proxy].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [use proxy]; otherwise, <c>false</c>.
        /// </value>
        public bool UseProxy { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [use cookies].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [use cookies]; otherwise, <c>false</c>.
        /// </value>
        public bool UseCookies { get; set; }

        /// <summary>
        ///     in miliseconds
        /// </summary>
        /// <value>
        ///     The timeout.
        /// </value>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        ///     Gets or sets the cookie container.
        /// </summary>
        /// <value>
        ///     The cookie container.
        /// </value>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [pre authenticate].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [pre authenticate]; otherwise, <c>false</c>.
        /// </value>
        public bool PreAuthenticate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [keep alive].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [keep alive]; otherwise, <c>false</c>.
        /// </value>
        public bool KeepAlive { get; set; }

        /// <summary>
        ///     Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
        /// <returns>
        ///     A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var wr = base.GetWebRequest(address);
            var httpWebRequest = wr as HttpWebRequest;

            if (httpWebRequest != null)
            {
                if (UseCookies)
                {
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, "redmineCookie");
                    httpWebRequest.CookieContainer = CookieContainer;
                }
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate |
                                                        DecompressionMethods.None;
                httpWebRequest.PreAuthenticate = PreAuthenticate;
                httpWebRequest.KeepAlive = KeepAlive;
                httpWebRequest.UseDefaultCredentials = UseDefaultCredentials;
                httpWebRequest.Credentials = Credentials;
                httpWebRequest.UserAgent = UA;
                httpWebRequest.CachePolicy = CachePolicy;

                if (UseProxy)
                {
                    if (Proxy != null)
                    {
                        Proxy.Credentials = Credentials;
                    }
                    httpWebRequest.Proxy = Proxy;
                }

                if (Timeout != null)
                    httpWebRequest.Timeout = Timeout.Value.Milliseconds;

                return httpWebRequest;
            }

            return base.GetWebRequest(address);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWebClient
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class WebClient : IWebClient
    {
        private readonly IRedmineSerializer serializer;

        /// <summary>
        /// 
        /// </summary>
        public WebClient(IRedmineSerializer serializer)
        {
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

        private HttpWebRequest InitWebRequest(string method, string requestUri)
        {
            var uri = new Uri(requestUri);

            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = method;
            //request.Timeout = ;
            
            return request;
        }

        private static string GetResponse(HttpWebRequest request)
        {
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream, utf8Encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        private static void WriteToRequestStream(HttpWebRequest request, string payload)
        {
            var bytes = utf8Encoding.GetBytes(payload);
            request.ContentType = "text/xml";
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

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
        
        private static readonly UTF8Encoding utf8Encoding = new UTF8Encoding();
    }
}