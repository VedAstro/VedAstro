
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

//gets data about screen from browser for logging
function getScreenData() {
    var screenData = {
        "Orientation": window.screen.orientation.type,
        "Width": window.screen.width,
        "Height": window.screen.height,
        "ColorDepth": window.screen.colorDepth
    }
    return screenData;
}
//get previous website for logging
var getOriginUrl = () => document.referrer;

//Jquery to show inputed element
//by class and ID (CSS selector)
function showWrapper(element) {
    console.log(`JS: showWrapper`);
    $(element).show();
};

//Jquery to hide inputed element
//by class and ID (CSS selector)
function hideWrapper(element) {
    console.log(`JS: hideWrapper`);
    $(element).hide();
};

//Jquery to attach event listener to inputed element
function addEventListenerWrapper(element, eventName, functionName) {
    console.log(`JS: addEventListenerWrapper : ${eventName} : ${functionName}`);
    element.addEventListener(eventName, window[functionName]);
};

//Jquery to attach event listener by class and ID (CSS selector)
function addEventListenerByClass(selector, eventName, functionName) {
    console.log(`JS: addEventListenerByClass : ${eventName} : ${functionName}`);

    //attach listener to each element
    $(selector).each(function () {
        this.addEventListener(eventName, window[functionName]);
    });

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
            { title: "Name", field: "Name", editor: "input", hozAlign: "center" },
            { title: "Time", field: "StartTime", editor: "input", hozAlign: "center" },
            { title: "Description", field: "Description", editor: "input", hozAlign: "center" },
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
    window.lifeEventsListTable.addData([{ Name: "New Life Event", StartTime: "00:00 10/10/2020 +08:00", Description: "Description", Nature: "Good" }], addToTopOfTable);
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
    var template = document.createElement("template");
    template.innerHTML = valueToInject;
    var nodeToInject = template.content.firstElementChild;

    //place new node in parent
    element.innerHTML = ''; //clear current children if any
    element.appendChild(nodeToInject);
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
        title: 'Notify me on update',
        input: 'email',
        inputPlaceholder: 'Enter your email address'
    });

    if (email) { Swal.fire('Thanks', 'We will update you soon..', 'success'); }

    //send email inputed to caller
    return email;
}

//Gets a mouses x axis relative inside the given element
//used to get mouse location on Dasa view
//returns 0 when mouse is out
function GetMousePositionInElement(mouseEventData, elementId) {

    //gets the measurements of the dasa view holder
    //the element where cursor line will be moving
    //TODO read val from global var
    let holderMeasurements = $(elementId)[0].getBoundingClientRect();

    //calculate mouse X relative to dasa view box
    let relativeMouseX = mouseEventData.clientX - holderMeasurements.left;
    let relativeMouseY = mouseEventData.clientY - holderMeasurements.top; //when mouse leaves top
    let relativeMouseYb = mouseEventData.clientY - holderMeasurements.bottom; //when mouse leaves bottom

    //if mouse out of element element, set 0 as indicator
    let mouseOut = relativeMouseY < 0 || relativeMouseX < 0 || relativeMouseYb > 0;

    if (mouseOut) {
        return 0;
    } else {

        var mouse = {
            xAxis: relativeMouseX,
            yAxis: relativeMouseY
        };
        return mouse;
    }

}

//gets all life event lines by class
//and attaches tooltip event on it to
//show data of the event on mouse hover
//uses Tippy js lib, needs to be called everytime
//new elements are created, because attach by direct
//element reference not class
function InitLifeEventLineToolTip() {

    $(".LifeEventLines").each(function () {

        var evName = this.getAttribute("eventname");

        tippy(this, {
            content: evName,
            placement: 'bottom',
            arrow: true
        });

    });

}


//functions used by localstorage manager in Blazor
var getProperty = key => key in localStorage ? JSON.parse(localStorage[key]) : null;
var removeProperty = key => key in localStorage ? localStorage.removeItem(key) : null;
var setProperty = (key, value) => { localStorage[key] = JSON.stringify(value); };
var watchProperty = async (instance, handlerName) => {
    window.addEventListener('storage', (e) => {
        instance.invokeMethodAsync(handlerName);
    });
};

