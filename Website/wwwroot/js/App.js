//POLYFILL
//this for getting max scroll vals
(function (elmProto) {
    if ('scrollTopMax' in elmProto) {
        return;
    }
    Object.defineProperties(elmProto, {
        'scrollTopMax': {
            get: function scrollTopMax() {
                return this.scrollHeight - this.clientHeight;
            }
        },
        'scrollLeftMax': {
            get: function scrollLeftMax() {
                return this.scrollWidth - this.clientWidth;
            }
        }
    });
}
)(Element.prototype);


//EVENT HANDLERS
//moves the line cursor in DasaViewBox
//and updates the time cursor legend box
function timeCursorEventHandler(mouse) {

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
    if (mouseOut) { $("#TimeVerticalLine").hide(); return; }
    else { $("#TimeVerticalLine").show(); }

    //move vertical line to under mouse inside dasa view box
    $("#TimeVerticalLine").css('left', relativeMouseX);

    //get date of birth
    //TODO read val from global var
    let birthTime = "12:44 23/04/1994 +08:00";

    //convert left position from px to time
    let cursorTime = PixelToTimeFromBirth(relativeMouseX, birthTime);
    $("#TimeCursorLegend").html(cursorTime);

}

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

//converts vertical scroll to horizontal scroll inside dasa view
function dasaViewScrollEventHandler(evt) {

    //stop the scroll from normally moving down
    evt.preventDefault();
    //move horizontal scroll
    evt.currentTarget.scrollLeft += evt.deltaY;

}



//CUSTOM LOGIC FUNCTIONS
//WHICH ARE CONSUMED BY OTHERS IN THIS FILE

function PixelToTimeFromBirth(pixelValue, stdTime) {
    return DotNet.invokeMethod('Website', 'PixelToTimeFromBirth', pixelValue, stdTime);
};

function executeFunctionByName(functionName, context /*, args */) {
    var args = Array.prototype.slice.call(arguments, 2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();
    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    }
    return context[func].apply(context, args);
}

//returns first elements in input list that is
//underneath (visually) the inputed top element
function getFirstElementUnder(topElement, possibleList) {

    //check if each possible element is under "top element"
    for (var i = 0; i < possibleList.length; i++) {

        //if under return it
        var isUnder = isElementUnderElement(topElement, possibleList[i]);
        if (isUnder) { return possibleList[i]; }
    }
}

// gets the year text under the auto scroll line
function getTextUnderAutoScrollLine() {

    //get the elements needed
    let topElement = $("#AutoScrollLine");
    let yearElementList = $("#YearRow").children();

    //find the element in list that is under top element
    let divUnder = getFirstElementUnder(topElement, yearElementList);

    //get the text and return it
    let yearText = $(divUnder).text();

    return yearText;
}

//Checks if top element is directly over under element
function isElementUnderElement(topElement, underElement) {
    //get measurements of element to check
    var topElementBox = topElement[0].getBoundingClientRect();//needs 0 because jq object
    var underElementBox = underElement.getBoundingClientRect();

    //do the checking
    var isUnder = !(topElementBox.right < underElementBox.left ||
        topElementBox.left > underElementBox.right ||
        topElementBox.bottom < underElementBox.top ||
        topElementBox.top > underElementBox.bottom);

    return isUnder;
}




//IN PRODUCTION USE CODE BELOW, CAREFUL!
//CALLED BY BLAZOR WRAPPER CLASS

window.jQueryWrapper = function (input) {
    return $(input);
};

window.addClassWrapper = function (element, classString) {
    console.log(`JS: addClassWrapper : ${classString}`);
    $(element).addClass(classString);
};

window.showWrapper = function (element) {
    console.log(`JS: showWrapper`);
    $(element).show();
};

window.hideWrapper = function (element) {
    console.log(`JS: hideWrapper`);
    $(element).hide();
};

window.getPropWrapper = function (element, propName) {
    let propVal = $(element).prop(propName);
    console.log(`JS: getPropWrapper : ${propName} : ${propVal}`);
    return propVal;
};

window.setPropWrapper = function (element, propName, propVal) {
    $(element).prop(propName, propVal);
    console.log(`JS: setPropWrapper : ${propName} : ${propVal}`);
    return propVal;
};

window.addEventListenerWrapper = function (element, eventName, functionName) {
    console.log(`JS: addEventListenerWrapper : ${eventName} : ${functionName}`);
    element.addEventListener(eventName, window[functionName]);
};

window.getElementWidth = function (element, parm) {
    console.log(`JS : getElementWidth : ${element.offsetWidth}`);
    return element.offsetWidth;
};

