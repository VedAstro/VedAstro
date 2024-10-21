updateHistory();

new PageTopNavbar("PageTopNavbar");
new DesktopSidebar("DesktopSidebarHolder");
new PageHeader("PageHeader");
new PageFooter("PageFooter");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");

var horoscopePersonSelector = new PersonSelectorBox("PersonSelectorBox");
var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox");


function OnClickAdvanced() {
    smoothSlideToggle('#GoodTimeFinderAdvancedInputHolder');
}

