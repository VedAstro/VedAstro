namespace API;

public record ParsedRequest(string UserId, string VisitorId)
{
    public override string ToString()
    {
        return $"{{ UserId = {UserId}, VisitorId = {VisitorId} }}";
    }


    /// <summary>
    /// true if user is logged in uses 101
    /// </summary>
    public bool IsLoggedIn => UserId != "101";
}