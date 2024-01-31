using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    /// <summary>
    /// Special set of functions to calculate events chart summary row
    /// See as filters for data from events, to highlight and dim the presented score
    /// </summary>
    public static class Algorithm
    {
        /// <summary>
        /// the amount of score to minus/add when weak/strong planet
        /// </summary>
        private static double _scoreStepSize = 1;

        /// <summary>
        /// Gets all algorithm methods in nice string for selection in Website
        /// DYNAMIC!
        /// </summary>
        public static MethodInfo[] All => typeof(Algorithm)
            .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
            .ToArray();

        /// <summary>
        /// Disables all auto coloring, for use with Highlight function. 💡✝️
        /// </summary>
        public static double Neutral(Event foundEvent, Person person) => 0;

        public static double General(Event foundEvent, Person person)
        {
            //score from general nature of event
            var generalScore = 0.0;
            switch (foundEvent?.Nature)
            {
                case EventNature.Good:
                    generalScore = 3;
                    break;
                case EventNature.Bad:
                    generalScore = -3;
                    break;
            }

            return generalScore;
        }

        /// <summary>
        /// Adds ashtakvarga bindu to only gochara events
        /// can give a varied score from +3 to -3
        /// </summary>
        public static double GocharaAshtakvargaBindu(Event foundEvent, Person person)
        {
            //TODO NOTE: SUSPECT INVALID OUTPUT DATA NEEDS VALIDATION

            //if not gochara event, end here with 0/Neutral score
            if (!foundEvent.Name.ToString().Contains("Gochara")) { return 0; }

            //get gochara house number and planet from name of event
            var gocharaHouse = foundEvent.GetRelatedHouse()[0];
            var gocharaPlanet = foundEvent.GetRelatedPlanet()[0];

            //no bindu for rahu & ketu, so default to 0/neutral
            if (gocharaPlanet.Name is PlanetName.PlanetNameEnum.Rahu or PlanetName.PlanetNameEnum.Ketu) { return 0; }

            //NOTE: Below we mix radical horoscope with now time = future prediction/muhurtha
            //get ashtakvarga bindu points to predict good/bad nature of ongoing gochara (percentage possible)
            //note here "Start Time" should be fine, since all throughout the event the house sign will be same as start
            //TODO NOT SURE WHICH sign to use
            //var houseSign = Calculate.PlanetZodiacSign(gocharaPlanet, foundEvent.StartTime);
            var houseSign = Calculate.HouseSignName(gocharaHouse, foundEvent.StartTime); //time here is current time, not birth
                                                                                         //here is birth time because ashtakvarga is based on birth
            var binduPoints = Calculate.PlanetAshtakvargaBindu(gocharaPlanet, houseSign, person.BirthTime);//here is birth


            //if bindu is below 3 and below bad
            if (binduPoints == 0) { return -3; }
            if (binduPoints == 1) { return -2; }
            if (binduPoints is >= 2 and <= 3) { return -1; }

            //if 4 and above is good
            if (binduPoints is >= 4 and <= 5) { return 1; }
            if (binduPoints is >= 6 and <= 7) { return 2; }
            if (binduPoints == 8) { return 3; }

            //end of line
            throw new Exception("Not meant to hit here");
        }

        /// <summary>
        /// if strongest planet, gets an extra point
        /// </summary>
        public static double StrongestPlanet(Event foundEvent, Person person)
        {
            //get top planet
            var allPlanetOrderedByStrength = Calculate.AllPlanetOrderedByStrength(person.BirthTime);
            var topPlanet = allPlanetOrderedByStrength[0];
            var top2ndPlanet = allPlanetOrderedByStrength[1];

            //get all planets in event, scan and give score
            var planetNatureScore = 0.0;
            foreach (var relatedPlanet in foundEvent.GetRelatedPlanet())
            {
                //is planet top planet
                var isTopPlanet = relatedPlanet == topPlanet ||
                                  relatedPlanet == top2ndPlanet;

                //if top planet than give score
                if (isTopPlanet)
                {
                    planetNatureScore += _scoreStepSize;
                }
            }

            return planetNatureScore;
        }

        public static double WeakestPlanet(Event foundEvent, Person person)
        {
            //get bottom planet
            var allPlanetOrderedByStrength = Calculate.AllPlanetOrderedByStrength(person.BirthTime);
            var bottomPlanet = allPlanetOrderedByStrength[8];
            var bottom2ndPlanet = allPlanetOrderedByStrength[7];

            //get all planets in event, scan and give score
            var planetNatureScore = 0.0;
            foreach (var relatedPlanet in foundEvent.GetRelatedPlanet())
            {
                //is planet bottom planet
                var isBottomPlanet = relatedPlanet == bottomPlanet ||
                                     relatedPlanet == bottom2ndPlanet;

                //if bottom planet than give score
                if (isBottomPlanet)
                {
                    planetNatureScore += -_scoreStepSize;
                }
            }

            return planetNatureScore;
        }

        public static double StrongestHouse(Event foundEvent, Person person)
        {
            //get top house
            var topHouse = Calculate.AllHousesOrderedByStrength(person.BirthTime)[0];

            //get all houses in event, scan and give score
            var houseNatureScore = 0.0;
            foreach (var relatedHouse in foundEvent.GetRelatedHouse())
            {
                //is house top house
                var isTopHouse = relatedHouse == topHouse;

                //if top house than give score
                if (isTopHouse)
                {
                    houseNatureScore += _scoreStepSize;
                }
            }

            return houseNatureScore;
        }

        public static double WeakestHouse(Event foundEvent, Person person)
        {
            //get bottom house
            var bottomHouse = Calculate.AllHousesOrderedByStrength(person.BirthTime)[11];

            //get all houses in event, scan and give score
            var houseNatureScore = 0.0;
            var relatedHouses = foundEvent.GetRelatedHouse();
            foreach (var relatedHouse in relatedHouses)
            {
                //is house bottom house
                var isBottomHouse = relatedHouse == bottomHouse;

                //if bottom house than give score
                if (isBottomHouse)
                {
                    houseNatureScore += -_scoreStepSize;
                }
            }

            return houseNatureScore;
        }

        /// <summary>
        /// If all planets bad, negative step size
        /// </summary>
        public static double CombinedBad(Event foundEvent, Person person)
        {
            //all planets in event is bad
            var planetList = foundEvent.GetRelatedPlanet();
            bool isAllPlanetBad = planetList.All(pln => Calculate.IsPlanetStrongInShadbala(pln, person.BirthTime) == false);

            //if all not bad, end here  
            if (!isAllPlanetBad) { return 0; }

            //if there is a house then all must be bad also
            var houseList = foundEvent.GetRelatedHouse();
            bool isAllHouseBad = houseList.All(hse => Calculate.IsHouseBeneficInShadbala(hse, person.BirthTime, 450) == false);

            //if all not bad, end here  
            if (!isAllHouseBad) { return 0; }

            //if control reaches here than all is bad
            return -_scoreStepSize;

        }

        /// <summary>
        /// only dasa events get good bad score based on ishata and kashata, bala book pg 110
        /// </summary>
        public static double IshtaKashtaPhala(Event foundEvent, Person person)
        {
            //must be a dasa event, has PD in event name
            var isDasaEvent = foundEvent.Name.ToString().Contains("PD");
            if (!isDasaEvent) { return 0; }

            //get the strongest planet of person's birth found in Event
            //NOTE: refer pg.110 Graha & Bhava bala, how planets trump each other
            var relatedPlanets = foundEvent.GetRelatedPlanet();
            var strongestPlanet = Calculate.PickOutStrongestPlanet(relatedPlanets, person.BirthTime);

            //get good or bad based on Ishta and Kashta, if former is more than good
            var score = Calculate.PlanetIshtaKashtaScore(strongestPlanet, person.BirthTime);

            //-1 bad, +1 good, no neutral
            return score;
        }

        public static double IshtaKashtaPhalaDegree(Event foundEvent, Person person)
        {
            //must be a dasa event, has PD in event name
            var isDasaEvent = foundEvent.Name.ToString().Contains("PD");
            if (!isDasaEvent) { return 0; }

            //get the strongest planet of person's birth found in Event
            //NOTE: refer pg.110 Graha & Bhava bala, how planets trump each other
            var relatedPlanets = foundEvent.GetRelatedPlanet();
            var strongestPlanet = Calculate.PickOutStrongestPlanet(relatedPlanets, person.BirthTime);

            //get good or bad based on Ishta and Kashta, if former is more than good
            var score = Calculate.PlanetIshtaKashtaScoreDegree(strongestPlanet, person.BirthTime);

            return score;
        }

        public static double PlanetStrengthDegree(Event foundEvent, Person person)
        {
            //get all planets in event, scan and give score
            var planetNatureScore = 0.0;
            foreach (var relatedPlanet in foundEvent.GetRelatedPlanet())
            {
                var rawScore = Calculate.PlanetPowerPercentage(relatedPlanet, person.BirthTime);

                //if below 50% than starts to turn RED
                var finalScore = 0.0;
                if (rawScore > 50)
                {
                    //remap the
                    finalScore = rawScore.Remap(fromMin: 0, fromMax: 100, toMin: 0, toMax: 1);
                }
                else
                {
                    //remap the
                    finalScore = rawScore.Remap(fromMin: 0, fromMax: 100, toMin: -1, toMax: 0);
                }

                planetNatureScore += finalScore;

            }

            return planetNatureScore;
        }

    }
}
