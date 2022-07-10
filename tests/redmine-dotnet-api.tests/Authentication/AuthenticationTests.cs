using Redmine.dotNet.Api;

namespace redmine_dotnet_api.tests;

public sealed class AuthenticationTests
{
    [Fact]
    public void TestApiKeyAuthNotDefined()
    {
        Assert.Throws<ArgumentException>(() => new RedmineApiKeyAuth(""));
    }

    [Fact]
    public void TestBasicAuthNotDefined()
    {
        Assert.Throws<ArgumentException>(() => new RedmineBasicAuth("", ""));
    }
   
}