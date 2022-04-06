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


window.initDasaViewBoxVerticalLine = function () {
    window.dasaViewHolder = document.querySelector('#DasaViewHolder');
    window.dasaViewVerticaLine = document.querySelector('.vt');
    console.log(`JS: initDasaViewBoxVerticalLine : window.dasaViewHolder : ${window.dasaViewHolder}`);
    console.log(`JS: initDasaViewBoxVerticalLine : window.dasaViewVerticaLine : ${window.dasaViewVerticaLine}`);

};

//event handler that moves vertical line in dasa view box
window.dasaViewHolder.addEventListener('mousemove', mouse => {

    //gets the measurements of the dasa view box
    let bbox_rect = window.dasaViewHolder.getBoundingClientRect();

    //calculate mouse X relative to dasa view box
    let relativeMouseX = mouse.clientX - bbox_rect.left;

    //move vertical line to under mouse inside dasa view box
    window.dasaViewVerticaLine.setAttribute('style', `left: ${relativeMouseX}px;`);
});