//Uses Bootstrap Jquery plugin to toggle any collapsible component by id
var toggleAccordion = (id) => $(id).collapse("toggle");

var showAccordion = (id) => $(id).collapse("show");

//scrolls element by id into view
var scrollIntoView = (id) => $(id)[0].scrollIntoView();

var addClassWrapper = (element, classString) => $(element).addClass(classString);
var toggleClassWrapper = (element, classString) => $(element).toggleClass(classString);
var removeClassWrapper = (element, classString) => $(element).removeClass(classString);



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


//makes a reference to SignInButton instance, to be used when user clicks sign in
//called in Blazor, after component render
var SignInButtonInstance = (instance) => window.SignInButtonInstance = instance;
//wrapper function to forward call to blazor (hardwired in Blazor HTML)
var OnGoogleSignInSuccessHandler = (response) => window.SignInButtonInstance.invokeMethodAsync('OnGoogleSignInSuccessHandler', response);


//called from Blazor when custom login button clicked
var facebookLogin = () => FB.login(callBackFB, { scope: 'email' });
//wrapper function to forward call to blazor (hardwired in Blazor HTML)
var callBackFB = (response) => window.SignInButtonInstance.invokeMethodAsync('OnFacebookSignInSuccessHandler', response);




//█▀▀ █░█ █▀▀ █▄░█ ▀█▀   █░█ ▄▀█ █▄░█ █▀▄ █░░ █▀▀ █▀█
//██▄ ▀▄▀ ██▄ █░▀█ ░█░   █▀█ █▀█ █░▀█ █▄▀ █▄▄ ██▄ █▀▄


//fired when mouse moves over dasa view box
//used to auto update cursor line & time legend
async function onMouseMoveDasaViewEventHandler(mouse) {

    //get relative position of mouse in Dasa view
    var mousePosition = GetMousePositionInElement(mouse, "#DasaViewHolder");

    //if mouse is out of dasa view hide cursor and end here
    if (mousePosition == 0) { $("#CursorLine").hide(); return; }
    else { $("#CursorLine").show(); }

    //move cursor line 1st for responsiveness
    autoMoveCursorLine(mousePosition.xAxis);

    //update time legend
    autoUpdateTimeLegend(mousePosition);
}

function autoMoveCursorLine(relativeMouseX) {

    //move vertical line to under mouse inside dasa view box
    $("#CursorLine").attr('transform', `matrix(1, 0, 0, 1, ${relativeMouseX}, 0)`);

}

//attached to dasa viewer to update time legend 
function autoUpdateTimeLegendOld(relativeMouseX) {

    //x axis is rounded because axis value in rect is whole numbers
    //and it has to be exact match to get it
    var mouseRoundedX = Math.round(relativeMouseX);

    //use the mouse position to get dasa elements also at said position
    //note: faster and less erroneous than using mouse.path 
    var allElementsAtX = $(`[x=${mouseRoundedX}]`);


    $("#GocharaLegend").empty();

    //extract event data out and place it in legend
    allElementsAtX.each(function () {

        //based on the type of the event 
        var type = this.getAttribute("type");
        var eventName = this.getAttribute("eventname");
        var color = this.getAttribute("fill");

        //based on event type insert into right place
        switch (type) {
            case "Dasa":
                $("#DasaLegend").text(`${eventName}`);
                $("#DasaLegend").css("color", `${color}`);
                //add in time & age
                var stdTime = this.getAttribute("stdtime");
                var age = this.getAttribute("age");
                $("#DateLegend").text(`${stdTime}`);
                $("#AgeLegend").text(`${age}`);

                break;
            case "Bhukti":
                $("#BhuktiLegend").text(`${eventName}`);
                $("#BhuktiLegend").css("color", `${color}`);
                break;
            case "Antaram":
                $("#AntaramLegend").text(`${eventName}`);
                $("#AntaramLegend").css("color", `${color}`);
                break;
            case "Gochara":
                var $newdiv1 = $(`<li class=\"list-group-item\"></li>`);
                $newdiv1.text(`${eventName}`);
                $newdiv1.css("color", `${color}`);
                $("#GocharaLegend").append($newdiv1);
                break;
            default:
                return;
        }

    });


}

