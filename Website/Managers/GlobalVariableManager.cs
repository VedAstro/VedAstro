using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Website.Pages;
using Website.Shared;

namespace Website
{

    /// <summary>
    /// Place to store global data accessible by all
    /// </summary>
    public class GlobalVariableManager
    {
        public IJSRuntime JsRuntime { get; set; }
        public  NavigationManager Navigation { get; set; }


        //-----------------------------FIELDS

        /// <summary>
        /// Time spent waiting before checking if components are ready
        /// </summary>
        private const int ComponentWaitDelayMs = 10;




        //-----------------------------PROPERTIES

        /// <summary>
        /// Alert box showed at top of page, located in MainLayout
        /// </summary>
        public AlertMessage Alert { get; set; }

        /// <summary>
        /// LoadingMessage box showed over page when loading, located in MainLayout
        /// </summary>
        public LoadingMessage LoadingMessage { get; set; }



        //-----------------------------METHODS

        /// <summary>
        /// The current time in the system
        /// </summary>
        public DateTimeOffset SystemTimeNow => DateTimeOffset.Now;

        /// <summary>
        /// Hold the control till components have been loaded
        /// Poll time 10ms
        /// TODO CAN make as static tool
        /// </summary>
        public async Task WaitTillComponentReady()
        {
            //wait till every component needed is ready
            while (LoadingMessage == null && Alert == null)
            {
                Console.WriteLine("WaitForLoadingComplete");
                await Task.Delay(ComponentWaitDelayMs);
            }
        }

        /// <summary>
        /// Gets all people list from API server
        /// Todo basic cache mechanism
        /// </summary>
        public async Task<List<Person>> GetPeopleList()
        {
            var personListRootXml = await ServerManager.ReadFromServer(ServerManager.GetPersonListAPI);
            var personList = personListRootXml.Elements().Select(personXml => Person.FromXml(personXml)).ToList();

            return personList;
        }

        public async Task<List<Person>> GetMalePeopleList()
        {
            var rawMaleListXml = await ServerManager.ReadFromServer(ServerManager.GetMaleListAPI);
            return rawMaleListXml.Elements().Select(maleXml => Person.FromXml(maleXml)).ToList();
        }
        
        public async Task<List<Person>> GetFemalePeopleList()
        {
            var rawMaleListXml = await ServerManager.ReadFromServer(ServerManager.GetFemaleListAPI);
            return rawMaleListXml.Elements().Select(maleXml => Person.FromXml(maleXml)).ToList();
        }

        /// <summary>
        /// Gets person instance from name contacts API
        /// Note: uses API to get latest data
        /// </summary>
        public async Task<Person> GetPersonFromName(string name)
        {
            //send newly created person to API server
            var xmlData = Tools.AnyTypeToXml(name);
            var result = await ServerManager.WriteToServer(ServerManager.GetPersonAPI, xmlData);

            var personXml = result.Element("Person");

            //parse received person
            var receivedPerson = Person.FromXml(personXml);

            return receivedPerson;
        }


    }
}
