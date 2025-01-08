updateHistory();

new PageHeader("PageHeader");

var horoscopePersonSelector = new PersonSelectorBox("PersonSelectorBox");
var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox", "RAMAN");
var strengthChart = new StrengthChart("StrengthChartHolder");
var indianChart = new IndianChart("IndianChartHolder", 'South', ['RasiD1', 'NavamshaD9']);

var allPlanetDataTable = new AllAstroDataTable("PlanetDataTable", "Planet", "twemoji:ringed-planet", ['PlanetZodiacSign', 'PlanetConstellation', 'HousePlanetOccupiesBasedOnSign', 'HousesOwnedByPlanet', 'PlanetLordOfZodiacSign', 'PlanetLordOfConstellation',]);

var allHouseDataTable = new AllAstroDataTable("HouseDataTable", "House", "fluent-emoji-flat:house", ['HouseZodiacSign', 'HouseConstellation', 'PlanetsInHouse', 'LordOfHouse', 'HouseConstellationLord', 'PlanetsAspectingHouse',]);

//initialize astro table
var settings = {
    ElementID: "AshtakvargaTable",
    KeyColumn: "Ashtakvarga",
    ShowHeader: true,
    HeaderIcon: "fluent:table-28-filled"
};
var ashtakvargaTable = new AshtakvargaTable(settings);

var horoscopePredictionTexts = new HoroscopePredictionTexts("HoroscopePredictionTexts");

new IconButton("IconButton_Calculate_Horoscope");
new IconButton("IconButton_Advanced_Horoscope");

//new InfoBox("InfoBox_AskAI_Horoscope");
//new InfoBox("InfoBox_EasyImport_Horoscope");
//new InfoBox("InfoBox_ForgotenTime_Horoscope");


//----------------------------------- FUNCS ------------------------------------------

function OnClickAdvanced_Horoscope() {
    smoothSlideToggle('#HoroscopeAdvancedInputHolder');
}

async function OnClickCalculate_Horoscope() {

    //get full data of selected person
    let selectedPerson = await horoscopePersonSelector.GetSelectedPerson();

    //if no selected person then ask user if sleeping 😴
    if (selectedPerson == null) {
        Swal.fire({ icon: 'error', title: 'Please select person, sir! 🙄', showConfirmButton: true });
        return;
    }

    //---------------------------------------- LETS START -----------------------------------------

    //show loading to user
    CommonTools.ShowLoading();

    //make output holder visible
    $("#outputHoroscope").show();

    //hide sidebar links for nice clean fit (UX improvement)
    $("#SidebarInfoBoxHolder").slideUp(500);

    //update page title with person name to for easy multi tab use (UX ease)
    document.title = `${selectedPerson.DisplayName} | Horoscope`;

    //get birth time of selected person (URL format)
    var birthTimeUrl = selectedPerson.BirthTime.ToUrl();

    //generate tables and charts
    //await generateHoroscopeChat(birthTimeUrl);

    //data used to generate chart
    var inputArguments = {
        TimeUrl: birthTimeUrl,
        Ayanamsa: ayanamsaSelector.SelectedAyanamsa
    };

    await indianChart.GenerateChart(inputArguments);
    await allPlanetDataTable.GenerateTable(inputArguments);
    await allHouseDataTable.GenerateTable(inputArguments);
    await ashtakvargaTable.GenerateTable(inputArguments);
    await strengthChart.GenerateChart(inputArguments);
    await horoscopePredictionTexts.GenerateTable(inputArguments);

    //play sound for better UX
    playBakingDoneSound();

    //hide loading
    Swal.close();
}

async function generateHoroscopeChat(birthTimeUrl) {

    var settingsHoroscopeChat = {
        ElementID: "HoroscopeChat",
        ShowHeader: true,
        HeaderIcon: "fluent:table-28-filled",
        SelectedBirthTime: birthTimeUrl,
        //Ayanamsa: ayanamsaSelector.SelectedAyanamsa //NOTE: hard set to Raman in Server since all predictions are from Raman 
    };

    //note: on generate, chat instance is loaded into window.vedastro.horoscopechat
    new HoroscopeChat(settingsHoroscopeChat);
}


