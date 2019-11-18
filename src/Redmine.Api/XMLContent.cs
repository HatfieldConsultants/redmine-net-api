 #if !(NET20)
using System.Net.Http;
using System.Text;

namespace Redmine.Api
{
    /// <summary>
    /// Provides HTTP content based on an <c>XML</c> string.
    /// </summary>
    public sealed class XMLContent : StringContent
    {
        private const string XML_MIME = "application/xml";

        /// <summary>
        /// Creates an instance of the <see cref="XMLContent"/> with the default encoding of <see cref="Encoding.UTF8"/>.
        /// </summary>
        public XMLContent(string xmlContent) : base(xmlContent, Encoding.UTF8, XML_MIME) { }
        
        /// <summary>
        /// Creates an instance of the <see cref="XMLContent"/> with the given <paramref name="encoding"/>.
        /// </summary>
        public XMLContent(string xmlContent, Encoding encoding) : base(xmlContent, encoding, XML_MIME) {}
    }
}
#endif