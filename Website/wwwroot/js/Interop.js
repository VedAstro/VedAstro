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
export var setTextWrapper = (element, value) => $(element).text(value);
export var getTextWrapper = (element) => $(element).text();
export var getValueWrapper = (element) => $(element).val();
export var setValueWrapper = (element, value) => $(element).val(value);
export var IsOnline = () => window.navigator.onLine;
export var getAllLocalStorageKeys = () => Object.keys(localStorage); //return array of local storage keys
export var showAccordion = (id) => $(id).collapse("show");
export var toggleAccordion = (id) => $(id).collapse("toggle"); //Uses Bootstrap Jquery plugin to toggle any collapsible component by id
export var scrollIntoView = (id) => $(id)[0].scrollIntoView(); //scrolls element by id into view
export var highlightByEventName = (keyword) => window.EventsChartLoaded.highlightByEventName(keyword);
export var AddEventsToGoogleCalendar = () => window.EventsChartLoaded.AddEventsToGoogleCalendar();
export var unhighlightByEventName = (keyword) => window.EventsChartLoaded.unhighlightByEventName(keyword);
const RETRY_COUNT = 5;


/* Sunday After Noon Jan 2024
A warm cup of Canadian soy, so fine,
With dairy milk from New Zealand's vine.

A sweet scent of Illinois' bloom,
Paired with a device, in the room.

Music so pure, it brings a tear,
A symphony for the soul to hear.

Pepper and turmeric from India's land,
With sweet bread, loaf in hand.

With the world at my fingertips, I ponder and muse,
What will I do for others? Which path will I choose?

 */


//below method needs to be called to initialize search with a list of text chunks
//that can be used later with 2nd call to do a search
// Initializes the FlexSearch index with a list of text chunks for future searches.
export async function InitializeSearchForAPICallList(textChunks) {

    //attach listener for input
    setupSearchInputListener();

    //save for resetting search later
    window.APICallListTextChunks = textChunks;

    const fuseOptions = {
        isCaseSensitive: false,
        includeScore: true,
        shouldSort: true,
        // includeMatches: false,
        findAllMatches: true, //show all possible API's
        minMatchCharLength: 2,
        // location: 0,
        // threshold: 0.6,
        // distance: 100,
        // useExtendedSearch: false,
        // ignoreLocation: false,
        // ignoreFieldNorm: false,
        // fieldNormWeight: 1,
        keys: [
            "name",
            "description"
        ]
    };
    window.APICallListSearchIndex = new Fuse(textChunks, fuseOptions);

    console.log('JS: APICallList Search Initialized.');

    // Sets up the search input listener and defines the search logic.
    function setupSearchInputListener() {
        let typingTimer; // Timer identifier for debounce mechanism.
        const typingDelay = 420; // Delay in ms after which search is triggered.

        var searchInput = $('#APICallList-SearchInputElement');

        // Start the debounce timer on keyup event, ignoring arrow keys.
        //done so that when user is mid-typing search does not slow down browser
        searchInput.keyup(function (event) {

            clearTimeout(typingTimer);
            if (![37, 38, 39, 40].includes(event.which)) { // Arrow keys
                $('#APICallList-LoadingIconHolder').show();//show loading icon

                typingTimer = setTimeout(performSearch, typingDelay);
            }
        });

        // Clear the debounce timer on keydown event.
        searchInput.keydown(function () {
            clearTimeout(typingTimer);
        });

        console.log('JS: Search input listener set up.');

        // Performs the search operation based on the user's input.
        function performSearch() {
            //remove if only white spaces
            const searchText = $('#APICallList-SearchInputElement').val().trim();

            //no search word, so show all
            if (searchText === '') { showHideChildren(window.APICallListTextChunks); }

            //do search for text
            else { searchApiMethod(searchText); }

            //remove loading once search task over
            $('#APICallList-LoadingIconHolder').hide();
        }

        // Searches for API methods using the provided search term.
        function searchApiMethod(searchTerm) {

            // Perform fuzzy matching and sort results by relevance.
            var fuseSearchResults = window.APICallListSearchIndex.search(searchTerm);

            // Map the sorted results to get the items.
            const searchResults = fuseSearchResults.map(result => result.item);

            // Display only the matched method information elements, sorted by score.
            showHideChildren(searchResults);
        }

        function showHideChildren(searchResults) {
            const parent = document.getElementById('APICallList-AllMethodInfoHolder');
            const children = Array.from(parent.children);
            const fragment = document.createDocumentFragment();
            const searchResultsMap = new Map(searchResults.map(result => [result.name, result]));

            // Update the displayed count of found methods.
            document.getElementById('FoundMethodInfoCountElm').textContent = searchResults.length.toString();

            // Clear the parent container before appending reordered children.
            parent.innerHTML = '';

            // Append matched children to the fragment in the order of searchResults.
            searchResults.forEach(result => {
                const matchedChild = children.find(child => child.classList.contains(result.name));
                if (matchedChild) {
                    matchedChild.style.display = '';
                    fragment.appendChild(matchedChild);
                }
            });
            // Hide unmatched children and append them to the fragment.
            children.forEach(child => {
                const className = child.classList.item(0); // Assuming the first class is the name.
                if (!searchResultsMap.has(className)) {
                    child.style.display = 'none';
                    fragment.appendChild(child);
                }
            });

            // Append the fragment to the parent to minimize reflows and maintain the new order.
            parent.appendChild(fragment);
        }
    }
}

