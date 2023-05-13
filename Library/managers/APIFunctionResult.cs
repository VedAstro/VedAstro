using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library;

/// <summary>
/// Holds calculator name and the result outputed by it
/// </summary>
public record APIFunctionResult(string Name, object Result)
{

    /// <summary>
    /// special override to print out any type of data
    /// nicely for HUMAN EYES
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var returnData = "";
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
}