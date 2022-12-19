


//█▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
//█▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█
//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE

//hardcoded list of countries
//used for easy life event location
window.countries = ["Afghanistan", "Aland Islands", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bonaire, Sint Eustatius and Saba", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, Democratic Republic of the Congo", "Cook Islands", "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Curacao", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Island and Mcdonald Islands", "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran, Islamic Republic of", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kosovo", "Kuwait", "Kyrgyzstan", "Lao People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macao", "Macedonia, the Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Palestinian Territory, Occupied", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Barthelemy", "Saint Helena", "Saint Kitts and Nevis", "Saint Lucia", "Saint Martin", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Serbia and Montenegro", "Seychelles", "Sierra Leone", "Singapore", "Sint Maarten", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Timor-Leste", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Viet Nam", "Virgin Islands, British", "Virgin Islands, U.s.", "Wallis and Futuna", "Western Sahara", "Yemen", "Zambia", "Zimbabwe"];


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
//so far only shows previous if on
//first visit from another page 
//todo test window.location.origin also possible to use
var getOriginUrl = () => document.referrer;

//return array of local storage keys
var getAllLocalStorageKeys = () => Object.keys(localStorage);

//Jquery to show inputed element
//by class and ID (CSS selector)
function showWrapper(element) {
    console.log(`JS: showWrapper`);
    $(element).show();
};

//function setTitleWrapper(newTitle) {
//    console.log(`JS: setTitleWrapper`);
//    //this has to be done like this for it to work
//    $(function () {
//        $(document).attr("title", newTitle);
//    });
//};

//var setTitleWrapper = (title) => { document.title = title; };


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
        //editable: false,
        layout: "fitColumns",      //fit columns to width of table
        responsiveLayout: "hide",  //hide columns that don't fit on the table
        //tooltips: true,            //show tool tips on cells
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
            { title: "Id", field: "id", hozAlign: "center" },
        ],
    });

    //handler when table row is clicked
    table.on("rowClick", function (e, row) {
        //get person name
        let personId = row._row.data.hash;
        //send user to person editor page with clicked person
        window.location.href = `/personeditor/${personId}`;
    });

    //same as click handler but for touch
    table.on("rowTap", function (e, row) {
        //get person name
        let personId = row._row.data.hash;
        //send user to person editor page with clicked person
        window.location.href = `/personeditor/${personId}`;
    });

}

function generatePlanetDataTable(tableId, tableData) {

    //set table data
    var table = new Tabulator(`#${tableId}`, {

        data: tableData,           //load row data from array
        //editable: false,
        layout: "fitDataTable",      //fit columns to width of table
        //tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: false,             //allow undo and redo actions on the table
        pagination: false,       //paginate the data
        movableColumns: false,      //allow column order to be changed
        resizableRows: false,       //allow row order to be changed
        //autoColumns: true,
        columns: [                 //define the table columns
            { title: "Planet", field: "planet", hozAlign: "center", frozen: true, responsive: 0, minWidth: "120px", cssClass: "PlanetColumn" },
            { title: "Total Strength", field: "shadbalaPinda", hozAlign: "center", responsive: 0 },
            { title: "Position Strength", field: "positionStrength", hozAlign: "center", responsive: 0 },
            { title: "Directional Strength", field: "directionalStrength", hozAlign: "center", responsive: 0 },
            { title: "Motional Strength", field: "motionalStrength", hozAlign: "center", responsive: 0 },
            { title: "Motion Name", field: "motionName", hozAlign: "center", responsive: 0 },
            { title: "Temporal Strength", field: "temporalStrength", hozAlign: "center", responsive: 0 },
            { title: "Aspect Strength", field: "aspectStrength", hozAlign: "center", responsive: 0 },
            { title: "Permanent Strength", field: "permanentStrength", hozAlign: "center", responsive: 0 },
            { title: "Current House", field: "planetCurrentHouse", hozAlign: "center", responsive: 0 },
            { title: "Sign", field: "planetCurrentSign", responsive: 0 },
            { title: "Current Navamsa Sign", field: "planetCurrentNavamsaSign", hozAlign: "center", responsive: 0 },
            { title: "Current Constellation", field: "planetCurrentConstellation", hozAlign: "center", responsive: 0 },
            { title: "Malefic Conjunct", field: "isPlanetConjunctWithMaleficPlanets", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Malefic Aspect", field: "isPlanetAspectedByMaleficPlanets", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Conjunct Planets", field: "conjunctPlanets", hozAlign: "center", responsive: 0 },
            { title: "Aspecting Planets", field: "aspectingPlanets", hozAlign: "center", responsive: 0 },
        ],
    });
}

//table with more prediction
function generatePlanetDataInfoTable(tableId, tableData) {

    //set table data
    var table = new Tabulator(`#${tableId}`, {

        data: tableData,           //load row data from array
        //editable: false,
        layout: "fitDataTable",      //fit columns to width of table
        //tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: false,             //allow undo and redo actions on the table
        pagination: false,       //paginate the data
        movableColumns: false,      //allow column order to be changed
        resizableRows: false,       //allow row order to be changed
        //autoColumns: true,
        columns: [                 //define the table columns
            { title: "Planet", field: "planet", hozAlign: "center", frozen: true, responsive: 0, minWidth: "120px", cssClass: "PlanetColumn" },
            { title: "House Type", field: "planetCurrentHouseType", responsive: 0 },
            { title: "House Relation", field: "houseRelation", hozAlign: "center", responsive: 0 },
            { title: "Navamsa Relation", field: "navamsaRelation", hozAlign: "center", responsive: 0 },
            { title: "Benefic", field: "isPlanetBeneficToLagna", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Yogakaraka", field: "isPlanetYogakarakaToLagna", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Malefic", field: "isPlanetMaleficToLagna", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Maraka", field: "isPlanetMarakaToLagna", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Planet Info", width: 250, resizable: true, field: "planetInfo", responsive: 0 },
            { title: "House Info", width: 250, resizable: true, field: "currentHouseDescription", responsive: 0 },
            { title: "Sign Info", width: 250, resizable: true, field: "currentSignDescription", responsive: 0 },
        ],
    });
}

