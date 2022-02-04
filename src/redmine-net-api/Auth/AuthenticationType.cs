namespace Redmine.Net.Api;

/// <summary>
/// Authentication protocols supported by the Redmine API
/// </summary>
public enum AuthenticationType
{
    /// <summary>
    /// Redmine api key
    /// </summary>
    ApiKey,

    /// <summary>
    /// Username &amp; password
    /// </summary>
    Basic,
    
    /// <summary>
    /// 
    /// </summary>
    NTLM,
    
    /// <summary>
    /// 
    /// </summary>
    Negotiate
}