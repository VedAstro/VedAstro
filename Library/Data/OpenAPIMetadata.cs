using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library;

// Class to hold method documentation
public class MethodDocumentation
{
    public string Description { get; set; } // Property to hold the summary of the method
    public Dictionary<string, string> Params { get; set; } // Property to hold the parameters and their comments
    public int LineNumber { get; set; } // Property to hold the line number of the method signature
    public string Signature { get; set; }

    public MethodDocumentation()
    {
        Params = new Dictionary<string, string>(); // Initialize the Params dictionary
    }
}

/// <summary>
/// static meta data for a given method in Open API
/// </summary>
public class OpenAPIMetadata : EventArgs, IToJson
{
    public static List<OpenAPIMetadata> CachedAllMethoInfoList { get; set; } = new();

    public static readonly OpenAPIMetadata Empty = new OpenAPIMetadata("Empty", "Empty", "Empty", "Empty", "Empty");

    private PlanetName _selectedPlanet = PlanetName.Sun; //set default so dropdown has something on load
    private string _name;

    /// <summary>
    /// Special name for use in ML Data Tables, includes first param value added to name
    /// </summary>
    public string MLTableName(object resultOverride = null) => Tools.GetSpecialMLTableName(this, resultOverride);

    public OpenAPIMetadata(string signature, string lineNumber, string paramDescription, string description, string exampleOutput, MethodInfo methodInfo = null)
    {
        Description = description;
        ExampleOutput = exampleOutput;
        Signature = signature;
        MethodInfo = methodInfo;
        LineNumber = lineNumber;
        ParameterDescription = paramDescription;
    }

    public OpenAPIMetadata() { }

    /// <summary>
    /// includes full method signature in string to ID the method
    /// </summary>
    public string Signature { get; set; }

    /// <summary>
    /// Combined text of signature and comments above code
    /// </summary>
    public string SearchText => Description + ParameterDescription + Signature;

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
    /// Line number in code where method is located
    /// used for navigating to code in GitHub
    /// </summary>
    public string LineNumber { get; set; }

    /// <summary>
    /// All parameters and their comments in a single string, format: "param1: comment1, param2: comment2"
    /// Note use of ExtractValue method to get the comment for a given parameter
    /// </summary>
    public string ParameterDescription { get; set; }

    /// <summary>
    /// Example output of the method printed as JSON
    /// </summary>
    public string ExampleOutput { get; set; }

    /// <summary>
    /// Direct name of method in code
    /// can be set, but default to name from Method info if not set
    /// </summary>
    public string Name
    {
        get
        {
            //if null use name from method info
            if (string.IsNullOrEmpty(_name)) { _name = MethodInfo?.Name; }

            return _name;
        }
        set => _name = value;
    }

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
    /// SPECIAL HACK METHOD to inject custom params for use in ML Data Generator
    /// made for Blazor binding
    /// </summary>
    public PlanetName SelectedPlanet
    {
        get => _selectedPlanet;
        set
        {
            //inject custom planet name as param into selected method meta
            this.SelectedParams?.Clear(); //important to clear previous values
            this.SelectedParams?.Add(value);

            _selectedPlanet = value;
        }
    }

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
        // Return cached list if available
        if (OpenAPIMetadata.CachedAllMethoInfoList.Any()) { return OpenAPIMetadata.CachedAllMethoInfoList; }

        // Calculate new list if cache is not available
        OpenAPIMetadata.CachedAllMethoInfoList = GenerateMetadataList();

        return OpenAPIMetadata.CachedAllMethoInfoList;

        //-----------------------------------------------------------
        List<OpenAPIMetadata> GenerateMetadataList()
        {
            // Include all API methods if calcList is null
            calcList ??= Tools.GetAllApiCalculatorsMethodInfo();
            var finalList = new List<OpenAPIMetadata>();
            // Populate final list with API description
            foreach (var calc in calcList)
            {
                // Get pre-created comments using the method's signature ID
                var signature = calc.GetMethodSignature();
                var metadata = OpenAPIStaticTable.Rows.FirstOrDefault(x => x.Signature == signature);

                // Log to server and skip current iteration if metadata is null
                if (metadata == null)
                {
                    LibLogger.Debug($"METHOD NOT FOUND!!!! --> {signature}");
                    continue;
                }

                // Link current code instance
                metadata.MethodInfo = calc;

                // Add to final list
                finalList.Add(metadata);
            }

            // Sort the list alphabetically based on the Name property
            finalList.Sort((metadata1, metadata2) => metadata1.Name.CompareTo(metadata2.Name));
            return finalList;
        }
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
        var clonedDolly = new OpenAPIMetadata(Signature, LineNumber, ParameterDescription, Description, ExampleOutput, MethodInfo);

