using System.Diagnostics;
using System.Net;

namespace Redmine.Api.Tests
{
	public class RedmineFixture
	{
	    public RedmineManager RedmineManager { get; private set; }
	    public RedmineConnectionSettings ConnectionSettings { get; private set; }	    
	    
	    public RedmineFixture ()
		{
			ConnectionSettings = new RedmineConnectionSettings(){
                Host = "http://192.168.1.53:8089",
                ApiKey =("a96e35d02bc6a6dbe655b83a2f6db57b82df2dff"),
                Credentials = new NetworkCredential("zapadi","1qaz2wsx"),
                UserName = "zapadi",
                Password = "1qaz2wsx",
                KeepAlive =  true,
                UseApiKey = true
            };
			SetMimeTypeXML();
			SetMimeTypeJSON();
			RedmineManager = new RedmineManager(ConnectionSettings);
		}

        [Conditional("JSON")]
		private void SetMimeTypeJSON()
		{
			ConnectionSettings.Format = MimeFormat.Json;
		}

        [Conditional("XML")]
		private void SetMimeTypeXML()
		{
			ConnectionSettings.Format = MimeFormat.Xml;
		}
	}
}