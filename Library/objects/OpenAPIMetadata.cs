namespace VedAstro.Library;


/// <summary>
/// static meta data for a given method in Open API
/// </summary>
public class OpenAPIMetadata
{
    public OpenAPIMetadata()
    {
    }

    public OpenAPIMetadata(string description, string exampleOutput, string signature)
    {
        Description = description;
        ExampleOutput = exampleOutput;
        Signature = signature;
    }

    /// <summary>
    /// includes full method signature in string to ID the method
    /// </summary>
    public string Signature { get; set; }

    /// <summary>
    /// comment from code injected here
    /// </summary>
    public string Description { get; set; }

    public string ExampleOutput { get; set; }
}