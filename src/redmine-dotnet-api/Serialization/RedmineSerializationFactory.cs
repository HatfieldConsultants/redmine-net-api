using System;
using Redmine.dotNet.Api.Serialization.Xml;

namespace Redmine.dotNet.Api;

internal sealed class RedmineSerializationFactory : IRedmineSerializationFactory
{
    private RedmineSerializationFactory()
    {
        
    }

    public static IRedmineSerializationFactory Create()
    {
        return new RedmineSerializationFactory();
    }
    
    
    public IRedmineSerialization Get(RedmineSerializationType serializationType)
    {
        return serializationType switch
        {
            RedmineSerializationType.Xml => new RedmineXmlSerializer(),
            RedmineSerializationType.Json => new RedmineJsonSerializer(),
            _ => throw new ArgumentOutOfRangeException(nameof(serializationType), serializationType, null)
        };
    }
}