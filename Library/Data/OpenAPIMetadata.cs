using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VedAstro.Library;

///// <summary>
///// Data structure to hold api call method's name and description
///// INT = Outputs: Int32, List<string> = List of String
///// </summary>
//public readonly record struct APICallData(string Name, string Description, List<Type> ParamTypeList, string ReturnType, string SearchText)
//{
//    /// <summary>
//    /// Given a type will return is most relevant name
//    /// </summary>
//    public static string GetTypeName(Type type)
//    {
//        if (type.IsGenericType)
//        {
//            Type itemType = type.GetGenericArguments()[0];
//            return type.Name.Split('`')[0] + " of " + itemType.Name;
//        }
//        else
//        {
//            return type.Name;
//        }
//    }


//    /// <summary>
//    /// Given a list of method info will convert to a list of API call Data
//    /// </summary>
//    public static List<APICallData> FromMethodInfoList(IEnumerable<MethodInfo> calcList)
//    {
//        var finalList = new List<APICallData>();

//        //make final list with API description
//        //get nice API calc name, shown in builder dropdown
//        foreach (var calc in calcList)
//        {
//            List<Type> parameterTypes = calc.GetParameters()
//                .Select(param => param.ParameterType)
//                .ToList();

//            var apiSpecialName = calc.Name;

//            var apiSpecialDescription = Tools.GetAPISpecialDescription(calc);
//            finalList.Add(new APICallData(
//                Name: apiSpecialName,
//                Description: apiSpecialDescription,
//                ParamTypeList: parameterTypes,
//                ReturnType: GetTypeName(calc.ReturnType), //gets type name nicely formatted
//                SearchText: calc.GetAllDataAsText() + apiSpecialDescription + apiSpecialName));
//        }

//        return finalList;
//    }

//}

/// <summary>
/// static meta data for a given method in Open API
/// </summary>
public class OpenAPIMetadata
{
    /// <summary>
    /// Special name for use in ML Data Tables, includes first param value added to name
    /// </summary>
    public string MLTableName => Tools.GetSpecialMLTableName(this);

    public OpenAPIMetadata(string signature, string description, string exampleOutput, MethodInfo methodInfo = null)
    {
        Description = description;
        ExampleOutput = exampleOutput;
        Signature = signature;
        MethodInfo = methodInfo;
    }

    public OpenAPIMetadata() { }

    /// <summary>
    /// includes full method signature in string to ID the method
    /// </summary>
    public string Signature { get; set; }


    /// <summary>
    /// Combined text of signature and comments above code
    /// </summary>
    public string SearchText => Description + Signature;

    /// <summary>
    /// The live reference to the method, only loaded later 
    /// </summary>
    public MethodInfo? MethodInfo { get; set; }

    /// <summary>
    /// Get all types of the parameter as list, used to generate new code & highlighting
    /// </summary>
    public List<Type> ParameterTypes => MethodInfo?.GetParameters().Select(param => param.ParameterType).ToList() ?? new List<Type>();

    /// <summary>
    /// comment from above method in C# code injected here
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Example output of the method printed as JSON
    /// </summary>
    public string ExampleOutput { get; set; }

    /// <summary>
    /// Direct name of method in code
    /// </summary>
    public string Name => MethodInfo?.Name ?? "";

    /// <summary>
    /// return type in string for show case
    /// </summary>
    public object ReturnType => GetTypeName(MethodInfo?.ReturnType);

    /// <summary>
    /// SPECIAL HACK METHOD to inject custom params for use in ML Data Generator
    /// Users inputs this params instances before executing API method
    /// </summary>
    public List<object> SelectedParams { get; set; }


    /// <summary>
    /// generates python method stub declaration code
    /// EXP: def HousePlanetIsIn(time: Time, planet_name: PlanetName) -> HouseName:
    /// </summary>
    public string ToPythonMethodNameStub() => Tools.GeneratePythonDef(this.MethodInfo);

    /// <summary>
    /// puts description into Python format
    /// </summary>
    public string ToPythonMethodDescStub()
    {
        var sb = new StringBuilder();
        //NOTE: Spacing below set by testing
        sb.AppendLine("\"\"\"");
        sb.AppendLine($"        {Description}");
        sb.AppendLine($"        :return: {this.MethodInfo?.ReturnType.Name ?? "NO NAME FOUND!"}"); //needed to make appear in PyCharm
        sb.Append("         \"\"\"");

        return sb.ToString();

    }

    /// <summary>
    /// Given a list of method info will convert to a list of API call Data
    /// if none provided, will use ALL from Open API
    /// </summary>
    public static List<OpenAPIMetadata> FromMethodInfoList(IEnumerable<MethodInfo> calcList = null)
    {
        //if null include all API methods
        calcList = calcList ?? Tools.GetAllApiCalculatorsMethodInfo();

        var finalList = new List<OpenAPIMetadata>();
        //make final list with API description
        //get nice API calc name, shown in builder dropdown
        foreach (var calc in calcList)
        {
            //using the method's signature ID get the pre created comments
            var signature = calc.GetMethodSignature();
            var metadata = OpenAPIStaticTable.Rows.Where(x => x.Signature == signature).FirstOrDefault();

            //if null than raise silent alarm!, don't add to return list todo log to server
            if (metadata == null) { Console.WriteLine($"METHOD NOT FOUND!!!! --> {signature}"); continue; }

            //add link to current code instance
            metadata.MethodInfo = calc;

            //add to final list
            finalList.Add(metadata);
        }
        // Sort the list alphabetically based on the Name property
        finalList = finalList.OrderBy(metadata => metadata.Name).ToList();
        return finalList;
    }

    /// <summary>
    /// Given a type will return is most relevant name
    /// </summary>
    public static string GetTypeName(Type type)
    {
        if (type == null) { return "null"; }

        if (type.IsGenericType)
        {
            Type itemType = type.GetGenericArguments()[0];
            return type.Name.Split('`')[0] + " of " + itemType.Name;
        }
        else
        {
            return type.Name;
        }
    }

    /// <summary>
    /// Safely adds to Selected param list no init needed
    /// </summary>
    public void AddSelectedParams(PlanetName inputPlanet)
    {
        //assign new if null
        this.SelectedParams ??= new List<object>();
        this.SelectedParams.Add(inputPlanet);
    }

    /// <summary>
    /// Return a new instance
    /// </summary>
    public OpenAPIMetadata Clone()
    {
        var clonedDolly = new OpenAPIMetadata(Signature, Description, ExampleOutput, MethodInfo);

        return clonedDolly;
    }
}