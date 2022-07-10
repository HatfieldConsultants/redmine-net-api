using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Redmine.dotNet.Api.Serialization.Xml.Converters;

public abstract class IdentifiableConverter: IXmlSerializable
{
    public Type Type => this.GetType();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public XmlSchema GetSchema() { return null; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    public virtual void ReadXml(XmlReader reader) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    public virtual void WriteXml(XmlWriter writer) { }
}