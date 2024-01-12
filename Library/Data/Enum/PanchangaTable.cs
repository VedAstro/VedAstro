using Newtonsoft.Json.Linq;
using System;
using System.Data;

namespace VedAstro.Library
{

    /// <summary>
    /// Data package for holding Panchanga Table as data
    /// </summary>
    public class PanchangaTable(LunarDay tithi, DayOfWeek weekDay, Constellation constellation, NithyaYoga yoga, Karana karana, string dishaShool) :IToJpeg, IToJson, IToDataTable
    {
        public byte[] ToJpeg() { var table = this.ToDataTable(); return Tools.DataTableToJpeg(table); }

        public DataTable ToDataTable()
        {
            // Create a new DataTable.
            DataTable table = new DataTable("PanchangaTable");

            // Define columns.
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(string));

            // fill rows
            table.Rows.Add("Tithi", tithi);
            table.Rows.Add("Vara", weekDay);
            table.Rows.Add("Nakshatra", constellation);
            table.Rows.Add("Yoga", yoga);
            table.Rows.Add("Karana", karana);
            table.Rows.Add("DishaShool", dishaShool);


            return table;
        }

        public JObject ToJson()
        {
            var returnVal = new JObject();
            returnVal["Tithi"] = tithi.ToJson();
            returnVal["Vara"] = weekDay.ToString();
            returnVal["Nakshatra"] = constellation.ToString();
            returnVal["Yoga"] = yoga.ToJson();
            returnVal["Karana"] = karana.ToString();
            returnVal["DishaShool"] = dishaShool.ToString();

            return returnVal;

        }
    }
}