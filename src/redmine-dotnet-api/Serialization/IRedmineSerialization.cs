using System.IO;
using Redmine.dotNet.Api.Serialization.Xml.Converters;

namespace Redmine.dotNet.Api;

public interface IRedmineSerialization
{
    RedmineSerializationType Type { get; }
    
    TOut Deserialize<TOut>(string input) where TOut: class;
    TOut Deserialize<TOut>(Stream input);
    
    string Serialize(IRedmineConverter input);
}