//generates the time legend shown next to dasa cursor line
//notes: a template row always exists in code,
//in client JS side uses template to create the rows from cloning it
//and modifying its prop as needed, as such any major edit needs to
//be done in API code

$(document).on('loadEventDescription', function (event, eventName) {
    //fill description box about event
    getEventDescription(eventName.replace(/ /g, ""))
        .then((eventDesc) => {
            var wrappedDescText = createSVGtext(eventDesc, 175, 24);
            $("#CursorLineLegendDescription").empty(); //clear previous desc
            $(wrappedDescText).appendTo("#CursorLineLegendDescription"); //add in new desc
        });
});

function autoUpdateTimeLegend(mousePosition) {

    //x axis is rounded because axis value in rect is whole numbers
    //and it has to be exact match to get it
    var mouseRoundedX = Math.round(mousePosition.xAxis);
    var mouseRoundedY = Math.round(mousePosition.yAxis);

    //use the mouse position to get all dasa rect
    //dasa elements at same X position inside the dasa svg
    //note: faster and less erroneous than using mouse.path
    var children = $("#DasaViewHolder").children();
    var allElementsAtX = children.find(`[x=${mouseRoundedX}]`);

    //template used to generate legend rows
    var holderTemplateId = `#CursorLineLegendTemplate`;

    //delete previously generated legend rows
    $(".CursorLineLegendClone").remove();


    //count good and bad events for summary row
    var goodCount = 0;
    var badCount = 0;
    var yAxis = 0;
    //extract event data out and place it in legend
    allElementsAtX.each(function () {

        //1 GET DATA
        //based on the type of the event 
        var type = this.getAttribute("type");

        //if no type exist, wrong elm skip it
        if (!type) { return; }

        //extract other data out of the rect
        var eventName = this.getAttribute("eventname");
        var color = this.getAttribute("fill");
        yAxis = parseInt(this.getAttribute("y"));//parse as num, for calculation

        //count good and bad events for summary row
        if (color == "red") { badCount++; }
        if (color == "green") { goodCount++; }


        //2 TIME & AGE LEGEND
        //create time legend at top only for first element
        if (allElementsAtX[0] === this) {
            var newTimeLegend = $(holderTemplateId).clone();
            newTimeLegend.removeAttr('id'); //remove the clone template id
            newTimeLegend.addClass("CursorLineLegendClone"); //to delete it on next run
            newTimeLegend.appendTo("#CursorLine"); //place new legend into parent
            newTimeLegend.show();//make cloned visible
            newTimeLegend.attr('transform', `matrix(1, 0, 0, 1, 10, ${yAxis - 15})`); //above 1st row
            var stdTime = this.getAttribute("stdtime");
            var age = this.getAttribute("age");
            newTimeLegend.children("text").text(`${stdTime} - AGE ${age}`);
            newTimeLegend.children("circle").hide();
        }

        //3 GENERATE EVENT ROW
        //make a copy of template for this event
        var newLegendRow = $(holderTemplateId).clone();
        newLegendRow.removeAttr('id'); //remove the clone template id
        newLegendRow.addClass("CursorLineLegendClone"); //to delete it on next run
        newLegendRow.appendTo("#CursorLine"); //place new legend into parent
        newLegendRow.show();//make cloned visible
        //position the group holding the legend over the event row which the legend represents
        newLegendRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${yAxis})`);

        //set event name text & color element
        var textElm = newLegendRow.children("text");
        var circleElm = newLegendRow.children("circle");
        textElm.text(`${eventName}`);
        circleElm.attr("fill", `${color}`);

        //4 GENERATE DESCRIPTION ROW LOGIC
        //check if mouse in within row of this event (y axis)
        var elementTopY = yAxis;
        var elementBottomY = yAxis + 15;
        if (mouseRoundedY >= elementTopY && mouseRoundedY <= elementBottomY) {
            //highlight event name row 
            textElm.css("fill", "red");
            textElm.css("font-weight", "600");

            //make holder visible
            $("#CursorLineLegendDescriptionHolder").show();

            $(document).trigger('loadEventDescription', eventName);
            
            
        } else {
            //todo
            //$("CursorLineLegendDescriptionHolder").hide(); //hide holder 
        }

    });


    //5 GENERATE LAST SUMMARY ROW
    //generate summary row at the bottom
    //make a copy of template for this event
    var newSummaryRow = $(holderTemplateId).clone();
    newSummaryRow.removeAttr('id'); //remove the clone template id
    newSummaryRow.addClass("CursorLineLegendClone"); //to delete it on next run
    newSummaryRow.appendTo("#CursorLine"); //place new legend into parent
    newSummaryRow.show();//make cloned visible
    //position the group holding the legend over the event row which the legend represents
    newSummaryRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${yAxis + 2 + 15})`);

    //set event name text & color element
    var textElm = newSummaryRow.children("text");
    var totalScore = goodCount + -Math.abs(badCount);
    textElm.text(`${totalScore} Good:${goodCount} Bad:${badCount}`);
    newSummaryRow.children("circle").hide();


}

