using System.Collections.Generic;
using System.Linq;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// A collection of methods used to calculate if an event is occuring
    /// Note: Attributes are used to link a particular method to the event data stored in database
    /// </summary>
    public static class EventCalculatorMethods
    {
        #region PERSONAL
        //[EventCalculator(EventName.GoodTarabala)] TODO Can be removed and fucntion moved to astronomical
        public static CalculatorResult IsGoodTarabalaOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

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
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

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
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);
            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Janma && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaSampatStrong)]
        public static CalculatorResult IsTarabalaSampatStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sampat && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaVipatStrong)]
        public static CalculatorResult IsTarabalaVipatStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Vipat && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaKshemaStrong)]
        public static CalculatorResult IsTarabalaKshemaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Kshema && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaPratyakStrong)]
        public static CalculatorResult IsTarabalaPratyakStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Pratyak && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaSadhanaStrong)]
        public static CalculatorResult IsTarabalaSadhanaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sadhana && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaNaidhanaStrong)]
        public static CalculatorResult IsTarabalaNaidhanaStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Naidhana && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaMitraStrong)]
        public static CalculatorResult IsTarabalaMitraStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Mitra && tarabala.GetCycle() == 1;

            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaParamaMitraStrong)]
        public static CalculatorResult IsTarabalaParamaMitraStrongOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.ParamaMitra && tarabala.GetCycle() == 1;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaJanmaMiddling)]
        public static CalculatorResult IsTarabalaJanmaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Janma && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaSampatMiddling)]
        public static CalculatorResult IsTarabalaSampatMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sampat && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaVipatMiddling)]
        public static CalculatorResult IsTarabalaVipatMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Vipat && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaKshemaMiddling)]
        public static CalculatorResult IsTarabalaKshemaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Kshema && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaPratyakMiddling)]
        public static CalculatorResult IsTarabalaPratyakMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Pratyak && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaSadhanaMiddling)]
        public static CalculatorResult IsTarabalaSadhanaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sadhana && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaNaidhanaMiddling)]
        public static CalculatorResult IsTarabalaNaidhanaMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Naidhana && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaMitraMiddling)]
        public static CalculatorResult IsTarabalaMitraMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Mitra && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaParamaMitraMiddling)]
        public static CalculatorResult IsTarabalaParamaMitraMiddlingOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.ParamaMitra && tarabala.GetCycle() == 2;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaJanmaWeak)]
        public static CalculatorResult IsTarabalaJanmaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Janma && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaSampatWeak)]
        public static CalculatorResult IsTarabalaSampatWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sampat && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaVipatWeak)]
        public static CalculatorResult IsTarabalaVipatWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Vipat && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaKshemaWeak)]
        public static CalculatorResult IsTarabalaKshemaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Kshema && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.TarabalaPratyakWeak)]
        public static CalculatorResult IsTarabalaPratyakWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Pratyak && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaSadhanaWeak)]
        public static CalculatorResult IsTarabalaSadhanaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Sadhana && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaNaidhanaWeak)]
        public static CalculatorResult IsTarabalaNaidhanaWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Naidhana && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaMitraWeak)]
        public static CalculatorResult IsTarabalaMitraWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

            //event occuring based on tarabala name & cycle
            var occuring = tarabala.GetName() == TarabalaName.Mitra && tarabala.GetCycle() == 3;
            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TarabalaParamaMitraWeak)]
        public static CalculatorResult IsTarabalaParamaMitraWeakOccuring(Time time, Person person)
        {
            //get tarabala for current time
            var tarabala = AstronomicalCalculator.GetTarabala(time, person);

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
            var chandrabalaNumber = AstronomicalCalculator.GetChandrabala(time, person);

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
                var moonPhase = AstronomicalCalculator.GetLunarDay(time).GetMoonPhase();

                //check if phase is correct 
                var rightPhase = moonPhase == MoonPhase.BrightHalf;

                //if not correct phase, end here as not occuring
                if (rightPhase == false) { return false; }


                //2. Moon occupies a benefic sign
                var moonSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, time);
                var relationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(PlanetName.Moon, moonSign.GetSignName(), time);

                //check if sign is benefic 
                var isBenefic = relationship == PlanetToSignRelationship.OwnVarga || //Swavarga - own varga
                                relationship == PlanetToSignRelationship.FriendVarga || //Mitravarga - friendly varga
                                relationship == PlanetToSignRelationship.BestFriendVarga; //Adhi Mitravarga - Intimate friend varga

                //if not benefic, end here as not occuring
                if (isBenefic == false) { return false; }


                //3. Moon occupies a benefic Navamsa sign
                var moonNavamsaSign = AstronomicalCalculator.GetPlanetNavamsaSign(PlanetName.Moon, time);
                var navamsaRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(PlanetName.Moon, moonNavamsaSign, time);

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
                var lord8th = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);

                //get relationship between moon and 8th lord
                var relationship =
                    AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(PlanetName.Moon, lord8th, time);

                var isFriends = relationship == PlanetToPlanetRelationship.AdhiMitra ||
                                relationship == PlanetToPlanetRelationship.Mitra;

                return isFriends;
            }

        }

        [EventCalculator(EventName.GoodPanchaka)]
        public static CalculatorResult IsGoodPanchakaOccuring(Time time, Person person)
        {
            //get occuring panchaka
            var panchakaName = AstronomicalCalculator.GetPanchaka(time);

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
            var birthRulingConstellation = AstronomicalCalculator.GetMoonConstellation(person.BirthTime);

            //get current rulling constellation
            var currentRulingConstellation = AstronomicalCalculator.GetMoonConstellation(time);

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
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();
            //get ruling constellation
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();


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
            var weekday = AstronomicalCalculator.GetDayOfWeek(time);

            //1. may be taken on Saturday or Monday.
            //right weekdays to look for
            var rightWeekday = weekday == DayOfWeek.Saturday || weekday == DayOfWeek.Monday;

            //if not correct weekdays, end here as not occuring
            if (rightWeekday == false) { return CalculatorResult.NotOccuring(); }


            //2. Aries, Taurus. Cancer and Virgo are auspicious
            //get rising sign
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Aries
                            || risingSign == ZodiacName.Taurus
                            || risingSign == ZodiacName.Cancer
                            || risingSign == ZodiacName.Virgo;

            //if not correct sign, end here as not occuring
            if (rightSign == false) { return CalculatorResult.NotOccuring(); }


            //3. The 8th house must be unoccupied
            var planets8thHouse = AstronomicalCalculator.GetPlanetsInHouse(8, time);

            //if got planets in 8th house, event not occuring
            if (planets8thHouse.Any()) { return CalculatorResult.NotOccuring(); }


            //4. Mercury is free from affliction
            var mercuryIsAfflicted = AstronomicalCalculator.IsMercuryAfflicted(time);

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
            var karana = AstronomicalCalculator.GetKarana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Taitula;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.MarsVenusIn7th)]
        public static CalculatorResult MarsVenusIn7th(Time time, Person person)
        {
            //When Mars and Venus are in the 7th, the boy or girl concerned will have strong sex instincts
            //and such an individual should be mated to one who has similar instincts

            //mars in 7th at birth
            var marsIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Mars, 7);

            //venus in 7th at birth
            var venusIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 7);

            //occuring if all conditions met
            var occuring = marsIn7th && venusIn7th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryOrJupiterIn7th)]
        public static CalculatorResult MercuryOrJupiterIn7th(Time time, Person person)
        {
            // Mercury or Jupiter in the 7th, makes one under-sexed.
            // And such an individual should not be mated to a person with strong sex instincts.

            //Mercury in 7th at birth
            var mercuryIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Mercury, 7);

            //Jupiter in 7th at birth
            var jupiterIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Jupiter, 7);

            //occuring if either conditions met
            var occuring = mercuryIn7th || jupiterIn7th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoLagna7thLordSaturnIn2)]
        public static CalculatorResult LeoLagna7thLordSaturnIn2(Time time, Person person)
        {
            //When Leo is Lagna and the 7th lord Saturn is in the 2nd, the
            // husband will be subservient to the wife carrying out all her orders.

            //lagna is leo
            var leoIsLagna = AstronomicalCalculator.GetHouseSignName(1, person.BirthTime) == ZodiacName.Leo;

            //is 7th lord saturn
            var isLord7thSaturn = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime) ==
                                  PlanetName.Saturn;

            //is saturn in 2nd
            var isSaturnIn2nd =
                AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Saturn, 2);


            //occuring conditions met
            var occuring = leoIsLagna && isLord7thSaturn && isSaturnIn2nd;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnIn7thNotLagnaLord)]
        public static CalculatorResult SaturnIn7thNotLagnaLord(Time time, Person person)
        {
            //Saturn in the 7th house is also indicative of unhappiness in marriage
            // unless Saturn happens to be either lord of Lagna or lord of the 7th.

            //is saturn in 7th house
            var isSaturnIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Saturn, 7);

            //saturn is not lord of lagna
            var saturnNotLagnaLord =
                AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime) != PlanetName.Saturn;

            //saturn is not lord of 7th
            var saturnNot7thLord =
                AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime) != PlanetName.Saturn;


            //occuring conditions met
            var occuring = isSaturnIn7th && saturnNotLagnaLord && saturnNot7thLord;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsIn7thNoBenefics)]
        public static CalculatorResult MarsIn7thNoBenefics(Time time, Person person)
        {
            //If Kuja is in the 7th house unaspected or not joined by benefics,
            //there will be frequent quarrels in the married life often leading to
            //misunderstandings and separation.

            //is mars in 7th house
            var isMarsIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Mars, 7);

            //no benefics aspecting 7th house
            var beneficsNotAspect7th = !AstronomicalCalculator.IsBeneficPlanetAspectHouse(HouseName.House7, person.BirthTime);

            //no benefics located in 7th
            var beneficNotFoundIn7th = !AstronomicalCalculator.IsBeneficPlanetInHouse(7, person.BirthTime);

            //occuring conditions met
            var occuring = isMarsIn7th && beneficsNotAspect7th && beneficNotFoundIn7th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunVenusIn5th7th9th)]
        public static CalculatorResult SunVenusIn5th7th9th(Time time, Person person)
        {
            //According to Prasna Marga the famous Kerala work on Astrology, if
            //the Sun and Venus occupy the 5th, 7th, or 9th house then the native will
            //lack marital happiness.
            //
            //NOTE : *is intepreted as in the same house at the same time

            //is sun & venus in 5th
            var isSunIn5th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Sun, 5);
            var isVenusIn5th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 5);
            var sunAndVenusIn5th = isSunIn5th && isVenusIn5th;

            //is sun & venus in 7th
            var isSunIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Sun, 7);
            var isVenusIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 7);
            var sunAndVenusIn7th = isSunIn7th && isVenusIn7th;

            //is sun & venus in 9th
            var isSunIn9th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Sun, 9);
            var isVenusIn9th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 9);
            var sunAndVenusIn9th = isSunIn9th && isVenusIn9th;


            //occuring conditions met
            var occuring = sunAndVenusIn5th || sunAndVenusIn7th || sunAndVenusIn9th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord7And1Friends)]
        public static CalculatorResult Lord7And1Friends(Time time, Person person)
        {

            //If the lords of the 7th and 1st are friends then the native will be loved
            //by his wife. Otherwise there will be no harmony.


            //get lord of 7th and 1st house
            var lord7 = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime);
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime);

            //get the relationship
            var lord7And1Relationship = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(lord7, lord1,
                person.BirthTime);

            //occuring only if best friends or normal friends nothing else
            var occuring = (lord7And1Relationship == PlanetToPlanetRelationship.AdhiMitra) ||
                           (lord7And1Relationship == PlanetToPlanetRelationship.Mitra);

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord7And1NotFriends)]
        public static CalculatorResult Lord7And1NotFriends(Time time, Person person)
        {

            //If the lords of the 7th and 1st are friends then the native will be loved
            //by his wife. Otherwise* there will be no harmony.
            //
            //* Intepreted as enemies or bitter enemies only, neutral is not inlcuded


            //get lord of 7th and 1st house
            var lord7 = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime);
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime);

            //get the relationship
            var lord7And1Relationship = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(lord7, lord1,
                person.BirthTime);

            //occuring only if bitter enemies or normal enemies nothing else
            var occuring = (lord7And1Relationship == PlanetToPlanetRelationship.AdhiSatru) ||
                           (lord7And1Relationship == PlanetToPlanetRelationship.Satru);

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnIn7th)]
        public static CalculatorResult SaturnIn7th(Time time, Person person)
        {
            //Saturn in the 7th
            //confers stability in the marriage but the, husband or wife manifests
            //coldness and not warmth.

            //is saturn in 7th house
            var isSaturnIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Saturn, 7);

            //occuring conditions met
            var occuring = isSaturnIn7th;

            return new() { Occuring = occuring };
        }



        #endregion

        #region GENERAL

        [EventCalculator(EventName.SakunaKarana)]
        public static CalculatorResult IsSakunaKaranaOccuring(Time time, Person person)
        {
            //For getting initiations into mantras Sakuni Karana is propitious.   

            var karana = AstronomicalCalculator.GetKarana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Sakuna;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.BavaKarana)]
        public static CalculatorResult IsBavaKaranaOccuring(Time time, Person person)
        {
            //Thus Bava is auspicious for starting works of permanent importance while
            var karana = AstronomicalCalculator.GetKarana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Bava;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.BhadraKarana)]
        public static CalculatorResult IsBhadraKaranaOccuring(Time time, Person person)
        {
            //Bhadra is unfit for any good work but is eminently suitable for
            //violent and cruel deeds.

            var karana = AstronomicalCalculator.GetKarana(time);

            //occuring if correct Karana
            var occuring = karana == Karana.Visti;
            return new() { Occuring = occuring };

        }

        [EventCalculator(EventName.Ekadashi)]
        public static CalculatorResult IsEkadashiOccuring(Time time, Person person)
        {
            // It is the 11th tithi

            //get lunar day
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var houseVenusIsIn = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus);

            //if venus is in 6th house
            if (houseVenusIsIn == 6)
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
            var houseMarsIsIn = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars);

            //if mars is in 8th house
            if (houseMarsIsIn == 8)
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
            var listOfEvilPlanets = AstronomicalCalculator.GetMaleficPlanetList(time);

            //2.0 Check if evil planets are in house 12 & 2

            //mark evil planet not found in 12th house first
            var evilPlanetFoundInHouse12 = false;
            //mark evil planet not found in 2nd house first
            var evilPlanetFoundInHouse2 = false;

            //get planets in 12th house
            List<PlanetName> planetsInHouse12 = AstronomicalCalculator.GetPlanetsInHouse(12, time);

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
                List<PlanetName> planetsInHouse2 = AstronomicalCalculator.GetPlanetsInHouse(2, time);

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
            var houseMoonIsIn = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon);

            //if house moon is in is 6, 8 or 12
            if (houseMoonIsIn == 6 || houseMoonIsIn == 8 || houseMoonIsIn == 12)
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
            var planetsInConjunct = AstronomicalCalculator.GetPlanetsInConjuction(time, PlanetName.Moon);

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
                bool lagnaLordInLagna = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House1, time);

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
                bool _7thLordIn7th = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House7, time);

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
                var navamsaLagnaSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House1, time);

                var navamsaLagnaLord = AstronomicalCalculator.GetLordOfZodiacSign(navamsaLagnaSign);

                //2.0 get signs navamsa lagna lord is aspecting
                var signsNavamsaLagnaLordIsAspecting =
                    AstronomicalCalculator.GetSignsPlanetIsAspecting(navamsaLagnaLord, time);

                //3.0 get sign of lagna
                var lagnaSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
                var navamsa7thSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House7, time);

                var navamsa7thLord = AstronomicalCalculator.GetLordOfZodiacSign(navamsa7thSign);

                //2.0 get signs navamsa 7th lord is aspecting
                var signsNavamsa7thLordIsAspecting =
                    AstronomicalCalculator.GetSignsPlanetIsAspecting(navamsa7thLord, time);

                //3.0 get sign of 7th
                var _7thSign = AstronomicalCalculator.GetHouseSignName(7, time);

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
                var lagnaLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);

                //get signs lagna lord is aspecting
                var signsLagnaLordIsAspecting = AstronomicalCalculator.GetSignsPlanetIsAspecting(lagnaLord, time);

                //2.0 get navamsa lagna sign
                //get navamsa lagna at house 1 longitude
                var navamsaLagnaSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House1, time);

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
                var _7thLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);

                //get signs 7th lord is aspecting
                var signs7thLordIsAspecting = AstronomicalCalculator.GetSignsPlanetIsAspecting(_7thLord, time);

                //2.0 get navamsa 7th sign
                var navamsa7thSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House7, time);

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
                var navamsaLagnaSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House1, time);

                //2.0 Get navamsa lagna lord's current sign
                //get navamsa lagna lord (planet)
                var navamsaLagnaLord = AstronomicalCalculator.GetLordOfZodiacSign(navamsaLagnaSign);

                //get ordinary sign of navamsa lagna lord
                var ordinarySignOfNavamsaLagnaLord = AstronomicalCalculator.GetPlanetRasiSign(navamsaLagnaLord, person.BirthTime).GetSignName();

                //3.0 Get sign of house 1
                var house1Sign = AstronomicalCalculator.GetHouseSignName(1, time);

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
                var navamsa7thSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House7, time);

                //2.0 Get navamsa 7th lord's current sign
                //get navamsa 7th lord (planet)
                var navamsa7thLord = AstronomicalCalculator.GetLordOfZodiacSign(navamsa7thSign);

                //get ordinary sign of navamsa 7th lord
                var ordinarySignOfNavamsa7thLord = AstronomicalCalculator.GetPlanetRasiSign(navamsa7thLord, person.BirthTime).GetSignName();

                //3.0 Get sign of house 7
                var house7Sign = AstronomicalCalculator.GetHouseSignName(7, time);

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
                var lagnaLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);

                //get navamsa sign of lagna lord
                var navamsaSignOfLagnaLord = AstronomicalCalculator.GetPlanetNavamsaSign(lagnaLord, time);

                //2.0 get navamsa lagna sign
                //get navamsa lagna at house 1 longitude
                var navamsaLagnaSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House1, time);

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
                var _7thLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);

                //get navamsa sign of 7th lord
                var navamsaSignOf7thLord = AstronomicalCalculator.GetPlanetNavamsaSign(_7thLord, time);

                //2.0 get navamsa 7th sign
                var navamsa7thSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House7, time);

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
                var navamsaLagnaSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House1, time);

                //2.0 Get navamsa lagna lord's current sign
                //get navamsa lagna lord (planet)
                var navamsaLagnaLord = AstronomicalCalculator.GetLordOfZodiacSign(navamsaLagnaSign);

                //get navamsa sign of navamsa lagna lord
                var navamsaSignOfNavamsaLagnaLord = AstronomicalCalculator.GetPlanetNavamsaSign(navamsaLagnaLord, time);

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
                var navamsa7thSign = AstronomicalCalculator.GetHouseNavamsaSign(HouseName.House7, time);

                //2.0 Get navamsa 7th lord's current sign
                //get navamsa 7th lord (planet)
                var navamsa7thLord = AstronomicalCalculator.GetLordOfZodiacSign(navamsa7thSign);

                //get navamsa sign of navamsa 7th lord
                var navamsaSignOfNavamsa7thLord = AstronomicalCalculator.GetPlanetNavamsaSign(navamsa7thLord, time);

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
            var house1MiddleLongitude = AstronomicalCalculator.GetHouse(HouseName.House1, time).GetMiddleLongitude();

            //get zodiac sign at lagna (middle longitude)
            var house1ZodiacSign = AstronomicalCalculator.GetZodiacSignAtLongitude(house1MiddleLongitude);

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
            if (house1SignName == ZodiacName.Pisces || house1SignName == ZodiacName.Capricornus ||
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
            var weekday = AstronomicalCalculator.GetDayOfWeek(time);

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
                var localApparentTime = AstronomicalCalculator.GetLocalApparentTime(time);
                //get apparent noon
                var apparentNoon = AstronomicalCalculator.GetNoonTime(time);

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
            var planetsInLagna = AstronomicalCalculator.GetPlanetsInHouse(1, time);

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
            var planetsIn11th = AstronomicalCalculator.GetPlanetsInHouse(11, time);

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
            var planetInKendra = AstronomicalCalculator.IsPlanetInKendra(PlanetName.Jupiter, time) ||
                                 AstronomicalCalculator.IsPlanetInKendra(PlanetName.Venus, time);

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
                var allMalefics = AstronomicalCalculator.GetMaleficPlanetList(time);

                //go through each malefic planet and
                //make sure each is in 3, 6 or 11th house
                foreach (var malefic in allMalefics)
                {
                    var planetHouse = AstronomicalCalculator.GetHousePlanetIsIn(time, malefic);

                    //if not in 3, 6 or 11, end here as not occuring
                    if (!(planetHouse == 3 || planetHouse == 6 || planetHouse == 11)) { return false; }
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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellation = AstronomicalCalculator.GetMoonConstellation(time);
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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day
                var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day
                var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day
                var lunarDayGroup = AstronomicalCalculator.GetLunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day
                var lunarDayGroup = AstronomicalCalculator.GetLunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day
                var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();
                //get lunar day
                var lunarDayGroup = AstronomicalCalculator.GetLunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day group
                var lunarDayGroup = AstronomicalCalculator.GetLunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
                //get lunar day
                var lunarDayGroup = AstronomicalCalculator.GetLunarDay(time).GetLunarDayGroup();
                //get ruling constellation
                var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);
            //get ruling constellation
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);

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
            var rulingConstellation = AstronomicalCalculator.GetMoonConstellation(time);
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

            var yoga = AstronomicalCalculator.GetNithyaYoga(time);

            if (yoga == NithyaYoga.Atiganda || yoga == NithyaYoga.Soola
                || yoga == NithyaYoga.Ganda || yoga == NithyaYoga.Vyatapata
                || yoga == NithyaYoga.Vaidhriti)
            {
                return CalculatorResult.NotOccuring();
            }

            //
            // - Karana chosen must be appropriate to the election in view.
            // Thus Bava is auspicious for starting works of permanent importance
            // while Thaithula is propitious for marriage.
            // Bhadra(vishti) is unfit for any good work but is eminently suitable for violent and cruel deeds.
            // For getting initiation into kshudra mantras Sakuni Havana is propitious.

            var karana = AstronomicalCalculator.GetKarana(time);
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

            var yoga = AstronomicalCalculator.GetNithyaYoga(time);

            if (yoga == NithyaYoga.Atiganda || yoga == NithyaYoga.Soola
                                            || yoga == NithyaYoga.Ganda || yoga == NithyaYoga.Vyatapata
                                            || yoga == NithyaYoga.Vaidhriti)
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
            var timeSunEnteredCurrentSign = AstronomicalCalculator.GetTimeSunEnteredCurrentSign(time);

            //get hours after entry into sign
            var hoursAfterEntryIntoSign = time.Subtract(timeSunEnteredCurrentSign).TotalHours;

            //if hours after entry is less than or equals hours to reject
            if (hoursAfterEntryIntoSign <= hoursToReject)
            {
                //return true
                return CalculatorResult.IsOccuring();
            }


            //get time sun will leave current sign
            var timeSunLeavesCurrentSign = AstronomicalCalculator.GetTimeSunLeavesCurrentSign(time);

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
            //TODO ALWAYS ON
            //DISABLE FOR NOW
            return CalculatorResult.NotOccuring();


            //Papashadvargs. - Malefics should not be strong in shadvargas in an election chart.
            //This event idicates that malefics are strong in shadvargas

            //TODO Note : It is possible that overall strenght of a malefic is considered,
            //            for now not 100% sure. Current method of using shadvarga bala calculation
            //            seems workable. Further verification is in order.
            //            Shadvarga bala uses malefic's relationship with sign to determine strenght

            //get all malefic planets
            var allMalefics = AstronomicalCalculator.GetMaleficPlanetList(time);

            //rahu & ketu are not included
            //TODO needs checking
            allMalefics.RemoveAll(name => name == PlanetName.Rahu || name == PlanetName.Ketu);

            //go through each malefic planet and
            //check if is strong in shadvarga
            foreach (var malefic in allMalefics)
            {
                //check if planet is strong
                var isStrong = AstronomicalCalculator.IsPlanetStrongInShadvarga(malefic, time);

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var dayOfWeek = AstronomicalCalculator.GetDayOfWeek(time);

            //check if days to avoid are occuring
            var avoidDays = dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday;

            //if wrong days then, end here as not occuring
            if (avoidDays == true) { return CalculatorResult.NotOccuring(); }



            //Avoid the 8th, 9th, 14th lunar
            //days as well as New and Full Moon days.

            //get lunar day
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

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
                var currentWeekday = AstronomicalCalculator.GetDayOfWeek(time);

                //check weekday
                var rightWeekday = currentWeekday == DayOfWeek.Thursday;

                //if not correct weekday, end here as not occuring
                if (rightWeekday == false) { return false; }


                //2. Jupiter is in Lagna
                //get planets in lagna 
                var currentPlanetsInLagna = AstronomicalCalculator.GetPlanetsInHouse(1, time);

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
                var currentWeekday = AstronomicalCalculator.GetDayOfWeek(time);

                //check weekday
                var rightWeekday = currentWeekday == DayOfWeek.Thursday;

                //if not correct weekday, end here as not occuring
                if (rightWeekday == false) { return false; }


                //2. Correct rising sign
                //get rising sign
                var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var currentWeekday = AstronomicalCalculator.GetDayOfWeek(time);

            //check weekday
            var rightWeekday = currentWeekday == DayOfWeek.Friday;

            //if not correct weekday, end here as not occuring
            if (rightWeekday == false) { return CalculatorResult.NotOccuring(); }


            //2. Planet is in Lagna
            //get planets in lagna 
            var currentPlanetsInLagna = AstronomicalCalculator.GetPlanetsInHouse(1, time);

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
            var currentWeekday = AstronomicalCalculator.GetDayOfWeek(time);

            //check weekday
            var rightWeekday = currentWeekday == DayOfWeek.Tuesday;

            //if not correct weekday, end here as not occuring
            if (rightWeekday == false) { return CalculatorResult.NotOccuring(); }


            //2. Planet is in Lagna
            //get planets in lagna 
            var currentPlanetsInLagna = AstronomicalCalculator.GetPlanetsInHouse(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

            //check rising sign
            var rightSign = risingSign == ZodiacName.Capricornus;

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var planets = AstronomicalCalculator.GetPlanetsInHouse(8, time);

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
            var lagnaLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);

            //get lord of weekday
            var weekdayLord = AstronomicalCalculator.GetLordOfWeekday(time);


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
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var risingSign = AstronomicalCalculator.GetHouseSignName(1, time);

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
            var moonAspectedByMars = AstronomicalCalculator.IsPlanetAspectedByPlanet(PlanetName.Moon, PlanetName.Mars, time);
            if (moonAspectedByMars) { return CalculatorResult.NotOccuring(); }

            //if mercury aspected by mars, end here as not occuring
            var mercuryAspectedByMars = AstronomicalCalculator.IsPlanetAspectedByPlanet(PlanetName.Mercury, PlanetName.Mars, time);
            if (mercuryAspectedByMars) { return CalculatorResult.NotOccuring(); }

            //if moon conjunct with mars, end here as not occuring
            var moonConjunctWithMars = AstronomicalCalculator.IsPlanetConjunctWithPlanet(PlanetName.Moon, PlanetName.Mars, time);
            if (moonConjunctWithMars) { return CalculatorResult.NotOccuring(); }

            //if mercury conjunct with mars, end here as not occuring
            var mercuryConjunctWithMars = AstronomicalCalculator.IsPlanetConjunctWithPlanet(PlanetName.Mercury, PlanetName.Mars, time);
            if (mercuryConjunctWithMars) { return CalculatorResult.NotOccuring(); }


            //2. The Moon's situation in Taurus. Cancer or Pisces would greatly help the seller.

            //get sign moon is in 
            var moonSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, time);

            //check if moon is in the correct sign
            var inCorrectSign = moonSign.GetSignName() == ZodiacName.Taurus ||
                                moonSign.GetSignName() == ZodiacName.Cancer ||
                                moonSign.GetSignName() == ZodiacName.Pisces;

            //if not correct sign, end here as not occuring
            if (inCorrectSign == false) { return CalculatorResult.NotOccuring(); }


            //3. Try to keep Mercury in a kendra from Lagna or at least in good aspect to Jupiter
            var mercuryInKendra = AstronomicalCalculator.IsPlanetInKendra(PlanetName.Mercury, time);
            var mercuryInGoodAspectToJupiter = AstronomicalCalculator.IsPlanetInGoodAspectToPlanet(PlanetName.Jupiter, PlanetName.Mercury, time);

            //if NOT in good aspect or in kendra, event not occuring
            if (!(mercuryInKendra || mercuryInGoodAspectToJupiter)) { return CalculatorResult.NotOccuring(); }


            //4. Tuesday should be avoided. Monday, Wednesday and Thursday are the best. While Friday
            // is unpropitious, Saturday is middling.

            //get weekday
            var weekDay = AstronomicalCalculator.GetDayOfWeek(time);

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
            var weekDay = AstronomicalCalculator.GetDayOfWeek(time);

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
            var moonSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, time);

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
            var rulingConstellationName = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationName();

            //check ruling constellation name
            var rightConstellation = rulingConstellationName == ConstellationName.Aslesha ||
                                     rulingConstellationName == ConstellationName.Moola ||
                                     rulingConstellationName == ConstellationName.Jyesta;


            //if not correct constellation, end here as not occuring
            if (rightConstellation == false) { return CalculatorResult.NotOccuring(); }



            //2. avoid the 8th and 9th lunar days and New Moon.
            //get lunar day
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var goodAspect = AstronomicalCalculator.IsPlanetInGoodAspectToPlanet(PlanetName.Moon, PlanetName.Jupiter, time);

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
            var goodAspect = AstronomicalCalculator.IsPlanetInGoodAspectToPlanet(PlanetName.Mars, PlanetName.Jupiter, time);

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
            var goodAspect = AstronomicalCalculator.IsPlanetInGoodAspectToPlanet(PlanetName.Saturn, PlanetName.Jupiter, time);

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
            var goodAspect = AstronomicalCalculator.IsPlanetInGoodAspectToHouse(HouseName.House1, PlanetName.Jupiter, time);

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
                var sunAspectStrength = AstronomicalCalculator.GetPlanetDrikBala(PlanetName.Sun, time);
                var moonAspectStrength = AstronomicalCalculator.GetPlanetDrikBala(PlanetName.Sun, time);

                //Note: positive bala = positive aspect, negative bala = negative aspect
                //if NOT postive number, end here as not good
                if (!(sunAspectStrength.ToDouble() > 0 && moonAspectStrength.ToDouble() > 0)) { return false; }


                //2. well situated
                //based on Planet Sthana Bala (Positonal strength)
                var sunPositionStrenght = AstronomicalCalculator.GetPlanetSthanaBala(PlanetName.Sun, time);
                var moonPositionStrenght = AstronomicalCalculator.GetPlanetSthanaBala(PlanetName.Moon, time);

                //Note: To determine if sthana bala is indicating good position or bad position
                //a neutral point is set, anything above is good & below is bad
                var sunNeutralPoint = AstronomicalCalculator.GetPlanetSthanaBalaNeutralPoint(PlanetName.Sun);
                var moonNeutralPoint = AstronomicalCalculator.GetPlanetSthanaBalaNeutralPoint(PlanetName.Moon);

                //if NOT above neutral number, end here as not good
                if (!(sunPositionStrenght.ToDouble() > sunNeutralPoint && moonPositionStrenght.ToDouble() > moonNeutralPoint)) { return false; }



                //if control reaches here then good aspectect & well situated
                return true;
            }

        }


        #endregion

        #region ASTRONOMICAL

        [EventCalculator(EventName.SunIsStrong)]
        public static CalculatorResult IsSunIsStrongOccuring(Time time, Person person)
        {
            var strongestPlanet = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0];
            var occuring = strongestPlanet == PlanetName.Sun;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Sun, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonIsStrong)]
        public static CalculatorResult IsMoonIsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0] == PlanetName.Moon;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsIsStrong)]
        public static CalculatorResult IsMarsIsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0] == PlanetName.Mars;

            //
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mars, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryIsStrong)]
        public static CalculatorResult IsMercuryIsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0] == PlanetName.Mercury;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mercury, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterIsStrong)]
        public static CalculatorResult IsJupiterIsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0] == PlanetName.Jupiter;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Jupiter, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusIsStrong)]
        public static CalculatorResult IsVenusIsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0] == PlanetName.Venus;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Venus, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnIsStrong)]
        public static CalculatorResult IsSaturnIsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllPlanetOrderedByStrength(time)[0] == PlanetName.Saturn;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Saturn, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House1IsStrong)]
        public static CalculatorResult IsHouse1IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House2IsStrong)]
        public static CalculatorResult IsHouse2IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House2;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House3IsStrong)]
        public static CalculatorResult IsHouse3IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House4IsStrong)]
        public static CalculatorResult IsHouse4IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House4;

            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House5IsStrong)]
        public static CalculatorResult IsHouse5IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House6IsStrong)]
        public static CalculatorResult IsHouse6IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House7IsStrong)]
        public static CalculatorResult IsHouse7IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House8IsStrong)]
        public static CalculatorResult IsHouse8IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House9IsStrong)]
        public static CalculatorResult IsHouse9IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House10IsStrong)]
        public static CalculatorResult IsHouse10IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House11IsStrong)]
        public static CalculatorResult IsHouse11IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.House12IsStrong)]
        public static CalculatorResult IsHouse12IsStrongOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetAllHousesOrderedByStrength(time)[0] == HouseName.House12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.Sunrise)]
        public static CalculatorResult IsSunriseOccuring(Time time, Person person)
        {
            //actual sunrise at location, when center of sun disk is at horizon

            //get sunrise time for that day
            var sunriseTime = AstronomicalCalculator.GetSunriseTime(time);

            //+-5 min added to get the event, otherwise match with exact time might miss
            var MIN_5 = 0.08333333;// in hours
            var _5minAfter = sunriseTime.AddHours(MIN_5);
            var _5minBefore = sunriseTime.SubtractHours(MIN_5);

            var isAfter = time.GetLmtDateTimeOffset() >= _5minBefore.GetLmtDateTimeOffset();//after -5min
            var isBefore = time.GetLmtDateTimeOffset() <= _5minAfter.GetLmtDateTimeOffset();//before +5min

            //time is within +-5min
            var occuring = isAfter && isBefore;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.Sunset)]
        public static CalculatorResult IsSunsetOccuring(Time time, Person person)
        {
            //actual sunset at location, when center of sun disk is at horizon

            //get sunset time for that day
            var sunsetTime = AstronomicalCalculator.GetSunsetTime(time);

            //+-5 min added to get the event, otherwise match with exact time might miss
            var MIN_5 = 0.08333333;// in hours
            var _5minAfter = sunsetTime.AddHours(MIN_5);
            var _5minBefore = sunsetTime.SubtractHours(MIN_5);

            var isAfter = time.GetLmtDateTimeOffset() >= _5minBefore.GetLmtDateTimeOffset();//after -5min
            var isBefore = time.GetLmtDateTimeOffset() <= _5minAfter.GetLmtDateTimeOffset();//before +5min

            //time is within +-5min
            var occuring = isAfter && isBefore;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.Midday)]
        public static CalculatorResult IsMiddayOccuring(Time time, Person person)
        {
            //This is marked when the centre of the Sun is exactly on the
            // meridian of the place. The apparent noon is
            // almost the same for all places.


            //get apparent time
            var localApparentTime = AstronomicalCalculator.GetLocalApparentTime(time);
            var apparentNoon = AstronomicalCalculator.GetNoonTime(time);

            //+-5 min added to get the event, otherwise match with exact time might miss
            var MIN_5 = 0.08333333;// in hours
            var _5minAfter = apparentNoon.AddHours(MIN_5);
            var _5minBefore = apparentNoon.AddHours(-MIN_5);

            var isAfter = localApparentTime >= _5minBefore;//after -5min
            var isBefore = localApparentTime <= _5minAfter;//before +5min

            //time is within 11:55AM to 12:05PM
            var occuring = isAfter && isBefore;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }


        #endregion

        #region HOROSCOPE

        #region Lord of 1st being Situated in Different Houses

        [EventCalculator(EventName.House1LordInHouse1)]
        public static CalculatorResult House1LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House1, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse2)]
        public static CalculatorResult House1LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House2, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House1LordInHouse3)]
        public static CalculatorResult House1LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House3, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse4)]
        public static CalculatorResult House1LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House4, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse5)]
        public static CalculatorResult House1LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House5, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse6)]
        public static CalculatorResult House1LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House6, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse7)]
        public static CalculatorResult House1LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House7, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse8)]
        public static CalculatorResult House1LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House8, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse9)]
        public static CalculatorResult House1LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House9, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House1LordInHouse10)]
        public static CalculatorResult House1LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House10, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House1LordInHouse11)]
        public static CalculatorResult House1LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House11, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House1LordInHouse12)]
        public static CalculatorResult House1LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House12, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 1 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        #endregion

        #region Lord of 2nd being Situated in Different Houses

        [EventCalculator(EventName.House2LordInHouse1)]
        public static CalculatorResult House2LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House1, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse2)]
        public static CalculatorResult House2LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House2, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse3)]
        public static CalculatorResult House2LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House3, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse4)]
        public static CalculatorResult House2LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House4, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse5)]
        public static CalculatorResult House2LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House5, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse6)]
        public static CalculatorResult House2LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House6, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse7)]
        public static CalculatorResult House2LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House7, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse8)]
        public static CalculatorResult House2LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House8, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse9)]
        public static CalculatorResult House2LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House9, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse10)]
        public static CalculatorResult House2LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House10, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse11)]
        public static CalculatorResult House2LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House11, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House2LordInHouse12)]
        public static CalculatorResult House2LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House12, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 2 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of 3rd being Situated in Different Houses

        [EventCalculator(EventName.House3LordInHouse1)]
        public static CalculatorResult House3LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House1, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse2)]
        public static CalculatorResult House3LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House2, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse3)]
        public static CalculatorResult House3LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House3, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse4)]
        public static CalculatorResult House3LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House4, time);
            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse5)]
        public static CalculatorResult House3LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House5, time);
            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse6)]
        public static CalculatorResult House3LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House6, time);
            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse7)]
        public static CalculatorResult House3LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House7, time);
            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse8)]
        public static CalculatorResult House3LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House8, time);
            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse9)]
        public static CalculatorResult House3LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House9, time);
            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House3LordInHouse10)]
        public static CalculatorResult House3LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House10, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House3LordInHouse11)]
        public static CalculatorResult House3LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House11, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House3LordInHouse12)]
        public static CalculatorResult House3LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House12, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 3 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        #endregion

        #region Lord of the 4th House Occupying Different Houses

        [EventCalculator(EventName.House4LordInHouse1)]
        public static CalculatorResult House4LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House1, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse2)]
        public static CalculatorResult House4LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House2, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse3)]
        public static CalculatorResult House4LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House3, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse4)]
        public static CalculatorResult House4LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House4, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse5)]
        public static CalculatorResult House4LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House5, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse6)]
        public static CalculatorResult House4LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House6, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House4LordInHouse7)]
        public static CalculatorResult House4LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House7, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House4LordInHouse8)]
        public static CalculatorResult House4LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House8, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House4LordInHouse9)]
        public static CalculatorResult House4LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House9, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse10)]
        public static CalculatorResult House4LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House10, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House4LordInHouse11)]
        public static CalculatorResult House4LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House11, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House4LordInHouse12)]
        public static CalculatorResult House4LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House12, time);

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House4, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 4 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 5th House Occupying Different Houses

        [EventCalculator(EventName.House5LordInHouse1)]
        public static CalculatorResult House5LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House1, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse2)]
        public static CalculatorResult House5LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House2, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse3)]
        public static CalculatorResult House5LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse4)]
        public static CalculatorResult House5LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse5)]
        public static CalculatorResult House5LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House5, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse6)]
        public static CalculatorResult House5LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse7)]
        public static CalculatorResult House5LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse8)]
        public static CalculatorResult House5LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse9)]
        public static CalculatorResult House5LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House9, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House5LordInHouse10)]
        public static CalculatorResult House5LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse11)]
        public static CalculatorResult House5LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House11, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House5LordInHouse12)]
        public static CalculatorResult House5LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House12, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 5 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 6th House Occupying Different Houses

        [EventCalculator(EventName.House6LordInHouse1)]
        public static CalculatorResult House6LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House1, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse2)]
        public static CalculatorResult House6LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House2, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse3)]
        public static CalculatorResult House6LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse4)]
        public static CalculatorResult House6LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse5)]
        public static CalculatorResult House6LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House5, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };

        }

        [EventCalculator(EventName.House6LordInHouse6)]
        public static CalculatorResult House6LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse7)]
        public static CalculatorResult House6LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse8)]
        public static CalculatorResult House6LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse9)]
        public static CalculatorResult House6LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House9, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse10)]
        public static CalculatorResult House6LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse11)]
        public static CalculatorResult House6LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House11, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House6LordInHouse12)]
        public static CalculatorResult House6LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House12, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 6 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 7th House Occupying Different Houses

        [EventCalculator(EventName.House7LordInHouse1)]
        public static CalculatorResult House7LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House1, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse2)]
        public static CalculatorResult House7LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House2, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse3)]
        public static CalculatorResult House7LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse4)]
        public static CalculatorResult House7LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse5)]
        public static CalculatorResult House7LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House5, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse6)]
        public static CalculatorResult House7LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse7)]
        public static CalculatorResult House7LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse8)]
        public static CalculatorResult House7LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse9)]
        public static CalculatorResult House7LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House9, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse10)]
        public static CalculatorResult House7LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse11)]
        public static CalculatorResult House7LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House11, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House7LordInHouse12)]
        public static CalculatorResult House7LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House12, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 7 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 8th House Occupying Different Houses

        [EventCalculator(EventName.House8LordInHouse1)]
        public static CalculatorResult House8LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House1, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse2)]
        public static CalculatorResult House8LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House2, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse3)]
        public static CalculatorResult House8LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse4)]
        public static CalculatorResult House8LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse5)]
        public static CalculatorResult House8LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House5, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse6)]
        public static CalculatorResult House8LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse7)]
        public static CalculatorResult House8LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse8)]
        public static CalculatorResult House8LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse9)]
        public static CalculatorResult House8LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House9, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse10)]
        public static CalculatorResult House8LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse11)]
        public static CalculatorResult House8LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House11, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House8LordInHouse12)]
        public static CalculatorResult House8LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House12, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 8 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 9th House Occupying Different Houses

        [EventCalculator(EventName.House9LordInHouse1)]
        public static CalculatorResult House9LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House1, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse2)]
        public static CalculatorResult House9LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House2, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse3)]
        public static CalculatorResult House9LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse4)]
        public static CalculatorResult House9LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse5)]
        public static CalculatorResult House9LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House5, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse6)]
        public static CalculatorResult House9LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse7)]
        public static CalculatorResult House9LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse8)]
        public static CalculatorResult House9LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse9)]
        public static CalculatorResult House9LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House9, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse10)]
        public static CalculatorResult House9LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse11)]
        public static CalculatorResult House9LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House11, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House9LordInHouse12)]
        public static CalculatorResult House9LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House12, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 9 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 10th House Occupying Different Houses

        [EventCalculator(EventName.House10LordInHouse1)]
        public static CalculatorResult House10LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House1, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse2)]
        public static CalculatorResult House10LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House2, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse3)]
        public static CalculatorResult House10LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse4)]
        public static CalculatorResult House10LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse5)]
        public static CalculatorResult House10LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House5, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse6)]
        public static CalculatorResult House10LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info }; ;
        }

        [EventCalculator(EventName.House10LordInHouse7)]
        public static CalculatorResult House10LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse8)]
        public static CalculatorResult House10LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse9)]
        public static CalculatorResult House10LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House9, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse10)]
        public static CalculatorResult House10LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse11)]
        public static CalculatorResult House10LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House11, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House10LordInHouse12)]
        public static CalculatorResult House10LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House12, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House10, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 10 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }


        #endregion

        #region Lord of the 11th House Occupying Different Houses

        [EventCalculator(EventName.House11LordInHouse1)]
        public static CalculatorResult House11LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House1, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse2)]
        public static CalculatorResult House11LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House2, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse3)]
        public static CalculatorResult House11LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House3, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse4)]
        public static CalculatorResult House11LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House4, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse5)]
        public static CalculatorResult House11LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House5, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse6)]
        public static CalculatorResult House11LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House6, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse7)]
        public static CalculatorResult House11LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House7, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse8)]
        public static CalculatorResult House11LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House8, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse9)]
        public static CalculatorResult House11LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House9, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse10)]
        public static CalculatorResult House11LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse11)]
        public static CalculatorResult House11LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House11, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House11LordInHouse12)]
        public static CalculatorResult House11LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House12, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House11, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 11 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion

        #region Lord of the 12th House Occupying Different Houses

        [EventCalculator(EventName.House12LordInHouse1)]
        public static CalculatorResult House12LordInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House1, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse2)]
        public static CalculatorResult House12LordInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House2, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse3)]
        public static CalculatorResult House12LordInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House3, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse4)]
        public static CalculatorResult House12LordInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House4, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse5)]
        public static CalculatorResult House12LordInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House5, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse6)]
        public static CalculatorResult House12LordInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House6, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse7)]
        public static CalculatorResult House12LordInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House7, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse8)]
        public static CalculatorResult House12LordInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House8, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse9)]
        public static CalculatorResult House12LordInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House9, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse10)]
        public static CalculatorResult House12LordInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House10, time);
            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse11)]
        public static CalculatorResult House12LordInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House11, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.House12LordInHouse12)]
        public static CalculatorResult House12LordInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House12, time);

            //INFO CALCULATION
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House12, time);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);
            var info = $"House 12 Lord : {lord} : {raw}";
            return new() { Occuring = occuring, Info = info };
        }

        #endregion


        #region Different Signs Ascending

        [EventCalculator(EventName.AriesRising)]
        public static CalculatorResult AriesRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Aries;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Aries);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.TaurusRising)]
        public static CalculatorResult TaurusRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Taurus;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Taurus);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.GeminiRising)]
        public static CalculatorResult GeminiRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Gemini;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Gemini);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.CancerRising)]
        public static CalculatorResult CancerRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Cancer;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Cancer);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.LeoRising)]
        public static CalculatorResult LeoRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Leo;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Leo);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VirgoRising)]
        public static CalculatorResult VirgoRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Virgo;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Virgo);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.LibraRising)]
        public static CalculatorResult LibraRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Libra;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Libra);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.ScorpioRising)]
        public static CalculatorResult ScorpioRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Scorpio;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Scorpio);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SagittariusRising)]
        public static CalculatorResult SagittariusRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Sagittarius;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Sagittarius);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.CapricornusRising)]
        public static CalculatorResult CapricornusRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Capricornus;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Capricornus);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.AquariusRising)]
        public static CalculatorResult AquariusRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Aquarius;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Aquarius);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.PiscesRising)]
        public static CalculatorResult PiscesRisingOccuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Pisces;

            //STRENGTH CALCULATION
            var lord = AstronomicalCalculator.GetLordOfZodiacSign(ZodiacName.Pisces);
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(lord, time);

            var strength = raw.ToString();
            return new() { Occuring = occuring, Info = strength };
        }

        #endregion


        //Planets in the 1-12th House

        #region Planets in the 1st House

        [EventCalculator(EventName.SunInHouse1)]
        public static CalculatorResult SunInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Sun, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse1)]
        public static CalculatorResult MoonInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse1)]
        public static CalculatorResult MarsInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mars, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse1)]
        public static CalculatorResult MercuryInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mercury, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse1)]
        public static CalculatorResult JupiterInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Jupiter, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse1)]
        public static CalculatorResult VenusInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Venus, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse1)]
        public static CalculatorResult SaturnInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 1;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Saturn, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse1)]
        public static CalculatorResult RahuInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 1;

            //STRENGTH CALCULATION
            var raw = 0; //TODO stregth for RAHU
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse1)]
        public static CalculatorResult KetuInHouse1Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 1;

            //STRENGTH CALCULATION
            var raw = 0; //TODO stregth for RAHU
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 2nd House

        [EventCalculator(EventName.SunInHouse2)]
        public static CalculatorResult SunInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Sun, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse2)]
        public static CalculatorResult MoonInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse2)]
        public static CalculatorResult MarsInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mars, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse2)]
        public static CalculatorResult MercuryInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mercury, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse2)]
        public static CalculatorResult JupiterInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Jupiter, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse2)]
        public static CalculatorResult VenusInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Venus, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse2)]
        public static CalculatorResult SaturnInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 2;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Saturn, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse2)]
        public static CalculatorResult RahuInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 2;

            //STRENGTH CALCULATION
            var raw = "0";//TODO impliment
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse2)]
        public static CalculatorResult KetuInHouse2Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 2;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 3rd House

        [EventCalculator(EventName.SunInHouse3)]
        public static CalculatorResult SunInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse3)]
        public static CalculatorResult MoonInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 3;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse3)]
        public static CalculatorResult MarsInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse3)]
        public static CalculatorResult MercuryInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse3)]
        public static CalculatorResult JupiterInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse3)]
        public static CalculatorResult VenusInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse3)]
        public static CalculatorResult SaturnInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse3)]
        public static CalculatorResult RahuInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse3)]
        public static CalculatorResult KetuInHouse3Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 3;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 4th House

        [EventCalculator(EventName.SunInHouse4)]
        public static CalculatorResult SunInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse4)]
        public static CalculatorResult MoonInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse4)]
        public static CalculatorResult MarsInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse4)]
        public static CalculatorResult MercuryInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse4)]
        public static CalculatorResult JupiterInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse4)]
        public static CalculatorResult VenusInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse4)]
        public static CalculatorResult SaturnInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse4)]
        public static CalculatorResult RahuInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse4)]
        public static CalculatorResult KetuInHouse4Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 4;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 5th House

        [EventCalculator(EventName.SunInHouse5)]
        public static CalculatorResult SunInHouse5Occuring(Time time, Person person)
        {

            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 5;
            var strength = "440"; //TODO Calulate proper

            return new CalculatorResult() { Occuring = occuring, Info = strength };

        }

        [EventCalculator(EventName.MoonInHouse5)]
        public static CalculatorResult MoonInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse5)]
        public static CalculatorResult MarsInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse5)]
        public static CalculatorResult MercuryInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse5)]
        public static CalculatorResult JupiterInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 5;

            //STRENGTH CALCULATION
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Jupiter, time);
            var strength = raw.ToString();

            return new CalculatorResult() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse5)]
        public static CalculatorResult VenusInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse5)]
        public static CalculatorResult SaturnInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse5)]
        public static CalculatorResult RahuInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 5;


            //STRENGTH CALCULATION
            //var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Rahu, time);
            //var strength = raw.ToString();

            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse5)]
        public static CalculatorResult KetuInHouse5Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 5;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 6th House

        [EventCalculator(EventName.SunInHouse6)]
        public static CalculatorResult SunInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse6)]
        public static CalculatorResult MoonInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse6)]
        public static CalculatorResult MarsInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse6)]
        public static CalculatorResult MercuryInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse6)]
        public static CalculatorResult JupiterInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse6)]
        public static CalculatorResult VenusInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse6)]
        public static CalculatorResult SaturnInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse6)]
        public static CalculatorResult RahuInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse6)]
        public static CalculatorResult KetuInHouse6Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 6;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }


        //Planets in the 7th House

        [EventCalculator(EventName.SunInHouse7)]
        public static CalculatorResult SunInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse7)]
        public static CalculatorResult MoonInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse7)]
        public static CalculatorResult MarsInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse7)]
        public static CalculatorResult MercuryInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse7)]
        public static CalculatorResult JupiterInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse7)]
        public static CalculatorResult VenusInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse7)]
        public static CalculatorResult SaturnInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse7)]
        public static CalculatorResult RahuInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse7)]
        public static CalculatorResult KetuInHouse7Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 7;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 8th House

        [EventCalculator(EventName.SunInHouse8)]
        public static CalculatorResult SunInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse8)]
        public static CalculatorResult MoonInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse8)]
        public static CalculatorResult MarsInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse8)]
        public static CalculatorResult MercuryInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse8)]
        public static CalculatorResult JupiterInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse8)]
        public static CalculatorResult VenusInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse8)]
        public static CalculatorResult SaturnInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse8)]
        public static CalculatorResult RahuInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse8)]
        public static CalculatorResult KetuInHouse8Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 8;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }


        #endregion

        #region Planets in the 9th House

        [EventCalculator(EventName.SunInHouse9)]
        public static CalculatorResult SunInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse9)]
        public static CalculatorResult MoonInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse9)]
        public static CalculatorResult MarsInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse9)]
        public static CalculatorResult MercuryInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse9)]
        public static CalculatorResult JupiterInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse9)]
        public static CalculatorResult VenusInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse9)]
        public static CalculatorResult SaturnInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse9)]
        public static CalculatorResult RahuInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse9)]
        public static CalculatorResult KetuInHouse9Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 9;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 10th House

        [EventCalculator(EventName.SunInHouse10)]
        public static CalculatorResult SunInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse10)]
        public static CalculatorResult MoonInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse10)]
        public static CalculatorResult MarsInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse10)]
        public static CalculatorResult MercuryInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse10)]
        public static CalculatorResult JupiterInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse10)]
        public static CalculatorResult VenusInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse10)]
        public static CalculatorResult SaturnInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse10)]
        public static CalculatorResult RahuInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse10)]
        public static CalculatorResult KetuInHouse10Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 10;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 11th House

        [EventCalculator(EventName.SunInHouse11)]
        public static CalculatorResult SunInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse11)]
        public static CalculatorResult MoonInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse11)]
        public static CalculatorResult MarsInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse11)]
        public static CalculatorResult MercuryInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse11)]
        public static CalculatorResult JupiterInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse11)]
        public static CalculatorResult VenusInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse11)]
        public static CalculatorResult SaturnInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse11)]
        public static CalculatorResult RahuInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse11)]
        public static CalculatorResult KetuInHouse11Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 11;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion

        #region Planets in the 12th House

        [EventCalculator(EventName.SunInHouse12)]
        public static CalculatorResult SunInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Sun, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MoonInHouse12)]
        public static CalculatorResult MoonInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MarsInHouse12)]
        public static CalculatorResult MarsInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mars, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.MercuryInHouse12)]
        public static CalculatorResult MercuryInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Mercury, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.JupiterInHouse12)]
        public static CalculatorResult JupiterInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Jupiter) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Jupiter, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.VenusInHouse12)]
        public static CalculatorResult VenusInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Venus) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Venus, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.SaturnInHouse12)]
        public static CalculatorResult SaturnInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Saturn, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.RahuInHouse12)]
        public static CalculatorResult RahuInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Rahu) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Rahu, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        [EventCalculator(EventName.KetuInHouse12)]
        public static CalculatorResult KetuInHouse12Occuring(Time time, Person person)
        {
            var occuring = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Ketu) == 12;
            var raw = AstronomicalCalculator.GetPlanetShadbalaPinda(PlanetName.Moon, time);
            var strength = raw.ToString();

            return new() { Occuring = occuring, Info = strength };
        }

        #endregion


        #region 2ND HOUSE SPECIAL COMBINATIONS

        [EventCalculator(EventName.Lord2WithEvilInHouse)]
        public static CalculatorResult Lord2WithEvilInHouse(Time time, Person person)
        {
            //If the 2nd lord is in the 2nd with(1) evil planets or aspected by him(2), he will be poor.
            //NOTE: 1."with" here is interpreted as same house
            //      2. interpreted as evil planets transmitting aspect to 2nd lord (receiving aspect)
            //TODO check validity


            //if 2nd lord not in second, end here
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lordPlace = AstronomicalCalculator.GetHousePlanetIsIn(time, lord);
            if (lordPlace != 2) { return CalculatorResult.NotOccuring(); }

            //evil planet in house 2, prediction occuring
            var evilInHouse2 = AstronomicalCalculator.IsMaleficPlanetInHouse(2, time);

            //if evil planets aspect the lord, prediction occuring
            var aspectedByEvil = AstronomicalCalculator.IsPlanetAspectedByMaleficPlanets(lord, time);

            //either one true for prediction to occur
            var occurring = evilInHouse2 || aspectedByEvil;

            var info = $"Lord:{lord}";
            return new CalculatorResult() { Occuring = occurring, Info = info };

        }

        [EventCalculator(EventName.SaturnIn2WithVenus)]
        public static CalculatorResult SaturnIn2WithVenus(Time time, Person person)
        {
            //Ordinary wealth is indicated if Saturn is in the 2nd aspected by Venus.

            //if saturn not in 2nd end here
            var saturnHouse = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn);
            var saturnIn2 = saturnHouse == 2;
            if (!saturnIn2) { return CalculatorResult.NotOccuring(); }

            //if venus is aspecting saturn, event occuring
            var venusAspecting =
                AstronomicalCalculator.IsPlanetAspectedByPlanet(PlanetName.Saturn, PlanetName.Venus, time);

            return new CalculatorResult() { Occuring = venusAspecting };
        }

        [EventCalculator(EventName.MoonMarsIn2WithSaturnAspect)]
        public static CalculatorResult MoonMarsIn2WithSaturnAspect(Time time, Person person)
        {
            //If the Moon and Mars reside in the 2nd bhava and Saturn aspects it,
            //he suffers from a peculiar skin disease.

            //moon and mars in 2nd
            var moonIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 2;
            var marsIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 2;

            //saturn aspects 2nd House
            var saturnAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Saturn, time);

            //check if all conditions met
            var occuring = moonIn2 && marsIn2 && saturnAspects2nd;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryAndEvilIn2WithMoonAspect)]
        public static CalculatorResult MercuryAndEvilIn2WithMoonAspect(Time time, Person person)
        {
            //The situation of Mercury in the 2nd with another evil planet aspected by the Moon is bad for saving money.
            //Even if there is any ancestral wealth, it will be spent—rather wasted on extravagant purposes.

            //is mercury in 2nd house
            var mercuryIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 2;

            //evil planet in 2nd house
            var evilPlanetIn2 = AstronomicalCalculator.IsMaleficPlanetInHouse(2, time);

            //moon aspects 2nd House
            var moonAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Moon, time);

            //check if all conditions met
            var occuring = mercuryIn2 && evilPlanetIn2 && moonAspects2nd;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunIn2WithNoSaturnAspect)]
        public static CalculatorResult SunIn2WithNoSaturnAspect(Time time, Person person)
        {
            //The Sun in the 2nd without being aspected by Saturn is favourable for a steady fortune.

            //sun in 2nd
            var sunIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 2;

            //saturn aspects 2nd House
            var saturnNotAspects2nd =
                !AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Saturn, time);

            //check if all conditions met
            var occuring = sunIn2 && saturnNotAspects2nd;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonIn2WithMercuryAspect)]
        public static CalculatorResult MoonIn2WithMercuryAspect(Time time, Person person)
        {
            //The Moon being placed in the 2nd and aspected by Mercury is favourable for earning money by self-exertion.

            //moon in 2nd
            var moonIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 2;

            //mercury aspects 2nd House
            var mercuryAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Mercury, time);

            //check if all conditions met
            var occuring = moonIn2 && mercuryAspects2nd;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord2And3In6WithEvilPlanet)]
        public static CalculatorResult Lord2And3In6WithEvilPlanet(Time time, Person person)
        {
            //He will be poor if lords of the 2nd and 3rd are in the 6th with or aspected by evil planets.

            //lord 2 in 6th
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 6;

            //lord 3 in 6th
            var lord3 = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var lord3In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord3) == 6;

            //evil planets in 6th house OR aspecting the 6th
            var evilPlanetIn6 = AstronomicalCalculator.IsMaleficPlanetInHouse(6, time);
            var evilPlanetAspects6 = AstronomicalCalculator.IsMaleficPlanetAspectHouse(HouseName.House6, time);
            var evilPresentIn6 = evilPlanetIn6 || evilPlanetAspects6;

            //check if all conditions met
            var occuring = lord2In6 && lord3In6 && evilPresentIn6;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord2InHouse1)]
        public static CalculatorResult Lord2InHouse1(Time time, Person person)
        {
            //If the second lord is in the first — One earns money by his own exertions and generally by manual labour.

            //lord 2 in house 1
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In1 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 1;

            //check if all conditions met
            var occuring = lord2In1;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse1AndLord1InHouse2)]
        public static CalculatorResult Lord2InHouse1AndLord1InHouse2(Time time, Person person)
        {
            //In the second — Riches will be acquired without effort if the 1st and 2nd lords have exchanged their houses.
            //Note: Prediction is part of positions of lord 2 in varies houses,
            //      but for lord 2 in house 2, this "exchange" is mentioned.
            //      Further checking needed.

            //lord 1 in house 2
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var lord1In2 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord1) == 2;

            //lord 2 in house 1
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In1 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 1;

            //check if all conditions met
            var occuring = lord2In1 && lord1In2;

            var info = $"Lord 1:{lord1}/n Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse3)]
        public static CalculatorResult Lord2InHouse3(Time time, Person person)
        {
            //In the third — Loss from relatives, brothers and gain from travels and journeys.

            //lord 2 in house 3
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In3 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 3;

            //check if all conditions met
            var occuring = lord2In3;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse4)]
        public static CalculatorResult Lord2InHouse4(Time time, Person person)
        {
            //In the fourth - Through mother, inheritance.

            //lord 2 in house 4
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In4 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 4;

            //check if all conditions met
            var occuring = lord2In4;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse5)]
        public static CalculatorResult Lord2InHouse5(Time time, Person person)
        {
            //In the fifth — Ancestral properties, speculation and chance games.

            //lord 2 in house 5
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In5 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 5;

            //check if all conditions met
            var occuring = lord2In5;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse6)]
        public static CalculatorResult Lord2InHouse6(Time time, Person person)
        {
            //In the sixth — Broker's business, loss from relatives.

            //lord 2 in house 6
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 6;

            //check if all conditions met
            var occuring = lord2In6;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse7)]
        public static CalculatorResult Lord2InHouse7(Time time, Person person)
        {
            //In the seventh — Gain after marriage but loss from sickness, etc., of wife.

            //lord 2 in house 7
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In7 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 7;

            //check if all conditions met
            var occuring = lord2In7;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse8)]
        public static CalculatorResult Lord2InHouse8(Time time, Person person)
        {
            //In the eighth — Legacies and enemies (source of income).

            //lord 2 in house 8
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In8 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 8;

            //check if all conditions met
            var occuring = lord2In8;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse9)]
        public static CalculatorResult Lord2InHouse9(Time time, Person person)
        {
            //In the ninth — From father, voyages and shipping.

            //lord 2 in house 9
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In9 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 9;

            //check if all conditions met
            var occuring = lord2In9;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse10)]
        public static CalculatorResult Lord2InHouse10(Time time, Person person)
        {
            //In the tenth — Profession, eminent people, government favours.

            //lord 2 in house 10
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In10 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 10;

            //check if all conditions met
            var occuring = lord2In10;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse11)]
        public static CalculatorResult Lord2InHouse11(Time time, Person person)
        {
            //In the eleventh — From different means.

            //lord 2 in house 11
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In11 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 11;

            //check if all conditions met
            var occuring = lord2In11;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.Lord2InHouse12)]
        public static CalculatorResult Lord2InHouse12(Time time, Person person)
        {
            //In the twelfth — Gain from servants and unscrupulous means including illegal gratifications.

            //lord 2 in house 12
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In12 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 12;

            //check if all conditions met
            var occuring = lord2In12;

            var info = $"Lord 2:{lord2}";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.MaleficIn11FromArudha)]
        public static CalculatorResult MaleficIn11FromArudha(Time time, Person person)
        {
            //The just or unjust means of earning depends upon the presence of
            //benefic or malefic planets in the 11th from Arudha Lagna.

            //Note : here only malefic is checked, if benefic are present than not accounted for
            //      it is gussed that results would be mixed, needs further confirmation


            //get Arudha Lagna
            var arudhaLagna = AstronomicalCalculator.GetArudhaLagnaSign(time);

            //get 11th sign from Arudha lagna
            var sign11fromArudha = AstronomicalCalculator.GetSignCountedFromInputSign(arudhaLagna, 11);

            //see if malefic planets are in that sign
            var maleficFound = AstronomicalCalculator.IsMaleficPlanetInSign(sign11fromArudha, time);

            //check if all conditions met
            var occuring = maleficFound;

            var malefics = AstronomicalCalculator.GetMaleficPlanetListInSign(sign11fromArudha, time);
            var info = $"Malefic: {string.Join(" , ", malefics)}"; //space needed for word search
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.BeneficIn11FromArudha)]
        public static CalculatorResult BeneficIn11FromArudha(Time time, Person person)
        {
            //The just or unjust means of earning depends upon the presence of
            //benefic or malefic planets in the 11th from Arudha Lagna.

            //Note : here only benefic is checked, if malefic are present than not accounted for
            //      it is gussed that results would be mixed, needs further confirmation


            //get Arudha Lagna
            var arudhaLagna = AstronomicalCalculator.GetArudhaLagnaSign(time);

            //get 11th sign from Arudha lagna
            var sign11fromArudha = AstronomicalCalculator.GetSignCountedFromInputSign(arudhaLagna, 11);

            //see if benefic planets are in that sign
            var beneficFound = AstronomicalCalculator.IsBeneficPlanetInSign(sign11fromArudha, time);

            //check if all conditions met
            var occuring = beneficFound;

            var benefics = AstronomicalCalculator.GetBeneficPlanetListInSign(sign11fromArudha, time);
            var info = $"Benefic: {string.Join(" , ", benefics)}"; //space needed for word search
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }





        //TODO 
        //If the lord of the 2nd is Jupiter or Jupiter resides unaspected by malefics, there will be much wealth.

        //If the lord of the 2nd is Jupiter or Jupiter resides unaspected by malefics, there will be much wealth.
        //He loses wealth if Mercury (aspected by the Moon) contacts this combination.

        //If lords of the 2nd and 11th interchange their places(1) or both are in kendras or quadrants and one aspected
        //or joined by Mercury or Jupiter, the person will be pretty rich.

        //One will always be indigent if lords of the 2nd and 11th remain separate without evil planets or aspected by them.

        //Money will be spent on moral purposes when Jupiter is in the 11th house, Venus in the 2nd and its lord with benefics.

        //If the 2nd lord is with good planets in a kendra or if the 2nd house has all the good
        //association and aspects he will be on good terms with relatives.

        //One becomes a good mathematician if Mars is in the 2nd with the Moon or aspected by Mercury. The same result can be
        //foretold if Jupiter is in the ascendant and Saturn in the 8th or if Jupiter is in a quadrant and the lord of Lagna or Mercury is exalted.

        //The person will be an able debator if the Sun or the Moon is aspected by Jupiter or Venus.


        #endregion


        #region PLANETS IN SIGN

        //SUN
        [EventCalculator(EventName.SunInAries)]
        public static CalculatorResult SunInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.SunInTaurus)]
        public static CalculatorResult SunInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.SunInGemini)]
        public static CalculatorResult SunInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.SunInCancer)]
        public static CalculatorResult SunInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.SunInLeo)]
        public static CalculatorResult SunInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.SunInVirgo)]
        public static CalculatorResult SunInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.SunInLibra)]
        public static CalculatorResult SunInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.SunInScorpio)]
        public static CalculatorResult SunInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.SunInSagittarius)]
        public static CalculatorResult SunInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.SunInCapricornus)]
        public static CalculatorResult SunInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.SunInAquarius)]
        public static CalculatorResult SunInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.SunInPisces)]
        public static CalculatorResult SunInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };

        //MOON
        [EventCalculator(EventName.MoonInAries)]
        public static CalculatorResult MoonInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.MoonInTaurus)]
        public static CalculatorResult MoonInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.MoonInGemini)]
        public static CalculatorResult MoonInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.MoonInCancer)]
        public static CalculatorResult MoonInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.MoonInLeo)]
        public static CalculatorResult MoonInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.MoonInVirgo)]
        public static CalculatorResult MoonInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.MoonInLibra)]
        public static CalculatorResult MoonInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.MoonInScorpio)]
        public static CalculatorResult MoonInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.MoonInSagittarius)]
        public static CalculatorResult MoonInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.MoonInCapricornus)]
        public static CalculatorResult MoonInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.MoonInAquarius)]
        public static CalculatorResult MoonInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.MoonInPisces)]
        public static CalculatorResult MoonInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };

        //MARS
        [EventCalculator(EventName.MarsInAries)]
        public static CalculatorResult MarsInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.MarsInTaurus)]
        public static CalculatorResult MarsInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.MarsInGemini)]
        public static CalculatorResult MarsInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.MarsInCancer)]
        public static CalculatorResult MarsInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.MarsInLeo)]
        public static CalculatorResult MarsInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.MarsInVirgo)]
        public static CalculatorResult MarsInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.MarsInLibra)]
        public static CalculatorResult MarsInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.MarsInScorpio)]
        public static CalculatorResult MarsInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.MarsInSagittarius)]
        public static CalculatorResult MarsInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.MarsInCapricornus)]
        public static CalculatorResult MarsInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.MarsInAquarius)]
        public static CalculatorResult MarsInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.MarsInPisces)]
        public static CalculatorResult MarsInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };


        //MERCURY
        [EventCalculator(EventName.MercuryInAries)]
        public static CalculatorResult MercuryInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.MercuryInTaurus)]
        public static CalculatorResult MercuryInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.MercuryInGemini)]
        public static CalculatorResult MercuryInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.MercuryInCancer)]
        public static CalculatorResult MercuryInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.MercuryInLeo)]
        public static CalculatorResult MercuryInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.MercuryInVirgo)]
        public static CalculatorResult MercuryInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.MercuryInLibra)]
        public static CalculatorResult MercuryInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.MercuryInScorpio)]
        public static CalculatorResult MercuryInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.MercuryInSagittarius)]
        public static CalculatorResult MercuryInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.MercuryInCapricornus)]
        public static CalculatorResult MercuryInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.MercuryInAquarius)]
        public static CalculatorResult MercuryInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.MercuryInPisces)]
        public static CalculatorResult MercuryInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };


        //JUPITER
        [EventCalculator(EventName.JupiterInAries)]
        public static CalculatorResult JupiterInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.JupiterInTaurus)]
        public static CalculatorResult JupiterInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.JupiterInGemini)]
        public static CalculatorResult JupiterInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.JupiterInCancer)]
        public static CalculatorResult JupiterInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.JupiterInLeo)]
        public static CalculatorResult JupiterInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.JupiterInVirgo)]
        public static CalculatorResult JupiterInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.JupiterInLibra)]
        public static CalculatorResult JupiterInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.JupiterInScorpio)]
        public static CalculatorResult JupiterInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.JupiterInSagittarius)]
        public static CalculatorResult JupiterInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.JupiterInCapricornus)]
        public static CalculatorResult JupiterInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.JupiterInAquarius)]
        public static CalculatorResult JupiterInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.JupiterInPisces)]
        public static CalculatorResult JupiterInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };


        //VENUS
        [EventCalculator(EventName.VenusInAries)]
        public static CalculatorResult VenusInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.VenusInTaurus)]
        public static CalculatorResult VenusInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.VenusInGemini)]
        public static CalculatorResult VenusInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.VenusInCancer)]
        public static CalculatorResult VenusInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.VenusInLeo)]
        public static CalculatorResult VenusInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.VenusInVirgo)]
        public static CalculatorResult VenusInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.VenusInLibra)]
        public static CalculatorResult VenusInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.VenusInScorpio)]
        public static CalculatorResult VenusInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.VenusInSagittarius)]
        public static CalculatorResult VenusInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.VenusInCapricornus)]
        public static CalculatorResult VenusInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.VenusInAquarius)]
        public static CalculatorResult VenusInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.VenusInPisces)]
        public static CalculatorResult VenusInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };


        //SATURN
        [EventCalculator(EventName.SaturnInAries)]
        public static CalculatorResult SaturnInAries(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Aries
        };
        [EventCalculator(EventName.SaturnInTaurus)]
        public static CalculatorResult SaturnInTaurus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Taurus
        };
        [EventCalculator(EventName.SaturnInGemini)]
        public static CalculatorResult SaturnInGemini(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Gemini
        };
        [EventCalculator(EventName.SaturnInCancer)]
        public static CalculatorResult SaturnInCancer(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Cancer
        };
        [EventCalculator(EventName.SaturnInLeo)]
        public static CalculatorResult SaturnInLeo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Leo
        };
        [EventCalculator(EventName.SaturnInVirgo)]
        public static CalculatorResult SaturnInVirgo(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Virgo
        };
        [EventCalculator(EventName.SaturnInLibra)]
        public static CalculatorResult SaturnInLibra(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Libra
        };
        [EventCalculator(EventName.SaturnInScorpio)]
        public static CalculatorResult SaturnInScorpio(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Scorpio
        };
        [EventCalculator(EventName.SaturnInSagittarius)]
        public static CalculatorResult SaturnInSagittarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Sagittarius
        };
        [EventCalculator(EventName.SaturnInCapricornus)]
        public static CalculatorResult SaturnInCapricornus(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Capricornus
        };
        [EventCalculator(EventName.SaturnInAquarius)]
        public static CalculatorResult SaturnInAquarius(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Aquarius
        };
        [EventCalculator(EventName.SaturnInPisces)]
        public static CalculatorResult SaturnInPisces(Time time, Person person) => new()
        {
            Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime)
                .GetSignName() == ZodiacName.Pisces
        };



        #endregion


        //CUSTOM
        [EventCalculator(EventName.GeminiRisingWithEvilPlanet)]
        public static CalculatorResult GeminiRisingWithEvilPlanet(Time time, Person person)
        {
            //1.gemini rising 
            var geminiRising = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Gemini;

            //2.find evil planets in gemini
            //get planets in sign
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(ZodiacName.Gemini, time);
            //filer in only evil (malefic) planets 
            var evilPlanets = planetsInSign.Where(planet => AstronomicalCalculator.IsPlanetMalefic(planet, time));
            //mark if evil planets found in sign
            var evilPlanetFound = evilPlanets.Any();


            //both must be true for event to occur
            var occuring = geminiRising && evilPlanetFound;

            //extra info
            var info = $"Malefic: {string.Join(" , ", evilPlanets)}"; //space needed for word search
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        [EventCalculator(EventName.AriesRisingWithEvilPlanet)]
        public static CalculatorResult AriesRisingWithEvilPlanet(Time time, Person person)
        {
            //Mental affliction and derangement are also likely since Saturn and the Moon are in Aries.

            //1.aries rising 
            var ariesRising = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Aries;

            //2.find if Saturn and the Moon are in Aries.
            //get planets in sign
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(ZodiacName.Aries, time);
            var evilPlanetFound = planetsInSign.Contains(PlanetName.Saturn) || planetsInSign.Contains(PlanetName.Moon);


            //both must be true for event to occur
            var occuring = ariesRising && evilPlanetFound;

            //extra info
            var info = $"Malefic:Saturn, Moon";
            return new CalculatorResult() { Occuring = occuring, Info = info };
        }

        #endregion

        #region GOCHARA

        [EventCalculator(EventName.SunGocharaInHouse1)]
        public static CalculatorResult SunGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 1) };

        [EventCalculator(EventName.SunGocharaInHouse2)]
        public static CalculatorResult SunGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 2) };

        [EventCalculator(EventName.SunGocharaInHouse3)]
        public static CalculatorResult SunGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 3) };

        [EventCalculator(EventName.SunGocharaInHouse4)]
        public static CalculatorResult SunGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 4) };

        [EventCalculator(EventName.SunGocharaInHouse5)]
        public static CalculatorResult SunGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 5) };

        [EventCalculator(EventName.SunGocharaInHouse6)]
        public static CalculatorResult SunGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 6) };

        [EventCalculator(EventName.SunGocharaInHouse7)]
        public static CalculatorResult SunGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 7) };

        [EventCalculator(EventName.SunGocharaInHouse8)]
        public static CalculatorResult SunGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 8) };

        [EventCalculator(EventName.SunGocharaInHouse9)]
        public static CalculatorResult SunGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 9) };

        [EventCalculator(EventName.SunGocharaInHouse10)]
        public static CalculatorResult SunGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 10) };

        [EventCalculator(EventName.SunGocharaInHouse11)]
        public static CalculatorResult SunGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 11) };

        [EventCalculator(EventName.SunGocharaInHouse12)]
        public static CalculatorResult SunGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 12) };

        //MOON
        [EventCalculator(EventName.MoonGocharaInHouse1)]
        public static CalculatorResult MoonGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 1) };

        [EventCalculator(EventName.MoonGocharaInHouse2)]
        public static CalculatorResult MoonGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 2) };

        [EventCalculator(EventName.MoonGocharaInHouse3)]
        public static CalculatorResult MoonGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 3) };

        [EventCalculator(EventName.MoonGocharaInHouse4)]
        public static CalculatorResult MoonGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 4) };

        [EventCalculator(EventName.MoonGocharaInHouse5)]
        public static CalculatorResult MoonGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 5) };

        [EventCalculator(EventName.MoonGocharaInHouse6)]
        public static CalculatorResult MoonGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 6) };

        [EventCalculator(EventName.MoonGocharaInHouse7)]
        public static CalculatorResult MoonGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 7) };

        [EventCalculator(EventName.MoonGocharaInHouse8)]
        public static CalculatorResult MoonGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 8) };

        [EventCalculator(EventName.MoonGocharaInHouse9)]
        public static CalculatorResult MoonGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 9) };

        [EventCalculator(EventName.MoonGocharaInHouse10)]
        public static CalculatorResult MoonGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 10) };

        [EventCalculator(EventName.MoonGocharaInHouse11)]
        public static CalculatorResult MoonGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 11) };

        [EventCalculator(EventName.MoonGocharaInHouse12)]
        public static CalculatorResult MoonGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 12) };


        //MARS
        [EventCalculator(EventName.MarsGocharaInHouse1)]
        public static CalculatorResult MarsGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 1) };

        [EventCalculator(EventName.MarsGocharaInHouse2)]
        public static CalculatorResult MarsGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 2) };

        [EventCalculator(EventName.MarsGocharaInHouse3)]
        public static CalculatorResult MarsGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 3) };

        [EventCalculator(EventName.MarsGocharaInHouse4)]
        public static CalculatorResult MarsGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 4) };

        [EventCalculator(EventName.MarsGocharaInHouse5)]
        public static CalculatorResult MarsGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 5) };

        [EventCalculator(EventName.MarsGocharaInHouse6)]
        public static CalculatorResult MarsGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 6) };

        [EventCalculator(EventName.MarsGocharaInHouse7)]
        public static CalculatorResult MarsGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 7) };

        [EventCalculator(EventName.MarsGocharaInHouse8)]
        public static CalculatorResult MarsGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 8) };

        [EventCalculator(EventName.MarsGocharaInHouse9)]
        public static CalculatorResult MarsGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 9) };

        [EventCalculator(EventName.MarsGocharaInHouse10)]
        public static CalculatorResult MarsGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 10) };

        [EventCalculator(EventName.MarsGocharaInHouse11)]
        public static CalculatorResult MarsGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 11) };

        [EventCalculator(EventName.MarsGocharaInHouse12)]
        public static CalculatorResult MarsGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 12) };



        //MERCURY
        [EventCalculator(EventName.MercuryGocharaInHouse1)]
        public static CalculatorResult MercuryGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 1) };

        [EventCalculator(EventName.MercuryGocharaInHouse2)]
        public static CalculatorResult MercuryGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 2) };

        [EventCalculator(EventName.MercuryGocharaInHouse3)]
        public static CalculatorResult MercuryGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 3) };

        [EventCalculator(EventName.MercuryGocharaInHouse4)]
        public static CalculatorResult MercuryGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 4) };

        [EventCalculator(EventName.MercuryGocharaInHouse5)]
        public static CalculatorResult MercuryGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 5) };

        [EventCalculator(EventName.MercuryGocharaInHouse6)]
        public static CalculatorResult MercuryGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 6) };

        [EventCalculator(EventName.MercuryGocharaInHouse7)]
        public static CalculatorResult MercuryGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 7) };

        [EventCalculator(EventName.MercuryGocharaInHouse8)]
        public static CalculatorResult MercuryGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 8) };

        [EventCalculator(EventName.MercuryGocharaInHouse9)]
        public static CalculatorResult MercuryGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 9) };

        [EventCalculator(EventName.MercuryGocharaInHouse10)]
        public static CalculatorResult MercuryGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 10) };

        [EventCalculator(EventName.MercuryGocharaInHouse11)]
        public static CalculatorResult MercuryGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 11) };

        [EventCalculator(EventName.MercuryGocharaInHouse12)]
        public static CalculatorResult MercuryGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 12) };


        //JUPITER
        [EventCalculator(EventName.JupiterGocharaInHouse1)]
        public static CalculatorResult JupiterGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 1) };

        [EventCalculator(EventName.JupiterGocharaInHouse2)]
        public static CalculatorResult JupiterGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 2) };

        [EventCalculator(EventName.JupiterGocharaInHouse3)]
        public static CalculatorResult JupiterGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 3) };

        [EventCalculator(EventName.JupiterGocharaInHouse4)]
        public static CalculatorResult JupiterGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 4) };

        [EventCalculator(EventName.JupiterGocharaInHouse5)]
        public static CalculatorResult JupiterGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 5) };

        [EventCalculator(EventName.JupiterGocharaInHouse6)]
        public static CalculatorResult JupiterGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 6) };

        [EventCalculator(EventName.JupiterGocharaInHouse7)]
        public static CalculatorResult JupiterGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 7) };

        [EventCalculator(EventName.JupiterGocharaInHouse8)]
        public static CalculatorResult JupiterGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 8) };

        [EventCalculator(EventName.JupiterGocharaInHouse9)]
        public static CalculatorResult JupiterGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 9) };

        [EventCalculator(EventName.JupiterGocharaInHouse10)]
        public static CalculatorResult JupiterGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 10) };

        [EventCalculator(EventName.JupiterGocharaInHouse11)]
        public static CalculatorResult JupiterGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 11) };

        [EventCalculator(EventName.JupiterGocharaInHouse12)]
        public static CalculatorResult JupiterGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 12) };


        //VENUS
        [EventCalculator(EventName.VenusGocharaInHouse1)]
        public static CalculatorResult VenusGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 1) };

        [EventCalculator(EventName.VenusGocharaInHouse2)]
        public static CalculatorResult VenusGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 2) };

        [EventCalculator(EventName.VenusGocharaInHouse3)]
        public static CalculatorResult VenusGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 3) };

        [EventCalculator(EventName.VenusGocharaInHouse4)]
        public static CalculatorResult VenusGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 4) };

        [EventCalculator(EventName.VenusGocharaInHouse5)]
        public static CalculatorResult VenusGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 5) };

        [EventCalculator(EventName.VenusGocharaInHouse6)]
        public static CalculatorResult VenusGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 6) };

        [EventCalculator(EventName.VenusGocharaInHouse7)]
        public static CalculatorResult VenusGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 7) };

        [EventCalculator(EventName.VenusGocharaInHouse8)]
        public static CalculatorResult VenusGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 8) };

        [EventCalculator(EventName.VenusGocharaInHouse9)]
        public static CalculatorResult VenusGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 9) };

        [EventCalculator(EventName.VenusGocharaInHouse10)]
        public static CalculatorResult VenusGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 10) };

        [EventCalculator(EventName.VenusGocharaInHouse11)]
        public static CalculatorResult VenusGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 11) };

        [EventCalculator(EventName.VenusGocharaInHouse12)]
        public static CalculatorResult VenusGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 12) };


        //SATURN
        [EventCalculator(EventName.SaturnGocharaInHouse1)]
        public static CalculatorResult SaturnGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 1) };

        [EventCalculator(EventName.SaturnGocharaInHouse2)]
        public static CalculatorResult SaturnGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 2) };

        [EventCalculator(EventName.SaturnGocharaInHouse3)]
        public static CalculatorResult SaturnGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 3) };

        [EventCalculator(EventName.SaturnGocharaInHouse4)]
        public static CalculatorResult SaturnGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 4) };

        [EventCalculator(EventName.SaturnGocharaInHouse5)]
        public static CalculatorResult SaturnGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 5) };

        [EventCalculator(EventName.SaturnGocharaInHouse6)]
        public static CalculatorResult SaturnGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 6) };

        [EventCalculator(EventName.SaturnGocharaInHouse7)]
        public static CalculatorResult SaturnGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 7) };

        [EventCalculator(EventName.SaturnGocharaInHouse8)]
        public static CalculatorResult SaturnGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 8) };

        [EventCalculator(EventName.SaturnGocharaInHouse9)]
        public static CalculatorResult SaturnGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 9) };

        [EventCalculator(EventName.SaturnGocharaInHouse10)]
        public static CalculatorResult SaturnGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 10) };

        [EventCalculator(EventName.SaturnGocharaInHouse11)]
        public static CalculatorResult SaturnGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 11) };

        [EventCalculator(EventName.SaturnGocharaInHouse12)]
        public static CalculatorResult SaturnGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 12) };


        //RAHU
        [EventCalculator(EventName.RahuGocharaInHouse1)]
        public static CalculatorResult RahuGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 1) };

        [EventCalculator(EventName.RahuGocharaInHouse2)]
        public static CalculatorResult RahuGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 2) };

        [EventCalculator(EventName.RahuGocharaInHouse3)]
        public static CalculatorResult RahuGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 3) };

        [EventCalculator(EventName.RahuGocharaInHouse4)]
        public static CalculatorResult RahuGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 4) };

        [EventCalculator(EventName.RahuGocharaInHouse5)]
        public static CalculatorResult RahuGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 5) };

        [EventCalculator(EventName.RahuGocharaInHouse6)]
        public static CalculatorResult RahuGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 6) };

        [EventCalculator(EventName.RahuGocharaInHouse7)]
        public static CalculatorResult RahuGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 7) };

        [EventCalculator(EventName.RahuGocharaInHouse8)]
        public static CalculatorResult RahuGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 8) };

        [EventCalculator(EventName.RahuGocharaInHouse9)]
        public static CalculatorResult RahuGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 9) };

        [EventCalculator(EventName.RahuGocharaInHouse10)]
        public static CalculatorResult RahuGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 10) };

        [EventCalculator(EventName.RahuGocharaInHouse11)]
        public static CalculatorResult RahuGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 11) };

        [EventCalculator(EventName.RahuGocharaInHouse12)]
        public static CalculatorResult RahuGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 12) };


        //KETU
        [EventCalculator(EventName.KetuGocharaInHouse1)]
        public static CalculatorResult KetuGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 1) };

        [EventCalculator(EventName.KetuGocharaInHouse2)]
        public static CalculatorResult KetuGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 2) };

        [EventCalculator(EventName.KetuGocharaInHouse3)]
        public static CalculatorResult KetuGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 3) };

        [EventCalculator(EventName.KetuGocharaInHouse4)]
        public static CalculatorResult KetuGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 4) };

        [EventCalculator(EventName.KetuGocharaInHouse5)]
        public static CalculatorResult KetuGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 5) };

        [EventCalculator(EventName.KetuGocharaInHouse6)]
        public static CalculatorResult KetuGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 6) };

        [EventCalculator(EventName.KetuGocharaInHouse7)]
        public static CalculatorResult KetuGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 7) };

        [EventCalculator(EventName.KetuGocharaInHouse8)]
        public static CalculatorResult KetuGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 8) };

        [EventCalculator(EventName.KetuGocharaInHouse9)]
        public static CalculatorResult KetuGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 9) };

        [EventCalculator(EventName.KetuGocharaInHouse10)]
        public static CalculatorResult KetuGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 10) };

        [EventCalculator(EventName.KetuGocharaInHouse11)]
        public static CalculatorResult KetuGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 11) };

        [EventCalculator(EventName.KetuGocharaInHouse12)]
        public static CalculatorResult KetuGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 12) };

        #endregion

        #region DASAS

        #region SUN DASA

        [EventCalculator(EventName.AriesSunDasa)]
        public static CalculatorResult AriesSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSunDasa)]
        public static CalculatorResult TaurusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSunDasa)]
        public static CalculatorResult GeminiSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSunDasa)]
        public static CalculatorResult CancerSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSunDasa)]
        public static CalculatorResult LeoSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSunDasa)]
        public static CalculatorResult VirgoSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSunDasa)]
        public static CalculatorResult LibraSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSunDasa)]
        public static CalculatorResult ScorpioSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSunDasa)]
        public static CalculatorResult SagittariusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusSunDasa)]
        public static CalculatorResult CapricornusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSunDasa)]
        public static CalculatorResult AquariusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSunDasa)]
        public static CalculatorResult PiscesSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }


        #endregion

        #region MOON DASA

        [EventCalculator(EventName.AriesMoonDasa)]
        public static CalculatorResult AriesMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMoonDasa)]
        public static CalculatorResult TaurusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMoonDasa)]
        public static CalculatorResult GeminiMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMoonDasa)]
        public static CalculatorResult CancerMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMoonDasa)]
        public static CalculatorResult LeoMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMoonDasa)]
        public static CalculatorResult VirgoMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMoonDasa)]
        public static CalculatorResult LibraMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMoonDasa)]
        public static CalculatorResult ScorpioMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMoonDasa)]
        public static CalculatorResult SagittariusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMoonDasa)]
        public static CalculatorResult CapricornusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMoonDasa)]
        public static CalculatorResult AquariusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMoonDasa)]
        public static CalculatorResult PiscesMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }


        #endregion

        #region MARS DASA

        [EventCalculator(EventName.AriesMarsDasa)]
        public static CalculatorResult AriesMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMarsDasa)]
        public static CalculatorResult TaurusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMarsDasa)]
        public static CalculatorResult GeminiMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMarsDasa)]
        public static CalculatorResult CancerMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMarsDasa)]
        public static CalculatorResult LeoMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMarsDasa)]
        public static CalculatorResult VirgoMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMarsDasa)]
        public static CalculatorResult LibraMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMarsDasa)]
        public static CalculatorResult ScorpioMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMarsDasa)]
        public static CalculatorResult SagittariusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMarsDasa)]
        public static CalculatorResult CapricornusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMarsDasa)]
        public static CalculatorResult AquariusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMarsDasa)]
        public static CalculatorResult PiscesMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion

        #region RAHU DASA

        [EventCalculator(EventName.AriesRahuDasa)]
        public static CalculatorResult AriesRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusRahuDasa)]
        public static CalculatorResult TaurusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiRahuDasa)]
        public static CalculatorResult GeminiRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerRahuDasa)]
        public static CalculatorResult CancerRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoRahuDasa)]
        public static CalculatorResult LeoRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoRahuDasa)]
        public static CalculatorResult VirgoRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraRahuDasa)]
        public static CalculatorResult LibraRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioRahuDasa)]
        public static CalculatorResult ScorpioRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusRahuDasa)]
        public static CalculatorResult SagittariusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusRahuDasa)]
        public static CalculatorResult CapricornusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusRahuDasa)]
        public static CalculatorResult AquariusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesRahuDasa)]
        public static CalculatorResult PiscesRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }


        #endregion

        #region JUPITER DASA

        [EventCalculator(EventName.AriesJupiterDasa)]
        public static CalculatorResult AriesJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusJupiterDasa)]
        public static CalculatorResult TaurusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiJupiterDasa)]
        public static CalculatorResult GeminiJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerJupiterDasa)]
        public static CalculatorResult CancerJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoJupiterDasa)]
        public static CalculatorResult LeoJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoJupiterDasa)]
        public static CalculatorResult VirgoJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraJupiterDasa)]
        public static CalculatorResult LibraJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioJupiterDasa)]
        public static CalculatorResult ScorpioJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusJupiterDasa)]
        public static CalculatorResult SagittariusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusJupiterDasa)]
        public static CalculatorResult CapricornusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusJupiterDasa)]
        public static CalculatorResult AquariusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesJupiterDasa)]
        public static CalculatorResult PiscesJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }



        #endregion

        #region SATURN DASA

        [EventCalculator(EventName.AriesSaturnDasa)]
        public static CalculatorResult AriesSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSaturnDasa)]
        public static CalculatorResult TaurusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSaturnDasa)]
        public static CalculatorResult GeminiSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSaturnDasa)]
        public static CalculatorResult CancerSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSaturnDasa)]
        public static CalculatorResult LeoSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSaturnDasa)]
        public static CalculatorResult VirgoSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSaturnDasa)]
        public static CalculatorResult LibraSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSaturnDasa)]
        public static CalculatorResult ScorpioSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSaturnDasa)]
        public static CalculatorResult SagittariusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusSaturnDasa)]
        public static CalculatorResult CapricornusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSaturnDasa)]
        public static CalculatorResult AquariusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSaturnDasa)]
        public static CalculatorResult PiscesSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion

        #region MERCURY DASA

        [EventCalculator(EventName.AriesMercuryDasa)]
        public static CalculatorResult AriesMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMercuryDasa)]
        public static CalculatorResult TaurusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMercuryDasa)]
        public static CalculatorResult GeminiMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMercuryDasa)]
        public static CalculatorResult CancerMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMercuryDasa)]
        public static CalculatorResult LeoMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMercuryDasa)]
        public static CalculatorResult VirgoMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMercuryDasa)]
        public static CalculatorResult LibraMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMercuryDasa)]
        public static CalculatorResult ScorpioMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMercuryDasa)]
        public static CalculatorResult SagittariusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMercuryDasa)]
        public static CalculatorResult CapricornusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMercuryDasa)]
        public static CalculatorResult AquariusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMercuryDasa)]
        public static CalculatorResult PiscesMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }


        #endregion

        #region KETU DASA
        [EventCalculator(EventName.AriesKetuDasa)]
        public static CalculatorResult AriesKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusKetuDasa)]
        public static CalculatorResult TaurusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiKetuDasa)]
        public static CalculatorResult GeminiKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerKetuDasa)]
        public static CalculatorResult CancerKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoKetuDasa)]
        public static CalculatorResult LeoKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoKetuDasa)]
        public static CalculatorResult VirgoKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraKetuDasa)]
        public static CalculatorResult LibraKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioKetuDasa)]
        public static CalculatorResult ScorpioKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusKetuDasa)]
        public static CalculatorResult SagittariusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusKetuDasa)]
        public static CalculatorResult CapricornusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusKetuDasa)]
        public static CalculatorResult AquariusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesKetuDasa)]
        public static CalculatorResult PiscesKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }



        #endregion

        #region VENUS DASA

        [EventCalculator(EventName.AriesVenusDasa)]
        public static CalculatorResult AriesVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusVenusDasa)]
        public static CalculatorResult TaurusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiVenusDasa)]
        public static CalculatorResult GeminiVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerVenusDasa)]
        public static CalculatorResult CancerVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoVenusDasa)]
        public static CalculatorResult LeoVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoVenusDasa)]
        public static CalculatorResult VirgoVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraVenusDasa)]
        public static CalculatorResult LibraVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioVenusDasa)]
        public static CalculatorResult ScorpioVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusVenusDasa)]
        public static CalculatorResult SagittariusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusVenusDasa)]
        public static CalculatorResult CapricornusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusVenusDasa)]
        public static CalculatorResult AquariusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesVenusDasa)]
        public static CalculatorResult PiscesVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion

        #region SUN BHUKTI

        [EventCalculator(EventName.SunDasaSunBhukti)]
        public static CalculatorResult SunDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaMoonBhukti)]
        public static CalculatorResult SunDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaMarsBhukti)]
        public static CalculatorResult SunDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaRahuBhukti)]
        public static CalculatorResult SunDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaJupiterBhukti)]
        public static CalculatorResult SunDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaSaturnBhukti)]
        public static CalculatorResult SunDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaMercuryBhukti)]
        public static CalculatorResult SunDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaKetuBhukti)]
        public static CalculatorResult SunDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaVenusBhukti)]
        public static CalculatorResult SunDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region MOON BHUKTI

        [EventCalculator(EventName.MoonDasaSunBhukti)]
        public static CalculatorResult MoonDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaMoonBhukti)]
        public static CalculatorResult MoonDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaMarsBhukti)]
        public static CalculatorResult MoonDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaRahuBhukti)]
        public static CalculatorResult MoonDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaJupiterBhukti)]
        public static CalculatorResult MoonDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaSaturnBhukti)]
        public static CalculatorResult MoonDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaMercuryBhukti)]
        public static CalculatorResult MoonDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaKetuBhukti)]
        public static CalculatorResult MoonDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaVenusBhukti)]
        public static CalculatorResult MoonDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region MARS BHUKTI

        [EventCalculator(EventName.MarsDasaSunBhukti)]
        public static CalculatorResult MarsDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaMoonBhukti)]
        public static CalculatorResult MarsDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaMarsBhukti)]
        public static CalculatorResult MarsDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaRahuBhukti)]
        public static CalculatorResult MarsDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaJupiterBhukti)]
        public static CalculatorResult MarsDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaSaturnBhukti)]
        public static CalculatorResult MarsDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaMercuryBhukti)]
        public static CalculatorResult MarsDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaKetuBhukti)]
        public static CalculatorResult MarsDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaVenusBhukti)]
        public static CalculatorResult MarsDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }



        #endregion

        #region RAHU BHUKTI

        [EventCalculator(EventName.RahuDasaSunBhukti)]
        public static CalculatorResult RahuDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaMoonBhukti)]
        public static CalculatorResult RahuDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaMarsBhukti)]
        public static CalculatorResult RahuDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaRahuBhukti)]
        public static CalculatorResult RahuDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaJupiterBhukti)]
        public static CalculatorResult RahuDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaSaturnBhukti)]
        public static CalculatorResult RahuDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaMercuryBhukti)]
        public static CalculatorResult RahuDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaKetuBhukti)]
        public static CalculatorResult RahuDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaVenusBhukti)]
        public static CalculatorResult RahuDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region JUPITER BHUKTI

        [EventCalculator(EventName.JupiterDasaSunBhukti)]
        public static CalculatorResult JupiterDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaMoonBhukti)]
        public static CalculatorResult JupiterDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaMarsBhukti)]
        public static CalculatorResult JupiterDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaRahuBhukti)]
        public static CalculatorResult JupiterDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaJupiterBhukti)]
        public static CalculatorResult JupiterDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaSaturnBhukti)]
        public static CalculatorResult JupiterDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaMercuryBhukti)]
        public static CalculatorResult JupiterDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaKetuBhukti)]
        public static CalculatorResult JupiterDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaVenusBhukti)]
        public static CalculatorResult JupiterDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region SATURN BHUKTI

        [EventCalculator(EventName.SaturnDasaSunBhukti)]
        public static CalculatorResult SaturnDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaMoonBhukti)]
        public static CalculatorResult SaturnDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaMarsBhukti)]
        public static CalculatorResult SaturnDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaRahuBhukti)]
        public static CalculatorResult SaturnDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaJupiterBhukti)]
        public static CalculatorResult SaturnDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaSaturnBhukti)]
        public static CalculatorResult SaturnDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaMercuryBhukti)]
        public static CalculatorResult SaturnDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaKetuBhukti)]
        public static CalculatorResult SaturnDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaVenusBhukti)]
        public static CalculatorResult SaturnDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region MERCURY BHUKTI

        [EventCalculator(EventName.MercuryDasaSunBhukti)]
        public static CalculatorResult MercuryDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaMoonBhukti)]
        public static CalculatorResult MercuryDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaMarsBhukti)]
        public static CalculatorResult MercuryDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaRahuBhukti)]
        public static CalculatorResult MercuryDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaJupiterBhukti)]
        public static CalculatorResult MercuryDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaSaturnBhukti)]
        public static CalculatorResult MercuryDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaMercuryBhukti)]
        public static CalculatorResult MercuryDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaKetuBhukti)]
        public static CalculatorResult MercuryDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaVenusBhukti)]
        public static CalculatorResult MercuryDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region KETU BHUKTI

        [EventCalculator(EventName.KetuDasaSunBhukti)]
        public static CalculatorResult KetuDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaMoonBhukti)]
        public static CalculatorResult KetuDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaMarsBhukti)]
        public static CalculatorResult KetuDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaRahuBhukti)]
        public static CalculatorResult KetuDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaJupiterBhukti)]
        public static CalculatorResult KetuDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaSaturnBhukti)]
        public static CalculatorResult KetuDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaMercuryBhukti)]
        public static CalculatorResult KetuDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaKetuBhukti)]
        public static CalculatorResult KetuDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaVenusBhukti)]
        public static CalculatorResult KetuDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion

        #region VENUS BHUKTI

        [EventCalculator(EventName.VenusDasaSunBhukti)]
        public static CalculatorResult VenusDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;


            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaMoonBhukti)]
        public static CalculatorResult VenusDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaMarsBhukti)]
        public static CalculatorResult VenusDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaRahuBhukti)]
        public static CalculatorResult VenusDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaJupiterBhukti)]
        public static CalculatorResult VenusDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaSaturnBhukti)]
        public static CalculatorResult VenusDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaMercuryBhukti)]
        public static CalculatorResult VenusDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaKetuBhukti)]
        public static CalculatorResult VenusDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaVenusBhukti)]
        public static CalculatorResult VenusDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;


            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }


        #endregion

        #region ANTARAM

        [EventCalculator(EventName.SunAntaram)]
        public static CalculatorResult SunAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Sun);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.MoonAntaram)]
        public static CalculatorResult MoonAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Moon);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.MarsAntaram)]
        public static CalculatorResult MarsAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Mars);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.RahuAntaram)]
        public static CalculatorResult RahuAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            //TODO rahu & ketu will always return neutral
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Rahu);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.JupiterAntaram)]
        public static CalculatorResult JupiterAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Jupiter);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.SaturnAntaram)]
        public static CalculatorResult SaturnAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Saturn);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.MercuryAntaram)]
        public static CalculatorResult MercuryAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Mercury);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.KetuAntaram)]
        public static CalculatorResult KetuAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            //TODO rahu & ketu will always return neutral
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Ketu);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        [EventCalculator(EventName.VenusAntaram)]
        public static CalculatorResult VenusAntaram(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check antaram
            var isCorrectDasa = currentDasaBhuktiAntaram.Antaram == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa;

            //override nature of antaram based on planet
            var planetNature = AstronomicalCalculator.GetPlanetAntaramNature(person, PlanetName.Venus);

            return new() { Occuring = occuring, NatureOverride = planetNature };
        }

        #endregion

        #region DASA SPECIAL RULES

        [EventCalculator(EventName.Lord6And8Dasa)]
        public static CalculatorResult Lord6And8Dasa(Time time, Person person)
        {
            //The Dasa period of the lords of the 6th and the 8th
            // produce harmful results unless they acquire beneficence
            // otherwise.


            //get lord 6th house
            var lord6th = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, person.BirthTime);

            //is lord 6th dasa occuring
            var isLord6thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord6th;


            //get lord 8th house
            var lord8th = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord8thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord8th;


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
            var lord5th = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, person.BirthTime);

            //is lord 5th dasa occuring
            var isLord5thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord5th;


            //get lord 9th house
            var lord9th = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord9thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord9th;


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
            var lord5th = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, person.BirthTime);
            //get lord 9th house
            var lord9th = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 5th dasa occuring
            var isLord5thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord5th;
            var isLord5thBhukti = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Bhukti == lord5th;

            //is lord 9th dasa occuring
            var isLord9thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord9th;
            var isLord9thBhukti = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Bhukti == lord9th;

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
            var buhktiLord = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Bhukti;

            //get dasa lord = 
            var dasaLord = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa;

            //condition 1
            //is bukti lord in 6th house at birth
            var bhuktiLordIn6th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, buhktiLord, 6);
            //is dasa lord in 8th house at birth
            var dasaLordIn8th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, dasaLord, 8);
            //check if both planets are in bad houses at the same time
            var buhktiDasaIn6And8 = bhuktiLordIn6th && dasaLordIn8th;


            //condition 2
            //is bukti lord in 12th house at birth
            var bhuktiLordIn12th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, buhktiLord, 12);
            //is dasa lord in 2nd house at birth
            var dasaLordIn2nd = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, dasaLord, 2);
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
            var lordHouse2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, person.BirthTime);

            //is lord 2nd dasa occuring
            var isLord2Dasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lordHouse2;

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
            var lordHouse3 = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, person.BirthTime);

            //is lord 3rd dasa occuring
            var isLord3Dasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lordHouse3;

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
            var lordHouse1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime);

            //is lord 1st house dasa occuring
            var isLord1Dasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lordHouse1;

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
            var isSaturnDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //is the 4th dasa
            var is4thDasa = AstronomicalCalculator.GetCurrentDasaCountFromBirth(person.BirthTime, time) == 4;

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
            var isJupiterDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //is the 6th dasa
            var is6thDasa = AstronomicalCalculator.GetCurrentDasaCountFromBirth(person.BirthTime, time) == 6;

            //occuring if one of the conditions met
            var occuring = isJupiterDasa && is6thDasa;

            return new() { Occuring = occuring };
        }




        #endregion

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
            var lunarMonth = AstronomicalCalculator.GetLunarMonth(time);

            if (lunarMonth is LunarMonth.Jaistam or LunarMonth.Ashadam or LunarMonth.Bhadrapadam
                or LunarMonth.Aswijam or LunarMonth.Margasiram or LunarMonth.Pooshiam or LunarMonth.Phalgunam)
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
            var sunSign = AstronomicalCalculator.GetSunSign(time).GetSignName();

            //check if sign is a fixed or movable sign
            var isFixedSign = AstronomicalCalculator.IsFixedSign(sunSign);
            var isMovableSign = AstronomicalCalculator.IsMovableSign(sunSign);
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
            var sunSign = AstronomicalCalculator.GetSunSign(time).GetSignName();

            //check if sign is a common sign
            var isCommonSign = AstronomicalCalculator.IsCommonSign(sunSign);
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
            var lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

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
            var weekday = AstronomicalCalculator.GetDayOfWeek(time);


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
            var moonPhase = AstronomicalCalculator.GetLunarDay(time).GetMoonPhase();

            //occuring when moon is wanning
            var occuring = moonPhase == MoonPhase.DarkHalf;

            return new CalculatorResult() { Occuring = occuring };
        }

        [EventCalculator(EventName.BadWeekDayForBuilding)]
        public static CalculatorResult BadWeekDayForBuilding(Time time, Person person)
        {
            //Saturday should be rejected as it connots frequent thefts. Sunday
            // should also be avoided unless the day is otherwise very auspicious.

            //get week day
            var weekday = AstronomicalCalculator.GetDayOfWeek(time);


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
            var weekday = AstronomicalCalculator.GetDayOfWeek(time);


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
            var isFriday = AstronomicalCalculator.GetDayOfWeek(time) == DayOfWeek.Friday;
            var lagnaIsTaurus = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Taurus;
            var lagnaIsLibra = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Libra;
            var isFridayLagnaTaurusLibra = isFriday && (lagnaIsLibra || lagnaIsTaurus);

            //monday & lagna is cancer
            var isMonday = AstronomicalCalculator.GetDayOfWeek(time) == DayOfWeek.Monday;
            var lagnaIsCancer = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Cancer;
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
            var beneficsInLagna = AstronomicalCalculator.IsBeneficPlanetInHouse(1, time);

            //monday in aquatic sign
            var moonSign = AstronomicalCalculator.GetMoonSignName(time);
            var isMoonInAquaticSign = AstronomicalCalculator.IsAquaticSign(moonSign);

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