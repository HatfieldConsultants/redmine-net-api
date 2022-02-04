using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Serialization;
using Redmine.Net.Api.Types;
using Group = Redmine.Net.Api.Types.Group;
using Version = Redmine.Net.Api.Types.Version;

namespace Redmine.Net.Api;

internal sealed class ApiRequester
{
    private readonly HttpClient _httpClient;
    private readonly RedmineApiCredentials _redmineApiCredentials;
    private readonly IRedmineSerializer _serializer;
    private readonly string _contentType;
    private readonly string suffix;
    
    private static readonly Dictionary<Type, string> mapTypeEndpoint = new Dictionary<Type, string>
    {
        {typeof(Issue), "issues"},
        {typeof(Project), "projects"},
        {typeof(User), "users"},
        {typeof(News), "news"},
        {typeof(Query), "queries"},
        {typeof(Version), "versions"},
        {typeof(Attachment), "attachments"},
        {typeof(IssueRelation), "relations"},
        {typeof(TimeEntry), "time_entries"},
        {typeof(IssueStatus), "issue_statuses"},
        {typeof(Tracker), "trackers"},
        {typeof(IssueCategory), "issue_categories"},
        {typeof(Role), "roles"},
        {typeof(ProjectMembership), "memberships"},
        {typeof(Group), "groups"},
        {typeof(TimeEntryActivity), "enumerations/time_entry_activities"},
        {typeof(IssuePriority), "enumerations/issue_priorities"},
        {typeof(Watcher), "watchers"},
        {typeof(IssueCustomField), "custom_fields"},
        {typeof(CustomField), "custom_fields"},
        {typeof(MembershipRole), "roles"},
            
        //       {typeof(WikiPage), ""}
    };
    
