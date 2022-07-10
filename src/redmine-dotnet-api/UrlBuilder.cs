using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Redmine.dotNet.Api;

internal sealed class UrlBuilder : IUrlBuilder
{
    private readonly string host;

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

    /// <summary>
    /// 
    /// </summary>
    private const string MY_ACCOUNT_FORMAT = "{0}/my/account.{1}";
    
    /// <summary>
    /// </summary>
    private const string CURRENT_USER_URI = "current";

    private readonly string format;
    
    
    private static readonly Dictionary<Type, string> mappingRouteType = new Dictionary<Type, string>
    {
        // {typeof(Issue), "issues"},
        // {typeof(Project), "projects"},
        // {typeof(User), "users"},
        // {typeof(News), "news"},
        // {typeof(Query), "queries"},
        // {typeof(Version), "versions"},
        // {typeof(Attachment), "attachments"},
        // {typeof(IssueRelation), "relations"},
        // {typeof(TimeEntry), "time_entries"},
        // {typeof(IssueStatus), "issue_statuses"},
        // {typeof(Tracker), "trackers"},
        // {typeof(IssueCategory), "issue_categories"},
        // {typeof(Role), "roles"},
        // {typeof(ProjectMembership), "memberships"},
        // {typeof(Group), "groups"},
        // {typeof(TimeEntryActivity), "enumerations/time_entry_activities"},
        // {typeof(IssuePriority), "enumerations/issue_priorities"},
        // {typeof(Watcher), "watchers"},
        // {typeof(IssueCustomField), "custom_fields"},
        // {typeof(CustomField), "custom_fields"}
    };

    /// <summary>
    /// 
    /// </summary>
    public static readonly Dictionary<Type, bool> mappingRouteTypeWithOffset = new Dictionary<Type, bool>{
        // {typeof(Issue), true},
        // {typeof(Project), true},
        // {typeof(User), true},
        // {typeof(News), true},
        // {typeof(Query), true},
        // {typeof(TimeEntry), true},
        // {typeof(ProjectMembership), true}
    };

    public UrlBuilder(string host,RedmineSerializationType redmineSerializationType)
    {
        this.host = host;
        format = redmineSerializationType switch
        {
            RedmineSerializationType.Xml => "xml",
            RedmineSerializationType.Json => "json",
            _ => throw new ArgumentOutOfRangeException(nameof(redmineSerializationType), redmineSerializationType, null)
        };
    }
    
    
    public string Upload<T>(int id) where T: class
    {
        var x = GetTypeAndRoute<T>();
        
        return Get(x.type, id, x.route);
    }
    
    public string Delete<T>(int id) where T: class
    {
        var x = GetTypeAndRoute<T>();
        
        return Get(x.type, id, x.route);
    }
     
    public string Get<T>(int id) where T: class
    {
        var x = GetTypeAndRoute<T>();
        
        return Get(x.type, id, x.route);
    }
    
