namespace VedAstro.Library;

public struct DegreeRange(double startDegree, double endDegree)
{
    public double StartDegree { get; set; } = startDegree;
    public double EndDegree { get; set; } = endDegree;

    /// <summary>
    /// returns true if given number is within start and end range
    /// </summary>
    public bool IsWithinRange(double degree) => degree >= StartDegree && degree <= EndDegree;
}