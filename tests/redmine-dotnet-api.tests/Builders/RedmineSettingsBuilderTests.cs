using Redmine.dotNet.Api;

namespace redmine_dotnet_api.tests;

public class RedmineSettingsBuilderTests
{
    [Fact]
    public void TestHostIsNotValid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            IRedmineSettingsBuilder redmineSettingsBuilder = new RedmineSettingsBuilder();
           
            redmineSettingsBuilder.Host = "test";

            redmineSettingsBuilder.Build();
        });
    }
}