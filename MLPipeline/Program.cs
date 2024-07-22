using VedAstro.Library;

namespace MLPipeline
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //#create dataset
            var computedAstroColumns = new[]
            {
                CalculateHoroscope.House7LordInHouse7Occuring,
                CalculateHoroscope.Lord7And1Friends,
            };
            //Dictionary<BodyPrediction, Time> inputData = DatasetFactory.GetBodyPredictionInputData();

            //#train dataset

            //#make prediction

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
