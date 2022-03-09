

using System;
using Genso.Astrology.Library;
using Genso.Astrology.Muhurtha.Core;

namespace Compatibility
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //get all the people
            var peopleList = MuhurthaCore.GetAllPeopleList();

            //filter out the male and get female ones we want
            var male = peopleList.Find(person => person.GetName() == "Ravi");
            var female = peopleList.Find(person => person.GetName() == "Prema");


            //do the calculations
            DinaKuta(male, female);
            GanaKuta(male, female);
            Mahendra(male, female);
            StreeDeergha(male, female);
            RasiKuta(male, female);
            GrahaMaitram(male, female);
            VasyaKuta(male, female);
            Rajju(male, female);
            VedhaKuta(male, female);


            Console.ReadLine();
        }

        public static void Mahendra(Person male, Person female)
        {
            //Mahendra. - The constellation of the boy counted from that of the girl
            // should be the 4th, 7th, 10th, 13th, 16th, 19th, 22nd or 25th. This
            // promotes well-being and increases longevity.

            //get ruling sign
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());

            var count = AstronomicalCalculator.CountFromConstellationToConstellation(femaleConstellation, maleConstellation);

            if (count == 4 || count == 7 || count == 10 | count == 13 || count == 16 || count == 19 || count == 22 || count == 25)
            {
                Console.WriteLine("Mahendra : Promotes well-being and increases longevity");
            }
            else
            {
                Console.WriteLine("Mahendra : Bad");
            }
        }

        public static void GanaKuta(Person male, Person female)
        {

        }

        public static void VedhaKuta(Person male, Person female)
        {
            Console.WriteLine("Vedha : pairs of constellations affect each other");

            //The following pairs of constellations affect each
            // other and, therefore, no marriage should be brought about between a
            // boy and girl whos Janma Nakshatras belong to the same pair unless the
            // are other relieving factors.

            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime()).GetConstellationName();
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime()).GetConstellationName();

            //check if any paris from both sides
            var maleFemale = PairFound(maleConstellation, femaleConstellation);
            var femaleMale = PairFound(femaleConstellation, maleConstellation);

            //if any pair found, end as bad
            if (maleFemale || femaleMale)
            {
                Console.WriteLine("Bad : Pair found");
            }
            else
            {
                Console.WriteLine("Good : No pairs");
            }



            bool PairFound(ConstellationName a, ConstellationName b)
            {
                //Aswini and Jyeshta;
                if (a == ConstellationName.Aswini && b == ConstellationName.Jyesta) { return true; }

                //Bharani and Anuradha;
                if (a == ConstellationName.Bharani && b == ConstellationName.Anuradha) { return true; }

                //Krittika and Visakha;
                if (a == ConstellationName.Krithika && b == ConstellationName.Vishhaka) { return true; }

                //Rohini and Swati;
                if (a == ConstellationName.Rohini && b == ConstellationName.Swathi) { return true; }

                //Aridra and Sravana 
                if (a == ConstellationName.Aridra && b == ConstellationName.Sravana) { return true; }

                //Punarvasu and Uttarashadha;
                if (a == ConstellationName.Punarvasu && b == ConstellationName.Uttarashada) { return true; }

                //Pusayami and Poorvashadha;
                if (a == ConstellationName.Pushyami && b == ConstellationName.Poorvashada) { return true; }

                //Aslesha and Moola;
                if (a == ConstellationName.Aslesha && b == ConstellationName.Moola) { return true; }

                //Makha and Revati;
                if (a == ConstellationName.Makha && b == ConstellationName.Revathi) { return true; }

                //Pubba and Uttarabhadra;
                if (a == ConstellationName.Pubba && b == ConstellationName.Uttarabhadra) { return true; }

                //Uttara and Poorvabhadra;
                if (a == ConstellationName.Uttara && b == ConstellationName.Poorvabhadra) { return true; }
                
                //Hasta and Satabhisha,
                if (a == ConstellationName.Hasta && b == ConstellationName.Satabhisha) { return true; }

                // Mrigasira and Dhanishta.
                if (a == ConstellationName.Mrigasira && b == ConstellationName.Dhanishta) { return true; }

                //when control is here, end as no pair found
                return false;
            }
        }


        public static void Rajju(Person male, Person female)
        {
            Console.WriteLine("Rajju : strength/duration of married life (special attention)");

            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime()).GetConstellationName();
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime()).GetConstellationName();


            //get group names
            var maleGroupName = GetGroupName(maleConstellation);
            var femaleGroupName = GetGroupName(femaleConstellation);

            //if group name matched
            if (maleGroupName == femaleGroupName && maleGroupName == "sira")
            {
                Console.WriteLine("Sira (head) husband's death is likely");
            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "kanta")
            {
                Console.WriteLine("Kantha (neck) the wife may die");
            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "udara")
            {
                Console.WriteLine("Udara (stomach) the children may die;");
            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "kati")
            {
                Console.WriteLine("Kati (waist) poverty may ensue;");
            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "pada")
            {
                Console.WriteLine("Pada (foot) the couple may be always wandering");
            }
            else
            {
                Console.WriteLine("Good");
            }


            string GetGroupName(ConstellationName name)
            {


                //Padarajju. - Aswini, Aslesha, Makha, Jyeshta. Moola. Revati.
                var pada = new List<ConstellationName>()
                {
                    ConstellationName.Aswini, ConstellationName.Aslesha, ConstellationName.Makha,
                    ConstellationName.Jyesta, ConstellationName.Moola, ConstellationName.Revathi
                };

                var found = pada.FindAll(constellation => constellation == name).Any();
                if (found) { return "pada"; }


                //Katirajju. - Bharani, Pushyami, Pubba, Anuradha, Uttarabhadra. Poorvashadha,
                var kati = new List<ConstellationName>()
                {
                    ConstellationName.Bharani, ConstellationName.Pushyami, ConstellationName.Pubba,
                    ConstellationName.Anuradha, ConstellationName.Uttarabhadra, ConstellationName.Poorvashada
                };

                found = kati.FindAll(constellation => constellation == name).Any();
                if (found) { return "kati"; }


                //Nabhi or Udararajju. - Krittika, Punarvasu, Uttara, Visakha,
                // Uttarashadha, Poorvabhadra.
                var udara = new List<ConstellationName>()
                {
                    ConstellationName.Krithika, ConstellationName.Punarvasu, ConstellationName.Uttara,
                    ConstellationName.Vishhaka, ConstellationName.Uttarashada, ConstellationName.Poorvabhadra
                };
                found = udara.FindAll(constellation => constellation == name).Any();
                if (found) { return "udara"; }


                //Kantarajju. - Rohini, Aridra Hasta. Swati. Sravana, and Satabhisha.
                var kanta = new List<ConstellationName>()
                {
                    ConstellationName.Rohini, ConstellationName.Aridra, ConstellationName.Hasta,
                    ConstellationName.Swathi, ConstellationName.Sravana, ConstellationName.Satabhisha
                };
                found = kanta.FindAll(constellation => constellation == name).Any();
                if (found) { return "kanta"; }


                //Sirorajju. - Dhanishta, Chitta and Mrigasira.
                var sira = new List<ConstellationName>()
                {
                    ConstellationName.Dhanishta, ConstellationName.Chitta, ConstellationName.Mrigasira
                };
                found = sira.FindAll(constellation => constellation == name).Any();
                if (found) { return "sira"; }

                return "";
            }


        }
        
        public static void VasyaKuta(Person male, Person female)
        {
            Console.WriteLine("VasyaKuta:degree of magnetic control");

            //Vasya Kuta. - This is important as suggesting the degree of magnetic
            // control or amenability the wife or husband would be able to exercise on
            // the other.
            // For Aries - Leo and Scorpio are amenable.
            // For Taurus - Cancer and Libra;
            // for Gemini - Virgo;
            // for Cancer - Scorpo and Sagittarius;
            // for Leo - Libra;
            // for Virgo - Pisces and Gemini;
            // for Libra - Capricorn and Virgo;
            // for Scorpio - Cancer;
            // for Sagittarius - Pisces;
            // for Capricorn - Aries and Aquarius;
            // for Aquarius - Aries;
            // for Pisces - Capricorn.
            // The unit of agreement is 2.


            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();



            var mainSign = maleRuleSign;
            var subSign = femaleRuleSign;
            var mainControlSub = false;
            var femaleControlMale = false;
            var maleControlFemale = false;
            var count = 0;


        CheckAgain:
            //if count above 1, switch the signs
            if (count == 1)
            {
                mainSign = femaleRuleSign;
                subSign = maleRuleSign;
            }


            if (mainSign == ZodiacName.Aries)
            {
                if (subSign is ZodiacName.Leo or ZodiacName.Scorpio)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Taurus)
            {
                if (subSign is ZodiacName.Cancer or ZodiacName.Libra)
                {
                    mainControlSub = true;
                }
            }

            if (mainSign == ZodiacName.Gemini)
            {
                if (subSign is ZodiacName.Virgo)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Cancer)
            {
                if (subSign is ZodiacName.Scorpio or ZodiacName.Sagittarius)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Leo)
            {
                if (subSign is ZodiacName.Libra)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Virgo)
            {
                if (subSign is ZodiacName.Pisces or ZodiacName.Gemini)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Libra)
            {
                if (subSign is ZodiacName.Capricornus or ZodiacName.Virgo)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Scorpio)
            {
                if (subSign is ZodiacName.Cancer)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Sagittarius)
            {
                if (subSign is ZodiacName.Pisces)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Capricornus)
            {
                if (subSign is ZodiacName.Aries or ZodiacName.Aquarius)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Aquarius)
            {
                if (subSign is ZodiacName.Aries)
                {
                    mainControlSub = true;
                }
            }
            if (mainSign == ZodiacName.Pisces)
            {
                if (subSign is ZodiacName.Capricornus)
                {
                    mainControlSub = true;
                }
            }


            if (count == 0)
            {
                //transfer the result
                maleControlFemale = mainControlSub;
                //incriment count 
                count++;
                //send control back up
                goto CheckAgain;
            }

            if (count == 1)
            {
                femaleControlMale = mainControlSub;
            }





            //PRINTING

            if (maleControlFemale)
            {
                Console.WriteLine("Male control Female");
            }
            else if (femaleControlMale)
            {
                Console.WriteLine("Female control Male");
            }
            else
            {
                Console.WriteLine("Neither controls the other");
            }


        }

        public static void GrahaMaitram(Person male, Person female)
        {

            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //get lords of sign
            var maleLord = AstronomicalCalculator.GetLordOfZodiacSign(maleRuleSign);
            var femaleLord = AstronomicalCalculator.GetLordOfZodiacSign(femaleRuleSign);

            //get relationship of planets
            var relation = AstronomicalCalculator.GetPlanetPermanentRelationshipWithPlanet(maleLord, femaleLord);

            if (relation is PlanetToPlanetRelationship.AdhiMitra or PlanetToPlanetRelationship.Mitra)
            {
                Console.WriteLine("Graha Maitram : Good");
            }
            else
            {
                Console.WriteLine("Graha Maitram : Bad");
            }

        }
        
        public static void RasiKuta(Person male, Person female)
        {

            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //count from female to male
            var femaleToMale = AstronomicalCalculator.CountFromSignToSign(femaleRuleSign, maleRuleSign);
            var maleToFemale = AstronomicalCalculator.CountFromSignToSign(maleRuleSign, femaleRuleSign);

            //lf the Rasi of the boy happens to be the 2nd from that of
            // the girl and if the Rasi of the girl happens to be the 12th from that of the
            // boy, evil results will follow.
            if (femaleToMale == 2 || maleToFemale == 12)
            {
                Console.WriteLine("RasiKuta: Evil results will follow"); return;
            }

            //But if, on the other hand, the Rasi of the boy
            // falls in the 12th from the girl's or the Rasi of the girl is in the 2nd from
            // that of the boy astrology predicts longevity for the couple.
            if (femaleToMale == 12 || maleToFemale == 2)
            {
                Console.WriteLine("RasiKuta: Longevity for the couple"); return;
            }

            //If the Rasi of
            // the boy is the 3rd from that of the girl. there will be misery and sorrow.
            if (femaleToMale == 3)
            {
                Console.WriteLine("RasiKuta: Misery and sorrow"); return;
            }

            //But if the Rasi of the girl is the 3rd from that of the boy, there will be
            // happiness.
            if (maleToFemale == 3)
            {
                Console.WriteLine("RasiKuta: Happiness"); return;
            }

            //If the boy's falls in the 4th from that of the girl's, then there
            // will be great poverty;
            if (femaleToMale == 4)
            {
                Console.WriteLine("RasiKuta: Great poverty"); return;
            }

            //Rasi of the girl happens to fall in the 4th
            //from the boy's there will be great wealth.
            if (maleToFemale == 4)
            {
                Console.WriteLine("RasiKuta: Great wealth"); return;
            }

            //If the boy's Rasi falls in the 5th
            // from that of the girl, there will be unhappiness.
            if (femaleToMale == 5)
            {
                Console.WriteLine("RasiKuta: unhappiness"); return;
            }

            //But if the girl's Rasi falls
            // in the 5th from that of the boy,
            // there will be enjoyment and prosperity.
            if (maleToFemale == 5)
            {
                Console.WriteLine("RasiKuta: enjoyment and prosperity"); return;
            }

            //But where the Rasis of the boy and the girl are in the 7th houses
            // mutually, then there will be health, agreement and happiness.
            if (maleToFemale == 7 && femaleToMale == 7)
            {
                Console.WriteLine("RasiKuta: health, agreement and happiness"); return;
            }

            //If the
            // boy's Rasi falls in the 6th from the girl's there will be loss of children,
            if (femaleToMale == 6)
            {
                Console.WriteLine("RasiKuta: loss of children"); return;
            }

            //if the girl's is the 6th from the boy's, then the progeny will prosper.
            if (maleToFemale == 6)
            {
                Console.WriteLine("RasiKuta: progeny will prosper"); return;
            }



        }

        public static void StreeDeergha(Person male, Person female)
        {
            Console.WriteLine("StreeDeergha : husband well being, longevity and prosperity.");

            //Stree-Deergha. - The boy's constellation should preferably be beyond
            // the 9th from that of the girl. According to some authorities the distance
            // should be more than 7 constellations.
            //get ruling sign
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());

            var count = AstronomicalCalculator.CountFromConstellationToConstellation(femaleConstellation, maleConstellation);

            if (count >= 9)
            {
                Console.WriteLine("Good");
            }
            else
            {
                Console.WriteLine("Bad");
            }

        }

        public static void DinaKuta(Person male, Person female)
        {

            //Dina Kuta. - Count the constellation of the boy from that of the girl
            // and divide the number by 9. If the remainder is 2, 4, 6, 8 or 0 it is good.
            // The number of units of compatibility assigned to this Kuta is 3 in case
            // agreement is found.

            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //count from female to male
            var count = AstronomicalCalculator.CountFromSignToSign(femaleRuleSign, maleRuleSign);

            //divide by 9 and get the remainder, done via modulus
            var remainder = count % 9;

            // If the remainder is 2, 4, 6, 8 or 0 it is good.
            if (remainder == 2 || remainder == 4 || remainder == 6 || remainder == 8 || remainder == 0)
            {
                Console.WriteLine("Dina Kuta : Good");
            }
            else
            {
                Console.WriteLine("Dina Kuta : Bad");
            }

        }

    }

}


