updateHistory();


new DesktopSidebar("DesktopSidebarHolder", links);
new PageHeader("PageHeader");

var personSelector = new PersonSelectorBox("PersonSelectorBox");


new IconButton("IconButton_Calculate");
//new IconButton("IconButton_Advanced");


//----------------------------------- FUNCS ------------------------------------------

function OnClickAdvanced() {
    smoothSlideToggle('#AdvancedInputHolder');
}
function OnClickCalculate() {

    //get full data of selected person
    let selectedMethod = $('#FinderMethodSelector').val();

    //if no selected person then ask user if sleeping 😴
    if (selectedPerson == null) { Swal.fire({ icon: 'error', title:'Are you sleeping? 😴', html: 'Please select a algorithm, sir!', showConfirmButton: true }); }

    //---------------------------------------- LETS START -----------------------------------------

    //show loading to user
    CommonTools.ShowLoading();

    //based on selected finder method, call API
    //NOTE: method value hard coded in HTML corresponds to API call name
    let timeFinderList = getTimeFinderListFromApi(selectedMethod); 

    //show results
    $('#OutputHolder').show();
}

async function getTimeFinderListFromApi(selectedApiCall, timeUrl) {
    try {
        const response = await fetch(`${VedAstro.ApiDomain}/Calculate/${selectedApiCall}/${timeUrl}`);
        if (!response.ok) { throw new Error(`HTTP error! status: ${response.status}`); } //server connection check

        const data = await response.json();
        if (data.Status !== 'Pass') { throw new Error('Failed to retrieve data. Status is not "Pass".'); } //calc data check

        
        return data.Payload[selectedApiCall];

    } catch (error) {
        console.error('Error fetching data:', error);
        return null; // Return an empty array if there's an error
    }
}
