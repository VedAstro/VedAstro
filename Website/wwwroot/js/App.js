
//█▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
//█▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█
//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE


//Initializes the global error cather, from here error handler in blazor is called.
//This is only a backup way to know unexpected exception occurred,
//not really caught & handled
//Notes:
//- called from MainLayout after render
//- needs to be called after index.html has fully loaded 
//- mainly used for logging global errors to API
//- from much testing even with ErrorBoundary, only the default blazor error message consistently gets called on error
//  as such error is known by watching this element's style display property 
function InitErrorCatcher() {

    //setup call to handler in blazor
    //call only if style display update
    var observer = new MutationObserver(function (mutationsList, observer) {
        for (var mutation of mutationsList) {
            //changes to style is assumed to be only changes
            //to display prop when error occurs
            if (mutation.attributeName == "style") {
                //call blazor handler
                DotNet.invokeMethod('Website', 'OnAppError');
            }
        }
    });

    //get the default error element
    var blazorDefaultErrorElem = $("#blazor-error-ui")[0];

    //attach handler to the element
    observer.observe(blazorDefaultErrorElem, { attributes: true });

}

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

//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
function generateLifeEventListTable(tableId, tableData) {

    //set table data
    window.lifeEventsListTable = new Tabulator(`#${tableId}`, {
        data: tableData,           //load row data from array
        editable: true,
        layout: "fitColumns",      //fit columns to width of table
        responsiveLayout: "hide",  //hide columns that don't fit on the table
        tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: false,             //allow undo and redo actions on the table
        pagination: false, //enable pagination
        // pagination: "local",       //paginate the data
        //paginationSize: 50,         //allow 7 rows per page of data
        //paginationCounter: "rows", //display count of paginated rows in footer
        movableColumns: false,      //allow column order to be changed
        resizableRows: true,       //allow row order to be changed
        initialSort: [             //set the initial sort order of the data
            { column: "name", dir: "asc" },
        ],
        columns: [                 //define the table columns
            { title: "Event Name", field: "Name", editor: "input", hozAlign: "center" },
            { title: "Start Time", field: "StartTime", editor: "input", hozAlign: "center" },
            { title: "End Time", field: "EndTime", editor: "input", hozAlign: "center" },
            { title: "Nature", field: "Nature", editor: "input", hozAlign: "center" },
            //code to delete button for row
            {
                formatter: "buttonCross", width: 40, hozAlign: "center", cellClick: function (e, cell) {
                    cell.getRow().delete();
                }
            }
        ],
    });

}

function getLifeEventsListTableData() {
    return window.lifeEventsListTable.getData();
}

//adds new row to life events table
//note: defaults are set here, maybe move to blazor side for conformity
function addNewLifeEventToTable() {

    var addToTopOfTable = true;
    window.lifeEventsListTable.addData([{ Name: "New Life Event", StartTime: "00:00 10/10/2020 +08:00", EndTime: "00:00 10/10/2020 +08:00", Nature: "Good" }], addToTopOfTable);
}

//async sleep millisecond
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
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

function InjectIntoElement(element, valueToInject) {

    //convert string to html node
    const template = document.createElement("template");
    template.innerHTML = valueToInject;
    const node = template.content.firstElementChild;

    //place new node in parent
    element.appendChild(node);
}

//async delay for specified time in ms
//example: await delay(1000);
function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}

//shows an email 
async function ShowLeaveEmailAlert() {

    //show alert to get email
    const { value: email } = await Swal.fire({
        title: 'Leave your email',
        input: 'email',
        inputPlaceholder: 'Enter your email address'
    });

    if (email) {
        Swal.fire('Thanks', 'We will update you soon..', 'success');
    }

    //send email inputed to caller
    return email;

}


//█▀█ █▀█ █▀█ █▀▀ █▀█ █▀▀ █▀ █▀   █▄▄ ▄▀█ █▀█
//█▀▀ █▀▄ █▄█ █▄█ █▀▄ ██▄ ▄█ ▄█   █▄█ █▀█ █▀▄

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

//Adds input value to current progress bar
function AddToProgressBar(percentage) {


    var value = percentage / 100; //convert 50 to 0.5
    window.ProgressBarTempValue += value;

    //if above 100 end here & reset to 0
    if (window.ProgressBarTempValue > 1) {
        window.ProgressBarTempValue = 0;
        return;
    }

    ProgressBarInstance.animate(window.ProgressBarTempValue);

}

//Adds input value to current progress bar
function SetProgressBar(percentage) {

    var value = percentage / 100; //convert 50 to 0.5
    window.ProgressBarTempValue = value;
    ProgressBarInstance.animate(window.ProgressBarTempValue);
}

