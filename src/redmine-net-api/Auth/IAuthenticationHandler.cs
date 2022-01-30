using Redmine.Net.Api.Types;

namespace Redmine.Net.Api;

interface IAuthenticationHandler
{
    void Authenticate(IRedmineApiRequest request, IRedmineApiCredentials credentials);
}