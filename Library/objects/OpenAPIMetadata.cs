using System.Reflection;
using System.Text;

namespace VedAstro.Library;


/// <summary>
/// static meta data for a given method in Open API
/// </summary>
public class OpenAPIMetadata
{
    public OpenAPIMetadata()
    {
    }

    public OpenAPIMetadata(string description, string exampleOutput, string signature, MethodInfo methodInfo)
    {
        Description = description;
        ExampleOutput = exampleOutput;
        Signature = signature;
        MethodInfo = methodInfo;
    }

    /// <summary>
    /// includes full method signature in string to ID the method
    /// </summary>
    public string Signature { get; set; }

    public MethodInfo MethodInfo { get; }

    /// <summary>
    /// comment from code injected here
    /// </summary>
    public string Description { get; set; }

    public string ExampleOutput { get; set; }

    /// <summary>
    /// generates python method stub declaration code
    /// EXP: def HousePlanetIsIn(time: Time, planet_name: PlanetName) -> HouseName:
    /// </summary>
    public string ToPythonMethodNameStub()=> Tools.GeneratePythonDef(this.MethodInfo);

    /// <summary>
    /// puts description into Python format
    /// </summary>
    public string ToPythonMethodDescStub()
    {
        var sb = new StringBuilder();
        //NOTE: Spacing below set by testing
        sb.AppendLine("\"\"\"");
        sb.AppendLine($"        {Description}");
        sb.AppendLine($"        :return: {this.MethodInfo.ReturnType.Name}"); //needed to make appear in PyCharm
        sb.Append("         \"\"\"");

        return sb.ToString();

    }
}