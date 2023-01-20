console.log("Events Chart Internal JS V1");


//make tick appear and disappear
//$("#LifeEventsCheckBox").click(function () {

//    //get tick element
//    var tickElm = $(this).find('.Tick');
//    if (tickElm.is(":visible")) {
//        tickElm.hide();
//    } else {
//        tickElm.show();
//    }
//});

$(".CheckBox").click(function () {

    var checkbox = $(this);
    var tickElm = checkbox.find('.Tick');

    //toggle display of the tick mark
    ToggleElm(tickElm);

    //get text of check box
    var text = checkbox.find("text").text();

    //based on text handle the call appropriately
    switch (text) {
        case 'Life Events': ToggleElm($("#LifeEventLinesHolder")); break;
        case 'Color Summary': ClickLifeEventsCheckBox(); break;
        case 'Bar Summary': ClickLifeEventsCheckBox(); break;
        case 'Sun': ClickLifeEventsCheckBox(); break;
        case 'Moon': ClickLifeEventsCheckBox(); break;
        case 'Mars': ClickLifeEventsCheckBox(); break;
        case 'Mercury': ClickLifeEventsCheckBox(); break;
        default: console.log('Selected value not handeled!');
    }
});

function ToggleElm(element) {

    var svgElm = SVG(element[0]);

    ////get visible status of element
    //var styleText = element?.attr("style") ?? "";
    //var isHidden = styleText.includes("display: none;");

    if (svgElm.visible()) {
        //element.css("display", "none");
        svgElm.hide();
    } else {
        //element.css("display", "block");
        svgElm.show();
    }

}

function ClickLifeEventsCheckBox() {

}