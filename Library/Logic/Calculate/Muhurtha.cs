

//▄▀█   █░░ █ █▀▀   █ █▀   ░░█ █░█ █▀ ▀█▀   ▄▀█   █▀▀ █▀█ █▀▀ ▄▀█ ▀█▀   █▀ ▀█▀ █▀█ █▀█ █▄█   ▀█▀ █░█ ▄▀█ ▀█▀
//█▀█   █▄▄ █ ██▄   █ ▄█   █▄█ █▄█ ▄█ ░█░   █▀█   █▄█ █▀▄ ██▄ █▀█ ░█░   ▄█ ░█░ █▄█ █▀▄ ░█░   ░█░ █▀█ █▀█ ░█░

//█▀ █▀█ █▀▄▀█ █▀▀ █▀█ █▄░█ █▀▀   █▀█ █░█ █ █▄░█ █▀▀ █▀▄   █░█░█ █ ▀█▀ █░█   ▀█▀ █░█ █▀▀   ▀█▀ █▀█ █░█ ▀█▀ █░█
//▄█ █▄█ █░▀░█ ██▄ █▄█ █░▀█ ██▄   █▀▄ █▄█ █ █░▀█ ██▄ █▄▀   ▀▄▀▄▀ █ ░█░ █▀█   ░█░ █▀█ ██▄   ░█░ █▀▄ █▄█ ░█░ █▀█
//Barnabus Stinson



