using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library;

/// <summary>
/// Holds calculator name and the result outputed by it
/// </summary>
public record APIFunctionResult(string Name, object Result)
{
    /// <summary>
    /// Special name for use in ML Data Tables, includes param value added to name
    /// </summary>
    public string MLTableName(object resultOverride = null) => Tools.GetSpecialMLTableName(this, resultOverride);

    /// <summary>
    /// SPECIAL HACK METHOD to inject custom params for use in ML Data Generator
    /// Users inputs this params instances before executing API method
    /// set via add method
    /// </summary>
    public List<object> SelectedParams { get; private set; }

    /// <summary>
    /// special override to print out any type of data
    /// nicely for HUMAN EYES
    /// </summary>
    public override string ToString()
    {
        //TODO handle dictionary
        if (Result is IList iList) //handles results that have many props from 1 call, exp : SwissEphemeris
        {
            //convert list to comma separated string
            var parsedList = iList.Cast<object>().ToList(); //cast otherwise won't masuk
            var stringComma = Tools.ListToString(parsedList);

            return stringComma;
        }
        //normal conversion via to string
        else
        {
            return Result?.ToString();
        }

    }

    public JProperty ToJson()
    {
        //process list differently
        JProperty rootPayloadJson;
        if (Result is IList iList) //handles results that have many props from 1 call, exp : SwissEphemeris
        {
            //convert list to comma separated string
            var parsedList = iList.Cast<object>().ToList();
            var stringComma = Tools.ListToString(parsedList);

            rootPayloadJson = new JProperty(Name, stringComma);
        }
        //custom JSON converter available
        else if (Result is IToJson iToJson)
        {
            rootPayloadJson = new JProperty(Name, iToJson.ToJson());
        }
        //normal conversion via to string
        else
        {
            rootPayloadJson = new JProperty(Name, Result?.ToString());
        }


        return rootPayloadJson;

    }

    public static JToken ToJsonList(List<APIFunctionResult> apiFunctionResultList)
    {
        var returnOb = new JArray();
        var returnOb2 = new JObject();
        foreach (var apiFunctionResult in apiFunctionResultList)
        {
            var content = apiFunctionResult.Result;

            //pass in name of function as header
            var methodName = apiFunctionResult.Name;
            var rootPayloadJson = Tools.AnyToJSON(methodName, content);

            //if already contains mini data inside than we have to this one, else can't add into return object
            if (rootPayloadJson is JObject)
            {
                returnOb2.Add(methodName, rootPayloadJson);
            }
            else
            {
                returnOb2.Add(rootPayloadJson);
            }
        }

        return returnOb2;

    }

    /// <summary>
    /// gets best representation of data in result as text
    /// </summary>
    public string ResultAsString() => Tools.AnyToString(Result);

    /// <summary>
    /// Safely adds to Selected param list no init needed
    /// </summary>
    public void AddSelectedParams(dynamic inputPlanet)
    {
        //assign new if null
        this.SelectedParams ??= new List<object>();
        this.SelectedParams.Add(inputPlanet);
    }
}