
//██╗      ██████╗  ██████╗ ██╗ ██████╗
//██║     ██╔═══██╗██╔════╝ ██║██╔════╝
//██║     ██║   ██║██║  ███╗██║██║
//██║     ██║   ██║██║   ██║██║██║
//███████╗╚██████╔╝╚██████╔╝██║╚██████╗
//╚══════╝ ╚═════╝  ╚═════╝ ╚═╝ ╚═════╝
//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE


//uses UAParser library to extract user data
function getVisitorData() {
    console.log(`JS: getVisitorData`);
    var parser = new UAParser();
    return parser.getResult();
}

//Jquery to show inputed element
function showWrapper(element) {
    console.log(`JS: showWrapper`);
    $(element).show();
};

//Jquery to hide inputed element
function hideWrapper(element) {
    console.log(`JS: hideWrapper`);
    $(element).hide();
};

//function to get cookies given a name
function getCookiesWrapper(cookieName) {
    console.log(`JS: getCookiesWrapper`);
    return Cookies.get(cookieName);
};

//function to set cookies given a name
function setCookiesWrapper(cookieName, cookieValue) {
    console.log(`JS: setCookiesWrapper`);
    return Cookies.set(cookieName, cookieValue);
};

//Jquery to attach event listener to inputed element
function addEventListenerWrapper(element, eventName, functionName) {
    console.log(`JS: addEventListenerWrapper : ${eventName} : ${functionName}`);
    element.addEventListener(eventName, window[functionName]);
};

//gets current page url
function getUrl() {
    console.log(`JS: getUrl`);
    return window.location.href;
};

function getGoogleUserName() {
    console.log(`JS: getGoogleUserName`);
    return window.googleUserName;
};

function getGoogleUserEmail() {
    console.log(`JS: googleUserEmail`);
    return window.googleUserEmail;
};

function getGoogleUserIdToken() {
    console.log(`JS: googleUserIdToken`);
    return window.googleUserIdToken;
};



//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
function generateWebsiteTaskListTable(tableId, tableData) {


    var table = new Tabulator(`#${tableId}`, {
        data: tableData,           //load row data from array
        layout: "fitColumns",      //fit columns to width of table
        responsiveLayout: "hide",  //hide columns that don't fit on the table
        tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: true,             //allow undo and redo actions on the table
        pagination: "local",       //paginate the data
        paginationSize: 7,         //allow 7 rows per page of data
        paginationCounter: "rows", //display count of paginated rows in footer
        movableColumns: false,      //allow column order to be changed
        resizableRows: true,       //allow row order to be changed
        initialSort: [             //set the initial sort order of the data
            { column: "name", dir: "asc" },
        ],
        columns: [                 //define the table columns
            { title: "Name", field: "name", editor: "input" },
            { title: "Description", field: "description", editor: "input" },
            { title: "Status", field: "status", editor: "select", editorParams: { values: ["Pending", "Complete"] } },
            { title: "Date", field: "date", sorter: "date", hozAlign: "center" },
        ],
    });
}

//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
function generatePersonListTable(tableId, tableData) {

    //set table data
    var table = new Tabulator(`#${tableId}`, {
        data: tableData,           //load row data from array
        editable: false,
        layout: "fitColumns",      //fit columns to width of table
        responsiveLayout: "hide",  //hide columns that don't fit on the table
        tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: false,             //allow undo and redo actions on the table
        pagination: "local",       //paginate the data
        paginationSize: 50,         //allow 7 rows per page of data
        paginationCounter: "rows", //display count of paginated rows in footer
        movableColumns: false,      //allow column order to be changed
        resizableRows: true,       //allow row order to be changed
        initialSort: [             //set the initial sort order of the data
            { column: "name", dir: "asc" },
        ],
        columns: [                 //define the table columns
            { title: "Name", field: "name", hozAlign: "center" },
            { title: "Gender", field: "genderString", hozAlign: "center" },
            { title: "Birth", field: "birthTimeString", hozAlign: "center" },
            { title: "Hash", field: "hash", hozAlign: "center" },
        ],
    });

    //handler when table row is clicked
    table.on("rowClick", function (e, row) {
        //get person name
        let personHash = row._row.data.hash;
        //send user to person editor page with clicked person
        window.location.href = `/personeditor/${personHash}`;
    });

    //same as click handler but for touch
    table.on("rowTap", function (e, row) {
        //get person name
        let personHash = row._row.data.hash;
        //send user to person editor page with clicked person
        window.location.href = `/personeditor/${personHash}`;
    });

}

