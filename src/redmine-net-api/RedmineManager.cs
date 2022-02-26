/*
   Copyright 2011 - 2021 Adrian Popescu.

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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Redmine.Net.Api.Exceptions;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Old.Internals;
using Redmine.Net.Api.Serialization;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api
{
    /// <summary>
    ///     The main class to access Redmine API.
    /// </summary>
    public class RedmineManager 
    {
        /// <summary>
        /// </summary>
        public const int DEFAULT_PAGE_SIZE_VALUE = 25;
        
        private readonly ApiRequester _apiRequester;

        private readonly AuthenticatorFactory authenticatorFactory = new AuthenticatorFactory();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redmineApiConfig"></param>
        /// <exception cref="RedmineException"></exception>
        public RedmineManager(RedmineApiConfigBuilder redmineApiConfig)
        {
            Ensure.ArgumentNotNull(redmineApiConfig, nameof(redmineApiConfig));

            Ensure.ArgumentNotNullOrEmptyString(redmineApiConfig.Host, "Host is undefined.");

            if (!Uri.TryCreate(redmineApiConfig.Host, UriKind.RelativeOrAbsolute, out var uri))
            {
                throw new RedmineException("Host is not a valid uri.", nameof(redmineApiConfig.Host));
            }

            if (!uri.IsAbsoluteUri)
            {
                throw new RedmineException("The base address for the connection must be an absolute URI.",
                    new ArgumentException(nameof(redmineApiConfig.Host)));
            }

            Uri = uri;

            Ensure.ArgumentNotNull(redmineApiConfig.Credentials,"Credentials are not set.");

            UserAgent = redmineApiConfig.UserAgent;
            
            _apiRequester = new ApiRequester(redmineApiConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; }

        private string ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Gets the credential store for the connection.
        /// </summary>
        public IRedmineApiCredentials Credentials { get; }

        /// <summary>
        /// Gets the HTTP client for the connection.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ImpersonateUser
        {
            get => _apiRequester.ImpersonateUser;
            set => _apiRequester.ImpersonateUser = value;
        }

        private static HttpClient CreateDefaultClient(RedmineApiConfigBuilder redmineApiConfig)
        {
            var handler = new HttpClientHandler()
            {
                Proxy = redmineApiConfig.Proxy,
                ClientCertificateOptions = redmineApiConfig.ClientCertificateOption,
                ServerCertificateCustomValidationCallback = redmineApiConfig.ServerCertificateCustomValidationCallback,

                //4.7.1
                SslProtocols = redmineApiConfig.SslProtocols,
                CheckCertificateRevocationList = redmineApiConfig.CheckCertificateRevocationList,
            };

            handler.AllowAutoRedirect = redmineApiConfig.AllowAutoRedirect;


            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            if (handler.SupportsProxy && redmineApiConfig.Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = redmineApiConfig.Proxy;
            }

            if (redmineApiConfig.CookieContainer != null)
            {
                handler.CookieContainer = redmineApiConfig.CookieContainer;
            }

            if (redmineApiConfig.MaxAutomaticRedirections.HasValue)
            {
                handler.MaxAutomaticRedirections = redmineApiConfig.MaxAutomaticRedirections.Value;
            }

            if (redmineApiConfig.MaxConnectionsPerServer.HasValue)
            {
                handler.MaxConnectionsPerServer = redmineApiConfig.MaxConnectionsPerServer.Value;
            }

            if (redmineApiConfig.MaxRequestContentBufferSize.HasValue)
            {
                handler.MaxRequestContentBufferSize = redmineApiConfig.MaxRequestContentBufferSize.Value;
            }

            if (redmineApiConfig.MaxResponseHeadersLength.HasValue)
            {
                handler.MaxResponseHeadersLength = redmineApiConfig.MaxResponseHeadersLength.Value;
            }

            return new HttpClient(handler, false);
        }
        
       
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="include"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<int> CountAsync<T>(string[] include, CancellationToken cancellationToken = default) where T : class, new()
        {
            var nv = include.SetInclude();
            
            return await CountAsync<T>(nv, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<int> CountAsync<T>(NameValueCollection parameters = null, CancellationToken cancellationToken = default) where T : class, new()
        {
            try
            {
                parameters.SetOffset(0).SetLimit(1);
                var tempResult = await GetPaginatedObjectsAsync<T>(parameters, cancellationToken).ConfigureAwait(false);
                if (tempResult != null)
                {
                   return tempResult.TotalItems;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return 0;
        }

        /// <summary>
        ///     Gets a Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of objects to retrieve.</typeparam>
        /// <param name="id">The id of the object.</param>
        /// <param name="parameters">Optional filters and/or optional fetched data.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> GetObjectAsync<T>(string id, NameValueCollection parameters = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            var response = await _apiRequester.GetAsync<T>(id, parameters, cancellationToken).ConfigureAwait(false);

            return response;
        }
        
        /// <summary>
        ///     Gets the objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> GetObjectsAsync<T>(string[] include, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            var nv = include.SetInclude();
            
            return await GetObjectsAsync<T>(nv, cancellationToken).ConfigureAwait(false);
        }
      
        /// <summary>
        ///     Gets the objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> GetObjectsAsync<T>(int offset, int limit = DEFAULT_PAGE_SIZE_VALUE, string[] include = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            var nv = include.SetInclude().SetOffset(offset).SetLimit(limit);

            return await GetObjectsAsync<T>(nv, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> GetObjectsAsync<T>(NameValueCollection parameters = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            List<T> resultList = null;
            
            var limit = parameters.GetLimitOrDefault(DEFAULT_PAGE_SIZE_VALUE);

            var offset = parameters.GetOffsetOrDefault();
            
            parameters.SetOffset(offset);
            parameters.SetLimit(limit);

            try
            {
                var totalCount = 0;
                do
                {
                    var tempResult = await GetPaginatedObjectsAsync<T>(parameters, cancellationToken).ConfigureAwait(false);

                    totalCount = Math.Min(limit, tempResult.TotalItems);

                    if (tempResult?.Items != null)
                    {
                        if (resultList == null)
                        {
                            resultList = new List<T>(tempResult.Items);
                        }
                        else
                        {
                            resultList.AddRange(tempResult.Items);
                        }
                    }

                    offset += limit;
                    parameters.SetOffset(offset);
                    
                } while (offset < totalCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return resultList;
        }

        // public async Task<IEnumerable<T>> GetObjectsEnumerableAsync<T>(NameValueCollection parameters = null) where T : class, new()
        // {
        //     var limit = parameters.GetLimitOrDefault(DEFAULT_PAGE_SIZE_VALUE);
        //     var offset = parameters.GetOffsetOrDefault();
        //     
        //     parameters.SetOffset(offset);
        //     parameters.SetLimit(limit);
        //
        //     try
        //     {
        //         var totalCount = 0;
        //         do
        //         {
        //             var tempResult = await GetPaginatedObjectsEnumerableAsync<T>(parameters).ConfigureAwait(false);
        //
        //             totalCount = Math.Min(limit, tempResult.TotalItems);
        //
        //             if (tempResult?.Items != null)
        //             {
        //                 if (resultList == null)
        //                 {
        //                     resultList = new List<T>(tempResult.Items);
        //                 }
        //                 else
        //                 {
        //                     resultList.AddRange(tempResult.Items);
        //                 }
        //             }
        //
        //             offset += limit;
        //             parameters.SetOffset(offset);
        //             
        //         } while (offset < totalCount);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="include"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResults<T>> GetPaginatedObjectsAsync<T>(int offset = 0, int limit = DEFAULT_PAGE_SIZE_VALUE, string[] include = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            var nv = include.SetInclude().SetOffset(offset).SetLimit(limit);

            return await GetPaginatedObjectsAsync<T>(nv, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the paginated objects asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResults<T>> GetPaginatedObjectsAsync<T>(NameValueCollection parameters = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Creates a new Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="entity">The object to create.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> CreateObjectAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            return await CreateObjectAsync(entity, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Creates a new Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="entity">The object to create.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> CreateObjectAsync<T>(T entity, string ownerId, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            return await _apiRequester.PostAsync(entity, ownerId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Updates the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The object.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateObjectAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : Identifiable<T>, new()
        {
            await UpdateObjectAsync(entity, null, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="projectId"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateObjectAsync<T>(T entity, string projectId, CancellationToken cancellationToken = default)
            where T : Identifiable<T>, new()
        {
            await _apiRequester.PutAsync(entity, projectId, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="projectId"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> UpdateAndRetrieveObjectAsync<T>(T entity, string projectId, CancellationToken cancellationToken = default)
            where T : Identifiable<T>, new()
        {
            await UpdateObjectAsync(entity, projectId, cancellationToken).ConfigureAwait(false);

            return await GetObjectAsync<T>(entity.Id.ToString(CultureInfo.InvariantCulture), cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        ///     Deletes the Redmine object. This method does not block the calling thread.
        /// </summary>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        /// <param name="id">The id of the object to delete</param>
        /// <param name="nvc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteObjectAsync<T>(string id, NameValueCollection nvc = null, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            await _apiRequester.DeleteAsync<T>(id, nvc, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="nvc"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task DeleteObjectAsync<T>(T entity, NameValueCollection nvc = null, CancellationToken cancellationToken = default)
            where T : Identifiable<T>, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await DeleteObjectAsync<T>(entity.Id.ToString(CultureInfo.InvariantCulture), nvc, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Support for adding attachments through the REST API is added in Redmine 1.4.0.
        ///     Upload a file to server. This method does not block the calling thread.
        /// </summary>
        /// <param name="data">The content of the file that will be uploaded on server.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        ///     .
        /// </returns>
        public async Task<Upload> UploadFileAsync(byte[] data, CancellationToken cancellationToken = default)
        {
           return  await _apiRequester.PostAsync<Upload>(data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="attachment"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateAttachment(int issueId, Attachment attachment, CancellationToken cancellationToken = default)
        {
             await _apiRequester.PatchAsync(new Attachments { { attachment.Id, attachment } }, issueId.ToString(CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Downloads the file asynchronous.
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<byte[]> DownloadFileAsync(string relativeUri, CancellationToken cancellationToken = default)
        {
           var result = await _apiRequester.DownloadAsync(relativeUri, cancellationToken).ConfigureAwait(false);

           return result;
        }
    }
}