export async function InitializeSearchForAPISelector(textChunks) {

    //attach listener for input
    setupSearchInputListener();

    //save for resetting search later
    window.APISelectorTextChunks = textChunks;

    const fuseOptions = {
        isCaseSensitive: false,
        includeScore: true,
        shouldSort: true,
        // includeMatches: false,
        findAllMatches: true, //show all possible API's
        minMatchCharLength: 2,
        // location: 0,
        // threshold: 0.6,
        // distance: 100,
        // useExtendedSearch: false,
        // ignoreLocation: false,
        // ignoreFieldNorm: false,
        // fieldNormWeight: 1,
        keys: [
            "name",
            "description"
        ]
    };
    window.APISelectorSearchIndex = new Fuse(textChunks, fuseOptions);

    console.log('JS: APISelector Search Initialized.');

    // Sets up the search input listener and defines the search logic.
    function setupSearchInputListener() {
        let typingTimer; // Timer identifier for debounce mechanism.
        const typingDelay = 420; // Delay in ms after which search is triggered.

        var searchInput = $('#APISelector-SearchInputElement');

        // Start the debounce timer on keyup event, ignoring arrow keys.
        //done so that when user is mid-typing search does not slow down browser
        searchInput.keyup(function (event) {

            clearTimeout(typingTimer);
            if (![37, 38, 39, 40].includes(event.which)) { // Arrow keys
                $('#APISelector-LoadingIconHolder').show();//show loading icon

                typingTimer = setTimeout(performSearch, typingDelay);
            }
        });

        // Clear the debounce timer on keydown event.
        searchInput.keydown(function () {
            clearTimeout(typingTimer);
        });

        console.log('JS: Search input listener set up.');

        // Performs the search operation based on the user's input.
        function performSearch() {
            //remove if only white spaces
            const searchText = $('#APISelector-SearchInputElement').val().trim();

            //no search word, so show all
            if (searchText === '') { showHideChildren(window.APISelectorTextChunks); }

            //do search for text
            else { searchApiMethod(searchText); }

            //remove loading once search task over
            $('#APISelector-LoadingIconHolder').hide();
        }

        // Searches for API methods using the provided search term.
        function searchApiMethod(searchTerm) {

            // Perform fuzzy matching and sort results by relevance.
            var fuseSearchResults = window.APISelectorSearchIndex.search(searchTerm);

            // Map the sorted results to get the items.
            const searchResults = fuseSearchResults.map(result => result.item);

            // Display only the matched method information elements, sorted by score.
            showHideChildren(searchResults);
        }

        function showHideChildren(searchResults) {
            const parent = document.getElementById('APISelector-AllMethodInfoHolder');
            const children = Array.from(parent.children);
            const fragment = document.createDocumentFragment();
            const searchResultsMap = new Map(searchResults.map(result => [result.name, result]));

            // Clear the parent container before appending reordered children.
            parent.innerHTML = '';

            // Append matched children to the fragment in the order of searchResults.
            searchResults.forEach(result => {
                const matchedChild = children.find(child => child.classList.contains(result.name));
                if (matchedChild) {
                    matchedChild.style.display = '';
                    fragment.appendChild(matchedChild);
                }
            });
            // Hide unmatched children and append them to the fragment.
            children.forEach(child => {
                const className = child.classList.item(0); // Assuming the first class is the name.
                if (!searchResultsMap.has(className)) {
                    child.style.display = 'none';
                    fragment.appendChild(child);
                }
            });

            // Append the fragment to the parent to minimize reflows and maintain the new order.
            parent.appendChild(fragment);
        }
    }
}



