//░█▀▀█ ░█─── ─█▀▀█ ░█▀▀▀█ ░█▀▀▀█ ░█▀▀█ 　 ▀█▀ ░█▄─░█ ▀▀█▀▀ ░█▀▀▀ ░█▀▀█ ░█▀▀▀█ ░█▀▀█
//░█▀▀▄ ░█─── ░█▄▄█ ─▄▄▄▀▀ ░█──░█ ░█▄▄▀ 　 ░█─ ░█░█░█ ─░█── ░█▀▀▀ ░█▄▄▀ ░█──░█ ░█▄▄█
//░█▄▄█ ░█▄▄█ ░█─░█ ░█▄▄▄█ ░█▄▄▄█ ░█─░█ 　 ▄█▄ ░█──▀█ ─░█── ░█▄▄▄ ░█─░█ ░█▄▄▄█ ░█───
// All code called from BLAZOR resides here

console.log(`INTEROP.js - Loaded`);

//functions used by localstorage manager in Blazor
export var getProperty = key => key in localStorage ? JSON.parse(localStorage[key]) : null;
export var removeProperty = key => key in localStorage ? localStorage.removeItem(key) : null;
export var setProperty = (key, value) => { localStorage[key] = JSON.stringify(value); };
export var removeClassWrapper = (element, classString) => $(element).removeClass(classString);
export var addClassWrapper = (element, classString) => $(element).addClass(classString);
export var toggleClassWrapper = (element, classString) => $(element).toggleClass(classString);
export var getTextWrapper = (element) => $(element).text();
export var getValueWrapper = (element) => $(element).val();
export var setValueWrapper = (element, value) => $(element).val(value);
export var IsOnline = () => window.navigator.onLine;
export var getAllLocalStorageKeys = () => Object.keys(localStorage); //return array of local storage keys
export var showAccordion = (id) => $(id).collapse("show");
export var toggleAccordion = (id) => $(id).collapse("toggle"); //Uses Bootstrap Jquery plugin to toggle any collapsible component by id
export var scrollIntoView = (id) => $(id)[0].scrollIntoView(); //scrolls element by id into view
export var highlightByEventName = (keyword) => window.EventsChartLoaded.highlightByEventName(keyword);
export var unhighlightByEventName = (keyword) => window.EventsChartLoaded.unhighlightByEventName(keyword);

const RETRY_COUNT = 5;



//--------------------------CALENDAR INPUT SELECTOR CODE
//DESCRIPTION
//This file stores all code fo js date picker (VanillaCalendar)
//To use: first load file via blazor
//then call LoadCalendar
//make sure empty calendar div exists

export function InitCalendarPicker() {

    //global space to store calendar related refs
    window.Calendar = {};

    //date input element
    window.Calendar.hourInputId = '#HourInput';
    window.Calendar.minuteInputId = '#MinuteInput';
    window.Calendar.meridianInputId = '#MeridianInput';
    window.Calendar.dateInputId = '#DateInput';
    window.Calendar.monthInputId = '#MonthInput';
    window.Calendar.yearInputId = '#YearInput';

    window.Calendar.calendarPickerHolderId = '#CalendarPickerHolder';

    window.Calendar.hourInputElm = document.querySelector(window.Calendar.hourInputId);
    window.Calendar.minuteInputElm = document.querySelector(window.Calendar.minuteInputId);
    window.Calendar.meridianInputElm = document.querySelector(window.Calendar.meridianInputId);
    window.Calendar.dateInputElm = document.querySelector(window.Calendar.dateInputId);
    window.Calendar.monthInputElm = document.querySelector(window.Calendar.monthInputId);
    window.Calendar.yearInputElm = document.querySelector(window.Calendar.yearInputId);

    //date picker holder element
    window.Calendar.calendarDatepickerPopupEl = document.querySelector(window.Calendar.calendarPickerHolderId);

}

