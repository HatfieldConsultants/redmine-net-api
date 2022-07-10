using System.Threading.Tasks;

namespace Redmine.dotNet.Api;

public static class RedmineManagerExtensions
{
    public static async ValueTask<IRedmineResponse<object>> CurrentUser(this RedmineManager redmineManager)
    {
        var response = await redmineManager.GetAsync<object>(new RedmineRequest()).ConfigureAwait(false);
        return response;
    }
}