//SCROLL SPY NAV
//when run will attach events to all with .scrollspy
//this then will used to highlight the Index link
export async function InitializeInPageNav() {


    //# dynamic change highlight
    //attaches a handler when scroll
    $(window).bind('scroll', async function () {
        var currentTop = $(window).scrollTop();

        //all content element with class "scrollspy"
        var elems = $('.scrollspy');

        for (let index = 0; index < elems.length; index++) {
            var elemTop = $(elems[index]).offset().top;
            var elemBottom = elemTop + $(elems[index]).height();
            var offset = 200; // Adjust this value to your needs
            if (currentTop >= elemTop - offset && currentTop <= elemBottom) {
                var contentId = $(elems[index]).attr('id');
                var navLink = $(`#${contentId}-Link`);

                //remove previous selected active
                navLink.siblings().removeClass('active');

                //make current active
                navLink.addClass('active');

                //set the nav menu to nicely appear at mid center vertically
                var windowHeight = $(window).height();
                var divHeight = $("#inPageNavBar").height();
                var divOffset = (windowHeight - divHeight) / 2;
                $("#inPageNavBar").css("top", divOffset + "px");

            }
        }
    });
}

//gets random text from given list
export function getRandomText(possibleTextsArray) {
    var index = Math.floor(Math.random() * possibleTextsArray.length);
    return possibleTextsArray[index];
}

//simple pop up for coming soon, with encouraged donation :D
export function FunFeaturePopUp() {

    //get interesting donate prompt text
    var texts = ["Build it faster!", "Speed Up Development", "Support Development"];
    var donateText = getRandomText(texts);

    Swal.fire({
        html: "<a target=\"_blank\" style=\"text-decoration-line: none;\" href=\"https://vedastro.org/Donate/\" class=\"link-primary fw-bold\">Fund</a> this feature for faster development",
        iconHtml: "<span class=\"iconify\" data-icon=\"openmoji:love-letter\" data-inline=\"false\"></span>",
        title: "Coming soon...",
        showConfirmButton: true,
        confirmButtonText: donateText,
        showCancelButton: true,
        cancelButtonText: "I can wait"
    }).then((result) => {

        if (result.isConfirmed) {
            window.open('https://vedastro.org/Donate', '_blank').focus();
        };
    });
}

//--------------------------CALENDAR INPUT SELECTOR CODE
//DESCRIPTION
//This file stores all code fo js date picker (VanillaCalendar)
//To use:
//#1. first load html file via blazor / js
//#2. then call LoadCalendar
//make sure empty calendar div exists

export function InitCalendarPicker(tableId) {

    //global space to store calendar related refs
    window[tableId] = {};

    //date input element
    window[tableId].hourInputId = `#HourInput${tableId}`;
    window[tableId].minuteInputId = `#MinuteInput${tableId}`;
    window[tableId].meridianInputId = `#MeridianInput${tableId}`;
    window[tableId].dateInputId = `#DateInput${tableId}`;
    window[tableId].monthInputId = `#MonthInput${tableId}`;
    window[tableId].yearInputId = `#YearInput${tableId}`;

    window[tableId].calendarPickerHolderId = `#CalendarPickerHolder${tableId}`;

    window[tableId].hourInputElm = document.querySelector(window[tableId].hourInputId);
    window[tableId].minuteInputElm = document.querySelector(window[tableId].minuteInputId);
    window[tableId].meridianInputElm = document.querySelector(window[tableId].meridianInputId);
    window[tableId].dateInputElm = document.querySelector(window[tableId].dateInputId);
    window[tableId].monthInputElm = document.querySelector(window[tableId].monthInputId);
    window[tableId].yearInputElm = document.querySelector(window[tableId].yearInputId);

    //date picker holder element
    window[tableId].calendarDatepickerPopupEl = document.querySelector(window[tableId].calendarPickerHolderId);

}

