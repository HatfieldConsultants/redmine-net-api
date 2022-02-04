using System;

namespace Redmine.Net.Api;

internal class AuthenticatorFactory
{
    public IAuthenticationHandler GetRedmineAuthenticatorHandler(AuthenticationType authenticationType)
    {
        return authenticationType switch
        {
            AuthenticationType.ApiKey => Create<ApiKeyAuthenticator>(),
            AuthenticationType.Basic => Create<BasicAuthenticator>(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private T Create<T>()
        where T : new()
    {
        return Cache<T>.Instance;
    }

    private static class Cache<T>
        where T : new()
    {
        public static readonly T Instance = new();
    }
}