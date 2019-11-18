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



#if !(NET20 || NET40)
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Redmine.Api.Exceptions;
using Redmine.Api.Internals;
using Redmine.Api.Types;

namespace Redmine.Api.Extensions.Async
{
    /// <summary>
    /// </summary>
    public static class RedmineManagerAsync
    {
        /// <summary>
        ///     Gets the current user asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static async Task<User> GetCurrentUserAsync(this RedmineManager redmineManager, NameValueCollection parameters = null)
        {
            if (!redmineManager.FlagWebClient)
            {
                var uri = UrlHelper.GetCurrentUserUrl(redmineManager);
                return await WebApiAsyncHelper.ExecuteDownload<User>(redmineManager, uri, "GetCurrentUserAsync", parameters).ConfigureAwait(false);
            }
            else
            {
                try
                {
                    var url = ApiUrls.CurrentUser(redmineManager.redmineConnectionSettings.Host, redmineManager.redmineConnectionSettings.Format.ToString("G").ToLowerInvariant());
                    var result = await redmineManager.x.GetAsync(new HttpRequest(url, HttpVerbs.GET,redmineManager.redmineConnectionSettings.Headers, TimeSpan.Zero, redmineManager.redmineConnectionSettings.ContentType)).ConfigureAwait(false);

                    if (!result.Body.IsNullOrWhiteSpace())
                    {
                        return redmineManager.Serializer.Deserialize<User>(result.Body);
                    }
                }
                catch (Exception exception)
                {
                    if (exception is WebException)
                    {
                        ((WebException)exception).HandleWebException(redmineManager.Serializer);
                    }
                    else
                    {
                        throw new RedmineException(exception.Message, exception);
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     Creates the or update wiki page asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="wikiPage">The wiki page.</param>
        /// <returns></returns>
        public static async Task<WikiPage> CreateOrUpdateWikiPageAsync(this RedmineManager redmineManager, string projectId, string pageName, WikiPage wikiPage)
        {
            var uri = UrlHelper.GetWikiCreateOrUpdaterUrl(redmineManager, projectId, pageName);
            var data = redmineManager.Serializer.Serialize(wikiPage);

            var response = await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.PUT, data, "CreateOrUpdateWikiPageAsync").ConfigureAwait(false);
            return redmineManager.Serializer.Deserialize<WikiPage>(response);
        }

        /// <summary>
        ///     Deletes the wiki page asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <returns></returns>
        public static async Task DeleteWikiPageAsync(this RedmineManager redmineManager, string projectId,
            string pageName)
        {
            var uri = UrlHelper.GetDeleteWikirUrl(redmineManager, projectId, pageName);
            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.DELETE, string.Empty, "DeleteWikiPageAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Support for adding attachments through the REST API is added in Redmine 1.4.0.
        ///     Upload a file to server. This method does not block the calling thread.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="data">The content of the file that will be uploaded on server.</param>
        /// <returns>
        ///     .
        /// </returns>
        public static async Task<Upload> UploadFileAsync(this RedmineManager redmineManager, byte[] data)
        {
            var uri = UrlHelper.GetUploadFileUrl(redmineManager);
            return await WebApiAsyncHelper.ExecuteUploadFile(redmineManager, uri, data, "UploadFileAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Downloads the file asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static async Task<byte[]> DownloadFileAsync(this RedmineManager redmineManager, string address)
        {
            return await WebApiAsyncHelper.ExecuteDownloadFile(redmineManager, address, "DownloadFileAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the wiki page asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static async Task<WikiPage> GetWikiPageAsync(this RedmineManager redmineManager, string projectId,
            NameValueCollection parameters, string pageName, uint version = 0)
        {
            var uri = UrlHelper.GetWikiPageUrl(redmineManager, projectId, parameters, pageName, version);
            return await WebApiAsyncHelper.ExecuteDownload<WikiPage>(redmineManager, uri, "GetWikiPageAsync", parameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets all wiki pages asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public static async Task<List<WikiPage>> GetAllWikiPagesAsync(this RedmineManager redmineManager, NameValueCollection parameters, string projectId)
        {
            var uri = UrlHelper.GetWikisUrl(redmineManager, projectId);
            return await WebApiAsyncHelper.ExecuteDownloadList<WikiPage>(redmineManager, uri, "GetAllWikiPagesAsync", parameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Adds an existing user to a group. This method does not block the calling thread.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>
        ///     Returns the Guid associated with the async request.
        /// </returns>
        public static async Task AddUserToGroupAsync(this RedmineManager redmineManager, int groupId, int userId)
        {
            var data = DataHelper.UserData(userId, redmineManager.MimeFormat);
            var uri = UrlHelper.GetAddUserToGroupUrl(redmineManager, groupId);

            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.POST, data, "AddUserToGroupAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Removes an user from a group. This method does not block the calling thread.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static async Task DeleteUserFromGroupAsync(this RedmineManager redmineManager, int groupId, int userId)
        {
            var uri = UrlHelper.GetRemoveUserFromGroupUrl(redmineManager, groupId, userId);
            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.DELETE, string.Empty, "DeleteUserFromGroupAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Adds the watcher asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static async Task AddWatcherAsync(this RedmineManager redmineManager, int issueId, int userId)
        {
            var data = DataHelper.UserData(userId, redmineManager.MimeFormat);
            var uri = UrlHelper.GetAddWatcherUrl(redmineManager, issueId, userId);

            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.POST, data, "AddWatcherAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Removes the watcher asynchronous.
        /// </summary>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static async Task RemoveWatcherAsync(this RedmineManager redmineManager, int issueId, int userId)
        {
            var uri = UrlHelper.GetRemoveWatcherUrl(redmineManager, issueId, userId);
            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.DELETE, string.Empty, "RemoveWatcherAsync").ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public static async Task<int> CountAsync<T>(this RedmineManager redmineManager, params string[] include) where T : class, new()
        {
            var parameters = new NameValueCollection();

            if (include != null)
            {
                parameters.Add(RedmineKeys.INCLUDE, string.Join(",", include));
            }

            return await CountAsync<T>(redmineManager, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<int> CountAsync<T>(this RedmineManager redmineManager, NameValueCollection parameters) where T : class, new()
        {
            int totalCount = 0, pageSize = 1, offset = 0;

            if (parameters == null)
            {
                parameters = new NameValueCollection();
            }

            parameters.Set(RedmineKeys.LIMIT, pageSize.ToString(CultureInfo.InvariantCulture));
            parameters.Set(RedmineKeys.OFFSET, offset.ToString(CultureInfo.InvariantCulture));

            try
            {
                var tempResult = await GetPaginatedObjectsAsync<T>(redmineManager, parameters);
                if (tempResult != null)
                {
                    totalCount = tempResult.TotalItems;
                }
            }
            catch (WebException wex)
            {
                wex.HandleWebException(redmineManager.Serializer);
            }

            return totalCount;
        }


        /// <summary>
        ///     Gets the paginated objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static async Task<PagedResults<T>> GetPaginatedObjectsAsync<T>(this RedmineManager redmineManager,
            NameValueCollection parameters)
            where T : class, new()
        {
            var uri = UrlHelper.GetListUrl<T>(redmineManager, parameters);
            return await WebApiAsyncHelper.ExecuteDownloadPaginatedList<T>(redmineManager, uri, "GetPaginatedObjectsAsync", parameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static async Task<List<T>> GetObjectsAsync<T>(this RedmineManager redmineManager, NameValueCollection parameters)
            where T : class, new()
        {
            int totalCount = 0, pageSize, offset;
            List<T> resultList = null;

            if (parameters == null) parameters = new NameValueCollection();

            int.TryParse(parameters[RedmineKeys.LIMIT], out pageSize);
            int.TryParse(parameters[RedmineKeys.OFFSET], out offset);
            if (pageSize == default(int))
            {
                pageSize = redmineManager.PageSize > 0
                    ? redmineManager.PageSize
                    : RedmineManager.DEFAULT_PAGE_SIZE_VALUE;
                parameters.Set(RedmineKeys.LIMIT, pageSize.ToString(CultureInfo.InvariantCulture));
            }
            try
            {
                do
                {
                    parameters.Set(RedmineKeys.OFFSET, offset.ToString(CultureInfo.InvariantCulture));
                    var tempResult = await redmineManager.GetPaginatedObjectsAsync<T>(parameters).ConfigureAwait(false);
                    if (tempResult != null)
                    {
                        if (resultList == null)
                        {
                            resultList = tempResult.Items.ToList();
                            totalCount = tempResult.TotalItems;
                        }
                        else
                        {
                            resultList.AddRange(tempResult.Items);
                        }
                    }
                    offset += pageSize;
                } while (offset < totalCount);
            }
            catch (WebException wex)
            {
                wex.HandleWebException(redmineManager.Serializer);
            }
            return resultList;
        }

        /// <summary>
        ///     Gets a Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of objects to retrieve.</typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="id">The id of the object.</param>
        /// <param name="parameters">Optional filters and/or optional fetched data.</param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(this RedmineManager redmineManager, string id, NameValueCollection parameters)
            where T : class, new()
        {
            var uri = UrlHelper.GetGetUrl<T>(redmineManager, id);
            return await WebApiAsyncHelper.ExecuteDownload<T>(redmineManager, uri, "GetobjectAsync", parameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Creates a new Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="obj">The object to create.</param>
        /// <returns></returns>
        public static async Task<T> CreateObjectAsync<T>(this RedmineManager redmineManager, T obj)
            where T : class, new()
        {
            return await CreateObjectAsync(redmineManager, obj, null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Creates a new Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="obj">The object to create.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns></returns>
        public static async Task<T> CreateObjectAsync<T>(this RedmineManager redmineManager, T obj, string ownerId)
            where T : class, new()
        {
            var uri = UrlHelper.GetCreateUrl<T>(redmineManager, ownerId);
            var data = redmineManager.Serializer.Serialize(obj);

            var response = await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.POST, data, "CreateObjectAsync").ConfigureAwait(false);
            return redmineManager.Serializer.Deserialize<T>(response);
        }

        /// <summary>
        ///     Updates the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="obj">The object.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public static async Task UpdateObjectAsync<T>(this RedmineManager redmineManager, string id, T obj, string projectId = null)
            where T : class, new()
        {
            var uri = UrlHelper.GetUploadUrl(redmineManager, id, obj, projectId);
            var data = redmineManager.Serializer.Serialize(obj);
            data = Regex.Replace(data, @"\r\n|\r|\n", "\r\n");

            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.PUT, data, "UpdateObjectAsync").ConfigureAwait(false);
        }

        /// <summary>
        ///     Deletes the Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        /// <param name="redmineManager">The redmine manager.</param>
        /// <param name="id">The id of the object to delete</param>
        /// <param name="parameters">Optional filters and/or optional fetched data.</param>
        /// <returns></returns>
        public static async Task DeleteObjectAsync<T>(this RedmineManager redmineManager, string id, NameValueCollection parameters)
            where T : class, new()
        {
            var uri = UrlHelper.GetDeleteUrl<T>(redmineManager, id);
            await WebApiAsyncHelper.ExecuteUpload(redmineManager, uri, HttpVerbs.DELETE, string.Empty, "DeleteObjectAsync").ConfigureAwait(false);
        }
    }
}
#endif