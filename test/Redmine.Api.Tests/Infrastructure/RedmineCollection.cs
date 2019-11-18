#if !(NET20 || NET40)
using Xunit;

namespace Redmine.Api.Tests.Infrastructure
{
	[CollectionDefinition("RedmineCollection")]
	public class RedmineCollection : ICollectionFixture<RedmineFixture>
	{
		
	}
}
#endif