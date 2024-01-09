using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;

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

        /// <summary>
        /// AI written code 😁
        /// </summary>
        public byte[] ToJpeg()
        {
            //convert current instance to a table format
            var table = this.ToDataTable();

            // Create a new Bitmap object
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);

            // Calculate the maximum width for each column
            int[] columnWidths = new int[table.Columns.Count];
            for (int j = 0; j < table.Columns.Count; j++)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var font = i == 0 ? new Font("Arial", 10, FontStyle.Bold) : new Font("Arial", 10);
                    var textSize = g.MeasureString(table.Rows[i][j].ToString(), font);
                    columnWidths[j] = Math.Max(columnWidths[j], (int)textSize.Width);
                }
            }

            // Dispose of the initial Graphics object and create a new one with the correct dimensions
            g.Dispose();
            int totalWidth = columnWidths.Sum() + table.Columns.Count * 2; // Add padding
            bitmap = new Bitmap(totalWidth, table.Rows.Count * 20);
            g = Graphics.FromImage(bitmap);

            // Draw the table
            int currentX = 0;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    g.FillRectangle(Brushes.White, new Rectangle(currentX, i * 20, columnWidths[j] + 2, 20)); // Add padding
                    g.DrawRectangle(Pens.Black, new Rectangle(currentX, i * 20, columnWidths[j] + 2, 20)); // Add padding

                    // Use a bold font for the first row (header)
                    var font = i == 0 ? new Font("Arial", 10, FontStyle.Bold) : new Font("Arial", 10);

                    // Add padding
                    var leftPadding = 3;
                    var topPadding = 1;
                    g.DrawString(table.Rows[i][j].ToString(), font, Brushes.Black, new PointF(currentX + leftPadding, i * 20 + topPadding));
                }
                currentX += columnWidths[j] + 2; // Move to next column position
            }

            // Convert the image to a byte array
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }


        public DataTable ToDataTable()
        {
            // Create a new DataTable.
            DataTable table = new DataTable("Table1");

            // Define columns.
            table.Columns.Add("Planet", typeof(string));
            table.Columns.Add("Sign", typeof(string));
            table.Columns.Add("KakshaScore", typeof(string));
            table.Columns.Add("KakshaLord", typeof(string));
            table.Columns.Add("Ashtaka", typeof(string));
            table.Columns.Add("Sarvashtaka", typeof(string));

            //first row is column headers
            table.Rows.Add("Planet",
                "Sign",
                "KakshaScore",
                "KakshaLord",
                "Ashtaka",
                "Sarvashtaka");

            //fill table with data in rows
            foreach (var mainPlanet in Column1)
            {
                table.Rows.Add(mainPlanet.Name.ToString(),
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
