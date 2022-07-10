using System;

namespace Redmine.dotNet.Api
{
    public sealed class RedmineSettingsBuilder : IRedmineSettingsBuilder
    {
        public string Host { get; set; }
        public RedmineSerializationType SerializationType { get; set; } = RedmineSerializationType.Xml;
        public IRedmineAuth Authentication { get; set; } 
        public bool VerifyServerCert { get; set; } = true;

        //  public IWebProxy Proxy{ get;  }
        //   public SecurityProtocolType SecurityProtocolType { get;  } = default;
        public TimeSpan? Timeout { get; set; }

        public IRedmineSettings Build()
        {
            var uri = TryCreateUri(Host);

            if (Authentication == null)
            {
                throw new ArgumentException("No authentication set.", nameof(Authentication));
            }
            
            return new RedmineSettings()
            {
                Authentication = Authentication,
                Host = uri,
                Timeout = Timeout,
                SerializationType = SerializationType,
                VerifyServerCert = VerifyServerCert
            };
        }

        private static Uri TryCreateUri(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Host cannot be null or empty", nameof(host));
            }

            if (!Uri.TryCreate(host, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Host is not valid.", nameof(host));
            }
            
            if (Uri.UriSchemeHttp.Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase) || Uri.UriSchemeHttps.Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return uri;
            }
                
            throw new ArgumentException("Host schema is not valid. Only http and https is accepted.", nameof(host));

        }
    }
}