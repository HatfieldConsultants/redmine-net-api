using Redmine.dotNet.Api;
using Redmine.dotNet.Api.Serialization.Xml.Converters;
using Redmine.dotNet.Api.Types;

namespace redmine_dotnet_api.tests;

public sealed class UserTests
{
    [Fact]
    public void Serialize()
    {
        IRedmineSerializationFactory redmineSerializationFactory = RedmineSerializationFactory.Create();
        IRedmineSerialization serialization = redmineSerializationFactory.Get(RedmineSerializationType.Xml);

        var converter = new UserConverter()
        {
            Payload = new User()
            {
               
            }
        };
        
        var result = serialization.Serialize(converter);
    }

}