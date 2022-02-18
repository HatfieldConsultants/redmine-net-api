using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using Redmine.Net.Api.Exceptions;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Types;
using Version = Redmine.Net.Api.Types.Version;


namespace Redmine.Net.Api;

internal static class RedmineApiUrls
{
    /// <summary>
    /// </summary>
    private const string REQUEST_FORMAT = "{0}/{1}/{2}.{3}";

    /// <summary>
    /// </summary>
    private const string FORMAT = "{0}/{1}.{2}";

    /// <summary>
    /// </summary>
    private const string WIKI_INDEX_FORMAT = "{0}/projects/{1}/wiki/index.{2}";

    /// <summary>
    /// </summary>
    private const string WIKI_PAGE_FORMAT = "{0}/projects/{1}/wiki/{2}.{3}";

    /// <summary>
    /// </summary>
    private const string WIKI_VERSION_FORMAT = "{0}/projects/{1}/wiki/{2}/{3}.{4}";

    /// <summary>
    /// </summary>
    private const string ENTITY_WITH_PARENT_FORMAT = "{0}/{1}/{2}/{3}.{4}";

    /// <summary>
    /// </summary>
    private const string ATTACHMENT_UPDATE_FORMAT = "{0}/attachments/issues/{1}.{2}";

    /// <summary>
    /// 
    /// </summary>
    private const string FILE_URL_FORMAT = "{0}/projects/{1}/files.{2}";

    private const string MY_ACCOUNT_FORMAT = "{0}/my/account.{1}";


    /// <summary>
    /// </summary>
    private const string CURRENT_USER_URI = "current";


    public static string MyAccount()
    {
        return "my/account";
    }

    public static string CurrentUser()
    {
        return CURRENT_USER_URI;
    }

    public static string ProjectWikiIndex(string projectId)
    {
        return $"projects/{projectId}/wiki/index";
    }

    public static string ProjectWikiPage(string projectId, string wikiPageName)
    {
        return $"projects/{projectId}/wiki/{wikiPageName}";
    }

    public static string ProjectWikiPageVersion(string projectId, string wikiPageName, string version)
    {
        return $"projects/{projectId}/wiki/{wikiPageName}/{version}";
    }

    public static string ProjectFiles(string projectId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new RedmineException("The owner id(project id) is mandatory!");
        }

        return $"projects/{projectId}/files";
    }

    public static string IssueAttachment(string issueId)
    {
        if (string.IsNullOrWhiteSpace(issueId))
        {
            throw new RedmineException("The issue id is mandatory!");
        }

        return $"/attachments/issues/{issueId}";
    }

    public static string ProjectParent(string projectId, string mapTypeFragment)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new RedmineException("The owner project id is mandatory!");
        }

        return $"{RedmineKeys.PROJECTS}/{projectId}/{mapTypeFragment}";
    }

    public static string IssueParent(string issueId, string mapTypeFragment)
    {
        if (string.IsNullOrEmpty(issueId))
        {
            throw new RedmineException("The owner issue id is mandatory!");
        }

        return $"{RedmineKeys.ISSUES}/{issueId}/{mapTypeFragment}";
    }

    public static string TypeFragment(Dictionary<Type, string> mapTypeUrlFragments, Type type)
    {
        if (!mapTypeUrlFragments.TryGetValue(type, out var fragment))
        {
            throw new KeyNotFoundException(type.Name);
        }

        return fragment;
    }

    public static string CreateEntityUrl<T>(Dictionary<Type, string> mapTypeUrlFragments, string ownerId)
    {
        var type = typeof(T);

        if (type == typeof(Version) || type == typeof(IssueCategory) || type == typeof(ProjectMembership))
        {
            return ProjectParent(ownerId, mapTypeUrlFragments[type]);
        }

        if (type == typeof(IssueRelation))
        {
            return IssueParent(ownerId, mapTypeUrlFragments[type]);
        }

        if (type == typeof(File))
        {
            return ProjectFiles(ownerId);
        }

        if (type == typeof(Upload))
        {
            return RedmineKeys.UPLOADS;
        }

        if (type == typeof(Attachment) || type == typeof(Attachments))
        {
            return IssueAttachment(ownerId);
        }

        return TypeFragment(mapTypeUrlFragments, type);
    }

    public static string GetEntitiesUrl<T>(Dictionary<Type, string> mapTypeUrlFragments, NameValueCollection parameters)
        where T : class, new()
    {
        var type = typeof(T);

        if (type == typeof(Version) || type == typeof(IssueCategory) || type == typeof(ProjectMembership))
        {
            var projectId = parameters.GetParameterValue(RedmineKeys.PROJECT_ID);
            return ProjectParent(projectId, mapTypeUrlFragments[type]);
        }

        if (type == typeof(IssueRelation))
        {
            var issueId = parameters.GetParameterValue(RedmineKeys.ISSUE_ID);
            return IssueParent(issueId, mapTypeUrlFragments[type]);
        }

        if (type == typeof(File))
        {
            var projectId = parameters.GetParameterValue(RedmineKeys.PROJECT_ID);
            return ProjectFiles(projectId);
        }

        return TypeFragment(mapTypeUrlFragments, type);
    }

    public static string PatchEntityUrl<T>(Dictionary<Type, string> mapTypeUrlFragments, string ownerId)
    {
        var type = typeof(T);

        if (type == typeof(Attachment) || type == typeof(Attachments))
        {
            return IssueAttachment(ownerId);
        }

        throw new ArgumentException($"No endpoint defined for type {type} for PATCH operation.");
    }

    public static string GetEntityUrl<T>(Dictionary<Type, string> mapTypeUrlFragments)
    {
        return null;
    }

    public static string DeleteEntityUrl<T>(Dictionary<Type, string> mapTypeUrlFragments, string id)
    {
        var type = typeof(T);

        return TypeFragment(mapTypeUrlFragments, type);
    }

    public static string UpdateEntityUrl<T>(Dictionary<Type, string> mapTypeEndpoint, object id)
    {
        throw new NotImplementedException();
    }
}