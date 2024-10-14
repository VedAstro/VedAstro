//-----------------------------> HOROSCOPE PAGE
updateHistory();

new PageTopNavbar("PageTopNavbar");
new DesktopSidebar("DesktopSidebarHolder");
new PageHeader("HoroscopePageHeader");
var horoscopePersonSelector = new PersonSelectorBox("PersonSelectorBox_Horoscope");
var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox");
var strengthChart = new StrengthChart("StrengthChartHolder");
new IconButton("IconButton_Calculate_Horoscope");
new IconButton("IconButton_Advanced_Horoscope");

new InfoBox("InfoBox_AskAI_Horoscope");
new InfoBox("InfoBox_EasyImport_Horoscope");
new InfoBox("InfoBox_ForgotenTime_Horoscope");


function OnClickAdvanced_Horoscope() {
    smoothSlideToggle('#HoroscopeAdvancedInputHolder');
}

async function OnClickCalculate_Horoscope() {

    //show loading to user
    CommonTools.ShowLoading();

    //make output holder visible
    $("#outputHoroscope").show();

    //hide sidebar links for nice clean fit (UX improvement)
    $("#SidebarInfoBoxHolder").slideUp(500);

    //get full data of selected person
    let selectedPerson = await horoscopePersonSelector.getSelectedPerson();

    //update page title with person name to for easy multi tab use (UX ease)
    document.title = `${selectedPerson.DisplayName} | Horoscope`;

    //get birth time of selected person (URL format)
    var timeUrl = selectedPerson.BirthTime.ToUrl();
    timeUrl = timeUrl.substring(1) + "/"; //remove leading / and add trailing / (minor format correction)

    //generate tables and charts
    await initHoroscopeChart(timeUrl);
    await initStrengthChart(timeUrl);
    await initPlanetDataTable(timeUrl);
    await initHouseDataTable(timeUrl);
    await initAshtakvargaTable(timeUrl);

    //hide loading
    Swal.close();
}

async function initHoroscopeChart(birthTimeUrl) {

    var settingsHoroscopeChat = {
        ElementID: "HoroscopeChat",
        ShowHeader: true,
        HeaderIcon: "fluent:table-28-filled",
        SelectedBirthTime: birthTimeUrl,
        //Ayanamsa: ayanamsaSelector.SelectedAyanamsa //NOTE: hard set to Raman in Server since all predictions are from Raman 
    };

    //note: on init, chat instance is loaded into window.vedastro.horoscopechat
    new HoroscopeChat(settingsHoroscopeChat);
}
async function initStrengthChart(birthTimeUrl) {

    //data used to generate table
    var inputArguments = {
        TimeUrl: birthTimeUrl,
        Ayanamsa: ayanamsaSelector.SelectedAyanamsa
    };

    strengthChart.GenerateChart(inputArguments);
}

async function initPlanetDataTable(birthTimeUrl) {

    //----------------------PLANET DATA----------------------------
    var planetColumns = [
        { Api: "PlanetZodiacSign", Enabled: true, Name: "Sign" },
        { Api: "PlanetConstellation", Enabled: true, Name: "Constellation" },
        { Api: "HousePlanetOccupies", Enabled: true, Name: "Occupies" },
        { Api: "HousesOwnedByPlanet", Enabled: true, Name: "Owns" },
        { Api: "PlanetLordOfZodiacSign", Enabled: true, Name: "Sign Lord" },
        { Api: "PlanetLordOfConstellation", Enabled: true, Name: "Const. Lord" },
        { Api: "Empty", Enabled: false, Name: "Empty" },
        { Api: "Empty", Enabled: false, Name: "Empty" },
    ];


    //initialize astro table
    var settings = {
        ElementID: "PlanetDataTable",
        KeyColumn: "Planet",
        ShowHeader: true,
        HeaderIcon: "twemoji:ringed-planet",
        ColumnData: planetColumns, //columns names to create table
        EnableSorting: false,
        SaveSettings: false,
    };

    //make new astro table
    var planetDataTable = new AstroTable(settings);

    //data used to generate table
    var inputArguments = {
        TimeUrl: birthTimeUrl,
        HoraryNumber: 0,
        RotateDegrees: 0,
        Ayanamsa: ayanamsaSelector.SelectedAyanamsa
    };

    //generate table
    await planetDataTable.GenerateTable(inputArguments);

}

async function initHouseDataTable(birthTimeUrl) {
    //----------------------HOUSE DATA----------------------------
    var houseColumns = [
        { Api: "HouseZodiacSign", Enabled: true, Name: "Sign" },
        { Api: "HouseConstellation", Enabled: true, Name: "Constellation" },
        { Api: "PlanetsInHouse", Enabled: true, Name: "Planets In" },
        { Api: "LordOfHouse", Enabled: true, Name: "House Lord" },
        { Api: "HouseConstellationLord", Enabled: true, Name: "Const. Lord" },
        { Api: "PlanetsAspectingHouse", Enabled: true, Name: "Aspects" },
        { Api: "Empty", Enabled: false, Name: "Empty" },
        { Api: "Empty", Enabled: false, Name: "Empty" },
    ];


    //initialize astro table
    var settings = {
        ElementID: "HouseDataTable",
        KeyColumn: "House",
        ShowHeader: true,
        HeaderIcon: "fluent-emoji-flat:house",
        ColumnData: houseColumns, //columns names to create table
        EnableSorting: false,
        SaveSettings: false,
    };

    //make new astro table
    var houseDataTable = new AstroTable(settings);

    //data used to generate table
    var inputArguments = {
        TimeUrl: birthTimeUrl, 
        HoraryNumber: 0,
        RotateDegrees: 0,
        Ayanamsa: ayanamsaSelector.SelectedAyanamsa
    };

    //generate table
    await houseDataTable.GenerateTable(inputArguments);

}

async function initAshtakvargaTable(birthTimeUrl) {

    //initialize astro table
    var settings = {
        ElementID: "AshtakvargaTable",
        KeyColumn: "Ashtakvarga",
        ShowHeader: true,
        HeaderIcon: "fluent:table-28-filled"
    };

    // Initialize astro table
    var ashtakvargaTable = new AshtakvargaTable(settings);

    //data used to generate table
    var inputArguments = {
        TimeUrl: birthTimeUrl,
        Ayanamsa: ayanamsaSelector.SelectedAyanamsa
    };

    // Generate table
    await ashtakvargaTable.GenerateTable(inputArguments);

}
