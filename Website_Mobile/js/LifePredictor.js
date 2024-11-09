updateHistory();

new DesktopSidebar("DesktopSidebarHolder", links);
new PageHeader("PageHeader");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");

const defaultSelected = ['PD1', 'PD2', 'PD3', 'PD4', 'PD5', 'PD6', 'PD7'];
var dasaEventsSelector = new DasaEventsSelector("DasaEventsSelector", defaultSelected);



//SELECT DEFAULT ALGORITHMS
//SET ON : 8 JAN "24
//below algo tested well for Monroe and Steve
const defaultSelectedAlgo = "General,IshtaKashtaPhalaDegree,PlanetStrengthDegree";
var algoSelector = new AlgorithmsSelector("AlgorithmsSelector", defaultSelectedAlgo);

var personSelector = new PersonSelectorBox("PersonSelectorBox");

//NOTE: must init before Time range selector so that, it can catch default days between range
var daysPerPixelInput = new DayPerPixelInput("DayPerPixelInput");

const defaultPreset = "1month";
//NOTE: person selector is linked into time range so that age presets (age1to10) can be calculated
var timeRangeSelector = new TimeRangeSelector("TimeRangeSelector", personSelector, defaultPreset);

var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox", "RAMAN");



//------------------------ FUNCTIONS -----------------------------

function OnClickAdvanced() {
    smoothSlideToggle('#GoodTimeFinderAdvancedInputHolder');
}

async function OnClickCalculate() {

    //check if name is selected
    let selectedPerson = await personSelector.GetSelectedPerson();

    //if no selected person then ask user if sleeping 😴
    if (selectedPerson == null) { Swal.fire({ icon: 'error', title: 'Please select person, sir! 🙄', showConfirmButton: true }); }

    //make sure at least 1 event is selected
    let selectedEventTags = dasaEventsSelector.getSelectedEventsAsString(); //get selected events tag names
    if (selectedEventTags == null) { Swal.fire({ icon: 'error', title: 'Select an Event Type', html: 'Minimum 1 <strong>Event Type</strong> is needed. Without it what to calculate?😨', showConfirmButton: true }); }

    //check if time range is valid, will auto show invalid msg
    let rangeIsValid = timeRangeSelector.isValid();
    if (!rangeIsValid) { return; } //end here if not valid


    //------------------------------ OK LETS START -----------------------------

    //show loading to user
    CommonTools.ShowLoading();

    //update page title with person name to for easy multi tab use (UX ease)
    document.title = `${selectedPerson.DisplayName} | Life Predictor`;

    //if on mobile hide sidebar to remove distraction in limited space (UX improvement)
    if (CommonTools.IsMobile()) { $("#SidebarInfoBoxHolder").slideUp(500); }


    //------------------------------- CALL API ---------------------

    var selectedPersonId = selectedPerson.PersonId;

    //get selected coloring algorithms //TODO need to be compulsory
    var selectedAlgorithms = algoSelector.getSelectedAlgorithmsAsString();

    //NOTE: time range can be both custom & presets
    let timeRangeUrl = timeRangeSelector.getSelectedTimeRangeAsURLString();

    //based on time range calculate days per pixel for 1000px (days between/width in px)
    let daysPerPixel = daysPerPixelInput.getValue();

    //construct API call URL in correct format
    let apiUrl = `${VedAstro.ApiDomain}/EventsChart/${selectedPersonId}/${timeRangeUrl}/${daysPerPixel}/${selectedEventTags}/${selectedAlgorithms}/Ayanamsa/${ayanamsaSelector.SelectedAyanamsa}`;

    //call the API and wait for the chart to be complete
    const fetchApi = async () => {
        try {
            const response = await fetch(apiUrl);
            const callStatus = response.headers.get('Call-Status');

            //if chart is still being built
            if (callStatus === 'Running') {
                await new Promise(resolve => setTimeout(resolve, 5000)); // wait 5 seconds
                return fetchApi(); // make the call again
            } else if (callStatus === 'Fail') {
                throw new Error('API call failed');
            } else if (callStatus === 'Pass') {
                const svgString = await response.text();
                return svgString;
            }
        } catch (error) {
            console.error(error);
            throw error;
        }
    };

    //get chart from api as SVG string
    let svgString = await fetchApi();

    // inject SVG string into element with id "EventsChartSvgHolder"
    document.getElementById("EventsChartSvgHolder").innerHTML = svgString;

    // get id of SVG element
    let svgElement = document.getElementById("EventsChartSvgHolder").querySelector("svg");
    let chartId = svgElement.id; //NOTE: unique ID of the chart made by server

    //brings to life & makes available in window.EventsChartList
    new EventsChart(chartId);

    //let caller know all went well
    console.log(`🤲 Amen! Chart Loaded : ID:${chartId}`);

    //make chart holder with buttons visible
    $('#EventsChartMainElement').show();

    //hide placeholder text
    $('#EventsChartPlaceHolderMessage').hide();

    //play sound for better UX
    playBakingDoneSound();

    //hide loading
    Swal.close();

}



