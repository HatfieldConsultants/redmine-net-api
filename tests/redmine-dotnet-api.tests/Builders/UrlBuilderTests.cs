using Redmine.dotNet.Api;

namespace redmine_dotnet_api.tests;

public sealed class UrlBuilderTests
{
    [Fact]
    public void TestUrlBuilder()
    {
        IUrlBuilder urlBuilder = new UrlBuilder("https://www.testHost.com" ,RedmineSerializationType.Xml);
        var result =urlBuilder.CurrentUser();
        
        result =urlBuilder.DeleteWiki(1,"test");
        result =urlBuilder.GetWikis(2);
        result =urlBuilder.MyAccount();
        result =urlBuilder.UploadFile();
        result =urlBuilder.IssueAttachmentUpdate(4);
        result =urlBuilder.AddUserToGroup(5);
        result =urlBuilder.AddWatcherToIssue(6);
        result =urlBuilder.RemoveUserFromGroup(7,8);
        result =urlBuilder.RemoveWatcherFromIssue(9,10);
        result =urlBuilder.GetWikiPage(3,"wikiTestPage");
        result =urlBuilder.WikiCreateOrUpdate(3,"wikiTestPage2");
        result =urlBuilder.GetWikiPage(3,"wikiTestPage",1);
        //result =urlBuilder.Get();
    }
}