//using Microsoft.VisualBasic.Devices;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static VedAstro.Library.PlanetName;

//namespace VedAstro.Library
//{


//    public class CalculateHoroscope
//    {
//        /// <summary>
//        /// Definition.-If Jupiter is in a kendra from the
//        /// Moon the combination goes under the name Gajakesari.
//        /// 
//        /// Results.-Many relations, polite and generous,
//        /// builder of villages and towns or magistrate over
//        /// them; will have a lasting reputation even long after
//        /// death.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.GajakesariYoga)]
//        public static CalculatorResult GajakesariYoga(Time birthTime)
//        {
//            //If Jupiter is in a kendra from the Moon
//            var jupiterInKendraFromMoon = Calculate.IsPlanetInKendraFromPlanet(Jupiter, Moon, birthTime);

//            return CalculatorResult.New(jupiterInKendraFromMoon, new[] { Jupiter, Moon }, birthTime);
//        }

//        /// <summary>
//        /// Definition: If there are planets (excepting the
//        /// Sun) in the second house from the Moon, Sunapha
//        /// is caused.
//        /// 
//        /// Results: Self-earned property, king, ruler or his
//        /// equal, intelligent, wealthy and good reputation.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.SunaphaYoga)]
//        public static CalculatorResult SunaphaYoga(Time birthTime)
//        {
//            //sun not in house 2 from moon
//            var sunMoonDistance = Calculate.SignDistanceFromPlanetToPlanet(Moon, Sun, birthTime);
//            var sunNotIn2 = sunMoonDistance != 2;

//            //If there are planets
//            //get sign 2nd house from moon
//            var moon2ndHseSign = Calculate.SignCountedFromPlanetSign(2, Moon, birthTime);

//            //get planets in that 2nd hse sign
//            var planetsIn2 = Calculate.PlanetsInSign(moon2ndHseSign, birthTime);

//            //both conditions have to be met
//            var isOccuring = sunNotIn2 && planetsIn2.Any();

//            return CalculatorResult.New(isOccuring, new[] { House2 }, new[] { Moon }, birthTime);
//        }

//        /// <summary>
//        /// Definition: If there are planets in the 12th from
//        /// the Moon, Anapha Yoga is formed.
//        /// 
//        /// Results: Well-formed organs, majestic appearance,
//        /// good reputation, polite, generous, self-respect,
//        /// fond of dress and sense pleasures. In later life,
//        /// renunciation and austerity
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.AnaphaYoga)]
//        public static CalculatorResult AnaphaYoga(Time birthTime)
//        {
//            //If there are planets in the 12th from moon
//            var moon12ndHseSign = Calculate.SignCountedFromPlanetSign(12, Moon, birthTime);

//            //get planets in that 12th hse sign from moon
//            var planetsIn12 = Calculate.PlanetsInSign(moon12ndHseSign, birthTime);

//            //Remarks.- In Anapha also the Sun is not taken
//            //into account. The remarks made for Sunapha apply
//            //to this also with slight variation.

//            //remove sun if found
//            planetsIn12.RemoveAll(x => x.Name == PlanetNameEnum.Sun);

//            //both conditions have to be met
//            var isOccuring = planetsIn12.Any();

//            return CalculatorResult.New(isOccuring, new[] { House12 }, new[] { Moon }, birthTime);
//        }

//        /// <summary>
//        /// Definition: If there are planets on either side of
//        /// the Moon, the combination goes under the name of
//        /// Dhurdhura.
//        /// 
//        /// Results: The native is bountiful. He will be
//        /// blessed with much wealth and conveyances.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.DhurdhuraYoga)]
//        public static CalculatorResult DhurdhuraYoga(Time birthTime)
//        {
//            //If there are planets on either side of the Moon
//            var topSideSign = Calculate.SignCountedFromPlanetSign(2, Moon, birthTime);
//            var planetsInTop = Calculate.PlanetsInSign(topSideSign, birthTime).Any();

//            var bottomSideSign = Calculate.SignCountedFromPlanetSign(12, Moon, birthTime);
//            var planetsInBottom = Calculate.PlanetsInSign(bottomSideSign, birthTime).Any();

//            //on either side of  the Moon
//            var planetOnBothSides = planetsInBottom || planetsInTop;

//            return CalculatorResult.New(planetOnBothSides, new[] { Moon }, birthTime);
//        }

//        /// <summary>
//        /// Definition: When there are no planets on both
//        /// sides of the Moon, Kemadruma Yoga is formed
//        /// 
//        /// Results: The person will be dirty, sorrowful,
//        /// doing unrighteous deeds, poor, dependent, a rogue
//        /// and a swindler
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.KemadrumaYoga)]
//        public static CalculatorResult KemadrumaYoga(Time birthTime)
//        {
//            //If there are planets on either side of the Moon
//            //count to sign next to moon on right
//            var topSideSign = Calculate.SignCountedFromPlanetSign(2, Moon, birthTime);
//            var noPlanetsInTop = Calculate.PlanetsInSign(topSideSign, birthTime).Any() == false;

