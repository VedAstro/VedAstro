//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE

//██╗      ██████╗  ██████╗ ██╗ ██████╗
//██║     ██╔═══██╗██╔════╝ ██║██╔════╝
//██║     ██║   ██║██║  ███╗██║██║
//██║     ██║   ██║██║   ██║██║██║
//███████╗╚██████╔╝╚██████╔╝██║╚██████╗
//╚══════╝ ╚═════╝  ╚═════╝ ╚═╝ ╚═════╝


//uses UAParser library to extract user data
function getVisitorData() {
    console.log(`JS: getVisitorData`);
    var parser = new UAParser();
    return parser.getResult();
}

//Jquery to show inputed element
function showWrapper(element) {
    console.log(`JS: showWrapper`);
    $(element).show();
};

//Jquery to hide inputed element
function hideWrapper (element) {
    console.log(`JS: hideWrapper`);
    $(element).hide();
};

//Jquery to attach event listener to inputed element
function addEventListenerWrapper (element, eventName, functionName) {
    console.log(`JS: addEventListenerWrapper : ${eventName} : ${functionName}`);
    element.addEventListener(eventName, window[functionName]);
};





//███████╗██╗░░░██╗███████╗███╗░░██╗████████╗  ██╗░░██╗░█████╗░███╗░░██╗██████╗░██╗░░░░░███████╗██████╗░░██████╗
//██╔════╝██║░░░██║██╔════╝████╗░██║╚══██╔══╝  ██║░░██║██╔══██╗████╗░██║██╔══██╗██║░░░░░██╔════╝██╔══██╗██╔════╝
//█████╗░░╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░  ███████║███████║██╔██╗██║██║░░██║██║░░░░░█████╗░░██████╔╝╚█████╗░
//██╔══╝░░░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░  ██╔══██║██╔══██║██║╚████║██║░░██║██║░░░░░██╔══╝░░██╔══██╗░╚═══██╗
//███████╗░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░  ██║░░██║██║░░██║██║░╚███║██████╔╝███████╗███████╗██║░░██║██████╔╝
//╚══════╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░  ╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝╚═════╝░╚══════╝╚══════╝╚═╝░░╚═╝╚═════╝░



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