//async sleep millisecond
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function InitProgressBar(ms) {

    window.ProgressBarTempValue = 0;

    window.ProgressBarInstance = new ProgressBar.Line('#progressBar', {
        strokeWidth: 4,
        easing: 'linear',
        duration: 1400,
        color: '#2dd128',
        trailColor: '#eee',
        trailWidth: 1,
        svgStyle: { width: '100%', height: '100%' },
        text: {
            style: {
                // Text color.
                // Default: same as stroke color (options.color)
                color: '#999',
                position: 'absolute',
                right: '0',
                top: '30px',
                padding: 0,
                margin: 0,
                transform: null
            },
            autoStyleContainer: false
        },
        from: { color: '#FFEA82' },
        to: { color: '#ED6A5A' },
        step: (state, bar) => {
            bar.setText(Math.round(bar.value() * 100) + ' %');
        }
    });

}

function SetProgressBar(percentage) {


    var value = percentage / 100; //convert 50 to 0.5
    window.ProgressBarTempValue += value;

    //if above 100 end here & reset to 0
    if (window.ProgressBarTempValue > 1) {
        window.ProgressBarTempValue = 0;
        return;
    }

    ProgressBarInstance.animate(window.ProgressBarTempValue);

}
function ResetProgressBar(percentage) {
    //reset both view & data
    window.ProgressBarTempValue = 0;
    ProgressBarInstance.animate(0);
}


//loads js file programatically,
//equivalent to js include in header
function loadJs(sourceUrl) {
    if (sourceUrl.Length == 0) {
        console.error("JS: loadJs: Invalid source URL");
        return;
    }

    var tag = document.createElement('script');
    tag.src = sourceUrl;
    tag.type = "text/javascript";

    tag.onload = function () {
        console.log("JS: loadJs: Script loaded successfully");
    }

    tag.onerror = function () {
        console.error("JS: loadJs: Failed to load script");
    }

    document.body.appendChild(tag);
}



//███████╗██╗░░░██╗███████╗███╗░░██╗████████╗  ██╗░░██╗░█████╗░███╗░░██╗██████╗░██╗░░░░░███████╗██████╗░░██████╗
//██╔════╝██║░░░██║██╔════╝████╗░██║╚══██╔══╝  ██║░░██║██╔══██╗████╗░██║██╔══██╗██║░░░░░██╔════╝██╔══██╗██╔════╝
//█████╗░░╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░  ███████║███████║██╔██╗██║██║░░██║██║░░░░░█████╗░░██████╔╝╚█████╗░
//██╔══╝░░░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░  ██╔══██║██╔══██║██║╚████║██║░░██║██║░░░░░██╔══╝░░██╔══██╗░╚═══██╗
//███████╗░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░  ██║░░██║██║░░██║██║░╚███║██████╔╝███████╗███████╗██║░░██║██████╔╝
//╚══════╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░  ╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝╚═════╝░╚══════╝╚══════╝╚═╝░░╚═╝╚═════╝░


//attached to dasa viewer to update time legend 
function mouseOverDasaViewHandler(mouse) {

    //only continue if mouse is exactly over 
    //a time slice (svg rect element), else end here
    let timeSlice = mouse.path[0];
    let isTimeSlice = timeSlice.localName == "rect";
    if (!isTimeSlice) { return; }

    //get details from inside the time slice
    var eventName = timeSlice.getAttribute("eventname");
    var stdTime = timeSlice.getAttribute("stdtime");
    var age = timeSlice.getAttribute("age");

    //place data into view
    $("#TimeCursorLegend").html(`${eventName} - ${stdTime} - AGE : ${age}`);

}

//attached to event viewer to update time legend
function mouseOverEventsViewHandler(mouse) {

    //only continue if mouse is exactly over 
    //a time slice (svg rect element), else end here
    let timeSlice = mouse.path[0];
    let isTimeSlice = timeSlice.localName == "rect";
    if (!isTimeSlice) { return; }

    //get details from inside the time slice
    var eventName = timeSlice.getAttribute("eventname");
    var stdTime = timeSlice.getAttribute("stdtime");

    //place data into view
    $("#TimeCursorLegend").html(`${eventName} - ${stdTime}`);

}

//called by sign in button & page refresh
//note : this function's name is hardwired in Blazor
function onSignInSuccessHandler(googleUser) {

    console.log(`JS: onSignInSuccessHandler`);

    //get the google user's details and save it to be accessed by Blazor
    var profile = googleUser.getBasicProfile();
    window.googleUserName = profile.getName();
    window.googleUserEmail = profile.getEmail();
    var id_token = googleUser.getAuthResponse().id_token;
    window.googleUserIdToken = profile.getId();


    //fire event in Blazor, that user just signed in
    DotNet.invokeMethod('Website', 'InvokeOnUserSignIn');
}

//called by sign out button does the actual sign out process
function onClickGoogleSignOutButton() {

    console.log(`JS: onSignOutEventHandler`);

    //do the sign out
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut();

    //reset sign in details
    window.googleUserName = "";
    window.googleUserEmail = "";
    window.googleUserIdToken = "";

    //fire event in Blazor, that user just signed out
    DotNet.invokeMethod('Website', 'InvokeOnUserSignOut');

}