//sets the input dates and initializes the calendar
export function LoadCalendar(tableId, hour12, minute, meridian, date, month, year) {
    // CSS Selector
    window[tableId].calendar = new VanillaCalendar(window[tableId].calendarPickerHolderId, {
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
        //this is where time is sent back to blazor, by setting straight to dom
        actions: {
            changeTime(e, time, hours, minutes, keeping) {
                window[tableId].hourInputElm.innerText = hours;
                window[tableId].minuteInputElm.innerText = minutes;
                window[tableId].meridianInputElm.innerText = keeping;
            },
            clickDay(e, dates) {
                //if date selected, hide date picker
                if (dates[0]) {
                    window[tableId].calendarDatepickerPopupEl.classList.add('visually-hidden');
                }

                //check needed because random clicks get through
                if (dates[0] !== undefined) {
                    //format the selected date for blazor
                    const choppedTimeData = dates[0].split("-");
                    var year = choppedTimeData[0];
                    var month = choppedTimeData[1];
                    var day = choppedTimeData[2];

                    //inject the values into the text input
                    window[tableId].dateInputElm.innerText = day;
                    window[tableId].monthInputElm.innerText = month;
                    window[tableId].yearInputElm.innerText = year;
                }

            },
            //update year & month immediately even though not yet click date
            //allows user to change only month or year
            clickMonth(e, month) {
                month = month + 1; //correction for JS lib bug
                var with0 = ('0' + month).slice(-2);//change 9 to 09
                window[tableId].monthInputElm.innerText = with0;
            },
            clickYear(e, year) { window[tableId].yearInputElm.innerText = year; }
        },
    });

    //when module is loaded, calendar is initialized but not visible
    //click event in blazor will make picker visible
    window[tableId].calendar.init();

    //handle clicks outside of picker
    document.addEventListener('click', autoHidePicker, { capture: true });




    //----------------------------------------------------LOCAL FUNCS

    //if click is outside picker & input then hide it
    function autoHidePicker(e) {

        //check if click was outside input
        const pickerHolder = e.target.closest(window[tableId].calendarPickerHolderId);
        const timeInput = e.target.closest(`#TimeInputHolder${tableId}`); //reference in Blazor

        //if click is not on either inputs then hide picker
        if (!(timeInput || pickerHolder)) {
            window[tableId].calendarDatepickerPopupEl.classList.add('visually-hidden');
        }
    }

}

