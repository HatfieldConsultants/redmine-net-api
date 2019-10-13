﻿/*
   Copyright 2011 - 2019 Adrian Popescu.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#if NET40
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Redmine.Api.Types;

namespace Redmine.Api.Async
{
    /// <summary>
    /// 
    /// </summary>
    public static class RedmineManagerAsync
    {
        /// <summary>
        /// Gets the current user asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static Task<User> GetCurrentUserAsync(this RedmineManager redmineManager, NameValueCollection parameters = null)
        {
            return Task.Factory.StartNew(() => redmineManager.GetCurrentUser(parameters));
        }

        /// <summary>
        /// Creates the or update wiki page asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="wikiPage">The wiki page.</param>
        /// <returns></returns>
        public static Task<WikiPage> CreateOrUpdateWikiPageAsync(this RedmineManager redmineManager, string projectId, string pageName, WikiPage wikiPage)
        {
            return Task.Factory.StartNew(() => redmineManager.CreateOrUpdateWikiPage(projectId, pageName, wikiPage));
        }

        /// <summary>
        /// Deletes the wiki page asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <returns></returns>
        public static Task DeleteWikiPageAsync(this RedmineManager redmineManager, string projectId, string pageName)
        {
            return Task.Factory.StartNew(() => redmineManager.DeleteWikiPage(projectId, pageName));
        }

        /// <summary>
        /// Gets the wiki page asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static Task<WikiPage> GetWikiPageAsync(this RedmineManager redmineManager, string projectId, NameValueCollection parameters, string pageName, uint version = 0)
        {
            return Task.Factory.StartNew(() => redmineManager.GetWikiPage(projectId, parameters, pageName, version));
        }

        /// <summary>
        /// Gets all wiki pages asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public static Task<List<WikiPage>> GetAllWikiPagesAsync(this RedmineManager redmineManager, string projectId)
        {
            return Task.Factory.StartNew(() => redmineManager.GetAllWikiPages(projectId));
        }

        /// <summary>
        /// Adds the user to group asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static Task AddUserToGroupAsync(this RedmineManager redmineManager, int groupId, int userId)
        {
            return Task.Factory.StartNew(() => redmineManager.AddUserToGroup(groupId, userId));
        }

        /// <summary>
        /// Removes the user from group asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static Task RemoveUserFromGroupAsync(this RedmineManager redmineManager, int groupId, int userId)
        {
            return Task.Factory.StartNew(() => redmineManager.RemoveUserFromGroup(groupId, userId));
        }

        /// <summary>
        /// Adds the watcher to issue asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static Task AddWatcherToIssueAsync(this RedmineManager redmineManager, int issueId, int userId)
        {
            return Task.Factory.StartNew(() => redmineManager.AddWatcherToIssue(issueId, userId));
        }

        /// <summary>
        /// Removes the watcher from issue asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static Task RemoveWatcherFromIssueAsync(this RedmineManager redmineManager, int issueId, int userId)
        {
            return Task.Factory.StartNew(() => redmineManager.RemoveWatcherFromIssue(issueId, userId));
        }

        /// <summary>
        /// Gets the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static Task<T> GetObjectAsync<T>(this RedmineManager redmineManager, string id, NameValueCollection parameters) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.GetObject<T>(id, parameters));
        }

        /// <summary>
        /// Creates the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Task<T> CreateObjectAsync<T>(this RedmineManager redmineManager, T obj) where T : class, new()
        {
            return CreateObjectAsync(redmineManager, obj, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager"></param>
        /// <param name="include"></param>
        /// <returns></returns>        
        public static Task<int> CountAsync<T>(this RedmineManager redmineManager, params string[] include) where T : class, new()
        {
            return Task.Factory.StartNew(()=> redmineManager.Count<T>(include));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Task<int> CountAsync<T>(this RedmineManager redmineManager, NameValueCollection parameters) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.Count<T>(parameters));
        }

        /// <summary>
        /// Creates the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="obj">The object.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns></returns>
        public static Task<T> CreateObjectAsync<T>(this RedmineManager redmineManager, T obj, string ownerId) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.CreateObject(obj, ownerId));
        }

        /// <summary>
        /// Gets the paginated objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static Task<PagedResults<T>> GetPaginatedObjectsAsync<T>(this RedmineManager redmineManager, NameValueCollection parameters) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.GetPaginatedObjects<T>(parameters));
        }

        /// <summary>
        /// Gets the objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static Task<List<T>> GetObjectsAsync<T>(this RedmineManager redmineManager, NameValueCollection parameters) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.GetObjects<T>(parameters));
        }

        /// <summary>
        /// Updates the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="obj">The object.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public static Task UpdateObjectAsync<T>(this RedmineManager redmineManager, string id, T obj, string projectId = null) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.UpdateObject(id, obj, projectId));
        }

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static Task DeleteObjectAsync<T>(this RedmineManager redmineManager, string id, NameValueCollection parameters) where T : class, new()
        {
            return Task.Factory.StartNew(() => redmineManager.DeleteObject<T>(id));
        }

        /// <summary>
        /// Uploads the file asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Task<Upload> UploadFileAsync(this RedmineManager redmineManager, byte[] data)
        {
            return Task.Factory.StartNew(() => redmineManager.UploadFile(data));
        }

        /// <summary>
        /// Downloads the file asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static Task<byte[]> DownloadFileAsync(this RedmineManager redmineManager, string address)
        {
            return Task.Factory.StartNew(() => redmineManager.DownloadFile(address));
        }
    }
}
#endif