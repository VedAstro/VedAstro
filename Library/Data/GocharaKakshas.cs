using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

namespace VedAstro.Library
{
    /// <summary>
    /// Data representation of Gochara Kakshas
    /// </summary>
    public class GocharaKakshas : IToJson, IToJpeg, IToDataTable
    {
        public GocharaKakshas(List<PlanetName> column1, Dictionary<PlanetName, ZodiacSign> column2, Dictionary<PlanetName, string> column3, Dictionary<PlanetName, int> column4, Dictionary<PlanetName, int> column5, Dictionary<PlanetName, int> column6)
        {
            Column1 = column1;
            Column2 = column2;
            Column3 = column3;
            Column4 = column4;
            Column5 = column5;
            Column6 = column6;
        }

        public List<PlanetName> Column1 { get; }
        public Dictionary<PlanetName, ZodiacSign> Column2 { get; }
        public Dictionary<PlanetName, string> Column3 { get; }
        public Dictionary<PlanetName, int> Column4 { get; }
        public Dictionary<PlanetName, int> Column5 { get; }
        public Dictionary<PlanetName, int> Column6 { get; }

        public JObject ToJson()
        {
            //put together final table for caller
            var holder = new JObject();
            foreach (var mainPlanet in Column1)
            {
                //package the row
                var valueHolder = new JObject
                {
                    //make the columns
                    ["Sign"] = Column2[mainPlanet].GetSignName().ToString(),//current sign
                    ["KakshaScore"] = Column4[mainPlanet],
                    ["KakshaLord"] = Column3[mainPlanet],
                    ["Ashtaka"] = Column5[mainPlanet],
                    ["Sarvashtaka"] = Column6[mainPlanet],
                };

                holder[mainPlanet.Name.ToString()] = valueHolder;
            }

            return holder;

        }

        public byte[] ToJpeg()
        {
            //convert current instance to a table format
            var table = this.ToDataTable();

            // Create a new Bitmap object
            Bitmap bitmap = new Bitmap(table.Columns.Count * 60, table.Rows.Count * 20);
            Graphics g = Graphics.FromImage(bitmap);

            // Draw the table
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    g.FillRectangle(Brushes.White, new Rectangle(j * 60, i * 20, 60, 20));
                    g.DrawRectangle(Pens.Black, new Rectangle(j * 60, i * 20, 60, 20));
                    g.DrawString(table.Rows[i][j].ToString(), new Font("Arial", 10), Brushes.Black, new PointF(j * 60, i * 20));
                }
            }

            // Convert the image to a byte array
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }

        }

        public DataTable ToDataTable()
        {
            // Create a new DataTable.
            DataTable table = new DataTable("Table1");

            // Define columns.
            table.Columns.Add("Planet", typeof(int));
            table.Columns.Add("Sign", typeof(string));
            table.Columns.Add("KakshaScore", typeof(string));
            table.Columns.Add("KakshaLord", typeof(string));
            table.Columns.Add("Ashtaka", typeof(string));
            table.Columns.Add("Sarvashtaka", typeof(string));

            // Populate the table with sample data.
            foreach (var mainPlanet in Column1)
            {
                table.Rows.Add(mainPlanet.Name,
                    Column2[mainPlanet].GetSignName().ToString(),
                    Column4[mainPlanet],
                    Column3[mainPlanet],
                    Column5[mainPlanet],
                    Column6[mainPlanet]);
            }

            return table;
        }
    }

}
