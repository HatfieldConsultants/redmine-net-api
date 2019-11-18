 #if !(NET20)
using System.Net.Http;
using System.Text;

namespace Redmine.Api
{
    /// <summary>
    /// Provides HTTP content based on a <c>JSON</c> string.
    /// </summary>
    public sealed class JSONContent : StringContent
    {
        private const string JSON_MIME = "application/json";

        /// <summary>
        /// Creates an instance of the <see cref="JSONContent"/> with the default encoding of <see cref="Encoding.UTF8"/>.
        /// </summary>
        public JSONContent(string jsonContent) : base(jsonContent, Encoding.UTF8, JSON_MIME) {}

        /// <summary>
        /// Creates an instance of the <see cref="JSONContent"/> with the given <paramref name="encoding"/>.
        /// </summary>
        public JSONContent(string jsonContent, Encoding encoding) : base(jsonContent, encoding, JSON_MIME) {}
    }
}
#endif