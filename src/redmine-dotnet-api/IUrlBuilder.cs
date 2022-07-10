using System;
using System.Collections.Specialized;

namespace Redmine.dotNet.Api;

internal interface IUrlBuilder
{
    string Create<T>(int? ownerOrProjectId = null);
    string Get<T>(int id) where T : class;
    string List<T>(NameValueCollection parameters);
    string Delete<T>(int id) where T : class;
    string Upload<T>(int id) where T:class;

    string Get(Type type, int id, string route);

    string GetWikis(int projectId);
    string GetWikiPage(int projectId, string pageName, uint version = 0);
    string AddUserToGroup(int groupId);
    string RemoveUserFromGroup(int groupId, int userId);
    string UploadFile();
    string CurrentUser();
    string MyAccount();
    string WikiCreateOrUpdate(int projectId, string pageName);
    string DeleteWiki(int projectId, string pageName);
    string AddWatcherToIssue(int issueId);
    string RemoveWatcherFromIssue(int issueId, int userId);
    string IssueAttachmentUpdate(int issueId);
}