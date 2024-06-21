using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;


namespace VedAstro.Library
{

    /// <summary>
    /// Collection of calculators for match/compatibility 
    /// </summary>
    public static class MatchReportFactory
    {
        /// <summary>
        /// Gets the compatibility report for a male & female
        /// The place where compatibility report gets generated
        /// </summary>
        public static MatchReport GetNewMatchReport(Person male, Person female, string userId)
        {
            //calculators are designed to fail 1st,
            //as such if they fail don't shut down the whole show!

            //list all calculators here, to be processed one by one
            List<Func<Person, Person, MatchPrediction>> calculatorList = new List<Func<Person, Person, MatchPrediction>>()
            {
                MatchReportFactory.GrahaMaitram, //5
                MatchReportFactory.Rajju,
                MatchReportFactory.NadiKuta, //8
                MatchReportFactory.VasyaKuta, //2
                MatchReportFactory.DinaKuta, //3
                MatchReportFactory.GunaKuta,//6
                MatchReportFactory.Mahendra,
                MatchReportFactory.StreeDeergha,
                MatchReportFactory.RasiKuta,//7
                MatchReportFactory.VedhaKuta,
                MatchReportFactory.Varna, //1
                MatchReportFactory.YoniKuta,//4
                MatchReportFactory.LagnaAndHouse7Good,
                MatchReportFactory.KujaDosa,
                MatchReportFactory.BadConstellations,
                MatchReportFactory.SexEnergy
            };

            //place to put results
            List<MatchPrediction> compatibilityPredictions = new List<MatchPrediction>();

            //now calculate one by one safely
            foreach (var calculator in calculatorList)
            {
                MatchPrediction prediction;

                try
                {
                    prediction = calculator(male, female);
                }
                catch (Exception e)
                {
                    //log error
                    LibLogger.Debug(e, $"Male:{male.Name} Female:{female.Name}");

                    //return empty
                    prediction = MatchPrediction.Empty; //default empty
                }

                //add to return list
                compatibilityPredictions.Add(prediction);
            }

            //parse data
            //note KUTA score added below
            var report = new MatchReport(Tools.GenerateId(), male, female, 0, "...", compatibilityPredictions, new[] { userId }); //at creation only 1 user

            //count the total points
            report.KutaScore = CalculateTotalPoints(report);

            //generate ML embeddings
            report.Embeddings = CalculateEmbeddings(report);

            //check results for exceptions
            HandleExceptions(ref report);

            return report;

            //-------------------------LOCAL FUNCTIONS

            //checks & modifies results for exceptions 
            void HandleExceptions(ref MatchReport report)
            {

                var list = report.PredictionList;

                //each exception below modifies the list if needed

                streeDeerghaException(list);

                rajjuException(list);

                nadiKutaException(list);



                //FUNCTIONS

                void streeDeerghaException(List<MatchPrediction> list)
                {
                    //1.The absence of Stree-Deerga may be ignored if
                    //  Rasi Kuta add Graha Maitri are present.

                    //get the needed prediction
                    var streeDeerga = list.Find(pr => pr.Name == MatchPredictionName.StreeDeergha);
                    var rasiKuta = list.Find(pr => pr.Name == MatchPredictionName.RasiKuta);
                    var grahaMaitram = list.Find(pr => pr.Name == MatchPredictionName.GrahaMaitram);

                    //if prediction is bad and exception can be applied
                    var streeDeergaIsBad = streeDeerga?.Nature == EventNature.Bad;
                    var rasiKutaIsGood = rasiKuta?.Nature == EventNature.Good;
                    var grahaMaitramIsGood = grahaMaitram?.Nature == EventNature.Good;

                    if (streeDeergaIsBad && rasiKutaIsGood && grahaMaitramIsGood)
                    {
                        //create new prediction
                        var newPrediction = new MatchPrediction()
                        {
                            Name = streeDeerga.Name,
                            Description = streeDeerga.Description,
                            FemaleInfo = streeDeerga.FemaleInfo,
                            MaleInfo = streeDeerga.MaleInfo,
                            Info = "bad Stree-Deerga is neutralized by good Rasi Kuta and Graha Maitri",
                            Nature = EventNature.Neutral
                        };

                        //replace old prediction with new one
                        list.Remove(streeDeerga);
                        list.Add(newPrediction);

                    }

                }

                void rajjuException(List<MatchPrediction> list)
                {
                    //2.Rajju Kuta need not be considered in case Graha Maitri, Rasi, Dina
                    //  and Mahendra Kuta are present.

                    //get the needed prediction
                    var rajju = list.Find(pr => pr.Name == MatchPredictionName.Rajju);
                    var grahaMaitram = list.Find(pr => pr.Name == MatchPredictionName.GrahaMaitram);
                    var rasiKuta = list.Find(pr => pr.Name == MatchPredictionName.RasiKuta);
                    var dinaKuta = list.Find(pr => pr.Name == MatchPredictionName.DinaKuta);
                    var mahendra = list.Find(pr => pr.Name == MatchPredictionName.Mahendra);


                    //if prediction is bad and exception can be applied
                    var rajjuIsBad = rajju?.Nature == EventNature.Bad;
                    var grahaMaitramIsGood = grahaMaitram?.Nature == EventNature.Good;
                    var rasiKutaIsGood = rasiKuta?.Nature == EventNature.Good;
                    var dinaKutaIsGood = dinaKuta?.Nature == EventNature.Good;
                    var mahendraIsGood = mahendra?.Nature == EventNature.Good;


                    if (rajjuIsBad && grahaMaitramIsGood && rasiKutaIsGood && dinaKutaIsGood && mahendraIsGood)
                    {
                        //create new prediction
                        var newPrediction = new MatchPrediction()
                        {
                            Name = rajju.Name,
                            Description = rajju.Description,
                            FemaleInfo = rajju.FemaleInfo,
                            MaleInfo = rajju.MaleInfo,
                            Info = "bad Rajju Kuta is neutralized by good Graha Maitri, Rasi, Dina and Mahendra",
                            Nature = EventNature.Neutral
                        };

                        //replace old prediction with new one
                        list.Remove(rajju);
                        list.Add(newPrediction);

                    }

                }

                void nadiKutaException(List<MatchPrediction> list)
                {
                    //The evil due to Nadi Kuta can be ignored subject to the following
                    // conditions: -
                    // (a) The Rasi and Rajju Kuta prevail

                    //get the needed prediction
                    var nadiKuta = list.Find(pr => pr.Name == MatchPredictionName.NadiKuta);
                    var rasiKuta = list.Find(pr => pr.Name == MatchPredictionName.RasiKuta);
                    var rajju = list.Find(pr => pr.Name == MatchPredictionName.Rajju);

                    //if prediction is bad and exception can be applied
                    var nadiKutaIsBad = nadiKuta?.Nature == EventNature.Bad;
                    var rasiKutaIsGood = rasiKuta?.Nature == EventNature.Good;
                    var rajjuIsGood = rajju?.Nature == EventNature.Good;

                    if (nadiKutaIsBad && rasiKutaIsGood && rajjuIsGood)
                    {
                        //create new prediction
                        var newPrediction = new MatchPrediction()
                        {
                            Name = nadiKuta.Name,
                            Description = nadiKuta.Description,
                            FemaleInfo = nadiKuta.FemaleInfo,
                            MaleInfo = nadiKuta.MaleInfo,
                            Info = "bad Nadi Kuta is neutralized by good Rasi Kuta and Rajju",
                            Nature = EventNature.Neutral
                        };

                        //replace old prediction with new one
                        list.Remove(nadiKuta);
                        list.Add(newPrediction);

                    }

                }

            }

            //Kutas analysis consist of analyzing 12 Factors.Every factor contributes
            //some points, toward a maximum total score of 36 points.
            double CalculateTotalPoints(MatchReport report)
            {

                double totalPoints = 0; //this is over 36

                //count points total 36 points
                foreach (var prediction in report.PredictionList)
                {
                    //only count if prediction is good
                    if (prediction.Nature != EventNature.Good) { continue; }

                    //based on prediction name add together the score
                    //only certain kuta have points to consider others do not
                    switch (prediction.Name)
                    {
                        //Dina Kuta (3 pts)
                        case MatchPredictionName.DinaKuta:
                            totalPoints += 3;
                            break;
                        //Gana Kuta: (6 pts)
                        case MatchPredictionName.GunaKuta:
                            totalPoints += 6;
                            break;
                        //Nadi Kuta: (8 pts)
                        case MatchPredictionName.NadiKuta:
                            totalPoints += 8;
                            break;
                        //Rashi Kuta - (7 pts)
                        case MatchPredictionName.RasiKuta:
                            totalPoints += 7;
                            break;
                        //Graha Maitram - (5 pts)
                        case MatchPredictionName.GrahaMaitram:
                            totalPoints += 5;
                            break;
                        // Vasyu Kuta - (2 pts).
                        case MatchPredictionName.VasyaKuta:
                            totalPoints += 2;
                            break;
                        // Varna Kuta - (1 pt)
                        case MatchPredictionName.Varna:
                            totalPoints += 1;
                            break;
                        //Yoni Kuta - (4 pts)
                        case MatchPredictionName.YoniKuta:
                            totalPoints += 4;
                            break;
                    }
                }

                //convert score to percentage of 36
                //note : should look like this here 42.4444444
                var rawKutaPercentage = (totalPoints / 36.0) * 100.0;

                //round to nearest for best accuracy
                var rounded = Math.Round(rawKutaPercentage / 5.0) * 5;

                return rounded;
            }

        }