        return clonedDolly;
    }

    /// <summary>
    /// Given a list of OpenAPIMeta wrapped in json will convert to instance
    /// used for transferring between server & client
    /// </summary>
    public static List<OpenAPIMetadata> FromJsonList(JToken columnDataList)
    {
        //if null empty list please
        if (columnDataList == null) { return new List<OpenAPIMetadata>(); }

        var returnList = new List<OpenAPIMetadata>();

        foreach (var json in columnDataList)
        {
            returnList.Add(OpenAPIMetadata.FromJson(json));
        }

        return returnList;
    }

    public static OpenAPIMetadata FromJson(JToken timeJson)
    {
        try
        {
            var signatureString = timeJson["Signature"].Value<string>();
            var lineNumberString = timeJson["LineNumber"].Value<string>();
            var parameterDescriptionString = timeJson["ParameterDescription"].Value<string>();
            var descriptionString = timeJson["Description"].Value<string>();
            var exampleOutputString = timeJson["ExampleOutput"].Value<string>();

            //# SELECTED PARAMS
            var selectedParamsString = timeJson["SelectedParams"];
            var parsedTime = new OpenAPIMetadata(signatureString, lineNumberString, parameterDescriptionString, descriptionString, exampleOutputString);
            var parsedParamList = new List<object>();
            foreach (var xx in selectedParamsString)
            {
                //get type name
                var jProperty = (JProperty)xx;
                var jPropertyName = jProperty.Name;

                //dig deep do take out value
                var value = jProperty.Value.Value<string>();

                //based on type name, recreate instance
                switch (jPropertyName)
                {
                    case "PlanetName":
                        parsedParamList.Add(PlanetName.Parse(value));
                        break;
                    case "HouseName":
                        parsedParamList.Add(Enum.Parse<HouseName>(value));
                        break;

                }
            }

            parsedTime.SelectedParams = parsedParamList;

            //# METHOD INFO
            var methodInfoJson = (JObject)timeJson["MethodInfo"];
            var vv = Tools.MethodInfoFromJson(methodInfoJson);
            parsedTime.MethodInfo = vv;

            return parsedTime;

        }
        catch (Exception e)
        {
            LibLogger.Debug(e, "Failed to Parse");
            return OpenAPIMetadata.Empty;
        }
    }

    public JObject ToJson()
    {
        var temp = new JObject();
        temp[nameof(this.Signature)] = this.Signature;
        temp[nameof(this.Description)] = this.Description;
        temp[nameof(this.ExampleOutput)] = this.ExampleOutput;

        //# SELECTED PARAMS

        //place each property in nicely in Json
        var package = new JObject();
        if (this?.SelectedParams != null)
        {
            foreach (var xx in this?.SelectedParams)
            {
                var typeNameStr = xx.GetType().Name;
                var valueStr = xx.ToString();
                var yy = new JProperty(typeNameStr, valueStr);
                package.Add(yy);
            }
        }

        temp[nameof(this.SelectedParams)] = package;


        //# METHOD INFO
        temp[nameof(this.MethodInfo)] = this.MethodInfo.ToJson();



        return temp;
    }

    /// <summary>
    /// Given a parameter name will return the comment for that parameter
    /// </summary>
    public string GetParamDesc(string paramName)
    {
        //raw format is "param1: comment1, param2: comment2"
        string[] pairs = ParameterDescription.Split(',');
        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split(':');
            var nameInDesc = keyValue[0].Trim().ToLower();
            var inputedName = paramName.ToLower();
            var isMatch = nameInDesc == inputedName;
            if (keyValue.Length == 2 && isMatch)
            {
                //return the C# comments for the parameter
                return keyValue[1].Trim();
            }
        }
        return "";
    }

    public List<ParameterMetadata> GetParamList()
    {
        //set new params
        var methodInf = this?.MethodInfo;

        //extract data of param types and their names in C# code (which will be displayed)
        var paramTypeList = new List<ParameterMetadata>();
        foreach (var param in methodInf.GetParameters())
        {
            var paramDescription = this?.GetParamDesc(param.Name) ?? "";
            paramTypeList.Add(new(param, paramDescription));
        }


        return paramTypeList;

    }

}