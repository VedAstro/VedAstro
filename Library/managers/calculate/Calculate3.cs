
//< !--Mona Lisa by Leonardo da Vinci

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!>''''''<!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!'''''`             ``'!!!!!!!!!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!!!!!!!!''`          .....         `'!!!!!!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!!!!!'`      .      :::::'            `'!!!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!!!'     .   '     .::::'                `!!!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!!'      :          `````                   `!!!!!!!!!!!!!!
//!!!!!!!!!!!!!!!!        ., cchcccccc,,.                       `!!!!!!!!!!!!
//!!!!!!!!!!!!!!!     .-"?$$$$$$$$$$$$$$c,                      `!!!!!!!!!!!
//!!!!!!!!!!!!!!    , ccc$$$$$$$$$$$$$$$$$$$,                     `!!!!!!!!!!
//!!!!!!!!!!!!!    z$$$$$$$$$$$$$$$$$$$$$$$$;.                    `!!!!!!!!!
//!!!!!!!!!!!!    < $$$$$$$$$$$$$$$$$$$$$$$$$$:.                    `!!!!!!!!
//!!!!!!!!!!!$$$$$$$$$$$$$$$$$$$$$$$$$$$h;:.                   !!!!!!!!
//!!!!!!!!!!'     $$$$$$$$$$$$$$$$$$$$$$$$$$$$$h;.                   !!!!!!!
//!!!!!!!!!'     <$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$                   !!!!!!!
//!!!!!!!!'      `$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$F                   `!!!!!!
//!!!!!!!!        c$$$$???$$$$$$$P""  """??????"                      !!!!!!
//!!!!!!!         `"" .,.. "$$$$F    .,zcr                            !!!!!!
//!!!!!!!         .  dL    .?$$$   ., cc,      ., z$h.                  !!!!!!
//!!!!!!!!        <. $$c= <$d$$$   <$$$$=-=+"$$$$$$$                  !!!!!!
//!!!!!!!         d$$$hcccd$$$$$   d$$$hcccd$$$$$$$F                  `!!!!!
//!!!!!!         ,$$$$$$$$$$$$$$h d$$$$$$$$$$$$$$$$                   `!!!!!
//!!!!!          `$$$$$$$$$$$$$$$<$$$$$$$$$$$$$$$$'                    !!!!!
//!!!!!          `$$$$$$$$$$$$$$$$"$$$$$$$$$$$$$P>                     !!!!!
//!!!!!           ?$$$$$$$$$$$$??$c`$$$$$$$$$$$?>'                     `!!!!
//!!!!!           `?$$$$$$I7?""    ,$$$$$$$$$?>>'                       !!!!
//!!!!!.           <<?$$$$$$c.    , d$$?$$$$$F>>''                       `!!!
//!!!!!!            <i?$P"??$$r--"?""  ,$$$$h;> ''                       `!!!
//!!!!!!$$$hccccccccc = cc$$$$$$$>> '                         !!!
//!!!!!              `?$$$$$$F""""  `"$$$$$>>>''                         `!!
//!!!!!                "?$$$$$cccccc$$$$??>>>>'                           !!
//!!!!>                  "$$$$$$$$$$$$$F>>>>''                            `!
//!!!!!                    "$$$$$$$$???>'''                                !
//!!!!!>                     `"""""                                        `
//!!!!!!;                       .                                          `
//!!!!!!!                       ? h.
//!!!!!!!!                       $$c,
//!!!!!!!!>                      ?$$$h.              ., c
//!!!!!!!!!                       $$$$$$$$$hc,.,, cc$$$$$
//!!!!!!!!!                  ., zcc$$$$$$$$$$$$$$$$$$$$$$
//!!!!!!!!!               .z$$$$$$$$$$$$$$$$$$$$$$$$$$$$
//!!!!!!!!!             , d$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$          .
//!!!!!!!!!           , d$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$         !!
//!!!!!!!!!         , d$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$        ,!'
//!!!!!!!!>        c$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$.       !'
//!!!!!!''       , d$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$>       '
//!!!''         z$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$>
//!'           ,$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$>             ..
//            z$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$'           ;!!!!''`
//            $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$F       ,; ; !'`'.''
//           < $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$>    ,; '`'  ,;
//           `$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$F - '   ,;!!'
//            "?$$$$$$$$$$?$$$$$$$$$$$$$$$$$$$$$$$$$$F     .<!!!'''       <!
//         ! > "" ?? $$$? C3$$$$$$$$$$$$$$$$$$$$$$$$""     ;!'''          !!!
//       ; !!!!;,      `"''""????$$$$$$$$$$$$$$$$""   ,;-''               ',!
//      ; !!!!< !!!; .                `"""""""""""    `'                  ' '
//      !!!!; !!! ; !!!!>;,;, ..' .                   '  '
//     !!' ,;!!! ;'`!!!!!!!!; !!!!!;  .        > ' .''                 ;
//    !!' ;!!'!';! !! !!!!!!!!!!!!!  ' - '
//   < !!!! `!; ! `!' !!!!!!!!!!<!       .
//   `!  ; !  ; !!! < ' <!!!! `!!! <       /
//  `; !>  < !! ; '  !!!!'!!';!     ;'
//   !!!!!!   `!!!  ; !!!'  '
//  ;   `!  `!! ,'    !'; !'
//      '   /`! !    <     !! <      '
//           / ; !        >; ! ;>
//             !'       ; !! '
//          ' ;!        > ! '
//            '
//Allen Mullen-->


