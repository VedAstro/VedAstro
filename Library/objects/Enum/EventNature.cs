namespace VedAstro.Library
{
    /// <summary>
    /// General type to represent good, bad , neutral
    /// </summary>
    public enum EventNature
    {
        /// <summary>
        /// default value when not assigned
        /// </summary>
        Empty = 0,
        Good,
        /// <summary>
        /// Also represents Mixed results with good and bad
        /// </summary>
        Neutral, 
        Bad
    }
}