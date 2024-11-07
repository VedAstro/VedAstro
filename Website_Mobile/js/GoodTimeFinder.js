updateHistory();

new DesktopSidebar("DesktopSidebarHolder", links);
new PageHeader("PageHeader");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");

const defaultSelected = ['General', 'Personal'];
const allowedParentCheckboxes = ['General', 'Personal', 'Agriculture', 'Building',
    'Astronomical', 'BuyingSelling', 'Medical', 'Marriage', 'Travel', 'Studies', 'HairNailCutting'];
var eventsSelector = new EventsSelector("EventsSelector", allowedParentCheckboxes, defaultSelected);

var timeRangeSelector = new TimeRangeSelector("TimeRangeSelector");


//SELECT DEFAULT ALGORITHMS
//SET ON : 8 JAN "24
//below algo tested well for Monroe and Steve
//General,IshtaKashtaPhalaDegree,PlanetStrengthDegree
var algoSelector = new AlgorithmsSelector("AlgorithmsSelector", "General,PlanetStrengthDegree,IshtaKashtaPhalaDegree");

var personSelector = new PersonSelectorBox("PersonSelectorBox");

var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox");


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

    //------------------------------OK LETS START-----------------------------

    //update page title with person name to for easy multi tab use (UX ease)
    document.title = `${selectedPerson.DisplayName} | Good Time Finder`;

    //-------------------------------CALCULATION STUFF ---------------------
    var selectedPersonId = selectedPerson.PersonId;

    //get selected events tag names
    var selectedEventTags = eventsSelector.getSelectedTagNamesAsString();

    //get selected coloring algorithms
    var selectedAlgorithms = algoSelector.getSelectedAlgorithmsAsString();

    //NOTE: time range can be both custom & presets
    let timeRangeUrl = timeRangeSelector.getSelectedTimeRangeAsURLString();

    //based on time range calculate days per pixel for 1000px
    let daysPerPixel = "0.1";

    //construct API call URL in correct format
    // .../Viknesh1994                     : 0
    // /Start/00:00/01/01/2011             : 2,3,4,5
    // /End/00:00/31/12/2024/+08:00        : 7,8,9,10,11
    // /5.439                              : 12 DaysPerPixel
    // /PD1,PD2,PD3,PD4,PD5,AshtakvargaGochara,Gochara :13 EventTagList
    // /GetGeneralScore,GocharaAshtakvargaBindu        :14 SelectedAlgorithm
    // /Ayanamsa/LahiriChitrapaksha                    :15 Optional Ayanamsa, must be last
    let apiUrl = `${VedAstro.ApiDomain}/${selectedPersonId}/${timeRangeUrl}/${daysPerPixel}/${selectedEventTags}/${selectedAlgorithms}`;

    console.log(apiUrl);
}