//needs to be run once before get event description method is used
//loads xml file located in wwwroot to xml global data
async function LoadEventDataListFile() {

    var url = `${window.location.origin}/data/EventDataList.xml`;
    var response = await fetch(url, { mode: 'no-cors' });
    var dataListStr = await response.text();

    //parse as XML, to search through
    //and save as global data for access later
    var parser = new DOMParser();
    var xmlDoc = parser.parseFromString(dataListStr, "text/xml");
    window.EventDataListXml = $(xmlDoc); //jquery for use with .filter
}

//gets events from EventDataList.xml
//and injects them into dasa view
async function getEventDescription(eventName) {

    //search for matching event name
    var eventXmlList = window.EventDataListXml.find('Event'); //get all event elements
    var results = eventXmlList.filter(
        function () {
            var eventNameXml = $(this).children('Name').eq(0);
            return eventNameXml.text() === eventName;
        });

    var eventDescription = results.eq(0).children('Description').eq(0).text();

    return eventDescription;

}


//  This function attempts to create a new svg "text" element, chopping
//  it up into "tspan" pieces, if the caption is too long
function createSVGtext(caption, x, y) {
    var svgText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
    svgText.setAttributeNS(null, 'x', x);
    svgText.setAttributeNS(null, 'y', y);
    svgText.setAttributeNS(null, 'font-size', 10);
    svgText.setAttributeNS(null, 'fill', '#FFFFFF');         //  White text
    svgText.setAttributeNS(null, 'text-anchor', 'left');   //  Center the text

    //  The following two variables should really be passed as parameters
    var MAXIMUM_CHARS_PER_LINE = 30;
    var LINE_HEIGHT = 10;

    var words = caption.split(" ");
    var line = "";

    for (var n = 0; n < words.length; n++) {
        var testLine = line + words[n] + " ";
        if (testLine.length > MAXIMUM_CHARS_PER_LINE) {
            //  Add a new <tspan> element
            var svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
            svgTSpan.setAttributeNS(null, 'x', x);
            svgTSpan.setAttributeNS(null, 'y', y);

            var tSpanTextNode = document.createTextNode(line);
            svgTSpan.appendChild(tSpanTextNode);
            svgText.appendChild(svgTSpan);

            line = words[n] + " ";
            y += LINE_HEIGHT;
        }
        else {
            line = testLine;
        }
    }

    var svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
    svgTSpan.setAttributeNS(null, 'x', x);
    svgTSpan.setAttributeNS(null, 'y', y);

    var tSpanTextNode = document.createTextNode(line);
    svgTSpan.appendChild(tSpanTextNode);

    svgText.appendChild(svgTSpan);

    return svgText;
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
        // alert("todo touch not implemented!!!");
        autoMoveCursorLine(mouse);
        //todo call to this method is out dated
        autoUpdateTimeLegend(mouse);
    });

    //window.hammerJs = new Hammer(myElement, myOptions);

    //window.hammerJs.on('pan', function (ev) {
    //    console.log(ev);
    //});
}

