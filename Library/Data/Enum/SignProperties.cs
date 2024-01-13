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
        private class SignInfo
        {
            public string Abbreviations { get; }
            public string Nature { get; }
            public string Element { get; }
            public string Gender { get; }

            public SignInfo(string abbreviations, string nature, string element, string gender)
            {
                Abbreviations = abbreviations;
                Nature = nature;
                Element = element;
                Gender = gender;
            }
        }

        private SignInfo Properties { get; set; }

        private static readonly Dictionary<ZodiacName, SignInfo> signData = new()
            {
                { ZodiacName.Aries, new SignInfo("Ar", "Movable/Chara/Mutable", "Fiery/Fire", "Male") },
                { ZodiacName.Taurus, new SignInfo("Ta", "Fixed", "Prithvi/Earth", "Female") },
                { ZodiacName.Gemini, new SignInfo("Ge", "Dual", "Airy", "Male") },
                { ZodiacName.Cancer, new SignInfo("Cn", "Movable/Chara/Mutable", "Water", "Female") },
                // ... other zodiac data
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

            // Fill rows with properties.
            table.Rows.Add("Abbreviations", Properties.Abbreviations);
            table.Rows.Add("Nature", Properties.Nature);
            table.Rows.Add("Element", Properties.Element);
            table.Rows.Add("Gender", Properties.Gender);

            return table;
        }

        public JObject ToJson()
        {
            var returnVal = new JObject
            {
                ["Abbreviations"] = Properties.Abbreviations,
                ["Nature"] = Properties.Nature,
                ["Element"] = Properties.Element,
                ["Gender"] = Properties.Gender
            };

            return returnVal;
        }
    }
}