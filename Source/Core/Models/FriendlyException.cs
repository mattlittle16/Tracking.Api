namespace Tracking.Api.Core.Models;

public sealed class FriendlyException : Exception
{
    public string Title { get; }
    public int Status { get; }
    public string Details { get; }

    public FriendlyException(string title, int status = 500, string details = "An error occurred")
        : base(details)
    {
        Title = title;
        Status = status;
        Details = details;
    }
}