function generateHouseDataTable(tableId, tableData) {

    //set table data
    var table = new Tabulator(`#${tableId}`, {
        data: tableData, //load row data from array
        //editable: false,
        layout: "fitDataTable", //fit columns to width of table
        //tooltips: true,            //show tool tips on cells
        addRowPos: "top", //when adding a new row, add it to the top of the table
        history: false, //allow undo and redo actions on the table
        pagination: false, //paginate the data
        movableColumns: false, //allow column order to be changed
        resizableRows: false, //allow row order to be changed
        columns: [//define the table columns
            {
                title: "House",
                field: "house",
                hozAlign: "center",
                frozen: true,
                responsive: 0,
                minWidth: "120px",
                cssClass: "PlanetColumn"
            },
            { title: "Strength", field: "houseStrength", hozAlign: "center", responsive: 0 },
            { title: "Sign", field: "houseSign", hozAlign: "center", responsive: 0 },
            { title: "Navamsa Sign", field: "houseNavamsaSign", hozAlign: "center", responsive: 0 },
            { title: "Lord", field: "houseLord", hozAlign: "center", responsive: 0 },
            { title: "Lord Exalted", field: "lordExalted", hozAlign: "center", formatter: "tickCross", responsive: 0 },
            {
                title: "Lord Debilitated",
                field: "lordDebilitated",
                hozAlign: "center",
                formatter: "tickCross",
                responsive: 0
            },
            { title: "Planets Inside", field: "planetsInHouse", hozAlign: "center", responsive: 0 },
            { title: "Planets Aspecting House", field: "planetsAspectingHouse", hozAlign: "center", responsive: 0 },

        ],
    });


}

function generateHouseDataInfoTable(tableId, tableData) {

    //set table data
    var table = new Tabulator(`#${tableId}`, {
        data: tableData, //load row data from array
        //editable: false,
        layout: "fitDataTable", //fit columns to width of table
        //tooltips: true,            //show tool tips on cells
        addRowPos: "top", //when adding a new row, add it to the top of the table
        history: false, //allow undo and redo actions on the table
        pagination: false, //paginate the data
        movableColumns: false, //allow column order to be changed
        resizableRows: false, //allow row order to be changed
        columns: [//define the table columns
            {
                title: "House",
                field: "house",
                hozAlign: "center",
                frozen: true,
                responsive: 0,
                minWidth: "120px",
                cssClass: "PlanetColumn"
            },
            { title: "Lord Info", width: 450, resizable: true, field: "lordInfo", responsive: 0 },
            { title: "House Info", width: 450, resizable: true, field: "houseDetails", responsive: 0 },
            { title: "Sign Info", width: 450, resizable: true, field: "currentSignDescription", responsive: 0 },
        ],
    });


}

//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
function generateLifeEventListTable(tableId, tableData) {

    //set table data
    window.lifeEventsListTable = new Tabulator(`#${tableId}`, {
        data: tableData,           //load row data from array
        //editable: true,
        layout: "fitDataStretch",  // fit their data, and stretch the final column
        //responsiveLayout: "hide",  //hide columns that don't fit on the table
        //tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: false,             //allow undo and redo actions on the table
        //pagination: false, //enable pagination
        pagination: "local",       //paginate the data
        paginationSize: 25,         //allow 7 rows per page of data
        paginationCounter: "rows", //display count of paginated rows in footer
        movableColumns: false,      //allow column order to be changed
        resizableRows: true,       //allow row order to be changed
        initialSort: [             //set the initial sort order of the data
            { column: "StartTime", dir: "desc" },
        ],
        //define the table columns
        columns: [
            //code to delete button for row
            {
                formatter: "buttonCross", hozAlign: "center", width: 40, cellClick: function (e, cell) {
                    cell.getRow().delete();
                }
            },
            { title: "Name", field: "Name", editor: "input", hozAlign: "center" },
            {
                title: "Time", field: "StartTime", editor: "datetime", editorParams: { format: "HH:mm dd/MM/yyyy" }, sorter: "datetime", sorterParams: {
                    format: "HH:mm dd/MM/yyyy",
                    elementAttributes: {
                        title: "slide bar to choose option" // custom tooltip
                    },
                    alignEmptyValues: "top",
                }
            },
            {
                title: "Location", field: "Location", editor: "list", hozAlign: "center", editorParams: {
                    //Value Options (You should use ONE of these per editor)
                    values: window.countries, //an array of country names
                    valuesLookup: "active", //get the values from the currently active rows in this column
                    autocomplete: "true", allowEmpty: true, listOnEmpty: true
                }
            },
            {
                title: "Nature", field: "Nature", editor: "list", hozAlign: "center", editorParams: {
                    //Value Options (You should use ONE of these per editor)
                    values: ["Good", "Neutral", "Bad"], //an array of values or value/label objects
                    valuesLookup: "active", //get the values from the currently active rows in this column
                }
            },
            { title: "Description", field: "Description", editor: "input" },

        ]
    });

}

//gets data out of events list table, to be used by blazor
function getLifeEventsListTableData() {
    return window.lifeEventsListTable.getData();
}