using System;
using System.Collections.Generic;
using System.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// A collection of methods used to calculate if an event is ocuring
    /// Note:
    /// - Attributes are used to link a particular method to the event data stored in database
    /// - Split across file because VS IDE started to lag with autocomplete (too much code at once)
    /// </summary>
    public static partial class EventCalculatorMethods
    {
        [EventCalculator(EventName.Empty)]
        public static CalculatorResult Empty(Time time, Person person) => CalculatorResult.NotOccuring();

        #region PANCHA PAKSHI

        [EventCalculator(EventName.BirdRuling)]
        public static CalculatorResult BirdRuling(Time time, Person person) => new() { Occuring = Calculate.MainActivity(person.BirthTime, time) == BirdActivity.Ruling };

        [EventCalculator(EventName.BirdEating)]
        public static CalculatorResult BirdEating(Time time, Person person) => new() { Occuring = Calculate.MainActivity(person.BirthTime, time) == BirdActivity.Eating };

        [EventCalculator(EventName.BirdWalking)]
        public static CalculatorResult BirdWalking(Time time, Person person) => new() { Occuring = Calculate.MainActivity(person.BirthTime, time) == BirdActivity.Walking };

        [EventCalculator(EventName.BirdSleeping)]
        public static CalculatorResult BirdSleeping(Time time, Person person) => new() { Occuring = Calculate.MainActivity(person.BirthTime, time) == BirdActivity.Sleeping };

        [EventCalculator(EventName.BirdDying)]
        public static CalculatorResult BirdDying(Time time, Person person) => new() { Occuring = Calculate.MainActivity(person.BirthTime, time) == BirdActivity.Dying };

        #endregion

        #region TRAVEL

        //Doctor Manhatan can walk on the surface of the sun,
        //has seen events so fast & small that they can't be said to have happened
        //And yet, he noticed his girlfried getting old.
        //He does not control his emotions. He does not control his mind.

        /// <summary>
        /// For journeys/travel the best lunar days are the 2nd, 3rd, 5th, 7th, 10th, 11th and 13th.
        /// </summary>
        [EventCalculator(EventName.GoodLunarDayForTravel)]
        public static CalculatorResult GoodLunarDayForTravel(Time time, Person person)
        {

            //get lunar day to check
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //right lunar days to look for
            var rightLunarDay = lunarDayNumber == 2 ||
                                lunarDayNumber == 3 ||
                                lunarDayNumber == 5 ||
                                lunarDayNumber == 7 ||
                                lunarDayNumber == 10 ||
                                lunarDayNumber == 11 ||
                                lunarDayNumber == 13;


            //event occuring if right lunar day
            var occuring = rightLunarDay == true;

            return new() { Occuring = occuring };
        }

        /// <summary>
        /// For journeys/travel the 14th lunar day and Full and New Moon days should be avoided at any cost.
        /// </summary>
        [EventCalculator(EventName.BadLunarDayForTravel)]
        public static CalculatorResult BadLunarDayForTravel(Time time, Person person)
        {

            //get lunar day to check
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //bad lunar days to look for
            var badLunarDay = lunarDayNumber == 1 || //new moon
                                lunarDayNumber == 14 ||
                                lunarDayNumber == 15; //full moon


            //event occuring if bad lunar day is occuring
            var occuring = badLunarDay == true;

            return new() { Occuring = occuring };
        }

        /// <summary>
        /// If a journey is undertaken in the following constellations, the person is
        /// supposed to return back early after satisfactorily completing his work:
        /// Mrigasira, Aswini, Pushya, Punarvasu, Hasta, Anuradha, Sravana,
        /// Moola, Dhanishta and Revati.It is better that the journey is commenced
        /// in the 2nd, 3rd or last quarter of the constellation.The first quarter may
        /// be avoided as far as possible.
        /// </summary>
        [EventCalculator(EventName.GoodConstellationForTravel)]
        public static CalculatorResult GoodConstellationForTravel(Time time, Person person)
        {
            //get ruling constellation
            var rulingConstellation = Calculate.MoonConstellation(time);
            var rulingConstellationName = rulingConstellation.GetConstellationName();

            //check if good constellation
            var isGoodConstellation = false;
            if (rulingConstellationName == ConstellationName.Mrigasira ||
                rulingConstellationName == ConstellationName.Aswini ||
                rulingConstellationName == ConstellationName.Pushyami ||
                rulingConstellationName == ConstellationName.Punarvasu ||
                rulingConstellationName == ConstellationName.Hasta ||
                rulingConstellationName == ConstellationName.Anuradha ||
                rulingConstellationName == ConstellationName.Sravana ||
                rulingConstellationName == ConstellationName.Moola ||
                rulingConstellationName == ConstellationName.Dhanishta ||
                rulingConstellationName == ConstellationName.Revathi)
            {

                isGoodConstellation = true;
            }

            //all quaters buts the first
            var isCorrectQuater = rulingConstellation.GetQuarter() != 1;

            //event occuring if is good constellation & correct quater
            var occuring = isGoodConstellation && isCorrectQuater;

            return new() { Occuring = occuring };

        }

        /// <summary>
        /// No journey should be undertaken on days ruled by Krithika, Bharani.
        /// Aslesha. Visakha, Pubba, Poorvabhadra and Aridra.
        ///
        /// TODO Below
        /// Of these, the following nakshatras may be deemed fit for travelling beyond the
        /// spheres of evil influence: - Krittika - 13 ghatis; Bharani - 7; Makha - 14;
        /// Pubba, Poorvashadha and Poorvabhadra - 16; Swati, Aslesha and
        /// Visakha - 14.
        ///
        /// In our humble experience, it is found that Bharani and
        /// Krithika should always be avoided while the other stars given in this
        /// paragraph, journeys can be undertaken in.
        /// 
        /// We have to emphasize that Bharani and Krithika should be invariably
        /// rejected.
        /// </summary>
        [EventCalculator(EventName.BadConstellationForTravel)]
        public static CalculatorResult BadConstellationForTravel(Time time, Person person)
        {
            //get ruling constellation
            var rulingConstellation = Calculate.MoonConstellation(time);
            var rulingConstellationName = rulingConstellation.GetConstellationName();

            //check if bad constellation
            //No journey should be undertaken on days ruled by Krithika, Bharani.
            //Aslesha. Visakha, Pubba, Poorvabhadra and Aridra.
            var isBadConstellation = false;
            if (rulingConstellationName == ConstellationName.Krithika ||
                rulingConstellationName == ConstellationName.Bharani ||
                rulingConstellationName == ConstellationName.Aslesha ||
                rulingConstellationName == ConstellationName.Vishhaka ||
                rulingConstellationName == ConstellationName.Pubba ||
                rulingConstellationName == ConstellationName.Poorvabhadra ||
                rulingConstellationName == ConstellationName.Aridra)
            {

                isBadConstellation = true;
            }

            //event occuring if is good constellation & correct quater
            var occuring = isBadConstellation;

            return new() { Occuring = occuring };

        }

        /// <summary>
        /// Do not travel towards the East on Saturday and Monday
        /// Provided the journey is timed to begin
        /// beyond 8 ghatis on Saturday and Monday, the
        /// above restriction does not hold good.
        /// </summary>
        [EventCalculator(EventName.BadWeekdayForTravelEast)]
        public static CalculatorResult BadWeekdayForTravelEast(Time time, Person person)
        {
            //get week day
            var weekday = Calculate.DayOfWeek(time);

            switch (weekday)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Monday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring
                    return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// Do not travel towards South on Thursday.
        /// Provided the journey is timed to begin
        /// beyond 22 ghatis on Thursday, the
        /// above restriction does not hold good.
        /// </summary>
        [EventCalculator(EventName.BadWeekdayForTravelSouth)]
        public static CalculatorResult BadWeekdayForTravelSouth(Time time, Person person)
        {
            //get week day
            var weekday = Calculate.DayOfWeek(time);

            switch (weekday)
            {
                case DayOfWeek.Thursday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring
                    return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// Do not travel towards West on Sunday and Friday.
        /// Provided the journey is timed to begin
        /// beyond 15 ghatis on Friday and Sunday, the
        /// above restriction does not hold good.
        /// </summary>
        [EventCalculator(EventName.BadWeekdayForTravelWest)]
        public static CalculatorResult BadWeekdayForTravelWest(Time time, Person person)
        {
            //get week day
            var weekday = Calculate.DayOfWeek(time);

            switch (weekday)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Friday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring
                    return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// Do not travel towards North on Wednesday and Tuesday.
        /// Provided the journey is timed to begin
        /// beyond 12 ghatis on Tuesday and Wednesday, the
        /// above restriction does not hold good
        /// 
        /// In our view, Tuesday must
        /// preferably be avoided.
        /// </summary>
        [EventCalculator(EventName.BadWeekdayForTravelNorth)]
        public static CalculatorResult BadWeekdayForTravelNorth(Time time, Person person)
        {
            //get week day
            var weekday = Calculate.DayOfWeek(time);

            switch (weekday)
            {
                case DayOfWeek.Wednesday:
                case DayOfWeek.Tuesday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring
                    return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// Aries, Taurus, Cancer, Leo, Libra and Sagittarius are favorable signs
        /// for starting on a journey.
        /// </summary>
        [EventCalculator(EventName.GoodLagnaForTravel)]
        public static CalculatorResult GoodLagnaForTravel(Time time, Person person)
        {
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //if rising sign is the right one then event is occuring
            if (risingSign == ZodiacName.Aries ||
                risingSign == ZodiacName.Taurus ||
                risingSign == ZodiacName.Cancer ||
                risingSign == ZodiacName.Leo ||
                risingSign == ZodiacName.Libra ||
                risingSign == ZodiacName.Sagittarius)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //if different sign, not occuring
                return CalculatorResult.NotOccuring();
            }

        }

        /// <summary>
        /// Rising sign at the time of journey being one's Janma Rasi is highly
        /// favoured.
        /// </summary>
        [EventCalculator(EventName.BestLagnaForTravel)]
        public static CalculatorResult BestLagnaForTravel(Time time, Person person)
        {
            //get janma rasi
            var janmaRasi = Calculate.MoonSignName(person.BirthTime);

            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //if current rissing sign same as birth moon sign
            if (janmaRasi == risingSign)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //if different sign, not occuring
                return CalculatorResult.NotOccuring();
            }

        }

        /// <summary>
        /// Rising sign at the time of journey should never be the sign of one's Janma Lagna.
        /// </summary>
        [EventCalculator(EventName.WorstLagnaForTravel)]
        public static CalculatorResult WorstLagnaForTravel(Time time, Person person)
        {
            //get janma lagna
            var janmaLagna = Calculate.HouseSignName(HouseName.House1, person.BirthTime);

            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //if current rissing sign same as birth moon sign
            if (janmaLagna == risingSign)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //if different sign, not occuring
                return CalculatorResult.NotOccuring();
            }

        }

        /// <summary>
        /// Journey should not also be undertaken when the Lagna is the 5th, 7th
        /// or the 9th from one's Janma Lagna.
        /// </summary>
        [EventCalculator(EventName.BadLagnaForTravel)]
        public static CalculatorResult BadLagnaForTravel(Time time, Person person)
        {
            //get janma lagna
            var janmaLagna = Calculate.HouseSignName(HouseName.House1, person.BirthTime);

            //get all the signs from lagna that needs to be checked
            //TODO possible count should be done the other way??
            var _5thSign = Calculate.SignCountedFromLagnaSign(5, time);
            var _7thSign = Calculate.SignCountedFromLagnaSign(7, time);
            var _9thSign = Calculate.SignCountedFromLagnaSign(9, time);

            //if any of the signs match lagna than event is occuring
            var isMatch = _5thSign == janmaLagna || _7thSign == janmaLagna || _9thSign == janmaLagna;

            if (isMatch)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //if different sign, not occuring
                return CalculatorResult.NotOccuring();
            }

        }

        /// <summary>
        /// Let Jupiter or Venus be well placed in Lagna at the time of starting. This
        /// makes the journey successful.
        /// </summary>
        [EventCalculator(EventName.GoodPlanetsInLagnaForTravel)]
        public static CalculatorResult GoodPlanetsInLagnaForTravel(Time time, Person person)
        {
            //planets in lagna
            var planetsInLagna = Calculate.PlanetsInHouse(HouseName.House1, time);

            //check all planets in lagna
            foreach (var planetName in planetsInLagna)
            {
                //only for jupiter and venus
                if (planetName == PlanetName.Jupiter || planetName == PlanetName.Venus)
                {
                    //to make sure planet is "well placed"
                    //if any of the planets is not enemy with the sign
                    var relationship = Calculate.PlanetRelationshipWithHouse(HouseName.House1, planetName, time);
                    var isNotEnemy = relationship != PlanetToSignRelationship.EnemyVarga ||
                                     relationship != PlanetToSignRelationship.BitterEnemyVarga;
                    if (isNotEnemy)
                    {
                        //if control comes here than "well placed" & Jupiter/Venus in lagna
                        return CalculatorResult.IsOccuring();
                    }
                }
            }

            //if control comes here than not occuring
            return CalculatorResult.NotOccuring();
        }

        /// <summary>
        /// The Moon should be in the 3rd, 6th, 9th or 12th and Jupiter in a
        /// kendra from Lagna.
        /// </summary>
        [EventCalculator(EventName.GoodMoonJupiterTravelYoga)]
        public static CalculatorResult GoodMoonJupiterTravelYoga(Time time, Person person)
        {
            //Moon should be in the 3rd, 6th, 9th or 12th
            var moonIn3_6_9_12 = Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House3, time) ||
                                 Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House6, time) ||
                                 Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House9, time) ||
                                 Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House12, time);

            //Jupiter in a kendra from Lagna.
            var isJupiterInKendra = Calculate.IsPlanetInKendra(PlanetName.Jupiter, time);

            //both conditions need to be met
            var occuring = isJupiterInKendra && moonIn3_6_9_12;

            return new() { Occuring = occuring };

        }

        /// <summary>
        /// Start when the Moon is in Lagna fortified by the disposition of Jupiter 
        /// or Venus in a kendra.
        /// </summary>
        [EventCalculator(EventName.FortifiedMoonTravelYoga)]
        public static CalculatorResult FortifiedMoonTravelYoga(Time time, Person person)
        {
            //moon in lagna
            var isMoonInLagna = Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House1, time);

            //venus or jupiter in Kendra
            var planetNames = new PlanetName[] { PlanetName.Jupiter, PlanetName.Venus };
            var isVenusJupiterKendra = Calculate.IsPlanetInKendra(planetNames, time);

            //both conditions need to be met to fortify moon
            var occuring = isMoonInLagna && isVenusJupiterKendra;

            return new() { Occuring = occuring };

        }


        /// <summary>
        /// Jupiter strong in Lagna and the Moon in any place other than the 8th 
        /// would be a strong combination. 
        /// </summary>
        [EventCalculator(EventName.StrongJupiterTravelYoga)]
        public static CalculatorResult StrongJupiterTravelYoga(Time time, Person person)
        {
            //check if jupiter is in lagna
            var jupiterInLagna = Calculate.IsPlanetInHouse(PlanetName.Jupiter, HouseName.House1, time);

            //check if jupiter is strong (shadbala)
            var jupiterIsStrong = Calculate.IsPlanetStrongInShadbala(PlanetName.Jupiter, time);

            //moon NOT in 8th house
            var moonNotIn8 = !Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House8, time);

            //all conditions need to meet
            var occuring = jupiterInLagna && jupiterIsStrong && moonNotIn8;

            return new() { Occuring = occuring };

        }

        /// <summary>
        /// The journey will be easy and peaceful if the Moon be in the 7th and
        /// Venus and Mercury be in the 4th. 
        /// </summary>
        [EventCalculator(EventName.EasyPeacefulTravelYoga)]
        public static CalculatorResult EasyPeacefulTravelYoga(Time time, Person person)
        {
            //moon in the 7th
            var isMoonIn7th = Calculate.IsPlanetInHouse(PlanetName.Moon, HouseName.House7, time);

            //venus & mercury in 4th
            var isVenusIn4th = Calculate.IsPlanetInHouse(PlanetName.Venus, HouseName.House4, time);
            var isMercuryIn4th = Calculate.IsPlanetInHouse(PlanetName.Mercury, HouseName.House4, time);

            //all conditions need to meet
            var occuring = isMoonIn7th && isVenusIn4th && isMercuryIn4th;

            return new() { Occuring = occuring };

        }

        /// <summary>
        /// Mercury in the 4th, Jupiter in the 2nd or 7th will neutralise all the 
        /// other adverse influences.
        /// </summary>
        [EventCalculator(EventName.NeutralizeBadTravelYoga)]
        public static CalculatorResult NeutralizeBadTravelYoga(Time time, Person person)
        {
            //Mercury in the 4th
            var isMercuryIn4th = Calculate.IsPlanetInHouse(PlanetName.Mercury, HouseName.House4, time);

            //Jupiter in the 2nd
            var isJupiterIn2nd = Calculate.IsPlanetInHouse(PlanetName.Jupiter, HouseName.House2, time);

            //Jupiter in the 7th
            var isJupiterIn7th = Calculate.IsPlanetInHouse(PlanetName.Jupiter, HouseName.House7, time);

            //either 1 conditions meets 
            var occuring = isMercuryIn4th || isJupiterIn2nd || isJupiterIn7th;

            return new() { Occuring = occuring };

        }

        /// <summary>
        /// Benefics dignified in kendras or trikonas act as powerful antidotes for 
        /// all evils. 
        /// </summary>
        [EventCalculator(EventName.StrongBeneficsTravelYoga)]
        public static CalculatorResult StrongBeneficsTravelYoga(Time time, Person person)
        {
            //get all benefic planets at current time
            var beneficPlanets = Calculate.BeneficPlanetList(time);

            //check if any of the benefic is in kendra or trikona and is strong as well
            //else event not occuring
            foreach (var planet in beneficPlanets)
            {
                //in trine or kendra
                var isInKendra = Calculate.IsPlanetInKendra(planet, time);
                var isInTrikona = Calculate.IsPlanetInTrikona(planet, time);
                var trikonaOrKendra = isInKendra || isInTrikona;

                //strong
                var isStrong = Calculate.IsPlanetStrongInShadbala(planet, time);

                //both conditions met, event occuring
                if (trikonaOrKendra && isStrong) { return CalculatorResult.IsOccuring(); }

            }

            //if control reache here than not occuring
            return CalculatorResult.NotOccuring();

        }

        /// <summary>
        /// Jupiter in Lagna, malefics in Upachayas and Venus in any house
        /// other than the 7th would be an ideal combinations.
        /// </summary>
        [EventCalculator(EventName.IdealPlanetsTravelYoga)]
        public static CalculatorResult IdealPlanetsTravelYoga(Time time, Person person)
        {
            //check if jupiter is in lagna 
            var isJupiterInLagna = Calculate.IsPlanetInHouse(PlanetName.Jupiter, HouseName.House1, time);

            //malefics in Upachayas  ( 3rd, 6th, 10th, and 11th)
            var isMaleficsInUpachayas = Calculate.IsAllMaleficsInUpachayas(time);

            //Venus in any house other than the 7th
            var venusNotIn7th = !Calculate.IsPlanetInHouse(PlanetName.Venus, HouseName.House7, time);

            //all has to meet
            var occuring = isJupiterInLagna && isMaleficsInUpachayas && venusNotIn7th;

            return new() { Occuring = occuring };

        }


        #endregion

        #region GHATAKA CHAKRA

        /// <summary>
        ///  If the ghataka element happens to be Moon, then the injury is on reputation,
        /// </summary>
        [EventCalculator(EventName.GhatakaMoonSign)]
        public static CalculatorResult GhatakaMoonSign(Time time, Person person)
        {
            var row = new GhatakaRow();
            var ghatakaRow = nameof(row.MoonSign);
            return CalculatorResult.New(Calculate.GhatakaChakra(time, person.BirthTime).Contains(ghatakaRow));
        }

        /// <summary>
        ///  If the ghataka element happens to be Tithi or Nakshatra, the injury is more mental and emotional.
        /// </summary>
        [EventCalculator(EventName.GhatakaTithiGroup)]
        public static CalculatorResult GhatakaTithiGroup(Time time, Person person)
        {
            var row = new GhatakaRow();
            var ghatakaRow = nameof(row.TithiGroup);
            return CalculatorResult.New(Calculate.GhatakaChakra(time, person.BirthTime).Contains(ghatakaRow));
        }

        /// <summary>
        ///  If the ghataka element happens to be Weekday or the Lagna, the injury is more physical
        /// </summary>
        [EventCalculator(EventName.GhatakaWeekDay)]
        public static CalculatorResult WeekDayGroup(Time time, Person person)
        {
            var row = new GhatakaRow();
            var ghatakaRow = nameof(row.WeekDay);
            return CalculatorResult.New(Calculate.GhatakaChakra(time, person.BirthTime).Contains(ghatakaRow));
        }

        /// <summary>
        /// If the ghataka element happens to be Tithi or Nakshatra, the injury is more mental and emotional.
        /// </summary>
        [EventCalculator(EventName.GhatakaMoonConstellation)]
        public static CalculatorResult MoonConstellationGroup(Time time, Person person)
        {
            var row = new GhatakaRow();
            var ghatakaRow = nameof(row.MoonConstellation);
            return CalculatorResult.New(Calculate.GhatakaChakra(time, person.BirthTime).Contains(ghatakaRow));
        }

        /// <summary>
        /// If the ghataka element happens to be Weekday or the Lagna, the injury is more physical
        /// </summary>
        [EventCalculator(EventName.GhatakaLagna)]
        public static CalculatorResult GhatakaLagna(Time time, Person person)
        {
            var row = new GhatakaRow();
            var ghatakaRow = nameof(row.LagnaSameSex);
            return CalculatorResult.New(Calculate.GhatakaChakra(time, person.BirthTime).Contains(ghatakaRow));
        }


        #endregion

        #region PERSONAL

        //[EventCalculator(EventName.GoodTarabala)] TODO Can be removed and fucntion moved to astronomical
        public static CalculatorResult IsGoodTarabalaOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            switch (tarabala.GetName())
            {   //return true for good tarabala names
                case TarabalaName.Sampat:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.Kshema:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.Sadhana:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.Mitra:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.ParamaMitra:
                    return CalculatorResult.IsOccuring();
                //return false if no good tarabala names matched
                default:
                    return CalculatorResult.NotOccuring();
            }
        }

        //[EventCalculator(EventName.BadTarabala)] TODO Can be removed and fucntion moved to astronomical
        public static CalculatorResult IsBadTarabalaOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            switch (tarabala.GetName())
            {   //return true if tarabala is false
                case TarabalaName.Janma:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.Vipat:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.Pratyak:
                    return CalculatorResult.IsOccuring();
                case TarabalaName.Naidhana:
                    return CalculatorResult.IsOccuring();
                //return false if no bad tarabala names matched
                default:
                    return CalculatorResult.NotOccuring();
            }
        }

        [EventCalculator(EventName.TarabalaJanmaStrong)]
        public static CalculatorResult IsTarabalaJanmaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);
            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Janma && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaSampatStrong)]
        public static CalculatorResult IsTarabalaSampatStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sampat && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaVipatStrong)]
        public static CalculatorResult IsTarabalaVipatStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Vipat && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaKshemaStrong)]
        public static CalculatorResult IsTarabalaKshemaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Kshema && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaPratyakStrong)]
        public static CalculatorResult IsTarabalaPratyakStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Pratyak && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaSadhanaStrong)]
        public static CalculatorResult IsTarabalaSadhanaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sadhana && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaNaidhanaStrong)]
        public static CalculatorResult IsTarabalaNaidhanaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Naidhana && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaMitraStrong)]
        public static CalculatorResult IsTarabalaMitraStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Mitra && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaParamaMitraStrong)]
        public static CalculatorResult IsTarabalaParamaMitraStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.ParamaMitra && tarabala.GetCycle() == 1;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaJanmaMiddling)]
        public static CalculatorResult IsTarabalaJanmaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Janma && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaSampatMiddling)]
        public static CalculatorResult IsTarabalaSampatMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sampat && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaVipatMiddling)]
        public static CalculatorResult IsTarabalaVipatMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Vipat && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaKshemaMiddling)]
        public static CalculatorResult IsTarabalaKshemaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Kshema && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaPratyakMiddling)]
        public static CalculatorResult IsTarabalaPratyakMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Pratyak && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaSadhanaMiddling)]
        public static CalculatorResult IsTarabalaSadhanaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sadhana && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaNaidhanaMiddling)]
        public static CalculatorResult IsTarabalaNaidhanaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Naidhana && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaMitraMiddling)]
        public static CalculatorResult IsTarabalaMitraMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Mitra && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaParamaMitraMiddling)]
        public static CalculatorResult IsTarabalaParamaMitraMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.ParamaMitra && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaJanmaWeak)]
        public static CalculatorResult IsTarabalaJanmaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Janma && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaSampatWeak)]
        public static CalculatorResult IsTarabalaSampatWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sampat && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaVipatWeak)]
        public static CalculatorResult IsTarabalaVipatWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Vipat && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaKshemaWeak)]
        public static CalculatorResult IsTarabalaKshemaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Kshema && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaPratyakWeak)]
        public static CalculatorResult IsTarabalaPratyakWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Pratyak && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaSadhanaWeak)]
        public static CalculatorResult IsTarabalaSadhanaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sadhana && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaNaidhanaWeak)]
        public static CalculatorResult IsTarabalaNaidhanaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Naidhana && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaMitraWeak)]
        public static CalculatorResult IsTarabalaMitraWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Mitra && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaParamaMitraWeak)]
        public static CalculatorResult IsTarabalaParamaMitraWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = Calculate.Tarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.ParamaMitra && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }



        [EventCalculator(EventName.GoodChandrabala)]
        public static CalculatorResult IsGoodChandrabalaOccuring(Time time, Person person)
        {
            //if bad chandrabala is occuring
            if (IsBadChandrabalaOccuring(time, person).Occuring)
            {
                //return false
                return CalculatorResult.NotOccuring();
            }
            else
            {   //if bad chandrabala is not occuring good chandrabala is occuring
                return CalculatorResult.IsOccuring();
            }
        }

        [EventCalculator(EventName.BadChandrabala)]
        public static CalculatorResult IsBadChandrabalaOccuring(Time time, Person person)
        {
            //get chandrabala number for time
            var chandrabalaNumber = Calculate.Chandrabala(time, person);

            switch (chandrabalaNumber)
            {
                case 6:
                    return CalculatorResult.IsOccuring();
                case 8:
                    {
                        //Chandrashtama shows no evil when the Moon is waxing and
                        // occupies a benefic sign and a benefic Navamsa, or when there is
                        // Tarabala. The sting is lost when the Moon and the 8th lord are friends.

                        //if any of the 3 exception conditions are met, bad chandrabala is nulified
                        if (condition1() || condition2() || condition3()) { return CalculatorResult.NotOccuring(); }

                        return CalculatorResult.IsOccuring();
                    }

                case 12:
                    return CalculatorResult.IsOccuring();
                default:
                    return CalculatorResult.NotOccuring();
            }

            //condition 1 : Moon is waxing AND occupies a benefic sign AND a benefic Navamsa
            bool condition1()
            {
                //1. Moon is waxing
                var moonPhase = Calculate.LunarDay(time).GetMoonPhase();

                //check if phase is correct 
                var rightPhase = moonPhase == MoonPhase.BrightHalf;

                //if not correct phase, end here as not occuring
                if (rightPhase == false) { return false; }


                //2. Moon occupies a benefic sign
                var moonSign = Calculate.PlanetZodiacSign(PlanetName.Moon, time);
                var relationship = Calculate.PlanetRelationshipWithSign(PlanetName.Moon, moonSign.GetSignName(), time);

                //check if sign is benefic 
                var isBenefic = relationship == PlanetToSignRelationship.OwnVarga || //Swavarga - own varga
                                relationship == PlanetToSignRelationship.FriendVarga || //Mitravarga - friendly varga
                                relationship == PlanetToSignRelationship.BestFriendVarga; //Adhi Mitravarga - Intimate friend varga

                //if not benefic, end here as not occuring
                if (isBenefic == false) { return false; }


                //3. Moon occupies a benefic Navamsa sign
                var moonNavamsaSign = Calculate.PlanetNavamsaSign(PlanetName.Moon, time);
                var navamsaRelationship = Calculate.PlanetRelationshipWithSign(PlanetName.Moon, moonNavamsaSign, time);

                //check if sign is benefic 
                var isBeneficNavamsa = navamsaRelationship == PlanetToSignRelationship.OwnVarga || //Swavarga - own varga
                                navamsaRelationship == PlanetToSignRelationship.FriendVarga || //Mitravarga - friendly varga
                                navamsaRelationship == PlanetToSignRelationship.BestFriendVarga; //Adhi Mitravarga - Intimate friend varga

                //if not benefic, end here as not occuring
                if (isBeneficNavamsa == false) { return false; }


                //if control reaches here then condition is met
                return true;
            }

            //condition 2 : there is good Tarabala
            bool condition2()
            {
                return IsGoodTarabalaOccuring(time, person).Occuring;
            }

            //condition 3 : when the Moon and the 8th lord are friends
            bool condition3()
            {
                //get lord of 8th house
                var lord8th = Calculate.LordOfHouse(HouseName.House8, time);

                //get relationship between moon and 8th lord
                var relationship =
                    Calculate.PlanetCombinedRelationshipWithPlanet(PlanetName.Moon, lord8th, time);

                var isFriends = relationship == PlanetToPlanetRelationship.BestFriend ||
                                relationship == PlanetToPlanetRelationship.Friend;

                return isFriends;
            }

        }

        [EventCalculator(EventName.GoodPanchaka)]
        public static CalculatorResult IsGoodPanchakaOccuring(Time time, Person person)
        {
            //get occuring panchaka
            var panchakaName = Calculate.Panchaka(time);

            //if panchaka is good (subha)
            if (panchakaName == PanchakaName.Shubha)
            {
                //event is occuring
                return CalculatorResult.IsOccuring();
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        [EventCalculator(EventName.BadPanchaka)]
        public static CalculatorResult IsBadPanchakaOccuring(Time time, Person person)
        {
            //check if good panchaka occuring 
            var goodPanchakaOcurring = IsGoodPanchakaOccuring(time, person).Occuring;

            //if good panchaka is occuring
            if (goodPanchakaOcurring)
            {
                //bad panchaka is not occuring
                return CalculatorResult.NotOccuring();
            }
            else
            {
                //else bad panchaka is occuring
                return CalculatorResult.IsOccuring();
            }
        }


        //MARKED FOR DELETION BECAUSE NEW VIEW DOESN NEED IT, CREATED FOR GOOGLE CALENDAR

        //[EventCalculator(EventName.BadTaraChandraPanchaka)]
        //public static CalculatorResult IsBadTaraChandraPanchakaOccuring(Time time, Person person)
        //{
        //    //bad tarabala
        //    var badTarabala = IsBadTarabalaOccuring(time, person).Occuring;

        //    //bad chandrabala
        //    var badChandrabala = IsBadChandrabalaOccuring(time, person).Occuring;

        //    //bad panchaka
        //    var badPanchaka = IsBadPanchakaOccuring(time, person).Occuring;


        //    if (badTarabala && badChandrabala && badPanchaka)
        //    {
        //        return CalculatorResult.IsOccuring();
        //    }
        //    else
        //    {
        //        return CalculatorResult.NotOccuring();
        //    }

        //}

        //[EventCalculator(EventName.GoodTaraChandraPanchaka)]
        //public static CalculatorResult IsGoodTaraChandraPanchakaOccuring(Time time, Person person)
        //{
        //    //good tarabala
        //    var goodTarabala = IsGoodTarabalaOccuring(time, person).Occuring;

        //    //good chandrabala
        //    var goodChandrabala = IsGoodChandrabalaOccuring(time, person).Occuring;

        //    //good panchaka
        //    var goodPanchaka = IsGoodPanchakaOccuring(time, person).Occuring;


        //    if (goodTarabala && goodChandrabala && goodPanchaka)
        //    {
        //        return CalculatorResult.IsOccuring();
        //    }
        //    else
        //    {
        //        return CalculatorResult.NotOccuring();
        //    }

        //}

        //[EventCalculator(EventName.GoodTaraChandra)]
        //public static CalculatorResult IsGoodTaraChandraOccuring(Time time, Person person)
        //{
        //    //good tarabala
        //    var goodTarabala = IsGoodTarabalaOccuring(time, person).Occuring;

        //    //good chandrabala
        //    var goodChandrabala = IsGoodChandrabalaOccuring(time, person).Occuring;


        //    if (goodTarabala && goodChandrabala)
        //    {
        //        return CalculatorResult.IsOccuring();
        //    }
        //    else
        //    {
        //        return CalculatorResult.NotOccuring();
        //    }

        //}

        //[EventCalculator(EventName.BadTaraChandra)]
        //public static CalculatorResult IsBadTaraChandraOccuring(Time time, Person person)
        //{
        //    //bad tarabala
        //    var badTarabala = IsBadTarabalaOccuring(time, person).Occuring;

        //    //bad chandrabala
        //    var badChandrabala = IsBadChandrabalaOccuring(time, person).Occuring;


        //    if (badTarabala && badChandrabala)
        //    {
        //        return CalculatorResult.IsOccuring();
        //    }
        //    else
        //    {
        //        return CalculatorResult.NotOccuring();
        //    }

        //}

        [EventCalculator(EventName.JanmaNakshatraRulling)]
        public static CalculatorResult IsJanmaNakshatraRullingOccuring(Time time, Person person)
        {
            //A day ruled by one's Janma Nakshatra is ordinarily held to be
            // unfavourable for an election. But in regard to nuptials, sacrifices, first
            // feeding, agriculture, upanayanam, coronation, buying lands, learning
            // the alphabet, Janma Nakshatra is favourable without exception. But it is
            // inauspicious for war, sexual union, shaving, taking medical treatment,
            // travel and marriage. For a woman, Janma Nakshatra would be quite
            // favourable for marriage.

            //get birth rulling costellation 
            var birthRulingConstellation = Calculate.MoonConstellation(person.BirthTime);

            //get current rulling constellation
            var currentRulingConstellation = Calculate.MoonConstellation(time);

            //check only if constellation "name" is match (not checking quater), if match event occuring
            var occuring = birthRulingConstellation.GetConstellationName() == currentRulingConstellation.GetConstellationName();
            return new() { Occuring = occuring };

        }


        #endregion

        #region MEDICAL

        [EventCalculator(EventName.UgraYoga)]
        public static CalculatorResult IsUgraYogaOccuring(Time time, Person person)
        {
            //Any treatment commenced under Ugra yogas are supposed to prove
            // successful. Ugra yogas arise when the 3rd (or 9th), 4th, 5th, 6th, 7th,
            // 9th, 10th, 12th (or 3rd) and 13th lunar days coincide respectively with
            // Rohini, Uttara, Sravana, Mrigasira. Revats, Krittika, Pushva, Anuradha.
            // Krittika (or Makha).

            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();


            if (lunarDayNumber == 3 || lunarDayNumber == 9
                || lunarDayNumber == 4 || lunarDayNumber == 5
                || lunarDayNumber == 6 || lunarDayNumber == 7
                || lunarDayNumber == 10 || lunarDayNumber == 12
                || lunarDayNumber == 13)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Rohini ||
                    rulingConstellationName == ConstellationName.Uttara ||
                    rulingConstellationName == ConstellationName.Sravana ||
                    rulingConstellationName == ConstellationName.Mrigasira ||
                    rulingConstellationName == ConstellationName.Revathi ||
                    rulingConstellationName == ConstellationName.Krithika ||
                    rulingConstellationName == ConstellationName.Pushyami ||
                    rulingConstellationName == ConstellationName.Anuradha ||
                    rulingConstellationName == ConstellationName.Makha)
                {
                    return CalculatorResult.IsOccuring();
                }

            }


            //if none of the conditions above are met return false
            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.GoodTakingInjections)]
        public static CalculatorResult IsGoodTakingInjectionsOccuring(Time time, Person person)
        {
            //Injections may be taken on Saturday or Monday.
            // Aries, Taurus. Cancer and Virgo are auspicious. The 8th house must be
            // unoccupied. See that Mercury'is free from affliction; as otherwise the
            // pain wilt be severe and nervous weakness may set in.

            //get current weekday
            var weekday = Calculate.DayOfWeek(time);

            //1. may be taken on Saturday or Monday.
            //right weekdays to look for
            var rightWeekday = weekday == DayOfWeek.Saturday || weekday == DayOfWeek.Monday;

            //if not correct weekdays, end here as not occuring
            if (rightWeekday == false) { return CalculatorResult.NotOccuring(); }


            //2. Aries, Taurus. Cancer and Virgo are auspicious
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Aries
                            || risingSign == ZodiacName.Taurus
                            || risingSign == ZodiacName.Cancer
                            || risingSign == ZodiacName.Virgo;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }


            //3. The 8th house must be unoccupied
            var planets8thHouse = Calculate.PlanetsInHouse(HouseName.House8, time);

            //if got planets in 8th house, event not occuring
            if (planets8thHouse.Any()) { return CalculatorResult.NotOccuring(); }


            //4. Mercury is free from affliction
            var mercuryIsAfflicted = Calculate.IsMercuryAfflicted(time);

            //if afflicted, event not occuring
            if (mercuryIsAfflicted) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        #endregion

        #region DEBUG

        //[EventCalculator(EventName.CustomEvent)]
        public static CalculatorResult IsCustomEventOccuring(Time time, Person person)
        {
            //good tarabala
            var goodTarabala = IsGoodTarabalaOccuring(time, person).Occuring;

            //good chandrabala
            var goodChandrabala = IsGoodChandrabalaOccuring(time, person).Occuring;

            //ugra yoga
            var ugraYoga = IsUgraYogaOccuring(time, person).Occuring;

            if (goodTarabala && goodChandrabala && ugraYoga)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }

        }


        #endregion

        #region HOUSE
        //[EventCalculator(EventName.BadSunSignForHouseBuilding)]
        //public static Prediction IsKujasthamaOccuring(Time time, Person person)
        //{
        //    //The Sun should occupy fixed signs or
        //    // at least movable signs but no building work should be undertaken when
        //    // the Sun is in common signs.

        //    //get sign Sun is in
        //    var sunSignName = AstronomicalCalculator.GetSunSign(time);

        //    //check if sign is a common sign


        //    //if it is common sign



        //}



        #endregion

        #region MARRIAGE

        [EventCalculator(EventName.TaitulaKarana)]
        public static CalculatorResult IsTaitulaKaranaOccuring(Time time, Person person)
        {
            //Thaithula is propitious for marriage
            var karana = Calculate.Karana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Taitula;
            return new() { Occuring = occuring };

        }



        #endregion

        #region GENERAL

        [EventCalculator(EventName.SakunaKarana)]
        public static CalculatorResult IsSakunaKaranaOccuring(Time time, Person person)
        {
            //For getting initiations into mantras Sakuni Karana is propitious.   

            var karana = Calculate.Karana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Sakuna;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.BavaKarana)]
        public static CalculatorResult IsBavaKaranaOccuring(Time time, Person person)
        {
            //Thus Bava is auspicious for starting works of permanent importance while
            var karana = Calculate.Karana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Bava;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.BhadraKarana)]
        public static CalculatorResult IsBhadraKaranaOccuring(Time time, Person person)
        {
            //Bhadra is unfit for any good work but is eminently suitable for
            //violent and cruel deeds.

            var karana = Calculate.Karana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Visti;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.Ekadashi)]
        public static CalculatorResult IsEkadashiOccuring(Time time, Person person)
        {
            // It is the 11th tithi

            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //right lunar days to look for
            var rightLunarDay = lunarDayNumber == 11;

            //if not correct lunar days, end here as not occuring
            if (rightLunarDay == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.BhriguShatka)]
        public static CalculatorResult IsBhriguShatkaOccuring(Time time, Person person)
        {
            //The position of Venus in the 6th is injurious. This is
            // especially so in regard to marriage. Even when Venus is exalted and
            // associated with benefics, such a disposition is not approved.

            //get house venus is in
            var houseVenusIsIn = Calculate.HousePlanetOccupies(PlanetName.Venus, time);

            //if venus is in 6th house
            if (houseVenusIsIn == HouseName.House6)
            {
                //event is occuring
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //event is not occuring
                return CalculatorResult.NotOccuring();
            }


        }

        [EventCalculator(EventName.Kujasthama)]
        public static CalculatorResult IsKujasthamaOccuring(Time time, Person person)
        {

            //Mars should be avoided in the 8th house, as it
            // indicates destruction of the object in view. In a marriage election chart.
            // Mars in the 8th is unthinkable. Even if Mars is otherwise powerful, he
            // should not occupy the 8th house.

            //get house mars is in
            var houseMarsIsIn = Calculate.HousePlanetOccupies(PlanetName.Mars, time);

            //if mars is in 8th house
            if (houseMarsIsIn == HouseName.House8)
            {
                //event is occuring
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //event is not occuring
                return CalculatorResult.NotOccuring();
            }


        }

        [EventCalculator(EventName.KarthariDosha)]
        public static CalculatorResult IsKarthariDoshaOccuring(Time time, Person person)
        {
            //Karthari Dosha. - Karthari means scissors. In an election, when two
            // evil planets are placed on either side of the Lagna, the combination
            // goes under the special name of Karthari Dosha and it should be
            // rejected for good work particularly in regard to marriage.

            //get name of planets that are evil
            //Evil planets
            // Sun, New Moon (weak), Badly associated mercury, mars, saturn, rahu, ketu
            //

            //1.0 Get list of evil planets
            var listOfEvilPlanets = Calculate.MaleficPlanetList(time);

            //2.0 Check if evil planets are in house 12 & 2

            //mark evil planet not found in 12th house first
            var evilPlanetFoundInHouse12 = false;
            //mark evil planet not found in 2nd house first
            var evilPlanetFoundInHouse2 = false;

            //get planets in 12th house
            List<PlanetName> planetsInHouse12 = Calculate.PlanetsInHouse(HouseName.House12, time);

            //check if evil planets are found in house 12
            foreach (var planet in listOfEvilPlanets)
            {
                //if evil planet found, set flag
                evilPlanetFoundInHouse12 = planetsInHouse12.Contains(planet);

                //once evil planet found
                if (evilPlanetFoundInHouse12)
                {
                    //break loop, stop looking
                    break;
                }
            }

            //if evil planet found in house 12, check house 2 for evil planets
            if (evilPlanetFoundInHouse12)
            {
                //get planets in 2nd house
                List<PlanetName> planetsInHouse2 = Calculate.PlanetsInHouse(HouseName.House2, time);

                //check if evil planets are found in house 2
                foreach (var planet in listOfEvilPlanets)
                {
                    //if evil planet found, set flag
                    evilPlanetFoundInHouse2 = planetsInHouse2.Contains(planet);

                    //once evil planet found
                    if (evilPlanetFoundInHouse2)
                    {
                        //break loop, stop looking
                        break;
                    }
                }

            }

            //3.0 If evil planets found in both houses, event is occuring
            if (evilPlanetFoundInHouse12 && evilPlanetFoundInHouse2)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //event not occuring
                return CalculatorResult.NotOccuring();
            }

        }

        [EventCalculator(EventName.ShashtashtaRiphagathaChandraDosha)]
        public static CalculatorResult IsShashtashtaRiphagathaChandraDoshaOccuring(Time time, Person person)
        {
            //Shashtashta Riphagatha Chandra Dosha. - The Moon should
            // invariably be avoided in the 6th, 8th and 12th houses from the Lagna
            // rising in an election chart.

            //get house moon is in
            var houseMoonIsIn = Calculate.HousePlanetOccupies(PlanetName.Moon, time);

            //if house moon is in is 6, 8 or 12
            if (houseMoonIsIn == HouseName.House6 || houseMoonIsIn == HouseName.House8 || houseMoonIsIn == HouseName.House12)
            {
                //event is occuring
                return CalculatorResult.IsOccuring();
            }

            //else event is not occuring
            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.SagrahaChandraDosha)]
        public static CalculatorResult IsSagrahaChandraDoshaOccuring(Time time, Person person)
        {
            //Sagraha Chandra Dosha. - The Moon's association (conjunction) with any other
            // planet, benefic or malefic, should be avoided. This injunction is specially
            // applicable in case of marriage.

            //get planets in conjunction with the moon
            var planetsInConjunct = Calculate.PlanetsInConjuction(PlanetName.Moon, time);

            //if any planets are in conjunct with moon, event is occuring
            if (planetsInConjunct.Count > 0) { return CalculatorResult.IsOccuring(); }

            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.UdayasthaSuddhi)]
        public static CalculatorResult IsUdayasthaSuddhiOccuring(Time time, Person person)
        {

            //6. Udayasta Suddhi. - The Lagna and the seventh should be strong.
            // The Lagna should be occupied by its own lord and the Navamsa Lagna
            // by its own lord or vice versa or lord of Lagna should aspect Navamsa
            // Lagna and vice versa. Similarly the seventh and the lord of the seventh
            // Bhava should be favourably disposed. The strength of Lagna and the
            // seventh is necessary in all elections but especially so in regard to
            // marriage.

            //TODO NOTE : Below reference can be used but not sure.
            //"Jupiter, Mercury or Venus in Lagna, malefics in the 3rd or 11th, would
            // constitute a formidable force in rendering the Lagna strong."

            //Note : "Strength" here is determined based on the rules above only, (shadbala is not used)

            //check if lagna is strong
            var lagnaIsStrong = IsStrongLagnaOccuring(time);

            //if lagna is NOT strong, end here as not occuring
            if (!lagnaIsStrong) { return CalculatorResult.NotOccuring(); }

            //check if 7th is strong
            var house7IsStrong = IsStrongHouse7Occuring(time);


            //ocurring if lagna & house 7 is strong
            var occuring = lagnaIsStrong && house7IsStrong;
            return new() { Occuring = occuring };




            //-----------------------------------------FUNCTIONS--------------------------------


            //strenght here is based on rules above
            //Note : not 100% sure if strenght calculated here can be used else where
            bool IsStrongLagnaOccuring(Time time)
            {
                // 1. The Lagna should be occupied by its own lord and the Navamsa Lagna
                // by its own lord or vice versa or lord of Lagna should aspect Navamsa
                // Lagna and vice versa.

                //Note 3 possible condition for event

                //POSSIBLE 1
                //1.1 Lagna occupied by Lagna Lord
                bool lagnaLordInLagna = Calculate.IsHouseLordInHouse(HouseName.House1, HouseName.House1, time);

                //1.2 Navamsa lagna occupied Navamsa Lagna Lord
                bool navamsaLagnaLordInNavamsaLagna = IsNavamsaLagnaLordInNavamsaLagnaOccuring(time);

                if (lagnaLordInLagna && navamsaLagnaLordInNavamsaLagna)
                {
                    //event is occuring
                    return true;
                }

                //POSSIBLE 2
                //1.3 Lagna occupied by Navamsa Lagna Lord
                bool navamsaLagnaLordInLagna = IsNavamsaLagnaLordInLagna(time);

                //1.4 Navamsa lagna occupied by Lagna Lord
                bool lagnaLordInNavamsaLagna = IsLagnaLordInNavamsaLagnaOccuring(time);

                if (navamsaLagnaLordInLagna && lagnaLordInNavamsaLagna)
                {
                    //event is occuring
                    return true;
                }

                //POSSIBLE 3
                //lord of Lagna should aspect Navamsa Lagna
                bool lordOfLagnaAspectsNavamsaLagna = IsLagnaLordAspectingNavamsaLagnaOccuring(time);

                //1.6 lord of Navamsa Lagna should aspect Lagna
                bool lordOfNavamsaLagnaAspectsLagna = IsNavamsaLagnaLordAspectingLagna(time);

                if (lordOfLagnaAspectsNavamsaLagna && lordOfNavamsaLagnaAspectsLagna)
                {
                    //event is occuring
                    return true;
                }

                //if no above condition met, event not occuring
                return false;
            }

            //strenght here is based on rules above
            //Note : not 100% sure if strenght calculated here can be used else where
            bool IsStrongHouse7Occuring(Time time)
            {

                //Note 3 possible condition for event


                //POSSIBLE 1
                //7th occupied by 7th Lord
                bool _7thLordIn7th = Calculate.IsHouseLordInHouse(HouseName.House7, HouseName.House7, time);

                //Navamsa 7th occupied Navamsa 7th Lord
                bool navamsa7thLordInNavamsa7th = IsNavamsa7thLordInNavamsa7thOccuring(time);

                if (_7thLordIn7th && navamsa7thLordInNavamsa7th)
                {
                    //event is occuring
                    return true;
                }


                //POSSIBLE 2
                //7th occupied by Navamsa 7th Lord
                bool navamsa7thLordIn7th = IsNavamsa7thLordIn7th(time);

                //Navamsa 7th occupied by 7th Lord
                bool _7thLordInNavamsa7th = Is7thLordInNavamsa7thOccuring(time);

                if (navamsa7thLordIn7th && _7thLordInNavamsa7th)
                {
                    //event is occuring
                    return true;
                }


                //POSSIBLE 3
                //lord of 7th should aspect Navamsa 7th
                bool lordOf7thAspectsNavamsa7th = Is7thLordAspectingNavamsa7thOccuring(time);

                //lord of Navamsa Lagna should aspect Lagna
                bool lordOfNavamsaLagnaAspectsLagna = IsNavamsa7thLordAspecting7th(time);

                if (lordOf7thAspectsNavamsa7th && lordOfNavamsaLagnaAspectsLagna)
                {
                    //event is occuring
                    return true;
                }

                //if no above condition met, event not occuring
                return false;
            }

            bool IsNavamsaLagnaLordAspectingLagna(Time time)
            {
                //1.0 get navamsa lagna lord
                //1.1 get navamsa lagna sign
                var navamsaLagnaSign = Calculate.HouseNavamsaSign(HouseName.House1, time);

                var navamsaLagnaLord = Calculate.LordOfZodiacSign(navamsaLagnaSign);

                //2.0 get signs navamsa lagna lord is aspecting
                var signsNavamsaLagnaLordIsAspecting =
                    Calculate.SignsPlanetIsAspecting(navamsaLagnaLord, time);

                //3.0 get sign of lagna
                var lagnaSign = Calculate.HouseSignName(HouseName.House1, time);

                //4.0 check if lagna is in one of the signs navamsa lagna lord is aspecting
                if (signsNavamsaLagnaLordIsAspecting.Contains(lagnaSign))
                {
                    //event is occuring
                    return true;
                }
                else
                {
                    return false;
                }

            }

            bool IsNavamsa7thLordAspecting7th(Time time)
            {
                //1.0 get navamsa 7th lord
                //1.1 get navamsa 7th sign
                var navamsa7thSign = Calculate.HouseNavamsaSign(HouseName.House7, time);

                var navamsa7thLord = Calculate.LordOfZodiacSign(navamsa7thSign);

                //2.0 get signs navamsa 7th lord is aspecting
                var signsNavamsa7thLordIsAspecting =
                    Calculate.SignsPlanetIsAspecting(navamsa7thLord, time);

                //3.0 get sign of 7th
                var _7thSign = Calculate.HouseSignName(HouseName.House7, time);

                //4.0 check if 7th is in one of the signs navamsa 7th lord is aspecting
                if (signsNavamsa7thLordIsAspecting.Contains(_7thSign))
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            bool IsLagnaLordAspectingNavamsaLagnaOccuring(Time time)
            {
                //1.0 gets sign navamsa lord is aspecting
                //get lagna lord (house 1)
                var lagnaLord = Calculate.LordOfHouse(HouseName.House1, time);

                //get signs lagna lord is aspecting
                var signsLagnaLordIsAspecting = Calculate.SignsPlanetIsAspecting(lagnaLord, time);

                //2.0 get navamsa lagna sign
                //get navamsa lagna at house 1 longitude
                var navamsaLagnaSign = Calculate.HouseNavamsaSign(HouseName.House1, time);

                //3.0
                //check if navamsa lagna is one of the signs lagna lord is aspecting
                if (signsLagnaLordIsAspecting.Contains(navamsaLagnaSign))
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            bool Is7thLordAspectingNavamsa7thOccuring(Time time)
            {
                //1.0 gets sign navamsa lord is aspecting
                //get 7th lord
                var _7thLord = Calculate.LordOfHouse(HouseName.House7, time);

                //get signs 7th lord is aspecting
                var signs7thLordIsAspecting = Calculate.SignsPlanetIsAspecting(_7thLord, time);

                //2.0 get navamsa 7th sign
                var navamsa7thSign = Calculate.HouseNavamsaSign(HouseName.House7, time);

                //3.0
                //check if navamsa 7th is one of the signs 7th lord is aspecting
                if (signs7thLordIsAspecting.Contains(navamsa7thSign))
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            bool IsNavamsaLagnaLordInLagna(Time time)
            {
                //1.0 get navamsa lagna sign
                //get navamsa lagna at house 1 longitude
                var navamsaLagnaSign = Calculate.HouseNavamsaSign(HouseName.House1, time);

                //2.0 Get navamsa lagna lord's current sign
                //get navamsa lagna lord (planet)
                var navamsaLagnaLord = Calculate.LordOfZodiacSign(navamsaLagnaSign);

                //get ordinary sign of navamsa lagna lord
                var ordinarySignOfNavamsaLagnaLord = Calculate.PlanetZodiacSign(navamsaLagnaLord, person.BirthTime).GetSignName();

                //3.0 Get sign of house 1
                var house1Sign = Calculate.HouseSignName(HouseName.House1, time);

                //check if house 1 sign is same sign as the one navamsa lagna lord is in
                if (house1Sign == ordinarySignOfNavamsaLagnaLord)
                {
                    //event occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            /// <summary>
            /// TODO : Needs validation
            /// </summary>
            bool IsNavamsa7thLordIn7th(Time time)
            {
                //1.0 get navamsa 7th sign
                var navamsa7thSign = Calculate.HouseNavamsaSign(HouseName.House7, time);

                //2.0 Get navamsa 7th lord's current sign
                //get navamsa 7th lord (planet)
                var navamsa7thLord = Calculate.LordOfZodiacSign(navamsa7thSign);

                //get ordinary sign of navamsa 7th lord
                var ordinarySignOfNavamsa7thLord = Calculate.PlanetZodiacSign(navamsa7thLord, person.BirthTime).GetSignName();

                //3.0 Get sign of house 7
                var house7Sign = Calculate.HouseSignName(HouseName.House7, time);

                //check if house 7 sign is same sign as the one navamsa 7th lord is in
                if (house7Sign == ordinarySignOfNavamsa7thLord)
                {
                    //event occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            bool IsLagnaLordInNavamsaLagnaOccuring(Time time)
            {
                //1.0 Get
                //get lord of house 1 (lagna)
                var lagnaLord = Calculate.LordOfHouse(HouseName.House1, time);

                //get navamsa sign of lagna lord
                var navamsaSignOfLagnaLord = Calculate.PlanetNavamsaSign(lagnaLord, time);

                //2.0 get navamsa lagna sign
                //get navamsa lagna at house 1 longitude
                var navamsaLagnaSign = Calculate.HouseNavamsaSign(HouseName.House1, time);

                //3.0 check if lagna lord in navamsa lagna sign
                if (navamsaSignOfLagnaLord == navamsaLagnaSign)
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            bool Is7thLordInNavamsa7thOccuring(Time time)
            {
                //1.0 Get
                //get lord of house 7
                var _7thLord = Calculate.LordOfHouse(HouseName.House7, time);

                //get navamsa sign of 7th lord
                var navamsaSignOf7thLord = Calculate.PlanetNavamsaSign(_7thLord, time);

                //2.0 get navamsa 7th sign
                var navamsa7thSign = Calculate.HouseNavamsaSign(HouseName.House7, time);

                //3.0 check if 7th lord in navamsa 7th sign
                if (navamsaSignOf7thLord == navamsa7thSign)
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }

            }

            bool IsNavamsaLagnaLordInNavamsaLagnaOccuring(Time time)
            {

                //1.0 get navamsa lagna sign
                //get navamsa lagna at house 1 longitude
                var navamsaLagnaSign = Calculate.HouseNavamsaSign(HouseName.House1, time);

                //2.0 Get navamsa lagna lord's current sign
                //get navamsa lagna lord (planet)
                var navamsaLagnaLord = Calculate.LordOfZodiacSign(navamsaLagnaSign);

                //get navamsa sign of navamsa lagna lord
                var navamsaSignOfNavamsaLagnaLord = Calculate.PlanetNavamsaSign(navamsaLagnaLord, time);

                //3.0
                //check if lagna lord is in navamsa lagna sign
                if (navamsaSignOfNavamsaLagnaLord == navamsaLagnaSign)
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }
            }

            bool IsNavamsa7thLordInNavamsa7thOccuring(Time time)
            {

                //1.0 get 7th house navamsa sign
                var navamsa7thSign = Calculate.HouseNavamsaSign(HouseName.House7, time);

                //2.0 Get navamsa 7th lord's current sign
                //get navamsa 7th lord (planet)
                var navamsa7thLord = Calculate.LordOfZodiacSign(navamsa7thSign);

                //get navamsa sign of navamsa 7th lord
                var navamsaSignOfNavamsa7thLord = Calculate.PlanetNavamsaSign(navamsa7thLord, time);

                //3.0
                //check if 7th lord is in navamsa 7th sign
                if (navamsaSignOfNavamsa7thLord == navamsa7thSign)
                {
                    //event is occuring
                    return true; ;
                }
                else
                {
                    return false;
                }
            }

        }

        [EventCalculator(EventName.LagnaThyajya)]
        public static CalculatorResult IsLagnaThyajyaOccuring(Time time, Person person)
        {

            //get house 1 middle longitude
            var house1MiddleLongitude = Calculate.HouseLongitude(HouseName.House1, time).GetMiddleLongitude();

            //get zodiac sign at lagna (middle longitude)
            var house1ZodiacSign = Calculate.ZodiacSignAtLongitude(house1MiddleLongitude);

            var house1SignName = house1ZodiacSign.GetSignName();
            var house1DegreesInSign = house1ZodiacSign.GetDegreesInSign().TotalDegrees;

            //In Aries, Taurus, Sagittarius and Virgo, the first three degrees should be
            // avoided as it is supposed to be in the nature of a serpent (bhujanga)
            // and hence destructive.
            if (house1SignName == ZodiacName.Aries || house1SignName == ZodiacName.Taurus ||
                house1SignName == ZodiacName.Sagittarius || house1SignName == ZodiacName.Virgo)
            {
                if (house1DegreesInSign >= 0 && house1DegreesInSign < 4)
                {
                    return CalculatorResult.IsOccuring();
                }
                else
                {
                    return CalculatorResult.NotOccuring();
                }

            }


            //In regard to Pisces, Capricorn, Cancer and Scorpio, the last (three degrees) (27 to 30)
            //has to be avoided as it is supposed to be presided over by the evil force of Rahu.
            if (house1SignName == ZodiacName.Pisces || house1SignName == ZodiacName.Capricorn ||
                house1SignName == ZodiacName.Cancer || house1SignName == ZodiacName.Scorpio)
            {
                if (house1DegreesInSign >= 27 && house1DegreesInSign <= 30)
                {
                    return CalculatorResult.IsOccuring();
                }
                else
                {
                    return CalculatorResult.NotOccuring();
                }

            }

            // The middle half ghati (13°30' to 16° 30') should be rejected with
            // regard to Gemini, Libra, Leo and Aquarius
            // as it is ruled by an evil force termed Gridhra.
            if (house1SignName == ZodiacName.Gemini || house1SignName == ZodiacName.Libra ||
                house1SignName == ZodiacName.Leo || house1SignName == ZodiacName.Aquarius)
            {
                if (house1DegreesInSign >= 13.50 && house1DegreesInSign <= 16.50)
                {
                    return CalculatorResult.IsOccuring();
                }
                else
                {
                    return CalculatorResult.NotOccuring();
                }

            }

            //if no condition above met, event not occuring
            return CalculatorResult.NotOccuring();

        }

        [EventCalculator(EventName.NotAuspiciousDay)]
        public static CalculatorResult IsNotAuspiciousDayOccuring(Time time, Person person)
        {
            //Tuesday and Saturday should be avoided for all good and-auspicious works
            //Tuesday is not evil after midday

            //get current weekday
            var weekday = Calculate.DayOfWeek(time);

            //if tuesday & after midday then not occuring, end here
            if (weekday == DayOfWeek.Tuesday && isAfterMidday()) { return CalculatorResult.NotOccuring(); }

            //if tuesday or saturday event occuring
            if (weekday == DayOfWeek.Tuesday || weekday == DayOfWeek.Saturday) { return CalculatorResult.IsOccuring(); }

            //if control reaches here, not occuring
            return CalculatorResult.NotOccuring();

            //------------FUNCTIONS
            bool isAfterMidday()
            {
                //get current apparent time
                var localApparentTime = Calculate.LocalApparentTime(time);
                //get apparent noon
                var apparentNoon = Calculate.NoonTime(time);

                //if current time is past noon, then occuring
                return localApparentTime > apparentNoon;
            }
        }

        [EventCalculator(EventName.GoodPlanetsInLagna)]
        public static CalculatorResult IsGoodPlanetsInLagnaOccuring(Time time, Person person)
        {
            //Venus, Mercury or Jupiter in the ascendant will completely destroy all
            //other adverse influences

            //get planets in 1st house (ascendant)
            var planetsInLagna = Calculate.PlanetsInHouse(HouseName.House1, time);

            //list of good planets to look for
            var goodList = new List<PlanetName>() { PlanetName.Venus, PlanetName.Mercury, PlanetName.Jupiter };

            foreach (var planetName in planetsInLagna)
            {
                //if planet is good one, event is occuring
                if (goodList.Contains(planetName)) { return CalculatorResult.IsOccuring(); }
            }

            //if control reaches here, event not occuring
            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.GoodPlanetsIn11th)]
        public static CalculatorResult IsGoodPlanetsIn11thOccuring(Time time, Person person)
        {
            //The mere presence of the Moon or the Sun in the 11th will act as an
            // antidote for other evils obtaining in the horoscope

            //get planets in 11st house 
            var planetsIn11th = Calculate.PlanetsInHouse(HouseName.House11, time);

            //list of good planets to look for
            var goodList = new List<PlanetName>() { PlanetName.Moon, PlanetName.Sun };

            foreach (var planetName in planetsIn11th)
            {
                //if planet is found good list, event is occuring
                if (goodList.Contains(planetName)) { return CalculatorResult.IsOccuring(); }
            }

            //if control reaches here, event not occuring
            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.GoodPlanetsInKendra)]
        public static CalculatorResult IsGoodPlanetsInKendraOccuring(Time time, Person person)
        {
            //Jupiter or Venus in a kendra (quadrant/angles) and malefics in 3, 6 or 11
            //will remove all the flaws arising on account of unfavourable weekday,
            //constellation, lunar day and yoga.

            //Note: Planets in angles (kendra) generally become fairly strong


            //1
            //check if Jupiter or Venus in a kendra (quadrant)
            var planetInKendra = Calculate.IsPlanetInKendra(PlanetName.Jupiter, time) ||
                                 Calculate.IsPlanetInKendra(PlanetName.Venus, time);

            //if neither planet in kendra, end here as not occuring
            if (planetInKendra == false) { return CalculatorResult.NotOccuring(); }


            //2
            var maleficsIn3rd6th11th = isAllMaleficsIn3rd6th11th();

            //if all melefics are NOT in 3,6,11, end here as not occuring
            if (maleficsIn3rd6th11th == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here, event is occuring
            return CalculatorResult.IsOccuring();


            //---------------FUNCTIONS-----

            //returns true if all malefics is contained in 3,6 or 11th house, false otherwise
            bool isAllMaleficsIn3rd6th11th()
            {
                //get all malefic planets
                var allMalefics = Calculate.MaleficPlanetList(time);

                //go through each malefic planet and
                //make sure each is in 3, 6 or 11th house
                foreach (var malefic in allMalefics)
                {
                    var planetHouse = Calculate.HousePlanetOccupies(malefic, time);

                    //if not in 3, 6 or 11, end here as not occuring
                    if (!(planetHouse == HouseName.House3 || planetHouse == HouseName.House6 || planetHouse == HouseName.House11)) { return false; }
                }

                //if control reaches here, than it is occuring
                return true;
            }

        }

        [EventCalculator(EventName.GoodRullingConstellation)]
        public static CalculatorResult IsGoodRullingConstellationOccuring(Time time, Person person)
        {
            //Pushya rulling is a constellation par excellence that could
            //be universally employed for all purposes, excepting of course marriage

            //1. Constellation
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Pushyami;

            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.BadRullingConstellation)]
        public static CalculatorResult IsBadRullingConstellationOccuring(Time time, Person person)
        {

            //Certain constellations, apart from their being harmonious or otherwise disposed
            //with reference to one's own Janma Nakshatra, should be avoided for
            //certain specific purposes on account of their inherent evil natures. In
            //fact, Bharani is condemned for all good work and it has to be
            //scrupulously avoided for all good work.

            //When a constellation like
            //Bharani(beta Arietis) is held to be constitutionally unfit for certain types
            //of elections; it means that the vibrations emanating from it are
            //destructive in character

            //Of the several Nakshatras, Bharani and Krittika
            // should be avoided for all auspicious works as these two are said to be
            // presided over by the god of death (Yama) and the god of fire (Agni)
            // respectively. In urgent cases if the Lagna could be fortified, the dosha
            // due to nakshatra may get neutralised. The last parts of Aslesha,
            // Jyeshta and Revati should also be avoided.


            //1. Constellation
            //get ruling constellation
            var rulingConstellation = Calculate.MoonConstellation(time);
            var rulingConstellationName = rulingConstellation.GetConstellationName();


            if (rulingConstellationName == ConstellationName.Bharani || rulingConstellationName == ConstellationName.Krithika)
            {
                //event occuring
                return CalculatorResult.IsOccuring();
            }


            if (rulingConstellationName == ConstellationName.Aslesha || rulingConstellationName == ConstellationName.Jyesta
                                                                     || rulingConstellationName == ConstellationName.Revathi)
            {
                if (rulingConstellation.GetQuarter() == 4)
                {
                    //event occuring
                    return CalculatorResult.IsOccuring();
                }

            }



            //if control reaches here then event is NOT occuring
            return CalculatorResult.NotOccuring();

        }

        [EventCalculator(EventName.SiddhaYoga)]
        public static CalculatorResult IsSiddhaYogaOccuring(Time time, Person person)
        {
            var siddhaYogaMonday = IsSiddhaYogaMondayOccuring(time, person);
            var siddhaYogaTuesday = IsSiddhaYogaTuesdayOccuring(time, person);
            var siddhaYogaWednesday = IsSiddhaYogaWednesdayOccuring(time, person);
            var siddhaYogaThursday = IsSiddhaYogaThursdayOccuring(time, person);
            var siddhaYogaFriday = IsSiddhaYogaFridayOccuring(time, person);
            var siddhaYogaSaturday = IsSiddhaYogaSaturdayOccuring(time, person);
            var siddhaYogaSunday = IsSiddhaYogaSundayOccuring(time, person);

            if (siddhaYogaMonday || siddhaYogaTuesday || siddhaYogaWednesday || siddhaYogaThursday ||
                siddhaYogaFriday || siddhaYogaSaturday || siddhaYogaSunday)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }


            //NOTE : All sidhha yoga since same, under 1 name
            bool IsSiddhaYogaSundayOccuring(Time time, Person person)
            {
                //Sunday coinciding with the 1st, 4th, 6th, 7th or 12th lunar day and ruled
                // by the constellations Pushya, Hasta, Uttara, Uttarashadha, Moola,
                // Sravana or Uttarabhadra gives rise to Siddha Yoga.
                //

                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day
                var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    //check lunar day number
                    if (lunarDayNumber == 1 || lunarDayNumber == 4 ||
                        lunarDayNumber == 6 || lunarDayNumber == 7 ||
                        lunarDayNumber == 12)
                    {
                        //check ruling constellation name
                        if (rulingConstellationName == ConstellationName.Pushyami ||
                            rulingConstellationName == ConstellationName.Hasta ||
                            rulingConstellationName == ConstellationName.Uttara ||
                            rulingConstellationName == ConstellationName.Uttarashada ||
                            rulingConstellationName == ConstellationName.Moola ||
                            rulingConstellationName == ConstellationName.Sravana ||
                            rulingConstellationName == ConstellationName.Uttarashada)
                        {
                            return true;
                        }
                    }
                }

                //if none of the conditions above are met return false
                return false;
            }

            bool IsSiddhaYogaMondayOccuring(Time time, Person person)
            {
                //Monday identical with the 2nd, 7th or 12th lunar day and with the
                // constellations Rohini, Mrigasira, Punarvasu, Chitta, Sravana,
                // Satabhisha, Dhanishta or Poorvabhadra produces Siddha Yoga.

                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day
                var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Monday)
                {
                    //check lunar day number
                    if (lunarDayNumber == 2 || lunarDayNumber == 7 ||
                        lunarDayNumber == 12)
                    {
                        //check ruling constellation name
                        if (rulingConstellationName == ConstellationName.Rohini ||
                            rulingConstellationName == ConstellationName.Mrigasira ||
                            rulingConstellationName == ConstellationName.Punarvasu ||
                            rulingConstellationName == ConstellationName.Chitta ||
                            rulingConstellationName == ConstellationName.Sravana ||
                            rulingConstellationName == ConstellationName.Satabhisha ||
                            rulingConstellationName == ConstellationName.Dhanishta ||
                            rulingConstellationName == ConstellationName.Poorvabhadra)
                        {
                            return true;
                        }
                    }
                }

                //if none of the conditions above are met return false
                return false;
            }

            bool IsSiddhaYogaTuesdayOccuring(Time time, Person person)
            {
                //Tuesday falling on a day ruled by Aswini, Mrigasira, Chitta, Anuradha,
                // Moola, Uttara, Dhanishta or Poorvabhadra gives rise to Siddha Yoga.
                //OR
                //Tuesday coinciding with Jaya (3rd 8th and 13th lunar days)

                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day
                var lunarDayGroup = Calculate.LunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Tuesday)
                {
                    //check ruling constellation name
                    if (rulingConstellationName == ConstellationName.Aswini ||
                        rulingConstellationName == ConstellationName.Mrigasira ||
                        rulingConstellationName == ConstellationName.Chitta ||
                        rulingConstellationName == ConstellationName.Anuradha ||
                        rulingConstellationName == ConstellationName.Moola ||
                        rulingConstellationName == ConstellationName.Uttara ||
                        rulingConstellationName == ConstellationName.Dhanishta ||
                        rulingConstellationName == ConstellationName.Poorvabhadra)
                    {
                        return true;
                    }

                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Jaya)
                    {
                        return true;
                    }

                }

                //if none of the conditions above are met return false
                return false;
            }

            bool IsSiddhaYogaWednesdayOccuring(Time time, Person person)
            {
                //Wednesday coinciding with Bhadra and Jaya and with the constellations
                // Rohini, Mrigasira, Aridra, Uttara, Uttarashadha or Anuradha generates
                // Siddha Yoga.
                //OR
                //Wednesday identical with Bhadra (2nd, 7th and 12th lunar days),

                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day
                var lunarDayGroup = Calculate.LunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Wednesday)
                {
                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Bhadra || lunarDayGroup == LunarDayGroup.Jaya)
                    {
                        //check ruling constellation name
                        if (rulingConstellationName == ConstellationName.Rohini ||
                            rulingConstellationName == ConstellationName.Mrigasira ||
                            rulingConstellationName == ConstellationName.Aridra ||
                            rulingConstellationName == ConstellationName.Uttara ||
                            rulingConstellationName == ConstellationName.Uttarashada ||
                            rulingConstellationName == ConstellationName.Anuradha)
                        {
                            return true;
                        }
                    }

                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Bhadra)
                    {
                        return true;
                    }
                }

                //if none of the conditions above are met return false
                return false;
            }

            bool IsSiddhaYogaThursdayOccuring(Time time, Person person)
            {
                //Thursday identical with the 4th, 5th, 7th, 9th, 13th or 14th lunar day and
                // with tne asterisms Makha, Pushya, Punarvasu, Swati. Poorvashadha,
                // Poorvabhadra, Revati or Aswini gives rise to Siddha Yoga.
                //OR
                //Thursday falling on 5th, 10th or 15th(Poorna)

                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day
                var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();
                //get lunar day
                var lunarDayGroup = Calculate.LunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Thursday)
                {
                    //check lunar day number
                    if (lunarDayNumber == 4 || lunarDayNumber == 5 ||
                        lunarDayNumber == 7 || lunarDayNumber == 9 ||
                        lunarDayNumber == 13 || lunarDayNumber == 14)
                    {
                        //check ruling constellation name
                        if (rulingConstellationName == ConstellationName.Makha ||
                            rulingConstellationName == ConstellationName.Pushyami ||
                            rulingConstellationName == ConstellationName.Punarvasu ||
                            rulingConstellationName == ConstellationName.Swathi ||
                            rulingConstellationName == ConstellationName.Poorvashada ||
                            rulingConstellationName == ConstellationName.Poorvabhadra ||
                            rulingConstellationName == ConstellationName.Revathi ||
                            rulingConstellationName == ConstellationName.Aswini)
                        {
                            return true;
                        }
                    }

                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Purna)
                    {
                        return true;
                    }

                }

                //if none of the conditions above are met return false
                return false;
            }

            bool IsSiddhaYogaFridayOccuring(Time time, Person person)
            {
                //Friday ruled by Aswini, Bharani, Aridra, Uttara, Chitta, Swati,
                // Poorvashadha or Revati coinciding with Nanda and Bhadra constitutes
                // this beneficial yoga.
                // OR
                //A Friday coinciding with Nanda (1st, 6th and 11th lunar days)


                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day group
                var lunarDayGroup = Calculate.LunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Friday)
                {
                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Bhadra || lunarDayGroup == LunarDayGroup.Nanda)
                    {
                        //check ruling constellation name
                        if (rulingConstellationName == ConstellationName.Aswini ||
                            rulingConstellationName == ConstellationName.Bharani ||
                            rulingConstellationName == ConstellationName.Aridra ||
                            rulingConstellationName == ConstellationName.Uttara ||
                            rulingConstellationName == ConstellationName.Chitta ||
                            rulingConstellationName == ConstellationName.Swathi ||
                            rulingConstellationName == ConstellationName.Poorvashada ||
                            rulingConstellationName == ConstellationName.Revathi)
                        {
                            return true;
                        }
                    }

                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Nanda)
                    {
                        return true;
                    }
                }

                //if none of the conditions above are met return false
                return false;
            }

            bool IsSiddhaYogaSaturdayOccuring(Time time, Person person)
            {
                //Saturday falling on a day ruled by Swati, Rohini, Visakha, Anuradha,
                // Dhanishta or Satabhisha and with lunar days Bhadra and Riktha
                // generates the same auspicious yoga.
                //OR
                //Saturday falling on a Riktha tithi(4th, 9th and 14th lunar days)

                //get weekday
                var dayOfWeek = Calculate.DayOfWeek(time);
                //get lunar day
                var lunarDayGroup = Calculate.LunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

                //check day of week
                if (dayOfWeek == DayOfWeek.Saturday)
                {
                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Bhadra || lunarDayGroup == LunarDayGroup.Rikta)
                    {
                        //check ruling constellation name
                        if (rulingConstellationName == ConstellationName.Swathi ||
                            rulingConstellationName == ConstellationName.Rohini ||
                            rulingConstellationName == ConstellationName.Vishhaka ||
                            rulingConstellationName == ConstellationName.Anuradha ||
                            rulingConstellationName == ConstellationName.Dhanishta ||
                            rulingConstellationName == ConstellationName.Satabhisha)
                        {
                            return true;
                        }
                    }

                    //check lunar day group
                    if (lunarDayGroup == LunarDayGroup.Rikta)
                    {
                        return true;
                    }
                }

                //if none of the conditions above are met return false
                return false;
            }
        }

        [EventCalculator(EventName.AmritaSiddhaYoga)]
        public static CalculatorResult IsAmritaSiddhaYogaOccuring(Time time, Person person)
        {
            //Sunday to Saturday respectively coinciding with the constellations
            // Hasta(Sunday), Sravana(Monday), Aswini(Tuesday), Anuradha(Wednesday), Pushya(Thursday), Revati(Friday) and Rohini(Saturday) will give
            // rise to Amita Siddha Yoga.

            //get weekday
            var dayOfWeek = Calculate.DayOfWeek(time);
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check day of week
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Hasta)
                {
                    return CalculatorResult.IsOccuring();
                }
            }

            //check day of week
            if (dayOfWeek == DayOfWeek.Monday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Sravana)
                {
                    return CalculatorResult.IsOccuring();
                }
            }

            //check day of week
            if (dayOfWeek == DayOfWeek.Tuesday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Aswini)
                {
                    return CalculatorResult.IsOccuring();
                }
            }

            //check day of week
            if (dayOfWeek == DayOfWeek.Wednesday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Anuradha)
                {
                    return CalculatorResult.IsOccuring();
                }
            }

            //check day of week
            if (dayOfWeek == DayOfWeek.Thursday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Pushyami)
                {
                    return CalculatorResult.IsOccuring();
                }
            }

            //check day of week
            if (dayOfWeek == DayOfWeek.Friday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Revathi)
                {
                    return CalculatorResult.IsOccuring();
                }
            }

            //check day of week
            if (dayOfWeek == DayOfWeek.Saturday)
            {
                //check ruling constellation name
                if (rulingConstellationName == ConstellationName.Rohini)
                {
                    return CalculatorResult.IsOccuring();
                }
            }


            //if none of the conditions above are met return false
            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.PanchangaSuddhi)]
        public static CalculatorResult IsPanchangaSuddhiOccuring(Time time, Person person)
        {
            //TODO Needs to be fixed

            //We have already said that a Panchanga
            // consists of tithi, vara, nakshatra. yoga and karana. All these must be
            // auspicious.
            // - In regard to lunar days, the 4th, 6th, 8th, 12th and 14th, full
            // and new moon days should be avoided.

            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            if (lunarDayNumber == 4 || lunarDayNumber == 6
                || lunarDayNumber == 8 || lunarDayNumber == 12
                || lunarDayNumber == 14 || lunarDayNumber == 15 || lunarDayNumber == 1)
            {
                return CalculatorResult.NotOccuring();
            }

            //
            // - In regard to vara, Thursday and Friday are held to be suitable for all works. Tuesday, is to be generally
            // avoided except when it happens to be the 10th, 12th or 16th day of the
            // child's birth when the child's Namakarana (baptising or giving name)
            // may be performed.
            //

            //get weekday
            var dayOfWeek = Calculate.DayOfWeek(time);

            if (dayOfWeek != DayOfWeek.Thursday || dayOfWeek != DayOfWeek.Friday)
            {
                return CalculatorResult.NotOccuring();
            }

            // - Of the several Nakshatras, Bharani and Krittika
            // should be avoided for all auspicious works as these two are said to be
            // presided over by the god of death (Yama) and the god of fire (Agni)
            //respectively. In urgent cases if the Lagna could be fortified, the dosha
            // due to nakshatra may get neutralised. The last parts of Aslesha,
            // Jyeshta and Revati should also be avoided.

            //get ruling constellation
            var rulingConstellation = Calculate.MoonConstellation(time);
            var rulingConstellationName = rulingConstellation.GetConstellationName();

            if (rulingConstellationName == ConstellationName.Bharani || rulingConstellationName == ConstellationName.Krithika)
            {
                return CalculatorResult.NotOccuring();
            }

            if (rulingConstellationName == ConstellationName.Aslesha || rulingConstellationName == ConstellationName.Jyesta
                || rulingConstellationName == ConstellationName.Revathi)
            {
                if (rulingConstellation.GetQuarter() == 4)
                {
                    return CalculatorResult.NotOccuring();
                }

            }


            //
            // - Coming to the Yoga the 6th (Atiganda). 9th (Soola). 10th (Ganda), 17th (Vyatipata)
            // and 27th (Vydhruti) have deleterious effects upon events which are
            // started or commenced under them. -

            var yoga = Calculate.NithyaYoga(time);

            if (yoga.Name == NithyaYogaName.Atiganda || yoga.Name == NithyaYogaName.Soola
                || yoga.Name == NithyaYogaName.Ganda || yoga.Name == NithyaYogaName.Vyatapata
                || yoga.Name == NithyaYogaName.Vaidhriti)
            {
                return CalculatorResult.NotOccuring();
            }

            //
            // - Karana chosen must be appropriate to the election in view.
            // Thus Bava is auspicious for starting works of permanent importance
            // while Thaithula is propitious for marriage.
            // Bhadra(vishti) is unfit for any good work but is eminently suitable for violent and cruel deeds.
            // For getting initiation into kshudra mantras Sakuni Havana is propitious.

            var karana = Calculate.Karana(time);
            //all karana mentioned are included
            if (karana != Karana.Bava || karana != Karana.Taitula || karana != Karana.Visti || karana != Karana.Sakuna)
            {
                return CalculatorResult.NotOccuring();
            }

            // Therefore, Panchanga Suddhi means a good lunar day, a beneficial
            // weekday, an auspicious constellation, a good yoga and a fertilising
            // Karana.

            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.BadNithyaYoga)]
        public static CalculatorResult IsBadNithyaYogaOccuring(Time time, Person person)
        {
            // - Coming to the Yoga the 6th (Atiganda). 9th (Soola). 10th (Ganda), 17th (Vyatipata)
            // and 27th (Vydhruti) have deleterious effects upon events which are
            // started or commenced under them. -

            var yoga = Calculate.NithyaYoga(time);

            if (yoga.Name == NithyaYogaName.Atiganda || yoga.Name == NithyaYogaName.Soola
                                            || yoga.Name == NithyaYogaName.Ganda || yoga.Name == NithyaYogaName.Vyatapata
                                            || yoga.Name == NithyaYogaName.Vaidhriti)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }

        }

        [EventCalculator(EventName.SuryaSankramana)]
        public static CalculatorResult IsSuryaSankramanaOccuring(Time time, Person person)
        {
            //Surya Sankramana. - The 2nd great evil is Surya Sankramana or the
            // solar ingress into different zodiacal signs. When the Sun is about to
            // leave one sign and enter another there seem to occur certain
            // disturbances in the organisation of the solar forces and such times are
            // not recommended for any good work. On the contrary, they are held to
            // be propitious for meditation, initiation into secret mantras and
            // performance of certain religious rites which are held to purify not only
            // the bodily electrical discharges but also the mental currents. Sixteen
            // ghatis (6 hours 24 minutes) both before and after the entry of the Sun
            // into a new sign should be rejected for all new works.

            //hours to reject (6 hours 24 minutes / 6.4 hours)
            const double hoursToReject = 6.4;

            //get time sun entered into current sign
            var timeSunEnteredCurrentSign = Calculate.TimeSunEnteredCurrentSign(time);

            //get hours after entry into sign
            var hoursAfterEntryIntoSign = time.Subtract(timeSunEnteredCurrentSign).TotalHours;

            //if hours after entry is less than or equals hours to reject
            if (hoursAfterEntryIntoSign <= hoursToReject)
            {
                //return true
                return CalculatorResult.IsOccuring();
            }


            //get time sun will leave current sign
            var timeSunLeavesCurrentSign = Calculate.TimeSunLeavesCurrentSign(time);

            //get hours before entry into new sign
            var hoursBeforeEntryIntoSign = timeSunLeavesCurrentSign.Subtract(time).TotalHours;

            //if hours before entry is less than or equals hours to reject
            if (hoursBeforeEntryIntoSign <= hoursToReject)
            {
                //return true
                return CalculatorResult.IsOccuring();
            }


            //if no conditions met return false
            return CalculatorResult.NotOccuring();
        }

        [EventCalculator(EventName.Papashadvargas)]
        public static CalculatorResult IsPapashadvargasOccuring(Time time, Person person)
        {
            ////TODO ALWAYS ON
            ////DISABLE FOR NOW
            //return CalculatorResult.NotOccuring();


            //Papashadvargs. - Malefics should not be strong in shadvargas in an election chart.
            //This event idicates that malefics are strong in shadvargas

            //TODO Note : It is possible that overall strenght of a malefic is considered,
            //            for now not 100% sure. Current method of using shadvarga bala calculation
            //            seems workable. Further verification is in order.
            //            Shadvarga bala uses malefic's relationship with sign to determine strenght

            //get all malefic planets
            var allMalefics = Calculate.MaleficPlanetList(time);

            //rahu & ketu are not included
            //TODO needs checking
            allMalefics.RemoveAll(name => name == PlanetName.Rahu || name == PlanetName.Ketu);

            //go through each malefic planet and
            //check if is strong in shadvarga
            foreach (var malefic in allMalefics)
            {
                //check if planet is strong
                var isStrong = Calculate.IsPlanetStrongInShadbala(malefic, time);

                //if any one malefic is strong, end here as occuring
                if (isStrong) { return CalculatorResult.IsOccuring(); }

            }

            //if control reaches here, than it is NOT occuring
            return CalculatorResult.NotOccuring();

        }

        #endregion

        #region HAIR AND NAIL

        [EventCalculator(EventName.GoodHairCutting)]
        public static CalculatorResult IsGoodHairCuttingOccuring(Time time, Person person)
        {
            //1. Shaving may be had in the constellation of Pushya, Punarvasu, Revat,
            //  Haste, Sravana, Dhanishta, Mrigasira, Aswini, Chitta, Jyeshta,
            //  Satabhisha and Swati

            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var correctConstellation = rulingConstellationName == ConstellationName.Pushyami ||
                    rulingConstellationName == ConstellationName.Punarvasu ||
                    rulingConstellationName == ConstellationName.Revathi ||
                    rulingConstellationName == ConstellationName.Hasta ||
                    rulingConstellationName == ConstellationName.Sravana ||
                    rulingConstellationName == ConstellationName.Dhanishta ||
                    rulingConstellationName == ConstellationName.Mrigasira ||
                    rulingConstellationName == ConstellationName.Aswini ||
                    rulingConstellationName == ConstellationName.Chitta ||
                    rulingConstellationName == ConstellationName.Jyesta ||
                    rulingConstellationName == ConstellationName.Satabhisha ||
                    rulingConstellationName == ConstellationName.Swathi;

            //if not correct constellation, end here as not occuring
            if (correctConstellation == false) { return CalculatorResult.NotOccuring(); }


            //2. 4th, 6th and 14th lunar days as also New Moon
            //   and Full Moon days are not desirable

            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //lunar days to avoid
            var avoidLunarDay = lunarDayNumber == 4 ||
                                  lunarDayNumber == 6 ||
                                  lunarDayNumber == 14 ||
                                  lunarDayNumber == 1 || //new moon
                                  lunarDayNumber == 15; //full moon

            //if the lunar days to avoid are occuring, end here as not occuring
            if (avoidLunarDay == true) { return CalculatorResult.NotOccuring(); }


            //if conrtol reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodNailCutting)]
        public static CalculatorResult IsGoodNailCuttingOccuring(Time time, Person person)
        {
            //Avoid Fridays and Saturdays - 


            //get weekday
            var dayOfWeek = Calculate.DayOfWeek(time);

            //check if days to avoid are occuring
            var avoidDays = dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday;

            //if wrong days then, end here as not occuring
            if (avoidDays == true) { return CalculatorResult.NotOccuring(); }



            //Avoid the 8th, 9th, 14th lunar
            //days as well as New and Full Moon days.

            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //lunar days to avoid
            var avoidLunarDay = lunarDayNumber == 8 ||
                                lunarDayNumber == 9 ||
                                lunarDayNumber == 14 ||
                                lunarDayNumber == 1 || //new moon
                                lunarDayNumber == 15; //full moon

            //if the lunar days to avoid are occuring, end here as not occuring
            if (avoidLunarDay == true) { return CalculatorResult.NotOccuring(); }


            //if conrtol reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }


        #endregion

        #region RULLING CONSTLLATION

        [EventCalculator(EventName.FixedConstellationRuling)]
        public static CalculatorResult IsFixedConstellationRulingOccuring(Time time, Person person)
        {
            //Rohini, Uttara, -Uttamsliadka and Ufctarabhadra
            //are supposed to be fixed constellations
            //and they are favourable' for coronations, laying"
            //the foundation of cities, sowing operations,,
            //planting trees and other permanent things.


            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var fixedConstellation = rulingConstellationName == ConstellationName.Rohini ||
                                       rulingConstellationName == ConstellationName.Uttara ||
                                       rulingConstellationName == ConstellationName.Uttarashada ||
                                       rulingConstellationName == ConstellationName.Uttarabhadra;

            //if not correct constellation, end here as not occuring
            if (fixedConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if conrtol reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.SoftConstellationRuling)]
        public static CalculatorResult IsSoftConstellationRulingOccuring(Time time, Person person)
        {
            //Chitta, Anuradha, Mrigasira and Revati are soft constellations. They are
            //good for wearing new apparel, learning dancing, music and fine arts,
            //sexual union and performance of auspicious ceremonies.


            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var softConstellation = rulingConstellationName == ConstellationName.Chitta ||
                                       rulingConstellationName == ConstellationName.Anuradha ||
                                       rulingConstellationName == ConstellationName.Mrigasira ||
                                       rulingConstellationName == ConstellationName.Revathi;

            //if not correct constellation, end here as not occuring
            if (softConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.LightConstellationRuling)]
        public static CalculatorResult IsLightConstellationRulingOccuring(Time time, Person person)
        {

            //Aswini, Pushya, Hasta and Abhijit are light constellations, and they can
            //be selected for putting ornamentation, pleasures and sports,
            //administering medicine, starting industries and undertaking travels.


            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var lightConstellation = rulingConstellationName == ConstellationName.Aswini ||
                                       rulingConstellationName == ConstellationName.Pushyami ||
                                       rulingConstellationName == ConstellationName.Hasta;

            //if not correct constellation, end here as not occuring
            if (lightConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.SharpConstellationRuling)]
        public static CalculatorResult IsSharpConstellationRulingOccuring(Time time, Person person)
        {
            //Moola, Jyestha, Aridra and Aslesha are sharp in nature and they are
            //favourable for incantations, invoking spirits, for imprisonment, murders,
            //and separation of friends.

            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var sharpConstellation = rulingConstellationName == ConstellationName.Moola ||
                                       rulingConstellationName == ConstellationName.Jyesta ||
                                       rulingConstellationName == ConstellationName.Aridra ||
                                       rulingConstellationName == ConstellationName.Aslesha;

            //if not correct constellation, end here as not occuring
            if (sharpConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.MovableConstellationRuling)]
        public static CalculatorResult IsMovableConstellationRulingOccuring(Time time, Person person)
        {
            //Saravana, Dhanishta, Satabhisha, Punarvasu and Swati are movable
            //stars and they are auspicious fcr acquiring vehicles, for gardening and
            //for going on procession.

            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var movableConstellation = rulingConstellationName == ConstellationName.Sravana ||
                                       rulingConstellationName == ConstellationName.Dhanishta ||
                                       rulingConstellationName == ConstellationName.Satabhisha ||
                                       rulingConstellationName == ConstellationName.Punarvasu ||
                                       rulingConstellationName == ConstellationName.Swathi;

            //if not correct constellation, end here as not occuring
            if (movableConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.DreadfulConstellationRuling)]
        public static CalculatorResult IsDreadfulConstellationRulingOccuring(Time time, Person person)
        {
            //Pubba, Poorvashadha and Poorvabhadra, Bharani and Makha are
            //dreadful stars and they are suitable for nefarious schemes, poisoning,
            //deceit, imprisonment, setting fire and other evil deeds.

            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var dreadfulConstellation = rulingConstellationName == ConstellationName.Pubba ||
                                        rulingConstellationName == ConstellationName.Poorvashada ||
                                        rulingConstellationName == ConstellationName.Poorvabhadra ||
                                        rulingConstellationName == ConstellationName.Bharani ||
                                        rulingConstellationName == ConstellationName.Makha;

            //if not correct constellation, end here as not occuring
            if (dreadfulConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.MixedConstellationRuling)]
        public static CalculatorResult IsMixedConstellationRulingOccuring(Time time, Person person)
        {
            //Krittika and Visakha are mixed constellations and during their
            //influences, works of day-to-day importance can be undertaken.

            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var mixedConstellation = rulingConstellationName == ConstellationName.Krithika ||
                                     rulingConstellationName == ConstellationName.Vishhaka;

            //if not correct constellation, end here as not occuring
            if (mixedConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }


        #endregion

        #region AGRICULTURE

        [EventCalculator(EventName.GoodAnySeedsSowing)]
        public static CalculatorResult IsGoodAnySeedsSowingOccuring(Time time, Person person)
        {
            //Sowing : Any seeds can be sown on a day ruled by Hasta,
            //Chitta, Swati, Makha, Pushyami, Uttara, Uttarashadha, Uttarabhadra,
            //Rohini, Revati, Aswini, Moola or Anuradha provided the lunar day is
            //also propitious. Choose a Lagna, owned by the planet who is lord of the
            //weekday in question.


            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Constellation
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Hasta ||
                                     rulingConstellationName == ConstellationName.Chitta ||
                                     rulingConstellationName == ConstellationName.Swathi ||
                                     rulingConstellationName == ConstellationName.Makha ||
                                     rulingConstellationName == ConstellationName.Pushyami ||
                                     rulingConstellationName == ConstellationName.Uttara ||
                                     rulingConstellationName == ConstellationName.Uttarashada ||
                                     rulingConstellationName == ConstellationName.Uttarabhadra ||
                                     rulingConstellationName == ConstellationName.Rohini ||
                                     rulingConstellationName == ConstellationName.Revathi ||
                                     rulingConstellationName == ConstellationName.Aswini ||
                                     rulingConstellationName == ConstellationName.Moola ||
                                     rulingConstellationName == ConstellationName.Anuradha;

            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingTrees)]
        public static CalculatorResult IsGoodForPlantingTreesOccuring(Time time, Person person)
        {
            //good for trees in Rohini

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Constellation
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Rohini;

            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingFlowerSeeds)]
        public static CalculatorResult IsGoodForPlantingFlowerSeedsOccuring(Time time, Person person)
        {
            //Seeds of flower plants, and fruit-bearing creepers should be sown in the asterisms in 
            //Mriyusira, Punarvasu, Hasta, Chitta, Swati, Anuradha and Revati


            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Constellation
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Mrigasira ||
                                     rulingConstellationName == ConstellationName.Punarvasu ||
                                     rulingConstellationName == ConstellationName.Hasta ||
                                     rulingConstellationName == ConstellationName.Chitta ||
                                     rulingConstellationName == ConstellationName.Swathi ||
                                     rulingConstellationName == ConstellationName.Anuradha ||
                                     rulingConstellationName == ConstellationName.Revathi;


            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingSugarcane)]
        public static CalculatorResult IsGoodForPlantingSugarcaneOccuring(Time time, Person person)
        {
            //Sugarcane in Punarvasu

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Constellation
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Punarvasu;

            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingFruitTrees)]
        public static CalculatorResult IsGoodForPlantingFruitTreesOccuring(Time time, Person person)
        {

            //if either 1 event is occuring
            if (fruitTree1() || fruitTree2())
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }

            //Seedlings of long-lived fruit trees when Jupiter is in Lagna on Thursday
            bool fruitTree1()
            {

                //1. General good yoga for planting
                var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

                //if not right, end here as not occuring
                if (rightYoga == false) { return false; }


                //3. Weekday
                //get current weekday
                var currentWeekday = Calculate.DayOfWeek(time);

                //check weekday
                var rightWeekday = currentWeekday == DayOfWeek.Thursday;

                //if not correct weekday, end here as not occuring
                if (rightWeekday == false) { return false; }


                //2. Jupiter is in Lagna
                //get planets in lagna 
                var currentPlanetsInLagna = Calculate.PlanetsInHouse(HouseName.House1, time);

                //check if jupiter is in lagna
                var rightPlanet = currentPlanetsInLagna.Contains(PlanetName.Jupiter);

                //if not correct planet, end here as not occurings
                if (rightPlanet == false) { return false; }


                //if control reaches here then event is ocuring
                return true;

            }

            //fruit trees may be planted when Sagittarius and Pisces are rising on Thursday
            bool fruitTree2()
            {
                //1. General good yoga for planting
                var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

                //if not right, end here as not occuring
                if (rightYoga == false) { return false; }


                //3. Weekday
                //get current weekday
                var currentWeekday = Calculate.DayOfWeek(time);

                //check weekday
                var rightWeekday = currentWeekday == DayOfWeek.Thursday;

                //if not correct weekday, end here as not occuring
                if (rightWeekday == false) { return false; }


                //2. Correct rising sign
                //get rising sign
                var risingSign = Calculate.HouseSignName(HouseName.House1, time);

                //check rising sign
                var rightSign = risingSign == ZodiacName.Sagittarius ||
                                risingSign == ZodiacName.Pisces;

                //if not correct sign, end here as not occuring
                if (rightSign == false) { return false; }



                //if control reaches here then event is ocuring
                return true;

            }
        }

        [EventCalculator(EventName.GoodForPlantingFlowerTrees)]
        public static CalculatorResult IsGoodForPlantingFlowerTreesOccuring(Time time, Person person)
        {
            //Seedlings of flower trees when Venus is in Lagna on Friday


            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //3. Weekday
            //get current weekday
            var currentWeekday = Calculate.DayOfWeek(time);

            //check weekday
            var rightWeekday = currentWeekday == DayOfWeek.Friday;

            //if not correct weekday, end here as not occuring
            if (rightWeekday == false) { return CalculatorResult.NotOccuring(); }


            //2. Planet is in Lagna
            //get planets in lagna 
            var currentPlanetsInLagna = Calculate.PlanetsInHouse(HouseName.House1, time);

            //check if correct planet is in lagna
            var rightPlanet = currentPlanetsInLagna.Contains(PlanetName.Venus);

            //if not correct planet, end here as not occurings
            if (rightPlanet == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingFlowers)]
        public static CalculatorResult IsGoodForPlantingFlowersOccuring(Time time, Person person)
        {
            //Seedlings of flower sown when Mars is in Lagna on Tuesday


            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //3. Weekday
            //get current weekday
            var currentWeekday = Calculate.DayOfWeek(time);

            //check weekday
            var rightWeekday = currentWeekday == DayOfWeek.Tuesday;

            //if not correct weekday, end here as not occuring
            if (rightWeekday == false) { return CalculatorResult.NotOccuring(); }


            //2. Planet is in Lagna
            //get planets in lagna 
            var currentPlanetsInLagna = Calculate.PlanetsInHouse(HouseName.House1, time);

            //check if correct planet is in lagna
            var rightPlanet = currentPlanetsInLagna.Contains(PlanetName.Mars);

            //if not correct planet, end here as not occurings
            if (rightPlanet == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingFlowerCuttings)]
        public static CalculatorResult IsGoodForPlantingFlowerCuttingsOccuring(Time time, Person person)
        {
            //Flower seeds and cuttings may be sown when Taurus or Libra rising


            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Taurus ||
                            risingSign == ZodiacName.Libra;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }



            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodForPlantingFloweringPlants)]
        public static CalculatorResult IsGoodForPlantingFloweringPlantsOccuring(Time time, Person person)
        {
            //Flowering plants in Virgo

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Virgo;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingGarlic)]
        public static CalculatorResult IsGoodForPlantingGarlicOccuring(Time time, Person person)
        {
            //Garlic in Aries

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Aries;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingPeachAndOthers)]
        public static CalculatorResult IsGoodForPlantingPeachAndOthersOccuring(Time time, Person person)
        {
            //Peach, plum, potatoes, radishes, onion sets and turnips in Taurus

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Taurus;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingTomatoesAndOthers)]
        public static CalculatorResult IsGoodForPlantingTomatoesAndOthersOccuring(Time time, Person person)
        {
            //Beans, cabbage, corn, cucumber, lettuce, melons, pumpkins, tomatoes, cauliflower, water-melons, and cereals in Cancer

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Cancer;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingGrains)]
        public static CalculatorResult IsGoodForPlantingGrainsOccuring(Time time, Person person)
        {
            //wheat, rye, barley, rice and other field crops in Libra

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Libra;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingOnionAndOthers)]
        public static CalculatorResult IsGoodForPlantingOnionAndOthersOccuring(Time time, Person person)
        {
            //Garlic and onion seeds in Scorpio

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Scorpio;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingPepperAndOthers)]
        public static CalculatorResult IsGoodForPlantingPepperAndOthersOccuring(Time time, Person person)
        {
            //Pepper and other spring crops and garlic in Sagittarius

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Sagittarius;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingPotatoAndOthers)]
        public static CalculatorResult IsGoodForPlantingPotatoAndOthersOccuring(Time time, Person person)
        {
            //Potato, radishes and turnips in Capricorn

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Capricorn;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingGrainsAndOthers)]
        public static CalculatorResult IsGoodForPlantingGrainsAndOthersOccuring(Time time, Person person)
        {
            //All black cereals and grains in Aquarius

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Aquarius;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForPlantingPumpkinsAndOthers)]
        public static CalculatorResult IsGoodForPlantingPumpkinsAndOthersOccuring(Time time, Person person)
        {
            //Cucumbers, pumpkins, radishes, water-melons and carrots in Pisces

            //1. General good yoga for planting
            var rightYoga = IsGoodYogaForAllAgricultureOccuring(time, person).Occuring;

            //if not right, end here as not occuring
            if (rightYoga == false) { return CalculatorResult.NotOccuring(); }


            //2. Correct rising sign
            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Pisces;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodYogaForAllAgriculture)]
        public static CalculatorResult IsGoodYogaForAllAgricultureOccuring(Time time, Person person)
        {
            //1. Lunar Day
            //provided the lunar day is also propitious.

            //right lunar days for agriculture occuring
            var rightLunarDay = IsGoodLunarDayAgricultureOccuring(time, person).Occuring;

            //if not correct lunar days, end here as not occuring
            if (rightLunarDay == false) { return CalculatorResult.NotOccuring(); }


            //2. Lagna Lord
            //Choose a Lagna, owned by the planet who is lord of the weekday in question.
            var lagnaLordIsWeekdayLord = IsLagnaLordIsWeekdayLordOccuring(time, person).Occuring;

            //if not correct lagna, end here as not occuring
            if (lagnaLordIsWeekdayLord == false) { return CalculatorResult.NotOccuring(); }


            //3. House
            //While beginning all agricultural operations, see that the 8th house is unoccupied
            var house8Occupied = IsBadForStartingAllAgricultureOccuring(time, person).Occuring;

            //if 8th house is occupied, end here as not occuring
            if (house8Occupied == true) { return CalculatorResult.NotOccuring(); }


            //4. Rising Sign
            //Gemini: Not favourable for any planting being a barren sign.
            //Leo: Not good for any planting, especially bad for underground plants such as potato.
            var badRising = IsBadLagnaForAllAgricultureOccuring(time, person).Occuring;

            //if bad rising sign, end here as not occuring
            if (badRising == true) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.BadForStartingAllAgriculture)]
        public static CalculatorResult IsBadForStartingAllAgricultureOccuring(Time time, Person person)
        {
            //While beginning all agricultural operations, see that the 8th house is unoccupied

            //get all planets in 8th house
            var planets = Calculate.PlanetsInHouse(HouseName.House8, time);

            //if any planets in 8th house, return occuring
            if (planets.Any())
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {   //if no planets, event not occuring
                return CalculatorResult.NotOccuring();
            }
        }

        [EventCalculator(EventName.LagnaLordIsWeekdayLord)]
        public static CalculatorResult IsLagnaLordIsWeekdayLordOccuring(Time time, Person person)
        {
            //Lagna owned by the planet who is lord of the weekday in question

            //get lord of lagna
            var lagnaLord = Calculate.LordOfHouse(HouseName.House1, time);

            //get lord of weekday
            var weekdayLord = Calculate.LordOfWeekday(time);


            //if the lord of lagna & lord of weekday same, then event occuring
            if (weekdayLord == lagnaLord)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {   //event not occuring, if planet not same
                return CalculatorResult.NotOccuring();
            }
        }

        [EventCalculator(EventName.GoodLunarDayAgriculture)]
        public static CalculatorResult IsGoodLunarDayAgricultureOccuring(Time time, Person person)
        {
            //All odd lunar days except the 9th are good.All even tithis except the
            //2nd and 4th should be avoided.
            //Thus: 1,2,3,4,5,7,11,13,15


            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //right lunar days to look for
            var rightLunarDay = lunarDayNumber == 1 ||
                                lunarDayNumber == 2 ||
                                lunarDayNumber == 3 ||
                                lunarDayNumber == 4 ||
                                lunarDayNumber == 5 ||
                                lunarDayNumber == 7 ||
                                lunarDayNumber == 11 ||
                                lunarDayNumber == 13 ||
                                lunarDayNumber == 15; //full moon

            //if not correct lunar days, end here as not occuring
            if (rightLunarDay == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.BadLagnaForAllAgriculture)]
        public static CalculatorResult IsBadLagnaForAllAgricultureOccuring(Time time, Person person)
        {
            //Gemini: Not favourable for any planting being a barren sign.
            //Leo: Not good for any planting, especially bad for underground plants such as potato.

            //get rising sign
            var risingSign = Calculate.HouseSignName(HouseName.House1, time);

            //if rising sign is Gemini or Leo, then event is occuring
            if (risingSign == ZodiacName.Gemini || risingSign == ZodiacName.Leo)
            {
                return CalculatorResult.IsOccuring();
            }
            else
            {
                //if different sign, not occuring
                return CalculatorResult.NotOccuring();
            }

        }

        #endregion

        #region BUYING AND SELLING

        [EventCalculator(EventName.GoodSellingForProfit)]
        public static CalculatorResult IsGoodSellingForProfitOccuring(Time time, Person person)
        {
            //Selling for Profit. - Let the Moon and Mercury be free from the
            // conjunction or aspect of Mars. The Moon's situation in Taurus. Cancer
            // or Pisces would greatly help the seller. Try to keep Mercury in a kendra
            // from Lagna or at least in good aspect to Jupiter. Tuesday should be
            // avoided. Monday, Wednesday and Thursday are the best. While Friday
            // is unpropitious, Saturday is middling.


            //1.Let the Moon and Mercury be free from the conjunction or aspect of Mars

            //if moon aspected by mars, end here as not occuring
            var moonAspectedByMars = Calculate.IsPlanetAspectedByPlanet(PlanetName.Moon, PlanetName.Mars, time);
            if (moonAspectedByMars) { return CalculatorResult.NotOccuring(); }

            //if mercury aspected by mars, end here as not occuring
            var mercuryAspectedByMars = Calculate.IsPlanetAspectedByPlanet(PlanetName.Mercury, PlanetName.Mars, time);
            if (mercuryAspectedByMars) { return CalculatorResult.NotOccuring(); }

            //if moon conjunct with mars, end here as not occuring
            var moonConjunctWithMars = Calculate.IsPlanetConjunctWithPlanet(PlanetName.Moon, PlanetName.Mars, time);
            if (moonConjunctWithMars) { return CalculatorResult.NotOccuring(); }

            //if mercury conjunct with mars, end here as not occuring
            var mercuryConjunctWithMars = Calculate.IsPlanetConjunctWithPlanet(PlanetName.Mercury, PlanetName.Mars, time);
            if (mercuryConjunctWithMars) { return CalculatorResult.NotOccuring(); }


            //2. The Moon's situation in Taurus. Cancer or Pisces would greatly help the seller.

            //get sign moon is in 
            var moonSign = Calculate.PlanetZodiacSign(PlanetName.Moon, time);

            //check if moon is in the correct sign
            var inCorrectSign = moonSign.GetSignName() == ZodiacName.Taurus ||
                                moonSign.GetSignName() == ZodiacName.Cancer ||
                                moonSign.GetSignName() == ZodiacName.Pisces;

            //if not correct sign, end here as not occuring
            if (inCorrectSign == false) { return CalculatorResult.NotOccuring(); }


            //3. Try to keep Mercury in a kendra from Lagna or at least in good aspect to Jupiter
            var mercuryInKendra = Calculate.IsPlanetInKendra(PlanetName.Mercury, time);
            var mercuryInGoodAspectToJupiter = Calculate.IsPlanetInGoodAspectToPlanet(PlanetName.Jupiter, PlanetName.Mercury, time);

            //if NOT in good aspect or in kendra, event not occuring
            if (!(mercuryInKendra || mercuryInGoodAspectToJupiter)) { return CalculatorResult.NotOccuring(); }


            //4. Tuesday should be avoided. Monday, Wednesday and Thursday are the best. While Friday
            // is unpropitious, Saturday is middling.

            //get weekday
            var weekDay = Calculate.DayOfWeek(time);

            //check if weekday correct 
            var inCorrectWeekday = weekDay == DayOfWeek.Monday ||
                                   weekDay == DayOfWeek.Wednesday ||
                                   weekDay == DayOfWeek.Thursday ||
                                   weekDay == DayOfWeek.Saturday;

            //if not correct weekday, end here as not occuring
            if (inCorrectWeekday == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodWeekdayForSelling)]
        public static CalculatorResult IsGoodWeekdayForSellingOccuring(Time time, Person person)
        {
            //Selling for Profit. -  Monday, Wednesday and Thursday are the best.


            //4. Monday, Wednesday and Thursday are the best.

            //get weekday
            var weekDay = Calculate.DayOfWeek(time);

            //check if weekday correct 
            var inCorrectWeekday = weekDay == DayOfWeek.Monday ||
                                   weekDay == DayOfWeek.Wednesday ||
                                   weekDay == DayOfWeek.Thursday;

            //if not correct weekday, end here as not occuring
            if (inCorrectWeekday == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodMoonSignForSelling)]
        public static CalculatorResult IsGoodMoonSignForSellingOccuring(Time time, Person person)
        {
            //Selling for Profit. - The Moon's situation in Taurus. Cancer
            // or Pisces would greatly help the seller.


            //2. The Moon's situation in Taurus. Cancer or Pisces would greatly help the seller.

            //get sign moon is in 
            var moonSign = Calculate.PlanetZodiacSign(PlanetName.Moon, time);

            //check if moon is in the correct sign
            var inCorrectSign = moonSign.GetSignName() == ZodiacName.Taurus ||
                                moonSign.GetSignName() == ZodiacName.Cancer ||
                                moonSign.GetSignName() == ZodiacName.Pisces;

            //if not correct sign, end here as not occuring
            if (inCorrectSign == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.BadForBuyingToolsUtensilsJewellery)]
        public static CalculatorResult IsBadForBuyingToolsUtensilsJewelleryOccuring(Time time, Person person)
        {
            //Buying Utensils, etc. - Place Jupiter in good aspect to the Moon while
            // buying brass vessels; to Mars when buying vessels of copper; to Saturn
            // if steel and iron; to ascendant if of silver. Avoid the asterisms of
            // Aslesha. Moola and Jyeshta. For buying tools, similarly avoid the 8th
            // and 9th lunar days and New Moon.

            //Buying Jewellery. -As usual unfavourable lunar days and asterisms should be avoided.

            //NOTE : Only bad constellaion & lunar days are used here


            //1.Avoid the asterisms of Aslesha. Moola and Jyeshta.
            //get ruling constellation
            var rulingConstellationName = Calculate.MoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Aslesha ||
                                     rulingConstellationName == ConstellationName.Moola ||
                                     rulingConstellationName == ConstellationName.Jyesta;


            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }



            //2. avoid the 8th and 9th lunar days and New Moon.
            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //right lunar days to look for
            var rightLunarDay = lunarDayNumber == 8 ||
                                lunarDayNumber == 9 ||
                                lunarDayNumber == 1;

            //if not correct lunar days, end here as not occuring
            if (rightLunarDay == false) { return CalculatorResult.NotOccuring(); }

            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForBuyingBrassVessels)]
        public static CalculatorResult IsGoodForBuyingBrassVesselsOccuring(Time time, Person person)
        {
            //Buying Utensils, etc. - Place Jupiter in good aspect to the Moon while
            // buying brass vessels;
            // Avoid the asterisms of
            // Aslesha. Moola and Jyeshta. For buying tools, similarly avoid the 8th
            // and 9th lunar days and New Moon.

            //1
            //bad tithi & constellation for buying not occuring
            var badYoga = IsBadForBuyingToolsUtensilsJewelleryOccuring(time, person).Occuring;

            //if occuring end here, as not occuring
            if (badYoga) { return CalculatorResult.NotOccuring(); }


            //2. Place Jupiter in good aspect to the Moon while buying brass vessels
            //check aspect
            var goodAspect = Calculate.IsPlanetInGoodAspectToPlanet(PlanetName.Moon, PlanetName.Jupiter, time);

            //if NOT occuring end here, as not occuring
            if (goodAspect == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForBuyingCopperVessels)]
        public static CalculatorResult IsGoodForBuyingCopperVesselsOccuring(Time time, Person person)
        {
            //Buying Utensils, etc. -
            //Place Jupiter in good aspect to Mars when buying vessels of copper;
            //Avoid the asterisms of
            // Aslesha. Moola and Jyeshta. For buying tools, similarly avoid the 8th
            // and 9th lunar days and New Moon.

            //1
            //bad tithi & constellation for buying not occuring
            var badYoga = IsBadForBuyingToolsUtensilsJewelleryOccuring(time, person).Occuring;

            //if occuring end here, as not occuring
            if (badYoga) { return CalculatorResult.NotOccuring(); }


            //2. Place Jupiter in good aspect to Mars when buying vessels of copper;
            //check aspect
            var goodAspect = Calculate.IsPlanetInGoodAspectToPlanet(PlanetName.Mars, PlanetName.Jupiter, time);

            //if NOT occuring end here, as not occuring
            if (goodAspect == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForBuyingSteelIronVessels)]
        public static CalculatorResult IsGoodForBuyingSteelIronVesselsOccuring(Time time, Person person)
        {
            //Buying Utensils, etc. -
            //Place Jupiter in good aspect to Saturn if steel and iron.
            // Avoid the asterisms of
            // Aslesha. Moola and Jyeshta. For buying tools, similarly avoid the 8th
            // and 9th lunar days and New Moon.

            //1
            //bad tithi & constellation for buying not occuring
            var badYoga = IsBadForBuyingToolsUtensilsJewelleryOccuring(time, person).Occuring;

            //if occuring end here, as not occuring
            if (badYoga) { return CalculatorResult.NotOccuring(); }


            //2. Place Jupiter in good aspect to Saturn when buying vessels of copper;
            //check aspect
            var goodAspect = Calculate.IsPlanetInGoodAspectToPlanet(PlanetName.Saturn, PlanetName.Jupiter, time);

            //if NOT occuring end here, as not occuring
            if (goodAspect == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForBuyingSilverVessels)]
        public static CalculatorResult IsGoodForBuyingSilverVesselsOccuring(Time time, Person person)
        {
            //Buying Utensils, etc. -
            //Place Jupiter in good aspect to ascendant if of silver.
            //Avoid the asterisms of
            //Aslesha. Moola and Jyeshta. For buying tools, similarly avoid the 8th
            // and 9th lunar days and New Moon.

            //1
            //bad tithi & constellation for buying not occuring
            var badYoga = IsBadForBuyingToolsUtensilsJewelleryOccuring(time, person).Occuring;

            //if occuring end here, as not occuring
            if (badYoga) { return CalculatorResult.NotOccuring(); }


            //2. Place Jupiter in good aspect to ascendant if of silver;
            //check aspect
            var goodAspect = Calculate.IsPlanetInGoodAspectToHouse(HouseName.House1, PlanetName.Jupiter, time);

            //if NOT occuring end here, as not occuring
            if (goodAspect == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();
        }

        [EventCalculator(EventName.GoodForBuyingJewellery)]
        public static CalculatorResult IsGoodForBuyingJewelleryOccuring(Time time, Person person)
        {
            //Buying Jewellery. - The Sun and the Moon should be well situated and
            // aspected. As usual unfavourable lunar days and asterisms should be
            // avoided.

            //1
            //bad tithi & constellation for buying not occuring
            var badYoga = IsBadForBuyingToolsUtensilsJewelleryOccuring(time, person).Occuring;

            //if occuring end here, as not occuring
            if (badYoga) { return CalculatorResult.NotOccuring(); }


            //2. The Sun and the Moon should be well situated and aspected.
            //check aspect
            var isGood = SunAndMoonWellSituatedAndAspected();

            //if NOT occuring end here, as not occuring
            if (isGood == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();


            //returns true if well
            //NOTE : This method is experimental using smaller parts of shadbala calculators
            //TODO Testing needed
            bool SunAndMoonWellSituatedAndAspected()
            {
                //1. good aspect
                //the goodness of the aspect received by Sun & Moon is 
                //based on Drik Bala calculations (aspect strenght)
                var sunAspectStrength = Calculate.PlanetDrikBala(PlanetName.Sun, time);
                var moonAspectStrength = Calculate.PlanetDrikBala(PlanetName.Sun, time);

                //Note: positive bala = positive aspect, negative bala = negative aspect
                //if NOT postive number, end here as not good
                if (!(sunAspectStrength.ToDouble() > 0 && moonAspectStrength.ToDouble() > 0)) { return false; }


                //2. well situated
                //based on Planet Sthana Bala (Positonal strength)
                var sunPositionStrenght = Calculate.PlanetSthanaBala(PlanetName.Sun, time);
                var moonPositionStrenght = Calculate.PlanetSthanaBala(PlanetName.Moon, time);

                //Note: To determine if sthana bala is indicating good position or bad position
                //a neutral point is set, anything above is good & below is bad
                var sunNeutralPoint = Calculate.PlanetSthanaBalaNeutralPoint(PlanetName.Sun);
                var moonNeutralPoint = Calculate.PlanetSthanaBalaNeutralPoint(PlanetName.Moon);

                //if NOT above neutral number, end here as not good
                if (!(sunPositionStrenght.ToDouble() > sunNeutralPoint && moonPositionStrenght.ToDouble() > moonNeutralPoint)) { return false; }



                //if control reaches here then good aspectect & well situated
                return true;
            }

        }


        #endregion

        #region ASTRONOMICAL

        [EventCalculator(EventName.Yama1)]
        public static CalculatorResult Yama1(Time time, Person person) => new() { Occuring = Calculate.BirthYama(time).YamaCount == 1 };
        [EventCalculator(EventName.Yama2)]
        public static CalculatorResult Yama2(Time time, Person person) => new() { Occuring = Calculate.BirthYama(time).YamaCount == 2 };
        [EventCalculator(EventName.Yama3)]
        public static CalculatorResult Yama3(Time time, Person person) => new() { Occuring = Calculate.BirthYama(time).YamaCount == 3 };
        [EventCalculator(EventName.Yama4)]
        public static CalculatorResult Yama4(Time time, Person person) => new() { Occuring = Calculate.BirthYama(time).YamaCount == 4 };
        [EventCalculator(EventName.Yama5)]
        public static CalculatorResult Yama5(Time time, Person person) => new() { Occuring = Calculate.BirthYama(time).YamaCount == 5 };


        [EventCalculator(EventName.SunIsStrong)]
        public static CalculatorResult IsSunIsStrongOccuring(Time time, Person person)
        {
            var strongestPlanet = Calculate.AllPlanetOrderedByStrength(time)[0];
            var occuring = strongestPlanet == PlanetName.Sun;


            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonIsStrong)]
        public static CalculatorResult IsMoonIsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllPlanetOrderedByStrength(time)[0] == PlanetName.Moon;

            //STRENGTH CALCULATION



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsIsStrong)]
        public static CalculatorResult IsMarsIsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllPlanetOrderedByStrength(time)[0] == PlanetName.Mars;


            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryIsStrong)]
        public static CalculatorResult IsMercuryIsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllPlanetOrderedByStrength(time)[0] == PlanetName.Mercury;


            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterIsStrong)]
        public static CalculatorResult IsJupiterIsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllPlanetOrderedByStrength(time)[0] == PlanetName.Jupiter;

            //STRENGTH CALCULATION



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusIsStrong)]
        public static CalculatorResult IsVenusIsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllPlanetOrderedByStrength(time)[0] == PlanetName.Venus;


            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnIsStrong)]
        public static CalculatorResult IsSaturnIsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllPlanetOrderedByStrength(time)[0] == PlanetName.Saturn;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House1IsStrong)]
        public static CalculatorResult IsHouse1IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House1;

            //STRENGTH CALCULATION



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House2IsStrong)]
        public static CalculatorResult IsHouse2IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House2;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House3IsStrong)]
        public static CalculatorResult IsHouse3IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House3;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House4IsStrong)]
        public static CalculatorResult IsHouse4IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House4;




            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House5IsStrong)]
        public static CalculatorResult IsHouse5IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House5;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House6IsStrong)]
        public static CalculatorResult IsHouse6IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House6;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House7IsStrong)]
        public static CalculatorResult IsHouse7IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House7;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House8IsStrong)]
        public static CalculatorResult IsHouse8IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House8;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House9IsStrong)]
        public static CalculatorResult IsHouse9IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House9;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House10IsStrong)]
        public static CalculatorResult IsHouse10IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House10;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House11IsStrong)]
        public static CalculatorResult IsHouse11IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House11;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.House12IsStrong)]
        public static CalculatorResult IsHouse12IsStrongOccuring(Time time, Person person)
        {
            var occuring = Calculate.AllHousesOrderedByStrength(time)[0] == HouseName.House12;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Sunrise)]
        public static CalculatorResult IsSunriseOccuring(Time time, Person person)
        {
            //actual sunrise at location, when center of sun disk is at horizon

            //get sunrise time for that day
            var sunriseTime = Calculate.SunriseTime(time);

            //+-5 min added to get the event, otherwise match with exact time might miss
            var MIN_5 = 0.08333333;// in hours
            var _5minAfter = sunriseTime.AddHours(MIN_5);
            var _5minBefore = sunriseTime.SubtractHours(MIN_5);

            var isAfter = time.GetLmtDateTimeOffset() >= _5minBefore.GetLmtDateTimeOffset();//after -5min
            var isBefore = time.GetLmtDateTimeOffset() <= _5minAfter.GetLmtDateTimeOffset();//before +5min

            //time is within +-5min
            var occuring = isAfter && isBefore;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Sunset)]
        public static CalculatorResult IsSunsetOccuring(Time time, Person person)
        {
            //actual sunset at location, when center of sun disk is at horizon

            //get sunset time for that day
            var sunsetTime = Calculate.SunsetTime(time);

            //+-5 min added to get the event, otherwise match with exact time might miss
            var MIN_5 = 0.08333333;// in hours
            var _5minAfter = sunsetTime.AddHours(MIN_5);
            var _5minBefore = sunsetTime.SubtractHours(MIN_5);

            var isAfter = time.GetLmtDateTimeOffset() >= _5minBefore.GetLmtDateTimeOffset();//after -5min
            var isBefore = time.GetLmtDateTimeOffset() <= _5minAfter.GetLmtDateTimeOffset();//before +5min

            //time is within +-5min
            var occuring = isAfter && isBefore;



            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Midday)]
        public static CalculatorResult IsMiddayOccuring(Time time, Person person)
        {
            //This is marked when the centre of the Sun is exactly on the
            // meridian of the place. The apparent noon is
            // almost the same for all places.


            //get apparent time
            var localApparentTime = Calculate.LocalApparentTime(time);
            var apparentNoon = Calculate.NoonTime(time);

            //+-5 min added to get the event, otherwise match with exact time might miss
            var MIN_5 = 0.08333333;// in hours
            var _5minAfter = apparentNoon.AddHours(MIN_5);
            var _5minBefore = apparentNoon.AddHours(-MIN_5);

            var isAfter = localApparentTime >= _5minBefore;//after -5min
            var isBefore = localApparentTime <= _5minAfter;//before +5min

            //time is within 11:55AM to 12:05PM
            var occuring = isAfter && isBefore;



            return new() { Occuring = occuring };
        }


        #endregion

        #region BUILDING CONSTRUCTION

        [EventCalculator(EventName.BadLunarMonthForBuilding)]
        public static CalculatorResult BadLunarMonthForBuilding(Time time, Person person)
        {
            //No house-building should be commenced in the lunar months of
            // Jyeshta, Ashadha, Bhadrapada, Aswayuja, Margasira, Pushya and
            // Phalguna as they connote respectively death, destruction, disease,
            // quarrels and misunderstandings, loss of wealth, incendiarism and
            // physical danger

            //get lunar current lunar month
            var lunarMonth = Calculate.LunarMonth(time);

            if (lunarMonth is LunarMonth.Jyeshtha or LunarMonth.Aashaadha or LunarMonth.Bhaadrapada
                or LunarMonth.Aaswayuja or LunarMonth.Maargasira or LunarMonth.Pushya or LunarMonth.Phaalguna)
            {
                return CalculatorResult.IsOccuring();
            }

            //if conrtol reaches here then event is ocuring
            return CalculatorResult.NotOccuring();
        }


        // The lunar months of Chaitra, Vaisakha, Sravana,
        // Kartika and Magha are the best.


        [EventCalculator(EventName.GoodSunSignForBuilding)]
        public static CalculatorResult GoodSunSignForBuilding(Time time, Person person)
        {
            // The Sun should occupy fixed signs or
            // at least movable signs but

            //get sign sun is in
            var sunSign = Calculate.SunSign(time).GetSignName();

            //check if sign is a fixed or movable sign
            var isFixedSign = Calculate.IsFixedSign(sunSign);
            var isMovableSign = Calculate.IsMovableSign(sunSign);
            var occuring = isFixedSign || isMovableSign;

            //if conrtol reaches here then event is ocuring
            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.BadSunSignForBuilding)]
        public static CalculatorResult BadSunSignForBuilding(Time time, Person person)
        {
            // no building work should be undertaken when
            // the Sun is in common signs.

            //get sign sun is in
            var sunSign = Calculate.SunSign(time).GetSignName();

            //check if sign is a common sign
            var isCommonSign = Calculate.IsCommonSign(sunSign);
            var occuring = isCommonSign;

            //if conrtol reaches here then event is ocuring
            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.GoodLunarDayForBuilding)]
        public static CalculatorResult GoodLunarDayForBuilding(Time time, Person person)
        {
            //All odd tithis (lunar days) except the 9th are good.
            //odd numbers : 1, 3, 5, 7, 11, 13, 15

            //Of the even tithis the 2nd, 6th and 10th are auspicious.
            //even numbers : 2, 6, 10,

            //get lunar day
            var lunarDayNumber = Calculate.LunarDay(time).GetLunarDayNumber();

            //right lunar days to look for
            var rightLunarDay = lunarDayNumber == 1 ||
                                lunarDayNumber == 3 ||
                                lunarDayNumber == 5 ||
                                lunarDayNumber == 7 ||
                                lunarDayNumber == 11 ||
                                lunarDayNumber == 13 ||
                                lunarDayNumber == 15 ||//full moon
                                lunarDayNumber == 2 ||
                                lunarDayNumber == 6 ||
                                lunarDayNumber == 10;

            //if not correct lunar days, end here as not occuring
            if (rightLunarDay == false) { return CalculatorResult.NotOccuring(); }


            //if control reaches here then event is ocuring
            return CalculatorResult.IsOccuring();

        }

        [EventCalculator(EventName.GoodWeekDayForBuilding)]
        public static CalculatorResult GoodWeekDayForBuilding(Time time, Person person)
        {
            // Monday, Wednesday, Thursday and  Friday are the best,
            // Monday, Wednesday and Thursday are the best.

            //get week day
            var weekday = Calculate.DayOfWeek(time);


            switch (weekday)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring, if planet not same
                    return CalculatorResult.NotOccuring();
            }

        }

        [EventCalculator(EventName.BadLunarPhaseForBuilding)]
        public static CalculatorResult BadLunarPhaseForBuilding(Time time, Person person)
        {
            // Even Monday should be rejected when the Moon is
            // waning.
            //Waning moon is bad

            //get the moon phase
            var moonPhase = Calculate.LunarDay(time).GetMoonPhase();

            //occuring when moon is wanning
            var occuring = moonPhase == MoonPhase.DarkHalf;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.BadWeekDayForBuilding)]
        public static CalculatorResult BadWeekDayForBuilding(Time time, Person person)
        {
            //Saturday should be rejected as it connots frequent thefts. Sunday
            //should also be avoided unless the day is otherwise very auspicious.

            //get week day
            var weekday = Calculate.DayOfWeek(time);


            switch (weekday)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring, if planet not same
                    return CalculatorResult.NotOccuring();
            }

        }

        [EventCalculator(EventName.BadWeekDayForRepairs)]
        public static CalculatorResult BadWeekDayForRepairs(Time time, Person person)
        {
            // Do not commence repairs on Tuesdays.

            //get week day
            var weekday = Calculate.DayOfWeek(time);


            switch (weekday)
            {
                case DayOfWeek.Tuesday:
                    return CalculatorResult.IsOccuring();
                default:
                    //event not occuring, if planet not same
                    return CalculatorResult.NotOccuring();
            }

        }


        [EventCalculator(EventName.GoodYogaForRepairs)]
        public static CalculatorResult GoodYogaForRepairs(Time time, Person person)
        {

            // Friday at a moment when Lagna is Taurus or Libra and
            // Monday when Cancer is rising are very suitable for beginning repairs.

            //friday & lagna is taurus or libra
            var isFriday = Calculate.DayOfWeek(time) == DayOfWeek.Friday;
            var lagnaIsTaurus = Calculate.HouseSignName(HouseName.House1, time) == ZodiacName.Taurus;
            var lagnaIsLibra = Calculate.HouseSignName(HouseName.House1, time) == ZodiacName.Libra;
            var isFridayLagnaTaurusLibra = isFriday && (lagnaIsLibra || lagnaIsTaurus);

            //monday & lagna is cancer
            var isMonday = Calculate.DayOfWeek(time) == DayOfWeek.Monday;
            var lagnaIsCancer = Calculate.HouseSignName(HouseName.House1, time) == ZodiacName.Cancer;
            var isMondayLagnaCancer = isMonday && lagnaIsCancer;

            //if either is true
            var occuring = isFridayLagnaTaurusLibra || isMondayLagnaCancer;

            return new CalculatorResult() { Occuring = occuring };

        }


        [EventCalculator(EventName.GoodYogaForRepairs2)]
        public static CalculatorResult GoodYogaForRepairs2(Time time, Person person)
        {

            // The Lagna must be occupied by a benefic and the Moon should be in an aquatic sign.

            //benefic in lagna
            var beneficsInLagna = Calculate.IsBeneficPlanetInHouse(HouseName.House1, time);

            //monday in aquatic sign
            var moonSign = Calculate.MoonSignName(time);
            var isMoonInAquaticSign = Calculate.IsWaterSign(moonSign);

            //if either is true
            var occuring = isMoonInAquaticSign || beneficsInLagna;

            return new CalculatorResult() { Occuring = occuring };

        }

        //[EventCalculator(EventName.BadConstellationForRepairs)]
        //public static Prediction BadConstellationForRepairs(Time time, Person person)
        //{

        //    //No repairs should be started under the constellations of Krittika, Makha.
        //    // Pushyami, Pubba, Hasta, Moola and Revati when Mars is transiting
        //    // these constellations.



        //    var occuring = isMoonInAquaticSign || beneficsInLagna;

        //    return new Prediction() { Occuring = occuring };

        //}


        #endregion

        #region ASHTAKVARGA_GOCHARA

        //SUN
        [EventCalculator(EventName.SunTransit8Bindu)]
        public static CalculatorResult SunTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 8) };

        [EventCalculator(EventName.SunTransit7Bindu)]
        public static CalculatorResult SunTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 7) };

        [EventCalculator(EventName.SunTransit6Bindu)]
        public static CalculatorResult SunTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 6) };

        [EventCalculator(EventName.SunTransit5Bindu)]
        public static CalculatorResult SunTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 5) };

        [EventCalculator(EventName.SunTransit4Bindu)]
        public static CalculatorResult SunTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 4) };

        [EventCalculator(EventName.SunTransit3Bindu)]
        public static CalculatorResult SunTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 3) };

        [EventCalculator(EventName.SunTransit2Bindu)]
        public static CalculatorResult SunTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 2) };

        [EventCalculator(EventName.SunTransit1Bindu)]
        public static CalculatorResult SunTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 1) };

        [EventCalculator(EventName.SunTransit0Bindu)]
        public static CalculatorResult SunTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Sun, 0) };

        //Moon
        [EventCalculator(EventName.MoonTransit8Bindu)]
        public static CalculatorResult MoonTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 8) };

        [EventCalculator(EventName.MoonTransit7Bindu)]
        public static CalculatorResult MoonTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 7) };

        [EventCalculator(EventName.MoonTransit6Bindu)]
        public static CalculatorResult MoonTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 6) };

        [EventCalculator(EventName.MoonTransit5Bindu)]
        public static CalculatorResult MoonTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 5) };

        [EventCalculator(EventName.MoonTransit4Bindu)]
        public static CalculatorResult MoonTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 4) };

        [EventCalculator(EventName.MoonTransit3Bindu)]
        public static CalculatorResult MoonTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 3) };

        [EventCalculator(EventName.MoonTransit2Bindu)]
        public static CalculatorResult MoonTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 2) };

        [EventCalculator(EventName.MoonTransit1Bindu)]
        public static CalculatorResult MoonTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 1) };

        [EventCalculator(EventName.MoonTransit0Bindu)]
        public static CalculatorResult MoonTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Moon, 0) };

        //Mars
        [EventCalculator(EventName.MarsTransit8Bindu)]
        public static CalculatorResult MarsTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 8) };

        [EventCalculator(EventName.MarsTransit7Bindu)]
        public static CalculatorResult MarsTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 7) };

        [EventCalculator(EventName.MarsTransit6Bindu)]
        public static CalculatorResult MarsTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 6) };

        [EventCalculator(EventName.MarsTransit5Bindu)]
        public static CalculatorResult MarsTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 5) };

        [EventCalculator(EventName.MarsTransit4Bindu)]
        public static CalculatorResult MarsTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 4) };

        [EventCalculator(EventName.MarsTransit3Bindu)]
        public static CalculatorResult MarsTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 3) };

        [EventCalculator(EventName.MarsTransit2Bindu)]
        public static CalculatorResult MarsTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 2) };

        [EventCalculator(EventName.MarsTransit1Bindu)]
        public static CalculatorResult MarsTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 1) };

        [EventCalculator(EventName.MarsTransit0Bindu)]
        public static CalculatorResult MarsTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mars, 0) };

        //Mercury
        [EventCalculator(EventName.MercuryTransit8Bindu)]
        public static CalculatorResult MercuryTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 8) };

        [EventCalculator(EventName.MercuryTransit7Bindu)]
        public static CalculatorResult MercuryTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 7) };

        [EventCalculator(EventName.MercuryTransit6Bindu)]
        public static CalculatorResult MercuryTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 6) };

        [EventCalculator(EventName.MercuryTransit5Bindu)]
        public static CalculatorResult MercuryTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 5) };

        [EventCalculator(EventName.MercuryTransit4Bindu)]
        public static CalculatorResult MercuryTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 4) };

        [EventCalculator(EventName.MercuryTransit3Bindu)]
        public static CalculatorResult MercuryTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 3) };

        [EventCalculator(EventName.MercuryTransit2Bindu)]
        public static CalculatorResult MercuryTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 2) };

        [EventCalculator(EventName.MercuryTransit1Bindu)]
        public static CalculatorResult MercuryTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 1) };

        [EventCalculator(EventName.MercuryTransit0Bindu)]
        public static CalculatorResult MercuryTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Mercury, 0) };


        //Venus
        [EventCalculator(EventName.VenusTransit8Bindu)]
        public static CalculatorResult VenusTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 8) };

        [EventCalculator(EventName.VenusTransit7Bindu)]
        public static CalculatorResult VenusTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 7) };

        [EventCalculator(EventName.VenusTransit6Bindu)]
        public static CalculatorResult VenusTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 6) };

        [EventCalculator(EventName.VenusTransit5Bindu)]
        public static CalculatorResult VenusTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 5) };

        [EventCalculator(EventName.VenusTransit4Bindu)]
        public static CalculatorResult VenusTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 4) };

        [EventCalculator(EventName.VenusTransit3Bindu)]
        public static CalculatorResult VenusTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 3) };

        [EventCalculator(EventName.VenusTransit2Bindu)]
        public static CalculatorResult VenusTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 2) };

        [EventCalculator(EventName.VenusTransit1Bindu)]
        public static CalculatorResult VenusTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 1) };

        [EventCalculator(EventName.VenusTransit0Bindu)]
        public static CalculatorResult VenusTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Venus, 0) };


        //Jupiter
        [EventCalculator(EventName.JupiterTransit8Bindu)]
        public static CalculatorResult JupiterTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 8) };

        [EventCalculator(EventName.JupiterTransit7Bindu)]
        public static CalculatorResult JupiterTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 7) };

        [EventCalculator(EventName.JupiterTransit6Bindu)]
        public static CalculatorResult JupiterTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 6) };

        [EventCalculator(EventName.JupiterTransit5Bindu)]
        public static CalculatorResult JupiterTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 5) };

        [EventCalculator(EventName.JupiterTransit4Bindu)]
        public static CalculatorResult JupiterTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 4) };

        [EventCalculator(EventName.JupiterTransit3Bindu)]
        public static CalculatorResult JupiterTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 3) };

        [EventCalculator(EventName.JupiterTransit2Bindu)]
        public static CalculatorResult JupiterTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 2) };

        [EventCalculator(EventName.JupiterTransit1Bindu)]
        public static CalculatorResult JupiterTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 1) };

        [EventCalculator(EventName.JupiterTransit0Bindu)]
        public static CalculatorResult JupiterTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Jupiter, 0) };

        //Saturn
        [EventCalculator(EventName.SaturnTransit8Bindu)]
        public static CalculatorResult SaturnTransit8Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 8) };

        [EventCalculator(EventName.SaturnTransit7Bindu)]
        public static CalculatorResult SaturnTransit7Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 7) };

        [EventCalculator(EventName.SaturnTransit6Bindu)]
        public static CalculatorResult SaturnTransit6Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 6) };

        [EventCalculator(EventName.SaturnTransit5Bindu)]
        public static CalculatorResult SaturnTransit5Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 5) };

        [EventCalculator(EventName.SaturnTransit4Bindu)]
        public static CalculatorResult SaturnTransit4Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 4) };

        [EventCalculator(EventName.SaturnTransit3Bindu)]
        public static CalculatorResult SaturnTransit3Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 3) };

        [EventCalculator(EventName.SaturnTransit2Bindu)]
        public static CalculatorResult SaturnTransit2Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 2) };

        [EventCalculator(EventName.SaturnTransit1Bindu)]
        public static CalculatorResult SaturnTransit1Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 1) };

        [EventCalculator(EventName.SaturnTransit0Bindu)]
        public static CalculatorResult SaturnTransit0Bindu(Time time, Person person) => new() { Occuring = Calculate.IsPlanetGocharaBindu(person.BirthTime, time, PlanetName.Saturn, 0) };



        #endregion

        #region GOCHARA

        [EventCalculator(EventName.GocharaSummary)]
        public static CalculatorResult GocharaSummary(Time time, Person person)
        {
            return CalculatorResult.NotOccuring();
            //get all gochara ocured att time
            var occuringGocharaList = new List<CalculatorResult>() { };

            //loop list
            var goodCount = 0;
            var badCount = 0;
            foreach (var result in occuringGocharaList)
            {
                //result.
            }

            //summarize good & bad count to final value

            //Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 1)
        }

        [EventCalculator(EventName.SunGocharaInHouse1)]
        public static CalculatorResult SunGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 1) };

        [EventCalculator(EventName.SunGocharaInHouse2)]
        public static CalculatorResult SunGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 2) };

        [EventCalculator(EventName.SunGocharaInHouse3)]
        public static CalculatorResult SunGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 3) };

        [EventCalculator(EventName.SunGocharaInHouse4)]
        public static CalculatorResult SunGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 4) };

        [EventCalculator(EventName.SunGocharaInHouse5)]
        public static CalculatorResult SunGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 5) };

        [EventCalculator(EventName.SunGocharaInHouse6)]
        public static CalculatorResult SunGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 6) };

        [EventCalculator(EventName.SunGocharaInHouse7)]
        public static CalculatorResult SunGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 7) };

        [EventCalculator(EventName.SunGocharaInHouse8)]
        public static CalculatorResult SunGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 8) };

        [EventCalculator(EventName.SunGocharaInHouse9)]
        public static CalculatorResult SunGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 9) };

        [EventCalculator(EventName.SunGocharaInHouse10)]
        public static CalculatorResult SunGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 10) };

        [EventCalculator(EventName.SunGocharaInHouse11)]
        public static CalculatorResult SunGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 11) };

        [EventCalculator(EventName.SunGocharaInHouse12)]
        public static CalculatorResult SunGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 12) };

        //MOON
        [EventCalculator(EventName.MoonGocharaInHouse1)]
        public static CalculatorResult MoonGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 1) };

        [EventCalculator(EventName.MoonGocharaInHouse2)]
        public static CalculatorResult MoonGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 2) };

        [EventCalculator(EventName.MoonGocharaInHouse3)]
        public static CalculatorResult MoonGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 3) };

        [EventCalculator(EventName.MoonGocharaInHouse4)]
        public static CalculatorResult MoonGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 4) };

        [EventCalculator(EventName.MoonGocharaInHouse5)]
        public static CalculatorResult MoonGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 5) };

        [EventCalculator(EventName.MoonGocharaInHouse6)]
        public static CalculatorResult MoonGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 6) };

        [EventCalculator(EventName.MoonGocharaInHouse7)]
        public static CalculatorResult MoonGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 7) };

        [EventCalculator(EventName.MoonGocharaInHouse8)]
        public static CalculatorResult MoonGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 8) };

        [EventCalculator(EventName.MoonGocharaInHouse9)]
        public static CalculatorResult MoonGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 9) };

        [EventCalculator(EventName.MoonGocharaInHouse10)]
        public static CalculatorResult MoonGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 10) };

        [EventCalculator(EventName.MoonGocharaInHouse11)]
        public static CalculatorResult MoonGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 11) };

        [EventCalculator(EventName.MoonGocharaInHouse12)]
        public static CalculatorResult MoonGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 12) };

        //MARS
        [EventCalculator(EventName.MarsGocharaInHouse1)]
        public static CalculatorResult MarsGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 1) };

        [EventCalculator(EventName.MarsGocharaInHouse2)]
        public static CalculatorResult MarsGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 2) };

        [EventCalculator(EventName.MarsGocharaInHouse3)]
        public static CalculatorResult MarsGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 3) };

        [EventCalculator(EventName.MarsGocharaInHouse4)]
        public static CalculatorResult MarsGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 4) };

        [EventCalculator(EventName.MarsGocharaInHouse5)]
        public static CalculatorResult MarsGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 5) };

        [EventCalculator(EventName.MarsGocharaInHouse6)]
        public static CalculatorResult MarsGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 6) };

        [EventCalculator(EventName.MarsGocharaInHouse7)]
        public static CalculatorResult MarsGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 7) };

        [EventCalculator(EventName.MarsGocharaInHouse8)]
        public static CalculatorResult MarsGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 8) };

        [EventCalculator(EventName.MarsGocharaInHouse9)]
        public static CalculatorResult MarsGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 9) };

        [EventCalculator(EventName.MarsGocharaInHouse10)]
        public static CalculatorResult MarsGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 10) };

        [EventCalculator(EventName.MarsGocharaInHouse11)]
        public static CalculatorResult MarsGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 11) };

        [EventCalculator(EventName.MarsGocharaInHouse12)]
        public static CalculatorResult MarsGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 12) };

        //MERCURY
        [EventCalculator(EventName.MercuryGocharaInHouse1)]
        public static CalculatorResult MercuryGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 1) };

        [EventCalculator(EventName.MercuryGocharaInHouse2)]
        public static CalculatorResult MercuryGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 2) };

        [EventCalculator(EventName.MercuryGocharaInHouse3)]
        public static CalculatorResult MercuryGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 3) };

        [EventCalculator(EventName.MercuryGocharaInHouse4)]
        public static CalculatorResult MercuryGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 4) };

        [EventCalculator(EventName.MercuryGocharaInHouse5)]
        public static CalculatorResult MercuryGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 5) };

        [EventCalculator(EventName.MercuryGocharaInHouse6)]
        public static CalculatorResult MercuryGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 6) };

        [EventCalculator(EventName.MercuryGocharaInHouse7)]
        public static CalculatorResult MercuryGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 7) };

        [EventCalculator(EventName.MercuryGocharaInHouse8)]
        public static CalculatorResult MercuryGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 8) };

        [EventCalculator(EventName.MercuryGocharaInHouse9)]
        public static CalculatorResult MercuryGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 9) };

        [EventCalculator(EventName.MercuryGocharaInHouse10)]
        public static CalculatorResult MercuryGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 10) };

        [EventCalculator(EventName.MercuryGocharaInHouse11)]
        public static CalculatorResult MercuryGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 11) };

        [EventCalculator(EventName.MercuryGocharaInHouse12)]
        public static CalculatorResult MercuryGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 12) };

        //JUPITER
        [EventCalculator(EventName.JupiterGocharaInHouse1)]
        public static CalculatorResult JupiterGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 1) };

        [EventCalculator(EventName.JupiterGocharaInHouse2)]
        public static CalculatorResult JupiterGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 2) };

        [EventCalculator(EventName.JupiterGocharaInHouse3)]
        public static CalculatorResult JupiterGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 3) };

        [EventCalculator(EventName.JupiterGocharaInHouse4)]
        public static CalculatorResult JupiterGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 4) };

        [EventCalculator(EventName.JupiterGocharaInHouse5)]
        public static CalculatorResult JupiterGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 5) };

        [EventCalculator(EventName.JupiterGocharaInHouse6)]
        public static CalculatorResult JupiterGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 6) };

        [EventCalculator(EventName.JupiterGocharaInHouse7)]
        public static CalculatorResult JupiterGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 7) };

        [EventCalculator(EventName.JupiterGocharaInHouse8)]
        public static CalculatorResult JupiterGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 8) };

        [EventCalculator(EventName.JupiterGocharaInHouse9)]
        public static CalculatorResult JupiterGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 9) };

        [EventCalculator(EventName.JupiterGocharaInHouse10)]
        public static CalculatorResult JupiterGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 10) };

        [EventCalculator(EventName.JupiterGocharaInHouse11)]
        public static CalculatorResult JupiterGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 11) };

        [EventCalculator(EventName.JupiterGocharaInHouse12)]
        public static CalculatorResult JupiterGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 12) };

        //VENUS
        [EventCalculator(EventName.VenusGocharaInHouse1)]
        public static CalculatorResult VenusGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 1) };

        [EventCalculator(EventName.VenusGocharaInHouse2)]
        public static CalculatorResult VenusGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 2) };

        [EventCalculator(EventName.VenusGocharaInHouse3)]
        public static CalculatorResult VenusGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 3) };

        [EventCalculator(EventName.VenusGocharaInHouse4)]
        public static CalculatorResult VenusGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 4) };

        [EventCalculator(EventName.VenusGocharaInHouse5)]
        public static CalculatorResult VenusGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 5) };

        [EventCalculator(EventName.VenusGocharaInHouse6)]
        public static CalculatorResult VenusGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 6) };

        [EventCalculator(EventName.VenusGocharaInHouse7)]
        public static CalculatorResult VenusGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 7) };

        [EventCalculator(EventName.VenusGocharaInHouse8)]
        public static CalculatorResult VenusGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 8) };

        [EventCalculator(EventName.VenusGocharaInHouse9)]
        public static CalculatorResult VenusGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 9) };

        [EventCalculator(EventName.VenusGocharaInHouse10)]
        public static CalculatorResult VenusGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 10) };

        [EventCalculator(EventName.VenusGocharaInHouse11)]
        public static CalculatorResult VenusGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 11) };

        [EventCalculator(EventName.VenusGocharaInHouse12)]
        public static CalculatorResult VenusGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 12) };

        //SATURN
        [EventCalculator(EventName.SaturnGocharaInHouse1)]
        public static CalculatorResult SaturnGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 1) };

        [EventCalculator(EventName.SaturnGocharaInHouse2)]
        public static CalculatorResult SaturnGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 2) };

        [EventCalculator(EventName.SaturnGocharaInHouse3)]
        public static CalculatorResult SaturnGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 3) };

        [EventCalculator(EventName.SaturnGocharaInHouse4)]
        public static CalculatorResult SaturnGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 4) };

        [EventCalculator(EventName.SaturnGocharaInHouse5)]
        public static CalculatorResult SaturnGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 5) };

        [EventCalculator(EventName.SaturnGocharaInHouse6)]
        public static CalculatorResult SaturnGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 6) };

        [EventCalculator(EventName.SaturnGocharaInHouse7)]
        public static CalculatorResult SaturnGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 7) };

        [EventCalculator(EventName.SaturnGocharaInHouse8)]
        public static CalculatorResult SaturnGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 8) };

        [EventCalculator(EventName.SaturnGocharaInHouse9)]
        public static CalculatorResult SaturnGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 9) };

        [EventCalculator(EventName.SaturnGocharaInHouse10)]
        public static CalculatorResult SaturnGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 10) };

        [EventCalculator(EventName.SaturnGocharaInHouse11)]
        public static CalculatorResult SaturnGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 11) };

        [EventCalculator(EventName.SaturnGocharaInHouse12)]
        public static CalculatorResult SaturnGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 12) };

        //RAHU
        [EventCalculator(EventName.RahuGocharaInHouse1)]
        public static CalculatorResult RahuGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 1) };

        [EventCalculator(EventName.RahuGocharaInHouse2)]
        public static CalculatorResult RahuGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 2) };

        [EventCalculator(EventName.RahuGocharaInHouse3)]
        public static CalculatorResult RahuGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 3) };

        [EventCalculator(EventName.RahuGocharaInHouse4)]
        public static CalculatorResult RahuGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 4) };

        [EventCalculator(EventName.RahuGocharaInHouse5)]
        public static CalculatorResult RahuGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 5) };

        [EventCalculator(EventName.RahuGocharaInHouse6)]
        public static CalculatorResult RahuGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 6) };

        [EventCalculator(EventName.RahuGocharaInHouse7)]
        public static CalculatorResult RahuGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 7) };

        [EventCalculator(EventName.RahuGocharaInHouse8)]
        public static CalculatorResult RahuGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 8) };

        [EventCalculator(EventName.RahuGocharaInHouse9)]
        public static CalculatorResult RahuGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 9) };

        [EventCalculator(EventName.RahuGocharaInHouse10)]
        public static CalculatorResult RahuGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 10) };

        [EventCalculator(EventName.RahuGocharaInHouse11)]
        public static CalculatorResult RahuGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 11) };

        [EventCalculator(EventName.RahuGocharaInHouse12)]
        public static CalculatorResult RahuGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 12) };

        //KETU
        [EventCalculator(EventName.KetuGocharaInHouse1)]
        public static CalculatorResult KetuGocharaInHouse1(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 1) };

        [EventCalculator(EventName.KetuGocharaInHouse2)]
        public static CalculatorResult KetuGocharaInHouse2(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 2) };

        [EventCalculator(EventName.KetuGocharaInHouse3)]
        public static CalculatorResult KetuGocharaInHouse3(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 3) };

        [EventCalculator(EventName.KetuGocharaInHouse4)]
        public static CalculatorResult KetuGocharaInHouse4(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 4) };

        [EventCalculator(EventName.KetuGocharaInHouse5)]
        public static CalculatorResult KetuGocharaInHouse5(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 5) };

        [EventCalculator(EventName.KetuGocharaInHouse6)]
        public static CalculatorResult KetuGocharaInHouse6(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 6) };

        [EventCalculator(EventName.KetuGocharaInHouse7)]
        public static CalculatorResult KetuGocharaInHouse7(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 7) };

        [EventCalculator(EventName.KetuGocharaInHouse8)]
        public static CalculatorResult KetuGocharaInHouse8(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 8) };

        [EventCalculator(EventName.KetuGocharaInHouse9)]
        public static CalculatorResult KetuGocharaInHouse9(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 9) };

        [EventCalculator(EventName.KetuGocharaInHouse10)]
        public static CalculatorResult KetuGocharaInHouse10(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 10) };

        [EventCalculator(EventName.KetuGocharaInHouse11)]
        public static CalculatorResult KetuGocharaInHouse11(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 11) };

        [EventCalculator(EventName.KetuGocharaInHouse12)]
        public static CalculatorResult KetuGocharaInHouse12(Time time, Person person) => new() { Occuring = Calculate.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 12) };

        #endregion GOCHARA

        #region DASAS

        #region SUN DASA

        [EventCalculator(EventName.AriesSunPD1)]
        public static CalculatorResult AriesSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSunPD1)]
        public static CalculatorResult TaurusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSunPD1)]
        public static CalculatorResult GeminiSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSunPD1)]
        public static CalculatorResult CancerSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSunPD1)]
        public static CalculatorResult LeoSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSunPD1)]
        public static CalculatorResult VirgoSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSunPD1)]
        public static CalculatorResult LibraSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSunPD1)]
        public static CalculatorResult ScorpioSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSunPD1)]
        public static CalculatorResult SagittariusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornSunPD1)]
        public static CalculatorResult CapricornSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSunPD1)]
        public static CalculatorResult AquariusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSunPD1)]
        public static CalculatorResult PiscesSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion SUN DASA

        #region MOON DASA

        [EventCalculator(EventName.AriesMoonPD1)]
        public static CalculatorResult AriesMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMoonPD1)]
        public static CalculatorResult TaurusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMoonPD1)]
        public static CalculatorResult GeminiMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMoonPD1)]
        public static CalculatorResult CancerMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMoonPD1)]
        public static CalculatorResult LeoMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMoonPD1)]
        public static CalculatorResult VirgoMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMoonPD1)]
        public static CalculatorResult LibraMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMoonPD1)]
        public static CalculatorResult ScorpioMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMoonPD1)]
        public static CalculatorResult SagittariusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornMoonPD1)]
        public static CalculatorResult CapricornMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMoonPD1)]
        public static CalculatorResult AquariusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMoonPD1)]
        public static CalculatorResult PiscesMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion MOON DASA

        #region MARS DASA

        [EventCalculator(EventName.AriesMarsPD1)]
        public static CalculatorResult AriesMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMarsPD1)]
        public static CalculatorResult TaurusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMarsPD1)]
        public static CalculatorResult GeminiMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMarsPD1)]
        public static CalculatorResult CancerMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMarsPD1)]
        public static CalculatorResult LeoMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMarsPD1)]
        public static CalculatorResult VirgoMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMarsPD1)]
        public static CalculatorResult LibraMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMarsPD1)]
        public static CalculatorResult ScorpioMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMarsPD1)]
        public static CalculatorResult SagittariusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornMarsPD1)]
        public static CalculatorResult CapricornMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMarsPD1)]
        public static CalculatorResult AquariusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMarsPD1)]
        public static CalculatorResult PiscesMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion MARS DASA

        #region RAHU DASA

        [EventCalculator(EventName.AriesRahuPD1)]
        public static CalculatorResult AriesRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusRahuPD1)]
        public static CalculatorResult TaurusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiRahuPD1)]
        public static CalculatorResult GeminiRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerRahuPD1)]
        public static CalculatorResult CancerRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoRahuPD1)]
        public static CalculatorResult LeoRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoRahuPD1)]
        public static CalculatorResult VirgoRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraRahuPD1)]
        public static CalculatorResult LibraRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioRahuPD1)]
        public static CalculatorResult ScorpioRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusRahuPD1)]
        public static CalculatorResult SagittariusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornRahuPD1)]
        public static CalculatorResult CapricornRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusRahuPD1)]
        public static CalculatorResult AquariusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesRahuPD1)]
        public static CalculatorResult PiscesRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion RAHU DASA

        #region JUPITER DASA

        [EventCalculator(EventName.AriesJupiterPD1)]
        public static CalculatorResult AriesJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusJupiterPD1)]
        public static CalculatorResult TaurusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiJupiterPD1)]
        public static CalculatorResult GeminiJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerJupiterPD1)]
        public static CalculatorResult CancerJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoJupiterPD1)]
        public static CalculatorResult LeoJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoJupiterPD1)]
        public static CalculatorResult VirgoJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraJupiterPD1)]
        public static CalculatorResult LibraJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioJupiterPD1)]
        public static CalculatorResult ScorpioJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusJupiterPD1)]
        public static CalculatorResult SagittariusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornJupiterPD1)]
        public static CalculatorResult CapricornJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusJupiterPD1)]
        public static CalculatorResult AquariusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesJupiterPD1)]
        public static CalculatorResult PiscesJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion JUPITER DASA

        #region SATURN DASA

        [EventCalculator(EventName.AriesSaturnPD1)]
        public static CalculatorResult AriesSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSaturnPD1)]
        public static CalculatorResult TaurusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSaturnPD1)]
        public static CalculatorResult GeminiSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSaturnPD1)]
        public static CalculatorResult CancerSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSaturnPD1)]
        public static CalculatorResult LeoSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSaturnPD1)]
        public static CalculatorResult VirgoSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSaturnPD1)]
        public static CalculatorResult LibraSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSaturnPD1)]
        public static CalculatorResult ScorpioSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSaturnPD1)]
        public static CalculatorResult SagittariusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornSaturnPD1)]
        public static CalculatorResult CapricornSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSaturnPD1)]
        public static CalculatorResult AquariusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSaturnPD1)]
        public static CalculatorResult PiscesSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion SATURN DASA

        #region MERCURY DASA

        [EventCalculator(EventName.AriesMercuryPD1)]
        public static CalculatorResult AriesMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMercuryPD1)]
        public static CalculatorResult TaurusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMercuryPD1)]
        public static CalculatorResult GeminiMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMercuryPD1)]
        public static CalculatorResult CancerMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMercuryPD1)]
        public static CalculatorResult LeoMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMercuryPD1)]
        public static CalculatorResult VirgoMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMercuryPD1)]
        public static CalculatorResult LibraMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMercuryPD1)]
        public static CalculatorResult ScorpioMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMercuryPD1)]
        public static CalculatorResult SagittariusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornMercuryPD1)]
        public static CalculatorResult CapricornMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMercuryPD1)]
        public static CalculatorResult AquariusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMercuryPD1)]
        public static CalculatorResult PiscesMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion MERCURY DASA

        #region KETU DASA

        [EventCalculator(EventName.AriesKetuPD1)]
        public static CalculatorResult AriesKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusKetuPD1)]
        public static CalculatorResult TaurusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiKetuPD1)]
        public static CalculatorResult GeminiKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerKetuPD1)]
        public static CalculatorResult CancerKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoKetuPD1)]
        public static CalculatorResult LeoKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoKetuPD1)]
        public static CalculatorResult VirgoKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraKetuPD1)]
        public static CalculatorResult LibraKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioKetuPD1)]
        public static CalculatorResult ScorpioKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusKetuPD1)]
        public static CalculatorResult SagittariusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornKetuPD1)]
        public static CalculatorResult CapricornKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusKetuPD1)]
        public static CalculatorResult AquariusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesKetuPD1)]
        public static CalculatorResult PiscesKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion KETU DASA

        #region VENUS DASA

        [EventCalculator(EventName.AriesVenusPD1)]
        public static CalculatorResult AriesVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusVenusPD1)]
        public static CalculatorResult TaurusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiVenusPD1)]
        public static CalculatorResult GeminiVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerVenusPD1)]
        public static CalculatorResult CancerVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoVenusPD1)]
        public static CalculatorResult LeoVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoVenusPD1)]
        public static CalculatorResult VirgoVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraVenusPD1)]
        public static CalculatorResult LibraVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioVenusPD1)]
        public static CalculatorResult ScorpioVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusVenusPD1)]
        public static CalculatorResult SagittariusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornVenusPD1)]
        public static CalculatorResult CapricornVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Capricorn;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusVenusPD1)]
        public static CalculatorResult AquariusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesVenusPD1)]
        public static CalculatorResult PiscesVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = Calculate.PlanetZodiacSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion VENUS DASA

        #endregion DASAS

        //PD2

        #region SUN PD2

        [EventCalculator(EventName.SunSunPD2)]
        public static CalculatorResult SunSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunMoonPD2)]
        public static CalculatorResult SunMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunMarsPD2)]
        public static CalculatorResult SunMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunRahuPD2)]
        public static CalculatorResult SunRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunJupiterPD2)]
        public static CalculatorResult SunJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunSaturnPD2)]
        public static CalculatorResult SunSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunMercuryPD2)]
        public static CalculatorResult SunMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunKetuPD2)]
        public static CalculatorResult SunKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunVenusPD2)]
        public static CalculatorResult SunVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion SUN PD2

        #region MOON PD2

        [EventCalculator(EventName.MoonSunPD2)]
        public static CalculatorResult MoonSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonMoonPD2)]
        public static CalculatorResult MoonMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonMarsPD2)]
        public static CalculatorResult MoonMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonRahuPD2)]
        public static CalculatorResult MoonRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonJupiterPD2)]
        public static CalculatorResult MoonJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonSaturnPD2)]
        public static CalculatorResult MoonSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonMercuryPD2)]
        public static CalculatorResult MoonMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonKetuPD2)]
        public static CalculatorResult MoonKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonVenusPD2)]
        public static CalculatorResult MoonVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion MOON PD2

        #region MARS PD2

        [EventCalculator(EventName.MarsSunPD2)]
        public static CalculatorResult MarsSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsMoonPD2)]
        public static CalculatorResult MarsMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsMarsPD2)]
        public static CalculatorResult MarsMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsRahuPD2)]
        public static CalculatorResult MarsRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsJupiterPD2)]
        public static CalculatorResult MarsJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsSaturnPD2)]
        public static CalculatorResult MarsSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsMercuryPD2)]
        public static CalculatorResult MarsMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsKetuPD2)]
        public static CalculatorResult MarsKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsVenusPD2)]
        public static CalculatorResult MarsVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion MARS PD2

        #region RAHU PD2

        [EventCalculator(EventName.RahuSunPD2)]
        public static CalculatorResult RahuSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuMoonPD2)]
        public static CalculatorResult RahuMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuMarsPD2)]
        public static CalculatorResult RahuMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuRahuPD2)]
        public static CalculatorResult RahuRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuJupiterPD2)]
        public static CalculatorResult RahuJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuSaturnPD2)]
        public static CalculatorResult RahuSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuMercuryPD2)]
        public static CalculatorResult RahuMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuKetuPD2)]
        public static CalculatorResult RahuKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuVenusPD2)]
        public static CalculatorResult RahuVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion RAHU PD2

        #region JUPITER PD2

        [EventCalculator(EventName.JupiterSunPD2)]
        public static CalculatorResult JupiterSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterMoonPD2)]
        public static CalculatorResult JupiterMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterMarsPD2)]
        public static CalculatorResult JupiterMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterRahuPD2)]
        public static CalculatorResult JupiterRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterJupiterPD2)]
        public static CalculatorResult JupiterJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterSaturnPD2)]
        public static CalculatorResult JupiterSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterMercuryPD2)]
        public static CalculatorResult JupiterMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterKetuPD2)]
        public static CalculatorResult JupiterKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterVenusPD2)]
        public static CalculatorResult JupiterVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion JUPITER PD2

        #region SATURN PD2

        [EventCalculator(EventName.SaturnSunPD2)]
        public static CalculatorResult SaturnSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnMoonPD2)]
        public static CalculatorResult SaturnMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnMarsPD2)]
        public static CalculatorResult SaturnMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnRahuPD2)]
        public static CalculatorResult SaturnRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnJupiterPD2)]
        public static CalculatorResult SaturnJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnSaturnPD2)]
        public static CalculatorResult SaturnSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnMercuryPD2)]
        public static CalculatorResult SaturnMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnKetuPD2)]
        public static CalculatorResult SaturnKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnVenusPD2)]
        public static CalculatorResult SaturnVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion SATURN PD2

        #region MERCURY PD2

        [EventCalculator(EventName.MercurySunPD2)]
        public static CalculatorResult MercurySunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryMoonPD2)]
        public static CalculatorResult MercuryMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryMarsPD2)]
        public static CalculatorResult MercuryMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryRahuPD2)]
        public static CalculatorResult MercuryRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryJupiterPD2)]
        public static CalculatorResult MercuryJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercurySaturnPD2)]
        public static CalculatorResult MercurySaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryMercuryPD2)]
        public static CalculatorResult MercuryMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryKetuPD2)]
        public static CalculatorResult MercuryKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryVenusPD2)]
        public static CalculatorResult MercuryVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion MERCURY PD2

        #region KETU PD2

        [EventCalculator(EventName.KetuSunPD2)]
        public static CalculatorResult KetuSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuMoonPD2)]
        public static CalculatorResult KetuMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuMarsPD2)]
        public static CalculatorResult KetuMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuRahuPD2)]
        public static CalculatorResult KetuRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuJupiterPD2)]
        public static CalculatorResult KetuJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuSaturnPD2)]
        public static CalculatorResult KetuSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuMercuryPD2)]
        public static CalculatorResult KetuMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuKetuPD2)]
        public static CalculatorResult KetuKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuVenusPD2)]
        public static CalculatorResult KetuVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion KETU PD2

        #region VENUS PD2

        [EventCalculator(EventName.VenusSunPD2)]
        public static CalculatorResult VenusSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusMoonPD2)]
        public static CalculatorResult VenusMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusMarsPD2)]
        public static CalculatorResult VenusMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusRahuPD2)]
        public static CalculatorResult VenusRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusJupiterPD2)]
        public static CalculatorResult VenusJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusSaturnPD2)]
        public static CalculatorResult VenusSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusMercuryPD2)]
        public static CalculatorResult VenusMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusKetuPD2)]
        public static CalculatorResult VenusKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusVenusPD2)]
        public static CalculatorResult VenusVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion VENUS PD2

        //ANTARAM

        #region SUN PD3

        [EventCalculator(EventName.SunSunPD3)]
        public static CalculatorResult SunSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD3)]
        public static CalculatorResult MoonSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD3)]
        public static CalculatorResult MarsSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD3)]
        public static CalculatorResult RahuSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD3)]
        public static CalculatorResult JupiterSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD3)]
        public static CalculatorResult SaturnSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD3)]
        public static CalculatorResult MercurySunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD3)]
        public static CalculatorResult KetuSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD3)]
        public static CalculatorResult VenusSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD3

        #region MOON PD3

        [EventCalculator(EventName.SunMoonPD3)]
        public static CalculatorResult SunMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD3)]
        public static CalculatorResult MoonMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD3)]
        public static CalculatorResult MarsMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD3)]
        public static CalculatorResult RahuMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD3)]
        public static CalculatorResult JupiterMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD3)]
        public static CalculatorResult SaturnMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD3)]
        public static CalculatorResult MercuryMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD3)]
        public static CalculatorResult KetuMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD3)]
        public static CalculatorResult VenusMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD3

        #region MARS PD3

        [EventCalculator(EventName.SunMarsPD3)]
        public static CalculatorResult SunMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD3)]
        public static CalculatorResult MoonMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD3)]
        public static CalculatorResult MarsMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD3)]
        public static CalculatorResult RahuMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD3)]
        public static CalculatorResult JupiterMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD3)]
        public static CalculatorResult SaturnMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD3)]
        public static CalculatorResult MercuryMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD3)]
        public static CalculatorResult KetuMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD3)]
        public static CalculatorResult VenusMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD3

        #region RAHU PD3

        [EventCalculator(EventName.SunRahuPD3)]
        public static CalculatorResult SunRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD3)]
        public static CalculatorResult MoonRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD3)]
        public static CalculatorResult MarsRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD3)]
        public static CalculatorResult RahuRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD3)]
        public static CalculatorResult JupiterRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD3)]
        public static CalculatorResult SaturnRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD3)]
        public static CalculatorResult MercuryRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD3)]
        public static CalculatorResult KetuRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD3)]
        public static CalculatorResult VenusRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD3

        #region JUPITER PD3

        [EventCalculator(EventName.SunJupiterPD3)]
        public static CalculatorResult SunJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD3)]
        public static CalculatorResult MoonJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD3)]
        public static CalculatorResult MarsJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD3)]
        public static CalculatorResult RahuJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD3)]
        public static CalculatorResult JupiterJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD3)]
        public static CalculatorResult SaturnJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD3)]
        public static CalculatorResult MercuryJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD3)]
        public static CalculatorResult KetuJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD3)]
        public static CalculatorResult VenusJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD3

        #region SATURN PD3

        [EventCalculator(EventName.SunSaturnPD3)]
        public static CalculatorResult SunSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD3)]
        public static CalculatorResult MoonSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD3)]
        public static CalculatorResult MarsSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD3)]
        public static CalculatorResult RahuSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD3)]
        public static CalculatorResult JupiterSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD3)]
        public static CalculatorResult SaturnSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD3)]
        public static CalculatorResult MercurySaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD3)]
        public static CalculatorResult KetuSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD3)]
        public static CalculatorResult VenusSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD3

        #region MERCURY PD3

        [EventCalculator(EventName.SunMercuryPD3)]
        public static CalculatorResult SunMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD3)]
        public static CalculatorResult MoonMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD3)]
        public static CalculatorResult MarsMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD3)]
        public static CalculatorResult RahuMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD3)]
        public static CalculatorResult JupiterMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD3)]
        public static CalculatorResult SaturnMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD3)]
        public static CalculatorResult MercuryMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD3)]
        public static CalculatorResult KetuMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD3)]
        public static CalculatorResult VenusMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD3

        #region KETU PD3

        [EventCalculator(EventName.SunKetuPD3)]
        public static CalculatorResult SunKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD3)]
        public static CalculatorResult MoonKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD3)]
        public static CalculatorResult MarsKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD3)]
        public static CalculatorResult RahuKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD3)]
        public static CalculatorResult JupiterKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD3)]
        public static CalculatorResult SaturnKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD3)]
        public static CalculatorResult MercuryKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD3)]
        public static CalculatorResult KetuKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD3)]
        public static CalculatorResult VenusKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD3

        #region VENUS PD3

        [EventCalculator(EventName.SunVenusPD3)]
        public static CalculatorResult SunVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD3)]
        public static CalculatorResult MoonVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD3)]
        public static CalculatorResult MarsVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD3)]
        public static CalculatorResult RahuVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD3)]
        public static CalculatorResult JupiterVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD3)]
        public static CalculatorResult SaturnVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD3)]
        public static CalculatorResult MercuryVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD3)]
        public static CalculatorResult KetuVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD3)]
        public static CalculatorResult VenusVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD3

        //SUKSHMA

        #region SUN PD4

        [EventCalculator(EventName.SunSunPD4)]
        public static CalculatorResult SunSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD4)]
        public static CalculatorResult MoonSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD4)]
        public static CalculatorResult MarsSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD4)]
        public static CalculatorResult RahuSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD4)]
        public static CalculatorResult JupiterSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD4)]
        public static CalculatorResult SaturnSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD4)]
        public static CalculatorResult MercurySunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD4)]
        public static CalculatorResult KetuSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD4)]
        public static CalculatorResult VenusSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD4

        #region MOON PD4

        [EventCalculator(EventName.SunMoonPD4)]
        public static CalculatorResult SunMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD4)]
        public static CalculatorResult MoonMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD4)]
        public static CalculatorResult MarsMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD4)]
        public static CalculatorResult RahuMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD4)]
        public static CalculatorResult JupiterMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD4)]
        public static CalculatorResult SaturnMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD4)]
        public static CalculatorResult MercuryMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD4)]
        public static CalculatorResult KetuMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD4)]
        public static CalculatorResult VenusMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD4

        #region MARS PD4

        [EventCalculator(EventName.SunMarsPD4)]
        public static CalculatorResult SunMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD4)]
        public static CalculatorResult MoonMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD4)]
        public static CalculatorResult MarsMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD4)]
        public static CalculatorResult RahuMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD4)]
        public static CalculatorResult JupiterMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD4)]
        public static CalculatorResult SaturnMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD4)]
        public static CalculatorResult MercuryMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD4)]
        public static CalculatorResult KetuMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD4)]
        public static CalculatorResult VenusMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD4

        #region RAHU PD4

        [EventCalculator(EventName.SunRahuPD4)]
        public static CalculatorResult SunRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD4)]
        public static CalculatorResult MoonRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD4)]
        public static CalculatorResult MarsRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD4)]
        public static CalculatorResult RahuRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD4)]
        public static CalculatorResult JupiterRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD4)]
        public static CalculatorResult SaturnRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD4)]
        public static CalculatorResult MercuryRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD4)]
        public static CalculatorResult KetuRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD4)]
        public static CalculatorResult VenusRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD4

        #region JUPITER PD4

        [EventCalculator(EventName.SunJupiterPD4)]
        public static CalculatorResult SunJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD4)]
        public static CalculatorResult MoonJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD4)]
        public static CalculatorResult MarsJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD4)]
        public static CalculatorResult RahuJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD4)]
        public static CalculatorResult JupiterJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD4)]
        public static CalculatorResult SaturnJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD4)]
        public static CalculatorResult MercuryJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD4)]
        public static CalculatorResult KetuJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD4)]
        public static CalculatorResult VenusJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD4

        #region SATURN PD4

        [EventCalculator(EventName.SunSaturnPD4)]
        public static CalculatorResult SunSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD4)]
        public static CalculatorResult MoonSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD4)]
        public static CalculatorResult MarsSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD4)]
        public static CalculatorResult RahuSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD4)]
        public static CalculatorResult JupiterSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD4)]
        public static CalculatorResult SaturnSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD4)]
        public static CalculatorResult MercurySaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD4)]
        public static CalculatorResult KetuSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD4)]
        public static CalculatorResult VenusSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD4

        #region MERCURY PD4

        [EventCalculator(EventName.SunMercuryPD4)]
        public static CalculatorResult SunMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD4)]
        public static CalculatorResult MoonMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD4)]
        public static CalculatorResult MarsMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD4)]
        public static CalculatorResult RahuMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD4)]
        public static CalculatorResult JupiterMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD4)]
        public static CalculatorResult SaturnMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD4)]
        public static CalculatorResult MercuryMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD4)]
        public static CalculatorResult KetuMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD4)]
        public static CalculatorResult VenusMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD4

        #region KETU PD4

        [EventCalculator(EventName.SunKetuPD4)]
        public static CalculatorResult SunKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD4)]
        public static CalculatorResult MoonKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD4)]
        public static CalculatorResult MarsKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD4)]
        public static CalculatorResult RahuKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD4)]
        public static CalculatorResult JupiterKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD4)]
        public static CalculatorResult SaturnKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD4)]
        public static CalculatorResult MercuryKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD4)]
        public static CalculatorResult KetuKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD4)]
        public static CalculatorResult VenusKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD4

        #region VENUS PD4

        [EventCalculator(EventName.SunVenusPD4)]
        public static CalculatorResult SunVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD4)]
        public static CalculatorResult MoonVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD4)]
        public static CalculatorResult MarsVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD4)]
        public static CalculatorResult RahuVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD4)]
        public static CalculatorResult JupiterVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD4)]
        public static CalculatorResult SaturnVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD4)]
        public static CalculatorResult MercuryVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD4)]
        public static CalculatorResult KetuVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD4)]
        public static CalculatorResult VenusVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD4

        //PRANA

        #region SUN PD5

        [EventCalculator(EventName.SunSunPD5)]
        public static CalculatorResult SunSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD5)]
        public static CalculatorResult MoonSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD5)]
        public static CalculatorResult MarsSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD5)]
        public static CalculatorResult RahuSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD5)]
        public static CalculatorResult JupiterSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD5)]
        public static CalculatorResult SaturnSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD5)]
        public static CalculatorResult MercurySunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD5)]
        public static CalculatorResult KetuSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD5)]
        public static CalculatorResult VenusSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD5

        #region MOON PD5

        [EventCalculator(EventName.SunMoonPD5)]
        public static CalculatorResult SunMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD5)]
        public static CalculatorResult MoonMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD5)]
        public static CalculatorResult MarsMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD5)]
        public static CalculatorResult RahuMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD5)]
        public static CalculatorResult JupiterMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD5)]
        public static CalculatorResult SaturnMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD5)]
        public static CalculatorResult MercuryMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD5)]
        public static CalculatorResult KetuMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD5)]
        public static CalculatorResult VenusMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD5

        #region MARS PD5

        [EventCalculator(EventName.SunMarsPD5)]
        public static CalculatorResult SunMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD5)]
        public static CalculatorResult MoonMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD5)]
        public static CalculatorResult MarsMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD5)]
        public static CalculatorResult RahuMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD5)]
        public static CalculatorResult JupiterMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD5)]
        public static CalculatorResult SaturnMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD5)]
        public static CalculatorResult MercuryMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD5)]
        public static CalculatorResult KetuMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD5)]
        public static CalculatorResult VenusMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD5

        #region RAHU PD5

        [EventCalculator(EventName.SunRahuPD5)]
        public static CalculatorResult SunRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD5)]
        public static CalculatorResult MoonRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD5)]
        public static CalculatorResult MarsRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD5)]
        public static CalculatorResult RahuRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD5)]
        public static CalculatorResult JupiterRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD5)]
        public static CalculatorResult SaturnRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD5)]
        public static CalculatorResult MercuryRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD5)]
        public static CalculatorResult KetuRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD5)]
        public static CalculatorResult VenusRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD5

        #region JUPITER PD5

        [EventCalculator(EventName.SunJupiterPD5)]
        public static CalculatorResult SunJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD5)]
        public static CalculatorResult MoonJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD5)]
        public static CalculatorResult MarsJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD5)]
        public static CalculatorResult RahuJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD5)]
        public static CalculatorResult JupiterJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD5)]
        public static CalculatorResult SaturnJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD5)]
        public static CalculatorResult MercuryJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD5)]
        public static CalculatorResult KetuJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD5)]
        public static CalculatorResult VenusJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD5

        #region SATURN PD5

        [EventCalculator(EventName.SunSaturnPD5)]
        public static CalculatorResult SunSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD5)]
        public static CalculatorResult MoonSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD5)]
        public static CalculatorResult MarsSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD5)]
        public static CalculatorResult RahuSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD5)]
        public static CalculatorResult JupiterSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD5)]
        public static CalculatorResult SaturnSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD5)]
        public static CalculatorResult MercurySaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD5)]
        public static CalculatorResult KetuSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD5)]
        public static CalculatorResult VenusSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD5

        #region MERCURY PD5

        [EventCalculator(EventName.SunMercuryPD5)]
        public static CalculatorResult SunMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD5)]
        public static CalculatorResult MoonMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD5)]
        public static CalculatorResult MarsMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD5)]
        public static CalculatorResult RahuMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD5)]
        public static CalculatorResult JupiterMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD5)]
        public static CalculatorResult SaturnMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD5)]
        public static CalculatorResult MercuryMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD5)]
        public static CalculatorResult KetuMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD5)]
        public static CalculatorResult VenusMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD5

        #region KETU PD5

        [EventCalculator(EventName.SunKetuPD5)]
        public static CalculatorResult SunKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD5)]
        public static CalculatorResult MoonKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD5)]
        public static CalculatorResult MarsKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD5)]
        public static CalculatorResult RahuKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD5)]
        public static CalculatorResult JupiterKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD5)]
        public static CalculatorResult SaturnKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD5)]
        public static CalculatorResult MercuryKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD5)]
        public static CalculatorResult KetuKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD5)]
        public static CalculatorResult VenusKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD5

        #region VENUS PD5

        [EventCalculator(EventName.SunVenusPD5)]
        public static CalculatorResult SunVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD5)]
        public static CalculatorResult MoonVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD5)]
        public static CalculatorResult MarsVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD5)]
        public static CalculatorResult RahuVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD5)]
        public static CalculatorResult JupiterVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD5)]
        public static CalculatorResult SaturnVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD5)]
        public static CalculatorResult MercuryVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD5)]
        public static CalculatorResult KetuVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD5)]
        public static CalculatorResult VenusVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD5


        //PD6

        #region SUN PD6

        [EventCalculator(EventName.SunSunPD6)]
        public static CalculatorResult SunSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD6)]
        public static CalculatorResult MoonSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD6)]
        public static CalculatorResult MarsSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD6)]
        public static CalculatorResult RahuSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD6)]
        public static CalculatorResult JupiterSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD6)]
        public static CalculatorResult SaturnSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD6)]
        public static CalculatorResult MercurySunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD6)]
        public static CalculatorResult KetuSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD6)]
        public static CalculatorResult VenusSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD6

        #region MOON PD6

        [EventCalculator(EventName.SunMoonPD6)]
        public static CalculatorResult SunMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD6)]
        public static CalculatorResult MoonMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD6)]
        public static CalculatorResult MarsMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD6)]
        public static CalculatorResult RahuMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD6)]
        public static CalculatorResult JupiterMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD6)]
        public static CalculatorResult SaturnMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD6)]
        public static CalculatorResult MercuryMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD6)]
        public static CalculatorResult KetuMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD6)]
        public static CalculatorResult VenusMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD6

        #region MARS PD6

        [EventCalculator(EventName.SunMarsPD6)]
        public static CalculatorResult SunMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD6)]
        public static CalculatorResult MoonMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD6)]
        public static CalculatorResult MarsMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD6)]
        public static CalculatorResult RahuMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD6)]
        public static CalculatorResult JupiterMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD6)]
        public static CalculatorResult SaturnMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD6)]
        public static CalculatorResult MercuryMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD6)]
        public static CalculatorResult KetuMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD6)]
        public static CalculatorResult VenusMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD6

        #region RAHU PD6

        [EventCalculator(EventName.SunRahuPD6)]
        public static CalculatorResult SunRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD6)]
        public static CalculatorResult MoonRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD6)]
        public static CalculatorResult MarsRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD6)]
        public static CalculatorResult RahuRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD6)]
        public static CalculatorResult JupiterRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD6)]
        public static CalculatorResult SaturnRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD6)]
        public static CalculatorResult MercuryRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD6)]
        public static CalculatorResult KetuRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD6)]
        public static CalculatorResult VenusRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD6

        #region JUPITER PD6

        [EventCalculator(EventName.SunJupiterPD6)]
        public static CalculatorResult SunJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD6)]
        public static CalculatorResult MoonJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD6)]
        public static CalculatorResult MarsJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD6)]
        public static CalculatorResult RahuJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD6)]
        public static CalculatorResult JupiterJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD6)]
        public static CalculatorResult SaturnJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD6)]
        public static CalculatorResult MercuryJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD6)]
        public static CalculatorResult KetuJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD6)]
        public static CalculatorResult VenusJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD6

        #region SATURN PD6

        [EventCalculator(EventName.SunSaturnPD6)]
        public static CalculatorResult SunSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD6)]
        public static CalculatorResult MoonSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD6)]
        public static CalculatorResult MarsSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD6)]
        public static CalculatorResult RahuSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD6)]
        public static CalculatorResult JupiterSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD6)]
        public static CalculatorResult SaturnSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD6)]
        public static CalculatorResult MercurySaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD6)]
        public static CalculatorResult KetuSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD6)]
        public static CalculatorResult VenusSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD6

        #region MERCURY PD6

        [EventCalculator(EventName.SunMercuryPD6)]
        public static CalculatorResult SunMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD6)]
        public static CalculatorResult MoonMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD6)]
        public static CalculatorResult MarsMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD6)]
        public static CalculatorResult RahuMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD6)]
        public static CalculatorResult JupiterMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD6)]
        public static CalculatorResult SaturnMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD6)]
        public static CalculatorResult MercuryMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD6)]
        public static CalculatorResult KetuMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD6)]
        public static CalculatorResult VenusMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD6

        #region KETU PD6

        [EventCalculator(EventName.SunKetuPD6)]
        public static CalculatorResult SunKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD6)]
        public static CalculatorResult MoonKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD6)]
        public static CalculatorResult MarsKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD6)]
        public static CalculatorResult RahuKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD6)]
        public static CalculatorResult JupiterKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD6)]
        public static CalculatorResult SaturnKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD6)]
        public static CalculatorResult MercuryKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD6)]
        public static CalculatorResult KetuKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD6)]
        public static CalculatorResult VenusKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD6

        #region VENUS PD6

        [EventCalculator(EventName.SunVenusPD6)]
        public static CalculatorResult SunVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD6)]
        public static CalculatorResult MoonVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD6)]
        public static CalculatorResult MarsVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD6)]
        public static CalculatorResult RahuVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD6)]
        public static CalculatorResult JupiterVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD6)]
        public static CalculatorResult SaturnVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD6)]
        public static CalculatorResult MercuryVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD6)]
        public static CalculatorResult KetuVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD6)]
        public static CalculatorResult VenusVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD6


        //PD7

        #region SUN PD7

        [EventCalculator(EventName.SunSunPD7)]
        public static CalculatorResult SunSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD7)]
        public static CalculatorResult MoonSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD7)]
        public static CalculatorResult MarsSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD7)]
        public static CalculatorResult RahuSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD7)]
        public static CalculatorResult JupiterSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD7)]
        public static CalculatorResult SaturnSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD7)]
        public static CalculatorResult MercurySunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD7)]
        public static CalculatorResult KetuSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD7)]
        public static CalculatorResult VenusSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD7

        #region MOON PD7

        [EventCalculator(EventName.SunMoonPD7)]
        public static CalculatorResult SunMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD7)]
        public static CalculatorResult MoonMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD7)]
        public static CalculatorResult MarsMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD7)]
        public static CalculatorResult RahuMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD7)]
        public static CalculatorResult JupiterMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD7)]
        public static CalculatorResult SaturnMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD7)]
        public static CalculatorResult MercuryMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD7)]
        public static CalculatorResult KetuMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD7)]
        public static CalculatorResult VenusMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD7

        #region MARS PD7

        [EventCalculator(EventName.SunMarsPD7)]
        public static CalculatorResult SunMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD7)]
        public static CalculatorResult MoonMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD7)]
        public static CalculatorResult MarsMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD7)]
        public static CalculatorResult RahuMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD7)]
        public static CalculatorResult JupiterMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD7)]
        public static CalculatorResult SaturnMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD7)]
        public static CalculatorResult MercuryMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD7)]
        public static CalculatorResult KetuMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD7)]
        public static CalculatorResult VenusMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD7

        #region RAHU PD7

        [EventCalculator(EventName.SunRahuPD7)]
        public static CalculatorResult SunRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD7)]
        public static CalculatorResult MoonRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD7)]
        public static CalculatorResult MarsRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD7)]
        public static CalculatorResult RahuRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD7)]
        public static CalculatorResult JupiterRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD7)]
        public static CalculatorResult SaturnRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD7)]
        public static CalculatorResult MercuryRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD7)]
        public static CalculatorResult KetuRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD7)]
        public static CalculatorResult VenusRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD7

        #region JUPITER PD7

        [EventCalculator(EventName.SunJupiterPD7)]
        public static CalculatorResult SunJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD7)]
        public static CalculatorResult MoonJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD7)]
        public static CalculatorResult MarsJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD7)]
        public static CalculatorResult RahuJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD7)]
        public static CalculatorResult JupiterJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD7)]
        public static CalculatorResult SaturnJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD7)]
        public static CalculatorResult MercuryJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD7)]
        public static CalculatorResult KetuJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD7)]
        public static CalculatorResult VenusJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD7

        #region SATURN PD7

        [EventCalculator(EventName.SunSaturnPD7)]
        public static CalculatorResult SunSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD7)]
        public static CalculatorResult MoonSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD7)]
        public static CalculatorResult MarsSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD7)]
        public static CalculatorResult RahuSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD7)]
        public static CalculatorResult JupiterSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD7)]
        public static CalculatorResult SaturnSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD7)]
        public static CalculatorResult MercurySaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD7)]
        public static CalculatorResult KetuSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD7)]
        public static CalculatorResult VenusSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD7

        #region MERCURY PD7

        [EventCalculator(EventName.SunMercuryPD7)]
        public static CalculatorResult SunMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD7)]
        public static CalculatorResult MoonMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD7)]
        public static CalculatorResult MarsMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD7)]
        public static CalculatorResult RahuMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD7)]
        public static CalculatorResult JupiterMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD7)]
        public static CalculatorResult SaturnMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD7)]
        public static CalculatorResult MercuryMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD7)]
        public static CalculatorResult KetuMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD7)]
        public static CalculatorResult VenusMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD7

        #region KETU PD7

        [EventCalculator(EventName.SunKetuPD7)]
        public static CalculatorResult SunKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD7)]
        public static CalculatorResult MoonKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD7)]
        public static CalculatorResult MarsKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD7)]
        public static CalculatorResult RahuKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD7)]
        public static CalculatorResult JupiterKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD7)]
        public static CalculatorResult SaturnKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD7)]
        public static CalculatorResult MercuryKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD7)]
        public static CalculatorResult KetuKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD7)]
        public static CalculatorResult VenusKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD7

        #region VENUS PD7

        [EventCalculator(EventName.SunVenusPD7)]
        public static CalculatorResult SunVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD7)]
        public static CalculatorResult MoonVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD7)]
        public static CalculatorResult MarsVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD7)]
        public static CalculatorResult RahuVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD7)]
        public static CalculatorResult JupiterVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD7)]
        public static CalculatorResult SaturnVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD7)]
        public static CalculatorResult MercuryVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD7)]
        public static CalculatorResult KetuVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD7)]
        public static CalculatorResult VenusVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD7


        //PD8

        #region SUN PD8

        [EventCalculator(EventName.SunSunPD8)]
        public static CalculatorResult SunSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD8)]
        public static CalculatorResult MoonSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD8)]
        public static CalculatorResult MarsSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD8)]
        public static CalculatorResult RahuSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD8)]
        public static CalculatorResult JupiterSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD8)]
        public static CalculatorResult SaturnSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD8)]
        public static CalculatorResult MercurySunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD8)]
        public static CalculatorResult KetuSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD8)]
        public static CalculatorResult VenusSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD8

        #region MOON PD8

        [EventCalculator(EventName.SunMoonPD8)]
        public static CalculatorResult SunMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD8)]
        public static CalculatorResult MoonMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD8)]
        public static CalculatorResult MarsMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD8)]
        public static CalculatorResult RahuMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD8)]
        public static CalculatorResult JupiterMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD8)]
        public static CalculatorResult SaturnMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD8)]
        public static CalculatorResult MercuryMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD8)]
        public static CalculatorResult KetuMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD8)]
        public static CalculatorResult VenusMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD8

        #region MARS PD8

        [EventCalculator(EventName.SunMarsPD8)]
        public static CalculatorResult SunMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD8)]
        public static CalculatorResult MoonMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD8)]
        public static CalculatorResult MarsMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD8)]
        public static CalculatorResult RahuMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD8)]
        public static CalculatorResult JupiterMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD8)]
        public static CalculatorResult SaturnMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD8)]
        public static CalculatorResult MercuryMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD8)]
        public static CalculatorResult KetuMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD8)]
        public static CalculatorResult VenusMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD8

        #region RAHU PD8

        [EventCalculator(EventName.SunRahuPD8)]
        public static CalculatorResult SunRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD8)]
        public static CalculatorResult MoonRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD8)]
        public static CalculatorResult MarsRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD8)]
        public static CalculatorResult RahuRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD8)]
        public static CalculatorResult JupiterRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD8)]
        public static CalculatorResult SaturnRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD8)]
        public static CalculatorResult MercuryRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD8)]
        public static CalculatorResult KetuRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD8)]
        public static CalculatorResult VenusRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD8

        #region JUPITER PD8

        [EventCalculator(EventName.SunJupiterPD8)]
        public static CalculatorResult SunJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD8)]
        public static CalculatorResult MoonJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD8)]
        public static CalculatorResult MarsJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD8)]
        public static CalculatorResult RahuJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD8)]
        public static CalculatorResult JupiterJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD8)]
        public static CalculatorResult SaturnJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD8)]
        public static CalculatorResult MercuryJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD8)]
        public static CalculatorResult KetuJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD8)]
        public static CalculatorResult VenusJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD8

        #region SATURN PD8

        [EventCalculator(EventName.SunSaturnPD8)]
        public static CalculatorResult SunSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD8)]
        public static CalculatorResult MoonSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD8)]
        public static CalculatorResult MarsSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD8)]
        public static CalculatorResult RahuSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD8)]
        public static CalculatorResult JupiterSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD8)]
        public static CalculatorResult SaturnSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD8)]
        public static CalculatorResult MercurySaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD8)]
        public static CalculatorResult KetuSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD8)]
        public static CalculatorResult VenusSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD8

        #region MERCURY PD8

        [EventCalculator(EventName.SunMercuryPD8)]
        public static CalculatorResult SunMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD8)]
        public static CalculatorResult MoonMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD8)]
        public static CalculatorResult MarsMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD8)]
        public static CalculatorResult RahuMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD8)]
        public static CalculatorResult JupiterMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD8)]
        public static CalculatorResult SaturnMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD8)]
        public static CalculatorResult MercuryMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD8)]
        public static CalculatorResult KetuMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD8)]
        public static CalculatorResult VenusMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD8

        #region KETU PD8

        [EventCalculator(EventName.SunKetuPD8)]
        public static CalculatorResult SunKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD8)]
        public static CalculatorResult MoonKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD8)]
        public static CalculatorResult MarsKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD8)]
        public static CalculatorResult RahuKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD8)]
        public static CalculatorResult JupiterKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD8)]
        public static CalculatorResult SaturnKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD8)]
        public static CalculatorResult MercuryKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD8)]
        public static CalculatorResult KetuKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD8)]
        public static CalculatorResult VenusKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD8

        #region VENUS PD8

        [EventCalculator(EventName.SunVenusPD8)]
        public static CalculatorResult SunVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD8)]
        public static CalculatorResult MoonVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD8)]
        public static CalculatorResult MarsVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD8)]
        public static CalculatorResult RahuVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD8)]
        public static CalculatorResult JupiterVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD8)]
        public static CalculatorResult SaturnVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD8)]
        public static CalculatorResult MercuryVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD8)]
        public static CalculatorResult KetuVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD8)]
        public static CalculatorResult VenusVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD8

        #region DASA SPECIAL RULES

        [EventCalculator(EventName.Lord6And8Dasa)]
        public static CalculatorResult Lord6And8Dasa(Time time, Person person)
        {
            //The Dasa period of the lords of the 6th and the 8th
            // produce harmful results unless they acquire beneficence
            // otherwise.

            //get lord 6th house
            var lord6th = Calculate.LordOfHouse(HouseName.House6, person.BirthTime);

            //is lord 6th dasa occuring
            var isLord6thDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lord6th;

            //get lord 8th house
            var lord8th = Calculate.LordOfHouse(HouseName.House8, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord8thDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lord8th;

            //occuring if one of the conditions met
            var occuring = isLord6thDasa || isLord8thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord5And9Dasa)]
        public static CalculatorResult Lord5And9Dasa(Time time, Person person)
        {
            //The periods of lords of the 5th and the 9th are said
            // to be good, so much so that the periods of planets, which are
            // joined or otherwise related with them, are also supposed to
            // give rise to good.

            //get lord 5th house
            var lord5th = Calculate.LordOfHouse(HouseName.House5, person.BirthTime);

            //is lord 5th dasa occuring
            var isLord5thDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lord5th;

            //get lord 9th house
            var lord9th = Calculate.LordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord9thDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lord9th;

            //occuring if one of the conditions met
            var occuring = isLord5thDasa || isLord9thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord5And9DasaBhukti)]
        public static CalculatorResult Lord5And9DasaBhukti(Time time, Person person)
        {
            //The sub-period of the lord of the 5th in the major
            //period of the lord of the 9th or vice versa is supposed to
            //produce good effects.

            //get lord 5th house
            var lord5th = Calculate.LordOfHouse(HouseName.House5, person.BirthTime);
            //get lord 9th house
            var lord9th = Calculate.LordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 5th dasa occuring
            var isLord5thDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lord5th;
            var isLord5thBhukti = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD2 == lord5th;

            //is lord 9th dasa occuring
            var isLord9thDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lord9th;
            var isLord9thBhukti = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD2 == lord9th;

            //condition 1
            //bhukti 5th lord & dasa 9th lord
            var condition1 = isLord5thBhukti && isLord9thDasa;

            //condition 2
            //dasa 5th lord & bhukti 9th lord
            var condition2 = isLord5thDasa && isLord9thBhukti;

            //occuring if one of the conditions met
            var occuring = condition1 || condition2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.BhuktiDasaLordInBadHouses)]
        public static CalculatorResult BhuktiDasaLordInBadHouses(Time time, Person person)
        {
            //Unfavourable results will be realised when the sublord (bhukti)
            // and the major lord (dasa) are situated in the 6th and the 8th or
            // the 12th and the 2nd from each other respectively.

            //get bukti lord
            var buhktiLord = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD2;

            //get dasa lord =
            var dasaLord = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1;

            //condition 1
            //is bukti lord in 6th house at birth
            var bhuktiLordIn6th = Calculate.IsPlanetInHouse(buhktiLord, HouseName.House6, person.BirthTime);
            //is dasa lord in 8th house at birth
            var dasaLordIn8th = Calculate.IsPlanetInHouse(dasaLord, HouseName.House8, person.BirthTime);
            //check if both planets are in bad houses at the same time
            var buhktiDasaIn6And8 = bhuktiLordIn6th && dasaLordIn8th;

            //condition 2
            //is bukti lord in 12th house at birth
            var bhuktiLordIn12th = Calculate.IsPlanetInHouse(buhktiLord, HouseName.House12, person.BirthTime);
            //is dasa lord in 2nd house at birth
            var dasaLordIn2nd = Calculate.IsPlanetInHouse(dasaLord, HouseName.House2, person.BirthTime);
            //check if both planets are in bad houses at the same time
            var buhktiDasaIn12And2 = bhuktiLordIn12th && dasaLordIn2nd;

            //occuring if one of the conditions are met
            var occuring = buhktiDasaIn6And8 || buhktiDasaIn12And2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord2Dasa)]
        public static CalculatorResult Lord2Dasa(Time time, Person person)
        {
            //Lord of the 2nd in his Dasa gives wealth

            //get lord 2nd house
            var lordHouse2 = Calculate.LordOfHouse(HouseName.House2, person.BirthTime);

            //is lord 2nd dasa occuring
            var isLord2Dasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lordHouse2;

            //occuring if one of the conditions met
            var occuring = isLord2Dasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord3Dasa)]
        public static CalculatorResult Lord3Dasa(Time time, Person person)
        {
            //Lord of the 3rd during his Dasa gives new friends,
            // help to brothers, leadership, and physical pain (if afflicted).

            //get lord 3rd house
            var lordHouse3 = Calculate.LordOfHouse(HouseName.House3, person.BirthTime);

            //is lord 3rd dasa occuring
            var isLord3Dasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lordHouse3;

            //occuring if one of the conditions met
            var occuring = isLord3Dasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LagnaLordDasa)]
        public static CalculatorResult LagnaLordDasa(Time time, Person person)
        {
            //todo powerful not accounted for, future added it in

            //When Lagna (Ascendant) is powerful, during the Dasa
            // of lord of Lagna, favourable results can be expected to occur
            // - such as rise in profession, good health and general prosperity.

            //get lord of 1st house
            var lordHouse1 = Calculate.LordOfHouse(HouseName.House1, person.BirthTime);

            //is lord 1st house dasa occuring
            var isLord1Dasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == lordHouse1;

            //occuring if one of the conditions met
            var occuring = isLord1Dasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Saturn4thDasa)]
        public static CalculatorResult Saturn4thDasa(Time time, Person person)
        {
            //The Dasa period of Saturn, if it happens to be the 4th
            // Dasa, will be unfavourable. If Saturn is strong and favourably disposed,
            // the evil effects get considerably modified.

            //is saturn dasa occuring
            var isSaturnDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //is the 4th dasa
            var is4thDasa = VimshottariDasa.CurrentDasaCountFromBirth(person.BirthTime, time) == 4;

            //occuring if one of the conditions met
            var occuring = isSaturnDasa && is4thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Jupiter6thDasa)]
        public static CalculatorResult Jupiter6thDasa(Time time, Person person)
        {
            //The Dasa period of Jupiter will be unfavourable if it
            // happens to be the 6th Dasa.

            //is jupiter dasa occuring
            var isJupiterDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //is the 6th dasa
            var is6thDasa = VimshottariDasa.CurrentDasaCountFromBirth(person.BirthTime, time) == 6;

            //occuring if one of the conditions met
            var occuring = isJupiterDasa && is6thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ElevatedSunDasa)]
        public static CalculatorResult ElevatedSunDasa(Time time, Person person)
        {
            //If the Sun is elevated, he displays wisdom, gets
            //money, attains fame and happiness

            //is sun dasa occuring
            var isSunDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //is sun elevated
            //todo what is elvated?
            var isSunElevated = false;

            //occuring if one of the conditions met
            var occuring = isSunDasa && isSunElevated;

            return CalculatorResult.New(occuring, PlanetName.Sun);
        }

        [EventCalculator(EventName.SunWithLord9Or10Dasa)]
        public static CalculatorResult SunWithLord9Or10Dasa(Time time, Person person)
        {
            //The Sun in good position, in own house or joined with lord of 9 or
            //10 - happiness, gains, riches, honours

            //is sun dasa occuring
            var isSunDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //is sun in own house
            var sunInOwn = Calculate.IsPlanetInOwnHouse(PlanetName.Sun, person.BirthTime);

            //is sun joined (same house) with lord 9 or 10
            var a = Calculate.IsPlanetSameHouseWithHouseLord(9, PlanetName.Sun, person.BirthTime);
            var b = Calculate.IsPlanetSameHouseWithHouseLord(10, PlanetName.Sun, person.BirthTime);
            var sunJoined9Or10 = a || b;

            var sunInGoodPosition = sunInOwn || sunJoined9Or10;

            //occuring if one of the conditions met
            var occuring = isSunDasa && sunInGoodPosition;

            return CalculatorResult.New(occuring, new[] { HouseName.House9, HouseName.House10 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.SunWithLord5Dasa)]
        public static CalculatorResult SunWithLord5Dasa(Time time, Person person)
        {
            //the Sun with lord of 5 - birth of children.

            //is sun dasa occuring
            var isSunDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //is sun with lord of 5th
            var sunWithLord5th = Calculate.IsPlanetSameHouseWithHouseLord(5, PlanetName.Sun, person.BirthTime);

            //occuring if one of the conditions met
            var occuring = isSunDasa && sunWithLord5th;

            return CalculatorResult.New(occuring, new[] { HouseName.House5 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.SunWithLord2Dasa)]
        public static CalculatorResult SunWithLord2Dasa(Time time, Person person)
        {
            //The Sun when related to lord of 2 - becomes rich, earns money, secures
            //property, gains, favours from influential persons.

            //is sun dasa occuring
            var isSunDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            //is sun with lord of 2nd
            var sunWithLord2nd = Calculate.IsPlanetSameHouseWithHouseLord(2, PlanetName.Sun, person.BirthTime);

            //occuring if one of the conditions met
            var occuring = isSunDasa && sunWithLord2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.SunBadPositionDasa)]
        public static CalculatorResult SunBadPositionDasa(Time time, Person person)
        {
            //The Sun when debilitated or occupies the 6th or the
            //8th house or in cojunction with evil planets contracts evil diseases, loss of wealth, suffers from reverses
            //in employment, penalty and becomes ill.

            //TODO
            return new() { Occuring = false };
        }

        /// <summary>
        /// TODO NOTES : this used to be pumped into life predictor,
        ///              but now its just sitting here because suspected to overthrow the final prediction
        /// </summary>
        [EventCalculator(EventName.ExaltedSunDasa)]
        public static CalculatorResult ExaltedSunDasa(Time time, Person person)
        {
            //The Dasa of the Sun in deep exaltation : Sudden
            //gains in cattle and wealth, much travelling in eastern
            //countries, residence in foreign countries, quarrels
            //among friends and relations, pleasure trios and picnic
            //parties and lovely women.

            //is sun dasa occuring
            var isSunDasa = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time).PD1 == PlanetName.Sun;

            var isSunExalted = Calculate.IsPlanetExalted(PlanetName.Sun, time);

            //conditions met
            var occuring = isSunDasa && isSunExalted;

            return CalculatorResult.New(occuring, PlanetName.Sun);
        }

        #endregion DASA SPECIAL RULES

        #region SPECIAL SHORTCUT FUNCTIONS

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD2PlanetPD3(Time time, Person person, PlanetName bhuktiPlanet, PlanetName antaramPlanet)
        {
            //get dasas for current time
            var currentPlanetPeriod = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check bhukti
            var isCorrectBhukti = currentPlanetPeriod.PD2 == bhuktiPlanet;

            //check antaram
            var isCorrectAntaram = currentPlanetPeriod.PD3 == antaramPlanet;

            //occuring if all conditions met
            var occuring = isCorrectAntaram && isCorrectBhukti;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = VimshottariDasa.PlanetDasaMajorPlanetAndMinorRelationship(bhuktiPlanet, antaramPlanet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD3PlanetPD4(Time time, Person person, PlanetName antaramPlanet, PlanetName sukshmaPlanet)
        {
            //get dasas for current time
            var currentPlanetPeriod = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check Antaram
            var isCorrectAntaram = currentPlanetPeriod.PD3 == antaramPlanet;

            //check Sukshma
            var isCorrectSukshma = currentPlanetPeriod.PD4 == sukshmaPlanet;

            //occuring if all conditions met
            var occuring = isCorrectAntaram && isCorrectSukshma;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = VimshottariDasa.PlanetDasaMajorPlanetAndMinorRelationship(antaramPlanet, sukshmaPlanet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD4PlanetPD5(Time time, Person person, PlanetName PD4Planet, PlanetName PD5Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check PD4
            var isCorrectPD4 = currentPlanetPeriod.PD4 == PD4Planet;

            //check PD5
            var isCorrectPD5 = currentPlanetPeriod.PD5 == PD5Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD4 && isCorrectPD5;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = VimshottariDasa.PlanetDasaMajorPlanetAndMinorRelationship(PD4Planet, PD5Planet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD5PlanetPD6(Time time, Person person, PlanetName PD5Planet, PlanetName PD6Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check PD5
            var isCorrectPD5 = currentPlanetPeriod.PD5 == PD5Planet;

            //check PD6
            var isCorrectPD6 = currentPlanetPeriod.PD6 == PD6Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD5 && isCorrectPD6;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = VimshottariDasa.PlanetDasaMajorPlanetAndMinorRelationship(PD5Planet, PD6Planet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD6PlanetPD7(Time time, Person person, PlanetName PD6Planet, PlanetName PD7Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check PD6
            var isCorrectPD6 = currentPlanetPeriod.PD6 == PD6Planet;

            //check PD7
            var isCorrectPD7 = currentPlanetPeriod.PD7 == PD7Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD6 && isCorrectPD7;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = VimshottariDasa.PlanetDasaMajorPlanetAndMinorRelationship(PD6Planet, PD7Planet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD7PlanetPD8(Time time, Person person, PlanetName PD7Planet, PlanetName PD8Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = VimshottariDasa.CurrentDasa8Levels(person.BirthTime, time);

            //check PD7
            var isCorrectPD7 = currentPlanetPeriod.PD7 == PD7Planet;

            //check PD8
            var isCorrectPD8 = currentPlanetPeriod.PD8 == PD8Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD7 && isCorrectPD8;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = VimshottariDasa.PlanetDasaMajorPlanetAndMinorRelationship(PD7Planet, PD8Planet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        #endregion


    }

}

//--------------ARCHIVED CODE-----------------
//bool IsLagnaLordInLagnaOccuring(Time time)
//{
////lagna lord is in same sign as first house

////get lord of lagna (house 1)
//PlanetName lordOfLagna = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);

////get house 1 sign
//var house1Sign = AstronomicalCalculator.GetHouseSignName(1, time);

////get sign lagna lord is in
//var signLagnaLordIsIn = AstronomicalCalculator.GetPlanetRasiSign(lordOfLagna, time);

//    //if the house 1 sign & lagna lord sign is same
//    if (house1Sign == signLagnaLordIsIn.GetSignName())
//{
//    //event is occuring
//    return Prediction.IsOccuring();
//}
//else
//{
//    return Prediction.NotOccuring();
//}
//}
//bool Is7thLordIn7thOccuring(Time time)
// {
// //7t lord is in same sign as 7th house
// 
// //get lord of 7th 
// PlanetName lordOf7th = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
// 
// //get house 7 sign
// var house7Sign = AstronomicalCalculator.GetHouseSignName(7, time);
// 
// //get sign 7th lord is in
// var sign7thLordIsIn = AstronomicalCalculator.GetPlanetRasiSign(lordOf7th, time);
// 
// //if the house 7 sign & 7th lord sign is same
// if (house7Sign == sign7thLordIsIn.GetSignName())
// {
// //event is occuring
// return Prediction.IsOccuring();
// }
// else
// {
// return Prediction.NotOccuring();
// }
// }
// 