namespace Redmine.dotNet.Api;

internal interface IRedmineSerializationFactory
{
    IRedmineSerialization Get(RedmineSerializationType serializationType);
}