

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

            //filter out the male and female ones we want
            var maleName = "Rubeshen";
            var femaleName = "Dhiviya";
            var male = peopleList.Find(person => person.GetName() == maleName);
            var female = peopleList.Find(person => person.GetName() == femaleName);

            //var geoLocation = new GeoLocation("Ipoh", 101, 4.59); //todo check if change in location changes much

            //var stdTimeMale = DateTimeOffset.ParseExact("23:33 19/03/1989 +08:00", Time.GetDateTimeFormat(), null);
            //var stdTimeFemale = DateTimeOffset.ParseExact("10:27 14/02/1995 +08:00", Time.GetDateTimeFormat(), null);

            //var male = new Person("Male", new Time(stdTimeMale, geoLocation));
            //var female = new Person("Female", new Time(stdTimeFemale, geoLocation));


            PrintOneVsOne(male, female);
            //PrintOneVsList(female);

        }


        private static void PrintOneVsOne(Person male, Person female)
        {
            var report = GetCompatibilityReport(male, female);

            var maleName = male.GetName();
            var femaleName = female.GetName();

            printResult(ref report);



            //FUNCTIONS
            void printResult(ref CompatibilityReport report)
            {
                var list = report.PredictionList;

                //print header
                var maleYear = male.GetBirthDateTime().GetStdDateTimeOffset().Year;
                var femaleYear = female.GetBirthDateTime().GetStdDateTimeOffset().Year;
                Console.WriteLine($"{maleName}-{maleYear} <> {femaleName}-{femaleYear}");
                Console.WriteLine("Name#Nature#Description#Extra Info#Male#Female#");

                //print rows
                foreach (var prediction in list)
                {
                    //if prediction is empty, than skip it
                    if (prediction.Name == Name.Empty) { continue; }
                    Console.WriteLine($"{prediction.Name}#{prediction.Nature}#{prediction.Description}#{prediction.Info}#{prediction.MaleInfo}#{prediction.FemaleInfo}");
                }

                Console.WriteLine($"Total Score#{getScoreGrade(report.KutaScore)}#Total score must be above 50%#Score: {report.KutaScore}/100##");

                Console.ReadLine();
            }

        }


        private static void PrintOneVsList(Person person)
        {

            //get all the people
            var peopleList = MuhurthaCore.GetAllPeopleList();

            //given a list of people find good matches
            //var goodMatches = FindGoodMatches(peopleList);
            var goodMatches = GetAllMatchesForPersonByStrength(person, peopleList);

            //show final results to user
            printResultList(ref goodMatches);

            void printResultList(ref List<CompatibilityReport> reportList)
            {
                foreach (var report in reportList)
                {
                    Console.WriteLine($"{report.Male.GetName()}\t{report.Female.GetName()}\t{report.KutaScore}");
                }

                Console.ReadLine();
            }


        }

        private static EventNature getScoreGrade(double score)
        {
            if (score > 50)
            {
                return EventNature.Good;
            }
            else
            {
                return EventNature.Bad;
            }

        }


        private static List<CompatibilityReport> GetAllMatchesForPersonByStrength(Person inputPerson, List<Person> personList)
        {

            var returnList = new List<CompatibilityReport>();


            //this makes sure each person is cross checked against this person correctly
            foreach (var personMatch in personList)
            {
                //get needed details
                var inputPersonIsMale = inputPerson.GetGender() == Gender.Male;
                var inputPersonIsFemale = inputPerson.GetGender() == Gender.Female;
                var personMatchIsMale = personMatch.GetGender() == Gender.Male;
                var personMatchIsFemale = personMatch.GetGender() == Gender.Female;

                if (inputPersonIsMale && personMatchIsFemale)
                {
                    //add report to list
                    var report = GetCompatibilityReport(inputPerson, personMatch);
                    returnList.Add(report);
                }

                if (inputPersonIsFemale && personMatchIsMale)
                {
                    //add report to list
                    var report = GetCompatibilityReport(personMatch, inputPerson);
                    returnList.Add(report);
                }


            }


            //order the list by strength, highest at 0 index
            var SortedList = returnList.OrderBy(o => o.KutaScore).ToList();

            return SortedList;

        }

        /// <summary>
        /// Finds good matches from a list of people who meet the criteria
        /// </summary>
        private static List<CompatibilityReport> FindGoodMatches(List<Person> peopleList)
        {
            //from a list of people find good matches

            //split the sexes
            var femaleList = peopleList.FindAll(person => person.GetGender() == Gender.Female);
            var maleList = peopleList.FindAll(person => person.GetGender() == Gender.Male);

            var goodReports = new List<CompatibilityReport>();

            //cross reference male & female list
            foreach (var female in femaleList)
            {
                foreach (var male in maleList)
                {
                    var report = GetCompatibilityReport(male, female);
                    //if report meets criteria save it
                    if (report.KutaScore > 50)
                    {
                        goodReports.Add(report);
                    }
                }
            }

            //return reports that got saved
            return goodReports;
        }

        /// <summary>
        /// Gets the compatibility report for a male & female
        /// The place where compatibility report gets generated
        /// </summary>
        private static CompatibilityReport GetCompatibilityReport(Person male, Person female)
        {

            var report = new CompatibilityReport
            {
                Male = male,
                Female = female,
                //do the calculations & add results to a list
                PredictionList = new List<Prediction>(){
                    GrahaMaitram(male, female), //5
                    Rajju(male, female),
                    NadiKuta(male, female), //8
                    VasyaKuta(male, female), //2
                    DinaKuta(male, female), //3
                    GunaKuta(male, female),//6
                    Mahendra(male, female),
                    StreeDeergha(male, female),
                    RasiKuta(male, female),//7
                    VedhaKuta(male, female),
                    Varna(male, female), //1
                    YoniKuta(male, female),//4
                    LagnaAndHouse7Good(male, female),
                    KujaDosa(male, female),
                    BadConstellations(male, female)

                }
            };

            //count the total points
            calculateTotalPoints(ref report);

            //check results for exceptions
            handleExceptions(ref report);

            return report;
            
            //FUNCTIONS

            //checks & modifies results for exceptions 
            void handleExceptions(ref CompatibilityReport report)
            {

                var list = report.PredictionList;

                //each exception below modifies the list if needed

                streeDeerghaException(list);

                rajjuException(list);

                nadiKutaException(list);



                //FUNCTIONS

                void streeDeerghaException(List<Prediction> list)
                {
                    //1.The absence of Stree-Deerga may be ignored if
                    //  Rasi Kuta add Graha Maitri are present.

                    //get the needed prediction
                    var streeDeerga = list.Find(pr => pr.Name == Name.StreeDeergha);
                    var rasiKuta = list.Find(pr => pr.Name == Name.RasiKuta);
                    var grahaMaitram = list.Find(pr => pr.Name == Name.GrahaMaitram);

                    //if prediction is bad and exception can be applied
                    var streeDeergaIsBad = streeDeerga.Nature == EventNature.Bad;
                    var rasiKutaIsGood = rasiKuta.Nature == EventNature.Good;
                    var grahaMaitramIsGood = grahaMaitram.Nature == EventNature.Good;

                    if (streeDeergaIsBad && rasiKutaIsGood && grahaMaitramIsGood)
                    {
                        //create new prediction
                        var newPrediction = new Prediction()
                        {
                            Name = streeDeerga.Name,
                            Description = streeDeerga.Description,
                            FemaleInfo = streeDeerga.FemaleInfo,
                            MaleInfo = streeDeerga.MaleInfo,
                            Info = "bad Stree-Deerga is ignored, due to good Rasi Kuta and Graha Maitri",
                            Nature = EventNature.Neutral
                        };

                        //replace old prediction with new one
                        list.Remove(streeDeerga);
                        list.Add(newPrediction);

                    }

                }

                void rajjuException(List<Prediction> list)
                {
                    //2.Rajju Kuta need not be considered in case Graha Maitri, Rasi, Dina
                    //  and Mahendra Kuta are present.

                    //get the needed prediction
                    var rajju = list.Find(pr => pr.Name == Name.Rajju);
                    var grahaMaitram = list.Find(pr => pr.Name == Name.GrahaMaitram);
                    var rasiKuta = list.Find(pr => pr.Name == Name.RasiKuta);
                    var dinaKuta = list.Find(pr => pr.Name == Name.DinaKuta);
                    var mahendra = list.Find(pr => pr.Name == Name.Mahendra);


                    //if prediction is bad and exception can be applied
                    var rajjuIsBad = rajju.Nature == EventNature.Bad;
                    var grahaMaitramIsGood = grahaMaitram.Nature == EventNature.Good;
                    var rasiKutaIsGood = rasiKuta.Nature == EventNature.Good;
                    var dinaKutaIsGood = dinaKuta.Nature == EventNature.Good;
                    var mahendraIsGood = mahendra.Nature == EventNature.Good;


                    if (rajjuIsBad && grahaMaitramIsGood && rasiKutaIsGood && dinaKutaIsGood && mahendraIsGood)
                    {
                        //create new prediction
                        var newPrediction = new Prediction()
                        {
                            Name = rajju.Name,
                            Description = rajju.Description,
                            FemaleInfo = rajju.FemaleInfo,
                            MaleInfo = rajju.MaleInfo,
                            Info = "bad Rajju Kuta is ignored, due to good Graha Maitri, Rasi, Dina and Mahendra",
                            Nature = EventNature.Neutral
                        };

                        //replace old prediction with new one
                        list.Remove(rajju);
                        list.Add(newPrediction);

                    }

                }

                void nadiKutaException(List<Prediction> list)
                {
                    //The evil due to Nadi Kuta can be ignored subject to the following
                    // conditions: -
                    // (a) The Rasi and Rajju Kuta prevail

                    //get the needed prediction
                    var nadiKuta = list.Find(pr => pr.Name == Name.NadiKuta);
                    var rasiKuta = list.Find(pr => pr.Name == Name.RasiKuta);
                    var rajju = list.Find(pr => pr.Name == Name.Rajju);

                    //if prediction is bad and exception can be applied
                    var nadiKutaIsBad = nadiKuta.Nature == EventNature.Bad;
                    var rasiKutaIsGood = rasiKuta.Nature == EventNature.Good;
                    var rajjuIsGood = rajju.Nature == EventNature.Good;

                    if (nadiKutaIsBad && rasiKutaIsGood && rajjuIsGood)
                    {
                        //create new prediction
                        var newPrediction = new Prediction()
                        {
                            Name = nadiKuta.Name,
                            Description = nadiKuta.Description,
                            FemaleInfo = nadiKuta.FemaleInfo,
                            MaleInfo = nadiKuta.MaleInfo,
                            Info = "bad Nadi Kuta is ignored, due to good Rasi Kuta and Rajju",
                            Nature = EventNature.Neutral
                        };

                        //replace old prediction with new one
                        list.Remove(nadiKuta);
                        list.Add(newPrediction);

                    }

                }

            }

            void calculateTotalPoints(ref CompatibilityReport report)
            {

                //count points total 36 points
                foreach (var prediction in report.PredictionList)
                {
                    //only count if prediction is good
                    if (prediction.Nature != EventNature.Good) { continue; }

                    //based on prediction name add together the score
                    switch (prediction.Name)
                    {
                        case Name.Mahendra:
                            break;
                        case Name.NadiKuta:
                            report.KutaScore += 8;
                            break;
                        case Name.GunaKuta:
                            report.KutaScore += 6;
                            break;
                        case Name.Varna:
                            report.KutaScore += 1;
                            break;
                        case Name.Yoni:
                            report.KutaScore += 4;
                            break;
                        case Name.Vedha:
                            break;
                        case Name.VasyaKuta:
                            report.KutaScore += 2;
                            break;
                        case Name.GrahaMaitram:
                            report.KutaScore += 5;
                            break;
                        case Name.RasiKuta:
                            report.KutaScore += 7;
                            break;
                        case Name.StreeDeergha:
                            break;
                        case Name.DinaKuta:
                            report.KutaScore += 3;
                            break;
                        case Name.KujaDosa:
                            break;
                        case Name.Rajju:
                            break;
                        case Name.LagnaAnd7thGood:
                            break;
                        case Name.BadConstellation:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                //convert score to percentage of 36
                report.KutaScore = Math.Round((report.KutaScore / 36) * 100);

            }

        }


        public static Prediction Mahendra(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.Mahendra,
                Description = "well-being and longevity"
            };


            //Mahendra. - The constellation of the boy counted from that of the girl
            // should be the 4th, 7th, 10th, 13th, 16th, 19th, 22nd or 25th. This
            // promotes well-being and increases longevity.

            //get ruling sign
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());

            //show extra info
            prediction.MaleInfo = maleConstellation.ToString();
            prediction.FemaleInfo = femaleConstellation.ToString();

            var count = AstronomicalCalculator.CountFromConstellationToConstellation(femaleConstellation, maleConstellation);

            if (count == 4 || count == 7 || count == 10 | count == 13 || count == 16 || count == 19 || count == 22 || count == 25)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = $"promotes well-being and increases longevity, count {count}";
            }
            else
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = $"no well-being and no longevity, count {count}";
            }

            return prediction;
        }

        public static Prediction NadiKuta(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.NadiKuta,
                Description = "nervous energy compatibility (important)"
            };


            //get constellation
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());

            //get nadi
            var maleNadi = getNadi(maleConstellation.GetConstellationName());
            var femaleNadi = getNadi(femaleConstellation.GetConstellationName());

            //show user
            prediction.MaleInfo = maleNadi;
            prediction.FemaleInfo = femaleNadi;

            // A boy with a predominantly
            // windy or phlegmatic or bilious constitution
            // should not many a girl of the same type. The
            // girl should belong to a different’ temperament.
            if (maleNadi == femaleNadi)
            {
                //The evil due to Nadi Kuta can be ignored subject to the following conditions:

                //a.The same planet is lord of the Janma Rasis of both the male and the female,
                var maleJanmaLord = AstronomicalCalculator.GetLordOfZodiacSign(AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName());
                var femaleJanmaLord = AstronomicalCalculator.GetLordOfZodiacSign(AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName());
                var sameJanmaLord = maleJanmaLord == femaleJanmaLord;

                //b.The lords of the Janma Rasi of the couple are friends.
                var relationship = AstronomicalCalculator.GetPlanetPermanentRelationshipWithPlanet(maleJanmaLord, femaleJanmaLord);
                var janmaIsFriend = relationship is PlanetToPlanetRelationship.AdhiMitra or PlanetToPlanetRelationship.Mitra;

                //if any above exceptions met, change prediction
                if (sameJanmaLord || janmaIsFriend)
                {
                    prediction.Nature = EventNature.Neutral;
                    prediction.Info = "bad, but exception by Janma Rasi lord same/friend";
                }
                else
                {
                    prediction.Nature = EventNature.Bad;
                    prediction.Info = "same type, should belong to different";
                }

            }
            else
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "agreement between the couple will be good";
            }

            return prediction;

            //gets the nadi connected to the birth constellation
            string getNadi(ConstellationName constellation)
            {
                switch (constellation)
                {
                    case ConstellationName.Aswini:
                    case ConstellationName.Aridra:
                    case ConstellationName.Punarvasu:
                    case ConstellationName.Uttara:
                    case ConstellationName.Hasta:
                    case ConstellationName.Jyesta:
                    case ConstellationName.Moola:
                    case ConstellationName.Satabhisha:
                    case ConstellationName.Poorvabhadra:
                        return "Vatha";

                    case ConstellationName.Bharani:
                    case ConstellationName.Mrigasira:
                    case ConstellationName.Pushyami:
                    case ConstellationName.Pubba:
                    case ConstellationName.Chitta:
                    case ConstellationName.Anuradha:
                    case ConstellationName.Poorvashada:
                    case ConstellationName.Dhanishta:
                    case ConstellationName.Uttarabhadra:
                        return "Pittha";

                    case ConstellationName.Krithika:
                    case ConstellationName.Rohini:
                    case ConstellationName.Aslesha:
                    case ConstellationName.Makha:
                    case ConstellationName.Swathi:
                    case ConstellationName.Vishhaka:
                    case ConstellationName.Uttarashada:
                    case ConstellationName.Sravana:
                    case ConstellationName.Revathi:
                        return "Sleshma";


                    default:
                        throw new ArgumentOutOfRangeException(nameof(constellation), constellation, null);
                }
            }
        }

        public static Prediction GunaKuta(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.GunaKuta,
                Description = "temperament and character compatibility"
            };


            //get constellation
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());

            //get guna
            var maleGuna = getGuna(maleConstellation.GetConstellationName());
            var femaleGuna = getGuna(femaleConstellation.GetConstellationName());

            //show user
            prediction.MaleInfo = maleGuna.ToString();
            prediction.FemaleInfo = femaleGuna.ToString();

            //rename for readability
            var manIsManushaDeva = maleGuna is Guna.Deva or Guna.Manusha;
            var girlIsRakshasa = femaleGuna == Guna.Rakshasa;
            var femaleIsManushaDeva = femaleGuna is Guna.Deva or Guna.Manusha;
            var maleIsRakshasa = maleGuna == Guna.Rakshasa;


            //Hence one born in a Deva
            // constellation is not able to get on well with a person born in Rakshasa
            // constellation. A Deva can marry a Deva, a Manusha can marry a
            // Manusha and a Rakshasa can marry a Rakshasa.
            if (maleGuna == femaleGuna)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "same Guna";
            }

            // Manusha or a Deva man should not marry a Rakshasa girl unless there
            // are other neutralising factors.
            else if (manIsManushaDeva && girlIsRakshasa)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Manusha/Deva man not marry a Rakshasa girl unless there are other neutralizing factors.";
            }

            // But marriage between a Rakshasa man
            // and a Deva or Manusha girl is passable.
            else if (femaleIsManushaDeva && maleIsRakshasa)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "marriage between a Rakshasa man and a Deva/Manusha girl is passable.";
            }

            // If marriage is brought about
            // between prohibited Ganas there will be quarrels and disharmony
            else
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "quarrels and disharmony";
            }


            //EXCEPTION

            //If the asterism of the bride is beyond the 14th from that of the bridegroom the evil may be ignored.
            var maleToFemale = AstronomicalCalculator.CountFromConstellationToConstellation(maleConstellation, femaleConstellation);
            //only show user the exception when prediction thus is bad
            if (maleToFemale > 14 && prediction.Nature == EventNature.Bad)
            {
                //create new prediction
                var newPrediction = new Prediction()
                {
                    Name = prediction.Name,
                    Description = prediction.Description,
                    FemaleInfo = prediction.FemaleInfo,
                    MaleInfo = prediction.MaleInfo,
                    Info = "female star is more than 14th from male's, evil may be ignored",
                    Nature = EventNature.Neutral
                };

                //replace old prediction
                prediction = newPrediction;
            }

            return prediction;

            Guna getGuna(ConstellationName constellation)
            {
                switch (constellation)
                {
                    // Deva Gana. - Punarvasu, Pushyami, Swati, Hasta, Sravana, Revati,
                    // Anuradha, Mrigasira and Aswini.

                    case ConstellationName.Punarvasu:
                    case ConstellationName.Pushyami:
                    case ConstellationName.Swathi:
                    case ConstellationName.Hasta:
                    case ConstellationName.Sravana:
                    case ConstellationName.Revathi:
                    case ConstellationName.Anuradha:
                    case ConstellationName.Mrigasira:
                    case ConstellationName.Aswini:
                        return Guna.Deva;

                    // Manusha Gana. - Rohini, Pubba, Poorvashadha, Poorvabhadra,
                    // Bharani, Aridra, Uttara, Uttarashadha and Uttarabhadra.
                    case ConstellationName.Rohini:
                    case ConstellationName.Pubba:
                    case ConstellationName.Poorvashada:
                    case ConstellationName.Poorvabhadra:
                    case ConstellationName.Bharani:
                    case ConstellationName.Aridra:
                    case ConstellationName.Uttara:
                    case ConstellationName.Uttarashada:
                    case ConstellationName.Uttarabhadra:
                        return Guna.Manusha;

                    // Rakshasa Gana. - Krittika, Aslesha, Makha, Chitta, Visakha, Jyeshta,
                    // Moola, Dhanishta and Satabhisha.
                    case ConstellationName.Krithika:
                    case ConstellationName.Aslesha:
                    case ConstellationName.Makha:
                    case ConstellationName.Chitta:
                    case ConstellationName.Vishhaka:
                    case ConstellationName.Jyesta:
                    case ConstellationName.Moola:
                    case ConstellationName.Dhanishta:
                    case ConstellationName.Satabhisha:
                        return Guna.Rakshasa;


                    default: throw new Exception("");
                }
            }

        }

        public static Prediction Varna(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.Varna,
                Description = "spiritual/ego compatibility"
            };

            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //get grade
            var maleGrade = getGrade(maleRuleSign);
            var femaleGrade = getGrade(femaleRuleSign);

            //copy info into prediction data
            prediction.MaleInfo = getGradeName(maleGrade);
            prediction.FemaleInfo = getGradeName(femaleGrade);


            //A girl belonging fo a higher grade of
            // spiritual development should not be mated to a boy of lesser
            // development. The vice verse or both belonging to the same grade or
            // degree is allowed.

            if (femaleGrade > maleGrade)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "girl higher Varna not good match to boy lower Varna";
            }

            if ((maleGrade > femaleGrade) || (maleGrade == femaleGrade))
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "boy higher and girl lower or both same Varna, is allowed";
            }

            return prediction;

            string getGradeName(int grade)
            {
                switch (grade)
                {
                    case 1:
                        return "Sudra";
                    case 2:
                        return "Vaisya";
                    case 3:
                        return "Kshatriya";
                    case 4:
                        return "Brahmin";
                    default: throw new Exception();
                }
            }

            //higher grade is higher class
            int getGrade(ZodiacName sign)
            {
                switch (sign)
                {   //Pisces, Scorpio and Cancer represent the highest development - Brahmin 
                    case ZodiacName.Pisces:
                    case ZodiacName.Scorpio:
                    case ZodiacName.Cancer:
                        return 4;

                    //Leo, Sagittarius and Libra indicate the second grade - or Kshatriya;
                    case ZodiacName.Leo:
                    case ZodiacName.Sagittarius:
                    case ZodiacName.Libra:
                        return 3;

                    //Aries, Gemini and Aquarius suggest the third or the Vaisya;
                    case ZodiacName.Aries:
                    case ZodiacName.Gemini:
                    case ZodiacName.Aquarius:
                        return 2;

                    //while Taurus, Virgo and Capricorn indicate the last grade, viz., Sudra
                    case ZodiacName.Taurus:
                    case ZodiacName.Virgo:
                    case ZodiacName.Capricornus:
                        return 1;

                    default: throw new Exception("");
                }
            }
        }

        public static Prediction YoniKuta(Person male, Person female)
        {

            var prediction = new Prediction
            {
                Name = Name.Yoni,
                Description = "sex compatibility"
            };

            //1. Get Details

            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime()).GetConstellationName();
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime()).GetConstellationName();


            //get group names
            dynamic maleGroupName = getAnimal(maleConstellation);
            dynamic femaleGroupName = getAnimal(femaleConstellation);

            Animal maleAnimal = maleGroupName.Animal;
            Animal femaleAnimal = femaleGroupName.Animal;
            string maleGender = maleGroupName.Gender;
            string femaleGender = femaleGroupName.Gender;

            //get grade representing compatibility
            var compatibleGrade = getAnimalCompatible(maleAnimal, femaleAnimal);

            //save details to show user
            prediction.MaleInfo = $"{maleAnimal} - {maleGender}";
            prediction.FemaleInfo = $"{femaleAnimal} - {femaleGender}";


            //2. Interpret details


            //The following pairs are hostile and in matching Yoni Kuta,
            // they should be avoided.Cow and tiger;
            // In a similar way, similar pairs of constellations typifying
            // other hostile pairs as they occur in nature should
            // be avoided.
            var isHostileYoni = compatibleGrade <= 2;
            if (isHostileYoni)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "pairs are hostile, should be avoided.";
            }
            //else it is a friendly (3) or perfect (4) pair
            else
            {
                //Marriage between the constellations indicating same class of yoni and between the male and
                //female stars of that yoni conduces to great happiness, perfect harmony and progeny.
                //The union of these is agreeable and conduces to favourable results to the fullest extent.
                if (maleAnimal == femaleAnimal && maleGender != femaleGender)
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "favourable results to the fullest extent, harmony and progeny";
                }


                // If the male and female happen to be born in friendly yonies, but both
                // representing female constellations there will be fair happiness and agreement.
                var isFriendlyYoni = compatibleGrade == 3;
                var bothFemale = femaleGender == "Female" && maleGender == "Female";
                if (isFriendlyYoni && bothFemale)
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "fair happiness and agreement";
                }

                //friendly yoni & opposite genders
                if (isFriendlyYoni && maleGender != femaleGender)
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "passable, not best but ok";

                }

                // If the couple belong both to male
                // constellations there will be constant quarrels
                // and unhappiness.
                var bothMale = maleGender == "Male" && femaleGender == "Male";
                if (bothMale)
                {
                    prediction.Nature = EventNature.Bad;
                    prediction.Info = "both male constellations, constant quarrels and unhappiness";
                }

            }

            return prediction;

            //FUNCTIONS

            //gets animal compatibility grade
            int getAnimalCompatible(Animal a, Animal b)
            {
                //          Ho El   Sh  Se  Do  Ca  Ra  Co  Bu  Ti  Ha  Mo  Mg  Li
                // Horse.   4   2   2   3   2   2   2   1   0   1   3   3   2   1
                // Elephant 2   4   3   3   2   2   2   2   3   1   2   3   2   0
                // Sheep    2   3   4   2   1   2   1   3   3   1   2   0   3   1
                // Serpent  3   3   2   4   2   1   1   1   1   2   2   2   0   2
                // Dog      2   2   1   2   4   2   1   2   2   1   0   2   1   1
                // Cat      2   2   2   1   2   4   0   2   2   1   3   3   2   1
                // Rat      2   2   1   1   1   0   4   2   2   2   2   2   1   2
                // Cow      1   2   3   1   2   2   2   4   3   0   3   2   2   1
                // Buffalo  0   3   3   1   2   2   2   3   4   1   2   2   2   1
                // Tiger    1   1   1   2   1   1   2   0   1   4   1   1   2   1
                // Hare     1   2   2   2   0   3   2   3   2   1   4   2   2   1
                // Monkey   3   3   0   2   2   3   2   2   2   1   2   4   3   2
                // Mongoose 2   2   3   0   1   2   1   2   2   2   2   3   4   2
                // Lion     1   0   1   2   1   1   2   1   2   1   1   2   2   4


                int[,] list = new int[15, 15]
                {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,4,2,2,3,2,2,2,1,0,1,3,3,2,1}, //Horse
                    {0,2,4,3,3,2,2,2,2,3,1,2,3,2,0}, //Elephant
                    {0,2,3,4,2,1,2,1,3,3,1,2,0,3,1}, //Sheep
                    {0,3,3,2,4,2,1,1,1,1,2,2,2,0,2}, //Serpent
                    {0,2,2,1,2,4,2,1,2,2,1,0,2,1,1}, //Dog
                    {0,2,2,2,1,2,4,0,2,2,1,3,3,2,1}, //Cat
                    {0,2,2,1,1,1,0,4,2,2,2,2,2,1,2}, //Rat
                    {0,1,2,3,1,2,2,2,4,3,0,3,2,2,1},//Cow
                    {0,0,3,3,1,2,2,2,3,4,1,2,2,2,1},//Buffalo
                    {0,1,1,1,2,1,1,2,0,1,4,1,1,2,1},//Tiger
                    {0,1,2,2,2,0,3,2,3,2,1,4,2,2,1},//Hare
                    {0,3,3,0,2,2,3,2,2,2,1,2,4,3,2},//Monkey
                    {0,2,2,3,0,1,2,1,2,2,2,2,3,4,2},//Mongoose
                    {0,1,0,1,2,1,1,2,1,2,1,1,2,2,4} //Lion
                };

                var animalGrade = list[(int)a, (int)b];

                return animalGrade;
            }

            //higher grade is higher class
            object getAnimal(ConstellationName sign)
            {
                switch (sign)
                {
                    case ConstellationName.Aswini:
                        return new { Gender = "Male", Animal = Animal.Horse };
                    case ConstellationName.Satabhisha:
                        return new { Gender = "Female", Animal = Animal.Horse };
                    case ConstellationName.Bharani:
                        return new { Gender = "Male", Animal = Animal.Elephant };
                    case ConstellationName.Revathi:
                        return new { Gender = "Female", Animal = Animal.Elephant };
                    case ConstellationName.Pushyami:
                        return new { Gender = "Male", Animal = Animal.Sheep };
                    case ConstellationName.Krithika:
                        return new { Gender = "Female", Animal = Animal.Sheep };
                    case ConstellationName.Rohini:
                        return new { Gender = "Male", Animal = Animal.Serpent };
                    case ConstellationName.Mrigasira:
                        return new { Gender = "Female", Animal = Animal.Serpent };
                    case ConstellationName.Moola:
                        return new { Gender = "Male", Animal = Animal.Dog };
                    case ConstellationName.Aridra:
                        return new { Gender = "Female", Animal = Animal.Dog };
                    case ConstellationName.Aslesha:
                        return new { Gender = "Male", Animal = Animal.Cat };
                    case ConstellationName.Punarvasu:
                        return new { Gender = "Female", Animal = Animal.Cat };
                    case ConstellationName.Makha:
                        return new { Gender = "Male", Animal = Animal.Rat };
                    case ConstellationName.Pubba:
                        return new { Gender = "Female", Animal = Animal.Rat };
                    case ConstellationName.Uttara:
                        return new { Gender = "Male", Animal = Animal.Cow };
                    case ConstellationName.Uttarabhadra:
                        return new { Gender = "Female", Animal = Animal.Cow };
                    case ConstellationName.Swathi:
                        return new { Gender = "Male", Animal = Animal.Buffalo };
                    case ConstellationName.Hasta:
                        return new { Gender = "Female", Animal = Animal.Buffalo };
                    case ConstellationName.Vishhaka:
                        return new { Gender = "Male", Animal = Animal.Tiger };
                    case ConstellationName.Chitta:
                        return new { Gender = "Female", Animal = Animal.Tiger };
                    case ConstellationName.Jyesta:
                        return new { Gender = "Male", Animal = Animal.Hare };
                    case ConstellationName.Anuradha:
                        return new { Gender = "Female", Animal = Animal.Hare };
                    case ConstellationName.Poorvashada:
                        return new { Gender = "Male", Animal = Animal.Monkey };
                    case ConstellationName.Sravana:
                        return new { Gender = "Female", Animal = Animal.Monkey };
                    case ConstellationName.Poorvabhadra:
                        return new { Gender = "Male", Animal = Animal.Lion };
                    case ConstellationName.Dhanishta:
                        return new { Gender = "Female", Animal = Animal.Lion };
                    case ConstellationName.Uttarashada:
                        return new { Gender = "Male", Animal = Animal.Mongoose };



                    default: throw new Exception("");
                }
            }
        }

        public static Prediction VedhaKuta(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.Vedha,
                Description = "birth constellations compatibility"
            };


            //The following pairs of constellations affect each
            // other and, therefore, no marriage should be brought about between a
            // boy and girl whos Janma Nakshatras belong to the same pair unless the
            // are other relieving factors.

            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime()).GetConstellationName();
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime()).GetConstellationName();

            //show user
            prediction.MaleInfo = maleConstellation.ToString();
            prediction.FemaleInfo = femaleConstellation.ToString();

            //check if any paris from both sides
            var maleFemale = PairFound(maleConstellation, femaleConstellation);
            var femaleMale = PairFound(femaleConstellation, maleConstellation);

            //if any pair found, end as bad
            if (maleFemale || femaleMale)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "hostile constellation pair found";
            }
            else
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "constellation pair not hostile";
            }

            return prediction;

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

        public static Prediction Rajju(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.Rajju,
                Description = "strength/duration of married life (important)"
            };


            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime()).GetConstellationName();
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime()).GetConstellationName();


            //get group names
            var maleGroupName = GetGroupName(maleConstellation);
            var femaleGroupName = GetGroupName(femaleConstellation);

            //add group name to view
            prediction.MaleInfo = maleGroupName;
            prediction.FemaleInfo = femaleGroupName;

            //if group name matched
            if (maleGroupName == femaleGroupName && maleGroupName == "Sira")
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Sira (head) husband's death is likely";

            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "Kanta")
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Kantha (neck) the wife may die";

            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "Udara")
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Udara (stomach) the children may die";
            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "Kati")
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Kati (waist) poverty may ensue";

            }

            else if (maleGroupName == femaleGroupName && maleGroupName == "Pada")
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Pada (foot) the couple may be always wandering";

            }
            else
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "male and female constellations in different groups";
            }

            //Rajju Kuta need not be considered in case Graha Maitri, Rasi, Dina and Mahendra Kutas are present.

            return prediction;

            string GetGroupName(ConstellationName name)
            {


                //Padarajju. - Aswini, Aslesha, Makha, Jyeshta. Moola. Revati.
                var pada = new List<ConstellationName>()
                {
                    ConstellationName.Aswini, ConstellationName.Aslesha, ConstellationName.Makha,
                    ConstellationName.Jyesta, ConstellationName.Moola, ConstellationName.Revathi
                };

                var found = pada.FindAll(constellation => constellation == name).Any();
                if (found) { return "Pada"; }


                //Katirajju. - Bharani, Pushyami, Pubba, Anuradha, Uttarabhadra. Poorvashadha,
                var kati = new List<ConstellationName>()
                {
                    ConstellationName.Bharani, ConstellationName.Pushyami, ConstellationName.Pubba,
                    ConstellationName.Anuradha, ConstellationName.Uttarabhadra, ConstellationName.Poorvashada
                };

                found = kati.FindAll(constellation => constellation == name).Any();
                if (found) { return "Kati"; }


                //Nabhi or Udararajju. - Krittika, Punarvasu, Uttara, Visakha,
                // Uttarashadha, Poorvabhadra.
                var udara = new List<ConstellationName>()
                {
                    ConstellationName.Krithika, ConstellationName.Punarvasu, ConstellationName.Uttara,
                    ConstellationName.Vishhaka, ConstellationName.Uttarashada, ConstellationName.Poorvabhadra
                };
                found = udara.FindAll(constellation => constellation == name).Any();
                if (found) { return "Udara"; }


                //Kantarajju. - Rohini, Aridra Hasta. Swati. Sravana, and Satabhisha.
                var kanta = new List<ConstellationName>()
                {
                    ConstellationName.Rohini, ConstellationName.Aridra, ConstellationName.Hasta,
                    ConstellationName.Swathi, ConstellationName.Sravana, ConstellationName.Satabhisha
                };
                found = kanta.FindAll(constellation => constellation == name).Any();
                if (found) { return "Kanta"; }


                //Sirorajju. - Dhanishta, Chitta and Mrigasira.
                var sira = new List<ConstellationName>()
                {
                    ConstellationName.Dhanishta, ConstellationName.Chitta, ConstellationName.Mrigasira
                };
                found = sira.FindAll(constellation => constellation == name).Any();
                if (found) { return "Sira"; }

                return "";
            }


        }

        public static Prediction VasyaKuta(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.VasyaKuta,
                Description = "degree of magnetic control"
            };


            //Vasya Kuta. - This is important as suggesting the degree of magnetic
            // control or amenability the wife or husband would be able to exercise on
            // the other.
            // For Aries - Leo and Scorpio are amenable.
            // For Taurus - Cancer and Libra;
            // for Gemini - Virgo;
            // for Cancer - Scorpio and Sagittarius;
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

            //show the names of the sign to user
            prediction.MaleInfo = maleRuleSign.ToString();
            prediction.FemaleInfo = femaleRuleSign.ToString();



            //variables for looping
            var mainSign = maleRuleSign;
            var subSign = femaleRuleSign;
            var mainControlSub = false;
            var femaleControlMale = false;
            var maleControlFemale = false;
            var count = 0;

        //the point of this is to calculate both male & female 

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
                //increment count 
                count++;
                //send control back up
                goto CheckAgain;
            }

            if (count == 1)
            {
                //transfer the result
                femaleControlMale = mainControlSub;
            }





            //PRINTING

            if (maleControlFemale)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "Male control Female";

            }
            else if (femaleControlMale)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "Female control Male";
            }
            else
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "neither controls the other";
            }

            return prediction;
        }

        public static Prediction GrahaMaitram(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.GrahaMaitram,
                Description = "happiness, mental compatibility (important)"
            };


            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //get lords of sign
            var maleLord = AstronomicalCalculator.GetLordOfZodiacSign(maleRuleSign);
            var femaleLord = AstronomicalCalculator.GetLordOfZodiacSign(femaleRuleSign);

            // get permanent relationship of planets
            // Ref : Some suggest that in considering the planetary
            // relations,the temporary dispositions should
            // also be taken into account. This in my humble
            // opinion is uncalled for, because, the entire
            // subject of adaptability hinges on the birth
            // constellations and not on birth charts as a whole.
            var maleToFemaleRelation = AstronomicalCalculator.GetPlanetPermanentRelationshipWithPlanet(maleLord, femaleLord);
            var femaleToMaleRelation = AstronomicalCalculator.GetPlanetPermanentRelationshipWithPlanet(femaleLord, maleLord);

            //show user
            prediction.MaleInfo = maleToFemaleRelation.ToString();
            prediction.FemaleInfo = femaleToMaleRelation.ToString();

            //rename relationship for readability
            var isMaleFriend = maleToFemaleRelation is PlanetToPlanetRelationship.AdhiMitra or PlanetToPlanetRelationship.Mitra;
            var isFemaleFriend = femaleToMaleRelation is PlanetToPlanetRelationship.AdhiMitra or PlanetToPlanetRelationship.Mitra;
            var isMaleEnemy = maleToFemaleRelation is PlanetToPlanetRelationship.AdhiSatru or PlanetToPlanetRelationship.Satru;
            var isFemaleEnemy = femaleToMaleRelation is PlanetToPlanetRelationship.AdhiSatru or PlanetToPlanetRelationship.Satru;
            var isMaleNeutral = maleToFemaleRelation is PlanetToPlanetRelationship.Sama;
            var isFemaleNeutral = femaleToMaleRelation is PlanetToPlanetRelationship.Sama;
            var maleOrFemaleFriend = isMaleFriend || isFemaleFriend;
            var maleOrFemaleNeutral = isMaleNeutral || isFemaleNeutral;


            //When the lords of the Janma Rasis of the
            // bride and bridegroom are friends, then Rasi
            // Kuta is said to obtain in full.
            if (isMaleFriend && isFemaleFriend)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "Rasi Kuta is said to obtain in full.";
            }

            // When one is a friend and the other a neutral, it is passable.
            else if (maleOrFemaleFriend && maleOrFemaleNeutral)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "one is friend and the other a neutral, it is passable";

            }

            // When both are neutral Rasi Kuta is very ordinary.
            else if (isMaleNeutral && isFemaleNeutral)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "both are neutral Rasi Kuta is very ordinary";
            }

            // When both are enemies, the alliance
            // must be avoided.
            else if (isMaleEnemy && isFemaleEnemy)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "both are enemies, must be avoided";
            }

            //combination not mentioned
            else
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "no good connection between male and female";
            }

            return prediction;
        }

        public static Prediction RasiKuta(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.RasiKuta,
                Description = "rasi compatibility"
            };


            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //count from female to male
            var femaleToMale = AstronomicalCalculator.CountFromSignToSign(femaleRuleSign, maleRuleSign);
            var maleToFemale = AstronomicalCalculator.CountFromSignToSign(maleRuleSign, femaleRuleSign);

            //show extra info to user
            prediction.MaleInfo = maleRuleSign.ToString();
            prediction.FemaleInfo = femaleRuleSign.ToString();

            //lf the Rasi of the boy happens to be the 2nd from that of
            // the girl and if the Rasi of the girl happens to be the 12th from that of the
            // boy, evil results will follow.
            if (femaleToMale == 2 || maleToFemale == 12)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "evil results will follow";
            }

            //But if, on the other hand, the Rasi of the boy
            // falls in the 12th from the girl's or the Rasi of the girl is in the 2nd from
            // that of the boy astrology predicts longevity for the couple.
            if (femaleToMale == 12 || maleToFemale == 2)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "longevity for the couple";
            }

            //If the Rasi of
            // the boy is the 3rd from that of the girl. there will be misery and sorrow.
            if (femaleToMale == 3)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "misery and sorrow";
            }

            //But if the Rasi of the girl is the 3rd from that of the boy, there will be
            // happiness.
            if (maleToFemale == 3)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "happiness";
            }

            //If the boy's falls in the 4th from that of the girl's, then there
            // will be great poverty;
            if (femaleToMale == 4)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "great poverty";
            }

            //Rasi of the girl happens to fall in the 4th
            //from the boy's there will be great wealth.
            if (maleToFemale == 4)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "great wealth";
            }

            //If the boy's Rasi falls in the 5th
            // from that of the girl, there will be unhappiness.
            if (femaleToMale == 5)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "unhappiness";
            }

            //But if the girl's Rasi falls
            // in the 5th from that of the boy,
            // there will be enjoyment and prosperity.
            if (maleToFemale == 5)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "enjoyment and prosperity";
            }

            //But where the Rasis of the boy and the girl are in the 7th houses
            // mutually, then there will be health, agreement and happiness.
            if (maleToFemale == 7 && femaleToMale == 7)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "health, agreement and happiness";

            }

            //If the boy's Rasi falls in the 6th from the girl's there will be loss of children,
            if (femaleToMale == 6)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "loss of children";
            }

            //if the girl's is the 6th from the boy's, then the progeny will prosper.
            if (maleToFemale == 6)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "progeny will prosper";
            }


            //Common Janma Rasi. - Views differ as regards the results accruing
            // from the Janma Rasis being common. According to Narada, common
            // Janma Rasi would be conducive to the couple provided they are born in
            // different constellations. Garga opines.that under the above
            // circumstance, the asterism of the boy should precede that of the girl if
            // the marriage is to prove happy. Incase the reverse holds good (Streepurva).
            // i. e., the constellation of the girl precedes that of the boy, the
            // alliance should be rejected.
            if (maleRuleSign == femaleRuleSign)
            {
                //get male & female constellation number
                var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime()).GetConstellationNumber();
                var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime()).GetConstellationNumber();
                
                //male constellation number should precede (lower number)
                if (maleConstellation < femaleConstellation)
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "male constellation precede female, marriage to prove happy";
                }

                if (femaleConstellation < maleConstellation)
                {
                    prediction.Nature = EventNature.Bad;
                    prediction.Info = "female constellation precede male, alliance should be rejected.";

                }

                if (femaleConstellation == maleConstellation)
                {
                    throw new NotImplementedException();
                }

            }


            //EXCEPTION

            //When both the Rasis are owned by one planet or if the
            //lords of the two Rasis happen to be friends, the evil attributed above to
            //the inauspicious disposition of Rasis gets cancelled.

            //only check if prediction thus is bad
            if (prediction.Nature == EventNature.Bad)
            {            
                //a.The same planet is lord of the Janma Rasis of both the male and the female,
                var maleJanmaLord = AstronomicalCalculator.GetLordOfZodiacSign(AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName());
                var femaleJanmaLord = AstronomicalCalculator.GetLordOfZodiacSign(AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName());
                var sameJanmaLord = maleJanmaLord == femaleJanmaLord;

                //b.The lords of the Janma Rasi of the couple are friends.
                var relationship = AstronomicalCalculator.GetPlanetPermanentRelationshipWithPlanet(maleJanmaLord, femaleJanmaLord);
                var janmaIsFriend = relationship is PlanetToPlanetRelationship.AdhiMitra or PlanetToPlanetRelationship.Mitra;

                //if any above exceptions met, change prediction
                if (sameJanmaLord || janmaIsFriend)
                {
                    //create new prediction
                    var newPrediction = new Prediction()
                    {
                        Name = prediction.Name,
                        Description = prediction.Description,
                        FemaleInfo = prediction.FemaleInfo,
                        MaleInfo = prediction.MaleInfo,
                        Info = "bad, but exception by Janma Rasi lord same/friend",
                        Nature = EventNature.Neutral
                    };

                    //replace old prediction
                    prediction = newPrediction;
                }


            }



            return prediction;

        }

        public static Prediction StreeDeergha(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.StreeDeergha,
                Description = "husband well being, longevity and prosperity"
            };

            //Stree-Deergha. - The boy's constellation should preferably be beyond
            // the 9th from that of the girl. According to some authorities the distance
            // should be more than 7 constellations.
            //get ruling sign
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());

            //show user
            prediction.MaleInfo = maleConstellation.ToString();
            prediction.FemaleInfo = femaleConstellation.ToString();


            var count = AstronomicalCalculator.CountFromConstellationToConstellation(femaleConstellation, maleConstellation);

            if (count >= 9)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = $"constellation count beyond 9, it is {count}";
            }
            else
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = $"constellation count lower than 9, it is {count}";

            }

            return prediction;
        }

        public static Prediction DinaKuta(Person male, Person female)
        {
            var prediction = new Prediction
            {
                Name = Name.DinaKuta,
                Description = "day to day living compatibility"
            };


            //Dina Kuta. - Count the constellation of the boy from that of the girl
            // and divide the number by 9. If the remainder is 2, 4, 6, 8 or 0 it is good.
            // The number of units of compatibility assigned to this Kuta is 3 in case
            // agreement is found.

            //get ruling sign
            var maleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleRuleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();

            //show user
            prediction.MaleInfo = maleRuleSign.ToString();
            prediction.FemaleInfo = femaleRuleSign.ToString();

            //count from female to male
            var count = AstronomicalCalculator.CountFromSignToSign(femaleRuleSign, maleRuleSign);

            //divide by 9 and get the remainder, done via modulus
            var remainder = count % 9;

            // If the remainder is 2, 4, 6, 8 or 0 it is good.
            if (remainder == 2 || remainder == 4 || remainder == 6 || remainder == 8 || remainder == 0)
            {
                prediction.Nature = EventNature.Good;
            }
            else
            {
                prediction.Nature = EventNature.Bad;

            }

            prediction.Info = $"remainder is {remainder}";
            return prediction;

        }

        public static Prediction LagnaAndHouse7Good(Person male, Person female)
        {

            //get birth moon sign & lagna, details needed for prediction
            var maleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, male.GetBirthDateTime()).GetSignName();
            var femaleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, female.GetBirthDateTime()).GetSignName();
            var maleLagna = AstronomicalCalculator.GetHouseSignName(1, male.GetBirthDateTime());
            var femaleLagna = AstronomicalCalculator.GetHouseSignName(1, female.GetBirthDateTime());

            //If the Janma Rasi (Moon sign) of the wife (or husband) happens to be the Lagna of the husband (or wife)
            var femaleMoonSignIsMaleLagna = femaleSign == maleLagna;
            var maleMoonSignIsFemaleLagna = maleSign == femaleLagna;
            var moonSignIsLagna = femaleMoonSignIsMaleLagna || maleMoonSignIsFemaleLagna;

            //or if the Lagna of the wife (or husband) happens to be the 7th from the position of the
            //lord of the 7th (in the other)
            var female7thLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, female.GetBirthDateTime());
            var male7thLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, male.GetBirthDateTime());
            var female7LordSign = AstronomicalCalculator.GetPlanetRasiSign(female7thLord, female.GetBirthDateTime()).GetSignName();
            var male7LordSign = AstronomicalCalculator.GetPlanetRasiSign(male7thLord, male.GetBirthDateTime()).GetSignName();
            var femaleLagna7thFromMaleLord = AstronomicalCalculator.CountFromSignToSign(male7LordSign, femaleLagna) == 7;
            var maleLagna7thFromFemaleLord = AstronomicalCalculator.CountFromSignToSign(female7LordSign, maleLagna) == 7;
            var lagna7thFromLord = femaleLagna7thFromMaleLord || maleLagna7thFromFemaleLord;

            //if either condition is met
            var occuring = moonSignIsLagna || lagna7thFromLord;


            //fill details to show user if occuring, else nothing
            var prediction = new Prediction();
            if (occuring)
            {
                prediction.Name = Name.LagnaAnd7thGood;
                prediction.Nature = EventNature.Good;
                prediction.Info = "marriage stable, mutual understanding and affection";
                prediction.Description = "special combination";
                prediction.MaleInfo = maleLagna.ToString();
                prediction.FemaleInfo = femaleLagna.ToString();
            }

            return prediction;

        }

        public static Prediction KujaDosa(Person male, Person female)
        {

            //get kuja dosha score for male & female
            var maleScore = getTotalDosaScore(male.GetBirthDateTime());
            var femaleScore = getTotalDosaScore(female.GetBirthDateTime());

            //interpret results 
            var results = interpretScore(maleScore, femaleScore);


            return results;


            //FUNCTIONS

            //gets the total score for all the planets
            double getTotalDosaScore(Time birthTime)
            {
                //list of planets to check
                var dosaPlanets = new List<PlanetName> { PlanetName.Mars, PlanetName.Saturn, PlanetName.Rahu, PlanetName.Ketu, PlanetName.Sun };

                //dosa starts at 0
                double dosaCount = 0;

                //add together all the dosa score for each planet
                foreach (var planet in dosaPlanets)
                {
                    dosaCount += getDosaScoreForPlanet(planet, birthTime);
                }

                return dosaCount;
            }

            //based on planet gets the dosa score, 0 is no dosa
            double getDosaScoreForPlanet(PlanetName planet, Time birthTime)
            {
                //1. GET DETAILS
                //get planets house details
                var planetHouse = AstronomicalCalculator.GetHousePlanetIsIn(birthTime, planet);
                var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planet, birthTime).GetSignName();
                var planetIn7Or8 = planetHouse == 7 || planetHouse == 8;
                var planetIn2Or4Or12 = planetHouse == 2 || planetHouse == 4 || planetHouse == 12;

                //set what the input planet is
                var planetIsSaturnRahuKetu = planet == PlanetName.Saturn || planet == PlanetName.Rahu || planet == PlanetName.Ketu;
                var planetIsMars = planet == PlanetName.Mars;
                var planetIsSun = planet == PlanetName.Sun;


                //2. HANDLE EXCEPTIONS

                //if the planet is not in any of the special house, then end here as 0 dosa
                if (planetIn7Or8 == false && planetIn2Or4Or12 == false) { return 0; }

                //2.1   Mars in the 2nd can be said to be bad provided such
                //      2nd house is any other than Gemini and Virgo;
                var geminiOrVirgo = planetSign == ZodiacName.Gemini || planetSign == ZodiacName.Virgo;
                if (planet == PlanetName.Mars && planetHouse == 2 && geminiOrVirgo) { return 0; }

                //2.2   in the 12th the dosha is produced when such 12th house is
                //      any other than Taurus and Libra
                var taurusOrLibra = planetSign == ZodiacName.Taurus || planetSign == ZodiacName.Libra;
                if (planet == PlanetName.Mars && planetHouse == 12 && taurusOrLibra) { return 0; }

                //2.3   in the 4th house Mars causes dosha provided the house falls in
                //      any sign other than Aries and Scorpio;
                var ariesOrScorpio = planetSign == ZodiacName.Aries || planetSign == ZodiacName.Scorpio;
                if (planet == PlanetName.Mars && planetHouse == 4 && ariesOrScorpio) { return 0; }

                //2.4   when the 7th is other than Capricorn and Cancer, the dosha is given rise to;
                var cancerOrCapricorn = planetSign == ZodiacName.Capricornus || planetSign == ZodiacName.Cancer;
                if (planet == PlanetName.Mars && planetHouse == 7 && cancerOrCapricorn) { return 0; }

                //2.5   and Mars gives bad effects in the 8th, provided the 8th is any other than
                //      Sagittarius and Pisces.
                var sagittariusOrPisces = planetSign == ZodiacName.Sagittarius || planetSign == ZodiacName.Pisces;
                if (planet == PlanetName.Mars && planetHouse == 8 && sagittariusOrPisces) { return 0; }

                //2.6   In Aquarius and Leo, Mars produces no dosha whatsoever.
                var aquariusOrLeo = planetSign == ZodiacName.Aquarius || planetSign == ZodiacName.Leo;
                if (planet == PlanetName.Mars && aquariusOrLeo) { return 0; }

                //2.7   The dosha is counteracted by the conjunction of Mars and Jupiter or Mars and the Moon;
                var marsJupterConjunct = AstronomicalCalculator.IsPlanetConjunctWithPlanet(PlanetName.Mars, PlanetName.Jupiter, birthTime);
                var marsMoonConjunct = AstronomicalCalculator.IsPlanetConjunctWithPlanet(PlanetName.Mars, PlanetName.Moon, birthTime);
                if (marsJupterConjunct && marsMoonConjunct) { return 0; }

                //2.8   or by the presence of Jupiter or Venus in the ascendant.
                var jupiterInLagna = AstronomicalCalculator.IsPlanetInHouse(birthTime, PlanetName.Jupiter, 1);
                var venusInLagna = AstronomicalCalculator.IsPlanetInHouse(birthTime, PlanetName.Venus, 1);
                if (jupiterInLagna || venusInLagna) { return 0; }



                //3. CALCULATE RELATIONSHIP

                //get planet debilitated & exalted friendship
                var planetDebilitated = AstronomicalCalculator.IsPlanetDebilited(planet, birthTime);
                var planetExalted = AstronomicalCalculator.IsPlanetExaltated(planet, birthTime);


                //for rahu/ketu special method
                //Rahu and Ketu give the effects of the lords of the signs they are in
                //ref : vedictime.com/en/library/graha/rahu
                //TODO need checking if the above statement is correct, for now use it
                if (planet == PlanetName.Rahu || planet == PlanetName.Ketu)
                {
                    //change input planet from rahu/ketu to the lord of the sign
                    var lordOfRahuKetuSign = AstronomicalCalculator.GetLordOfZodiacSign(planetSign);
                    planet = lordOfRahuKetuSign;

                    //change current occupied rahu/ketu sign to the one occupied by the lord
                    planetSign = AstronomicalCalculator.GetPlanetRasiSign(lordOfRahuKetuSign, birthTime).GetSignName();
                }


                //get relationship between planet & occupied sign 
                var houseSignRelation = AstronomicalCalculator.GetPlanetRelationshipWithSign(planet, planetSign, birthTime);

                //first set all friendship as not true
                bool planetInEnemy = false, planetInOwn = false, planetInNeutral = false, planetInFriendly = false;


                //based on the relationship between planet & occupied sign set the relationship
                switch (houseSignRelation)
                {
                    case PlanetToSignRelationship.Swavarga:
                        planetInOwn = true;
                        break;
                    case PlanetToSignRelationship.AdhiMitravarga:
                    case PlanetToSignRelationship.Mitravarga:
                        planetInFriendly = true;
                        break;
                    case PlanetToSignRelationship.Samavarga:
                        planetInNeutral = true;
                        break;
                    case PlanetToSignRelationship.Satruvarga:
                    case PlanetToSignRelationship.AdhiSatruvarga:
                        planetInEnemy = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                //if more than 1 relation occuring, raise alarm
                var relationship = new[]
                {
                    planetInEnemy,
                    planetInOwn,
                    planetInNeutral,
                    planetInFriendly,
                    planetDebilitated,
                    planetExalted
                };
                var relationshipCount = relationship.Sum(x => x ? 1 : 0);
                if (relationshipCount > 1) { throw new Exception("Something wrong, more than 1 relationship found!"); }


                //4. INTERPRET THE RESULTS GOTTEN FROM ABOVE

                //if planet is in 8th or 7th house 
                if (planetIn7Or8)
                {
                    //process based on relationship
                    if (planetDebilitated)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 100; }
                        if (planetIsSaturnRahuKetu) { return 75; }
                        if (planetIsSun) { return 50; }
                    }
                    else if (planetInEnemy)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 90; }
                        if (planetIsSaturnRahuKetu) { return 67.5; }
                        if (planetIsSun) { return 45; }
                    }
                    else if (planetInNeutral)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 80; }
                        if (planetIsSaturnRahuKetu) { return 60; }
                        if (planetIsSun) { return 40; }
                    }
                    else if (planetInFriendly)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 70; }
                        if (planetIsSaturnRahuKetu) { return 52.50; }
                        if (planetIsSun) { return 35; }
                    }
                    else if (planetInOwn)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 60; }
                        if (planetIsSaturnRahuKetu) { return 45; }
                        if (planetIsSun) { return 30; }
                    }
                    else if (planetExalted)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 50; }
                        if (planetIsSaturnRahuKetu) { return 37.5; }
                        if (planetIsSun) { return 25; }
                    }

                }

                //if planet is in 2, 4, 12 house 
                if (planetIn2Or4Or12)
                {
                    //process based on relationship
                    if (planetDebilitated)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 50; }
                        if (planetIsSaturnRahuKetu) { return 37.5; }
                        if (planetIsSun) { return 25; }
                    }
                    else if (planetInEnemy)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 45; }
                        if (planetIsSaturnRahuKetu) { return 33.75; }
                        if (planetIsSun) { return 22.5; }
                    }
                    else if (planetInNeutral)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 40; }
                        if (planetIsSaturnRahuKetu) { return 30; }
                        if (planetIsSun) { return 20; }
                    }
                    else if (planetInFriendly)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 35; }
                        if (planetIsSaturnRahuKetu) { return 26.25; }
                        if (planetIsSun) { return 17.5; }
                    }
                    else if (planetInOwn)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 30; }
                        if (planetIsSaturnRahuKetu) { return 22.5; }
                        if (planetIsSun) { return 15; }
                    }
                    else if (planetExalted)
                    {
                        //based on planet set different values
                        if (planetIsMars) { return 25; }
                        if (planetIsSaturnRahuKetu) { return 18.75; }
                        if (planetIsSun) { return 12.5; }
                    }

                }

                //if control reaches here, than no kuja dosa score
                return 0;
            }

            //interpret kuja dosa & creates the prediction
            Prediction interpretScore(double scoreMale, double scoreFemale)
            {
                var prediction = new Prediction
                {
                    Name = Name.KujaDosa,
                    Description = "if bad, may cause death/bad health to spouse"
                };

                //show details to user
                prediction.MaleInfo = maleScore.ToString();
                prediction.FemaleInfo = femaleScore.ToString();


                //get score difference between female & male to set threshold
                var difference = Math.Abs(scoreMale - scoreFemale);
                var threshold = 5;
                var differenceIsBelowThreshold = difference <= threshold;

                //if the dosha units in both charts are equal or nearly so,
                //the matching can be said to be good.
                if (differenceIsBelowThreshold)
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "dosha in both are equal or nearly so, match is good";
                }
                else
                {
                    //if the female chart has more dosha (more than threshold)
                    //then the charts cannot be matched.
                    if (scoreFemale > scoreMale)
                    {
                        prediction.Nature = EventNature.Bad;
                        prediction.Info = "charts cannot be matched, female chart has more dosha";
                    }
                }

                //If the dosha in the male horoscope not exceeds the dosha
                //in the female horoscope by 25%, it is passable.
                if (scoreMale > scoreFemale && !differenceIsBelowThreshold)
                {
                    //get 25% of female score
                    var female25Percent = scoreFemale * 0.25;

                    //get amount male score exceed female 
                    var exceedAmount = scoreMale - scoreFemale;

                    //if exceeded amount is below 25% of female, pass
                    if (exceedAmount < female25Percent)
                    {
                        prediction.Nature = EventNature.Good;
                        prediction.Info = "it is passable, male dosha below 25% of female";
                    }
                    //If the dosha in the male horoscope exceeds this percentage
                    //then the charts cannot be matched.
                    else
                    {
                        prediction.Nature = EventNature.Bad;
                        prediction.Info = "charts cannot be matched, male dosha above 25% of female";
                    }
                }

                return prediction;
            }
        }

        public static Prediction BadConstellations(Person male, Person female)
        {
            var prediction = new Prediction();

            // Almost all authors agree that certain parts of Moola, Astesha, Jyeshta and Visakha are destructive
            // constellations -
            // Aslesha (first quarter) for husband's mother;
            // Jyeshta (first quarter) for girl's husband's elder brother;
            // and Visakha (last quarter) for husband's younger brother.

            //get female constellation
            var femaleConstellation = AstronomicalCalculator.GetMoonConstellation(female.GetBirthDateTime());
            var maleConstellation = AstronomicalCalculator.GetMoonConstellation(male.GetBirthDateTime());

            //Moola (first quarter) for husband's father;
            //the boy or girl born in the first quarter of Moola is to be
            //rejected as it is said to cause the death of the father-in-law.
            var isMoola1stQuarterFemale = femaleConstellation.GetConstellationName() == ConstellationName.Moola && femaleConstellation.GetQuarter() == 1;
            var isMoola1stQuarterMale = maleConstellation.GetConstellationName() == ConstellationName.Moola && maleConstellation.GetQuarter() == 1;

            if (isMoola1stQuarterFemale || isMoola1stQuarterMale)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "cause the death of the father-in-law";
            }

            //Aslesha (first quarter) for husband's mother;
            var isAslesha1stQuarterFemale = femaleConstellation.GetConstellationName() == ConstellationName.Aslesha && femaleConstellation.GetQuarter() == 1;
            if (isAslesha1stQuarterFemale)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "cause evil to husband's mother";
            }

            //Jyesta (first quarter) for girl's husband's elder brother;
            //A girl born in Jyesta is said to
            // cause evil to her husband's elder brother.
            var isJyesta1stQuarterFemale = femaleConstellation.GetConstellationName() == ConstellationName.Jyesta && femaleConstellation.GetQuarter() == 1;
            if (isJyesta1stQuarterFemale)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "cause evil to husband's elder brother";
            }

            //Visakha (last quarter) for husband's younger brother
            //girl born in Visakha is said to bring about the destruction of her husband's
            // younger brother
            var isVishhaka4thQuarterFemale = femaleConstellation.GetConstellationName() == ConstellationName.Vishhaka && femaleConstellation.GetQuarter() == 4;
            if (isVishhaka4thQuarterFemale)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "cause evil to husband's younger brother";
            }


            //if any of the above conditions met only then, fill in name & description
            if (prediction.Info != "")
            {
                prediction.Name = Name.BadConstellation;
                prediction.Description = "Evil constellation, if present analyse horoscope";
            }

            return prediction;

        }
    }

}


