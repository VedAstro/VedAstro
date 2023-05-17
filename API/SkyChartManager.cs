using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// Logic to create Sky Chart, simple chart with zodiac and planets in it
    /// </summary>
    public static class SkyChartManager
    {
        /// <summary>
        /// Sweet heart takes this away!
        /// </summary>
        public static string GenerateChart(Time time)
        {
            //PART I : declare the components
            string svgHead = null;
            string svgTail = null;
            string border = null;
            string contentTail = null;



            //PART II : fill the components in order

            GenerateComponents();



            //PART III : compile in right placement
            var final =
                $@" <!--MADE BY MACHINES FOR HUMAN EYES-->
                    {svgHead}
                        <circle cx=""50"" cy=""50"" r=""40"" stroke=""black"" stroke-width=""3"" fill=""red"" />
                    {svgTail}
                ";


            return final;




            //------------------------LOCALS NEEDED FOR REFS

            void GenerateComponents()
            {
                //STEP 1: USER INPUT > USABLE DATA
                var svgBackgroundColor = "#f0f9ff"; //not bleach white
                var randomId = Tools.GenerateId();


                //note: if width & height not hard set, parent div clips it
                var svgTotalHeight = 350;//todo for now hard set, future use: verticalYAxis;
                var svgTotalWidth = 360;//todo for now hard set, future use: verticalYAxis;
                var svgStyle = $@"width:{svgTotalWidth}px;height:{svgTotalHeight}px;background:{svgBackgroundColor};";//end of style tag
                svgHead = $"<svg class=\"EventChartHolder\" id=\"{randomId}\" style=\"{svgStyle}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">";//much needed for use tags to work

                svgTail = "</svg>";
                contentTail = "</g>";


            }

        }


        //----------PRIVATE



    }
}