using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VedAstro.Library
{
    public static partial class Calculate
    {

        #region PLANET BENEFIC & MALEFIC CALCULATION

        /// <summary>
        /// Whenever an affiiction by way of a malefic occupying
        /// a certain house or joining with a certain planet
        /// is suggested, by implication an aspect is also meant,
        /// though an affliction caused by aspect.is comparatively less malevolent
        ///
        /// Note:
        /// TODO presently not 100% sure, if what is meant by "affliction" is solely only limited to
        /// aspects & conjunction with bad planets. Or
        /// -Located in enemy sign an affliction?
        /// -Low shadbala an affliction?
        /// -Low drikbala an affliction?
        ///
        /// 
        /// At present, malefic aspects & conjunctions are used
        /// becasue it seems based on texts that this is correct.
        /// 
        /// But it seems mercury in enemny sign or position in a house should also play a role.
        /// 
        /// There must be a corelation between shadbala or drikbala to aspects & conjucntion
        /// A more precise way of mesurement it could be via the bala method.
        /// Needs testing for sure, to find out what bala values determine an afflicted mercury
        ///
        /// </summary>
        /// TODO POSSIBLE RENAME TO is IsMercuryMalefic
        public static bool IsMercuryAfflicted(Time time)
        {
            //for now only malefic conjunctions are considered
            return IsMercuryMalefic(time);

        }

        /// <summary>
        /// Check if Mercury is malefic (true), returns false if benefic 
        /// 
        ///
        /// References:
        /// 
        /// "Mercury, by nature, is called sournya or good. And if he is in
        /// conjunction with the Sun, Saturn, Mars, Rahu or Ketu, he will
        /// be a malefic. His conjunction with beneficial planets like Full
        /// Moon, Jupiter or Venus will classify him as a benefic. Benefic
        /// means a good and malefic means an evil planet."
        /// -TODO Does malefic moon make it malefic? (atm malefic moon makes it malefic)
        ///
        /// "Though in the earlier pages Mercury is defined either as a subba
        /// (benefic) or papa (malefic) according to its association is with a benefic or
        /// malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
        /// be deemed as a full benefic. This is in accord with the injunctions of
        /// classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).
        /// "
        ///11. Benefics and Malefics. Among these, Sūrya, Śani, Mangal, decreasing Candr, Rahu and
        /// Ketu (the ascending and the descending nodes of Candr) are malefics, while the rest are
        /// benefics. Budh, however, is a malefic, if he joins a malefic. 
        /// 
        /// Note:
        /// ATM malefic planets override benefic
        /// TODO not sure if malefic planet overrides benefic if both are conjunct
        /// </summary>
        public static bool IsMercuryMalefic(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsMercuryMalefic), time), _isMercuryMalefic);


            //UNDERLYING FUNCTION
            bool _isMercuryMalefic()
            {
                //if mercury is already with malefics,then not checking if conjunct with benefic (not 100% sure)
                if (conjunctWithMalefic()) { return true; }

                //if conjunct with benefic, then it is benefic
                if (conjunctWithBenefic()) { return false; }

				//if not conjunct with any planet, should be benefic
				//NOTE : Mercury, by nature, is called sournya or good.
				return false; // false means not malefic


                //------------FUNCTIONS-------------

                bool conjunctWithMalefic()
                {
                    //list the planets that will make mercury malefic
                    var evilPlanetNameList = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Saturn, Library.PlanetName.Mars, Library.PlanetName.Rahu, Library.PlanetName.Ketu };

                    //if moon is malefic, add to malefic list
                    if (!IsMoonBenefic(time)) { evilPlanetNameList.Add(Library.PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = Calculate.PlanetsInConjuction(time, Library.PlanetName.Mercury);

                    //mark evil planet not in conjunct at first
                    bool evilPlanetFoundInConjunct = false;

                    //check if evil planets are in conjunct
                    foreach (var planetName in evilPlanetNameList)
                    {
                        evilPlanetFoundInConjunct = planetsConjunct.Contains(planetName);

                        //if one evil planet is found, break loop, stop looking
                        if (evilPlanetFoundInConjunct) { break; }
                    }

                    //return flag of evil planets found in conjunct
                    return evilPlanetFoundInConjunct;

                }

                bool conjunctWithBenefic()
                {
                    var beneficPlanetNameList = new List<PlanetName>() { Library.PlanetName.Jupiter, Library.PlanetName.Venus };

                    //if moon is benefic, add to benefic list
                    if (IsMoonBenefic(time)) { beneficPlanetNameList.Add(Library.PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = Calculate.PlanetsInConjuction(time, Library.PlanetName.Mercury);

                    //mark benefic planet not in conjunct at first
                    bool beneficPlanetFoundInConjunct = false;

                    //check if benefic planets are in conjunct
                    foreach (var planetName in beneficPlanetNameList)
                    {
                        beneficPlanetFoundInConjunct = planetsConjunct.Contains(planetName);

                        //if one good planet is found, break loop, stop looking
                        if (beneficPlanetFoundInConjunct) { break; }
                    }

                    //return flag of benefic planets found in conjunct
                    return beneficPlanetFoundInConjunct;

                }

            }


        }

        /// <summary>
        /// Moon is a benefic from the 8th day of the bright half of the lunar month
        /// to the 8th day of the dark half of the lunar month
        /// and a malefic in the rest of the days.
        /// 
        /// Returns true if benefic & false if malefic
        /// </summary>
        public static bool IsMoonBenefic(Time time)
        {
            //get the lunar date number
            int lunarDateNumber = Calculate.LunarDay(time).GetLunarDateNumber();

            //8th day of the bright half = 8th lunar date
            //8th day of the dark half  = 23rd lunar date
            if (lunarDateNumber >= 8 && lunarDateNumber <= 23)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [API("Benefic")]
        public static bool IsPlanetBenefic(PlanetName planetName, Time time)
        {
            //get list of benefic planets
            var beneficPlanetList = BeneficPlanetList(time);

            //check if input planet is in the list
            var planetIsBenefic = beneficPlanetList.Contains(planetName);

            return planetIsBenefic;
        }

        /// <summary>
        /// Benefics, on the other hand, tend to do good ; but
        /// sometimes they also become capable of doing harm.
        /// </summary>
        [API("tend to do good", Category.StarsAboveMe)]
        public static List<PlanetName> BeneficPlanetList(Time time)
        {
            //Add permanent good planets to list first
            var listOfGoodPlanets = new List<PlanetName>() { Library.PlanetName.Jupiter, Library.PlanetName.Venus };

            //check if moon is benefic
            var moonIsBenefic = IsMoonBenefic(time);

            //if moon is benefic add to benefic list
            if (moonIsBenefic) { listOfGoodPlanets.Add(Library.PlanetName.Moon); }

            //check if mercury is good
            var mercuryIsBenefic = IsMercuryMalefic(time) == false;

            //if mercury is benefic add to benefic list
            if (mercuryIsBenefic) { listOfGoodPlanets.Add(Library.PlanetName.Mercury); }

            return listOfGoodPlanets;
        }

        [API("Malefic")]
        public static bool IsPlanetMalefic(PlanetName planetName, Time time)
        {
            //get list of malefic planets
            var maleficPlanetList = MaleficPlanetList(time);

            //check if input planet is in the list
            var planetIsMalefic = maleficPlanetList.Contains(planetName);

            return planetIsMalefic;
        }

		/// <summary>
		/// Gets list of permanent malefic planets,
		/// for moon & mercury it is based on changing factors
		///
		/// Malefics are always inclined to do harm, but under certain
		/// conditions, the intensity of the mischief is tempered.
		/// </summary>
		[API("list of permanent malefic planets, for moon & mercury it is based on changing factors", Category.StarsAboveMe)]
        public static List<PlanetName> MaleficPlanetList(Time time)
        {
            //Add permanent evil planets to list first
            var listOfEvilPlanets = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Saturn, Library.PlanetName.Mars, Library.PlanetName.Rahu, Library.PlanetName.Ketu };

            //check if moon is evil
            var moonIsEvil = IsMoonBenefic(time) == false;

            //if moon is evil add to evil list
            if (moonIsEvil)
            {
                listOfEvilPlanets.Add(Library.PlanetName.Moon);
            }

            //check if mercury is evil
            var mercuryIsEvil = IsMercuryMalefic(time);
            //if mercury is evil add to evil list
            if (mercuryIsEvil)
            {
                listOfEvilPlanets.Add(Library.PlanetName.Mercury);
            }

            return listOfEvilPlanets;
        }

        /// <summary>
        /// Gets planet aspected by the inputed planet
        /// </summary>
        [API("Gets planet aspected by the inputed planet")]
        public static List<PlanetName> PlanetsInAspect(PlanetName planet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = Calculate.SignsPlanetIsAspecting(planet, time);

            //get all the planets located in these signs
            var planetsAspected = new List<PlanetName>();
            foreach (var zodiacSign in signAspecting)
            {
                var planetInSign = Calculate.PlanetInSign(zodiacSign, time);
                //add to list
                planetsAspected.AddRange(planetInSign);
            }


            //return these planets as aspected by input planet
            return planetsAspected;

        }

        /// <summary>
        /// Gets all planets the transmitting aspect to inputed planet
        /// </summary>
        [API(" Gets all planets the transmitting aspect to inputed planet")]
        public static List<PlanetName> PlanetsAspectingPlanet(Time time, PlanetName receivingAspect)
        {
            //check if all planets is aspecting inputed planet
            var aspectFound = Library.PlanetName.All9Planets.FindAll(transmitPlanet => IsPlanetAspectedByPlanet(receivingAspect, transmitPlanet, time));

            return aspectFound;

        }

        /// <summary>
        /// Gets houses aspected by the inputed planet
        /// </summary>
        [API("Gets houses aspected by the inputed planet")]
        public static List<HouseName> HousesInAspect(PlanetName planet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = Calculate.SignsPlanetIsAspecting(planet, time);

            //get all the houses located in these signs
            var housesAspected = new List<HouseName>();
            foreach (var house in Library.House.AllHouses)
            {
                //get sign of house
                var houseSign = Calculate.HouseSignName(house, time);

                //add house to list if sign is aspected by planet
                if (signAspecting.Contains(houseSign)) { housesAspected.Add(house); }
            }

            //return the houses aspected by input planet
            return housesAspected;

        }

        /// <summary>
        /// Gets all planets aspecting inputed house
        /// </summary>
        [API("PlanetsAspectingHouse")]
        public static List<PlanetName> PlanetsAspectingHouse(HouseName inputHouse, Time time)
        {
            //create empty list
            var returnPlanetList = new List<PlanetName>();

            //check each planet if aspecting house
            foreach (var planet in Library.PlanetName.All9Planets)
            {
                //get houses
                var housesInAspect = HousesInAspect(planet, time);

                //check if any house is a match
                var houseMatch = housesInAspect.FindAll(house => house == inputHouse).Any();
                if (houseMatch)
                {
                    returnPlanetList.Add(planet);
                }
            }


            return returnPlanetList;
        }

        /// <summary>
        /// Checks if the a planet is aspected by another planet
        /// </summary>
        public static bool IsPlanetAspectedByPlanet(PlanetName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get planets aspected by transmiting planet
            var planetsInAspect = Calculate.PlanetsInAspect(transmitingAspect, time);

            //if receiving planet is in list of currently aspected
            return planetsInAspect.Contains(receiveingAspect);

        }
        /// <summary>
        /// Checks if a house is aspected by a planet
        /// </summary>
        public static bool IsHouseAspectedByPlanet(HouseName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get houses aspected by transmiting planet
            var houseInAspect = Calculate.HousesInAspect(transmitingAspect, time);

            //if receiving house is in list of currently aspected
            return houseInAspect.Contains(receiveingAspect);

        }


        /// <summary>
        /// Checks if the a planet is conjunct with another planet
        ///
        /// Note:
        /// Both planets A & B are checked if they are in conjunct with each other,
        /// performance might be effected mildly, but errors in conjunction calculation would be caught here.
        /// Can be removed once, conjunction calculator is confirmed accurate.
        /// </summary>
        public static bool IsPlanetConjunctWithPlanet(PlanetName planetA, PlanetName planetB, Time time)
        {
            //get planets in conjunt for A & B
            var planetAConjunct = Calculate.PlanetsInConjuction(time, planetA);
            var planetBConjunct = Calculate.PlanetsInConjuction(time, planetB);

            //check that A conjuncts B and B conjuncts A 
            var conjunctFound = planetAConjunct.Contains(planetB) && planetBConjunct.Contains(planetA);

            //erro check, can be removed upon corectness confirmation
            if (planetAConjunct.Contains(planetB) != planetBConjunct.Contains(planetA))
            {
                throw new Exception("Conjunct planet not uniform!");
            }

            return conjunctFound;
        }


        #endregion


        #region PLANET AND HOUSE STRENGHT CALCULATORS

        /// <summary>
        /// Returns an array of all planets sorted by strenght,
        /// 0 index being strongest to 8 index being weakest
        ///
        /// Note:
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// </summary>
        public static List<PlanetName> AllPlanetOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllPlanetOrderedByStrength), time), _getAllPlanetOrderedByStrength);


            //UNDERLYING FUNCTION
            List<PlanetName> _getAllPlanetOrderedByStrength()
            {
                var planetStrenghtList = new Dictionary<PlanetName, double>();

                //create a list with planet names & its strength (unsorted)
                foreach (var planet in Library.PlanetName.All9Planets)
                {
                    //get planet strength in rupas
                    var strength = PlanetShadbalaPinda(planet, time).ToDouble();

                    //place in list with planet name
                    planetStrenghtList.Add(planet, strength);
                }


                //sort that list from strongest planet to weakest planet
                var sortedList = planetStrenghtList.OrderByDescending(item => item.Value);
                var nameOnlyList = sortedList.Select(x => x.Key).ToList();

                return nameOnlyList;

                /*--------------FUNCTIONS----------------*/
            }
        }


        /// <summary>
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// Powerful Planets.-Ravi is befd to be
        /// powerful when hi~Shadbala Pinda is 5 or more
        /// rupas. Chandra becomes strong when his Shadbala
        /// Pinda is 6 or more rupas. Kuja becomes powerful
        /// when bis Shadbala Pinda does not fall short of
        /// 5 rupas.Budha becomes potent by having his
        /// Sbadbala Pinda as 7 rupas; Guru, Sukra and Sani
        /// become thoroughly powerful if their Shadbala
        /// Pindas are 6.5, 5.5 and 5 rupas or more respectively.
        /// </summary>
        public static bool IsPlanetBeneficInShadbala(PlanetName planet, Time time)   
        {

            var limit = 0.0;
            if (planet == Library.PlanetName.Sun) { limit = 5; }
            else if (planet == Library.PlanetName.Moon) { limit = 6; }
            else if (planet == Library.PlanetName.Mars) { limit = 5; }
            else if (planet == Library.PlanetName.Mercury) { limit = 7; }
            else if (planet == Library.PlanetName.Jupiter) { limit = 6.5; }
            else if (planet == Library.PlanetName.Venus) { limit = 5.5; }
            else if (planet == Library.PlanetName.Saturn) { limit = 5; }
            //todo rahu and ketu added later on based on saturn and mars
            else if (planet == Library.PlanetName.Rahu) { limit = 5; }
            else if (planet == Library.PlanetName.Ketu) { limit = 5; }

            //divide strength by minimum limit of power (based on planet)
            //if above limit than benefic, else return false
            var shadbalaRupa = Calculate.PlanetShadbalaPinda(planet, time);
            var rupa = Math.Round(shadbalaRupa.ToRupa(),1);
            var strengthAfterLimit = rupa / limit;

            //if 1 or above is positive, below 1 is below limit
            var isBenefic = strengthAfterLimit >= 1.1;

            return isBenefic;
        }
        public static bool IsHouseBeneficInShadbala(HouseName house, Time birthTime, double threshold)
        {
            //get house strength
            var strength = HouseStrength(house, birthTime).ToDouble();

            //if above 450 then good
            var isBenefic = strength > threshold;
            return isBenefic;
        }

			
		/// <summary>
		/// Gets all planets strength (shadbala)
		/// </summary>
		[API("Gets  strength (shadbala) of all 9 planets")]
		public static List<(double, PlanetName)> AllPlanetStrength(Time time)
        {
            var planetStrenghtList = new List<(double, PlanetName)>();

            //create a list with planet names & its strength (unsorted)
            foreach (var planet in Library.PlanetName.All9Planets)
            {
                //get planet strength in rupas
                var strength = PlanetShadbalaPinda(planet, time).ToDouble();

                //place in list with planet name
                planetStrenghtList.Add((strength, planet));

            }

            return planetStrenghtList;
        }

        /// <summary>
        /// Returns an array of all houses sorted by strength,
        /// 0 index being strongest to 11 index being weakest
        /// </summary>
        public static HouseName[] GetAllHousesOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetAllHousesOrderedByStrength", time), _getAllHousesOrderedByStrength);


            //UNDERLYING FUNCTION
            HouseName[] _getAllHousesOrderedByStrength()
            {
                var houseStrengthList = new Dictionary<double, HouseName>();

                //create a list with planet names & its strength (unsorted)
                foreach (var house in Library.House.AllHouses)
                {
                    //get house strength
                    var strength = HouseStrength(house, time).ToRupa();

                    //place in list with house number
                    houseStrengthList[strength] = house;

                }


                //sort that list from strongest house to weakest house
                var keysSorted = houseStrengthList.Keys.ToList();
                keysSorted.Sort();

                var sortedArray = new HouseName[12];
                var count = 11;
                foreach (var key in keysSorted)
                {
                    sortedArray[count] = houseStrengthList[key];
                    count--;
                }

                return sortedArray;
            }

        }

        /// <summary>
        /// THE FINAL TOTAL STRENGTH
        /// Shadbala :the six sources of strength and weakness the planets
        /// The importance of and the part played by the Shadbalas,
        /// in the science of horoscopy, are manifold
        ///
        /// In order to obtain the total strength of
        /// the Shadbala Pinda of each planet, we have to add
        /// together its Sthana Bala, Dik Bala, Kala Bala.
        /// 'Chesta Bala and Naisargika Bala. And the Graha's
        /// Drik Bala must be added to or subtracted from the
        /// above sum according as it is positive or negative.
        /// The result obtained is the Shadbala Pinda of the
        /// planet in Shashtiamsas.
        ///
        /// Note: Rahu & Ketu supported, via house lord
        /// </summary>
        [API("Shadbala :the six sources of strength and weakness the planets")]
        public static Shashtiamsa PlanetShadbalaPinda(PlanetName planetName, Time time)
        {
            //return 0 if null planet
            if (planetName == null) { return Shashtiamsa.Zero; }

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetShadbalaPinda", planetName, time), _getPlanetShadbalaPinda);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetShadbalaPinda()
            {

                //if planet name is rahu or ketu then replace with house lord's strength
                if (planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu)
                {
                    var houseLord = Calculate.LordOfHousePlanetIsIn(time, planetName);
                    planetName = houseLord;
                }

                //Sthana bala (Positional Strength)
                var sthanaBala = PlanetSthanaBala(planetName, time);

                //Get dik bala (Directional Strength)
                var digBala = PlanetDigBala(planetName, time);

                //Get kala bala (Temporal Strength)
                var kalaBala = PlanetKalaBala(planetName, time);

                //Get Chesta bala (Motional Strength)
                var chestaBala = PlanetChestaBala(planetName, time);

                //Get Naisargika bala (Natural Strength)
                var naisargikaBala = PlanetNaisargikaBala(planetName, time);

                //Get Drik/drug bala (Aspect Strength)
                var drikBala = PlanetDrikBala(planetName, time);


                //Get total Shadbala Pinda
                var total = sthanaBala + digBala + kalaBala + chestaBala + naisargikaBala + drikBala;

                //round it 2 decimal places
                var roundedTotal = new Shashtiamsa(Math.Round(total.ToDouble(), 2));

                return roundedTotal;
            }

        }

        /// <summary>
        /// get total combined strength of the inputed planet
        /// input birth time to get strength in horoscope
        /// note: an alias method to GetPlanetShadbalaPinda ("strength" is easier to remember)
        /// </summary>
        [API("get total combined strength of the inputed planet")]
        public static Shashtiamsa PlanetStrength(PlanetName planetName, Time time) => PlanetShadbalaPinda(planetName, time);

		/// <summary>
		/// Gets the lord of the house the inputed planet is in
		/// </summary>
		[API("Gets the lord of the house the inputed planet is in")]
		private static PlanetName LordOfHousePlanetIsIn(Time time, PlanetName planetName)
        {
            var currentHouse = Calculate.HousePlanetIsIn(time, planetName);
            var houseLord = Calculate.LordOfHouse((HouseName)currentHouse, time);

            return houseLord;
        }

        /// <summary>
        /// Aspect strength
        ///
        /// This strength is gained by the virtue of the aspect
        /// (Graha Dristi) of different planets on other planet.
        /// The aspect of benefics is considered to be strength and
        /// the aspect of malefics is considered to be weaknesses.
        ///
        /// 
        /// Drik Bala.-This means aspect strength.
        /// The Drik Bala of a Gqaha is one-fourth of the
        /// Drishti Pinda on it. It is positive or negative
        /// according as the Drishti Pinda is positive or
        /// negative.
        ///
        /// 
        /// See the formula given on page 85. There is
        /// special aspect for Jupiter, ,Mars and Saturn on the
        /// 5th and 9th, 4th and 8th and 3rd and 10th signs
        /// respectively.
        /// </summary>
        [API("This means aspect strength. It is positive or negative according as the Drishti Pinda")]
        public static Shashtiamsa PlanetDrikBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }

            double dk;
            var drishti = new Dictionary<String, double>();
            double vdrishti;
            var sp = new Dictionary<PlanetName, int>();


            foreach (var p in Library.PlanetName.All7Planets)
            {
                if (Calculate.IsPlanetBenefic(p, time))
                {
                    sp[p] = 1;
                }
                else
                {
                    sp[p] = -1;
                }

            }


            foreach (var i in Library.PlanetName.All7Planets)
            {
                foreach (var j in Library.PlanetName.All7Planets)
                {
                    //Finding Drishti Kendra or Aspect Angle
                    var planetNirayanaLongitude = Calculate.PlanetNirayanaLongitude(time, j).TotalDegrees;
                    var nirayanaLongitude = Calculate.PlanetNirayanaLongitude(time, i).TotalDegrees;
                    dk = planetNirayanaLongitude - nirayanaLongitude;

                    if (dk < 0) { dk += 360; }

                    //get special aspect if any
                    vdrishti = FindViseshaDrishti(dk, i);

                    drishti[i.ToString() + j.ToString()] = FindDrishtiValue(dk) + vdrishti;

                }
            }

            double bala = 0;

            var DrikBala = new Dictionary<PlanetName, double>();

            foreach (var i in Library.PlanetName.All7Planets)
            {
                bala = 0;

                foreach (var j in Library.PlanetName.All7Planets)
                {
                    bala = bala + (sp[j] * drishti[j.ToString() + i.ToString()]);

                }

                DrikBala[i] = bala / 4;
            }



            return new Shashtiamsa(DrikBala[planetName]);



        }

        /// <summary>
        /// Get special aspect if any of Kuja, Guru and Sani
        /// </summary>
        public static double FindViseshaDrishti(double dk, PlanetName p)
        {
            double vdrishti = 0;

            if (p == Library.PlanetName.Saturn)
            {
                if (((dk >= 60) && (dk <= 90)) || ((dk >= 270) && (dk <= 300)))
                {
                    vdrishti = 45;
                }

            }
            else if (p == Library.PlanetName.Jupiter)
            {

                if (((dk >= 120) && (dk <= 150))
                    || ((dk >= 240) && (dk <= 270)))
                {
                    vdrishti = 30;
                }

            }
            else if (p == Library.PlanetName.Mars)
            {
                if (((dk >= 90) && (dk <= 120)) || ((dk >= 210) && (dk <= 240)))
                {
                    vdrishti = 15;
                }

            }
            else
            {
                vdrishti = 0;
            }


            return vdrishti;

        }

        public static double FindDrishtiValue(double dk)
        {

            double drishti = 0;

            if ((dk >= 30.0) && (dk <= 60))
            {
                drishti = (dk - 30) / 2;
            }
            else if ((dk > 60.0) && (dk <= 90))
            {
                drishti = (dk - 60) + 15;
            }
            else if ((dk > 90.0) && (dk <= 120))
            {
                drishti = ((120 - dk) / 2) + 30;
            }
            else if ((dk > 120.0) && (dk <= 150))
            {
                drishti = (150 - dk);
            }
            else if ((dk > 150.0) && (dk <= 180))
            {
                drishti = (dk - 150) * 2;
            }
            else if ((dk > 180.0) && (dk <= 300))
            {
                drishti = (300 - dk) / 2;
            }

            return drishti;

        }

        /// <summary>
        /// Nalsargika Bala.-This is the natural
        /// strength that each Graha possesses. The value
        /// assigned to each depends upon its luminosity.
        /// Ravi, the brightest of all planets, has the greatest
        /// Naisargika strength while Sani, the darkest, has
        /// the least Naisargika Bala.
        ///
        /// This is the natural or inherent strength of a planet.
        /// </summary>
        [API("This is the natural or inherent strength of a planet.")]
        public static Shashtiamsa PlanetNaisargikaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            if (planetName == Library.PlanetName.Sun) { return new Shashtiamsa(60); }
            else if (planetName == Library.PlanetName.Moon) { return new Shashtiamsa(51.43); }
            else if (planetName == Library.PlanetName.Venus) { return new Shashtiamsa(42.85); }
            else if (planetName == Library.PlanetName.Jupiter) { return new Shashtiamsa(34.28); }
            else if (planetName == Library.PlanetName.Mercury) { return new Shashtiamsa(25.70); }
            else if (planetName == Library.PlanetName.Mars) { return new Shashtiamsa(17.14); }
            else if (planetName == Library.PlanetName.Saturn) { return new Shashtiamsa(8.57); }

            throw new Exception("Planet not specified!");
        }

        /// <summary>
        /// NOTE: sun, moon get score for ISHTA/KESHA calculation TODO
        /// MOTIONAL STRENGTH
        /// Chesta here means Vakra Chesta or act of retrogression. Each planet, except the Sun and the Moon,
        /// and shadowy planets get into the state of Vakra or retrogression
        /// when its distance from the Sun exceeds a particular limit.
        /// And the strength or potency due to the planet on account of the arc of the retrogression is
        /// termed as Chesta Bala
        ///
        /// Deduct from the Seeghrocbcha, half the sum
        /// of the True and Mean Longitudes of planets and
        /// divide the difference by 3. The quotient is the
        /// Chestabala.
        /// Max 60, meaning Retrograde/Vakra
        /// When the distance of any one planet from
        /// the Sun exceeds a particular limit, it becomes
        /// retrograde, i.e., when the planet goes from
        /// perihelion (the point in a planet's orbit nearest
        /// to the Sun) to aphelion (the part of a planet's
        /// oroit most distant from the Sun) as it recedes
        /// from the Sun, it gradually loses the power
        /// of the Sun's gravitation and consequently, 
        /// </summary>
        [API("Chesta here means Vakra Chesta or act of retrogression. strength or potency termed as Chesta Bala")]
        public static Shashtiamsa PlanetChestaBala(PlanetName planetName, Time time, bool includeSunMoon = false)
        {
            //if include Sun/Moon specified, then use special function (used for Ishta/Kashta score)
            var isSunMoon = planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Moon;
            if (isSunMoon && includeSunMoon)
            {
                return SunMoonChestaBala(planetName);
            }

            //the Sun,Moon,Rahu and Ketu does not not retrograde, so 0 chesta bala
            if (isSunMoon || planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu) { return Shashtiamsa.Zero; }

            //get the interval between birth date and the date of the epoch (1900)
            //verified standard horoscope = 6862.579
            //NOTE: dates before 1900 give negative values
            var interval = EpochInterval(time);

            //get the mean/average longitudes of all planets
            var madhya = Madhya(interval, time);

            //get the apogee of all planets (apogee=earth, aphelion=sun)
            // aphelion (the part of a planet's orbit most distant from the Sun) 
            var seegh = GetSeeghrochcha(madhya, interval, time);


            //calculate chesta kendra, also called Seeghra kendra
            var planetLongitude = Calculate.PlanetNirayanaLongitude(time, planetName).TotalDegrees;
            //This is the Arc of retrogression.
            var planetAphelion = seegh[planetName]; //fixed most distant point from sun
            var planetMeanCircle = madhya[planetName]; //planet average distant point from sun (CIRCLE ORBIT)
            //Chesta kendra = 180 degrees = Retrograde
            //Because the orbits are elliptical
            //and not circular, equations are applied to the mean positions to get the true longitudes.
            var trueLongitude = ((planetMeanCircle + planetLongitude) / 2.0);
            //distance from stationary point, if less than 0 than add 360 
            var chestaKendra = (planetAphelion - trueLongitude);


            //If the Chesta kendra exceeds 180° (maximum retrograde), subtract it from 360, otherwise
            //keep it as it is. The remainder represents the reduced Chesta kendra
            //NOTE: done to reduce value of direct motion, only value relative to retro motion
            if (chestaKendra < 360.00)
            {
                chestaKendra = chestaKendra + 360;
            }
            chestaKendra = chestaKendra % 360;
            if (chestaKendra > 180.00) { chestaKendra = 360 - chestaKendra; }


            //The Chesta Bala is zero when the Chesta kendra is also zero. When it is
            //180° the Chesta Bala is 60 Shashtiamsas. In intermediate position, the
            //Bala is found by proportion (devide by 3)
            var chestaBala = (chestaKendra / 3.00);

            return new Shashtiamsa(chestaBala);



            //------------------------FUNCTIONS--------------


            //Seeghrochcha is the aphelion of the planet
            //It is required to find the Chesta kendra.
            //NOTE:aphelion (the part of a planet's orbit most distant from the Sun)
            Dictionary<PlanetName, double> GetSeeghrochcha(Dictionary<PlanetName, double> mean, double epochToBirthDays, Time time1)
            {
                int _birthYear = time1.GetLmtDateTimeOffset().Year;
                var seegh = new Dictionary<PlanetName, double>();
                double correction;

                //The mean longitude of the Sun will be the Seeghrochcha of Kuja, Guru and Sani.
                seegh[Library.PlanetName.Mars] = seegh[Library.PlanetName.Jupiter] = seegh[Library.PlanetName.Saturn] = mean[Library.PlanetName.Sun];

                correction = 6.670 + (0.00133 * (_birthYear - 1900));
                double changeDuringIntervalMercury = (epochToBirthDays * 4.092385);
                const double aphelionAtEpochMercury = 164.00; // The Seeghrochcha of Budha at the epoch
                double mercuryAphelion = changeDuringIntervalMercury < 0 ? aphelionAtEpochMercury - changeDuringIntervalMercury : aphelionAtEpochMercury + changeDuringIntervalMercury;
                mercuryAphelion -= correction; //further correction of +6.670-0133
                seegh[Library.PlanetName.Mercury] = (mercuryAphelion + correction) % 360;

                correction = 5 + (0.0001 * (_birthYear - 1900));
                double changeDuringIntervalVenus = (epochToBirthDays * 1.602159);
                const double aphelionAtEpochVenus = 328.51; // The Seeghrochcha of Budha at the epoch
                double venusAphelion = changeDuringIntervalVenus < 0 ? aphelionAtEpochVenus - changeDuringIntervalVenus : aphelionAtEpochVenus + changeDuringIntervalVenus;
                venusAphelion -= correction; //diminish the sum by 5 + 0.001*t (where t = birth year - 1900)
                seegh[Library.PlanetName.Venus] = venusAphelion % 360;

                return seegh;
            }

        }


		/// <summary>
		/// special function to get chesta score for Ishta/Kashta score
		/// Bala book pg. 108
		/// </summary>
		[API("special function to get chesta score for Ishta/Kashta score")]
        public static Shashtiamsa SunMoonChestaBala(PlanetName planetName)
        {
            return Shashtiamsa.Zero;
            throw new NotImplementedException();
        }

		/// <summary>
		/// The mean position of a planet is the position which it would have attained at a uniform
		/// rate of motion and the corrections to be applied in respect of the eccentricity of the orbit are not considered
		/// </summary>
		[API("mean position of a planet it would have attained at a uniform rate of motion, eccentricity of the orbit are not considered")]
		public static Dictionary<PlanetName, double> Madhya(double epochToBirthDays, Time time1)
        {
            int _birthYear = time1.GetLmtDateTimeOffset().Year;

            var madhya = new Dictionary<PlanetName, double>();

            //calculate chesta kendra, also called Seeghra kendra

            //SUN 
            //Start from the epoch. Calculate the time of interval from the epoch to the day of birth
            //and multiply the same by the daily motion of the planet, and the change during the interval is obtained.
            var sunEpochMean = 257.4568; //epoch the mean position
            double changeDuringIntervalSun = (epochToBirthDays * 0.9855931);

            //This change being added to or subtracted from the mean position at the
            //time of epoch as the date is posterior or anterior to the epoch day, the mean position is arrived at.
            double meanPositionSun = changeDuringIntervalSun < 0 ? sunEpochMean - changeDuringIntervalSun : sunEpochMean + changeDuringIntervalSun;
            meanPositionSun = meanPositionSun % 360; //expunge
            madhya[Library.PlanetName.Sun] = meanPositionSun;

            //Mean Longitudes of -Inferior Planets.-The mean longitudes of Budba and Sukra are the same as that of the Sun.
            //same for venus & mercury because closer to sun than earth it self
            madhya[Library.PlanetName.Mercury] = madhya[Library.PlanetName.Venus] = madhya[Library.PlanetName.Sun];

            //MARS
            var marsEpochMean = 270.22;
            double changeDuringIntervalMars = (epochToBirthDays * 0.5240218);
            double meanPositionMars = changeDuringIntervalMars < 0 ? marsEpochMean - changeDuringIntervalMars : marsEpochMean + changeDuringIntervalMars;
            meanPositionMars = meanPositionMars % 360; //expunge
            madhya[Library.PlanetName.Mars] = meanPositionMars;

            //JUPITER
            var jupiterEpochMean = 220.04;
            double changeDuringIntervalJupiter = (epochToBirthDays * 0.08310024);
            double meanPositionJupiter = changeDuringIntervalJupiter < 0 ? jupiterEpochMean - changeDuringIntervalJupiter : jupiterEpochMean + changeDuringIntervalJupiter;
            var correction1 = 3.33 + (0.0067 * (_birthYear - 1900));
            meanPositionJupiter -= correction1; //deduct from the total 3.33 + 0.0067*t (where t=birth year-1900).
            meanPositionJupiter %= 360; //expunge
            madhya[Library.PlanetName.Jupiter] = meanPositionJupiter;

            //SATURN
            var saturnEpochMean = 220.04;
            double changeDuringIntervalSaturn = (epochToBirthDays * 0.03333857);
            double meanPositionSaturn = changeDuringIntervalSaturn < 0 ? saturnEpochMean - changeDuringIntervalSaturn : saturnEpochMean + changeDuringIntervalSaturn;
            var correction2 = 5 + (0.001 * (_birthYear - 1900));
            meanPositionSaturn += correction2; //add 5°+0.001*t (where t = birth year - 1900)
            meanPositionSaturn %= 360; //expunge
            madhya[Library.PlanetName.Saturn] = meanPositionSaturn;

            //raise alarm if negative, since that is clearly an error, no negative mean longitude
            if (madhya.Any(x => x.Value < 0)) { throw new Exception("Madya/Mean can't be negative!"); }

            return madhya;
        }


		/// <summary>
		/// Get interval from the epoch to the birth date in days
		/// The result represents the interval from the epoch to the birth date.
		/// </summary>
		[API("Get interval from the epoch to the birth date in days")]
		public static double EpochInterval(Time time1)
        {
            //Determine the interval between birth date and the date of the epoch thus.

            int birthYear = time1.GetLmtDateTimeOffset().Year;
            int birthMonth = time1.GetLmtDateTimeOffset().Month;
            int birthDate = time1.GetLmtDateTimeOffset().Day;

            //month ends in days
            int[] monthEnds = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };

            //Deduct 1900 from the Christian Era. The difference will be past
            //years when positive and coming years when negative.
            int yrdiff = birthYear - 1900;

            //Multiply the same by 365 and to the product add the intervening bi-sextile days.
            var epochDays = ((yrdiff * 365) + (yrdiff / 4) + monthEnds[birthMonth - 1]) - 1 + birthDate;


            int hour = time1.GetLmtDateTimeOffset().Hour;
            int minute = time1.GetLmtDateTimeOffset().Minute;
            double offsetHours = time1.GetLmtDateTimeOffset().Offset.TotalHours;
            double utime = new TimeSpan(hour, minute, 0).TotalHours + ((5 + (double)(4.00 / 60.00)) - offsetHours);

            //The result represents the interval from the epoch to the birth date.
            double interval = epochDays + (double)(utime / 24.00);
            interval = Math.Round(interval, 3);//round to 3 places decimal

            return interval;
        }


        /// <summary>
        /// Gets the planets motion name, can be Retrograde, Direct, Stationary
        /// a name version of Chesta Bala
        /// </summary>
        [API("Gets the planets motion name, can be Retrograde, Direct, Stationary, a name version of Chesta Bala")]
        public static PlanetMotion PlanetMotionName(PlanetName planetName, Time time)
        {
            //sun, moon, rahu & ketu don' have retrograde so always direct
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu) { return PlanetMotion.Direct; }

            //get chestaBala
            var chestaBala = Calculate.PlanetChestaBala(planetName, time).ToDouble();

            //based on chesta bala assign name to it
            //Chesta kendra = 180 degrees = Retrograde
            switch (chestaBala)
            {
                case <= 60 and > 45: return PlanetMotion.Retrograde;
                case <= 45 and > 15: return PlanetMotion.Direct;
                case <= 15 and >= 0: return PlanetMotion.Stationary;
                default:
                    throw new Exception($"Error in GetPlanetMotionName : {chestaBala}");
            }

        }


		/// <summary>
		/// circulation time of the objects in years, used by cheshta bala calculation
		/// </summary>
		[API("circulation time of the objects in years, used by cheshta bala calculation")]
		public static double PlanetCirculationTime(PlanetName planetName)
        {

            if (planetName == Library.PlanetName.Sun) { return 1.0; }
            if (planetName == Library.PlanetName.Moon) { return .082; }
            if (planetName == Library.PlanetName.Mars) { return 1.88; }
            if (planetName == Library.PlanetName.Mercury) { return .24; }
            if (planetName == Library.PlanetName.Jupiter) { return 11.86; }
            if (planetName == Library.PlanetName.Venus) { return .62; }
            if (planetName == Library.PlanetName.Saturn) { return 29.46; }

            throw new Exception("Planet circulation time not defined!");

        }

        /// <summary>
        /// Sapthavargajabala: This is the strength of a
        /// planet due to its residence in the seven sub-divisions
        /// according to its relation with the dispositor.
        ///
        /// Saptavargaja bala means the strength a
        /// planet gets by virtue of its disposition in a friendly,
        /// neutral or inimical Rasi, Hora, Drekkana, Sapthamsa,
        /// Navamsa, Dwadasamsa and Thrimsamsa.
        /// </summary>
        [API(" strength of a planet due to its residence in the seven sub-divisions, Rasi, Hora, Drekkana, Sapthamsa, Navamsa, Dwadasamsa and Thrimsamsa")]
        public static Shashtiamsa PlanetSaptavargajaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSaptavargajaBala", planetName, time), _getPlanetSaptavargajaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSaptavargajaBala()
            {
                //declare total value
                double totalSaptavargajaBalaInShashtiamsa = 0;

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();


                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = Calculate.IsPlanetInMoolatrikona(planetName, time);

                //if planet is in moolatrikona
                if (planetInMoolatrikona)
                {
                    //add the relationship to the list
                    planetSignRelationshipList.Add(PlanetToSignRelationship.Moolatrikona);
                }
                else
                //else get planet's normal relationship with rasi
                {
                    //get planet rasi
                    var planetRasi = Calculate.PlanetRasiSign(planetName, time).GetSignName();
                    var rasiSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //get planet hora
                var planetHora = Calculate.PlanetHoraSign(planetName, time);
                var horaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);


                //get planet drekkana
                var planetDrekkana = Calculate.PlanetDrekkanaSign(planetName, time);
                var drekkanaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //get planet saptamsa
                var planetSaptamsa = Calculate.PlanetSaptamsaSign(planetName, time);
                var saptamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetSaptamsa, time);
                //add planet saptamsa relationship to list
                planetSignRelationshipList.Add(saptamsaSignRelationship);


                //get planet navamsa
                var planetNavamsa = Calculate.PlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //get planet dwadasamsa
                var planetDwadasamsa = Calculate.PlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //get planet thrimsamsa
                var planetThrimsamsa = Calculate.PlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
                //add planet thrimsamsa relationship to list
                planetSignRelationshipList.Add(thrimsamsaSignRelationship);


                //calculate total Saptavargaja Bala

                //loop through all the relationship
                foreach (var planetToSignRelationship in planetSignRelationshipList)
                {
                    //add relationship point accordingly

                    //A planet in its Moolatrikona is assigned a value of 45 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Moolatrikona)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 45;
                    }

                    //in Swavarga 30 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.OwnVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.BestFriendVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.FriendVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.NeutralVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.EnemyVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.BitterEnemyVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 1.875;
                    }

                }


                return new Shashtiamsa(totalSaptavargajaBalaInShashtiamsa);

            }

        }


        /// <summary>
        /// Shadvarga bala: This is the strength of a
        /// planet due to its residence in the 6 sub-divisions
        /// according to its relation with the dispositor.
        ///
        /// They are (1) Rasi, {2) Hora, (3) Drekkana, (4) Navamsa, (5)
        /// Dwadasamsa and (6) Trimsamsa.
        /// </summary>
        [API("strength of a  planet due to its residence in the 6 sub-divisions, Rasi, Hora, Drekkana, Navamsa, Dwadasamsa and Trimsamsa")]
        public static Shashtiamsa PlanetShadvargaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetShadvargaBala", planetName, time), _getPlanetShadvargaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetShadvargaBala()
            {

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();

                //1.
                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = Calculate.IsPlanetInMoolatrikona(planetName, time);

                //if planet is in moolatrikona
                if (planetInMoolatrikona)
                {
                    //add the relationship to the list
                    planetSignRelationshipList.Add(PlanetToSignRelationship.Moolatrikona);
                }
                else
                //else get planet's normal relationship with rasi
                {
                    //get planet rasi
                    var planetRasi = Calculate.PlanetRasiSign(planetName, time).GetSignName();
                    var rasiSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //2.
                //get planet hora
                var planetHora = Calculate.PlanetHoraSign(planetName, time);
                var horaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);

                //3.
                //get planet drekkana
                var planetDrekkana = Calculate.PlanetDrekkanaSign(planetName, time);
                var drekkanaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //4.
                //get planet navamsa
                var planetNavamsa = Calculate.PlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //5.
                //get planet dwadasamsa
                var planetDwadasamsa = Calculate.PlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //6.
                //get planet thrimsamsa
                var planetThrimsamsa = Calculate.PlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
                //add planet thrimsamsa relationship to list
                planetSignRelationshipList.Add(thrimsamsaSignRelationship);


                //calculate total Saptavargaja Bala

                //a place to store total value
                double totalShadvargaBalaInShashtiamsa = 0;

                //loop through all the relationship
                foreach (var planetToSignRelationship in planetSignRelationshipList)
                {
                    //add relationship point accordingly

                    //A planet in its Moolatrikona is assigned a value of 45 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Moolatrikona)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 45;
                    }

                    //in Swavarga 30 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.OwnVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.BestFriendVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.FriendVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.NeutralVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.EnemyVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.BitterEnemyVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 1.875;
                    }

                }


                return new Shashtiamsa(totalShadvargaBalaInShashtiamsa);

            }

        }

        /// <summary>
        /// TODO : use of shadvarga bala might be wrong here, needs clarification
        /// problem is too much of time goes under bad, doesnt seem right
        /// for now we put it 140 threhold so guarenteed to be strong
        /// and doesn not occur all the time
        /// </summary>
        public static bool IsPlanetStrongInShadvarga(PlanetName planet, Time time)
        {
            //get planet shadvarga bala
            var planetBala = Calculate.PlanetShadvargaBala(planet, time).ToDouble();

            //Note: To determine if shadvarga bala value is strong or weak
            //a neutral point is set, anything above is strong & below is weak
            var neutralPoint = Calculate.PlanetShadvargaBalaNeutralPoint(planet);

            //if above neutral number, is strong else it is weak
            return planetBala > 140;
        }


        /// <summary>
        /// Positonal strength
        /// 
        /// A planet occupies a
        /// certain sign in a Rasi and friendly, neutrai or
        /// inimical varga~. It is either exalted or debilitated·
        /// lt ocupies its Moolathrikona or it has its own
        /// varga. All these states refer to the position or
        /// residence of the planet and as such a certain degree
        /// of strength or weakness attends on it. This strength
        /// or potency is known as the Sthanabala.
        /// 
        /// 
        /// 1.Uccha Bala:
        /// Uccha means exaltation. When a planet is placed in its highest exaltation point,
        /// it is of full strength and when it is in its deepest debilitation point, it is devoid of any strength.
        /// When in between the strength is calculated proportionately dependent on the distance these planets are
        /// placed from the highest exaltation or deepest debilitation point.
        ///
        /// 2.Sapta Vargiya Bala:
        /// Rashi, Hora, Drekkana, Saptamsha, Navamsha, Dwadasamsha and Trimsamsha constitute the Sapta Varga.
        /// The strength of the planets in these seven divisional charts based on their placements in Mulatrikona,
        /// own sign, friendly sign etc. constitute the Sapta vargiya bala.
        /// 
        /// 3.Oja-Yugma Rashi-Amsha Bala:
        /// Oja means odd signs and Yugma means even signs. Thus, as the name imply, this strength is derived from
        /// a planet’s placement in the odd or even signs in the Rashi and Navamsha.
        /// 
        /// 4.Kendradi Bala:
        /// The name itself implies how to compute this strength. A planet in a Kendra (1-4-7-10) gets full strength,
        /// while one in Panapara (2-5-8-11) gets half and the one in Apoklimas (12-3-6-9) gets quarter strength.
        ///
        /// 5.Drekkana Bala:
        /// Due to placement in first, second, or third Drekkana of a sign, male, female and hermaphrodite planets respectively,
        /// get a quarter strength according to placements in the first, second and third Drekkana.
        /// </summary>
        [API("residence of the planet and as such a certain degree of strength or weakness attends on it")]
        public static Shashtiamsa PlanetSthanaBala(PlanetName planetName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSthanaBala", planetName, time), _getPlanetSthanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSthanaBala()
            {
                //Get Ochcha Bala (exaltation strength)
                var ochchaBala = PlanetOchchaBala(planetName, time);

                //Get Saptavargaja Bala
                var saptavargajaBala = PlanetSaptavargajaBala(planetName, time);

                //Get Ojayugmarasyamsa Bala
                var ojayugmarasymsaBala = PlanetOjayugmarasyamsaBala(planetName, time);

                //Get Kendra Bala
                var kendraBala = PlanetKendraBala(planetName, time);

                //Drekkana Bala
                var drekkanaBala = PlanetDrekkanaBala(planetName, time);

                //Total Sthana Bala
                var totalSthanaBala = ochchaBala + saptavargajaBala + ojayugmarasymsaBala + kendraBala + drekkanaBala;

                return totalSthanaBala;

            }

        }

        /// <summary>
        /// Drekkanabala: The Sun, Jupiter and Mars
        /// in the lst ; Saturn and Mercury in the 2nd ; and
        /// the Moon and Venus in the last Drekkana, get full
        /// strength of 60 shashtiamsas.
        /// </summary>
        [API("Planet fixed bala, Moon and Venus in the last Drekkana, get full strength of 60 shashtiamsas")]
        public static Shashtiamsa PlanetDrekkanaBala(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = Calculate.PlanetRasiSign(planetName, time);

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //if male planet -Ravi, Guru and Kuja.
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mars)
            {
                //if planet is in 1st drekkana
                if (degreesInSign >= 0 && degreesInSign <= 10.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Hermaphrodite Planets.-Sani and Budba
            if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Mercury)
            {
                //if planet is in 2nd drekkana
                if (degreesInSign > 10 && degreesInSign <= 20.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Female Planets.-Chandra and Sukra
            if (planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Venus)
            {
                //if planet is in 3rd drekkana
                if (degreesInSign > 20 && degreesInSign <= 30.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }
            }

            //if none above conditions met return 0 bala
            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Kendrtzbala: Planets in Kendras get 60
        /// shashtiamsas; in Panapara 30, and in Apoklima 15.
        /// </summary>
        [API("Planets in Kendras get 60 shashtiamsas; in Panapara 30, and in Apoklima 15.")]
        public static Shashtiamsa PlanetKendraBala(PlanetName planetName, Time time)
        {
            //get number of the sign planet is in
            var planetSignNumber = (int)Calculate.PlanetRasiSign(planetName, time).GetSignName();

            //A planet in a kendra sign  gets 60 Shashtiamsas as its strength ;
            //Quadrants.-Kendras-1 (Ar, 4, 7 and 10.
            if (planetSignNumber == 1 || planetSignNumber == 4 || planetSignNumber == 7 || planetSignNumber == 10)
            {
                return new Shashtiamsa(60);
            }

            //in a Panapara sign 30 Shashtiamsas;
            //-Panaparas-2, 5, 8 and 11.
            if (planetSignNumber == 2 || planetSignNumber == 5 || planetSignNumber == 8 || planetSignNumber == 11)
            {
                return new Shashtiamsa(30);
            }


            //and in an Apoklima sign 15 Shashtiamsas.
            //Apoklimas-3, 6, 9 and 12 {9th being a trikona must be omitted).
            if (planetSignNumber == 3 || planetSignNumber == 6 || planetSignNumber == 9 || planetSignNumber == 12)
            {
                return new Shashtiamsa(15);
            }


            throw new Exception("Kendra Bala not found, error");
        }

		/// <summary>
		/// Ojayugmarasyamsa: In odd Rasi and Navamsa,
		/// the Sun, Mars, Jupiter, Mercury and Saturn
		/// get strength and the rest in even signs
		/// </summary>
		[API("In odd Rasi and Navamsa the Sun, Mars, Jupiter, Mercury and Saturn get strength and the rest in even signs")]
        public static Shashtiamsa PlanetOjayugmarasyamsaBala(PlanetName planetName, Time time)
        {
            //get planet rasi sign
            var planetRasiSign = Calculate.PlanetRasiSign(planetName, time).GetSignName();

            //get planet navamsa sign
            var planetNavamsaSign = Calculate.PlanetNavamsaSign(planetName, time);

            //declare total Ojayugmarasyamsa Bala as 0 first
            double totalOjayugmarasyamsaBalaInShashtiamsas = 0;

            //if planet is the moon or venus
            if (planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Venus)
            {
                //if rasi sign is an even sign
                if (Calculate.IsEvenSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an even sign
                if (Calculate.IsEvenSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }
            //if planet is Sun, Mars, Jupiter, Mercury and Saturn
            else if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars ||
                     planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mercury || planetName == Library.PlanetName.Saturn)
            {
                //if rasi sign is an odd sign
                if (Calculate.IsOddSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an odd sign
                if (Calculate.IsOddSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }

            return new Shashtiamsa(totalOjayugmarasyamsaBalaInShashtiamsas);
        }

        [API("KalaBala")]
        public static Shashtiamsa PlanetKalaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }



            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetKalaBala), planetName, time), _getPlanetKalaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetKalaBala()
            {
                //place to store pre kala bala values
                var kalaBalaList = new Dictionary<PlanetName, Shashtiamsa>();


                //Yuddha Bala requires all planet's pre kala bala
                //so calculate pre kala bala for all planets first
                foreach (var planet in Library.PlanetName.All7Planets)
                {
                    //calculate pre kala bala
                    var preKalaBala = GetPreKalaBala(planet, time);

                    //store in a list sorted by planet name, to be accessed later
                    kalaBalaList.Add(planet, preKalaBala);
                }


                //calculate Yuddha Bala
                var yuddhaBala = PlanetYuddhaBala(planetName, kalaBalaList, time);


                //Total Kala Bala
                var total = kalaBalaList[planetName] + yuddhaBala;


                return total;


                //---------------FUNCTIONS--------------
                Shashtiamsa GetPreKalaBala(PlanetName planetName, Time time)
                {
                    //Nathonnatha Bala
                    var nathonnathaBala = PlanetNathonnathaBala(planetName, time);

                    //Paksha Bala
                    var pakshaBala = PlanetPakshaBala(planetName, time);

                    //Tribhaga Bala
                    var tribhagaBala = PlanetTribhagaBala(planetName, time);

                    //Abda Bala
                    var abdaBala = PlanetAbdaBala(planetName, time);

                    //Masa Bala
                    var masaBala = PlanetMasaBala(planetName, time);

                    //Vara Bala
                    var varaBala = PlanetVaraBala(planetName, time);

                    //Hora Bala
                    var horaBala = PlanetHoraBala(planetName, time);

                    //Ayana Bala
                    var ayanaBala = PlanetAyanaBala(planetName, time);

                    //combine all the kala bala calculated so far, and return the value
                    var preKalaBala = nathonnathaBala + pakshaBala + tribhagaBala + abdaBala + masaBala + varaBala + horaBala +
                                      ayanaBala;

                    return preKalaBala;
                }

            }

        }

		/// <summary>
		/// Two planets are said to be in Yuddha or fight when they are in
		/// conjunction and the distance between them is less than one degree.
		/// TODO Not fully tested
		///
		/// Yuddhabala : All planets excepting the Sun
		/// and the Moon enter into war when two planets are
		/// in the same degree. The pJanet having the lesser
		/// longitude is the winner. Find out the sum total of
		/// the SthanabaJa, Kalabala and Digbala of these two'
		/// planets. Difference between the two, divided by
		/// the difference of their diameters of its disc, gives
		/// the Yuddhabala. Add this to the victorious planet
		/// and dedu_ct it from the vanquished.
		/// </summary>
		[API("Two planets are said to be in Yuddha or fight when they are in conjunction and the distance between them is less than one degree")]
		public static Shashtiamsa PlanetYuddhaBala(PlanetName inputedPlanet, Dictionary<PlanetName, Shashtiamsa> preKalaBalaValues, Time time)
        {
            //All the planets excepting Sun and Moon may enter into war (Yuddha)
            if (inputedPlanet == Library.PlanetName.Moon || inputedPlanet == Library.PlanetName.Sun) { return Shashtiamsa.Zero; }


            //place to store winner & loser balas
            var yudhdhabala = new Dictionary<PlanetName, Shashtiamsa>();


            //get all planets that are conjunct with inputed planet
            var conjunctPlanetList = Calculate.PlanetsInConjuction(time, inputedPlanet);

            //remove rahu & kethu if present, they are not included in Yuddha Bala calculations
            conjunctPlanetList.RemoveAll(pl => pl == Library.PlanetName.Rahu || pl == Library.PlanetName.Ketu);


            foreach (var checkingPlanet in conjunctPlanetList)
            {

                //All the planets excepting Sun and Moon may enter into war (Yuddha)
                //no need to calculate Yuddha, move to next planet, if sun or moon
                if (checkingPlanet == Library.PlanetName.Moon || checkingPlanet == Library.PlanetName.Sun) { continue; }


                //get distance between conjunct planet & inputed planet
                var inputedPlanetLong = Calculate.PlanetNirayanaLongitude(time, inputedPlanet);
                var checkingPlanetLong = Calculate.PlanetNirayanaLongitude(time, checkingPlanet);
                var distance = Calculate.DistanceBetweenPlanets(inputedPlanetLong, checkingPlanetLong);


                //if the distance between them is less than one degree
                if (distance < Angle.FromDegrees(1))
                {
                    PlanetName winnerPlanet = null;
                    PlanetName losserPlanet = null;

                    //The conquering planet is the one whose longitude is less.
                    if (inputedPlanetLong < checkingPlanetLong) { winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet; } //inputed planet won
                    else if (inputedPlanetLong > checkingPlanetLong) { winnerPlanet = checkingPlanet; losserPlanet = inputedPlanet; } //checking planet won
                    else if (inputedPlanetLong == checkingPlanetLong)
                    {
                        //unlikely chance, log error & set inputed planet as winner (random)
                        LogManager.Error($"Planets same longitude! Not expected, random result used!");
                        winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet;
                    }

                    //When two planets are in war, get the sum of the various Balas, viv., Sthanabala, the
                    // Dikbala and the Kalabala (up to Horabala) described hitherto of the fighting planets. Find out the
                    // difference between these two sums.
                    var shadbaladiff = Math.Abs(preKalaBalaValues[inputedPlanet].ToDouble() - preKalaBalaValues[checkingPlanet].ToDouble());


                    //Divide shadbala difference by the difference between the diameters of the discs of the two fighting planets
                    var diameterDifference = PlanetDiscDiameter(inputedPlanet).GetDifference(PlanetDiscDiameter(checkingPlanet));


                    //And the resulting quotient which is the Yuddhabala (Shashtiamsa) must be added to the total of the Kalabala (detailed
                    // hitherto) of the victorious planet and must be subtracted from the total Kalabala of the vanquished planet.
                    var shadbala = diameterDifference.TotalDegrees / shadbaladiff;

                    yudhdhabala[winnerPlanet] = new Shashtiamsa(shadbala);
                    yudhdhabala[losserPlanet] = new Shashtiamsa(-shadbala);

                }


            }


            //return yudhdhabala if it was calculated else, return 0 
            var found = yudhdhabala.TryGetValue(inputedPlanet, out var bala);
            return found ? bala : Shashtiamsa.Zero;




            //-----------FUNCTIONS----------------


        }

		/// <summary>
		/// Bimba Parimanas -This means the diameters of the discs of the planets.
		/// </summary>
		[API("Bimba Parimanas, This means the diameters of the discs of the planets")]
		static Angle PlanetDiscDiameter(PlanetName planet)
		{
			if (planet == Library.PlanetName.Mars) { return new Angle(0, 9, 4); }
			if (planet == Library.PlanetName.Mercury) { return new Angle(0, 6, 6); }
			if (planet == Library.PlanetName.Jupiter) { return new Angle(0, 190, 4); }
			if (planet == Library.PlanetName.Venus) { return new Angle(0, 16, 6); }
			if (planet == Library.PlanetName.Saturn) { return new Angle(0, 158, 0); }

			//control should not come here, report error
			throw new Exception("Disc diameter now found!");
		}


		/// <summary>
		/// Ayanabala : All planets get 30 shasbtiamsas
		/// at the equator. For the Sun, Jupiter, Mars
		/// and Venus add proportionately when they are in
		/// northern course and for the Moon and Saturn when
		/// in southern course. Deduct proportionately when
		/// they are in the opposite direction. Unit of strength
		/// is 60 shashtiamsas.
		///
		/// 
		/// TODO some values for calculation with standard hooscope out of whack,
		/// it seems small differences in longitude seem magnified at final value,
		/// not 100% sure, need further testing for confirmation, but final values seem ok so far
		/// </summary>
		[API("All planets get 30 Shashtiamsas at the equator. For the Sun, Jupiter, Mars and Venus add proportionately")]
        public static Shashtiamsa PlanetAyanaBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetAyanaBala), planetName, time), _getPlanetAyanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAyanaBala()
            {
                double ayanaBala = 0;

                //get plant kranti (negative south, positive north)
                var kranti = PlanetDeclination(planetName, time);

                //prepare values for calculation of ayanabala
                var x = Angle.FromDegrees(24);
                var isNorthDeclination = kranti < 0 ? false : true;

                //get declination without negative (absolute value), easier for calculation
                var absKranti = Math.Abs((double)kranti);

                //In case of Sukra, Ravi, Kuja and Guru their north declinations are
                //additive and south declinations are subtractive
                if (planetName == Library.PlanetName.Venus || planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars || planetName == Library.PlanetName.Jupiter)
                {
                    //additive
                    if (isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }

                    //And double the Ayanabala in the case of the Sun
                    if (planetName == Library.PlanetName.Sun) { ayanaBala = ayanaBala * 2; }

                }
                //In case of Sani and Chandra, their south declinations are additive while their
                //north declinations are subtractive.
                else if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Moon)
                {
                    //additive
                    if (!isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }
                }
                //For Budha the declination, north or south, is always additive.
                else if (planetName == Library.PlanetName.Mercury)
                {
                    ayanaBala = ((24 + absKranti) / 48) * 60;
                }


                return new Shashtiamsa(ayanaBala);

            }


        }

        /// <summary>
        /// A heavenly body moves northwards the equator for sometime and
        /// then gets southwards. This angular distance from
        /// the equinoctial or celestial equator is Kranti or the
        /// declination.
        ///
        /// Declinations are reckoned plus or minus according as the planet
        /// is situated in the northern or southern celestial hemisphere
        /// </summary>
        [API("This angular distance from the equinoctial or celestial equator is Kranti or the declination")]
        public static double PlanetDeclination(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetDeclination", planetName, time), _getPlanetDeclination);


            //UNDERLYING FUNCTION
            double _getPlanetDeclination()
            {
                //for degree to radian conversion
                const double DEG2RAD = 0.0174532925199433;

                var eps = EclipticObliquity(time);

                var tlen = Calculate.PlanetSayanaLongitude(time, planetName);
                var lat = Calculate.PlanetSayanaLatitude(time, planetName);

                //if kranti (declination), is a negative number, it means south, else north of equator
                var kranti = lat.TotalDegrees + eps * Math.Sin(DEG2RAD * tlen.TotalDegrees);

                return kranti;
            }

        }

        /// <summary>
        /// true obliquity of the Ecliptic (includes nutation)
        /// </summary>
        [API("true obliquity of the Ecliptic (includes nutation)")]
        public static double EclipticObliquity(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetEps", time), _getPlanetEps);


            //UNDERLYING FUNCTION
            double _getPlanetEps()
            {
                double eps;

                string err = "";
                double[] x = new double[6];

                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                var jul_day_ET = Calculate.TimeToEphemerisTime(time);

                ephemeris.swe_calc(jul_day_ET, SwissEph.SE_ECL_NUT, 0, x, ref err);

                eps = x[0];

                return eps;
            }

        }

        /// <summary>
        /// AKA Horadhipathi Bala
        /// </summary>
        [API("Hora Bala AKA Horadhipathi Bala")]
        public static Shashtiamsa PlanetHoraBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetHoraBala), planetName, time), _getPlanetHoraBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetHoraBala()
            {
                //first ascertain the weekday of birth
                var birthWeekday = Calculate.DayOfWeek(time);

                //ascertain the number of hours elapsed from sunrise to birth
                //This shows the number of horas passed.
                var hora = Calculate.HoraAtBirth(time);

                //get lord of hora (hour)
                var lord = Calculate.LordOfHora(hora, birthWeekday);

                //planet inputed is lord of hora, then 60 shashtiamsas
                if (lord == planetName)
                {
                    return new Shashtiamsa(60);
                }
                else
                {
                    return Shashtiamsa.Zero;
                }

            }



        }

        /// <summary>
        /// The planet who is the king of
        /// the year of birth is assigned a value of 15 Shashtiamsas
        /// as his Abdabala.
        /// </summary>
        [API("The planet who is the king of the year of birth is assigned a value of 15 Shashtiamsas as his Abdabala")]
        public static Shashtiamsa PlanetAbdaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetAbdaBala), planetName, time), _getPlanetAbdaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAbdaBala()
            {
                //calculate year lord
                dynamic yearAndMonthLord = YearAndMonthLord(time);
                PlanetName yearLord = yearAndMonthLord.YearLord;

                //if inputed planet is year lord than 15 Shashtiamsas
                if (yearLord == planetName) { return new Shashtiamsa(15); }

                //not year lord, 0 Shashtiamsas
                return Shashtiamsa.Zero;
            }


        }

        [API("the lord of the month of birth is assigned a value of 30 Shashtiamsas as his Masabala")]
        public static Shashtiamsa PlanetMasaBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the month of birth is assigned a value of 30 Shashtiamsas
            //as his Masabala.

            //calculate month lord
            dynamic yearAndMonthLord = YearAndMonthLord(time);
            PlanetName monthLord = yearAndMonthLord.MonthLord;

            //if inputed planet is month lord than 30 Shashtiamsas
            if (monthLord == planetName) { return new Shashtiamsa(30); }

            //not month lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;

        }

        [API("the lord of the day of birth is assigned a value of 45 Shashtiamsas as his Varabala")]
        public static Shashtiamsa PlanetVaraBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the day of birth is assigned a value of 45 Shashtiamsas
            //as his Varabala.

            //calculate day lord
            PlanetName dayLord = Calculate.LordOfWeekday(time);

            //if inputed planet is day lord than 45 Shashtiamsas
            if (dayLord == planetName) { return new Shashtiamsa(45); }

            //not day lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;

        }

        /// <summary>
        /// Gets year & month lord at inputed time
        /// </summary>
        [API("Gets year & month lord at given time", Category.StarsAboveMe)]
        public static object YearAndMonthLord(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(YearAndMonthLord), time), _getYearAndMonthLord);


            //UNDERLYING FUNCTION
            object _getYearAndMonthLord()
            {
                //set default
                var yearLord = Library.PlanetName.Sun;
                var monthLord = Library.PlanetName.Sun;

                //initialize ephemeris
                using SwissEph ephemeris = new SwissEph();

                double ut_arghana = ephemeris.swe_julday(1827, 5, 2, 0, SwissEph.SE_GREG_CAL);
                double ut_noon = Calculate.GreenwichLmtInJulianDays(time);

                double diff = ut_noon - ut_arghana;
                if (diff >= 0)
                {
                    double quo = Math.Floor(diff / 360.0);
                    diff -= quo * 360.0;
                }
                else
                {
                    double pdiff = -diff;
                    double quo = Math.Ceiling(pdiff / 360.0);
                    diff += quo * 360.0;
                }

                double diff_year = diff;
                while (diff > 30.0) diff -= 30.0;
                double diff_month = diff;
                while (diff > 7) diff -= 7.0;

                var yearLordRaw = ephemeris.swe_day_of_week(ut_noon - diff_year);
                var monthLordRaw = ephemeris.swe_day_of_week(ut_noon - diff_month);

                //parse raw weekday
                var yearWeekday = swissEphWeekDayToMuhurthaDay(yearLordRaw);
                var monthWeekday = swissEphWeekDayToMuhurthaDay(monthLordRaw);


                //Abdadbipat : the planet that rules over the weekday on which the year begins (hindu year)
                yearLord = Calculate.LordOfWeekday(yearWeekday);

                //Masadhipath : The planet that rules the weekday of the commencement of the month of the birth
                monthLord = Calculate.LordOfWeekday(monthWeekday);

                //package year & month lord together & return
                return new { YearLord = yearLord, MonthLord = monthLord };


                //---------------------FUNCTION--------------------

                //converts swiss epehmeris weekday numbering to muhurtha weekday numbering
                DayOfWeek swissEphWeekDayToMuhurthaDay(int dayNumber)
                {
                    switch (dayNumber)
                    {
                        case 0: return Library.DayOfWeek.Monday;
                        case 1: return Library.DayOfWeek.Tuesday;
                        case 2: return Library.DayOfWeek.Wednesday;
                        case 3: return Library.DayOfWeek.Thursday;
                        case 4: return Library.DayOfWeek.Friday;
                        case 5: return Library.DayOfWeek.Saturday;
                        case 6: return Library.DayOfWeek.Sunday;
                        default: throw new Exception("Invalid day number!");
                    }
                }

            }

        }

        /// <summary>
        /// Thribhagabala : Mercury, the Sun and
        /// Saturn get 60 shashtiamsas each, during the lst,
        /// 2nd and 3rd one-third positions of the day, respectively.
        /// The Moon, Venus and Mars govern the lst, 2nd and 3rd one-third portion of the night
        /// respectively. Jupiter is always strong and gets 60
        /// shashtiamsas of strength.
        /// </summary>
        [API("Planets gets bala during the 1st, 2nd and 3rd one-third positions of the day & night respectively.")]
        public static Shashtiamsa PlanetTribhagaBala(PlanetName planetName, Time time)
        {
            PlanetName ret = Library.PlanetName.Jupiter;

            var sunsetTime = Calculate.SunsetTime(time);

            if (IsDayBirth(time))
            {
                //find out which part of the day birth took place

                var sunriseTime = Calculate.SunriseTime(time);

                //substraction should always return a positive number, since sunset is always after sunrise
                double lengthHours = (sunsetTime.Subtract(sunriseTime).TotalHours) / 3;
                double offset = time.Subtract(sunriseTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = Library.PlanetName.Mercury; break;
                    case 1: ret = Library.PlanetName.Sun; break;
                    case 2: ret = Library.PlanetName.Saturn; break;
                }
            }
            else
            {
                //get sunrise time at on next day to get duration of the night
                var nextDayTime = time.AddHours(24);
                var nextDaySunrise = Calculate.SunriseTime(nextDayTime);

                double lengthHours = (nextDaySunrise.Subtract(sunsetTime).TotalHours) / 3;
                double offset = time.Subtract(sunsetTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = Library.PlanetName.Moon; break;
                    case 1: ret = Library.PlanetName.Venus; break;
                    case 2: ret = Library.PlanetName.Mars; break;
                }
            }

            //Always assign a value of 60 Shashtiamsas
            //to Guru irrespective of whether birth is during
            //night or during day.
            if (planetName == Library.PlanetName.Jupiter || planetName == ret) { return new Shashtiamsa(60); }

            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Oochchabala : The distance between the
        /// planet's longitude and its debilitation point, divided
        /// by 3, gives its exaltation strength or oochchabaJa.
        /// </summary>
        [API("OchchaBala")]
        public static Shashtiamsa PlanetOchchaBala(PlanetName planetName, Time time)
        {
            //1.0 Get Planet longitude
            var planetLongitude = Calculate.PlanetNirayanaLongitude(time, planetName);

            //2.0 Get planet debilitation point
            var planetDebilitationPoint = Calculate.PlanetDebilitationPoint(planetName);
            //convert to planet longitude
            var debilitationLongitude = LongitudeAtZodiacSign(planetDebilitationPoint);

            //3.0 Get difference between planet longitude & debilitation point
            //var difference = planetLongitude.GetDifference(planetDebilitationPoint); //todo need checking
            var difference = DistanceBetweenPlanets(planetLongitude, debilitationLongitude);

            //4.0 If difference is more than 180 degrees
            if (difference.TotalDegrees > 180)
            {
                //get the difference of it with 360 degrees
                //difference = difference.GetDifference(Angle.Degrees360);
                difference = Calculate.DistanceBetweenPlanets(difference, Angle.Degrees360);

            }

            //5.0 Divide difference with 3 to get ochchabala
            var ochchabalaInShashtiamsa = difference.TotalDegrees / 3;

            //return value in shashtiamsa type
            return new Shashtiamsa(ochchabalaInShashtiamsa);
        }



        /// <summary>
        /// Determines if the input time is day during day, used for birth times
        /// if day returns true
        /// </summary>
        [API("Determines if the input time is day, if day returns true", Category.StarsAboveMe)]
        public static bool IsDayBirth(Time time)
        {
            //get sunrise & sunset times
            var sunrise = Calculate.SunriseTime(time).GetLmtDateTimeOffset();
            var sunset = Calculate.SunsetTime(time).GetLmtDateTimeOffset();
            var checkingTime = time.GetLmtDateTimeOffset();

            //if time is after sunrise & before sunset, than it is during the day
            if (checkingTime >= sunrise && checkingTime <= sunset)
            {
                return true;
            }
            //else during night
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Pakshabala : When the Moon is waxing,
        /// take the distance from the Sun to the Moon and
        /// divide it by 3. The quotient is the Pakshabala.
        /// When the Moon is waning, take the distance from
        /// the Moon to the· Sun, and divide it by 3 for assessing
        /// Pakshabala. Moon, Jupiter, Venus and
        /// Mercury are strong in Sukla Paksba and the others
        /// in Krishna Paksha.
        ///
        /// Note: Mercury is benefic or malefic based on planets conjunct with it
        /// </summary>
        [API("distance from the Sun to the Moon and divide it by 3. The quotient is the Pakshabala.")]
        public static Shashtiamsa PlanetPakshaBala(PlanetName planetName, Time time)
        {
            double pakshaBala = 0;

            //get moon phase
            var moonPhase = Calculate.LunarDay(time).GetMoonPhase();

            var sunLongitude = Calculate.PlanetNirayanaLongitude(time, Library.PlanetName.Sun);
            var moonLongitude = Calculate.PlanetNirayanaLongitude(time, Library.PlanetName.Moon);

            //var differenceBetweenMoonSun = moonLongitude.GetDifference(sunLongitude);
            var differenceBetweenMoonSun = Calculate.DistanceBetweenPlanets(moonLongitude, sunLongitude);

            //When Moon's Long.-Sun's Long. exceeds 180, deduct it from 360°
            if (differenceBetweenMoonSun.TotalDegrees > 180)
            {
                differenceBetweenMoonSun = Calculate.DistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360);
            }

            double pakshaBalaOfSubhas = 0;

            //get paksha Bala Of Subhas
            if (moonPhase == MoonPhase.BrightHalf)
            {
                //If birth has occurred during Sukla Paksha subtract the Sun's longitude from that of the Moon· Divide the
                // balance by 3. The result represents the Paksha Bala of Subhas.
                pakshaBalaOfSubhas = differenceBetweenMoonSun.TotalDegrees / 3.0;
            }
            else if (moonPhase == MoonPhase.DarkHalf)
            {
                //Subtract the remainder again from 360 degrees and divide the balance divide 3
                var totalDegrees = Calculate.DistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360).TotalDegrees;
                pakshaBalaOfSubhas = totalDegrees / 3.0;
            }

            //60 Shashtiamsas diminished by paksha Bala Of Subhas gives the Paksha Bala of Papas
            var pakshaBalaOfPapas = 60.0 - pakshaBalaOfSubhas;

            //if planet is malefic
            var planetIsMalefic = Calculate.IsPlanetMalefic(planetName, time);
            var planesIsBenefic = Calculate.IsPlanetBenefic(planetName, time);

            if (planesIsBenefic == true && planetIsMalefic == false)
            {
                pakshaBala = pakshaBalaOfSubhas;
            }

            if (planesIsBenefic == false && planetIsMalefic == true)
            {
                pakshaBala = pakshaBalaOfPapas;
            }

            //Chandra's Paksha Bala is always to be doubled
            if (planetName == Library.PlanetName.Moon)
            {
                pakshaBala = pakshaBala * 2.0;
            }

            //if paksha bala is 0
            if (pakshaBala == 0)
            {
                //raise error
                throw new Exception("Paksha bala not found, error!");
            }

            return new Shashtiamsa(pakshaBala);
        }

        /// <summary>
        /// Nathonnathabala: Midnight to midday,
        /// the Sun, Jupiter and Venus gain strength proportionately
        /// till they get maximum at zenith. The other
        /// planets, except Mercury. a,re gaining strength from
        /// midday to midnight proportionately. In the same
        /// way, Mercury is always strong and gets 60 shashtiamsas.
        /// </summary>
        [API("Strength gained by planet from Midnight to midday, midday to midnight proportionately.")]
        public static Shashtiamsa PlanetNathonnathaBala(PlanetName planetName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            //get local apparent time
            var localApparentTime = Calculate.LocalApparentTime(time);

            //convert birth time (reckoned from midnight) into degrees at 15 degrees per hour
            var hour = localApparentTime.Hour;
            var minuteInHours = localApparentTime.Minute / 60.0;
            var secondInHours = localApparentTime.Second / 3600.0;
            //total hours
            var totalTimeInHours = hour + minuteInHours + secondInHours;

            //convert hours to degrees
            const double degreesPerHour = 15;
            var birthTimeInDegrees = totalTimeInHours * degreesPerHour;

            //if birth time in degrees exceeds 180 degrees subtract it from 360
            if (birthTimeInDegrees > 180)
            {
                birthTimeInDegrees = 360 - birthTimeInDegrees;
            }

            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Venus)
            {
                var divBala = birthTimeInDegrees / 3;

                return new Shashtiamsa(divBala);
            }

            if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Mars)
            {
                var ratriBala = (180 - birthTimeInDegrees) / 3;

                return new Shashtiamsa(ratriBala);
            }

            if (planetName == Library.PlanetName.Mercury)
            {
                //Budha has always a Divaratri Bala of 60 Shashtiamsas
                return new Shashtiamsa(60);

            }

            throw new Exception("Planet Nathonnatha Bala not found, error!");
        }

        /// <summary>
        /// Jupiter and Mercury are strong in Lagoa (Ascendant),
        /// 'the Sun and Mars in the 10th, Saturn in
        /// the 7th and the Moon and Venus in the 4th. The
        /// opposite houses are weak , points. Divide the
        /// distance between the longitude of the planet and
        /// its depression point by 3. Quotient is the strength.
        /// </summary>
        [API("Divide the distance between the longitude of the planet and its depression point by 3. Quotient is the strength.")]
        public static Shashtiamsa PlanetDigBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            //get planet longitude
            var planetLongitude = Calculate.PlanetNirayanaLongitude(time, planetName);

            //
            Angle powerlessPointLongitude = null;
            House powerlessHouse;


            //subtract the longitude of the 4th house from the longitudes of the Sun and Mars.
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars)
            {
                powerlessHouse = Calculate.House(HouseName.House4, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //Subtract the 7th house, from Jupiter and Mercury.
            if (planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mercury)
            {
                powerlessHouse = Calculate.House(HouseName.House7, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //Subtracc the 10th house from Venus and the Moon
            if (planetName == Library.PlanetName.Venus || planetName == Library.PlanetName.Moon)
            {
                powerlessHouse = Calculate.House(HouseName.House10, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //from Saturn, the ascendant.
            if (planetName == Library.PlanetName.Saturn)
            {
                powerlessHouse = Calculate.House(HouseName.House1, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //get Digbala arc
            //Digbala arc= planet's long. - its powerless cardinal point.
            //var digBalaArc = planetLongitude.GetDifference(powerlessPointLongitude);
            var xxx = powerlessPointLongitude.TotalDegrees == null ? Angle.Zero : powerlessPointLongitude;
            var digBalaArc = Calculate.DistanceBetweenPlanets(planetLongitude, xxx);

            //If difference is more than 180° 
            if (digBalaArc > Angle.Degrees180)
            {
                //subtract it from 360 degrees.
                //digBalaArc = digBalaArc.GetDifference(Angle.Degrees360);
                digBalaArc = Calculate.DistanceBetweenPlanets(digBalaArc, Angle.Degrees360);
            }

            //The Digbala arc of a ptanet, divided by 3, gives the Digbala
            var digBala = digBalaArc.TotalDegrees / 3;



            return new Shashtiamsa(digBala);

        }

        /// <summary>
        /// Bhava Bala.-Bhava means house and
        /// Bala means strength. Bhava Bala is the potency or
        /// strength of the house or bhava or signification. We
        /// have already seen that there are 12 bhavas which
        /// comprehend all human events. Each bhava signifies
        /// or indicates certain events or functions. For
        /// instance, the first bhava represents Thanu or body,
        /// the appearance of the individual, his complexion,
        /// his disposition, his stature, etc.
        ///
        /// If it attains certain strength, the native will enjoy the indications of
        /// the bhava fully, otherwise he will not sufficiently
        /// enjoy them. The strength of a bhava is composed
        /// of three factors, viz., (1) Bhavadhipathi Bala,
        /// (2) Bhava Digbala, (3) Bhava Drishti Bala.
        /// </summary>
        [API("Bhava Bala is the potency or strength of the house or bhava or signification")]
        public static Shashtiamsa HouseStrength(HouseName inputHouse, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseStrength), inputHouse, time), _getBhavabala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getBhavabala()
            {
                //get all the sub-strengths into a list 
                var subStrengthList = new List<HouseSubStrength>();

                subStrengthList.Add(BhavaAdhipathiBala(time));
                subStrengthList.Add(CalcBhavaDigBala(time));
                subStrengthList.Add(BhavaDrishtiBala(time));

                var totalBhavaBala = new Dictionary<HouseName, double>();

                foreach (var houseToTotal in Library.House.AllHouses)
                {
                    //to get the total strength of the a house, we combine 3 types sub-strengths
                    double total = 0;
                    foreach (var subStrength in subStrengthList) { total += subStrength.Power[houseToTotal]; }
                    totalBhavaBala[houseToTotal] = total;
                }

                return new Shashtiamsa(totalBhavaBala[inputHouse]);

            }

        }

        /// <summary>
        /// Bhavadrishti Bala.-Each bhava in a
        /// horoscope remains aspected by certain planets.
        /// Sometimes the aspect cast on a bhava will be positive
        /// and sometimes it will be negative according
        /// as it is aspected by benefics or malefics.
        /// For all 12 houses
        /// </summary>
        [API("House received aspect strength")]
        public static HouseSubStrength BhavaDrishtiBala(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(BhavaDrishtiBala), time), _calcBhavaDrishtiBala);


            //UNDERLYING FUNCTION
            HouseSubStrength _calcBhavaDrishtiBala()
            {
                double vdrishti;

                //assign initial negative or positive value based on benefic or malefic planet
                var sp = goodAndBad();


                var drishti = GetDrishtiKendra(time);


                double bala = 0;

                var BhavaDrishtiBala = new Dictionary<HouseName, double>();

                foreach (var house in Library.House.AllHouses)
                {

                    bala = 0;

                    foreach (var planet in Library.PlanetName.All7Planets)
                    {

                        bala += (sp[planet] * drishti[planet.ToString() + house.ToString()]);

                    }

                    BhavaDrishtiBala[house] = bala;
                }


                var newHouseResult = new HouseSubStrength(BhavaDrishtiBala, "BhavaDrishtiBala");

                return newHouseResult;



                //------------------LOCAL FUNCTIONS---------------------

                Dictionary<PlanetName, int> goodAndBad()
                {

                    var _sp = new Dictionary<PlanetName, int>();

                    //assign initial negative or positive value based on benefic or malefic planet
                    foreach (var p in Library.PlanetName.All7Planets)
                    {
                        //Though in the earlier pages Mercury is defined either as a subba
                        //(benefic) or papa (malefic) according to its association is with a benefic or
                        //malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
                        //be deemed as a full benefic. This is in accord with the injunctions of
                        //classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).

                        if (p == Library.PlanetName.Mercury)
                        {
                            _sp[p] = 1;
                            continue;
                        }

                        if (Calculate.IsPlanetBenefic(p, time))
                        {
                            _sp[p] = 1;
                        }
                        else
                        {
                            _sp[p] = -1;
                        }
                    }

                    return _sp;
                }

                Dictionary<String, double> GetDrishtiKendra(Time time1)
                {

                    //planet & house no. is used key
                    var _drishti = new Dictionary<String, double>();

                    double drishtiKendra;

                    foreach (var planet in Library.PlanetName.All7Planets)
                    {
                        foreach (var houseNo in Library.House.AllHouses)
                        {
                            //house is considered as a Drusya Graha (aspected body)
                            var houseMid = Calculate.House(houseNo, time1).GetMiddleLongitude();
                            var plantLong = Calculate.PlanetNirayanaLongitude(time1, planet);

                            //Subtract the longitude of the Drishti (aspecting)
                            // planet from that of the Drusya (aspected) Bhava
                            // Madhya. The Drishti Kendra is obtained.
                            drishtiKendra = (houseMid - plantLong).TotalDegrees;

                            //In finding the Drishti Kendra always add 360° to the longitude of the
                            //Drusya (Bhava Madhya) when it is less than the longitude of the
                            //Drishta (aspecting Graha).
                            if (drishtiKendra < 0) { drishtiKendra += 360; }

                            //get special aspect if any
                            vdrishti = FindViseshaDrishti(drishtiKendra, planet);

                            if ((planet == Library.PlanetName.Mercury) || (planet == Library.PlanetName.Jupiter))
                            {
                                //take the Drishti values of Guru and Budha on the Bhava Madhya as they are
                                _drishti[planet.ToString() + (houseNo).ToString()] = FindDrishtiValue(drishtiKendra) + vdrishti;
                            }
                            else
                            {
                                //take a fourth of the aspect value of other Grahas over the Bhava Madhya
                                _drishti[planet.ToString() + (houseNo).ToString()] = (FindDrishtiValue(drishtiKendra) + vdrishti) / 4.00;
                            }
                        }
                    }


                    return _drishti;
                }
            }

        }

        /// <summary>
        /// Bhava Digbala.-This is the strength
        /// acquired by the different bhavas falling in the
        /// different groups or types of signs.
        /// For all 12 houses
        /// </summary>
        [API("House strength from different types of signs")]
        public static HouseSubStrength CalcBhavaDigBala(Time time)
        {

            var BhavaDigBala = new Dictionary<HouseName, double>();

            int dig = 0;

            //for every house
            foreach (var houseNumber in Library.House.AllHouses)
            {
                //a particular bhava acquires strength by its mid-point
                //falling in a particular kind of sign.

                //so get mid point of house
                var mid = Calculate.House(houseNumber, time).GetMiddleLongitude().TotalDegrees;
                var houseSign = Calculate.HouseSignName(houseNumber, time);

                //Therefore first find the number of a given Bhava Madhya and subtract
                // it from 1, if the given Bhava Madhya is situated
                // in Vrischika
                if ((mid >= 210.00)
                    && (mid <= 240.00))
                {
                    dig = 1 - (int)houseNumber;
                }
                //Subtract it from 4, if the given Bhava
                // Madhya is situated in Mesha, Vrishabha, Simha,
                // first half of Makara or last half of Dhanus.
                else if (((mid >= 0.00) && (mid <= 60.00))
                         || ((mid >= 120.00) && (mid <= 150.00))
                         || ((mid >= 255.00) && (mid <= 285.00)))
                {
                    dig = 4 - (int)houseNumber;
                }
                //Subtract it from 7 if in Mithuna, Thula, Kumbha, Kanya or
                // first half of Dhanus
                else if (((mid >= 60.00) && (mid <= 90.00))
                         || ((mid >= 150.00) && (mid <= 210.00))
                         || ((mid >= 300.00) && (mid <= 330.00))
                         || ((mid >= 240.00) && (mid <= 255.00)))
                {
                    dig = 7 - (int)houseNumber;
                }
                //and lastly from 1O if in Kataka, Meena and last half of Makara.
                else if (((mid >= 90.00) && (mid <= 120.00))
                         || ((mid >= 330.00) && (mid <= 360.00))
                         || ((mid >= 285.00) && (mid <= 300.00)))
                {
                    dig = 10 - (int)houseNumber;
                }


                //If the difference exceeds 6, subtract it from 12, otherwise
                //take it as it is and multiply this difference by 1O.
                //And you will get Bhava digbala of the particular bhava.

                if (dig < 0)
                {
                    dig = dig + 12;
                }

                if (dig > 6)
                {
                    dig = 12 - dig;
                }

                //store digbala value in return list with house number
                BhavaDigBala[houseNumber] = (double)dig * 10;

            }


            var newHouseResult = new HouseSubStrength(BhavaDigBala, "BhavaDigBala");

            return newHouseResult;

        }

        /// <summary>
        /// Bhavadhipatbi Bala.-This is the potency
        /// of the lord of the bhava.
        /// For all 12 houses
        /// </summary>
        [API("This is the potency of the lord of the bhava, for all 12 houses")]
        public static HouseSubStrength BhavaAdhipathiBala(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(BhavaAdhipathiBala), time), _calcBhavaAdhipathiBala);


            //UNDERLYING FUNCTION
            HouseSubStrength _calcBhavaAdhipathiBala()
            {
                PlanetName houseLord;

                var BhavaAdhipathiBala = new Dictionary<HouseName, double>();

                foreach (var house in Library.House.AllHouses)
                {

                    //get current house lord
                    houseLord = Calculate.LordOfHouse(house, time);

                    //The Shadbala Pinda (aggregate of the Shadbalas) of the lord of the
                    //bhava constitutes its Bhavadhipathi Bala.
                    //get Shadbala Pinda of lord (total strength) in shashtiamsas
                    BhavaAdhipathiBala[house] = PlanetShadbalaPinda(houseLord, time).ToDouble();

                }

                var newHouseResult = new HouseSubStrength(BhavaAdhipathiBala, "BhavaAdhipathiBala");

                return newHouseResult;

            }

        }

        #endregion

        /// <summary>
        /// 0 index is strongest
        /// </summary>
        public static List<PlanetName> BeneficPlanetListByShadbala(Time personBirthTime, int threshold)
        {

            //get all planets
            //var allPlanetByStrenght = AstronomicalCalculator.GetAllPlanetOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<PlanetName>();
            var yyy = Calculate.AllPlanetStrength(personBirthTime);
            foreach (var planet in yyy)
            {
                if (planet.Item1 > threshold)
                {
                    returnList.Add(planet.Item2);
                }
            }
            return returnList;
        }

        public static List<PlanetName> BeneficPlanetListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllPlanetOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<PlanetName>();
            returnList.Add(allPlanetByStrenght[0]);
            //returnList.Add(allPlanetByStrenght[1]);
            //returnList.Add(allPlanetByStrenght[2]);

            return returnList;
        }


        /// <summary>
        /// 0 index is strongest
        /// </summary>
        public static List<HouseName> BeneficHouseListByShadbala(Time personBirthTime, int threshold)
        {
            var returnList = new List<HouseName>();

            //create a list with planet names & its strength (unsorted)
            foreach (var house in Library.House.AllHouses)
            {
                //get house strength
                var strength = HouseStrength(house, personBirthTime).ToDouble();

                if (strength > threshold)
                {
                    returnList.Add(house);
                }


            }

            return returnList;


        }
        
        public static List<HouseName> BeneficHouseListByShadbala(Time personBirthTime)
        {
            //get all planets
            var allPlanetByStrenght = Calculate.GetAllHousesOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<HouseName>();
            returnList.Add(allPlanetByStrenght[0]);
            //returnList.Add(allPlanetByStrenght[1]);
            //returnList.Add(allPlanetByStrenght[2]);

            return returnList;


        }

        public static List<PlanetName> MaleficPlanetListByShadbala(Time personBirthTime, int threshold)
        {

            var returnList = new List<PlanetName>();
            var yyy = Calculate.AllPlanetStrength(personBirthTime);
            foreach (var planet in yyy)
            {
                if (planet.Item1 < threshold)
                {
                    returnList.Add(planet.Item2);
                }
            }
            return returnList;
        }

        /// <summary>
        /// 0 index is most malefic
        /// </summary>
        public static List<PlanetName> MaleficPlanetListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllPlanetOrderedByStrength(personBirthTime);

            //take last 3 as needed planets
            var returnList = new List<PlanetName>();
            returnList.Add(allPlanetByStrenght[^1]);
            //returnList.Add(allPlanetByStrenght[^2]);
            //returnList.Add(allPlanetByStrenght[^3]);

            return returnList;

        }

        /// <summary>
        /// 0 index is most malefic
        /// </summary>
        public static List<HouseName> MaleficHouseListByShadbala(Time personBirthTime, int threshold)
        {
            var returnList = new List<HouseName>();

            //create a list with planet names & its strength (unsorted)
            foreach (var house in Library.House.AllHouses)
            {
                //get house strength
                var strength = HouseStrength(house, personBirthTime).ToDouble();

                if (strength < threshold)
                {
                    returnList.Add(house);
                }


            }

            return returnList;
        }
        
        public static List<HouseName> MaleficHouseListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.GetAllHousesOrderedByStrength(personBirthTime);

            //take last 3 as needed planets
            var returnList = new List<HouseName>();
            returnList.Add(allPlanetByStrenght[^1]);
            //returnList.Add(allPlanetByStrenght[^2]);
            //returnList.Add(allPlanetByStrenght[^3]);

            return returnList;

        }

        [API("Given a person will give yoni kuta animal with sex")]
		public static string YoniKutaAnimal(Person person)
        {
            var finalPrediction = "";

            var birthConst = Calculate.MoonConstellation(person.BirthTime);
            var animal = Calculate.YoniKutaAnimal(birthConst.GetConstellationName());

            finalPrediction += animal.ToString();

            return finalPrediction;
        }

		/// <summary>
		/// Given a constellation will give animal with sex, used for yoni kuta calculations
		/// and body appearance prediction
		/// </summary>
		[API("Given a constellation will give animal with sex, used for yoni kuta calculations")]
        public static ConstellationAnimal YoniKutaAnimal(ConstellationName sign)
        {
            switch (sign)
            {
                case ConstellationName.Aswini:
                    return new ConstellationAnimal("Male", AnimalName.Horse);
                case ConstellationName.Satabhisha:
                    return new ConstellationAnimal("Female", AnimalName.Horse);
                case ConstellationName.Bharani:
                    return new ConstellationAnimal("Male", AnimalName.Elephant);
                case ConstellationName.Revathi:
                    return new ConstellationAnimal("Female", AnimalName.Elephant);
                case ConstellationName.Pushyami:
                    return new ConstellationAnimal("Male", AnimalName.Sheep);
                case ConstellationName.Krithika:
                    return new ConstellationAnimal("Female", AnimalName.Sheep);
                case ConstellationName.Rohini:
                    return new ConstellationAnimal("Male", AnimalName.Serpent);
                case ConstellationName.Mrigasira:
                    return new ConstellationAnimal("Female", AnimalName.Serpent);
                case ConstellationName.Moola:
                    return new ConstellationAnimal("Male", AnimalName.Dog);
                case ConstellationName.Aridra:
                    return new ConstellationAnimal("Female", AnimalName.Dog);
                case ConstellationName.Aslesha:
                    return new ConstellationAnimal("Male", AnimalName.Cat);
                case ConstellationName.Punarvasu:
                    return new ConstellationAnimal("Female", AnimalName.Cat);
                case ConstellationName.Makha:
                    return new ConstellationAnimal("Male", AnimalName.Rat);
                case ConstellationName.Pubba:
                    return new ConstellationAnimal("Female", AnimalName.Rat);
                case ConstellationName.Uttara:
                    return new ConstellationAnimal("Male", AnimalName.Cow);
                case ConstellationName.Uttarabhadra:
                    return new ConstellationAnimal("Female", AnimalName.Cow);
                case ConstellationName.Swathi:
                    return new ConstellationAnimal("Male", AnimalName.Buffalo);
                case ConstellationName.Hasta:
                    return new ConstellationAnimal("Female", AnimalName.Buffalo);
                case ConstellationName.Vishhaka:
                    return new ConstellationAnimal("Male", AnimalName.Tiger);
                case ConstellationName.Chitta:
                    return new ConstellationAnimal("Female", AnimalName.Tiger);
                case ConstellationName.Jyesta:
                    return new ConstellationAnimal("Male", AnimalName.Hare);
                case ConstellationName.Anuradha:
                    return new ConstellationAnimal("Female", AnimalName.Hare);
                case ConstellationName.Poorvashada:
                    return new ConstellationAnimal("Male", AnimalName.Monkey);
                case ConstellationName.Sravana:
                    return new ConstellationAnimal("Female", AnimalName.Monkey);
                case ConstellationName.Poorvabhadra:
                    return new ConstellationAnimal("Male", AnimalName.Lion);
                case ConstellationName.Dhanishta:
                    return new ConstellationAnimal("Female", AnimalName.Lion);
                case ConstellationName.Uttarashada:
                    return new ConstellationAnimal("Male", AnimalName.Mongoose);



                default: throw new Exception("");
            }
        }
    }
}