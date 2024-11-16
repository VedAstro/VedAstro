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

const defaultPreset = "1year";
//NOTE: person selector is linked into time range so that age presets (age1to10) can be calculated
var timeRangeSelector = new TimeRangeSelector("TimeRangeSelector", personSelector, defaultPreset);

var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox", "RAMAN");

var lifePredictorChart = new EvensChartViewer("LifePredictorChartHolder");


//------------------------ FUNCTIONS -----------------------------

function OnClickAdvanced() {
    smoothSlideToggle('#AdvancedInputHolder');
}

async function OnClickCalculate() {

    //------------------------------ CHECK DATA -----------------------------

    //check if name is selected
    let selectedPerson = await personSelector.GetSelectedPerson();

    //if no selected person then ask user if sleeping 😴
    if (selectedPerson == null) {
        Swal.fire({ icon: 'error', title: 'Please select person, sir! 🙄', showConfirmButton: true });
        return;
    }

    //make sure at least 1 event is selected
    let selectedEventTags = dasaEventsSelector.getSelectedEventsAsString(); //get selected events tag names
    if (selectedEventTags == null) {
        Swal.fire({ icon: 'error', title: 'Select an Event Type', html: 'Minimum 1 <strong>Event Type</strong> is needed. Without it what to calculate?😨', showConfirmButton: true });
        return;
    }

    //get selected coloring algorithms
    let selectedAlgorithms = algoSelector.getSelectedAlgorithmsAsString();
    if (selectedAlgorithms == null) {
        Swal.fire({ icon: 'error', title: 'Select an Algorithm', html: 'Minimum 1 <strong>Algorithm</strong> is needed. If you don\'t want coloring check Neutral.', showConfirmButton: true });
        return;
    }

    //check if time range is valid, will auto show invalid msg
    let rangeIsValid = timeRangeSelector.isValid();
    if (!rangeIsValid) { return; } //end here if not valid


    //------------------------------ PREPARE GUI -----------------------------

    //show loading to user
    CommonTools.ShowLoading();

    //update page title with person name to for easy multi tab use (UX ease)
    document.title = `${selectedPerson.DisplayName} | Life Predictor`;

    //if on mobile hide sidebar to remove distraction in limited space (UX improvement)
    if (CommonTools.IsMobile()) { $("#SidebarInfoBoxHolder").slideUp(500); }


    //------------------------------- CALL API ---------------------

    var selectedPersonId = selectedPerson.PersonId;

    //NOTE: time range can be both custom & presets
    let timeRangeUrl = timeRangeSelector.getSelectedTimeRangeAsURLString();

    //based on time range calculate days per pixel for 1000px (days between/width in px)
    let daysPerPixel = daysPerPixelInput.getValue();

    //generate chart with data
    await lifePredictorChart.GenerateChart(selectedPersonId, timeRangeUrl, daysPerPixel, selectedEventTags, selectedAlgorithms, ayanamsaSelector.SelectedAyanamsa);

    //play sound for better UX
    playBakingDoneSound();

    //hide loading
    Swal.close();

}



