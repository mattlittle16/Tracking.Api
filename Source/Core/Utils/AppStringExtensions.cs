namespace Tracking.Api.Core.Utils;

public static class AppStringExtensions
{
    public static string ExtractCookieValue(string cookieHeader, string cookieName)
    {
        var span = cookieHeader.AsSpan();
        var nameSpan = $"{cookieName}=".AsSpan();
        
        var startIndex = span.IndexOf(nameSpan, StringComparison.OrdinalIgnoreCase);
        if (startIndex == -1) return string.Empty;
        
        var valueStart = startIndex + nameSpan.Length;
        var remainingSpan = span[valueStart..];
        
        var endIndex = remainingSpan.IndexOf(';');
        if (endIndex == -1) endIndex = remainingSpan.Length;
        
        return remainingSpan[..endIndex].ToString();
    }   
}