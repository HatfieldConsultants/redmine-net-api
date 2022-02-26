#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
public sealed class RedmineApiConfigBuilder
{
    private const int DEFAULT_PAGE_SIZE_VALUE = 25;

    /// <value>
    ///     The size of the page.
    /// </value>
    public int PageSize { get; set; } = DEFAULT_PAGE_SIZE_VALUE;
        
    /// <summary>
    /// 
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RedmineApiCredentials Credentials { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RedmineSerializationType SerializationType { get; set; } = RedmineSerializationType.Json;

    /// <summary>
    /// 
    /// </summary>
    public HttpClient? HttpClient  { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public IWebProxy? Proxy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public SecurityProtocolType SecurityProtocolType { get; set; } = SecurityProtocolType.Tls12;

   /// <summary>
   /// 
   /// </summary>
   public CookieContainer? CookieContainer { get; set; }

   /// <summary>
   /// 
   /// </summary>
   public ClientCertificateOption ClientCertificateOption { get; set; } = ClientCertificateOption.Manual;

   /// <summary>
   /// 
   /// </summary>
   public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12;
   
   /// <summary>
   /// 
   /// </summary>
   public bool AllowAutoRedirect { get; set; }
   /// <summary>
   /// 
   /// </summary>
   public int? MaxAutomaticRedirections { get; set; }
   /// <summary>
   /// 
   /// </summary>
   public bool CheckCertificateRevocationList { get; set; }
   /// <summary>
   /// 
   /// </summary>
   public int? MaxConnectionsPerServer { get; set; }
   /// <summary>
   /// 
   /// </summary>
   public long? MaxRequestContentBufferSize { get; set; }
   /// <summary>
   /// 
   /// </summary>
   public int? MaxResponseHeadersLength { get; set; }
   
   /// <summary>
   /// 
   /// </summary>
   public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>? ServerCertificateCustomValidationCallback { get; set; }

   /// <summary>
   /// 
   /// </summary>
   public string? UserAgent { get; set; }
}