namespace Genso.Astrology.Library
{
    /// <summary>
    /// A simple data type to hold calendars in an account, currently made for google,
    /// in future should e able to use for icloud & outlooks
    /// </summary>
    public class Calendar
    {
        public string Name { get; set; }
        public string Id { get; set; }


        public override string ToString() => Name;
    }
}