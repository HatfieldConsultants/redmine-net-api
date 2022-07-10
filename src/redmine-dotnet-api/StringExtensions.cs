namespace Redmine.dotNet.Api;

public static class StringExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
    
    public static string ToLowerInvariant(this string obj)
    {
        if (string.IsNullOrEmpty(obj))
        {
            return obj;
        }

        return obj.ToLowerInvariant();
    }
}

public static class ObjectExtensions
{
    public static string ToLowerInvariant<T>(this T obj) where T:struct
    {
        return obj.ToString().ToLowerInvariant();
    }
}