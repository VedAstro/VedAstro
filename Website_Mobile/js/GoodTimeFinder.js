updateHistory();

new DesktopSidebar("DesktopSidebarHolder", links);
new PageHeader("PageHeader");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");

new EventsSelector("EventsSelector");

//SELECT DEFAULT ALGORITHMS
//SET ON : 8 JAN "24
//below algo tested well for Monroe and Steve
//General,IshtaKashtaPhalaDegree,PlanetStrengthDegree
var algoSelector = new AlgorithmsSelector("AlgorithmsSelector", "General,PlanetStrengthDegree,IshtaKashtaPhalaDegree");

var horoscopePersonSelector = new PersonSelectorBox("PersonSelectorBox");
var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox");


function OnClickAdvanced() {
    smoothSlideToggle('#GoodTimeFinderAdvancedInputHolder');
}

function OnClickCalculate() {

    //check if name is selected

    //make sure at least 1 event is selected

    //------------------------------OK LETS START-----------------------------

    //get selected events
    var selectedEvents = algoSelector.getSelectedAlgorithmsAsString();


}

