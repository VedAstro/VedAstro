//CUSTOM LOGIC FUNCTIONS
//WHICH ARE CONSUMED BY OTHERS IN THIS FILE

function timeCursorEventHandler(mouse) {

    //gets the measurements of the dasa view holder
    //the element where cursor line will be moving
    let holderMeasurements = $("#DasaViewHolder")[0].getBoundingClientRect();

    //calculate mouse X relative to dasa view box
    let relativeMouseX = mouse.clientX - holderMeasurements.left;

    //if mouse is beyond element, then reset to 0
    relativeMouseX = relativeMouseX < 0 ? 0 : relativeMouseX;

    //move vertical line to under mouse inside dasa view box
    $("#TimeVerticalLine").css('left', relativeMouseX);

    //get date of birth
    let birthTime = "12:44 23/04/1994 +08:00";

    //convert left position from px to time
    let cursorTime = PixelToTimeFromBirth(relativeMouseX, birthTime);
    $("#TimeCursorLegend").html(cursorTime);

}


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



//IN PRODUCTION USE CODE BELOW, CAREFUL!

window.jQueryWrapper = function (input) {
    return $(input);
};

window.addClassWrapper = function (element, classString) {
    $(element).addClass(classString);

    console.log(`JS: addClassWrapper : ${classString}`);
};
window.showWrapper = function (element) {
    $(element).show();
    console.log(`JS: showWrapper`);
};
window.hideWrapper = function (element) {
    $(element).hide();
    console.log(`JS: hideWrapper`);
};



//UNSURE CODE BELOW, REMOVE IT IF SEE FIT

window.getElementWidth = function (element, parm) {
    console.log(`JS : getElementWidth : ${element.offsetWidth}`);
    return element.offsetWidth;
};

window.getWindowInnerWidth = function () {
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

window.addEventListenerWrapper = function (element, eventName, functionName) {

    element.addEventListener(eventName, window[functionName]);
}