//sets the input dates and initializes the calendar
export function LoadCalendar(hour12, minute, meridian, date, month, year) {
    // CSS Selector
    window.Calendar.calendar = new VanillaCalendar(window.Calendar.calendarPickerHolderId, {
        // Options
        date: {
            //set the date to show when calendar opens
            today: new Date(`${year}-${month}-${date}`),
        },
        settings: {
            range: {
                min: '0001-01-01',
                max: '9999-01-01'
            },
            selection: {
                time: 12, //AM/PM format
            },
            selected: {
                //set the time to show when calendar opens
                time: `${hour12}:${minute} ${meridian}`,
            },
        },
        actions: {
            changeTime(e, time, hours, minutes, keeping) {
                window.Calendar.hourInputElm.value = hours;
                window.Calendar.minuteInputElm.value = minutes;
                window.Calendar.meridianInputElm.value = keeping;
            },
            clickDay(e, dates) {
                //if date selected, hide date picker
                if (dates[0]) {
                    window.Calendar.calendarDatepickerPopupEl.classList.add('visually-hidden');
                }

                //check needed because random clicks get through
                if (dates[0] !== undefined) {
                    //format the selected date for blazor
                    const choppedTimeData = dates[0].split("-");
                    var year = choppedTimeData[0];
                    var month = choppedTimeData[1];
                    var day = choppedTimeData[2];

                    //inject the values into the text input
                    window.Calendar.dateInputElm.value = day;
                    window.Calendar.monthInputElm.value = month;
                    window.Calendar.yearInputElm.value = year;
                }

            },
            //update year & month immediately even though not yet click date
            //allows user to change only month or year
            clickMonth(e, month) {
                month = month + 1; //correction for jb lib bug
                var with0 = ('0' + month).slice(-2);//change 9 to 09
                window.Calendar.monthInputElm.value = with0;
            },
            clickYear(e, year) { window.Calendar.yearInputElm.value = year; }
        },
    });

    //when module is loaded, calendar is initialized but not visible
    //click event in blazor will make picker visible
    window.Calendar.calendar.init();

    //handle clicks outside of picker
    document.addEventListener('click', autoHidePicker, { capture: true });




    //----------------------------------------------------LOCAL FUNCS

    //if click is outside picker & input then hide it
    function autoHidePicker(e) {

        //check if click was outside input
        const pickerHolder = e.target.closest(window.Calendar.calendarPickerHolderId);
        const timeInput = e.target.closest("#TimeInputHolder"); //reference in Blazor

        //if click is not on either inputs then hide picker
        if (!(timeInput || pickerHolder)) {
            window.Calendar.calendarDatepickerPopupEl.classList.add('visually-hidden');
        }
    }

}

export function SaveAsFile(filename, data) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = data;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}


//TODO MARKED FOR DELETION SINCE CAN TOGGLE VIA CLASS IN BLAZOR
//export function togglePopup(e) {
//    const input = e.target.closest(dateInputId);
//    const calendar = e.target.closest(calendarPickerHolderId);

//    var timeInput = (input && !input.classList.contains('input_focus'));
//    if (timeInput || calendar) {
//        calendarDatepickerPopupEl.classList.remove('visually-hidden');
//    } else {
//        calendarDatepickerPopupEl.classList.add('visually-hidden');
//    }
//};