    public ApiRequester(RedmineApiConfigBuilder builder)
    {
        _httpClient = builder.HttpClient ?? new HttpClient();
        switch (builder.SerializationType)
        {
            case RedmineSerializationType.Json:
                _serializer =  new JsonRedmineSerializer();
                _contentType = "application/json";
                suffix = "json";
                break;
            case RedmineSerializationType.Xml:
                _serializer =  new XmlRedmineSerializer();
                _contentType = "application/xml";
                suffix = "xml";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _redmineApiCredentials = builder.Credentials;
        BaseAddress = new Uri(builder.Host);
        UserAgent = builder.UserAgent;
    }

    /// <summary>
    /// Base address for the connection.
    /// </summary>
    public Uri BaseAddress { get; private set; }

    public string UserAgent { get; private set; }

    public string ImpersonateUser { get; set; }
        
        
    public async Task<T> GetAsync<T>(string id, NameValueCollection parameters = null,  CancellationToken cancellationToken = default)
    {
        var result = await ExecuteAsync<T>(new RedmineApiRequest()
        {
            BaseAddress = BaseAddress,
            Method = HttpMethod.Get,
            Parameters = parameters,
            ContentType = _contentType,
            Endpoint = RedmineApiUrls.CreateEntityUrl<T>(mapTypeEndpoint,id)
        }, cancellationToken);
            
        return default;
    }

    public async Task<T> PostAsync<T>(T entity, string ownerId,  CancellationToken cancellationToken) where T: class, new()
    {
        var result = await ExecuteAsync<T>(new RedmineApiRequest()
        {
            BaseAddress = BaseAddress,
            Method = HttpMethod.Post,
            ContentType = _contentType,
            Endpoint = RedmineApiUrls.CreateEntityUrl<T>(mapTypeEndpoint, ownerId)
            ,Body  = _serializer.Serialize(entity)
        }, cancellationToken);
        
        return default;
    }
    
    public async Task<T> PostAsync<T>(byte[] data,  CancellationToken cancellationToken) where T: class, new()
    {
        var result = await ExecuteAsync<T>(new RedmineApiRequest()
        {
            BaseAddress = BaseAddress,
            Method = HttpMethod.Post,
            ContentType = _contentType,
            Endpoint = RedmineApiUrls.CreateEntityUrl<T>(mapTypeEndpoint, null)
            ,Body  = data
        }, cancellationToken);
        
        return default;
    }

    private static readonly Regex regex = new Regex(@"\r\n|\r|\n",  RegexOptions.Compiled);
    
    public async Task<T> PutAsync<T>(T data, string projectId, CancellationToken cancellationToken) where T: Identifiable<T>, new()
    {
        var serializeData = _serializer.Serialize(data);

        var body = regex.Replace(serializeData, "\r\n");
        
        await ExecuteAsync<T>(new RedmineApiRequest()
        {
            Body = body,
            BaseAddress = BaseAddress,
            Method = HttpMethod.Put,
        //    Parameters = nvc,
            ContentType = _contentType,
            Endpoint = RedmineApiUrls.UpdateEntityUrl<T>(mapTypeEndpoint, data.Id.ToString(CultureInfo.InvariantCulture))
        }, cancellationToken);
        return default;
    }

    public async Task<T> DeleteAsync<T>(string id, NameValueCollection nvc, CancellationToken cancellationToken)
    {
        await ExecuteAsync<T>(new RedmineApiRequest()
        {
            BaseAddress = BaseAddress,
            Parameters = nvc,
            ContentType = _contentType,
            Method = HttpMethod.Delete,
            Endpoint = RedmineApiUrls.DeleteEntityUrl<T>(mapTypeEndpoint,id)
        }, cancellationToken);
        return default;
    }

    public async Task PatchAsync<T>(T data, string issueId, CancellationToken cancellationToken) where T: class, new()
    {
        var result = await ExecuteAsync<T>(new RedmineApiRequest()
        {
            BaseAddress = BaseAddress,
            Method = new HttpMethod(HttpVerbs.PATCH),
            ContentType = _contentType,
            Endpoint = RedmineApiUrls.PatchEntityUrl<T>(mapTypeEndpoint, issueId)
            ,Body  = _serializer.Serialize(data)
        }, cancellationToken);
    }

    private static string[] binaryContentTypes = new[] {
       
        "application/zip" ,
        "application/x-gzip" ,
        "application/octet-stream"};
    
    private static MediaTypeHeaderValue octetStreamMediaType =new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
    
    private async Task<RedmineApiResponse> ExecuteAsync<T>(IRedmineApiRequest request, CancellationToken cancellationToken)
    {
        request.Headers.SetUserAgent(UserAgent); 
        request.Headers.SetImpersonateUser(ImpersonateUser);
        
        var query = request.Parameters.ToQueryString();
        var uri = $"{request.BaseAddress}/{request.Endpoint}.{suffix}{query}";

        HttpContent httpContent = CreateHttpContent(request.Body, request.ContentType);
        
        //  var authenticatorHandler = authenticatorFactory.GetRedmineAuthenticatorHandler(_redmineApiCredentials.AuthenticationType);

        // authenticatorHandler.Authenticate(request, Credentials);

        // request.ContentType = ContentType;

        using (var req = new HttpRequestMessage(request.Method, uri))
        {
            req.Content = httpContent;
            
            using (var response = await _httpClient
                       .SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                       .ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return new RedmineApiResponse(response.StatusCode, response.ReasonPhrase,
                        response.Headers.ToDictionary(), request.ContentType);
                }
            
                var responseContentType = response.Content.Headers?.ContentType?.MediaType;
                object content = null;
                if(responseContentType != null && binaryContentTypes
                    .Any(item => item.Equals(responseContentType, StringComparison.OrdinalIgnoreCase)))
                {
                    content =   await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
                else
                {
                    content =   await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                
               
                //
                //  using (var ms = await response.Content.ReadAsStreamAsync())
                // {
                //     ms.Seek(0, SeekOrigin.Begin);
                //     await ms.CopyToAsync(fs);
                // }
                
                return new RedmineApiResponse(response.StatusCode, content, response.Headers.ToDictionary(),
                    request.ContentType);
            }

            return new RedmineApiResponse(HttpStatusCode.Accepted, null, null, _contentType);
        }
    }

    public async Task<byte[]> DownloadAsync(string relativeUri, CancellationToken cancellationToken)
    {
        // var result = await ExecuteAsync<T>(new RedmineApiRequest()
        // {
        //     BaseAddress = BaseAddress,
        //     Method = HttpMethod.Get,
        //     ContentType = _contentType,
        //     Endpoint = relativeUri
        // }, cancellationToken);
        
       
        throw new NotImplementedException();
    }

    private HttpContent CreateHttpContent(object data, string contentType)
    {
        switch (data)
        {
            case string s:
                return  new StringContent(s, Encoding.UTF8, contentType ?? _contentType);
            case byte[] arr:
            {
                var httpContent =  new ByteArrayContent(arr);
                httpContent.Headers.ContentType = octetStreamMediaType;
                return httpContent;
            }
            default:
                return null;
        }
    }
    
    
    private HttpRequestMessage CreateRequestMessage(HttpMethod method, string uri, HttpContent httpContent)
    {
        var req = new HttpRequestMessage(method, uri);
        req.Content = httpContent;
        return req;
    }
}