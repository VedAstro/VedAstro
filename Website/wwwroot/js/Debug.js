//NOTE FUNCTIONS IN HERE ARE EXPERIMENTAION/DEBUGGING

//██████╗░███████╗██████╗░██╗░░░██╗░██████╗░
//██╔══██╗██╔════╝██╔══██╗██║░░░██║██╔════╝░
//██║░░██║█████╗░░██████╦╝██║░░░██║██║░░██╗░
//██║░░██║██╔══╝░░██╔══██╗██║░░░██║██║░░╚██╗
//██████╔╝███████╗██████╦╝╚██████╔╝╚██████╔╝
//╚═════╝░╚══════╝╚═════╝░░╚═════╝░░╚═════╝░

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