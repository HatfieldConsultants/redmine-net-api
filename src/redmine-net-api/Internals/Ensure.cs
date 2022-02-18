using Redmine.Net.Api.Exceptions;

namespace Redmine.Net.Api.Old.Internals;

/// <summary>
/// 
/// </summary>
public static class Ensure
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="RedmineException"></exception>
    public static void ArgumentNotNull(object value, string name)
    {
        if (value != null) return;

        throw new RedmineException("Argument null.",name);
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="RedmineException"></exception>
    public static void ArgumentNotNullOrEmptyString(string value, string name)
    {
        if (!string.IsNullOrWhiteSpace(value)) return;

        throw new RedmineException("String cannot be null or empty", name);
    }
}