//-----------------------FOR JSFetchWrapper
//calls to server from blazor come here not via blazor http client, reliable
export async function SkyChartInit(imageHolder, SkyChartUrl) {

    console.log("SKY CHART AUTO PREVIEW");

    //remove previous on multiple calculates
    $(imageHolder).empty();

    //make page loading icon visible
    var loadingIcon = $('#SkyChartLoadingIcon');

    loadingIcon.show();


    await MultiTry(LoadSvg); //load normal image first

    //let it go without wait
    MultiTry(LoadGif); //load svg




    //--------------------------------------

    async function MultiTry(failableFunction) {
        //number of tries to retry getting GIF
        //this is cached call, so expect failure as norm here
        let count = 10;
        while (count > 0) {

            //run code to get image that could fail on 1st call
            try { await failableFunction(); count = 0; }

            //on fail show messsage and try again
            catch (error) { console.log("JS: SKY CHART ERROR : TRY AGAIN"); }

            count -= 1;
        }

    }

    function TurnOnAutoGIFPreview() {

        //show and hide animated version automatically
        $(imageHolder).on("mouseout", function () {

            $('#SkyChartBlobGIF').show();
            $('#SkyChartBlob').hide();
        });

        $(imageHolder).on("mouseover", function () {
            $('#SkyChartBlobGIF').hide();
            $('#SkyChartBlob').show();
        });

    }

    async function LoadGif() {
        var res = await fetch(SkyChartUrl + "GIF");
        var myBlob = await res.blob();

        //window.SkyChartBlobGIF = myBlob;
        console.log("SKY CHART GIF LOADED");

        var imagesrc = URL.createObjectURL(myBlob);
        window.SkyChartBlobGIF = $('<img  />', { id: 'SkyChartBlobGIF', src: imagesrc });
        window.SkyChartBlobGIF.appendTo($(imageHolder));
        //img.css("position", "absolute");

        //show animation first since most likely user's mouse hasn't graced chart
        window.SkyChartBlobGIF.show();

        //show as soon as available
        window.SkyChartBlob?.hide();
        window.window.SkyChartBlobGIF?.show();

        //once loaded, turn on auto preview
        TurnOnAutoGIFPreview();
    }

    async function LoadSvg() {
        //now load normal image to show when cursor is placed over
        var res = await fetch(SkyChartUrl);
        var myBlob = await res.blob();

        window.SkyChartBlob = myBlob;
        console.log("SKY CHART SVG LOADED");

        var imagesrc = URL.createObjectURL(myBlob);
        window.SkyChartBlob = $('<img />', { id: 'SkyChartBlob', src: imagesrc });
        window.SkyChartBlob.appendTo($(imageHolder));

        //show as soon as available
        window?.SkyChartBlob?.show();
        window.window?.SkyChartBlobGIF?.hide();

        loadingIcon.hide();

    }

}

export async function SkyChartAnimate(imageHolder, SkyChartUrl) {

    var img = $('<img />', { src: SkyChartUrl });
    img.appendTo($(imageHolder));

    console.log("Normal loaded");
    $(imageHolder).on("mouseleave", function () {
        $("#log").append("<div>Handler for `mouseleave` called.</div>");
    });

    $(imageHolder);
    var img = $('<img />', { src: SkyChartUrl + 'GIF' });
    img.appendTo($(imageHolder));

}

export async function postWrapper(url, payloadXml) {
    console.log("JS > Sending POST request...");

    var response = await fetch(url, {
        "headers": { "accept": "*/*", "Connection": "keep-alive" },
        "body": payloadXml,
        "method": "POST"
    });

    var responseText = await response?.text();

    return responseText ?? "";
}


//give a relative URL will play
export async function PlaySoundFromUrl(fileUrl) {

    console.log("JS > Notification Play");

    var $audio = $("#NotificationPlayer");
    $audio.attr("src", fileUrl);
    /****************/
    $audio[0].pause();
    $audio[0].load();//suspends and restores all audio element
    $audio[0].oncanplaythrough = $audio[0].play();
    /****************/
}

//only give response if header says ok
//todo special to hadnle empty person list
export async function ReadOnlyIfPassJson(url) {
    console.log("JS > Read Only If Pass Json...");

    var response = await fetch(url, {
        "headers": { "accept": "*/*", "Connection": "keep-alive" },
        "method": "GET"
    });

    var callStatus = response.headers.get('Call-Status');

    if (callStatus === "Pass") {

        var responseText = await response?.text();

        var payload = { Status: "Pass", Payload: responseText };

        return payload;

    } else if (callStatus == "Fail") {

        var payload = { Status: "Fail", Payload: null };

        return payload;
    }

    //call should not come here
    else {
        console.log("ERROR: No Call Status Found!");
        var payload = { Status: "Fail", Payload: null };
        return payload;
    }

}

//will auto set GET or POST on if data to send is provided
//only gets data once done
export async function ReadOnlyIfPassString(url, dataToSend) {
    console.log("JS > Read Only If Pass String...");

    //make get or post based on if got data to send or not
    var httpCallProtocol = dataToSend == null ? "GET" : "POST";

    var callStatus = "";
    while (callStatus !== "Pass") { //must be pass


        //make call
        var response = await fetch(url, {
            "headers": { "accept": "*/*", "Connection": "keep-alive" },
            "method": httpCallProtocol,
            "body": dataToSend
        });

        var callStatus = response.headers.get('Call-Status');

        //easy debug
        console.log(`API SAID : ${callStatus}`);

        //if pass end here send data to caller
        if (callStatus === "Pass") {
            var responseText = await response?.text();

            return responseText;
        }
        if (callStatus === "Fail") { return null; } //don't bang API if said fail, tell caller with null
    }

}

