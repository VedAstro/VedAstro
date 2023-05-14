namespace API;

public record ParsedRequest(string UserId, string VisitorId)
{
    public override string ToString()
    {
        return $"{{ UserId = {UserId}, VisitorId = {VisitorId} }}";
    }

    //get caller ID, so can use back any data if available
    public string CallerId => GetCallerId();


    /// <summary>
    /// true if user is logged in uses 101
    /// </summary>
    public bool IsLoggedIn => UserId != "101";


    /// <summary>
    /// for getting cache not data from xml
    /// </summary>
    private  string GetCallerId()
    {

        if (this.IsLoggedIn)
        {
            return this.UserId;
        }
        //if user NOT logged in then take his visitor ID as caller id
        else
        {
            return this.VisitorId;
        }


    }

}