//adds new row to life events table
//note: defaults are set here, maybe move to blazor side for conformity
function addNewLifeEventToTable(defaultNewLifeEventStr) {

    //call blazor handler
    //var dateTimeStr = DotNet.invokeMethod('Website', 'GetNowTimeString');
    const defaultNewLifeEvent = JSON.parse(defaultNewLifeEventStr);
    var addToTopOfTable = true;
    window.lifeEventsListTable.addData([defaultNewLifeEvent], addToTopOfTable);
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

//TODO MARKED FOR DELETION ONCE SVG VERSION IMPLEMENTED
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
var getTextWrapper = (element) => $(element).text();
var getValueWrapper = (element) => $(element).val();
var setValueWrapper = (element, value) => $(element).val(value);
var IsOnline = () => window.navigator.onLine;

function getPropWrapper(element, propName) {
    let propVal = $(element).prop(propName);
    console.log(`JS: getPropWrapper : ${propName} : ${propVal}`);
    return propVal;
};

window.setPropWrapper = function (element, propName, propVal) {
    $(element).prop(propName, propVal);
    console.log(`JS: setPropWrapper : ${propName} : ${propVal}`);
    return propVal;
};
window.setAttrWrapper = function (element, propName, propVal) {
    $(element).attr(propName, propVal);
    console.log(`JS: setAttrWrapper : ${propName} : ${propVal}`);
    return propVal;
};

window.setCssWrapper = function (element, propName, propVal) {
    $(element).css(propName, propVal);
    console.log(`JS: setCssWrapper : ${propName} : ${propVal}`);
    return propVal;
};
window.getElementWidth = function (element, parm) {
    console.log(`JS : getElementWidth : ${element.offsetWidth}`);
    return element.offsetWidth;
};

//adds a width value to every child found in given element
window.addWidthToEveryChild = function (element, widthToAdd) {
    console.log(`JS: addWidthToEveryChild`);

    //add to each child of input element
    $(element).children().each(function () {
        //get current width
        let currentWidth = $(this).width();

        //set new width with input value
        $(this).width(currentWidth + widthToAdd);
    });
};


//scrolls to div on page and flashes div using JS
function scrollToDiv(elemId) {

    //scroll to element
    document.getElementById(elemId).scrollIntoView();

    //use JS to attarct attention to div
    var idString = `#${elmId}`;
    $(idString).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);

    $(idString).fadeTo(100, 0.3, function () { $(this).fadeTo(500, 1.0); });
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


//on mouse leave event chart, auto hide time legend
//attached by Blazor
async function onMouseLeaveEventChart(mouse) {
    $("#CursorLine").hide();
}

//fired when mouse moves over dasa view box
//used to auto update cursor line & time legend
async function onMouseMoveDasaViewEventHandler(mouse) {

    //get relative position of mouse in Dasa view
    var mousePosition = GetMousePositionInElement(mouse, "#EventChartHolder");

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

//event fired from autoUpdateTimeLegend
$(document).on('loadEventDescription', LoadEventDescription);

function LoadEventDescription(event, eventName) {

    //off events while firing
    $(document).off('loadEventDescription');

    //fill description box about event
    getEventDescription(eventName.replace(/ /g, ""))
        .then((eventDesc) => {
            //if no description than hide box & end here
            if (!eventDesc) { window.showDescription = false; return; }
            var wrappedDescText = createSVGtext(eventDesc, 175, 24);
            $("#CursorLineLegendDescription").empty(); //clear previous desc
            $(wrappedDescText).appendTo("#CursorLineLegendDescription"); //add in new desc
        });

    //turn events back on
    $(document).on('loadEventDescription', LoadEventDescription);

}

//SVG Event Chart Time Legend generator
//this is where the whole time legend that follows
//the mouse when placed on chart is generated
//notes: a template row always exists in code,
//in client JS side uses template to create the rows from cloning it
//and modifying its prop as needed, as such any major edit needs to
//be done in API code
function autoUpdateTimeLegend(mousePosition) {

    //x axis is rounded because axis value in rect is whole numbers
    //and it has to be exact match to get it
    var mouseRoundedX = Math.round(mousePosition.xAxis);
    var mouseRoundedY = Math.round(mousePosition.yAxis);

    //use the mouse position to get all dasa rect
    //dasa elements at same X position inside the dasa svg
    //note: faster and less erroneous than using mouse.path
    var children = $("#EventChartHolder").children();
    var allElementsAtX = children.find(`[x=${mouseRoundedX}]`);

    //template used to generate legend rows
    var holderTemplateId = `#CursorLineLegendTemplate`;

    //delete previously generated legend rows
    $(".CursorLineLegendClone").remove();


    //count good and bad events for summary row
    var goodCount = 0;
    var badCount = 0;
    var yAxis = 0;
    window.showDescription = false;//default description not shown
    //extract event data out and place it in legend
    allElementsAtX.each(function () {

        //1 GET DATA
        //extract other data out of the rect
        var eventName = this.getAttribute("eventname");
        //if no "eventname" exist, wrong elm skip it
        if (!eventName) { return; }

        var color = this.getAttribute("fill");
        yAxis = parseInt(this.getAttribute("y"));//parse as num, for calculation

        //count good and bad events for summary row
        var eventNatureName = "";// used later for icon color
        if (color === "#FF0000") { eventNatureName = "Bad", badCount++; }
        if (color === "#00FF00") { eventNatureName = "Good", goodCount++; }


        //2 TIME & AGE LEGEND
        //create time legend at top only for first element
        if (allElementsAtX[0] === this) {
            var newTimeLegend = $(holderTemplateId).clone();
            newTimeLegend.removeAttr('id'); //remove the clone template id
            newTimeLegend.addClass("CursorLineLegendClone"); //to delete it on next run
            newTimeLegend.appendTo("#CursorLineLegendHolder"); //place new legend into special holder
            newTimeLegend.show();//make cloned visible
            newTimeLegend.attr('transform', `matrix(1, 0, 0, 1, 10, ${yAxis - 15})`); //above 1st row
            //split time to remove timezone from event
            var stdTimeFull = this.getAttribute("stdtime");
            var stdTimeSplit = stdTimeFull.split(" ");
            var hourMin = stdTimeSplit[0];
            var date = stdTimeSplit[1];
            var timezone = stdTimeSplit[2];
            var age = this.getAttribute("age");
            newTimeLegend.children("text").text(`${hourMin} ${date}  AGE: ${age}`);
            //replace circle with clock icon
            newTimeLegend.children("use").attr("xlink:href", "#CursorLineClockIcon");
        }

        //3 GENERATE EVENT ROW
        //make a copy of template for this event
        var newLegendRow = $(holderTemplateId).clone();
        newLegendRow.removeAttr('id'); //remove the clone template id
        newLegendRow.addClass("CursorLineLegendClone"); //to delete it on next run
        newLegendRow.appendTo("#CursorLineLegendHolder"); //place new legend into special holder
        newLegendRow.show();//make cloned visible
        //position the group holding the legend over the event row which the legend represents
        newLegendRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${yAxis})`);

        //set event name text & color element
        var textElm = newLegendRow.children("text");
        var iconElm = newLegendRow.children("use");
        textElm.text(`${eventName}`);
        iconElm.attr("xlink:href", `#CursorLine${eventNatureName}Icon`); //set icon color based on nature

        //4 GENERATE DESCRIPTION ROW LOGIC
        //check if mouse in within row of this event (y axis)
        var elementTopY = yAxis;
        var elementBottomY = yAxis + 15;
        var mouseWithinRow = mouseRoundedY >= elementTopY && mouseRoundedY <= elementBottomY;
        //if event name is still the same then don't load description again
        var notSameEvent = window.previousHoverEventName !== eventName;

        //if mouse is in event's row then highlight that row
        if (mouseWithinRow) {
            //highlight event name row
            var backgroundElm = newLegendRow.children("rect");
            backgroundElm.css("fill", "white");
            textElm.css("fill", "black");
            textElm.css("font-weight", "700");
            //if mouse within show description box
            window.showDescription = true;
        }

        //if mouse within row AND the event has changed
        //then generate a new description
        //note: this is slow, so done only when absolutely needed
        if (mouseWithinRow && notSameEvent) {

            //make holder visible
            $("#CursorLineLegendDescriptionHolder").show();

            //note: using trigger to make it easy to skip multiple clogging events
            $(document).trigger('loadEventDescription', eventName);

            //update previous hover event
            window.previousHoverEventName = eventName;
        }

    });

    //auto show/hide description box based on mouse position
    if (window.showDescription) {
        $("#CursorLineLegendDescriptionHolder").show();
    } else {
        $("#CursorLineLegendDescriptionHolder").hide();
    }



    //5 GENERATE LAST SUMMARY ROW
    //generate summary row at the bottom
    //make a copy of template for this event
    var newSummaryRow = $(holderTemplateId).clone();
    newSummaryRow.removeAttr('id'); //remove the clone template id
    newSummaryRow.addClass("CursorLineLegendClone"); //to delete it on next run
    newSummaryRow.appendTo("#CursorLineLegendHolder"); //place new legend into parent
    newSummaryRow.show();//make cloned visible
    //position the group holding the legend over the event row which the legend represents
    newSummaryRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${yAxis + 1 + 15})`);

    //set event name text & color element
    var textElm = newSummaryRow.children("text");
    textElm.text(` Good : ${goodCount}   Bad : ${badCount}`);
    //change icon to summary icon
    newSummaryRow.children("use").attr("xlink:href", "#CursorLineSumIcon");

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
//for viewing in time legend
async function getEventDescription(eventName) {

    //search for matching event name
    var eventXmlList = window.EventDataListXml.find('Event'); //get all event elements
    var results = eventXmlList.filter(
        function () {
            var eventNameXml = $(this).children('Name').eq(0);
            return eventNameXml.text() === eventName;
        });

    var eventDescription = results.eq(0).children('Description').eq(0).text();

    //remove tabs and new line to make easy detection of empty string
    let cleaned = eventDescription.replace(/ {4}|[\t\n\r]/gm, '');
    return cleaned;

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

function DrawPlanetStrengthChart(sun, moon, mercury, mars, jupiter, saturn, venus) {

    //delete previous chart if any
    if (window.PlanetStrengthChart != null) { window.PlanetStrengthChart.destroy(); }

    var xValues = ["Sun", "Moon", "Mercury", "Mars", "Jupiter", "Saturn", "Venus"];
    var yValues = [sun, moon, mercury, mars, jupiter, saturn, venus];

    //this chart elm ID is hard coded in Blazor
    //note: stored in window so that can delete it on redraw
    window.PlanetStrengthChart = new Chart("PlanetChart",
        {
            type: "bar",
            data: {
                xAxisID: "Planets",
                yAxisID: "Strength",
                labels: xValues,
                datasets: [
                    {
                        data: yValues,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.7)',
                            'rgba(255, 159, 64, 0.7)',
                            'rgba(255, 205, 86, 0.7)',
                            'rgba(75, 192, 192, 0.7)',
                            'rgba(54, 162, 235, 0.7)',
                            'rgba(153, 102, 255, 0.7)',
                            'rgba(201, 203, 207, 0.7)'
                        ],
                        borderColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 159, 64)',
                            'rgb(255, 205, 86)',
                            'rgb(75, 192, 192)',
                            'rgb(54, 162, 235)',
                            'rgb(153, 102, 255)',
                            'rgb(201, 203, 207)'
                        ],
                        borderWidth: 1
                    }
                ]
            },
            options: {
                animation: false,// disables all animations
                scales: {
                    y: {
                        min: round(Math.min.apply(this, yValues) - 50),
                        max: round(Math.max.apply(this, yValues) + 50)
                    }
                },
                plugins: {
                    legend: {
                        display: false,
                    }
                }
            }
        });


    //make the chart more beautiful if your stepSize is 5
    function round(x) {
        return Math.ceil(x / 5) * 5;
    }
}

function DrawHouseStrengthChart(_house1,
    _house2,
    _house3,
    _house4,
    _house5,
    _house6,
    _house7,
    _house8,
    _house9,
    _house10,
    _house11,
    _house12) {

    //delete previous chart if any
    if (window.HouseStrengthChart != null) { window.HouseStrengthChart.destroy(); }

    var xValues = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
    var yValues = [_house1,
        _house2,
        _house3,
        _house4,
        _house5,
        _house6,
        _house7,
        _house8,
        _house9,
        _house10,
        _house11,
        _house12];

    //this chart elm ID is hard coded in Blazor
    //note: stored in window so that can delete it on redraw
    window.HouseStrengthChart = new Chart("HouseChart",
        {
            type: "bar",
            data: {
                labels: xValues,
                datasets: [
                    {
                        data: yValues,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.7)',
                            'rgba(255, 159, 64, 0.7)',
                            'rgba(255, 205, 86, 0.7)',
                            'rgba(75, 192, 192, 0.7)',
                            'rgba(54, 162, 235, 0.7)',
                            'rgba(153, 102, 255, 0.7)',
                            'rgba(201, 203, 207, 0.7)'
                        ],
                        borderColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 159, 64)',
                            'rgb(255, 205, 86)',
                            'rgb(75, 192, 192)',
                            'rgb(54, 162, 235)',
                            'rgb(153, 102, 255)',
                            'rgb(201, 203, 207)'
                        ],
                        borderWidth: 1
                    }
                ]
            },
            options: {
                animation: false,// disables all animations
                scales: {
                    y: {
                        min: round(Math.min.apply(this, yValues) - 50),
                        max: round(Math.max.apply(this, yValues) + 50)
                    }
                },
                plugins: {
                    legend: {
                        display: false,
                    }
                }
            }
        });


    //make the chart more beautiful if your stepSize is 5
    function round(x) {
        return Math.ceil(x / 5) * 5;
    }
}

//copies inputed text to clipboard
//used for copying direct link to chart
function CopyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        //alert("Copied to clipboard!");
    })
        .catch(function (error) {
            alert(error);
        });
}

function EventsChartInit() { EventsChart.animateEventsChart(); }

class EventsChart {
    //template used to generate legend rows
    static holderTemplateId = `#CursorLineLegendTemplate`;

    //get screen width
    static getScreenWidth() {
        return Math.max(
            document.body.scrollWidth,
            document.documentElement.scrollWidth,
            document.body.offsetWidth,
            document.documentElement.offsetWidth,
            document.documentElement.clientWidth
        );
    }

    //gets clients timezone offset, exp:"+08:00"
    static getParseableTimezone() {
        var date = new Date();
        var tzo = -date.getTimezoneOffset(),
            dif = tzo >= 0 ? '+' : '-',
            pad = function (num) {
                return (num < 10 ? '0' : '') + num;
            };

        return dif +
            pad(Math.floor(Math.abs(tzo) / 60)) +
            ':' +
            pad(Math.abs(tzo) % 60);
    }

    static getDataFromToolbar() {

        //get needed data out of selection boxes
        var chartData = {
            "timePreset": $("#TimePresetSelector").val(),
            "eventPreset": $("#EventPresetSelector").val(),
            "personId": window.PersonId,
            "maxWidth": EventsChart.getScreenWidth(),
            "timezone": EventsChart.getParseableTimezone(),
            "url": ""//empty string first
        };

        //reset parts of the URL with data from selection boxes
        var pathname = window.location.pathname.split("/");
        pathname[pathname.length - 1] = chartData.timePreset;
        pathname[pathname.length - 2] = chartData.eventPreset;
        pathname[pathname.length - 3] = chartData.personId;

        //construct the final URL back
        var newUrl = pathname.join('/');
        chartData.url = newUrl;
        return chartData;
    }

    static getDataFromUrl() {

        //get data out of url
        var pathname = window.location.pathname.split("/");
        //url pattern chart/{personId}/{eventPreset}/{timePreset}
        var returnVal = {
            "timePreset": pathname[pathname.length - 1],
            "eventPreset": pathname[pathname.length - 2],
            "personId": pathname[pathname.length - 3],
            "maxWidth": EventsChart.getScreenWidth(),
            "timezone": EventsChart.getParseableTimezone()
        };

        return returnVal;
    }

    //fired when mouse moves over dasa view box
    //used to auto update cursor line & time legend
    static async onMouseMoveHandler(mouse) {
        //console.log(`JS: onMouseMoveDasaViewEventHandler`);

        //get relative position of mouse in Dasa view
        var mousePosition = getMousePositionInElement(mouse, "#EventChartHolder");

        //if mouse is out of dasa view hide cursor and end here
        if (mousePosition == 0) { $("#CursorLine").hide(); return; }
        else { $("#CursorLine").show(); }

        //move cursor line 1st for responsiveness
        autoMoveCursorLine(mousePosition.xAxis);

        //update time legend
        generateTimeLegend(mousePosition);

        //FUNCTIONS
        //Gets a mouses x axis relative inside the given element
        //used to get mouse location on Dasa view
        //returns 0 when mouse is out
        function getMousePositionInElement(mouseEventData, elementId) {
            //console.log(`JS: GetMousePositionInElement`);

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

        function autoMoveCursorLine(relativeMouseX) {
            //console.log(`JS: autoMoveCursorLine`);

            //move vertical line to under mouse inside dasa view box
            $("#CursorLine").attr('transform', `matrix(1, 0, 0, 1, ${relativeMouseX}, 0)`);

        }

        //SVG Event Chart Time Legend generator
        //this is where the whole time legend that follows
        //the mouse when placed on chart is generated
        //notes: a template row always exists in code,
        //in client JS side uses template to create the rows from cloning it
        //and modifying its prop as needed, as such any major edit needs to
        //be done in API code
        function generateTimeLegend(mousePosition) {
            //console.log(`JS: autoUpdateTimeLegend`);

            //x axis is rounded because axis value in rect is whole numbers
            //and it has to be exact match to get it
            var mouseRoundedX = Math.round(mousePosition.xAxis);
            var mouseRoundedY = Math.round(mousePosition.yAxis);

            //use the mouse position to get all dasa rect
            //dasa elements at same X position inside the dasa svg
            //note: faster and less erroneous than using mouse.path
            var children = $("#EventChartHolder").children();
            var allElementsAtX = children.find(`[x=${mouseRoundedX}]`);


            //delete previously generated legend rows
            $(".CursorLineLegendClone").remove();


            //count good and bad events for summary row
            var goodCount = 0;
            var badCount = 0;
            var newRowYAxis = 0; //vertical position of the current event row
            window.showDescription = false;//default description not shown
            //extract event data out and place it in legend
            allElementsAtX.each(drawEventRow);

            //auto show/hide description box based on mouse position
            if (window.showDescription) {
                $("#CursorLineLegendDescriptionHolder").show();
            } else {
                $("#CursorLineLegendDescriptionHolder").hide();
            }



            //5 GENERATE LAST SUMMARY ROW
            //generate summary row at the bottom
            //make a copy of template for this event
            var newSummaryRow = $(EventsChart.holderTemplateId).clone();
            newSummaryRow.removeAttr('id'); //remove the clone template id
            newSummaryRow.addClass("CursorLineLegendClone"); //to delete it on next run
            newSummaryRow.appendTo("#CursorLineLegendHolder"); //place new legend into parent
            newSummaryRow.show();//make cloned visible
            //position the group holding the legend over the event row which the legend represents
            newSummaryRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis + 15 - 1})`);//minus 1 for perfect alignment

            //set event name text & color element
            var textElm = newSummaryRow.children("text");
            textElm.text(` Good : ${goodCount}   Bad : ${badCount}`);
            //change icon to summary icon
            newSummaryRow.children("use").attr("xlink:href", "#CursorLineSumIcon");

            //FUNCTIONS

            //
            function drawEventRow() {

                //this is the rect generated by API containing data about the event
                var svgEventRect = this;

                //1 GET DATA
                //extract other data out of the rect
                var eventName = svgEventRect.getAttribute("eventname");
                //if no "eventname" exist, wrong elm skip it
                if (!eventName) { return; }

                var color = svgEventRect.getAttribute("fill");
                newRowYAxis = parseInt(svgEventRect.getAttribute("y"));//parse as num, for calculation

                //count good and bad events for summary row
                var eventNatureName = "";// used later for icon color
                if (color === "#FF0000") { eventNatureName = "Bad", badCount++; }
                if (color === "#00FF00") { eventNatureName = "Good", goodCount++; }


                //2 TIME & AGE LEGEND
                //create time legend at top only for first element
                if (allElementsAtX[0] === svgEventRect) { drawTimeAgeLegendRow(); }

                //3 GENERATE EVENT ROW
                //make a copy of template for this event
                var newLegendRow = $(EventsChart.holderTemplateId).clone();
                newLegendRow.removeAttr('id'); //remove the clone template id
                newLegendRow.addClass("CursorLineLegendClone"); //to delete it on next run
                newLegendRow.appendTo("#CursorLineLegendHolder"); //place new legend into special holder
                newLegendRow.show();//make cloned visible
                //position the group holding the legend over the event row which the legend represents
                newLegendRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis - 2})`);//minus 2 for perfect alignment

                //set event name text & color element
                var textElm = newLegendRow.children("text");
                var iconElm = newLegendRow.children("use");
                textElm.text(`${eventName}`);
                iconElm.attr("xlink:href", `#CursorLine${eventNatureName}Icon`); //set icon color based on nature

                //4 GENERATE DESCRIPTION ROW LOGIC
                //check if mouse in within row of this event (y axis)
                var elementTopY = newRowYAxis;
                var elementBottomY = newRowYAxis + 15;
                var mouseWithinRow = mouseRoundedY >= elementTopY && mouseRoundedY <= elementBottomY;
                //if event name is still the same then don't load description again
                var notSameEvent = window.previousHoverEventName !== eventName;

                //HIGHLIGHT ROW
                //if mouse is in event's row then highlight that row
                if (mouseWithinRow) {
                    //highlight event name row
                    var backgroundElm = newLegendRow.children("rect");
                    backgroundElm.css("fill", "#003e99");
                    backgroundElm.css("opacity", "1");//solid
                    //textElm.css("fill", "#fff");
                    textElm.css("font-weight", "700");
                    //if mouse within show description box
                    window.showDescription = true;
                }

                //if mouse within row AND the event has changed
                //then generate a new description
                //note: this is slow, so done only when absolutely needed
                if (mouseWithinRow && notSameEvent) {

                    //make holder visible
                    $("#CursorLineLegendDescriptionHolder").show();

                    //move holder next to event
                    var descBoxYAxis = newRowYAxis - 9;//minus 5 for perfect alignment with event name row
                    $("#CursorLineLegendDescriptionHolder").attr("transform", `matrix(1, 0, 0, 1, 0, ${descBoxYAxis})`);

                    //note: using trigger to make it easy to skip multiple clogging events
                    $(document).trigger('loadEventDescription', eventName);

                    //update previous hover event
                    window.previousHoverEventName = eventName;
                }


                function drawTimeAgeLegendRow() {
                    var newTimeLegend = $(EventsChart.holderTemplateId).clone();
                    newTimeLegend.removeAttr('id'); //remove the clone template id
                    newTimeLegend.addClass("CursorLineLegendClone"); //to delete it on next run
                    newTimeLegend.appendTo("#CursorLineLegendHolder"); //place new legend into special holder
                    newTimeLegend.show();//make cloned visible
                    newTimeLegend.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis - 15})`); //above 1st row
                    //split time to remove timezone from event
                    var stdTimeFull = svgEventRect.getAttribute("stdtime");
                    var stdTimeSplit = stdTimeFull.split(" ");
                    var hourMin = stdTimeSplit[0];
                    var date = stdTimeSplit[1];
                    var timezone = stdTimeSplit[2];
                    var age = svgEventRect.getAttribute("age");
                    newTimeLegend.children("text").text(`${hourMin} ${date}  AGE: ${age}`);
                    //replace circle with clock icon
                    newTimeLegend.children("use").attr("xlink:href", "#CursorLineClockIcon");
                }

            }


        }

    }

    //on mouse leave event chart, auto hide time legend
    static async onMouseLeaveEventChart(mouse) {
        $("#CursorLine").hide();
    }

    //gets chart from API & injects it into page
    static async getEventsChartFromApi(chartData) {
        console.log(`Getting events chart from API...`);

        var payload = `<Root><PersonId>${chartData.personId}</PersonId><TimePreset>${chartData.timePreset}</TimePreset><EventPreset>${chartData.eventPreset}</EventPreset><Timezone>${chartData.timezone}</Timezone><MaxWidth>${chartData.maxWidth}</MaxWidth></Root>`;

        var response = await fetch("https://vedastroapi.azurewebsites.net/api/geteventscharteasy", {
            "headers": {
                "accept": "*/*",
                "accept-language": "en-GB,en-US;q=0.9,en;q=0.8",
                "content-type": "plain/text; charset=utf-8",
                "sec-ch-ua": "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"",
                "sec-ch-ua-mobile": "?0",
                "sec-ch-ua-platform": "\"Windows\"",
                "sec-fetch-dest": "empty",
                "sec-fetch-mode": "cors",
                "sec-fetch-site": "cross-site"
            },
            "referrer": "https://www.vedastro.org/",
            "referrerPolicy": "strict-origin-when-cross-origin",
            "body": payload,
            "method": "POST",
            "mode": "cors",
            "credentials": "omit"
        });

        //inject new svg chart into page
        var svgChartString = await response.text();
        injectIntoElement($("#DasaViewBox")[0], svgChartString);


        //LOCAL FUNCTIONS
        function injectIntoElement(element, valueToInject) {
            console.log(`Injecting SVG Chart into page...`);

            //convert string to html node
            var template = document.createElement("template");
            template.innerHTML = valueToInject;
            var svgElement = template.content.firstElementChild;

            //save for later use
            EventsChart.Element = svgElement;

            //place new node in parent
            element.innerHTML = ''; //clear current children if any
            element.appendChild(EventsChart.Element);
        }

    }

    static loadEventDescription(event, eventName) {

        //off events while firing
        $(document).off('loadEventDescription');

        //fill description box about event
        getEventDescription(eventName.replace(/ /g, "")).then(drawDescriptionBox);

        //turn events back on
        $(document).on('loadEventDescription', EventsChart.loadEventDescription);


        //FUNCTIONS
        function drawDescriptionBox(eventDesc) {
            //if no description than hide box & end here
            if (!eventDesc) { window.showDescription = false; return; }
            //convert text to svg and place inside holder
            var wrappedDescText = textToSvg(eventDesc, 175, 24);

            $("#CursorLineLegendDescription").empty(); //clear previous desc
            $(wrappedDescText).appendTo("#CursorLineLegendDescription"); //add in new desc
            //set height of desc box background
            $("#CursorLineLegendDescriptionBackground").attr("height", window.EventDescriptionTextHeight + 20); //plus little for padding

            //FUNCTION
            //  This function attempts to create a new svg "text" element, chopping
            //  it up into "tspan" pieces, if the caption is too long
            function textToSvg(caption, x, y) {
                //console.log(`JS: createSVGtext`);

                //svg "text" element to hold smaller text lines
                var svgTextHolder = document.createElementNS('http://www.w3.org/2000/svg', 'text');
                svgTextHolder.setAttributeNS(null, 'x', x);
                svgTextHolder.setAttributeNS(null, 'y', y);
                svgTextHolder.setAttributeNS(null, 'font-size', 10);
                svgTextHolder.setAttributeNS(null, 'fill', '#FFF');
                svgTextHolder.setAttributeNS(null, 'text-anchor', 'left');

                //The following two variables can be passed as parameters
                var maximumCharsPerLine = 30;
                var lineHeight = 10;

                var words = caption.split(" ");
                var line = "";

                //process text and create rows
                var svgTSpan;
                var lineCount = 0; //number of lines to calculate height
                var tSpanTextNode;
                for (var n = 0; n < words.length; n++) {
                    var testLine = line + words[n] + " ";
                    if (testLine.length > maximumCharsPerLine) {
                        //  Add a new <tspan> element
                        svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
                        svgTSpan.setAttributeNS(null, 'x', x);
                        svgTSpan.setAttributeNS(null, 'y', y);

                        tSpanTextNode = document.createTextNode(line);
                        svgTSpan.appendChild(tSpanTextNode);
                        svgTextHolder.appendChild(svgTSpan);

                        line = words[n] + " ";
                        y += lineHeight; //place next text row lower
                        lineCount++; //count a line
                    }
                    else {
                        line = testLine;
                    }
                }

                //calculate final height in px, save global to be accessed later
                window.EventDescriptionTextHeight = lineCount * lineHeight;
                //console.log(window.EventDescriptionTextHeight);

                svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
                svgTSpan.setAttributeNS(null, 'x', x);
                svgTSpan.setAttributeNS(null, 'y', y);

                tSpanTextNode = document.createTextNode(line);
                svgTSpan.appendChild(tSpanTextNode);

                svgTextHolder.appendChild(svgTSpan);

                return svgTextHolder;
            }

        }

        //gets events from EventDataList.xml
        //for viewing in time legend
        async function getEventDescription(eventName) {
            //console.log(`JS: getEventDescription`);

            //search for matching event name
            var eventXmlList = window.EventDataListXml.find('Event'); //get all event elements
            var results = eventXmlList.filter(
                function () {
                    var eventNameXml = $(this).children('Name').eq(0);
                    return eventNameXml.text() === eventName;
                });

            var eventDescription = results.eq(0).children('Description').eq(0).text();

            //remove tabs and new line to make easy detection of empty string
            let cleaned = eventDescription.replace(/ {4}|[\t\n\r]/gm, '');
            return cleaned;

        }

    }

    //needs to be run once before get event description method is used
    //loads xml file located in wwwroot to xml global data
    static async loadEventDataListFile() {

        //only load new file if none available
        if (!window.EventDataListXml) {
            //get data list from server and store it for later use
            //NOTE: CORS in Azure Website Storage needs to be disabled for this to work, outside of vedastro.org
            $(function () {
                $.ajax({
                    type: "get",
                    url: "https://www.vedastro.org/data/EventDataList.xml",
                    dataType: "xml",
                    success: function (data) {
                        //let user know
                        console.log(`Getting event data file from API...`);
                        //save for later
                        window.EventDataListXml = $(data); //jquery for use with .filter
                    },
                    error: function (xhr, status) {
                        /* handle error here */
                        console.log(status);
                    }
                });
            });
        } else {
            console.log(`Using cached event data file.`);
        }

    }

    //returns true if svg loaded
    static isSvgLoaded() {
        console.log(`Checking if SVG Chart is loaded...`);

        //check if svg already loaded into page
        var x = $("#DasaViewBox").children().first().is("svg");

        return x;
    }

    static hideLoading() {
        //hide default loading box
        $("#LoadingBox").hide();
        //show svg chart
        $("#DasaViewBox").show();
        //show toolbar
        $("#ToolBar").show();
    }

    static showLoading() {
        //show default loading box
        $("#LoadingBox").show();
        //hide svg chart
        $("#DasaViewBox").hide();
        //hide toolbar
        $("#ToolBar").hide();

    }

    static getScreenHeight() {
        return Math.max(
            document.body.scrollHeight,
            document.documentElement.scrollHeight,
            document.body.offsetHeight,
            document.documentElement.offsetHeight,
            document.documentElement.clientHeight
        );
    }

    //attaches all needed handlers to animate a chart
    static async initializeChart(chartElement$) {
        console.log("Attaching events to chart...");

        //save for easier reference
        EventsChart.Element = chartElement$;

        //attach mouse handler to auto move cursor line & update time legend
        //load event description file 1st
        await EventsChart.loadEventDataListFile();

        EventsChart.Element[0].addEventListener("mousemove", EventsChart.onMouseMoveHandler);

        EventsChart.Element[0].addEventListener("mouseleave", EventsChart.onMouseLeaveEventChart);

        //attach handler to load event description file before hand
        $(document).on('loadEventDescription', EventsChart.loadEventDescription);

        //save now line
        EventsChart.NowVerticalLine = EventsChart.Element.find('#NowVerticalLine');

        //update once now
        updateNowLine();

        //setup to auto update every 1 minute
        setInterval(function () { updateNowLine(); }, 60 * 1000); // 60*1000ms

        //update now line position
        function updateNowLine() {
            console.log("Updating now line position...");

            //get all rects
            var allEventRects = EventsChart.Element.find(".EventListHolder").children("rect");

            //find closest rect to now time
            var closestRectToNow;
            allEventRects.each(function (index) {
                //get parsed time from rect
                var svgEventRect = this;
                var rectTime = getTimeInRect(svgEventRect).getTime();//(milliseconds since 1 Jan 1970)
                var nowTime = Date.now();

                //if not yet reach continue, keep reference to this and goto next
                if (rectTime <= nowTime) {
                    closestRectToNow = svgEventRect;
                    return true; //go next
                }
                //already passed now time, use previous rect as now, stop looking
                else { return false; }
            });

            //get horizontal position of now rect (x axis)
            var xAxisNowRect = closestRectToNow.getAttribute("x");
            EventsChart.NowVerticalLine.attr('transform', `matrix(1, 0, 0, 1, ${xAxisNowRect}, 0)`);

        }

        //parses the STD time found in rect and returns it
        function getTimeInRect(eventRect$) {
            //convert "00:28 17/11/2022 +08:00" to "2019-01-01T00:00:00.000+00:00"
            var stdTimeRaw = eventRect$.getAttribute("stdtime");
            var stdTimeSplit = stdTimeRaw.split(" ");
            var hourMin = stdTimeSplit[0];
            var dateFull = stdTimeSplit[1].split('/');
            var date = dateFull[0];
            var month = dateFull[1];
            var year = dateFull[2];
            var timezone = stdTimeSplit[2];
            var rectTime = new Date(`${year}-${month}-${date}T${hourMin}:00.000${timezone}`);

            return rectTime;
        }
    }

    //coming from direct/url access page
    //this the method called to start the JS code to animate an already loaded chart
    static async animateEventsChart() {
        console.log(`Starting the engine...`);

        //set title for easy multi-tabbing (not chart related)
        document.title = `${window?.ChartType} | ${window?.PersonName}`;

        //set toolbar data (not chart related)
        $("#PersonNameBox").text("Person : " + window?.PersonName);

        var notLoaded = !EventsChart.isSvgLoaded();

        //try get svg from server if svg not loaded by now
        //coming from live chart generate
        if (notLoaded) {
            //get data to generate chart from URL
            var data = EventsChart.getDataFromUrl();

            await EventsChart.getEventsChartFromApi(data);

        }

        //attach mouse handler to auto move cursor line & update time legend
        //load event description file 1st
        var svgChart$ = $("#DasaViewBox").children().first();
        await EventsChart.initializeChart(svgChart$);

        //make toolbar visible
        $("#ToolBar").removeClass("visually-hidden");

        EventsChart.hideLoading();

    }
}