//gets current page url
export function getUrl() {
    console.log(`JS: getUrl`);
    return window.location.href;
};

//shows an email
export async function ShowLeaveEmailAlert() {
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
export async function ShowSendToEmail(message) {
    //show alert to get email
    const { value: email } = await Swal.fire({
        title: message,
        input: 'email',
        inputPlaceholder: 'Enter your email address'
    });

    //send email inputed to caller
    return email;
}

//loads js file programatically,
//equivalent to js include in header
export function loadJs(sourceUrl) {
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

//get previous website for logging
//so far only shows previous if on
//first visit from another page
export function getOriginUrl() {
    //try get value from here
    var originUrl = document.referrer;

    //if no url found, try a different place
    if (originUrl === false) { originUrl = window.location.origin; }

    return originUrl;
}

//Initializes the global error cather, from here error handler in blazor is called.
//This is only a backup way to know unexpected exception occurred,
//not really caught & handled
//Notes:
//- todo all methods must be in blazor to js interop table list
//- called from MainLayout after render
//- needs to be called after index.html has fully loaded
//- mainly used for logging global errors to API
//- from much testing even with ErrorBoundary, only the default blazor error message consistently gets called on error
//  as such error is known by watching this element's style display property
export function InitErrorCatcher() {
    //setup call to handler in blazor
    //call only if style display update
    var observer = new MutationObserver(function (mutationsList, observer) {
        for (var mutation of mutationsList) {
            //changes to style is assumed to be only changes
            //to display prop when error occurs
            if (mutation.attributeName == "style") {
                //call blazor handler
                DotNet.invokeMethodAsync('Website', 'OnAppError')
                    .then(data => {
                        console.log("JS: ERROR : Called Blazor Error handle");
                    });
            }
        }
    });

    //get the default error element
    var blazorDefaultErrorElem = $("#blazor-error-ui")[0];

    //attach handler to the element
    observer.observe(blazorDefaultErrorElem, { attributes: true });
}

export async function watchProperty(instance, handlerName) {
    window.addEventListener('storage', (e) => {
        instance
        MethodAsync(handlerName);
    });
}

export function getElementWidth(element, parm) {
    console.log(`JS : getElementWidth : ${element.offsetWidth}`);
    return element.offsetWidth;
};

export function addWidthToEveryChild(element, widthToAdd) {
    console.log(`JS: addWidthToEveryChild`);

    //add to each child of input element
    $(element).children().each(function () {
        //get current width
        let currentWidth = $(this).width();

        //set new width with input value
        $(this).width(currentWidth + widthToAdd);
    });
}

export function getPropWrapper(element, propName) {
    let propVal = $(element).prop(propName);
    console.log(`JS: getPropWrapper : ${propName} : ${propVal}`);
    return propVal;
}

export function setPropWrapper(element, propName, propVal) {
    $(element).prop(propName, propVal);
    console.log(`JS: setPropWrapper : ${propName} : ${propVal}`);
    return propVal;
}

export function setAttrWrapper(element, propName, propVal) {
    $(element).attr(propName, propVal);
    console.log(`JS: setAttrWrapper : ${propName} : ${propVal}`);
    return propVal;
}

export function setCssWrapper(element, propName, propVal) {
    $(element).css(propName, propVal);
    console.log(`JS: setCssWrapper : ${propName} : ${propVal}`);
    return propVal;
}

//Jquery to show inputed element
//by class and ID (CSS selector)
export function showWrapper(element) {
    console.log(`JS: showWrapper`);
    $(element).show();
};

//Jquery to hide inputed element
//by class and ID (CSS selector)
export function hideWrapper(element) {
    console.log(`JS: hideWrapper`);
    $(element).hide();
};

//Jquery to attach event listener to inputed element
export function addEventListenerWrapper(element, eventName, functionName) {
    console.log(`JS: addEventListenerWrapper : ${eventName} : ${functionName}`);
    element.addEventListener(eventName, window[functionName]);
};

//Jquery to attach event listener by class and ID (CSS selector)
export function addEventListenerByClass(selector, eventName, functionName) {
    console.log(`JS: addEventListenerByClass : ${eventName} : ${functionName}`);

    //attach listener to each element
    $(selector).each(function () {
        this.addEventListener(eventName, window[functionName]);
    });
};

export function InjectIntoElement(element, valueToInject) {
    //convert string to html node
    var template = document.createElement("template");
    template.innerHTML = valueToInject;
    var nodeToInject = template.content.firstElementChild;

    //place new node in parent
    element.innerHTML = ''; //clear current children if any
    element.appendChild(nodeToInject);
}

//uses UAParser library to extract user data
export function getVisitorData() {
    console.log(`JS: getVisitorData`);
    var parser = new UAParser();
    return parser.getResult();
}

//copies inputed text to clipboard
//used for copying direct link to chart
export function CopyToClipboard(text) {
    navigator.clipboard.writeText(text)
        .then(function () { console.log("Copied to clipboard!"); })
        .catch(function (error) { console.log(error); }); //todo raise proper error, logged
}

//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
export function generateLifeEventListTable(tableId, tableData) {
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
export function getLifeEventsListTableData() {
    return window.lifeEventsListTable.getData();
}

//adds new row to life events table
//note: defaults are set here, maybe move to blazor side for conformity
export function addNewLifeEventToTable(defaultNewLifeEventStr) {
    //call blazor handler
    //var dateTimeStr = DotNet.invokeMethod('Website', 'GetNowTimeString');
    const defaultNewLifeEvent = JSON.parse(defaultNewLifeEventStr);
    var addToTopOfTable = true;
    window.lifeEventsListTable.addData([defaultNewLifeEvent], addToTopOfTable);
}

export function DrawPlanetStrengthChart(sun, moon, mercury, mars, jupiter, saturn, venus, rahu, ketu) {
    //delete previous chart if any
    if (window.PlanetStrengthChart != null) { window.PlanetStrengthChart.destroy(); }

    var xValues = ["Sun", "Moon", "Mercury", "Mars", "Jupiter", "Saturn", "Venus", "Rahu", "Ketu"];
    var yValues = [sun, moon, mercury, mars, jupiter, saturn, venus, rahu, ketu];

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
                            'rgba(201, 203, 207, 0.7)',
                            'rgba(201, 162, 207, 0.7)',
                            'rgba(162, 203, 207, 0.7)'
                        ],
                        borderColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 159, 64)',
                            'rgb(255, 205, 86)',
                            'rgb(75, 192, 192)',
                            'rgb(54, 162, 235)',
                            'rgb(153, 102, 255)',
                            'rgb(201, 203, 207)',
                            'rgb(201, 162, 207)',
                            'rgb(162, 203, 207)'
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

export function DrawHouseStrengthChart(_house1,
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

//todo check for functionality

//scrolls to div on page and flashes div using JS
export function scrollToDiv(elmInput) {
    var $elm = $(elmInput);

    //scroll to element
    $elm[0].scrollIntoView();
    //document.getElementById(elemId).scrollIntoView();

    //use JS to attarct attention to div
    //var idString = `#${elmId}`;
    $elm.fadeOut(200).fadeIn(200).fadeOut(200).fadeIn(200);

    $elm.fadeTo(100, 0.4, function () { $(this).fadeTo(500, 1.0); });
}

export async function htmlToEmail(elmInput, fileName, fileFormat, receiverEmail) {

    //converts to pdf
    var pdfBlob = await htmlToPdfBlob(elmInput);

    //send to server for forwarding to email
    await pdfToEmail(fileName, fileFormat, receiverEmail, pdfBlob);

    Swal.fire(`Email Sent`, 'Wait a few minutes, if not found check junk folder', 'success');
}
export async function pdfToEmail(fileName, fileFormat, receiverEmail, inputedBlobFile) {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/pdf");

    var file = inputedBlobFile;

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: file,
        redirect: 'follow'
    };

    var apiDomain = window.URLS.APIDomain;
    fetch(`${apiDomain}/Send/${fileName}/${fileFormat}/${receiverEmail}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}
export async function htmlToPdfBlob(elmInput, pdfFileName) {

    //set a nice name for the file
    window.PDFOptions.filename = pdfFileName;

    //create the PDF
    var tempMatchPDFBlob = await html2pdf().set(window.PDFOptions).from(elmInput).toPdf().output('blob');

    return tempMatchPDFBlob;
}

export async function openPDFNewTab(elmInput, pdfFileName) {

    var tempMatchPDFBlob = await htmlToPdfBlob(elmInput, pdfFileName);

    //open a link in new tab to it, user than has choice to save or leave as is
    const url = URL.createObjectURL(tempMatchPDFBlob);
    window.open(url, '_blank');

    return tempMatchPDFBlob;
}

//html to PDF, starts save as well
//note options set in APP.JS
export async function htmlToPdfAutoDownload(elmInput, pdfFileName) {

    //set a nice name for the file
    window.PDFOptions.filename = pdfFileName;

    //generate & download to browser
    await html2pdf().set(window.PDFOptions).from(elmInput).save();
}

//▀█▀ ▄▀█ █▄▄ █░░ █▀▀   █▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ ▀█▀ █▀█ █▀█
//░█░ █▀█ █▄█ █▄▄ ██▄   █▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ ░█░ █▄█ █▀▄

//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
export function generateWebsiteTaskListTable(tableId, tableData) {
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

export function generatePlanetDataTable(tableId, tableData) {
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
            { title: "Current House", field: "planetCurrentHouse", hozAlign: "center", responsive: 0 },
            { title: "Nirayana/Movable Longitude", field: "nirayanaLongitude", responsive: 0 },
            { title: "Sayana/Fixed Longitude", field: "sayanaLongitude", responsive: 0 },
            { title: "Sign", field: "planetCurrentSign", responsive: 0 },
            { title: "Navamsa Sign", field: "planetCurrentNavamsaSign", hozAlign: "center", responsive: 0 },
            { title: "Constellation", field: "planetCurrentConstellation", hozAlign: "center", responsive: 0 },
            { title: "Malefic Conjunct", field: "isPlanetConjunctWithMaleficPlanets", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Malefic Aspect", field: "isPlanetAspectedByMaleficPlanets", formatter: "tickCross", hozAlign: "center", responsive: 0 },
            { title: "Conjunct Planets", field: "conjunctPlanets", hozAlign: "center", responsive: 0 },
            { title: "Aspecting Planets", field: "aspectingPlanets", hozAlign: "center", responsive: 0 },
            { title: "Total Strength", field: "shadbalaPinda", hozAlign: "center", responsive: 0 },
            { title: "Position Strength", field: "positionStrength", hozAlign: "center", responsive: 0 },
            { title: "Directional Strength", field: "directionalStrength", hozAlign: "center", responsive: 0 },
            { title: "Motional Strength", field: "motionalStrength", hozAlign: "center", responsive: 0 },
            { title: "Temporal Strength", field: "temporalStrength", hozAlign: "center", responsive: 0 },
            { title: "Aspect Strength", field: "aspectStrength", hozAlign: "center", responsive: 0 },
            { title: "Permanent Strength", field: "permanentStrength", hozAlign: "center", responsive: 0 }
        ],
    });
}

//table with more prediction
export function generatePlanetDataInfoTable(tableId, tableData) {
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
            { title: "Motion Name", field: "motionName", hozAlign: "center", responsive: 0 },
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

export function generateHouseDataTable(tableId, tableData) {
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

export function generateHouseDataInfoTable(tableId, tableData) {
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

export function shareDialogFacebook(shareUrl) {
    FB.ui({
        method: 'share',
        href: shareUrl,
    }, function (response) {
        console.log(response);
    });
}

//gets data about screen from browser for logging
export function getScreenData() {
    var screenData = {
        //"Orientation": window.screen.orientation.type,
        "Width": window.screen.width,
        "Height": window.screen.height,
        //    "ColorDepth": window.screen.colorDepth
    }
    return screenData;
}

//inject an option into select button directly
export function addOptionToSelectDropdown(selectElementRef, visibleText, selectValue) {

    //create artificially new option for select box
    var newOption = $("<option></option>").attr("value", selectValue).text(visibleText);

    //parse and inject into element
    $(selectElementRef).append(newOption);

}