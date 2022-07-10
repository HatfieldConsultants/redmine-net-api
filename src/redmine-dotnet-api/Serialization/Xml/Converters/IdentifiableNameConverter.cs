using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Redmine.dotNet.Api.Types;

namespace Redmine.dotNet.Api.Serialization.Xml.Converters;

public sealed class IdentifiableNameConverter: IdentifiableConverter, IRedmineConverter<IdentifiableName>
{
    public IdentifiableNameConverter()
    {
        
    }

    public IdentifiableNameConverter(IdentifiableName innerData)
    {
        Payload = innerData;
    }
    public IdentifiableName Payload { get; private set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    public override void ReadXml(XmlReader reader)
    {
        var id = reader.ReadAttributeAsInt(RedmineKeys.ID);
        var name = reader.GetAttribute(RedmineKeys.NAME);
        reader.Read();
        
        Payload = new IdentifiableName(id, name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    public override void WriteXml(XmlWriter writer)
    {
        if (Payload == null)
        {
            return;
        }
        writer.WriteAttributeString(RedmineKeys.ID, Payload.Id.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString(RedmineKeys.NAME, Payload.Name);
    }
}