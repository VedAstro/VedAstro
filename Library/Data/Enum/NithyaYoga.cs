using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    public enum NithyaYogaName
    {
        Vishkambha = 1,
        Priti = 2,
        Ayushman = 3,
        Saubhagya = 4,
        Sobhana = 5,
        Atiganda = 6,
        Sukarma = 7,
        Dhriti = 8,
        Soola = 9,
        Ganda = 10,
        Vriddha = 11,
        Dhruva = 12,
        Vyagatha = 13,
        Harshana = 14,
        Vajra = 15,
        Siddhi = 16,
        Vyatapata = 17,
        Variyan = 18,
        Parigha = 19,
        Siva = 20,
        Siddha = 21,
        Sadhya = 22,
        Subha = 23,
        Sukla = 24,
        Brahma = 25,
        Indra = 26,
        Vaidhriti = 27
    }

    public class NithyaYoga(NithyaYogaName name, string description) : IToJson, IToJpeg, IToDataTable
    {
        public NithyaYogaName Name { get; set; } = name;

        public string Description { get; set; } = description;

        /// <summary>
        /// Table programmed thanks to Mridul 🙏
        /// </summary>
        private static readonly Dictionary<NithyaYogaName, string> YogaDescriptions = new Dictionary<NithyaYogaName, string>
        {
            {NithyaYogaName.Vishkambha, "Door bolt/supporting pillar"},
            {NithyaYogaName.Priti, "Love/affection"},
            {NithyaYogaName.Ayushman, "Long-lived"},
            {NithyaYogaName.Saubhagya, "Long life of spouse (good fortune)"},
            {NithyaYogaName.Sobhana, "Splendid, bright"},
            {NithyaYogaName.Atiganda, "Great danger"},
            {NithyaYogaName.Sukarma, "One with good deeds"},
            {NithyaYogaName.Dhriti, "Firmness"},
            {NithyaYogaName.Soola, "Shiva's weapon of destruction (pain)"},
            {NithyaYogaName.Ganda, "Danger"},
            {NithyaYogaName.Vriddha, "Growth"},
            {NithyaYogaName.Dhruva, "Fixed, constant"},
            {NithyaYogaName.Vyagatha, "Great blow"},
            {NithyaYogaName.Harshana, "Cheerful"},
            {NithyaYogaName.Vajra, "Diamond (strong)"},
            {NithyaYogaName.Siddhi, "Accomplishment"},
            {NithyaYogaName.Vyatapata, "Great fall"},
            {NithyaYogaName.Variyan, "Chief/best"},
            {NithyaYogaName.Parigha, "Obstacle/hindrance"},
            {NithyaYogaName.Siva, "Lord Shiva (purity)"},
            {NithyaYogaName.Siddha, "Accomplished/ready"},
            {NithyaYogaName.Sadhya, "Possible"},
            {NithyaYogaName.Subha, "Auspicious"},
            {NithyaYogaName.Sukla, "White, bright"},
            {NithyaYogaName.Brahma, "Creator (good knowledge and purity)"},
            {NithyaYogaName.Indra, "Ruler of gods"},
            {NithyaYogaName.Vaidhriti, "A class of gods"}
        };

        public static NithyaYoga FromNumber(double nithyaYogaNumber)
        {
            var name = (NithyaYogaName)nithyaYogaNumber;
            var description = GetDescriptionFromName(name);
            return new NithyaYoga(name, description);
        }

        private static string GetDescriptionFromName(NithyaYogaName yogaName)
        {
            if (YogaDescriptions.TryGetValue(yogaName, out var description))
            {
                return description;
            }
            else
            {
                return "Unknown Yoga"; // Or any other default value
            }
        }

        public override string ToString()
        {
            return $"{Name.ToString()} - {Description}";
        }

        /// <summary>
        /// Made by AI 🤖🤖
        /// </summary>
        public DataTable ToDataTable()
        {
            var dataTable = new DataTable();

            // Assuming Name and Description are properties of the class
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            var row = dataTable.NewRow();
            row["Name"] = Name.ToString();
            row["Description"] = Description;

            dataTable.Rows.Add(row);

            return dataTable;
        }

        public byte[] ToJpeg() { var table = this.ToDataTable(); return Tools.DataTableToJpeg(table); }

        public JObject ToJson()
        {
            var returnVal = new JObject();
            returnVal["Name"] = Name.ToString();
            returnVal["Description"] = Description;

            return returnVal;
        }
    }
}
