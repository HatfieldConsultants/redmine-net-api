using System;

namespace Redmine.dotNet.Api;

public sealed class RedmineApiKeyAuth : IRedmineApiKeyAuth
{
    public RedmineApiKeyAuth(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException(nameof(apiKey));
        }
        ApiKey = apiKey;
    }
    
    public void Authenticate()
    {
        
    }

    public string ApiKey { get; }
}