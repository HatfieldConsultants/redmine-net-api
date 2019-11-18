using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
#if !(NET20 )
using System.Net.Http;
#endif
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Redmine.Api.Extensions;

namespace Redmine.Api
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RedmineConnectionSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MimeFormat Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IWebProxy WebProxy { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        public SecurityProtocolType SecurityProtocolType { get; set; }

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
        ///     Sets the timeout in seconds.
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
        /// 
        /// </summary>
        public bool AllowAutoRedirect { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool AllowWriteStreamBuffering { get; set; }
        
       

        /// <summary>
        /// 
        /// </summary>
        public DecompressionMethods AutomaticDecompression { get; set; } =
            DecompressionMethods.None | DecompressionMethods.GZip | DecompressionMethods.Deflate;


        private static string contentType; 
        
        /// <summary>
        /// 
        /// </summary>
        internal string ContentType 
        {
            get
            {
                if (contentType.IsNullOrWhiteSpace())
                {
                    contentType = Format == MimeFormat.Xml ? "application/xml" : "application/json";
                }
                
                return contentType;
            } 
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ICredentials Credentials { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool CheckCertificateRevocationList { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,string> Headers { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int ReadWriteTimeout { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string MediaType { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public bool Pipelined{ get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public Version ProtocolVersion{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Referer{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool UseDefaultCredentials{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RequestCachePolicy DefaultCachePolicy { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool UseApiKey { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }
        
#if !(NET20)
        /// <summary>
        /// 
        /// </summary>
        public HttpClient HttpClient { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool AllowReadStreamBuffering { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int ContinueTimeout { get; set; }
        
        
        /// <summary>
        /// 
        /// </summary>
        public ClientCertificateOption ClientCertificateOptions { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int MaxConnectionsPerServer { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public RemoteCertificateValidationCallback ServerCertificateValidationCallback{ get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ServerCertificateCustomValidationCallback { get;set;}

        /// <summary>
        /// 
        /// </summary>
        public SslProtocols SslProtocols { get; set; }

      

#else
        /// <summary>
        /// 
        /// </summary>
        public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }
#endif
    }
}