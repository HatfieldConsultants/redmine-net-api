using System;

namespace Redmine.dotNet.Api;

internal sealed class RedmineSettings : IRedmineSettings
{
    public Uri Host { get; set; }
   
    public RedmineSerializationType SerializationType { get; set; } = RedmineSerializationType.Xml;
    public IRedmineAuth Authentication { get; set; } 

    public bool VerifyServerCert { get; set; } = true;

    //  public IWebProxy Proxy{ get;  }
    //   public SecurityProtocolType SecurityProtocolType { get;  } = default;
    public TimeSpan? Timeout { get; set; }
}