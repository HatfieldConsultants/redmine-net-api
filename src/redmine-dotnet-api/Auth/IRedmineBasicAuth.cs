namespace Redmine.dotNet.Api;

public interface IRedmineBasicAuth : IRedmineAuth
{
    string Username { get; }
    string Password { get; }
}