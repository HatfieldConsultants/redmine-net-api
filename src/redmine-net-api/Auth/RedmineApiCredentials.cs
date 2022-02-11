using Redmine.Net.Api.Internals;
using Redmine.Net.Api.Old.Internals;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
public class RedmineApiCredentials: IRedmineApiCredentials
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="apiKey"></param>
    public RedmineApiCredentials(string apiKey) : this(apiKey, AuthenticationType.ApiKey) { }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    public RedmineApiCredentials(string login, string password) : this(login, password, AuthenticationType.Basic) { }

    private RedmineApiCredentials(string apiKey, AuthenticationType authenticationType)
    {
         Ensure.ArgumentNotNullOrEmptyString(apiKey, nameof(apiKey));

        Login = null;
        Password = apiKey;
        AuthenticationType = authenticationType;
    }
        
    private RedmineApiCredentials(string login, string password, AuthenticationType authenticationType)
    {
         Ensure.ArgumentNotNullOrEmptyString(login, nameof(login));
         Ensure.ArgumentNotNullOrEmptyString(password, nameof(password));

        Login = login;
        Password = password;
        AuthenticationType = authenticationType;
    }

    /// <inheritdoc />
    public string Login { get; }

    /// <inheritdoc />
    public string Password { get; }

    /// <inheritdoc />
    public AuthenticationType AuthenticationType { get; }
}