        /// <summary>
        /// Using the available kuta data in report embeddings similar to LLM is generated
        /// Embeddings is not normalized, since this is only 1 row
        /// </summary>
        public static double[] CalculateEmbeddings(MatchReport report)
        {
            //list each embedding
            double yoniKutaPoints = 0;
            double varnaPoints = 0;
            double vasyaKutaPoints = 0;
            double grahaMaitramPoints = 0;
            double rasiKutaPoints = 0;
            double nadiKutaPoints = 0;
            double gunaKutaPoints = 0;
            double dinaKutaPoints = 0;
            //ideas
            //total = compiled over 36 TODO needs testings

            //#1 : CREATE RAW EMBEDDING VECTORS
            //extract needed data from the prediction list
            foreach (var prediction in report.PredictionList)
            {
                //only count if prediction is good
                if (prediction.Nature != EventNature.Good) { continue; }

                //based on prediction name add together the score
                //only certain kuta have points to consider others do not
                switch (prediction.Name)
                {
                    //Dina Kuta (3 pts)
                    case MatchPredictionName.DinaKuta:
                        dinaKutaPoints = 3;
                        break;
                    //Gana Kuta: (6 pts)
                    case MatchPredictionName.GunaKuta:
                        gunaKutaPoints = 6;
                        break;
                    //Nadi Kuta: (8 pts)
                    case MatchPredictionName.NadiKuta:
                        nadiKutaPoints = 8;
                        break;
                    //Rashi Kuta - (7 pts)
                    case MatchPredictionName.RasiKuta:
                        rasiKutaPoints = 7;
                        break;
                    //Graha Maitram - (5 pts)
                    case MatchPredictionName.GrahaMaitram:
                        grahaMaitramPoints = 5;
                        break;
                    // Vasyu Kuta - (2 pts).
                    case MatchPredictionName.VasyaKuta:
                        vasyaKutaPoints = 2;
                        break;
                    // Varna Kuta - (1 pt)
                    case MatchPredictionName.Varna:
                        varnaPoints = 1;
                        break;
                    //Yoni Kuta - (4 pts)
                    case MatchPredictionName.YoniKuta:
                        yoniKutaPoints = 4;
                        break;
                }

            }


            //#2 : PACK & NORMALIZE EMBEDDINGS
            //put data in 1 big row (raw data)
            //NOTE: normalization abandoned at this stage since proper Geometry can be formed with just 1 row
            //NOTE: we need a modal to make embeddings, as such seems silly to do at this level
            var rawEmbeddings = new double[] { dinaKutaPoints, gunaKutaPoints, nadiKutaPoints, rasiKutaPoints, grahaMaitramPoints, vasyaKutaPoints, varnaPoints, yoniKutaPoints, };

            return rawEmbeddings;

        }

