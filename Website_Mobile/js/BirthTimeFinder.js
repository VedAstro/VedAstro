updateHistory();

new PageTopNavbar("PageTopNavbar");
new DesktopSidebar("DesktopSidebarHolder");
new PageHeader("PageHeader");
new PageFooter("PageFooter");

new IconButton("IconButton_Calculate");
new IconButton("IconButton_Advanced");


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


    //show results
    $('#OutputHolder').show();
}