//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
function generateTable(tableId, tableData) {


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



//EXPERIMENTAL CODE BELOW, REMOVE IT IF NEEDED

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

window.getWindowInnerWidth = function () {
    console.log(`JS : getWindowInnerWidth : ${window.innerWidth}`);
    return window.innerWidth;
};

window.scrollElementLeft = function (element, parm) {
    element.scrollLeft += 500;
    console.log(`JS : scrollElementLeft : ${element.scrollLeft}`);
};

window.getElementScrollLeft = function (element, parm) {
    console.log(`JS: getElementScrollLeft : ${element.scrollLeft}`);
    return element.scrollLeft;
};

window.getElementScrollWidth = function (element, parm) {
    return element.scrollWidth;
};

window.setElementScrollLeft = function (element, parm) {
    element.scrollLeft = parm;
    console.log(`JS: setElementScrollLeft : ${element.scrollLeft}`);

};

window.setElementStyleLeft = function (element, leftValue) {
    element.setAttribute('style', `left: ${leftValue}px;`);
    console.log(`JS: setElementStyleLeft : ${leftValue}`);
};

window.initDasaViewBoxVerticalLine = function () {
    window.dasaViewHolder = document.querySelector('#DasaViewHolder');
    window.dasaViewVerticaLine = document.querySelector('.vt');
    console.log(`JS: initDasaViewBoxVerticalLine : window.dasaViewHolder : ${window.dasaViewHolder}`);
    console.log(`JS: initDasaViewBoxVerticalLine : window.dasaViewVerticaLine : ${window.dasaViewVerticaLine}`);

};

window.saveYearDivUnderTimeCursor = function (referenceParent) {
    console.log(`JS: saveYearDivUnderTimeCursor`);

    window.yearDivUnderTimeCursor = getFirstElementUnder($("#TimePlaceLine"), $(referenceParent).children());

};

window.getTextUnderElement = function (referenceParent) {
    console.log(`JS: getTextUnderElement`);
    window.yearDivUnderTimeCursor = getFirstElementUnder($("#TimePlaceLine"), $(referenceParent).children());
};

window.autoScrollDasaViewBackZoomIn = function (scrollContainer, referenceParent) {
    console.log(`JS: autoScrollDasaViewBackZoomIn`);

    let elementFound = false;

    //move scroll until element is found
    while (!elementFound) {

        //increase left scroll because zoom in
        let newScrollLeft = $(scrollContainer).prop("scrollLeft") + 5;
        let maxScrollLeft = $(scrollContainer)[0].scrollLeftMax;

        if (newScrollLeft >= maxScrollLeft) {
            console.log(`JS: new scroll place not found!!`);
            //todo handle this properly
            elementFound = true;
        }

        //add buffer, so that cursor doesn't stand edge of dif, causes errors on next auto scroll
        newScrollLeft = newScrollLeft + 30;
        $(scrollContainer).prop("scrollLeft", newScrollLeft);

        let currentElementUnder = getFirstElementUnder($("#TimePlaceLine"), $(referenceParent).children());

        let old = $(window.yearDivUnderTimeCursor).text();
        let current = $(currentElementUnder).text();
        if (old == current) {
            elementFound = true;
        }
    }
};

window.autoScrollDasaViewBackZoomOut = function (scrollContainer) {
    console.log(`JS: autoScrollDasaViewBackZoomOut`);

    let autoCorrected = false;

    //move scroll until element is found
    while (!autoCorrected) {
        //increase left scroll because zoom in
        let newScrollLeft = $(scrollContainer).prop("scrollLeft") - 5;

        if (newScrollLeft <= 0) {
            console.log(`JS: new scroll place not found!!`);
            //todo handle this properly
            autoCorrected = true;
        }

        //add buffer, so that cursor doesn't stand edge of dif, causes errors on next auto scroll
        newScrollLeft = newScrollLeft - 30;
        //set scroll left
        $(scrollContainer).prop("scrollLeft", newScrollLeft);

        let currentElementUnder = getFirstElementUnder($("#TimePlaceLine"), $(".intersect"));

        let old = $(window.yearDivUnderTimeCursor).text();
        let current = $(currentElementUnder).text();
        if (old == current) {
            autoCorrected = true;
        }
    }
};

//this function scrolls a div box until two elements are on top each other
//make sure top element & bottom element move free of each other when scrolling
window.autoScrollToYear = function (scrollContainer, targetYearText) {
    console.log(`JS: autoScrollToYear`);

    let autoCorrected = false;

    //push scroll to start
    $(scrollContainer).prop("scrollLeft", 0); //0 is distance from left

    //set rate at cursor is moved to find element
    let increaseRate = 5;

    //move scroll from start to end looking for element
    while (!autoCorrected) {
        //calculate new left scroll because 
        let newScrollLeft = $(scrollContainer).prop("scrollLeft") + increaseRate;

        //if scroll values are invalid end here
        let maxScrollLeft = $(scrollContainer)[0].scrollLeftMax;
        let newScrollOverMax = newScrollLeft >= maxScrollLeft;
        let newScrollBelowMin = newScrollLeft <= 0;
        if (newScrollOverMax || newScrollBelowMin) { return; }

        //add buffer, so that cursor doesn't stand edge of dif, 
        //else causes errors on next auto scroll
        newScrollLeft = newScrollLeft + 30;

        //set scroll left
        $(scrollContainer).prop("scrollLeft", newScrollLeft);

        //get new text under element now
        let newTextUnder = getTextUnderAutoScrollLine();

        //compare year text inside previous & current divs
        //if match stop moving cursor (loop), found
        if (newTextUnder == targetYearText) { autoCorrected = true; }
    }
};



//ARCHIVED CODE

//function getAllElementsUnder() {
//    var inBox = [];
//    var divBox = document.getElementById("TimePlaceLine").getBoundingClientRect();
//    var images = document.getElementsByClassName("intersect");
//    for (var i = 0; i < images.length; i++) {
//        var imageBox = images[i].getBoundingClientRect();
//        var overlap = !(divBox.right < imageBox.left ||
//            divBox.left > imageBox.right ||
//            divBox.bottom < imageBox.top ||
//            divBox.top > imageBox.bottom);
//        if (overlap) {
//            inBox.push(images[i]);
//        }
//    }
//    return inBox;
//}
