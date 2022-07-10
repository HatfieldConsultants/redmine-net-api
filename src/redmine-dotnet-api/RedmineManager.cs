using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Redmine.dotNet.Api
{
    public sealed class RedmineManager: IRedmineManager
    {
        private readonly IRedmineClient redmineClient;
        
        public RedmineManager(IRedmineSettingsBuilder redmineSettingsBuilder)
        {
            RedmineSettings = redmineSettingsBuilder.Build();
        }
        
        public IRedmineSettings RedmineSettings { get; }

        public async ValueTask<IRedmineResponse<TOut>> GetAsync<TOut>(IRedmineRequest redmineRequest) where TOut: class
        {
            var response = await redmineClient.SendAsync<TOut>(redmineRequest);
            return response;
        }
        
        public async ValueTask<IRedmineResponse<TOut>> CreateAsync<TOut>(IRedmineRequest redmineRequest) where TOut: class
        {
            return default;
        } 
        
        public async ValueTask<IRedmineResponse> DeleteAsync<TOut>(IRedmineRequest redmineRequest) where TOut: class
        {
            return default;
        } 
        
        public async ValueTask<IRedmineResponse<TOut>> ListAsync<TOut>(IRedminePaginatedRequest redmineRequest) where TOut: class
        {
            return default;
        }
    }

    internal interface IRedmineClient
    {
        ValueTask<IRedmineResponse<TOut>> SendAsync<TOut>(IRedmineRequest request);
    }
    
    public interface IRedmineRequest
    {
        
    }
    
    public interface IRedminePaginatedRequest: IRedmineRequest
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
    }

    internal sealed class RedmineRequest : IRedmineRequest
    {
        
    }

    public interface IRedmineResponse
    {
        
    }
    
    public interface IRedmineResponse<TOut> : IRedmineResponse
    {
        
    }
}