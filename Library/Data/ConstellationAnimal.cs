using Newtonsoft.Json.Linq;

namespace VedAstro.Library;

public record ConstellationAnimal(string Gender, AnimalName Animal) : IToJson
{
    public override string ToString()
    {
        return $"{Animal.ToString()} - {Gender}";
    }

    public JObject ToJson()
    {
        //package the row
        var valueHolder = new JObject
        {
            //make the columns
            ["Animal"] = Animal.ToString(),
            ["Gender"] = Gender.ToString(),
        };

        return valueHolder;
    }
};