//            //count around to sign left side of moon (since counter only goes one way)
//            var bottomSideSign = Calculate.SignCountedFromPlanetSign(12, Moon, birthTime);
//            var noPlanetsInBottom = Calculate.PlanetsInSign(bottomSideSign, birthTime).Any() == false;

//            //no planets on both sides of the Moon
//            var planetOnBothSides = noPlanetsInBottom && noPlanetsInTop;

//            return CalculatorResult.New(planetOnBothSides, new[] { Moon }, birthTime);
//        }


//        /// <summary>
//        /// Definition: If Mars conjoins the Moon this
//        /// yoga is formed.
//        /// 
//        /// Results: Earnings through unscrupulous means,
//        /// a seller of women, treating mother harshly and doing
//        /// mischief to her and other relatives
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.ChandraMangalaYoga)]
//        public static CalculatorResult ChandraMangalaYoga(Time birthTime)
//        {
//            //If Mars conjoins the Moon
//            var marsConjunctMoon = Calculate.IsPlanetConjunctWithPlanet(Mars, Moon, birthTime);

//            return CalculatorResult.New(marsConjunctMoon, new[] { Moon, Mars }, birthTime);
//        }

//        /// <summary>
//        /// Definition: If benefics are situated in the 6th,
//        /// 7th and 8th from the Moon, the combination goes
//        /// under the name of Adhi Yoga
//        /// 
//        /// Results: The person will be polite and trustworthy, will have an enjoyable and happy life,
//        /// surrounded by luxuries and affluence, will inflict
//        /// defeats on his enemies, will be healthy and will live long.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.AdhiYoga)]
//        public static CalculatorResult AdhiYoga(Time birthTime)
//        {
//            //If benefics are situated in the 6th,7th and 8th from the Moon
//            int[] signsFromList = new[] { 6, 7, 8 };

//            //Varahamihira distinctly observes Sounyehi-implying
//            // clearly only the benefics, vz., Mercury, Jupiter and Venus.
//            PlanetName[] beneficList = new[] { Mercury, Jupiter, Venus }; //override standard benefics

//            var isOccuring = Calculate.IsPlanetsInSignsFromPlanet(signsFromList, beneficList, Moon, birthTime);

//            return CalculatorResult.New(isOccuring, new[] { Moon }, birthTime);
//        }

//        /// <summary>
//        /// Definition: Chatussagara Yoga is caused when
//        /// all the kendras are occupied by planets
//        /// 
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.ChatussagaraYoga)]
//        public static CalculatorResult ChatussagaraYoga(Time birthTime)
//        {
//            var isOccuring = true; //start with is occuring

//            //go through all the kendras if any one house,
//            //does not have a planet, than exit and mark as not occuring
//            //kendra house (1,4,7,10)
//            var kendraList = new HouseName[] { House1, House4, House7, House10 };
//            foreach (var house in kendraList)
//            {
//                //true if no planet found
//                var noPlanet = Calculate.PlanetsInHouse(house, birthTime).Any() == false;
//                if (noPlanet) { isOccuring = false; break; } //set as not occuring, stop checking anymore

//            }

//            return CalculatorResult.New(isOccuring, kendraList, birthTime);
//        }

//        /// <summary>
//        /// Definition: If benefics occupy the upachayas
//        /// (3,6,10 and 11) either from the ascendant or from
//        /// the Moon, the combination goes under the name of
//        /// Vasumathi Yoga.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.VasumathiYoga)]
//        public static CalculatorResult VasumathiYoga(Time birthTime)
//        {
//            //list upachayas houses (3,6,10 and 11)
//            var upachayasList = new[] { 3, 6, 10, 11 };

//            //check if there is benefics in upachayas from the ascendant/lagna
//            var beneficsFoundInUpachFromLagna = Calculate.IsBeneficsInSignsFromLagna(upachayasList, birthTime);

//            //check if there is benefics in upachayas from the Moon
//            var beneficsFoundInUpachFromMoon = Calculate.IsBeneficsInSignsFromPlanet(upachayasList, Moon, birthTime);

//            //for yoga to occur benefics has to be in either one above
//            var isOccuring = beneficsFoundInUpachFromLagna || beneficsFoundInUpachFromMoon;

//            //tell caller if Vasumathi Yoga is present in horoscope
//            return CalculatorResult.New(isOccuring);
//        }

//        /// <summary>
//        /// Definition: Jupiter, Venus, Mercury and the
//        /// Moon should be in Lagna or they should be placed
//        /// in kendra.
//        /// 
//        /// Results: The native will possess an attractive
//        /// appearance and he will be endowed with all the
//        /// good qualities of high personages.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.RajalakshanaYoga)]
//        public static CalculatorResult RajalakshanaYoga(Time birthTime)
//        {
//            //planets to check
//            var planetList = new[] { Jupiter, Venus, Mercury, Moon };

//            //is any of the planet in Kendra
//            var isInKendra = Calculate.IsPlanetInKendra(planetList, birthTime);

//            //is any of the planet in Lagna/Ascendant
//            var isInLagna = Calculate.IsAnyPlanetInHouse(planetList.ToList(), House1, birthTime);

//            //is occurring if either is true
//            var isOccuring = isInKendra || isInLagna;

