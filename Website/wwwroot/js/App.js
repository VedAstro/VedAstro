
//██╗      ██████╗  ██████╗ ██╗ ██████╗
//██║     ██╔═══██╗██╔════╝ ██║██╔════╝
//██║     ██║   ██║██║  ███╗██║██║
//██║     ██║   ██║██║   ██║██║██║
//███████╗╚██████╔╝╚██████╔╝██║╚██████╗
//╚══════╝ ╚═════╝  ╚═════╝ ╚═╝ ╚═════╝
//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE


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

//function to get cookies given a name
function getCookiesWrapper (cookieName) {
    console.log(`JS: getCookiesWrapper`);
    return Cookies.get(cookieName);
};

//function to set cookies given a name
function setCookiesWrapper (cookieName, cookieValue) {
    console.log(`JS: setCookiesWrapper`);
    return Cookies.set(cookieName, cookieValue);
};

//Jquery to attach event listener to inputed element
function addEventListenerWrapper (element, eventName, functionName) {
    console.log(`JS: addEventListenerWrapper : ${eventName} : ${functionName}`);
    element.addEventListener(eventName, window[functionName]);
};

//gets current page url
function getUrl () {
    console.log(`JS: getUrl`);
    return window.location.href;
};

function getGoogleUserName () {
    console.log(`JS: getGoogleUserName`);
    return window.googleUserName;
};

function getGoogleUserEmail () {
    console.log(`JS: googleUserEmail`);
    return window.googleUserEmail;
};

function getGoogleUserIdToken() {
    console.log(`JS: googleUserIdToken`);
    return window.googleUserIdToken;
};

//called by Google sign out button
function signOut() {

    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut();

    //fire event in Blazor, that user just signed out
    DotNet.invokeMethod('Website', 'InvokeOnUserSignOut');

}




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

//this function gets called by google sign in button after successful sign in
//note : this function's name is hardwired in Blazor
function onSignInEventHandler(googleUser) {

    console.log(`JS: onSignInEventHandler`);

    //get the google user's details and save it to be accessed by Blazor
    var profile = googleUser.getBasicProfile();
    window.googleUserName = profile.getName();
    window.googleUserEmail = profile.getEmail();
    var id_token = googleUser.getAuthResponse().id_token;
    window.googleUserIdToken = id_token;

    //fire event in Blazor, that user just signed in
    DotNet.invokeMethod('Website', 'InvokeOnUserSignIn');

}

