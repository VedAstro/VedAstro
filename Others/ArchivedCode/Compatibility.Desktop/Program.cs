using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;

namespace Compatibility
{
    internal class Program
    {
        const string dataPersonlistXml = "data\\PersonList.xml";

        static void Main(string[] args)
        {
            //get all the people
            var peopleList = DatabaseManager.GetPersonList(dataPersonlistXml);

            //filter out the male and female ones we want
            var maleName = "Rubeshen";
            var femaleName = "Dhiviya";
            var male = peopleList.Find(person => person.GetName() == maleName);
            var female = peopleList.Find(person => person.GetName() == femaleName);

            //var geoLocation = new GeoLocation("Ipoh", 101, 4.59); //todo check if change in location changes much

            //var stdTimeMale = DateTimeOffset.ParseExact("23:33 19/03/1989 +08:00", Time.DateTimeFormat, null);
            //var stdTimeFemale = DateTimeOffset.ParseExact("10:27 14/02/1995 +08:00", Time.DateTimeFormat, null);

            //var male = new Person("Male", new Time(stdTimeMale, geoLocation));
            //var female = new Person("Female", new Time(stdTimeFemale, geoLocation));


            PrintOneVsOne(male, female);
            //PrintOneVsList(female);

        }


        private static void PrintOneVsOne(Person male, Person female)
        {
            var report =  MatchCalculator.GetCompatibilityReport(male, female);

            var maleName = male.GetName();
            var femaleName = female.GetName();

            printResult(ref report);



            //FUNCTIONS
            void printResult(ref CompatibilityReport report)
            {
                var list = report.PredictionList;

                //print header
                var maleYear = male.BirthYear;
                var femaleYear = female.BirthYear;
                Console.WriteLine($"{maleName}-{maleYear} <> {femaleName}-{femaleYear}");
                Console.WriteLine("Name#Nature#Description#Extra Info#Male#Female#");

                //print rows
                foreach (var prediction in list)
                {
                    //if prediction is empty, than skip it
                    if (prediction.Name == MatchPredictionName.Empty) { continue; }
                    Console.WriteLine($"{prediction.Name}#{prediction.Nature}#{prediction.Description}#{prediction.Info}#{prediction.MaleInfo}#{prediction.FemaleInfo}");
                }

                Console.WriteLine($"Total Score#{getScoreGrade(report.KutaScore)}#Total score must be above 50%#Score: {report.KutaScore}/100##");

                Console.ReadLine();
            }

        }


        private static void PrintOneVsList(Person person)
        {

            //get all the people
            var peopleList = DatabaseManager.GetPersonList(dataPersonlistXml);

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
                    var report = MatchCalculator.GetCompatibilityReport(inputPerson, personMatch);
                    returnList.Add(report);
                }

                if (inputPersonIsFemale && personMatchIsMale)
                {
                    //add report to list
                    var report = MatchCalculator.GetCompatibilityReport(personMatch, inputPerson);
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
                    var report = MatchCalculator.GetCompatibilityReport(male, female);
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




    }

}


