 #if!(NET20)
using System.Threading;
using System.Threading.Tasks;

namespace Redmine.Api
{
    internal interface IHttpRequesterAsync
    {
        Task<IHttpResponse> GetAsync(IHttpRequest request, CancellationToken cancellationToken = default);
        Task<IHttpResponse> CreateAsync(IHttpRequest request, CancellationToken cancellationToken = default);
        Task<IHttpResponse> PatchAsync(IHttpRequest request, CancellationToken cancellationToken = default);
        Task<IHttpResponse> UpdateAsync(IHttpRequest request, CancellationToken cancellationToken = default);
        Task<IHttpResponse> DeleteAsync(IHttpRequest request, CancellationToken cancellationToken = default);
    }
}
#endif