        public static MatchPrediction Mahendra(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.Mahendra,
                Description = "well-being and longevity"
            };


            //Mahendra. - The constellation of the boy counted from that of the girl
            // should be the 4th, 7th, 10th, 13th, 16th, 19th, 22nd or 25th. This
            // promotes well-being and increases longevity.

            //get ruling sign
            var maleConstellation = Calculate.MoonConstellation(male.BirthTime);
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime);

            //show extra info
            prediction.MaleInfo = maleConstellation.ToString();
            prediction.FemaleInfo = femaleConstellation.ToString();

            var count = Calculate.CountFromConstellationToConstellation(femaleConstellation, maleConstellation);

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

        public static MatchPrediction NadiKuta(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.NadiKuta,
                Description = "nervous energy compatibility (important)"
            };


            //get constellation
            var maleConstellation = Calculate.MoonConstellation(male.BirthTime);
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime);

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
                var maleJanmaLord = Calculate.LordOfZodiacSign(Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName());
                var femaleJanmaLord = Calculate.LordOfZodiacSign(Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName());
                var sameJanmaLord = maleJanmaLord == femaleJanmaLord;

                //b.The lords of the Janma Rasi of the couple are friends.
                var relationship = Calculate.PlanetPermanentRelationshipWithPlanet(maleJanmaLord, femaleJanmaLord);
                var janmaIsFriend = relationship is PlanetToPlanetRelationship.BestFriend or PlanetToPlanetRelationship.Friend;

                //if any above exceptions met, change prediction
                if (sameJanmaLord || janmaIsFriend)
                {
                    prediction.Nature = EventNature.Neutral;
                    prediction.Info = "bad, but neutralized by friendly Janma Rasi lord";
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
                        return "Vatha (wind)";

                    case ConstellationName.Bharani:
                    case ConstellationName.Mrigasira:
                    case ConstellationName.Pushyami:
                    case ConstellationName.Pubba:
                    case ConstellationName.Chitta:
                    case ConstellationName.Anuradha:
                    case ConstellationName.Poorvashada:
                    case ConstellationName.Dhanishta:
                    case ConstellationName.Uttarabhadra:
                        return "Pitha (bile)";

                    case ConstellationName.Krithika:
                    case ConstellationName.Rohini:
                    case ConstellationName.Aslesha:
                    case ConstellationName.Makha:
                    case ConstellationName.Swathi:
                    case ConstellationName.Vishhaka:
                    case ConstellationName.Uttarashada:
                    case ConstellationName.Sravana:
                    case ConstellationName.Revathi:
                        return "Sleshma (phlegm)";


                    default:
                        throw new ArgumentOutOfRangeException(nameof(constellation), constellation, null);
                }
            }
        }

        public static MatchPrediction GunaKuta(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.GunaKuta,
                Description = "temperament and character compatibility"
            };


            //get constellation
            var maleConstellation = Calculate.MoonConstellation(male.BirthTime);
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime);

            //get guna
            var maleGuna = getGuna(maleConstellation.GetConstellationName());
            var femaleGuna = getGuna(femaleConstellation.GetConstellationName());

            //show user
            prediction.MaleInfo = maleGuna.ToString();
            prediction.FemaleInfo = femaleGuna.ToString();

            //rename for readability
            var manIsManushaDeva = maleGuna is Guna.DevaAngel or Guna.ManushaHuman;
            var girlIsRakshasa = femaleGuna == Guna.RakshasaDemon;
            var femaleIsManushaDeva = femaleGuna is Guna.DevaAngel or Guna.ManushaHuman;
            var maleIsRakshasa = maleGuna == Guna.RakshasaDemon;


            //Hence one born in a Deva
            // constellation is not able to get on well with a person born in Rakshasa
            // constellation. A Deva can marry a Deva, a Manusha can marry a
            // Manusha and a Rakshasa can marry a Rakshasa.
            if (maleGuna == femaleGuna)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = $"both are same {Format.FormatName(femaleGuna)} Guna";
            }

            // Manusha or a Deva man should not marry a Rakshasa girl unless there
            // are other neutralizing factors.
            else if (manIsManushaDeva && girlIsRakshasa)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "Manusha/Deva boy cannot marry a Rakshasa girl unless there are other neutralizing factors";
            }

            // But marriage between a Rakshasa man
            // and a Deva or Manusha girl is passable.
            else if (femaleIsManushaDeva && maleIsRakshasa)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "marriage between a Rakshasa boy and a Deva/Manusha girl is passable";
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
            var maleToFemale = Calculate.CountFromConstellationToConstellation(maleConstellation, femaleConstellation);
            //only show user the exception when prediction thus is bad
            if (maleToFemale > 14 && prediction.Nature == EventNature.Bad)
            {
                //create new prediction
                var newPrediction = new MatchPrediction()
                {
                    Name = prediction.Name,
                    Description = prediction.Description,
                    FemaleInfo = prediction.FemaleInfo,
                    MaleInfo = prediction.MaleInfo,
                    Info = "evil maybe ignored, female star is more than 14th from male's",
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
                        return Guna.DevaAngel;

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
                        return Guna.ManushaHuman;

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
                        return Guna.RakshasaDemon;


                    default: throw new Exception("");
                }
            }

        }

        public static MatchPrediction Varna(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.Varna,
                Description = "spiritual/ego compatibility"
            };


            //copy info into prediction data
            var maleGrade = Calculate.BirthVarna(male.BirthTime);
            prediction.MaleInfo = Format.FormatName(maleGrade.ToString());

            var femaleGrade = Calculate.BirthVarna(female.BirthTime);
            prediction.FemaleInfo = Format.FormatName(femaleGrade.ToString());


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
                prediction.Info = "boy higher and girl lower or both same Varna, is good";
            }

            return prediction;

        }

        public static MatchPrediction YoniKuta(Person male, Person female)
        {

            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.YoniKuta,
                Description = "sex compatibility"
            };

            //1. Get Details

            var maleConstellation = Calculate.MoonConstellation(male.BirthTime).GetConstellationName();
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime).GetConstellationName();


            //get group names
            var maleGroupName = Calculate.YoniKutaAnimalFromConstellation(maleConstellation);
            var femaleGroupName = Calculate.YoniKutaAnimalFromConstellation(femaleConstellation);

            AnimalName maleAnimal = maleGroupName.Animal;
            AnimalName femaleAnimal = femaleGroupName.Animal;
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
                var sameAnimal = maleAnimal == femaleAnimal;
                var sameGender = maleGender == femaleGender;
                if (sameAnimal && !sameGender) //same animal opposite gender
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "favourable results to the fullest extent, harmony and progeny";
                }
                else if (sameAnimal && sameGender) //same animal same gender 
                {
                    prediction.Nature = EventNature.Good;
                    prediction.Info = "not perfect, better than normal."; //(not 100% known)
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
            int getAnimalCompatible(AnimalName a, AnimalName b)
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


        }

        public static MatchPrediction VedhaKuta(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.Vedha,
                Description = "birth constellations compatibility"
            };


            //The following pairs of constellations affect each
            // other and, therefore, no marriage should be brought about between a
            // boy and girl whos Janma Nakshatras belong to the same pair unless the
            // are other relieving factors.

            var maleConstellation = Calculate.MoonConstellation(male.BirthTime).GetConstellationName();
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime).GetConstellationName();

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

        public static MatchPrediction Rajju(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.Rajju,
                Description = "strength/duration of married life (important)"
            };


            var maleConstellation = Calculate.MoonConstellation(male.BirthTime).GetConstellationName();
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime).GetConstellationName();


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
                prediction.Info = "both constellations are in different groups";
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

        public static MatchPrediction VasyaKuta(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.VasyaKuta,
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
            var maleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName();
            var femaleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName();

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
                if (subSign is ZodiacName.Capricorn or ZodiacName.Virgo)
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
            if (mainSign == ZodiacName.Capricorn)
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
                if (subSign is ZodiacName.Capricorn)
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
                prediction.Info = "male controls female";

            }
            else if (femaleControlMale)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "female controls male";
            }
            else
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "neither controls the other";
            }

            return prediction;
        }

        public static MatchPrediction GrahaMaitram(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.GrahaMaitram,
                Description = "happiness, mental compatibility (important)"
            };


            //get ruling sign
            var maleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName();
            var femaleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName();

            //get lords of sign
            var maleLord = Calculate.LordOfZodiacSign(maleRuleSign);
            var femaleLord = Calculate.LordOfZodiacSign(femaleRuleSign);

            // get permanent relationship of planets
            // Ref : Some suggest that in considering the planetary
            // relations,the temporary dispositions should
            // also be taken into account. This in my humble
            // opinion is uncalled for, because, the entire
            // subject of adaptability hinges on the birth
            // constellations and not on birth charts as a whole.
            var maleToFemaleRelation = Calculate.PlanetPermanentRelationshipWithPlanet(maleLord, femaleLord);
            var femaleToMaleRelation = Calculate.PlanetPermanentRelationshipWithPlanet(femaleLord, maleLord);

            //show user
            prediction.MaleInfo = maleToFemaleRelation.ToString() + " Sign";
            prediction.FemaleInfo = femaleToMaleRelation.ToString() + " Sign";

            //rename relationship for readability
            var isMaleFriend = maleToFemaleRelation is PlanetToPlanetRelationship.BestFriend or PlanetToPlanetRelationship.Friend;
            var isFemaleFriend = femaleToMaleRelation is PlanetToPlanetRelationship.BestFriend or PlanetToPlanetRelationship.Friend;
            var isMaleEnemy = maleToFemaleRelation is PlanetToPlanetRelationship.BitterEnemy or PlanetToPlanetRelationship.Enemy;
            var isFemaleEnemy = femaleToMaleRelation is PlanetToPlanetRelationship.BitterEnemy or PlanetToPlanetRelationship.Enemy;
            var isMaleNeutral = maleToFemaleRelation is PlanetToPlanetRelationship.Neutral;
            var isFemaleNeutral = femaleToMaleRelation is PlanetToPlanetRelationship.Neutral;
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
                prediction.Info = "no good connection between these horoscopes";
            }

            return prediction;
        }

        public static MatchPrediction RasiKuta(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.RasiKuta,
                Description = "rasi compatibility"
            };


            //get ruling sign
            var maleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName();
            var femaleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName();

            //count from female to male
            var femaleToMale = Calculate.CountFromSignToSign(femaleRuleSign, maleRuleSign);
            var maleToFemale = Calculate.CountFromSignToSign(maleRuleSign, femaleRuleSign);

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
                var maleConstellation = Calculate.MoonConstellation(male.BirthTime).GetConstellationNumber();
                var femaleConstellation = Calculate.MoonConstellation(female.BirthTime).GetConstellationNumber();

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
                //if same Janma Rasi is only good if in different constellation
                if (femaleConstellation == maleConstellation)
                {
                    prediction.Nature = EventNature.Bad;
                    prediction.Info = "same constellation, alliance should be rejected.";
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
                var maleJanmaLord = Calculate.LordOfZodiacSign(Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName());
                var femaleJanmaLord = Calculate.LordOfZodiacSign(Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName());
                var sameJanmaLord = maleJanmaLord == femaleJanmaLord;

                //b.The lords of the Janma Rasi of the couple are friends.
                var relationship = Calculate.PlanetPermanentRelationshipWithPlanet(maleJanmaLord, femaleJanmaLord);
                var janmaIsFriend = relationship is PlanetToPlanetRelationship.BestFriend or PlanetToPlanetRelationship.Friend;

                //if any above exceptions met, change prediction
                if (sameJanmaLord || janmaIsFriend)
                {
                    //create new prediction
                    var newPrediction = new MatchPrediction()
                    {
                        Name = prediction.Name,
                        Description = prediction.Description,
                        FemaleInfo = prediction.FemaleInfo,
                        MaleInfo = prediction.MaleInfo,
                        Info = "bad, but neutralized by friendly Janma Rasi lord",
                        Nature = EventNature.Neutral
                    };

                    //replace old prediction
                    prediction = newPrediction;
                }


            }



            return prediction;

        }

        public static MatchPrediction StreeDeergha(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.StreeDeergha,
                Description = "husband well being, longevity and prosperity"
            };

            //Stree-Deergha. - The boy's constellation should preferably be beyond
            // the 9th from that of the girl. According to some authorities the distance
            // should be more than 7 constellations.
            //get ruling sign
            var maleConstellation = Calculate.MoonConstellation(male.BirthTime);
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime);

            //show user
            prediction.MaleInfo = maleConstellation.ToString();
            prediction.FemaleInfo = femaleConstellation.ToString();


            var count = Calculate.CountFromConstellationToConstellation(femaleConstellation, maleConstellation);

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

        public static MatchPrediction DinaKuta(Person male, Person female)
        {
            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.DinaKuta,
                Description = "day to day living compatibility"
            };


            //Dina Kuta. - Count the constellation of the boy from that of the girl
            // and divide the number by 9. If the remainder is 2, 4, 6, 8 or 0 it is good.
            // The number of units of compatibility assigned to this Kuta is 3 in case
            // agreement is found.

            //get ruling sign
            var maleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName();
            var femaleRuleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName();

            //show user
            prediction.MaleInfo = maleRuleSign.ToString();
            prediction.FemaleInfo = femaleRuleSign.ToString();

            //count from female to male
            var count = Calculate.CountFromSignToSign(femaleRuleSign, maleRuleSign);

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

        public static MatchPrediction LagnaAndHouse7Good(Person male, Person female)
        {

            //get birth moon sign & lagna, details needed for prediction
            var maleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, male.BirthTime).GetSignName();
            var femaleSign = Calculate.PlanetZodiacSign(PlanetName.Moon, female.BirthTime).GetSignName();
            var maleLagna = Calculate.HouseSignName(HouseName.House1, male.BirthTime);
            var femaleLagna = Calculate.HouseSignName(HouseName.House1, female.BirthTime);

            //If the Janma Rasi (Moon sign) of the wife (or husband) happens to be the Lagna of the husband (or wife)
            var femaleMoonSignIsMaleLagna = femaleSign == maleLagna;
            var maleMoonSignIsFemaleLagna = maleSign == femaleLagna;
            var moonSignIsLagna = femaleMoonSignIsMaleLagna || maleMoonSignIsFemaleLagna;

            //or if the Lagna of the wife (or husband) happens to be the 7th from the position of the
            //lord of the 7th (in the other)
            var female7thLord = Calculate.LordOfHouse(HouseName.House7, female.BirthTime);
            var male7thLord = Calculate.LordOfHouse(HouseName.House7, male.BirthTime);
            var female7LordSign = Calculate.PlanetZodiacSign(female7thLord, female.BirthTime).GetSignName();
            var male7LordSign = Calculate.PlanetZodiacSign(male7thLord, male.BirthTime).GetSignName();
            var femaleLagna7thFromMaleLord = Calculate.CountFromSignToSign(male7LordSign, femaleLagna) == 7;
            var maleLagna7thFromFemaleLord = Calculate.CountFromSignToSign(female7LordSign, maleLagna) == 7;
            var lagna7thFromLord = femaleLagna7thFromMaleLord || maleLagna7thFromFemaleLord;

            //if either condition is met
            var occuring = moonSignIsLagna || lagna7thFromLord;


            //fill details to show user if occuring, else nothing
            var prediction = new MatchPrediction();
            if (occuring)
            {
                prediction.Name = MatchPredictionName.LagnaAnd7thGood;
                prediction.Nature = EventNature.Good;
                prediction.Info = "marriage stable, mutual understanding and affection";
                prediction.Description = "special combination";
                prediction.MaleInfo = maleLagna.ToString();
                prediction.FemaleInfo = femaleLagna.ToString();
            }

            return prediction;

        }

        public static MatchPrediction KujaDosa(Person male, Person female)
        {

            //get kuja dosha score for male & female
            var maleScore = getTotalDosaScore(male.BirthTime);
            var femaleScore = getTotalDosaScore(female.BirthTime);

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
                var planetHouse = Calculate.HousePlanetOccupies(planet, birthTime);
                var planetSign = Calculate.PlanetZodiacSign(planet, birthTime).GetSignName();
                var planetIn7Or8 = planetHouse == HouseName.House7 || planetHouse == HouseName.House8;
                var planetIn2Or4Or12 = planetHouse == HouseName.House2 || planetHouse == HouseName.House4 || planetHouse == HouseName.House12;

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
                if (planet == PlanetName.Mars && planetHouse == HouseName.House2 && geminiOrVirgo) { return 0; }

                //2.2   in the 12th the dosha is produced when such 12th house is
                //      any other than Taurus and Libra
                var taurusOrLibra = planetSign == ZodiacName.Taurus || planetSign == ZodiacName.Libra;
                if (planet == PlanetName.Mars && planetHouse == HouseName.House12 && taurusOrLibra) { return 0; }

                //2.3   in the 4th house Mars causes dosha provided the house falls in
                //      any sign other than Aries and Scorpio;
                var ariesOrScorpio = planetSign == ZodiacName.Aries || planetSign == ZodiacName.Scorpio;
                if (planet == PlanetName.Mars && planetHouse == HouseName.House4 && ariesOrScorpio) { return 0; }

                //2.4   when the 7th is other than Capricorn and Cancer, the dosha is given rise to;
                var cancerOrCapricorn = planetSign == ZodiacName.Capricorn || planetSign == ZodiacName.Cancer;
                if (planet == PlanetName.Mars && planetHouse == HouseName.House7 && cancerOrCapricorn) { return 0; }

                //2.5   and Mars gives bad effects in the 8th, provided the 8th is any other than
                //      Sagittarius and Pisces.
                var sagittariusOrPisces = planetSign == ZodiacName.Sagittarius || planetSign == ZodiacName.Pisces;
                if (planet == PlanetName.Mars && planetHouse == HouseName.House8 && sagittariusOrPisces) { return 0; }

                //2.6   In Aquarius and Leo, Mars produces no dosha whatsoever.
                var aquariusOrLeo = planetSign == ZodiacName.Aquarius || planetSign == ZodiacName.Leo;
                if (planet == PlanetName.Mars && aquariusOrLeo) { return 0; }

                //2.7   The dosha is counteracted by the conjunction of Mars and Jupiter or Mars and the Moon;
                var marsJupterConjunct = Calculate.IsPlanetConjunctWithPlanet(PlanetName.Mars, PlanetName.Jupiter, birthTime);
                var marsMoonConjunct = Calculate.IsPlanetConjunctWithPlanet(PlanetName.Mars, PlanetName.Moon, birthTime);
                if (marsJupterConjunct && marsMoonConjunct) { return 0; }

                //2.8   or by the presence of Jupiter or Venus in the ascendant.
                var jupiterInLagna = Calculate.IsPlanetInHouse(PlanetName.Jupiter, HouseName.House1, birthTime);
                var venusInLagna = Calculate.IsPlanetInHouse(PlanetName.Venus, HouseName.House1, birthTime);
                if (jupiterInLagna || venusInLagna) { return 0; }



                //3. CALCULATE RELATIONSHIP

                //get planet debilitated & exalted friendship
                var planetDebilitated = Calculate.IsPlanetDebilitated(planet, birthTime);
                var planetExalted = Calculate.IsPlanetExalted(planet, birthTime);


                //for rahu/ketu special method
                //Rahu and Ketu give the effects of the lords of the signs they are in
                //ref : vedictime.com/en/library/graha/rahu
                //TODO need checking if the above statement is correct, for now use it
                if (planet == PlanetName.Rahu || planet == PlanetName.Ketu)
                {
                    //change input planet from rahu/ketu to the lord of the sign
                    var lordOfRahuKetuSign = Calculate.LordOfZodiacSign(planetSign);
                    planet = lordOfRahuKetuSign;

                    //change current occupied rahu/ketu sign to the one occupied by the lord
                    planetSign = Calculate.PlanetZodiacSign(lordOfRahuKetuSign, birthTime).GetSignName();
                }


                //get relationship between planet & occupied sign 
                var houseSignRelation = Calculate.PlanetRelationshipWithSign(planet, planetSign, birthTime);

                //first set all friendship as not true
                bool planetInEnemy = false, planetInOwn = false, planetInNeutral = false, planetInFriendly = false;


                //based on the relationship between planet & occupied sign set the relationship
                switch (houseSignRelation)
                {
                    case PlanetToSignRelationship.OwnVarga:
                        planetInOwn = true;
                        break;
                    case PlanetToSignRelationship.BestFriendVarga:
                    case PlanetToSignRelationship.FriendVarga:
                        planetInFriendly = true;
                        break;
                    case PlanetToSignRelationship.NeutralVarga:
                        planetInNeutral = true;
                        break;
                    case PlanetToSignRelationship.EnemyVarga:
                    case PlanetToSignRelationship.BitterEnemyVarga:
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
            MatchPrediction interpretScore(double scoreMale, double scoreFemale)
            {
                var prediction = new MatchPrediction
                {
                    Name = MatchPredictionName.KujaDosa,
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
                    prediction.Info = "match is good, dosha in both are equal or close";
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

        public static MatchPrediction BadConstellations(Person male, Person female)
        {
            // Almost all authors agree that certain parts of Moola, Astesha, Jyeshta and Visakha are destructive
            // constellations -
            // Aslesha (first quarter) for husband's mother;
            // Jyeshta (first quarter) for girl's husband's elder brother;
            // and Visakha (last quarter) for husband's younger brother.


            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.BadConstellation,
                Description = "checks if evil constellation is in chart"
            };

            //get female constellation
            var femaleConstellation = Calculate.MoonConstellation(female.BirthTime);
            var maleConstellation = Calculate.MoonConstellation(male.BirthTime);

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

            //if no conditions above met, then it is good
            if (prediction.Info == "")
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "no evil constellation in either person";
            }

            return prediction;

        }

        public static MatchPrediction SexEnergy(Person male, Person female)
        {
            //When Mars and Venus are in the 7th, the boy or girl concerned will have strong sex instincts and
            //such an individual should be mated to one who has similar instincts and not a person having Mercury or Jupiter in the 7th,
            //as this makes one under-sexed. When sexual incompatibility sets in marriage,
            //life proves charmless and friction arises between the couple.

            var prediction = new MatchPrediction
            {
                Name = MatchPredictionName.SexEnergy,
                Description = "sexual compatibility based on planets in 7th house"
            };


            //get the data needed (borrow horoscope calculator)
            var maleStrongSex = CalculateHoroscope.MarsVenusIn7th(male.BirthTime).Occuring;
            var femaleStrongSex = CalculateHoroscope.MarsVenusIn7th(female.BirthTime).Occuring;
            var maleUnderSex = CalculateHoroscope.MercuryOrJupiterIn7th(male.BirthTime).Occuring;
            var femaleUnderSex = CalculateHoroscope.MercuryOrJupiterIn7th(female.BirthTime).Occuring;

            //fill extra info
            if (maleStrongSex) { prediction.MaleInfo = "Strong Sex"; }
            if (femaleStrongSex) { prediction.FemaleInfo = "Strong Sex"; }
            if (maleUnderSex) { prediction.MaleInfo = "Under-Sexed"; }
            if (femaleUnderSex) { prediction.FemaleInfo = "Under-Sexed"; }

            //both strong and under-sexed are present skip this prediction
            //note: this is just a precaution not to make any prediction
            //if conflict, this is open to improvement 
            var maleBoth = maleStrongSex && maleUnderSex;
            var femaleBoth = femaleStrongSex && femaleUnderSex;
            if (maleBoth || femaleBoth)
            {
                return prediction;
            }

            //check if occuring
            if (maleStrongSex && femaleStrongSex)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "both horoscope Mars & Venus In 7th (strong sex)";
            }

            if (maleUnderSex && femaleUnderSex)
            {
                prediction.Nature = EventNature.Good;
                prediction.Info = "both horoscope Mercury Or Jupiter In 7th (under-sexed)";
            }

            if (maleUnderSex && femaleStrongSex)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "male under-sexed & female strong sex";
            }

            if (maleStrongSex && femaleUnderSex)
            {
                prediction.Nature = EventNature.Bad;
                prediction.Info = "male strong sex & female under-sexed";
            }

            //if no conditions above met, then set as neutral
            if (prediction.Info == "")
            {
                prediction.Nature = EventNature.Neutral;
                prediction.Info = "no evil or good, neutral result";
            }


            return prediction;

        }
    }
}
