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
        case 'Color Summary': ToggleElm($("#ColorRow")); break;
        case 'Smart Summary': ToggleElm($("#BarChartRowSmart")); break;
        case 'Bar Summary': ToggleElm($("#BarChartRow")); break;
        case 'Sun': ClickLifeEventsCheckBox(); break;
        case 'Moon': ClickLifeEventsCheckBox(); break;
        case 'Mars': ClickLifeEventsCheckBox(); break;
        case 'Mercury': ClickLifeEventsCheckBox(); break;
        default: console.log('Selected value not handeled!');
    }
});

function ToggleElm(element) {

    var svgElm = SVG(element[0]);

    if (svgElm.visible()) {
        svgElm.hide();
    } else {
        svgElm.show();
    }

}

function ClickLifeEventsCheckBox() {

}