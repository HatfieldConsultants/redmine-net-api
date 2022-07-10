using System;
using System.IO;
using System.Text;
using Redmine.dotNet.Api.Serialization.Xml.Converters;

namespace Redmine.dotNet.Api;

internal sealed class RedmineJsonSerializer : IRedmineJsonSerialization
{
    public RedmineSerializationType Type { get; } = RedmineSerializationType.Json;
    public TOut Deserialize<TOut>(string input) where TOut: class
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input), $"Could not deserialize null or empty input for type '{typeof(TOut).Name}'.");
        }

        var isJsonSerializable = typeof(IJsonSerializable).IsAssignableFrom(typeof(TOut));

        if (!isJsonSerializable)
        {
            throw new RedmineException($"Entity of type '{typeof(TOut)}' should implement IJsonSerializable.");
        }

        using (var stringReader = new StringReader(input))
        {
            // using (var jsonReader = new JsonTextReader(stringReader))
            // {
            //     var obj = Activator.CreateInstance<TOut>();
            //
            //     if (!jsonReader.Read())
            //     {
            //         return obj;
            //     }
            //     
            //     if (jsonReader.Read())
            //     {
            //         ((IJsonSerializable)obj).ReadJson(jsonReader);
            //     }

            //    return obj;
            //}
            return null;
        }
    }

    public TOut Deserialize<TOut>(Stream input)
    {
        throw new NotImplementedException();
    }

    public string Serialize(IRedmineConverter input)
    {
        throw new NotImplementedException();
    }

    public string Serialize<TIn>(IRedmineConverter<TIn> entity) where TIn : class
    {
        if (entity == default(TIn))
        {
            throw new ArgumentNullException(nameof(entity), $"Could not serialize null of type {typeof(TIn).Name}");
        }

        var jsonSerializable = entity as IJsonSerializable;

        if (jsonSerializable == null)
        {
            throw new RedmineException($"Entity of type '{typeof(TIn)}' should implement IJsonSerializable.");
        }

        var stringBuilder = new StringBuilder();

        using (var sw = new StringWriter(stringBuilder))
        {
            // using (var writer = new JsonTextWriter(sw))
            // {
            //     writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            //     writer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //
            //     jsonSerializable.WriteJson(writer);
            //
            //     var json = stringBuilder.ToString();
            //
            //     stringBuilder.Length = 0;
            //    
            //     return json;
            // }
            return null;
        }
    }
}