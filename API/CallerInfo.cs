namespace API;

public class CallerInfo
{
    public CallerInfo(string visitorId, string userId)
    {
        this.VisitorId = visitorId;
        this.UserId = userId;

        //set caller ID here 
        CallerId = VedAstro.Library.Tools.GetCallerId(UserId, VisitorId);
    }

    /// <summary>
    /// Can be overriden if needed for cache sharing
    /// </summary>
    public string CallerId { get; set; }

    public string VisitorId { get; set; }

    public string UserId { get; set; }


    public override string ToString()
    {

        return $"{{ UserId = {UserId}, VisitorId = {VisitorId} }}";
    }

    /// <summary>
    /// true if user is logged in uses 101
    /// </summary>
    public bool IsLoggedIn => UserId != "101";

    /// <summary>
    /// Used to get guest public account data
    /// </summary>
    public bool Both101 => UserId == "101" && VisitorId == "101";


    /// <summary>
    /// for getting cache not data from xml
    /// </summary>

}