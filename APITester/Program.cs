namespace APITester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TARGET SERVER
            string SubDomain = "vedastroapi";
            //string SubDomain = "vedastroapi";
            //string SubDomain = "vedicastrogpt";

            //
            string LocalAPIServer = $"https://{SubDomain}.azurewebsites.net/api/";
            //string LocalAPIServer = "http://localhost:7071/api/";
            var appInstance = new TestMethods(LocalAPIServer);

            //PAUSE BEFORE TEST START
            Console.WriteLine("Press ENTER to start...");
            Console.ReadLine();


        //RUN TESTS
        RUN_TEST:
            Console.WriteLine("###### GEOLOCATION TEST START ######\n");
            CoalesceException(() => appInstance.GeoLocationToTimezoneTest().Result, null);
            CoalesceException(() => appInstance.AddressToGeoLocationTest().Result, null);
            CoalesceException(() => appInstance.CoordinatesToGeoLocationTest().Result, null);
            CoalesceException(() => appInstance.IpAddressToGeoLocationTest().Result, null);

            Console.WriteLine("###### DATA TEST START ######\n");
            CoalesceException(() => appInstance.GeoLocationToTimezoneTest().Result, null);
            CoalesceException(() => appInstance.AddressToGeoLocationTest().Result, null);
            CoalesceException(() => appInstance.CoordinatesToGeoLocationTest().Result, null);
            CoalesceException(() => appInstance.IpAddressToGeoLocationTest().Result, null);

            Console.WriteLine("###### MISSION CRITICAL ASTRO TEST ######\n");
            CoalesceException(() => appInstance.AllHouseData().Result, null);
            CoalesceException(() => appInstance.DasaAtRangeTest().Result, null);
            CoalesceException(() => appInstance.AllPlanetDataTest().Result, null);


            //HOLD CONTROL
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("###### TEST COMPLETE ######");
            Console.WriteLine(".....press ENTER to test again");
            Console.ReadLine();

            //rerun test without restart app
            goto RUN_TEST;
        }


        //wraps try catch into 1 line (AI made 🤖)
        public static void CoalesceException<T>(Func<T> expression, T defaultValue)
        {
            try
            {
                dynamic xx = expression();

                //print call URL to id
                Console.WriteLine($"## CALL URL --> {xx.URL}");
                Console.WriteLine($"## OUTPUT :\n{xx.OUTPUT}");//print everything
                Console.WriteLine("---------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {

                //Console.WriteLine(ex.InnerException?.InnerException?.InnerException?.Message);
                //Console.WriteLine(ex.InnerException?.InnerException?.Message);
                //Console.WriteLine(ex.InnerException?.Message);
                Console.WriteLine(ex.Message);

            }
        }

    }
}
