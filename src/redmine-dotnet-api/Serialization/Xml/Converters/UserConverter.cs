using Redmine.dotNet.Api.Types;

namespace Redmine.dotNet.Api.Serialization.Xml.Converters;

public sealed class UserConverter: IdentifiableConverter, IRedmineConverter<User>
{
    public User Payload { get; set; }
}