using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api;

/// <summary>
/// 
/// </summary>
public static class RedmineManagerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="id"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<TOut> GetObjectAsync<TOut>(this RedmineManager rm, int id,
        NameValueCollection parameters = null) where TOut : class, new()
    {
        return await rm.GetObjectAsync<TOut>(id.ToString(CultureInfo.InvariantCulture), parameters);
    }
        
        
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the my account details.</returns>
    public static async Task<MyAccount> GetMyAccountAsync(this RedmineManager rm)
    {
        // var url = UrlHelper.GetMyAccountUrl(this);
        // return WebApiHelper.ExecuteDownload<MyAccount>(this, url);
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Gets the current user asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    public static async Task<User> GetCurrentUserAsync(this RedmineManager rm, NameValueCollection parameters = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Creates the or update wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <param name="wikiPage">The wiki page.</param>
    /// <returns></returns>
    public static async Task<WikiPage> CreateWikiPageAsync(this RedmineManager rm, string projectId, string pageName, WikiPage wikiPage)
    {
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Creates the or update wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <param name="wikiPage">The wiki page.</param>
    /// <returns></returns>
    public static async Task<WikiPage> CreateWikiPageAsync(this RedmineManager rm, int projectId, string pageName, WikiPage wikiPage)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Creates the or update wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <param name="wikiPage">The wiki page.</param>
    /// <returns></returns>
    public static async Task UpdateWikiPageAsync(this RedmineManager rm, string projectId, string pageName, WikiPage wikiPage)
    {
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Creates the or update wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <param name="wikiPage">The wiki page.</param>
    /// <returns></returns>
    public static async Task UpdateWikiPageAsync(this RedmineManager rm, int projectId, string pageName, WikiPage wikiPage)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Deletes the wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <returns></returns>
    public static async Task DeleteWikiPageAsync(this RedmineManager rm, string projectId, string pageName)
    {
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Deletes the wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <returns></returns>
    public static async Task DeleteWikiPageAsync(this RedmineManager rm, int projectId, string pageName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Gets the wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <param name="version">The version.</param>
    /// <returns></returns>
    public static async Task<WikiPage> GetWikiPageAsync(this RedmineManager rm, string projectId, NameValueCollection parameters, string pageName, uint version = 0)
    {
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Gets the wiki page asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="pageName">Name of the page.</param>
    /// <param name="version">The version.</param>
    /// <returns></returns>
    public static async Task<WikiPage> GetWikiPageAsync(this RedmineManager rm, int projectId, NameValueCollection parameters, string pageName, uint version = 0)
    {
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Gets all wiki pages asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <returns></returns>
    public static async Task<List<WikiPage>> GetAllWikiPagesAsync(this RedmineManager rm, NameValueCollection parameters, string projectId)
    {
        throw new NotImplementedException();
    }
        
    /// <summary>
    ///     Gets all wiki pages asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="projectId">The project identifier.</param>
    /// <returns></returns>
    public static async Task<List<WikiPage>> GetAllWikiPagesAsync(this RedmineManager rm, NameValueCollection parameters, int projectId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Adds an existing user to a group. This method does not block the calling thread.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="groupId">The group id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>
    ///     Returns the Guid associated with the async request.
    /// </returns>
    public static async Task AddUserToGroupAsync(this RedmineManager rm, int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Removes an user from a group. This method does not block the calling thread.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="groupId">The group id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns></returns>
    public static async Task RemoveUserFromGroupAsync(this RedmineManager rm, int groupId, int userId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Adds the watcher asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="issueId">The issue identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns></returns>
    public static async Task AddWatcherToIssueAsync(this RedmineManager rm, int issueId, int userId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Removes the watcher asynchronous.
    /// </summary>
    /// <param name="rm">The redmine manager.</param>
    /// <param name="issueId">The issue identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns></returns>
    public static async Task RemoveWatcherFromIssueAsync(this RedmineManager rm, int issueId, int userId)
    {
        throw new NotImplementedException();
    }
}