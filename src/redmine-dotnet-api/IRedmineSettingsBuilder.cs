using System;

namespace Redmine.dotNet.Api;

public interface IRedmineSettingsBuilder
{
        public string Host { get; set; }
        public RedmineSerializationType SerializationType { get; set; }
        public IRedmineAuth Authentication { get; set; } 
        public bool VerifyServerCert { get; set; }
        
        public TimeSpan? Timeout { get; set; }
        
        public IRedmineSettings Build();
}