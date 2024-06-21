//LOGIN DATA

//# LOCAL <--> LIVE Switch
window.vedastro.ApiDomain = "https://vedastroapi.azurewebsites.net/api";
//window.vedastro.ApiDomain = "http://localhost:7071/api";

window.vedastro.Ayanamsa = "KRISHNAMURTI"; //default to KP
window.vedastro.ChartStyle = "South"; //default to South Indian Chart



////----------------------PLANET DATA----------------------------
//var planetColumns = [
//    { Api: "PlanetZodiacSign", Enabled: true, Name: "Sign" },
//    { Api: "PlanetConstellation", Enabled: true, Name: "Star" },
//    { Api: "HousePlanetOccupiesKP", Enabled: true, Name: "Occupies" },
//    { Api: "HousesOwnedByPlanetKP", Enabled: true, Name: "Owns" },
//    { Api: "PlanetLordOfZodiacSign", Enabled: true, Name: "Sign Lord" },
//    { Api: "PlanetLordOfConstellation", Enabled: true, Name: "Star Lord" },
//    { Api: "PlanetSubLordKP", Enabled: true, Name: "Sub Lord" },
//    { Api: "Empty", Enabled: false, Name: "Empty" },
//    { Api: "Empty", Enabled: false, Name: "Empty" },
//];

////initialize astro table
//var settings = {
//    ElementID: "PlanetDataTable",
//    KeyColumn: "Planet",
//    ShowHeader: true,
//    HeaderIcon: "twemoji:ringed-planet",
//    ColumnData: planetColumns, //columns names to create table
//    EnableSorting: false,
//    SaveSettings: false,
//};

////make new astro table
//var planetDataTable = new AstroTable(settings);

////data used to generate table
//var inputArguments = {
//    TimeUrl: "Location/Bengaluru/Time/11:00/25/07/1984/+00:00/",
//    HoraryNumber: 0,
//    RotateDegrees: 0,
//    Ayanamsa: "KRISHNAMURTI", //default is Lahiri
//};

////generate table
//planetDataTable.GenerateTable(inputArguments);

////----------------------Ashtakvarga Table----------------------------
//var settingsAshtakvarga = {
//    ElementID: "AshtakvargaTable",
//    KeyColumn: "Ashtakvarga",
//    ShowHeader: true,
//    HeaderIcon: "fluent:table-28-filled",
//};

//var ashtakvargaTable = new AshtakvargaTable(settingsAshtakvarga);

////data used to generate table
//var inputArguments = {
//    TimeUrl: "Location/Bengaluru/Time/11:00/25/07/1984/+00:00/",
//    Ayanamsa: "KRISHNAMURTI", //default is Lahiri
//};

//ashtakvargaTable.GenerateTable(inputArguments);

////----------------------AI CHAT----------------------------
//var settingsAIChat = {
//    ElementID: "AIChat",
//    ShowHeader: true,
//    HeaderIcon: "fluent:table-28-filled",
//};

////note: on init, chat instance is loaded into window.vedastro.chatapi
//new ChatInstance(settingsAIChat);
//window.vedastro.chatapi.waitForConnection();


//----------------------HOROSCOPE CHAT----------------------------
//var settingsHoroscopeChat = {
//    ElementID: "HoroscopeChat",
//    ShowHeader: true,
//    HeaderIcon: "fluent:table-28-filled",
//    /*SelectedBirthTime: "Location/Ipoh/Time/12:44/23/04/1994/+08:00"*/
//};

////note: on init, chat instance is loaded into window.vedastro.horoscopechat
//new HoroscopeChat(settingsHoroscopeChat);
//window.vedastro.horoscopechat.waitForConnection();
