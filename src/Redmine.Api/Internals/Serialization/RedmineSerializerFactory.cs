using System;

namespace Redmine.Api.Internals.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class RedmineSerializerFactory
    {
        private RedmineSerializerFactory()
        {
            
        }
        
        public static IRedmineSerializer Serializer(MimeFormat format)
        {
            switch (format)
            {
                case MimeFormat.Xml:
                    return  new XmlRedmineSerializer();
                case MimeFormat.Json:
                    return new JsonRedmineSerializer();
                default:
                    throw new NotImplementedException($"{nameof(format)} serializer");
            }
        }
    }
}