//given a base64 will convert file and init download
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
//defaults to get, but can be changed to any
export async function ReadOnlyIfPassJson(url, callMethod = "GET") {
    console.log("JS > Read Only If Pass Json...");

    var response = await fetch(url, {
        "headers": { "accept": "*/*", "Connection": "keep-alive" },
        "method": callMethod
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

/**
 * This function makes an HTTP request to a specified URL. It uses GET or POST method based on the provided data.
 * The function keeps making requests until the 'Call-Status' header in the response is 'Pass'.
 * If the 'Call-Status' is 'Fail', the function stops making requests and returns null.
 * 
 * @param {string} url - The URL to make the request to.
 * @param {Object} dataToSend - The data to send with the request. If this is null, a GET request is made. Otherwise, a POST request is made.
 * @returns {Promise<string|null>} - The response text if the 'Call-Status' is 'Pass'. Null if the 'Call-Status' is 'Fail'.
 */
export async function ReadOnlyIfPassString(url, dataToSend) {
    // Determine the HTTP method based on whether dataToSend is null or not
    const httpMethod = dataToSend == null ? "GET" : "POST";

    // Define the headers for the HTTP request
    const requestHeaders = {
        accept: "*/*",
        Connection: "keep-alive"
    };

    // Start a loop that will continue making requests until the 'Call-Status' is 'Pass'
    while (true) {
        try {
            // Make the HTTP request
            const response = await fetch(url, {
                headers: requestHeaders,
                method: httpMethod,
                body: dataToSend
            });

            // Get the 'Call-Status' from the response headers
            const callStatus = response.headers.get('Call-Status');
            console.log(`API SAID : ${callStatus}`);

            // If 'Call-Status' is 'Pass', return the response text
            if (callStatus === "Pass") {
                return await response.text();
            }

            // If 'Call-Status' is 'Fail', stop making requests and return null
            if (callStatus === "Fail") {
                return null;
            }

        } catch (error) {

            //TODO log to server
            // If an error occurs during the fetch operation, log the error and return null
            console.error(`Error occurred while making HTTP request: ${error}`);
            return null;
        }
    }
}


//gets current page url
export function getUrl() {
    console.log(`JS: getUrl`);
    return window.location.href;
};


export async function PopupTextInput(message, inputType, inputPlaceholder) {
    //show alert to get email
    const { value: email } = await Swal.fire({
        title: message,
        input: inputType,
        inputPlaceholder: inputPlaceholder
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
    //console.log(`JS: getPropWrapper : ${propName} : ${propVal}`);
    return propVal;
}

export function setPropWrapper(element, propName, propVal) {
    $(element).prop(propName, propVal);
    //console.log(`JS: setPropWrapper : ${propName} : ${propVal}`);
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

//Jquery to show inputed element
//by class and ID (CSS selector) given as an array
export function showListWrapper(idArray) {

    console.log(`JS: showListWrapper`);

    $.each(idArray, function (index, id) {
        $('#' + id).show();
    });

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

//add new bookmark to browser
export function AddNewBookmark(inputTitle, inputUrl) {

    // Create a new Bookmark object
    const bookmark = chrome.bookmarks.create({
        title: inputTitle,
        url: inputUrl,
    });

    console.log(`Added bookmark for ${url}`);
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
//can input both Element ref or CSS selector
export function scrollToDiv(elmInput) {
    var $elm = $(elmInput);

    //scroll to element
    $elm[0].scrollIntoView();

    animateHighlightElement(elmInput);
}

//flashes div using JS
//can input both Element ref or CSS selector
export function animateHighlightElement(elmInput) {

    var $elm = $(elmInput);

    //use JS to attract attention to div
    $elm.fadeOut(200).fadeIn(200).fadeOut(200).fadeIn(200);

    $elm.fadeTo(100, 0.4, function () { $(this).fadeTo(500, 1.0); });

}

//similar to JQuery's .slideToggle("slow")
// note that this function uses CSS transitions for the
//sliding effect, which are smoother than jQuery’s animations but might not be supported in all browsers
export function smoothSlideToggle(elementSelector, speed = 1000) {

    // Select the element
    let el = document.querySelector(elementSelector);

    // Check if the element is currently not displayed
    //SHOW
    if (window.getComputedStyle(el).display === 'none') {
        // Set the initial display to block
        el.style.display = 'block';

        // Capture the height of the element
        let height = el.offsetHeight;

        // Set the initial height to 0 and overflow to hidden
        el.style.height = 0;
        el.style.overflow = 'hidden';

        // Set the transition property for smooth animation
        el.style.transition = 'height 1s ease-in-out';

        // After a short delay, set the height to the element's original height
        setTimeout(() => {
            return el.style.height = height + 'px';
        }, 0);

        //once animation complete, make "help text" not cut when exceed div width
        //this is done by removing overflow property, which needs to be "hidden" during animation 
        el.addEventListener('transitionend', function transitionEnd(event) {
            // Remove the event listener
            event.target.removeEventListener('transitionend', transitionEnd);

            // Set the overflow to visible
            el.style.removeProperty('overflow');
        });
    }

    //HIDE
    else {

        //before animation starts, set overflow back to "hidden", for beautiful UX animation
        el.style.overflow = 'hidden';

        // If the element is currently displayed, set the transition property
        el.style.transition = 'height 1s ease-in-out';

        // Animate the height to 0
        el.style.height = 0;

        // After the transition is complete, set the display to none and remove the added styles
        setTimeout(() => {
            el.style.display = 'none';
            el.style.removeProperty('height');
            el.style.removeProperty('overflow');
            el.style.removeProperty('transition');
        }, speed);
    }
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