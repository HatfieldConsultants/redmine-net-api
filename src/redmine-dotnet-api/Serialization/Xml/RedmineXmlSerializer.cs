using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Redmine.dotNet.Api.Serialization.Xml.Converters;

namespace Redmine.dotNet.Api.Serialization.Xml;

internal sealed class RedmineXmlSerializer: IRedmineXmlSerialization
{
    private readonly XmlWriterSettings xmlWriterSettings;
    
    public RedmineXmlSerializer()
    {
        xmlWriterSettings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        };
    }

    //TODO: to ISerializerWriterSettings - XML & Json
    public RedmineXmlSerializer(XmlWriterSettings xmlWriterSettings)
    {
        this.xmlWriterSettings = xmlWriterSettings;
    }

    public RedmineSerializationType Type => RedmineSerializationType.Xml;

    public TOut Deserialize<TOut>(string input) where TOut: class
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input), $"Could not deserialize null or empty input for type '{typeof(TOut).Name}'.");
        }

        using (var textReader = new StringReader(input))
        {
            using (var xmlReader = XmlTextReaderBuilder.Create(textReader))
            {
                var serializer = new XmlSerializer(typeof(TOut));

                var entity = serializer.Deserialize(xmlReader);

                if (entity is TOut t)
                {
                    return t;
                }

                return default;
            }
        }
    }

    public TOut Deserialize<TOut>(Stream input)
    {
        throw new System.NotImplementedException();
    }

    public string Serialize(IRedmineConverter entity)
    {
        if (entity == default)
        {
            throw new ArgumentNullException(nameof(entity), $"Could not serialize a null converter");
        }

        using (var stringWriter = new StringWriter())
        {
            using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
            {
                var serializer = new XmlSerializer(entity.Type);

                serializer.Serialize(xmlWriter, entity);

                return stringWriter.ToString();
            }
        }
    }
}