namespace Redmine.dotNet.Api;

public interface IRedmineApiKeyAuth : IRedmineAuth
{
    string ApiKey { get; }
}