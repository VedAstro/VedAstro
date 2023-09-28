using System.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple data type to enclose the data coming out of a calculator.
    /// Prediction = did it occur & the strength 
    /// </summary>
    public class CalculatorResult
    {
        /// <summary>
        /// Indication if prediction is occurring
        /// </summary>
        public bool Occuring { get; set; }



        /// <summary>
        /// if specified overrides event nature from XML file
        /// will default to Empty when not set
        /// note: implemented to allow calculator method to modify final event nature
        /// </summary>
        public EventNature NatureOverride { get; set; }

        /// <summary>
        /// if specified overrides event description text from XML file
        /// will default to Empty when not set
        /// note: implemented to allow calculator method to modify final event nature
        /// </summary>
        public string DescriptionOverride { get; set; } = ""; //empty to detect if set

        ///// <summary>
        ///// Defaults set here
        ///// </summary>
        //public CalculatorResult()
        //{
        //    Info = "";
        //}

        /// <summary>
        /// Return an Not Occuring Prediction
        /// </summary>
        public static CalculatorResult NotOccuring()
        {
            var prediction = new CalculatorResult()
            {
                Occuring = false
            };

            return prediction;
        }
        /// <summary>
        /// Return an Occuring Prediction
        /// </summary>
        public static CalculatorResult IsOccuring()
        {
            var prediction = new CalculatorResult()
            {
                Occuring = true
                
            };

            return prediction;
        }

        
        public RelatedBody RelatedBody { get; set; } = new RelatedBody();


        //-------------------------------------------------------
        //BELOW IS A LIST OF SPECIAL HELPER METHODS TO RECEIVE
        //CALCULATOR RESULTS FOR MANY TYPES OF CALCULATION
        //-------------------------------------------------------

        /// <summary>
        /// Helper method to make new instance of calculator result
        /// </summary>
        public static CalculatorResult New(bool occuring)
        {
            var newCalcResult = new CalculatorResult{ Occuring = occuring };
            return newCalcResult;
        }


        /// <summary>
        /// Helper method to make new instance of calculator result
        /// </summary>
        public static CalculatorResult New(bool occuring, HouseName[] houseNames, Time time)
        {
            var newCalcResult = new CalculatorResult();
            newCalcResult.Occuring = occuring;
            newCalcResult.RelatedBody.RelatedHouses.AddRange(houseNames.ToList());
            var lordNames = Calculate.LordOfHouseList(newCalcResult.RelatedBody.RelatedHouses, time);
            newCalcResult.RelatedBody.RelatedPlanets.AddRange(lordNames.ToList());

            return newCalcResult;
        }

        /// <summary>
        /// Helper method to make new instance of calculator result
        /// </summary>
        public static CalculatorResult New(bool occuring, PlanetName lord)
        {
            var newCalcResult = new CalculatorResult();
            newCalcResult.Occuring = occuring;
            newCalcResult.RelatedBody.RelatedPlanets.Add(lord);
            return newCalcResult;
        }


        public static CalculatorResult New(bool occuring, PlanetName[] planetNames, Time birthTime)
        {
            var newCalcResult = new CalculatorResult();
            newCalcResult.Occuring = occuring;
            newCalcResult.RelatedBody.RelatedPlanets.AddRange(planetNames.ToList());

            return newCalcResult;
        }

        /// <summary>
        /// Helper method to make new instance of calculator result
        /// </summary>
        public static CalculatorResult New(bool occuring, HouseName[] houseNames, ZodiacName[] signNames, Time time)
        {
            //create with house names
            var newCalcResult = New(occuring, houseNames,time);

            //add in sign names
            newCalcResult.RelatedBody.RelatedZodiac.AddRange(signNames.ToList());

            //return to caller
            return newCalcResult;
        }

        public static CalculatorResult New(bool occuring, HouseName[] houseNames, PlanetName[] planetNames, ZodiacName[] signNames, Time time)
        {
            //create with house names
            var newCalcResult = New(occuring, houseNames, planetNames, time);

            //add in sign names
            newCalcResult.RelatedBody.RelatedZodiac.AddRange(signNames.ToList());

            //return to caller
            return newCalcResult;

        }

        public static CalculatorResult New(bool occuring, PlanetName[] planetNames, ZodiacName[] signNames, Time timeInput)
        {
            //create with house names
            var newCalcResult = New(occuring, planetNames, timeInput);

            //add in sign names
            newCalcResult.RelatedBody.RelatedZodiac.AddRange(signNames.ToList());

            //return to caller
            return newCalcResult;

        }


        /// <summary>
        /// Helper method to make new instance of calculator result
        /// </summary>
        public static CalculatorResult New(bool occuring, HouseName[] houseNames, PlanetName[] planetNames, Time time)
        {
            //create with house names
            var newCalcResult = New(occuring, houseNames,time);

            //todo can possibly add lord of houses into planet list

            //add in planet Names
            newCalcResult.RelatedBody.RelatedPlanets.AddRange(planetNames.ToList());

            //return to caller
            return newCalcResult;
        }

        /// <summary>
        /// Helper method to make new instance of calculator result
        /// </summary>
        public static CalculatorResult New(bool occuring, HouseName houseNumber, Time time)
        {
            var newCalcResult = new CalculatorResult();
            newCalcResult.Occuring = occuring;
            newCalcResult.RelatedBody.RelatedHouses.Add(houseNumber);
            var lord = Calculate.LordOfHouse(houseNumber, time);
            newCalcResult.RelatedBody.RelatedPlanets.Add(lord);
            return newCalcResult;
        }

    }
}
