namespace VedAstro.Library;

public record ConstellationAnimal(string Gender, AnimalName Animal)
{
    public override string ToString()
    {
        return $"{Animal.ToString()} - {Gender}";
    }
};