    public string Get(Type type, int id, string route)
    {
        return string.Format(CultureInfo.InvariantCulture,REQUEST_FORMAT, host, route, id.ToString(CultureInfo.InvariantCulture), format);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerOrProjectId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="RedmineException"></exception>
    public string Create<T>(int? ownerOrProjectId = null)
    {
        var x = GetTypeAndRoute<T>();
        
        // if (type == typeof(Version) || type == typeof(IssueCategory) || type == typeof(ProjectMembership))
        // {
        //     if (ownerOrProjectId == null)
        //     {
        //         throw new RedmineException("The owner id(project id) is mandatory!");
        //     }
        //     return string.Format(CultureInfo.InvariantCulture,ENTITY_WITH_PARENT_FORMAT, host, RedmineKeys.PROJECTS, ownerOrProjectId.Value.ToString(CultureInfo.InvariantCulture), route, format);
        // }
        //
        // if (type == typeof(IssueRelation))
        // {
        //     if (ownerOrProjectId == null)
        //     {
        //         throw new RedmineException("The owner id(issue id) is mandatory!");
        //     }
        //     
        //     return string.Format(CultureInfo.InvariantCulture,ENTITY_WITH_PARENT_FORMAT, host, RedmineKeys.ISSUES, ownerOrProjectId.Value.ToString(CultureInfo.InvariantCulture), route, format);
        // }
        //
        // if (type != typeof(File))
        // {
        //     return string.Format(CultureInfo.InvariantCulture, FORMAT, host, route, format);
        // }
        //
        if (ownerOrProjectId == null)
        {
            throw new RedmineException("The owner id(project id) is mandatory!");
        }
        return string.Format(CultureInfo.InvariantCulture,FILE_URL_FORMAT, host, ownerOrProjectId.Value.ToString(CultureInfo.InvariantCulture), format);

    }

    private static (Type type,string route) GetTypeAndRoute<T>()
    {
        var type = typeof(T);

        if (!mappingRouteType.TryGetValue(type, out var route))
        {
            throw new KeyNotFoundException(type.Name);
        }

        return (type, route);
    }
    
    public string List<T>(NameValueCollection parameters)
    {
        var x = GetTypeAndRoute<T>();
        
        // if (x.type == typeof(Version) || x.type == typeof(IssueCategory) || x.type == typeof(ProjectMembership))
        // {
        //     var projectId = parameters.GetParameterValue(RedmineKeys.PROJECT_ID);
        //     if (string.IsNullOrEmpty(projectId))
        //         throw new RedmineException("The project id is mandatory!\nCheck if you have included the parameter project_id to parameters.");
        //
        //     return string.Format(CultureInfo.InvariantCulture,ENTITY_WITH_PARENT_FORMAT, host, RedmineKeys.PROJECTS, projectId, x.route,format);
        // }
        // if (x.type == typeof(IssueRelation))
        // {
        //     var issueId = parameters.GetParameterValue(RedmineKeys.ISSUE_ID);
        //     if (string.IsNullOrEmpty(issueId))
        //         throw new RedmineException("The issue id is mandatory!\nCheck if you have included the parameter issue_id to parameters");
        //
        //     return string.Format(CultureInfo.InvariantCulture,ENTITY_WITH_PARENT_FORMAT, host, RedmineKeys.ISSUES, issueId, x.route, format);
        // }
        //
        // if (x.type == typeof(File))
        // {
        //     var projectId = parameters.GetParameterValue(RedmineKeys.PROJECT_ID);
        //     if (string.IsNullOrEmpty(projectId))
        //     {
        //         throw new RedmineException("The project id is mandatory!\nCheck if you have included the parameter project_id to parameters.");
        //     }
        //     return string.Format(CultureInfo.InvariantCulture,FILE_URL_FORMAT, host, projectId, format);
        // }
            
        return string.Format(CultureInfo.InvariantCulture,FORMAT, host, x.route, format);
    }
    
    public string GetWikis(int projectId)
    {
        return string.Format(CultureInfo.InvariantCulture,WIKI_INDEX_FORMAT, host, projectId.ToString(CultureInfo.InvariantCulture), format);
    } 
    
    public string GetWikiPage(int projectId, string pageName, uint version = 0)
    {
        var uri = version == 0
            ? string.Format(CultureInfo.InvariantCulture,WIKI_PAGE_FORMAT, host, projectId.ToString(CultureInfo.InvariantCulture), pageName, format)
            : string.Format(CultureInfo.InvariantCulture,WIKI_VERSION_FORMAT, host, projectId.ToString(CultureInfo.InvariantCulture), pageName, version.ToString(CultureInfo.InvariantCulture), format);
        return uri;
    }
    
    public string AddUserToGroup(int groupId)
    {
        return string.Format(CultureInfo.InvariantCulture,REQUEST_FORMAT, host, RedmineKeys.GROUPS, $"{groupId.ToString(CultureInfo.InvariantCulture)}/{RedmineKeys.USERS}", format);
    }
    
    public string RemoveUserFromGroup(int groupId, int userId)
    {
        return string.Format(CultureInfo.InvariantCulture,REQUEST_FORMAT, host, RedmineKeys.GROUPS, $"{groupId.ToString(CultureInfo.InvariantCulture)}/{RedmineKeys.USERS}/{userId.ToString(CultureInfo.InvariantCulture)}", format);
    }
    
    public string UploadFile()
    {
        return string.Format(CultureInfo.InvariantCulture,FORMAT, host, RedmineKeys.UPLOADS, format);
    }
    
    public string CurrentUser()
    {
        return string.Format(CultureInfo.InvariantCulture,REQUEST_FORMAT, host, RedmineKeys.USERS, CURRENT_USER_URI, format);
    }
    
    public string MyAccount()
    {
        return string.Format(CultureInfo.InvariantCulture,MY_ACCOUNT_FORMAT, host, format);
    }
 
    public string WikiCreateOrUpdate(int projectId, string pageName)
    {
        return string.Format(CultureInfo.InvariantCulture,WIKI_PAGE_FORMAT, host, projectId.ToString(CultureInfo.InvariantCulture), pageName, format);
    }
    
    public string DeleteWiki(int projectId, string pageName)
    {
        return string.Format(CultureInfo.InvariantCulture,WIKI_PAGE_FORMAT, host, projectId.ToString(CultureInfo.InvariantCulture), pageName, format);
    }
   
    public string AddWatcherToIssue(int issueId)
    {
        return string.Format(CultureInfo.InvariantCulture,REQUEST_FORMAT, host, RedmineKeys.ISSUES, $"{issueId.ToString(CultureInfo.InvariantCulture)}/{RedmineKeys.WATCHERS}",format);
    }
 
    public string RemoveWatcherFromIssue(int issueId, int userId)
    {
        return string.Format(CultureInfo.InvariantCulture,REQUEST_FORMAT, host, RedmineKeys.ISSUES, $"{issueId.ToString(CultureInfo.InvariantCulture)}/{RedmineKeys.WATCHERS}/{userId.ToString(CultureInfo.InvariantCulture)}",format);
    }
    
    public string IssueAttachmentUpdate(int issueId)
    {
        return string.Format(CultureInfo.InvariantCulture,ATTACHMENT_UPDATE_FORMAT, host, issueId.ToString(CultureInfo.InvariantCulture),format);
    }
}