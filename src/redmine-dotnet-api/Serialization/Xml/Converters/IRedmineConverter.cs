using System;

namespace Redmine.dotNet.Api.Serialization.Xml.Converters;

public interface IRedmineConverter
{
    Type Type { get; }
}
public interface IRedmineConverter<out T>: IRedmineConverter
{
    T Payload { get;  }
}