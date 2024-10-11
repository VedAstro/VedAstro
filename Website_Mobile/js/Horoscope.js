//-----------------------------> HOROSCOPE PAGE
updateHistory();

new DesktopSidebar("DesktopSidebarHolder");
new PageHeader("HoroscopePageHeader");
var horoscopePersonSelector = new PersonSelectorBox("PersonSelectorBox_Horoscope");
new IconButton("IconButton_Calculate_Horoscope");

new InfoBox("InfoBox_AskAI_Horoscope");
new InfoBox("InfoBox_EasyImport_Horoscope");
new InfoBox("InfoBox_ForgotenTime_Horoscope");


async function OnClickCalculate_Horoscope() {

    //show loading to user
    CommonTools.ShowLoading();

    //make output holder visible
    $("#outputHoroscope").show();

    //hide sidebar links for nice clean fit (UX improvement)
    $("#SidebarInfoBoxHolder").slideUp(500);

    //get birth time of selected person (URL format)
    let selectedPerson = await horoscopePersonSelector.getSelectedPerson();
    //get time in url format
    var timeUrl = selectedPerson.BirthTime.ToUrl();
    timeUrl = timeUrl.substring(1) + "/"; //remove leading / and add trailing / (minor format correction)

    //show planet data table
    await initPlanetDataTable(timeUrl);

    //hide loading
    Swal.close();
}

async function initPlanetDataTable(birthTimeUrl)
{
    //----------------------PLANET DATA----------------------------
    var planetColumns = [
        { Api: "PlanetZodiacSign", Enabled: true, Name: "Sign" },
        { Api: "PlanetConstellation", Enabled: true, Name: "Star" },
        { Api: "HousePlanetOccupies", Enabled: true, Name: "Occupies" },
        { Api: "HousesOwnedByPlanet", Enabled: true, Name: "Owns" },
        { Api: "PlanetLordOfZodiacSign", Enabled: true, Name: "Sign Lord" },
        { Api: "PlanetLordOfConstellation", Enabled: true, Name: "Star Lord" },
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
        //TimeUrl: "Location/Bengaluru/Time/11:00/25/07/1984/+00:00/",
        TimeUrl: birthTimeUrl,
        HoraryNumber: 0,
        RotateDegrees: 0,
        Ayanamsa: "KRISHNAMURTI", //default is Lahiri
    };

    //generate table
    await planetDataTable.GenerateTable(inputArguments);

}