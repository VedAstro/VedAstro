updateHistory();

new DesktopSidebar("DesktopSidebarHolder", links);
new PageHeader("PageHeader");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");

const defaultSelected = ['General', 'Personal'];
const allowedParentCheckboxes = ['General', 'Personal', 'Agriculture', 'Building',
    'Astronomical', 'BuyingSelling', 'Medical', 'Marriage', 'Travel', 'Studies', 'HairNailCutting'];
var eventsSelector = new EventsSelector("EventsSelector", allowedParentCheckboxes, defaultSelected);

const defaultPreset = "1month";
var timeRangeSelector = new TimeRangeSelector("TimeRangeSelector", defaultPreset);


//SELECT DEFAULT ALGORITHMS
//SET ON : 8 JAN "24
//below algo tested well for Monroe and Steve
//General,IshtaKashtaPhalaDegree,PlanetStrengthDegree
var algoSelector = new AlgorithmsSelector("AlgorithmsSelector", "General,PlanetStrengthDegree,IshtaKashtaPhalaDegree");

var personSelector = new PersonSelectorBox("PersonSelectorBox");

var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox");
var daysPerPixelInput = new DayPerPixelInput("DayPerPixelInput");


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
    let selectedEventTags = eventsSelector.getSelectedTagNamesAsString(); //get selected events tag names
    if (selectedEventTags == null) { Swal.fire({ icon: 'error', title: 'Select an Event Type', html: 'Minimum 1 <strong>Event Type</strong> is needed. Without it what to calculate?😨', showConfirmButton: true }); }

    //check if time range is valid, will auto show invalid msg
    let rangeIsValid = timeRangeSelector.isValid();
    if (!rangeIsValid) { return; } //end here if not valid


    //------------------------------ OK LETS START -----------------------------

    //show loading to user
    CommonTools.ShowLoading();

    //update page title with person name to for easy multi tab use (UX ease)
    document.title = `${selectedPerson.DisplayName} | Good Time Finder`;

    //------------------------------- CALL API ---------------------

    var selectedPersonId = selectedPerson.PersonId;

    //get selected coloring algorithms
    var selectedAlgorithms = algoSelector.getSelectedAlgorithmsAsString();

    //NOTE: time range can be both custom & presets
    let timeRangeUrl = timeRangeSelector.getSelectedTimeRangeAsURLString();

    //based on time range calculate days per pixel for 1000px (days between/width in px)
    let daysPerPixel = daysPerPixelInput.getValue();

    //construct API call URL in correct format

    let apiUrl = `${VedAstro.ApiDomain}/EventsChart/${selectedPersonId}/${timeRangeUrl}/${daysPerPixel}/${selectedEventTags}/${selectedAlgorithms}`;

    const fetchApi = async () => {
        try {
            const response = await fetch(apiUrl);
            const callStatus = response.headers.get('Call-Status');

            if (callStatus === 'Running') {
                await new Promise(resolve => setTimeout(resolve, 10000)); // wait 10 seconds
                return fetchApi(); // make the call again
            } else if (callStatus === 'Fail') {
                throw new Error('API call failed');
            } else if (callStatus === 'Pass') {
                const svgString = await response.text();
                return svgString;
            } else {
                throw new Error(`Unknown Call-Status: ${callStatus}`);
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
    let chartId = svgElement.id;

    //create instance
    //NOTE: unique ID of the chart made by server
    // chart is available in window.EventsChartList
    var index = new EventsChart(chartId); //brings to life

    //let caller know all went well
    console.log(`Amen! Chart Loaded : INDEX:${index}, ID:${chartId}`);

    //make chart holder with buttons visible
    $('#EventsChartMainElement').show();

    //hide placeholder text
    $('#EventsChartPlaceHolderMessage').hide();

    //play sound for better UX
    playBakingDoneSound();

    //hide loading
    Swal.close();

}

