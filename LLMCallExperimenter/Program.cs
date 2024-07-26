using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VedAstro.Library;

namespace LLMCallExperimenter
{
    internal class Program
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("secrets.json", optional: true, reloadOnChange: true).Build();

        static string GPT4oEndpoint = config["GPT4oEndpoint"];
        static string GPT4oApiKey = config["GPT4oApiKey"];

        static string Phi3medium128kinstructEndpoint = config["Phi3medium128kinstructEndpoint"];
        static string Phi3medium128kinstructApiKey = config["Phi3medium128kinstructApiKey"];

        static string MistralNemo128kEndpoint = config["MistralNemo128kEndpoint"];
        static string MistralNemo128kApiKey = config["MistralNemo128kApiKey"];

        static string CohereCommandRPlusEndpoint = config["CohereCommandRPlusEndpoint"];
        static string CohereCommandRPlusApiKey = config["CohereCommandRPlusApiKey"];


        static HttpClient client;
        static List<ConversationMessage> conversationHistory = new List<ConversationMessage>();

        static async Task Main(string[] args)
        {
            string apiKey = CohereCommandRPlusApiKey;
            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(CohereCommandRPlusEndpoint);

            while (true)
            {
                Console.Write("You: ");
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    break;
                }

                conversationHistory.Add(new ConversationMessage { Role = "user", Content = userInput });

                string response = await SendMessageToLLM(client, conversationHistory);
                Console.WriteLine($"LLM: {response}\n");
            }
        }

        static async Task<string> SendMessageToLLM(HttpClient client, List<ConversationMessage> conversationHistory)
        {
            var messages = new List<object>
            {
                //new { role = "system", content = "expert programmer helper" },
                 new { role = "user", content = "Javascript ES6 class \n"+@"```

class PersonSelectorBox {
    ElementID = """";
    TitleText = ""Title Goes Here"";
    SelectedPersonNameHolderElementID = ""selectedPersonNameHolder"";
    SearchInputElementClass = ""searchInputElementClass"";

    constructor(elementId) {
        this.ElementID = elementId; //element that is heart 💖

        //default data
        this.personList = [];
        this.publicPersonList = [];
        this.personListDisplay = [];
        this.publicPersonListDisplay = [];

        //get title and description from the elements custom attributes
        const element = document.getElementById(elementId);
        this.TitleText = element.getAttribute(""title-text"") || ""Title Goes Here"";

        //save to global for access by parent
        if (!window.VedAstro) { window.VedAstro = {}; }
        if (!window.VedAstro.PersonSelectorBoxInstances) { window.VedAstro.PersonSelectorBoxInstances = []; }
        window.VedAstro.PersonSelectorBoxInstances[this.ElementID] = this;

        //runs needed async methods properly
        this.init();

    }

    async init() {
        // get person list data from API or local storage
        await this.initializePersonListData();

        // inject in html
        await this.initializeMainBody();
    }

    async initializeMainBody() {
        //clean if any old stuff
        $(`#${this.ElementID}`).empty();

        //inject into page
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());
    }

    async initializePersonListData() {

        //PRIVATE
        const personListResponse = await fetch(`${window.vedastro.ApiDomain}/Calculate/GetPersonList/UserId/${window.vedastro.UserId}`);
        const personList = await personListResponse.json();
        this.personList = personList.Payload;
        this.personListDisplay = personList.Payload;

        //PUBLIC
        const publicPersonListResponse = await fetch(`${window.vedastro.ApiDomain}/Calculate/GetPersonList/UserId/101`);
        const publicPersonList = await publicPersonListResponse.json();
        this.publicPersonList = publicPersonList.Payload;
        this.publicPersonListDisplay = publicPersonList.Payload;

        //GLOBAL SELECTED PERSON
        const storedSelectedPerson = localStorage.getItem(""selectedPerson"");
        const selectedPerson = JSON.parse(storedSelectedPerson);
        if (selectedPerson && Object.keys(selectedPerson).length !== 0) {

            this.onClickPersonName(selectedPerson.id); //simulate click on person
        }

    }

    //when user clicks on person name in dropdown
    async onClickPersonName(personId) {

        //get full person details for given name
        var personData = this.getPersonDataById(personId);
        var displayName = this.getPersonDisplayName(personData);

        //update visible select button text
        var buttonTextHolder = $(`#${this.ElementID}`).find(`.${this.SelectedPersonNameHolderElementID}`);
        buttonTextHolder.html(displayName);

        //save to local storage for future use
        localStorage.setItem(""selectedPerson"", JSON.stringify({ id: personId }));

        //save for instance specific selection
        this.selectedPersonId = personId;
    }

    //given person json will return birth year
    getPersonBirthYear(person) {
        const birthTime = person.BirthTime.StdTime; // 13:54 25/10/1992 +08:00
        const [time, date] = birthTime.split(' ');
        const [hours, minutes] = time.split(':');
        const [day, month, year] = date.split('/');
        const birthDate = new Date(`${year}-${month}-${day}T${hours}:${minutes}:00.000Z`);
        return birthDate.getFullYear();
    };

    //name with birth year
    getPersonDisplayName(person) {
        const name = person.Name;
        const birthYear = this.getPersonBirthYear(person); // reuse the previous function
        return `${name} - ${birthYear}`;
    };

    /**
    * Handles the keyup event on the search input field.
    * Filters the person lists based on the search text.
    *
    * @param {Event} event The keyup event object.
    */
    onKeyUpSearchBar = (event) => {

        // Ignore certain keys to prevent unnecessary filtering
        if ($.inArray(event.code, [
            'ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight',
            'Space', 'ControlLeft', 'ControlRight', 'AltLeft',
            'AltRight', 'ShiftLeft', 'ShiftRight', 'Enter',
            'Tab', 'Escape'
        ]) !== -1) {
            return;
        }

        // Get the search text from the input field
        const searchText = event.target.value.toLowerCase();

        // Filter the person lists based on the search text
        var allPersonDropItems = $(`#${this.ElementID}`).find('.dropdown-menu li');
        allPersonDropItems.each(function () {
            const personName = $(this).text().toLowerCase();
            if (personName.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    async generateHtmlBody() {

        //generate html for person list
        this.personListHTML = this.generatePersonListHtml();
        this.publicPersonListHTML = this.generatePublicPersonListHtml();

        this.searchInput = document.getElementById('searchInput');


        return `
    <div>
      <label class=""form-label"">${this.TitleText}</label>
      <div class=""hstack"">
        <div class=""btn-group"" style=""width:100%;"">
          <button onclick=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onClickDropDown(event)"" type=""button"" class=""btn dropdown-toggle btn-outline-primary text-start"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
            <div class=""${this.SelectedPersonNameHolderElementID}"" style=""cursor: pointer;white-space: nowrap; display: inline-table;"" >Select Person</div>
          </button>
          <ul class=""dropdown-menu ps-2 pe-3"" style=""height: 412.5px; overflow: clip scroll;"">

            <!--SEARCH INPUT-->
            <div class=""hstack gap-2"">
              <input onkeyup=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onKeyUpSearchBar(event)"" type=""text"" class=""${this.SearchInputElementClass} form-control ms-0 mb-2 ps-3"" placeholder=""Search..."">
              <div class=""mb-2"" style=""cursor: pointer;"">
                <i class=""iconify"" data-icon=""pepicons-pop:list"" data-width=""25""></i>
              </div>
            </div>

            <!-- PRIVATE PERSON LIST -->
            ${this.personListHTML}

            <li>
              <hr class=""dropdown-divider"">
            </li>
            <div class=""ms-3 d-flex justify-content-between"">
              
              <div class="" hstack gap-2"" style="" "">
                
                <div class="""" style="""" _bl_134="""">
                  <i class=""iconify"" data-icon=""material-symbols:demography-rounded"" data-width=""25""></i>
                </div>
                
                <span style=""font-size: 13px; color: rgb(143, 143, 143); --darkreader-inline-color: #cdc4b5;"" data-darkreader-inline-color="""">
                  Examples</span>
              </div>
            </div>

            <li>
              <hr class=""dropdown-divider"">
            </li>

            <!-- PUBLIC PERSON LIST -->
            ${this.publicPersonListHTML}

          </ul>
        </div>
        <button style="" height:37.1px; width: fit-content; font-family: 'Lexend Deca', serif !important;"" class=""iconOnlyButton btn-primary btn ms-2"" _bl_98="""">
          <i class=""iconify"" data-icon=""ant-design:user-add-outlined"" data-width=""25""></i>
        </button>
      </div>
    </div>
  `;
    }

    //gets full person data from given list
    getPersonDataById(personId) {
        // Search public list
        const person = this.publicPersonListDisplay.find((person) => person.PersonId === personId);

        // If not found, search private list
        if (!person) {
            const privatePerson = this.personListDisplay.find((person) => person.PersonId === personId);
            return privatePerson;
        }

        return person;
    }

    generatePublicPersonListHtml() {

        const html = this.publicPersonListDisplay
            .map((person) => {
                return `<li onClick=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')"" class=""dropdown-item"" style=""cursor: pointer;"">${this.getPersonDisplayName(person)}</li>`;
            })
            .join("""");

        return html;
    }

    generatePersonListHtml() {

        const html = this.personListDisplay
            .map((person) => {
                return `<li onClick=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')"" class=""dropdown-item"" style=""cursor: pointer;"">${this.getPersonDisplayName(person)}</li>`;
            })
            .join("""");

        return html;
    }

    //when user clicks on dropdown button (not items)
    onClickDropDown(event) {

        //set curosor to search text box, so can input instantly
        //NOTE: UX feature so can search faster
        $(`#${this.ElementID}`).find(`.${this.SearchInputElementClass}`).focus();

    }
}



```
"},
                new { role = "assistant", content = "ok, I've parsed the code, how may I help with it?" }
            };

            foreach (var message in conversationHistory)
            {
                messages.Add(new { role = message.Role, content = message.Content });
            }

            var requestBodyObject = new
            {
                messages,
                max_tokens = 32000,
                temperature = 0.5,
                top_p = 1,
                //presence_penalty = 0,
                //frequency_penalty = 0
            };

            var requestBody = JsonConvert.SerializeObject(requestBodyObject);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("", content);

            var fullReplyRaw = await response.Content.ReadAsStringAsync();
            var fullReply = new Phi3ReplyJson(fullReplyRaw);

            var replyMessage = fullReply?.Choices?.FirstOrDefault()?.Message.Content ?? "No response!!";

            //add in LLM's reply for next chat round
            conversationHistory.Add(new ConversationMessage { Role = "assistant", Content = replyMessage });

            return replyMessage;
        }
    }

    public class ConversationMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}

////TODO -------  OLD CODE MARKED FOR OBLIVION!!

//public static async Task<string> MakeCall01()
//{
//    var handler = new HttpClientHandler()
//    {
//        ClientCertificateOptions = ClientCertificateOption.Manual,
//        ServerCertificateCustomValidationCallback =
//                   (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
//    };


//    using (var client = new HttpClient(handler))
//    {
//        var requestBodyObject = new
//        {
//            messages = new[]
//            {
//                            //new { role = "system", content = "given person name & birth year output marriage data" },
//                            new { role = "system", content = "given person name & birth year output only JSON body data in valid ```json" +
//                                                             @"{
//          ""body"": {
//            ""type"": ""ectomorph/mesomorph/endomorph"",
//            ""breastSize"": ""small/medium/large"",
//            ""height"": ""short/average/tall"",
//            ""weight"": ""light/medium/heavy"",
//            ""dataCredibility"": ""low/medium/high"",
//          }
//        }```" },
//                            //new { role = "user", content = $"Marilyn Monroe born 1926"}
//                            new { role = "user", content = $"angelina jolie born 1975"}
//                            },

//            max_tokens = 4096,
//            temperature = 0.1,
//            top_p = 0.1,
//            presence_penalty = 0,
//            frequency_penalty = 0
//        };

//        var requestBody = JsonConvert.SerializeObject(requestBodyObject);

//        client.BaseAddress = new Uri(GPT4oEndpoint);

//        var content = new StringContent(requestBody);
//        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
//        content.Headers.Add("api-key", GPT4oApiKey);

//        var response = await client.PostAsync("", content);

//        //get full reply and parse it
//        var fullReplyRaw = await response.Content.ReadAsStringAsync();
//        var fullReply = new Gpt4ReplyJson(fullReplyRaw);

//        //return only message text
//        var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
//        return replyMessage;
//    }
//}

//public static async Task<string> MakeCall02()
//{
//    var handler = new HttpClientHandler()
//    {
//        ClientCertificateOptions = ClientCertificateOption.Manual,
//        ServerCertificateCustomValidationCallback =
//                   (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
//    };


//    using (var client = new HttpClient(handler))
//    {
//        var requestBodyObject = new
//        {
//            messages = new[]
//            {
//                            new { role = "system", content = "expert programmer, generate valid code" },
//                            new { role = "user", content = "using below c# code as reference, generate a c# console app that allows basic chat with llm" },
//                            new { role = "user", content = @"```csharp
//        // .NET Framework 4.7.1 or greater must be used

//        using System;
//        using System.Collections.Generic;
//        using System.IO;
//        using System.Net.Http;
//        using System.Net.Http.Headers;
//        using System.Text;
//        using System.Threading.Tasks;

//        namespace CallRequestResponseService
//        {
//            class Program
//            {
//                static void Main(string[] args)
//                {
//                    InvokeRequestResponseService().Wait();
//                }

//                static async Task InvokeRequestResponseService()
//                {
//                    var handler = new HttpClientHandler()
//                    {
//                        ClientCertificateOptions = ClientCertificateOption.Manual,
//                        ServerCertificateCustomValidationCallback =
//                                (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
//                    };
//                    using (var client = new HttpClient(handler))
//                    {
//                        // Request data goes here
//                        // The example below assumes JSON formatting which may be updated
//                        // depending on the format your endpoint expects.
//                        // More information can be found here:
//                        // https://docs.microsoft.com/azure/machine-learning/how-to-deploy-advanced-entry-script
//                        var requestBody = @""{
//                          """"messages"""": [
//                            {
//                              """"role"""": """"user"""",
//                              """"content"""": """"I am going to Paris, what should I see?""""
//                            },
//                            {
//                              """"role"""": """"assistant"""",
//                              """"content"""": """"Paris, the capital of France, is known for its stunning architecture, art museums, historical landmarks, and romantic atmosphere. Here are some of the top attractions to see in Paris:\n\n1. The Eiffel Tower: The iconic Eiffel Tower is one of the most recognizable landmarks in the world and offers breathtaking views of the city.\n2. The Louvre Museum: The Louvre is one of the world's largest and most famous museums, housing an impressive collection of art and artifacts, including the Mona Lisa.\n3. Notre-Dame Cathedral: This beautiful cathedral is one of the most famous landmarks in Paris and is known for its Gothic architecture and stunning stained glass windows.\n\nThese are just a few of the many attractions that Paris has to offer. With so much to see and do, it's no wonder that Paris is one of the most popular tourist destinations in the world.""""
//                            },
//                            {
//                              """"role"""": """"user"""",
//                              """"content"""": """"What is so great about #1?""""
//                            }
//                          ],
//                          """"max_tokens"""": 1024,
//                          """"temperature"""": 0.7,
//                          """"top_p"""": 1,
//                          """"stream"""": false
//                        }"";

//                        // Replace this with the primary/secondary key, AMLToken, or Microsoft Entra ID token for the endpoint
//                        const string apiKey = """";
//                        if (string.IsNullOrEmpty(apiKey))
//                        {
//                            throw new Exception(""A key should be provided to invoke the endpoint"");
//                        }
//                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( ""Bearer"", apiKey);
//                        client.BaseAddress = new Uri(""https://Phi-3-medium-128k-instruct.eastus2.models.ai.azure.com/v1/chat/completions"");

//                        var content = new StringContent(requestBody);
//                        content.Headers.ContentType = new MediaTypeHeaderValue(""application/json"");

//                        // WARNING: The 'await' statement below can result in a deadlock
//                        // if you are calling this code from the UI thread of an ASP.Net application.
//                        // One way to address this would be to call ConfigureAwait(false)
//                        // so that the execution does not attempt to resume on the original context.
//                        // For instance, replace code such as:
//                        //      result = await DoSomeTask()
//                        // with the following:
//                        //      result = await DoSomeTask().ConfigureAwait(false)
//                        HttpResponseMessage response = await client.PostAsync("""", content);

//                        if (response.IsSuccessStatusCode)
//                        {
//                            string result = await response.Content.ReadAsStringAsync();
//                            Console.WriteLine(""Result: {0}"", result);
//                        }
//                        else
//                        {
//                            Console.WriteLine(string.Format(""The request failed with status code: {0}"", response.StatusCode));

//                            // Print the headers - they include the requert ID and the timestamp,
//                            // which are useful for debugging the failure
//                            Console.WriteLine(response.Headers.ToString());

//                            string responseContent = await response.Content.ReadAsStringAsync();
//                            Console.WriteLine(responseContent);
//                        }
//                    }
//                }
//            }
//        }
//        ```
//        "}
//                            },

//            max_tokens = 30000,
//            temperature = 0.7,
//            top_p = 1,
//            //presence_penalty = 0,
//            //frequency_penalty = 0
//        };

//        var requestBody = JsonConvert.SerializeObject(requestBodyObject);

//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MistralNemo128kApiKey);
//        client.BaseAddress = new Uri(MistralNemo128kEndpoint);
//        client.Timeout = Timeout.InfiniteTimeSpan;

//        var content = new StringContent(requestBody);
//        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//        var response = await client.PostAsync("", content);

//        //get full reply and parse it
//        var fullReplyRaw = await response.Content.ReadAsStringAsync();
//        var fullReply = new Phi3ReplyJson(fullReplyRaw);

//        //return only message text
//        var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
//        return replyMessage;
//    }
//}

//public static async Task<string> MakeCall03()
//{
//    var handler = new HttpClientHandler()
//    {
//        ClientCertificateOptions = ClientCertificateOption.Manual,
//        ServerCertificateCustomValidationCallback =
//                   (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
//    };


//    using (var client = new HttpClient(handler))
//    {
//        var requestBodyObject = new
//        {
//            messages = new[]
//            {
//                        new { role = "user", content = "improve Javascript ES6 class code quality & readability, use comments\n"+@"```

//        class PersonSelectorBox {
//            ElementID = """";
//            TitleText = ""Title Goes Here"";
//            SelectedPersonNameHolderElementID = ""selectedPersonNameHolder"";
//            SearchInputElementClass = ""searchInputElementClass"";

//            constructor(elementId) {
//                this.ElementID = elementId; //element that is heart 💖

//                //default data
//                this.personList = [];
//                this.publicPersonList = [];
//                this.personListDisplay = [];
//                this.publicPersonListDisplay = [];

//                //get title and description from the elements custom attributes
//                const element = document.getElementById(elementId);
//                this.TitleText = element.getAttribute(""title-text"") || ""Title Goes Here"";

//                //save to global for access by parent
//                if (!window.VedAstro) { window.VedAstro = {}; }
//                if (!window.VedAstro.PersonSelectorBoxInstances) { window.VedAstro.PersonSelectorBoxInstances = []; }
//                window.VedAstro.PersonSelectorBoxInstances[this.ElementID] = this;

//                //runs needed async methods properly
//                this.init();

//            }

//            async init() {
//                // get person list data from API or local storage
//                await this.initializePersonListData();

//                // inject in html
//                await this.initializeMainBody();
//            }

//            async initializeMainBody() {
//                //clean if any old stuff
//                $(`#${this.ElementID}`).empty();

//                //inject into page
//                $(`#${this.ElementID}`).html(await this.generateHtmlBody());
//            }

//            async initializePersonListData() {

//                //PRIVATE
//                const personListResponse = await fetch(`${window.vedastro.ApiDomain}/Calculate/GetPersonList/UserId/${window.vedastro.UserId}`);
//                const personList = await personListResponse.json();
//                this.personList = personList.Payload;
//                this.personListDisplay = personList.Payload;

//                //PUBLIC
//                const publicPersonListResponse = await fetch(`${window.vedastro.ApiDomain}/Calculate/GetPersonList/UserId/101`);
//                const publicPersonList = await publicPersonListResponse.json();
//                this.publicPersonList = publicPersonList.Payload;
//                this.publicPersonListDisplay = publicPersonList.Payload;

//                //GLOBAL SELECTED PERSON
//                const storedSelectedPerson = localStorage.getItem(""selectedPerson"");
//                const selectedPerson = JSON.parse(storedSelectedPerson);
//                if (selectedPerson && Object.keys(selectedPerson).length !== 0) {

//                    this.onClickPersonName(selectedPerson.id); //simulate click on person
//                }

//            }

//            //when user clicks on person name in dropdown
//            async onClickPersonName(personId) {

//                //get full person details for given name
//                var personData = this.getPersonDataById(personId);
//                var displayName = this.getPersonDisplayName(personData);

//                //update visible select button text
//                var buttonTextHolder = $(`#${this.ElementID}`).find(`.${this.SelectedPersonNameHolderElementID}`);
//                buttonTextHolder.html(displayName);

//                //save to local storage for future use
//                localStorage.setItem(""selectedPerson"", JSON.stringify({ id: personId }));

//                //save for instance specific selection
//                this.selectedPersonId = personId;
//            }

//            //given person json will return birth year
//            getPersonBirthYear(person) {
//                const birthTime = person.BirthTime.StdTime; // 13:54 25/10/1992 +08:00
//                const [time, date] = birthTime.split(' ');
//                const [hours, minutes] = time.split(':');
//                const [day, month, year] = date.split('/');
//                const birthDate = new Date(`${year}-${month}-${day}T${hours}:${minutes}:00.000Z`);
//                return birthDate.getFullYear();
//            };

//            //name with birth year
//            getPersonDisplayName(person) {
//                const name = person.Name;
//                const birthYear = this.getPersonBirthYear(person); // reuse the previous function
//                return `${name} - ${birthYear}`;
//            };

//            /**
//            * Handles the keyup event on the search input field.
//            * Filters the person lists based on the search text.
//            *
//            * @param {Event} event The keyup event object.
//            */
//            onKeyUpSearchBar = (event) => {

//                // Ignore certain keys to prevent unnecessary filtering
//                if ($.inArray(event.code, [
//                    'ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight',
//                    'Space', 'ControlLeft', 'ControlRight', 'AltLeft',
//                    'AltRight', 'ShiftLeft', 'ShiftRight', 'Enter',
//                    'Tab', 'Escape'
//                ]) !== -1) {
//                    return;
//                }

//                // Get the search text from the input field
//                const searchText = event.target.value.toLowerCase();

//                // Filter the person lists based on the search text
//                var allPersonDropItems = $(`#${this.ElementID}`).find('.dropdown-menu li');
//                allPersonDropItems.each(function () {
//                    const personName = $(this).text().toLowerCase();
//                    if (personName.includes(searchText)) {
//                        $(this).show();
//                    } else {
//                        $(this).hide();
//                    }
//                });
//            }

//            async generateHtmlBody() {

//                //generate html for person list
//                this.personListHTML = this.generatePersonListHtml();
//                this.publicPersonListHTML = this.generatePublicPersonListHtml();

//                this.searchInput = document.getElementById('searchInput');

//                return `
//                <div>
//                  <label class=""form-label"">${this.TitleText}</label>
//                  <div class=""hstack"">
//                    <div class=""btn-group"" style=""width:100%;"">
//                      <button onclick=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onClickDropDown(event)"" type=""button"" class=""btn dropdown-toggle btn-outline-primary text-start"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
//                        <div class=""${this.SelectedPersonNameHolderElementID}"" style=""cursor: pointer;white-space: nowrap; display: inline-table;"" >Select Person</div>
//                      </button>
//                      <ul class=""dropdown-menu ps-2 pe-3"" style=""height: 412.5px; overflow: clip scroll;"">

//                        <!--SEARCH INPUT-->
//                        <div class=""hstack gap-2"">
//                          <input onkeyup=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onKeyUpSearchBar(event)"" type=""text"" class=""${this.SearchInputElementClass} form-control ms-0 mb-2 ps-3"" placeholder=""Search..."">
//                          <div class=""mb-2"" style=""cursor: pointer;"">
//                            <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" aria-hidden=""true"" role=""img"" class=""iconify iconify--pepicons-pop"" width=""25"" height=""25"" preserveAspectRatio=""xMidYMid meet"" viewBox=""0 0 20 20"" data-icon=""pepicons-pop:list"" data-width=""25"">
//                              <g fill=""currentColor"" data-darkreader-inline-fill="""" style=""--darkreader-inline-fill: currentColor;"">
//                                <path d=""M6.5 6a1.5 1.5 0 1 1-3 0a1.5 1.5 0 0 1 3 0m0 4a1.5 1.5 0 1 1-3 0a1.5 1.5 0 0 1 3 0m0 4a1.5 1.5 0 1 1-3 0a1.5 1.5 0 0 1 3 0"">
//                                </path>
//                                <path fill-rule=""evenodd"" d=""M7.5 6a1 1 0 0 1 1-1h7a1 1 0 1 1 0 2h-7a1 1 0 0 1-1-1m0 4a1 1 0 0 1 1-1h7a1 1 0 1 1 0 2h-7a1 1 0 0 1-1-1m0 4a1 1 0 0 1 1-1h7a1 1 0 1 1 0 2h-7a1 1 0 0 1-1-1"" clip-rule=""evenodd"">
//                                </path>
//                              </g>
//                            </svg>
//                          </div>
//                        </div>


//                        <!-- PRIVATE PERSON LIST -->
//                        ${this.personListHTML}

//                        <li>
//                          <hr class=""dropdown-divider"">
//                        </li>
//                        <div class=""ms-3 d-flex justify-content-between"">

//                          <div class="" hstack gap-2"" style="" "">

//                            <div class="""" style="""" _bl_134="""">
//                              <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" aria-hidden=""true"" role=""img"" class=""iconify iconify--material-symbols"" width=""25"" height=""25"" preserveAspectRatio=""xMidYMid meet"" viewBox=""0 0 24 24"" data-icon=""material-symbols:demography-rounded"" data-width=""25"">
//                                <path fill=""currentColor"" d=""M18 18q.625 0 1.063-.437T19.5 16.5t-.437-1.062T18 15t-1.062.438T16.5 16.5t.438 1.063T18 18m0 3q.75 0 1.4-.35t1.075-.975q-.575-.35-1.2-.513T18 19t-1.275.162t-1.2.513q.425.625 1.075.975T18 21m0 2q-2.075 0-3.537-1.463T13 18t1.463-3.537T18 13t3.538 1.463T23 18t-1.463 3.538T18 23M8 9h8q.425 0 .713-.288T17 8t-.288-.712T16 7H8q-.425 0-.712.288T7 8t.288.713T8 9M5 21q-.825 0-1.412-.587T3 19V5q0-.825.588-1.412T5 3h14q.825 0 1.413.588T21 5v5.45q0 .45-.375.7t-.8.1q-.425-.125-.888-.188T18 11q-.275 0-.513.013t-.487.062q-.225-.05-.5-.062T16 11H8q-.425 0-.712.288T7 12t.288.713T8 13h5.125q-.45.425-.812.925T11.675 15H8q-.425 0-.712.288T7 16t.288.713T8 17h3.075q-.05.25-.062.488T11 18q0 .5.05.95t.175.875t-.125.8t-.675.375z"" data-darkreader-inline-fill="""" style=""--darkreader-inline-fill: currentColor;"">
//                                </path>
//                              </svg>
//                            </div>

//                            <span style=""font-size: 13px; color: rgb(143, 143, 143); --darkreader-inline-color: #cdc4b5;"" data-darkreader-inline-color="""">
//                              Examples</span>
//                          </div>
//                        </div>

//                        <li>
//                          <hr class=""dropdown-divider"">
//                        </li>

//                        <!-- PUBLIC PERSON LIST -->
//                        ${this.publicPersonListHTML}

//                      </ul>
//                    </div>
//                    <button style="" height:37.1px; width: fit-content; font-family: 'Lexend Deca', serif !important;"" class=""iconOnlyButton btn-primary btn ms-2"" _bl_98="""">
//                      <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" aria-hidden=""true"" role=""img"" class=""iconify iconify--ant-design"" width=""25"" height=""25"" preserveAspectRatio=""xMidYMid meet"" viewBox=""0 0 1024 1024"" data-icon=""ant-design:user-add-outlined"" data-width=""25"">
//                        <path fill=""currentColor"" d=""M678.3 642.4c24.2-13 51.9-20.4 81.4-20.4h.1c3 0 4.4-3.6 2.2-5.6a371.7 371.7 0 0 0-103.7-65.8c-.4-.2-.8-.3-1.2-.5C719.2 505 759.6 431.7 759.6 349c0-137-110.8-248-247.5-248S264.7 212 264.7 349c0 82.7 40.4 156 102.6 201.1c-.4.2-.8.3-1.2.5c-44.7 18.9-84.8 46-119.3 80.6a373.4 373.4 0 0 0-80.4 119.5A373.6 373.6 0 0 0 137 888.8a8 8 0 0 0 8 8.2h59.9c4.3 0 7.9-3.5 8-7.8c2-77.2 32.9-149.5 87.6-204.3C357 628.2 432.2 597 512.2 597c56.7 0 111.1 15.7 158 45.1a8.1 8.1 0 0 0 8.1.3M512.2 521c-45.8 0-88.9-17.9-121.4-50.4A171.2 171.2 0 0 1 340.5 349c0-45.9 17.9-89.1 50.3-121.6S466.3 177 512.2 177s88.9 17.9 121.4 50.4A171.2 171.2 0 0 1 683.9 349c0 45.9-17.9 89.1-50.3 121.6C601.1 503.1 558 521 512.2 521M880 759h-84v-84c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v84h-84c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h84v84c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8v-84h84c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8"" data-darkreader-inline-fill="""" style=""--darkreader-inline-fill: currentColor;"">
//                        </path>
//                      </svg>
//                    </button>
//                  </div>
//                </div>
//             `;
//            }

//            //gets full person data from given list
//            getPersonDataById(personId) {
//                // Search public list
//                const person = this.publicPersonListDisplay.find((person) => person.PersonId === personId);

//                // If not found, search private list
//                if (!person) {
//                    const privatePerson = this.personListDisplay.find((person) => person.PersonId === personId);
//                    return privatePerson;
//                }

//                return person;
//            }

//            generatePublicPersonListHtml() {

//                const html = this.publicPersonListDisplay
//                    .map((person) => {
//                        return `<li onClick=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')"" class=""dropdown-item"" style=""cursor: pointer;"">${this.getPersonDisplayName(person)}</li>`;
//                    })
//                    .join("""");

//                return html;
//            }

//            generatePersonListHtml() {

//                const html = this.personListDisplay
//                    .map((person) => {
//                        return `<li onClick=""window.VedAstro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')"" class=""dropdown-item"" style=""cursor: pointer;"">${this.getPersonDisplayName(person)}</li>`;
//                    })
//                    .join("""");

//                return html;
//            }

//            //when user clicks on dropdown button (not items)
//            onClickDropDown(event) {

//                //set curosor to search text box, so can input instantly
//                //NOTE: UX feature so can search faster
//                $(`#${this.ElementID}`).find(`.${this.SearchInputElementClass}`).focus();

//            }
//        }


//        ```
//        "}
//                    },

//            max_tokens = 30000,
//            temperature = 0.6,
//            top_p = 1,
//            //presence_penalty = 0,
//            //frequency_penalty = 0
//        };

//        var requestBody = JsonConvert.SerializeObject(requestBodyObject);

//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Phi3medium128kinstructApiKey);
//        client.BaseAddress = new Uri(Phi3medium128kinstructEndpoint);
//        client.Timeout = Timeout.InfiniteTimeSpan;

//        var content = new StringContent(requestBody);
//        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//        var response = await client.PostAsync("", content);

//        //get full reply and parse it
//        var fullReplyRaw = await response.Content.ReadAsStringAsync();
//        var fullReply = new Phi3ReplyJson(fullReplyRaw);

//        //return only message text
//        var replyMessage = fullReply?.Choices?.FirstOrDefault()?.Message.Content ?? "Nothing sir!";
//        return replyMessage;
//    }
//}
