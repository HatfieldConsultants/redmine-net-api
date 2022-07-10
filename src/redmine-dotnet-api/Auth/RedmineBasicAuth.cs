using System;

namespace Redmine.dotNet.Api;

public sealed class RedmineBasicAuth : IRedmineBasicAuth
{
    public RedmineBasicAuth(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(username));
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }
        
        Username = username;
        Password = password;
    }
    
    public void Authenticate()
    {
        
    }

    public string Username { get; }
    public string Password { get; }
}