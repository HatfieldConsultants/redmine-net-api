using Redmine.dotNet.Api;
using Redmine.dotNet.Api.Serialization.Xml.Converters;
using Redmine.dotNet.Api.Types;

namespace redmine_dotnet_api.tests;

public sealed class ProjectTests
{
    [Fact]
    public void Serialize()
    {
        IRedmineSerializationFactory redmineSerializationFactory = RedmineSerializationFactory.Create();
        IRedmineSerialization serialization = redmineSerializationFactory.Get(RedmineSerializationType.Xml);

        var converter = new ProjectConverter()
        {
            Payload = new Project()
            {
                Name = "Redmine Net Api Project Test All Properties",
                Description = "This is a test project.",
                Identifier = "rnaptap",
                HomePage = "www.redminetest.com",
                IsPublic = true,
                InheritMembers = true,
                EnabledModules = new List<ProjectEnabledModule>
                {
                    new() {Name = "issue_tracking"},
                    new() {Name = "time_tracking"}
                },
                Trackers = new List<ProjectTracker>
                {
                    IdentifiableName.Create<ProjectTracker>( 1),
                    IdentifiableName.Create<ProjectTracker>(2)
                }
            }
        };   
        
        var result = serialization.Serialize(converter);
    }

}