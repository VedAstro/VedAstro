updateHistory();

new PageTopNavbar("PageTopNavbar");
const links = [
    {
        url: 'Home',
        icon: 'ant-design:home-twotone',
        text: 'Home'
    },
    {
        url: 'MatchChecker',
        icon: 'bi:arrow-through-heart-fill',
        text: 'Match Checker'
    },
    {
        url: 'AIChat',
        icon: 'fluent-mdl2:chat-bot',
        text: 'AI Chat'
    },
    {
        url: 'LifePredictor',
        icon: 'gis:map-time',
        text: 'Life Predictor'
    },
    {
        url: 'MatchFinder',
        icon: 'game-icons:lovers',
        text: 'Match Finder'
    },
    {
        url: 'Horoscope',
        icon: 'fluent:book-star-20-filled',
        text: 'Horoscope'
    },
    {
        url: 'GoodTimeFinder',
        icon: 'svg-spinners:clock',
        text: 'Good Time Finder'
    },
    {
        url: 'APIBuilder',
        icon: 'mdi:cloud-tags',
        text: 'API Builder'
    },
    {
        url: 'Numerology',
        icon: 'fluent:text-number-format-20-filled',
        text: 'Numerology'
    },
    //{
    //    url: 'StarsAboveMe',
    //    icon: 'solar:moon-stars-bold',
    //    text: 'Stars Above Me'
    //}
];

new DesktopSidebar("DesktopSidebarHolder", links);
new PageHeader("PageHeader");
new PageFooter("PageFooter");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");

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