//auto updates progress bar till 100%
async function ProgressBarSlowAutoUpdate() {


    for (let percent = 0; percent < 100; percent++) {
        //set 0
        var progressVal = percent / 100; //convert exp: 50 to 0.5
        ProgressBarInstance.animate(progressVal);
        await delay(200);
    }


}

function ResetProgressBar() {
    //reset both view & data
    window.ProgressBarTempValue = 0;
    ProgressBarInstance.animate(0);
}

//Gets current value of progress bar
function GetProgressBarValue() {
    return window.ProgressBarTempValue;
}



//█▀▀ █▀█ █▀█ █▀▀ █░░ █▀▀   █░░ █▀█ █▀▀ █ █▄░█
//█▄█ █▄█ █▄█ █▄█ █▄▄ ██▄   █▄▄ █▄█ █▄█ █ █░▀█

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



//█▀▀ █░█ █▀▀ █▄░█ ▀█▀   █░█ ▄▀█ █▄░█ █▀▄ █░░ █▀▀ █▀█
//██▄ ▀▄▀ ██▄ █░▀█ ░█░   █▀█ █▀█ █░▀█ █▄▀ █▄▄ ██▄ █▀▄


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

//fired when mouse moves over dasa view box
//used to auto update cursor line & time legend
async function onMouseMoveDasaViewEventHandler(mouse) {

    //auto update time legend first then move cursor line
    //else cursor line will obstruct reading event slice rect 
    autoUpdateTimeLegend(mouse);
    //note: delay needed to reduce cursor line obstructing rate,
    //though it does happen when mouse move very slowly.
    await delay(100);
    autoMoveCursorLine(mouse);
}

function autoMoveCursorLine(mouse) {

    //gets the measurements of the dasa view holder
    //the element where cursor line will be moving
    //TODO read val from global var
    let holderMeasurements = $("#DasaViewHolder")[0].getBoundingClientRect();

    //calculate mouse X relative to dasa view box
    let relativeMouseX = mouse.clientX - holderMeasurements.left;
    let relativeMouseY = mouse.clientY - holderMeasurements.top; //when mouse leaves top
    let relativeMouseYb = mouse.clientY - holderMeasurements.bottom; //when mouse leaves bottom

    //if mouse out of element element, hide cursor and end here
    let mouseOut = relativeMouseY < 0 || relativeMouseX < 0 || relativeMouseYb > 0;
    if (mouseOut) { $("#CursorLine").hide(); return; }
    else { $("#CursorLine").show(); }

    //move vertical line to under mouse inside dasa view box
    $("#CursorLine").attr('transform', `matrix(1, 0, 0, 1, ${relativeMouseX}, 0)`);

}

//attached to dasa viewer to update time legend 
function autoUpdateTimeLegend(mouse) {

    //go through all the elements until found the one with events
    //var length = mouse.path.length;
    var elementFound = false;
    var elementIndex = 0; //start with 0
    var eventName = null;
    var elementUnderMouse = null;


    //note : this done because the element containing
    //the data is parent of element that starts event
    while (!elementFound) {

        //get any element under mouse and try to get values from it
        elementUnderMouse = mouse.path[elementIndex];

        //get details from inside the time slice
        eventName = elementUnderMouse.getAttribute("eventname");

        //if found, stop looking
        if (eventName != null) { elementFound = true; }
        //if not found, move to element higher in mouse path tree
        else { ++elementIndex; }

        //only go up 3 levels
        if (elementIndex > 3) { return; }
    }

    //get details from inside the time slice
    var stdTime = elementUnderMouse.getAttribute("stdtime");
    var age = elementUnderMouse.getAttribute("age");


    //place data into view
    $("#TimeCursorLegend").html(`${eventName} - ${stdTime} - AGE : ${age}`);

}

//element passed in is the element where touch will be detected
function InitTouchLib(element) {

    //var myElement = document.getElementById('myElement')

    // create a simple instance
    // by default, it only adds horizontal recognizers
    window.hammerJs = new Hammer(element, { touchAction: 'auto' });

    // listen to events...
    window.hammerJs.on("panleft panright tap", function (ev) {

        //when dragging the dasa report, this will stop
        //from detecting as out of element
        if (ev.center.x == 0) { return; }

        //converting the touch event into a mouse event
        var mouse = {
            clientX: ev.center.x,
            clientY: ev.center.y, 
            path: ev.srcEvent.path
        };

        //and calling the event handlers
        //that a mouse would normally fire
        autoMoveCursorLine(mouse);
        autoUpdateTimeLegend(mouse);
    });

    //window.hammerJs = new Hammer(myElement, myOptions);

//window.hammerJs.on('pan', function (ev) {
//    console.log(ev);
//});
}



