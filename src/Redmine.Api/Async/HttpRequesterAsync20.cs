
#if NET20
using System.IO;
using System.Net;
using System;
using System.Diagnostics;

namespace Redmine.Api
{
    internal partial class HttpRequester
    {
        private Action<IHttpResponse> httpResponseCallback;
        private Action<Exception> httpResponseErrorCallback;
        
        
        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)((object[])asynchronousResult.AsyncState)[0];
            using (var postStream = request.EndGetRequestStream(asynchronousResult))
            {
                var byteArray = (byte[])((object[])asynchronousResult.AsyncState)[1];

                postStream.Write(byteArray, 0, byteArray.Length);

            }
            request.BeginGetResponse(GetResponseCallback, request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            try
            {
                using (var response = (HttpWebResponse) request.EndGetResponse(asynchronousResult))
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            //   httpResponseCallback(new HttpResponse());
                            return;
                        }

                        using (var reader = new StreamReader(stream))
                        {
                            string responseString = reader.ReadToEnd();

                            //   httpResponseCallback(new HttpResponse());
                        }
                    }
                }

            }
            catch (WebException we)
            {
                string responseString = null;
                using (var stream = we.Response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseString = reader.ReadToEnd();
                            Debug.WriteLine(responseString);
                        }
                    }
                }
                
                httpResponseErrorCallback(we);
            }
        }

        private void SendAsync(IHttpRequest request, Action<IHttpResponse> responseCallback, Action<Exception> errorCallback)
        {
            httpResponseCallback = responseCallback;
            httpResponseErrorCallback = errorCallback;

            var rq = CreateHttpWebRequest(request);
            
            //request-body = POST|PUT|PATCH
            //rq.BeginGetRequestStream(GetRequestStreamCallback, new object[] { request, requestBody });
        }
    }
}
#endif