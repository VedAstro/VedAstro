using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace VedAstro.Library
{

    /// <summary>
    /// Compiled list of properties attached to a Sign
    /// </summary>
    public class SignProperties : IToJpeg, IToJson, IToDataTable
    {
        public class SignInfo
        {
            public string Abbreviations { get; }
            public string Nature { get; }
            public string Element { get; }
            public string Gender { get; }
            // New properties
            public string Direction { get; }
            public string Constitution { get; }
            public string Guna { get; }
            public string BeneficMalefic { get; }
            public string OddEven { get; }
            public string Caste { get; }
            public string Symbol { get; }
            public string SignLord { get; }
            public string ModeOfRising { get; }
            public string BodyPart { get; }
            public string PositiveTrait { get; }
            public string Ascension { get; }
            public string Radiance { get; }
            public string Resides { get; }
            public string Colour { get; }
            public string Purushartha { get; }
            public string NegativeTrait { get; }
            public string Appearance { get; }
            public string Feet { get; }
            public string Mansion { get; }
            public string RatriDivabali { get; }
            public string BodilyConstitution { get; }
            public string Fruitfulness { get; }

            public SignInfo(
                string abbreviations, string nature, string element, string gender,
                string direction, string constitution, string guna, string beneficMalefic,
                string oddEven, string caste, string symbol, string signLord,
                string modeOfRising, string bodyPart, string positiveTrait, string ascension,
                string radiance, string resides, string colour, string purushartha,
                string negativeTrait, string appearance, string feet, string mansion,
                string ratriDivabali, string bodilyConstitution, string fruitfulness)
            {
                Abbreviations = abbreviations;
                Nature = nature;
                Element = element;
                Gender = gender;
                // Initialize new properties
                Direction = direction;
                Constitution = constitution;
                Guna = guna;
                BeneficMalefic = beneficMalefic;
                OddEven = oddEven;
                Caste = caste;
                Symbol = symbol;
                SignLord = signLord;
                ModeOfRising = modeOfRising;
                BodyPart = bodyPart;
                PositiveTrait = positiveTrait;
                Ascension = ascension;
                Radiance = radiance;
                Resides = resides;
                Colour = colour;
                Purushartha = purushartha;
                NegativeTrait = negativeTrait;
                Appearance = appearance;
                Feet = feet;
                Mansion = mansion;
                RatriDivabali = ratriDivabali;
                BodilyConstitution = bodilyConstitution;
                Fruitfulness = fruitfulness;
            }
        }

        public SignInfo Properties { get; set; }

        private static readonly Dictionary<ZodiacName, SignInfo> signData = new()
            {
                { ZodiacName.Aries, new SignInfo("Ar", "Movable/Chara/Mutable", "Fiery/Fire", "Male", "East", "Dhaatu", "Rajasik", "Malefic", "Odd", "Kshatriya", "Goat/Ram", "Mars", "Prishtodaya", "Head", "Quick/Practical", "Short", "Lustreless", "Garden, Hills, Mountains", "Blood Red", "Dharma", "Bad temper, Haste, Headstrong", "Rough", "Quadraped", "Lunar", "Ratribali", "Pitta", "Barren") },
                { ZodiacName.Taurus, new SignInfo("Ta", "Fixed", "Prithvi/Earth", "Female", "South", "Mula", "Rajasik", "Benefic", "Even", "Vaishya", "Bull", "Venus", "Prishtodaya", "Face", "Practical, Forgiving, Patient", "Medium", "Lustreless", "Village", "White", "Artha", "Domineering/Rage like a Bull", "Quadruped", "Rough", "Lunar", "Ratribali", "Vata", "Semi-Fruitful") },
                { ZodiacName.Gemini, new SignInfo("Ge", "Dual", "Airy", "Male", "West", "Jeeva", "Tamasik", "Malefic", "Odd", "Shudra", "Couple (Male and Female) Holding a Lute", "Mercury", "Shirshodaya", "Shoulder", "Diplomatic/Good Speakers/Learned", "Long", "Shining", "Pleasure Houses", "Green", "Kama", "Lacks concentration, Hasty, Dual, Mind", "Biped", "Smooth", "Lunar", "Ratribali", "Mixed Doshas (mishra or sama doshas)", "Barren") },
                { ZodiacName.Cancer, new SignInfo("Cn", "Movable/Chara/Mutable", "Water", "Female", "North", "Dhathu", "Sattvik", "Benefic", "Even", "Brahmin", "Crab", "Moon", "Prishtodaya", "Chest", "Sociable, Nurturing", "Medium", "Lustreless", "Water body in forest (Absence of human)", "Reddish brown, Pink", "Moksha", "Self-centred, Vengeance, Secretive", "Centipede", "Smooth", "Lunar", "Ratribali", "Kapha", "Fruitful") },
                { ZodiacName.Leo, new SignInfo("Le", "Fixed", "Fire", "Male", "East", "Moola", "Sattvik", "Malefic", "Odd", "Kashatriya", "Lion", "Sun", "Shirshodaya", "Stomach", "Leadership, Loyal", "Short", "Lustreless", "Forest", "Pale White", "Dharma", "Egoistic, Dominating", "Quadruped", "Rough", "Solor", "Divabali", "Pitta", "Barren") },
                { ZodiacName.Virgo, new SignInfo("Vi", "Dual", "Earthly/Earth", "Female", "South", "Jeeva", "Tamasik", "Benefic", "Even", "Vaishya", "A maiden holding sheaf in his hand!", "Mercury", "Shirshodaya", "Belly", "Practical", "Short", "Lustreless", "Fields", "Green", "Moksha", "Self-centred, Vengeance, Secretive", "Centipede", "Smooth", "Lunar", "Ratribali", "Kapha", "Fruitful") },
                { ZodiacName.Libra, new SignInfo("Li", "Movable/Chara/Mutable", "Airy/Air", "Male", "West", "Jeeva", "Rajasik", "Benefic", "Odd", "Shudra", "A man holding balance in his hand!", "Venus", "Shirshodaya", "Lower abdomen", "Diplomatic, Balanced", "Medium", "Lustreless", "Open ground", "Black, Greenish white", "Dharma", "Indecisive, Lazy", "Quadruped", "Rough", "Solar", "Divabali", "Vata", "Semi-fruitful") },
                { ZodiacName.Scorpio, new SignInfo("Sc", "Fixed/Sthira", "Water", "Female", "North", "Moola", "Rajasik", "Benefic", "Even", "Brahmin", "Scorpion/Arthropod", "Mars", "Shirshodaya", "Private Parts", "Energetic, Bold, Shrewd, Spirituality", "Medium", "Shining", "Found in Holes or cracks in land", "Saffron/Grid", "Moksha", "Rash, Selfish, Secretive, Vengefiul", "Centripede", "Smooth","Solar", "Divabali", "Kapha", "Fruitful") },
                { ZodiacName.Sagittarius, new SignInfo("Sg", "Dual", "Fire", "Male", "East", "Jeeva", "Sattvik", "Malefic", "Odd", "Kshatriya", "A centaur holding a bow from which he is about to shoot an arrow", "Jupiter", "Prishtodaya", "Thighs", "Learned in Shastra, Philosophical", "Long", "Lustreless", "Battlefields", "Pale", "Dharma", "Over ambition, Exaggeration, False Promises", "Quadruped","Rough", "Lunar", "Ratribali", "Pitta", "Semi-Fruitful") },
                { ZodiacName.Capricorn, new SignInfo("Cp", "Movable/Chara/Mutable", "Earth", "Female", "South", "Dhaatu", "Tamasik", "Benefic", "Even", "Vaishya", "A crocodile with the upper body of a deer", "Saturn", "Prishtodaya", "Knees", "Clever, Learned, Common Sense", "Long", "Lustreless", "Wanders on land and in Forests", "Wheatish, Yellow", "Artha", "Lazy, Indolent, Workholic", "1st half Quadruped, 2nd half footless", "Rough", "Lunar", "Ratribali", "Vata", "Semi-Fruitful") },
                { ZodiacName.Aquarius, new SignInfo("Aq", "Fixed/Sthira", "Airy", "Male", "West", "Moola", "Tamasik", "Malefic", "Odd", "Shudra", "A man with a pitcher tilted on his shoulder and contents are being poured out", "Saturn", "Shirshodaya", "Ankle", "Philosophy, Service, Intellectual", "Medium", "Shining", "Near water on the land", "Dark Brown", "Kama", "Pessimistic, Worried, Gloomy, Lazy", "Biped", "Smooth", "Solar", "Divabali", "Mixed Doshas (mishra or sama doshas)", "Semi-Fruitful") },
                { ZodiacName.Pisces, new SignInfo("Pi", "Dual", "Water", "Female", "North", "Jeeva", "Sattvik", "Benefic", "Even", "Brahmin", "Pair of fish swimming in opposite directions", "Jupiter", "Ubhayodaya (Rising with Body)", "Feet", "Learned, Kind, Loving, Reserved", "Short", "Shining", "Clean Water", "Violet", "Moksha", "Selfish, Indecisive, Ego-centric, Moody", "Footless", "Smooth", "Solar", "Divabali", "Kapha", "Fruitful") }
            };

        public SignProperties(ZodiacName inputSignName)
        {
            if (signData.TryGetValue(inputSignName, out var signInfo))
            {
                Properties = signInfo;
            }
            else
            {
                throw new ArgumentException("Invalid ZodiacName");
            }
        }

        public byte[] ToJpeg()
        {
            var table = this.ToDataTable();
            return Tools.DataTableToJpeg(table);
        }

        public DataTable ToDataTable()
        {
            // Create a new DataTable.
            DataTable table = new DataTable("SignProperties");

            // Define columns.
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(string));

            // Use reflection to get properties of SignInfo.
            var properties = Properties.GetType().GetProperties();

            // Fill rows dynamically with properties.
            foreach (var property in properties)
            {
                table.Rows.Add(property.Name, property.GetValue(Properties));
            }

            return table;
        }

        public JObject ToJson()
        {
            var returnVal = new JObject();

            // Use reflection to get properties of SignInfo.
            var properties = Properties.GetType().GetProperties();

            // Add properties dynamically to JObject.
            foreach (var property in properties)
            {
                returnVal[property.Name] = property.GetValue(Properties)?.ToString();
            }

            return returnVal;
        }
    }
}