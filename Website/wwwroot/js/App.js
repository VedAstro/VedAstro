window.getBoundingClientRect = function (element, parm) {
    console.log("Hello world!");
    return element.getBoundingClientRect();
};

window.getWindowInnerWidth = function () {
    return window.innerWidth;
};

window.scrollElementLeft = function (element, parm) {
    console.log("JS : scrollLeft");

    element.scrollLeft += 500;
    console.log(`JS : ${element.scrollLeft}`);

};


window.getElementScrollLeft = function (element, parm) {
    console.log("JS : getElementScrollLeft");
    console.log(`JS: scrollWidth : ${element.scrollWidth}`);
    return element.scrollLeft;
};

window.getElementScrollWidth = function (element, parm) {
    return element.scrollWidth;
};

window.setElementScrollLeft = function (element, parm) {
    console.log("JS : setElementScrollLeft");
    element.scrollLeft = parm;
    console.log(`JS: newScrollPos : ${element.scrollLeft}`);

};