//            return CalculatorResult.New(isOccuring);
//        }

//        /// <summary>
//        /// Definition: The Lagna is occupied by a malefic
//        ///	with Gulika in a trine: or Gulika is associated with
//        ///	the lords of Kendras and Thrikonas; or the lord of
//        ///	Lagna is combined with Rahu, Saturn or Kethu.
//        ///
//        ///	Results: The native will always entertain feelings
//        ///	of suspicion towards others around him. He is afraid
//        ///	of being cheated, swindled and robbed.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.VanchanaChoraBheethiYoga)]
//        public static CalculatorResult VanchanaChoraBheethiYoga(Time birthTime)
//        {
//            //todo not implemented yet!
//            return CalculatorResult.NotOccuring();
//        }

//        /// <summary>
//        /// Definition: The Moon in the 12th, 6th or 8th
//        /// from Jupiter gives rise to Sakata Yoga
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.SakataYoga)]
//        public static CalculatorResult SakataYoga(Time birthTime)
//        {
//            //sign counts to check (house)
//            int[] signsFromList = new[] { 6, 8, 12 };

//            //check for jupiter only
//            PlanetName[] planetList = new[] { Moon };

//            var isOccuring = Calculate.IsPlanetsInSignsFromPlanet(signsFromList, planetList, Jupiter, birthTime);

//            return CalculatorResult.New(isOccuring, new[] { Moon, Jupiter }, birthTime);

//        }

//        /// <summary>
//        /// Definition: The 10th from the Moon or Lagna should be occupied by a benefic planet
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.AmalaYoga)]
//        public static CalculatorResult AmalaYoga(Time birthTime)
//        {
//            //The 10th from the Moon should be occupied by a benefic planet
//            int[] _10thFrom = new[] { 10 };
//            var _10thMoonIsBenefics = Calculate.IsBeneficsInSignsFromPlanet(_10thFrom, Moon, birthTime);

//            //The 10th from the Lagna should be occupied by a benefic planet
//            var _10thLagnaIsBenefics = Calculate.IsBeneficsInSignsFromLagna(_10thFrom, birthTime);

//            //is occurring if either is true
//            var isOccuring = _10thMoonIsBenefics || _10thLagnaIsBenefics;

//            return CalculatorResult.New(isOccuring);

//        }

//        /// <summary>
//        /// Definition: Benefics being disposed in Kendras,
//        /// the 6th and 8th houses should either be unoccupied
//        /// or occupied by benefic planets. This combination
//        /// goes under the name of Parvata Yoga
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.ParvataYoga)]
//        public static CalculatorResult ParvataYoga(Time birthTime)
//        {
//            //Benefics being disposed in Kendras
//            var beneficsInKendra = Calculate.IsBeneficsInKendra(birthTime);

//            //the 6th and 8th must be free
//            var _6thEmpty = Calculate.PlanetsInHouse(House6, birthTime).Count == 0;
//            var _8thEmpty = Calculate.PlanetsInHouse(House8, birthTime).Count == 0;
//            var _6th8thIsEmpty = _6thEmpty && _8thEmpty;

//            //the 6th and 8th occupied by benefics
//            var _6thHasBenefics = Calculate.IsBeneficPlanetInHouse(House6, birthTime);
//            var _8thHasBenefics = Calculate.IsBeneficPlanetInHouse(House8, birthTime);
//            var _6th8thHasBenefics = _6thHasBenefics && _8thHasBenefics;

//            //is occurring if conditions meet
//            var isOccuring = beneficsInKendra && (_6th8thIsEmpty || _6th8thHasBenefics);

//            return CalculatorResult.New(isOccuring);

//        }

//        /// <summary>
//        /// Definition:  Lords of the fourth and ninth houses
//        /// should be in Kendras from each other and the lord
//        /// of Lagna should be strongly disposed.
//        /// </summary>
//        [HoroscopeCalculator(HoroscopeName.KahalaYoga)]
//        public static CalculatorResult KahalaYoga(Time birthTime)
//        {
//            //# Condition 1
//            //get lords of the 4th and 9th houses
//            var lordOf4th = Calculate.LordOfHouse(House4, birthTime);
//            var lordOf9th = Calculate.LordOfHouse(House9, birthTime);

//            //should be in Kendras from each other
//            var possibleKendra1 = Calculate.IsPlanetInKendraFromPlanet(lordOf9th, lordOf4th, birthTime);
//            var possibleKendra2 = Calculate.IsPlanetInKendraFromPlanet(lordOf4th, lordOf9th, birthTime);
//            var isKendraFromEachOther = possibleKendra1 || possibleKendra2;

//            //# Condition 2
//            //lord of Lagna should be strongly disposed.
//            var lagnaLord = Calculate.LordOfHouse(House1, birthTime);

//            //strength here is based on minimum point set by BV Raman in Bala book
//            var lagnaLordIsStrong = Calculate.IsPlanetStrongInShadbala(lagnaLord, birthTime);

//            //is occurring if conditions meet
//            var isOccuring = isKendraFromEachOther && lagnaLordIsStrong;

//            return CalculatorResult.New(isOccuring);
//        }
//    }
//}
