namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
public interface IRedmineApiCredentials
{
    /// <summary>
    /// 
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// 
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// 
    /// </summary>
    public AuthenticationType AuthenticationType { get; }
}