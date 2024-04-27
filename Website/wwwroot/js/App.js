

//█▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
//█▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█
//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE

import { URLS } from '/js/URLS.js';
import * as Tools from '/js/Tools.js';
import * as Interop from '/js/Interop.js';

//HARD DATA
//hardcoded list of countries
//used for easy life event location
window.countries = ["Afghanistan", "Aland Islands", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bonaire, Sint Eustatius and Saba", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, Democratic Republic of the Congo", "Cook Islands", "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Curacao", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Island and Mcdonald Islands", "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran, Islamic Republic of", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kosovo", "Kuwait", "Kyrgyzstan", "Lao People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macao", "Macedonia, the Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Palestinian Territory, Occupied", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Barthelemy", "Saint Helena", "Saint Kitts and Nevis", "Saint Lucia", "Saint Martin", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Serbia and Montenegro", "Seychelles", "Sierra Leone", "Singapore", "Sint Maarten", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Timor-Leste", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Viet Nam", "Virgin Islands, British", "Virgin Islands, U.s.", "Wallis and Futuna", "Western Sahara", "Yemen", "Zambia", "Zimbabwe"];

//options for printing PDF, interop uses it
window.PDFOptions = {
    margin: 2,
    filename: `match-report.pdf`,
    //    image: { type: 'jpeg', quality: 0.98 },
    html2canvas: { scale: 3 },
    jsPDF: { unit: 'cm', format: 'A4', orientation: 'portrait' }
};



//SOFT DATA
//initialize separate worker thread to handle all logging
//goes first to make sure logger is ready to catch everybody else
window.LogThread = new Worker('js/LogThread.js');

//URL specific to this instance
//interop depends on this big time
window.URLS = new URLS();

//override the default error exception handler
//this allows us stop the JVM going red
onerror = handleErr;

//make interop available to blazor
window.Interop = Interop;

//TODO marked for oblivion
//var apiKey = "089J89JF9W8JFJN49"; //copy from account page
////this will be called by blazor as window.API.GetChart()
//window.API = new VedAstro(apiKey);


//print console greeting (file in wwwroot)
Tools.printConsoleMessage();


//small screen i told you so message
//will only show if small screen
await SmallScreenGreetingMessage();


/*
      Customized JS for initializing Darkmode.js
 */

const options = {
    mixColor: '#fff', // default: '#fff'
    backgroundColor: '#fff', // default: '#fff'
    buttonColorDark: '#100f2c', // default: '#100f2c'
    buttonColorLight: '#fff', // default: '#fff'
    saveInCookies: true, // default: true,
    autoMatchOsTheme: false // default: true
};

//makes dark mode toggle available to Blazor
window.DarkMode = new Darkmode(options);

//eyes on page logger
window.MinutesPassed = 0.5;
setInterval(EyesOnPageLogger, 30 * 1000);



//-------------------------------------------------------------------------


//█▀ █▀█ █▀▀ █▀▀ █ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ █▀
//▄█ █▀▀ ██▄ █▄▄ █ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ▄█



//simple log on time spent on page analytic
//track time spent to read page
async function EyesOnPageLogger() {

    //based on page make count of minutes
    var onSamePage = window.MinutedPassedPage == window.location.href;
    if (onSamePage) {
        //increment time on page
        window.MinutesPassed = window.MinutesPassed + 0.5;
        window.MinutedPassedPage = window.location.href; //save page
    } else {
        //reset since new page
        window.MinutedPassedPage = 0.5;
        window.MinutedPassedPage = window.location.href; //save page
    }

    //if time exceed 10 min no count, call it quits will log too much
    if (window.MinutesPassed > 10) { return; }

    //current page url
    var msg = `EYES ON PAGE ${window.MinutesPassed} MIN`;

    //get log payload from blazor
    //var payload = await DotNet.invokeMethodAsync('Website', 'GetDataLogPayload', msg);

    //send a copy to server for logging
    //window.LogThread.postMessage(payload);
}

//makes sure every function needed by blazor is ready to be called
//this allows full scale gonzo development pattern
window.GetInteropFuncList = () => {
    var list = Object.keys(Interop);
    return list;
}

//will show warning message to users with small screen or windows
//for site is better viewed above 1080p
async function SmallScreenGreetingMessage() {

    //only show warning once
    var isShownBefore = "IsShownSmallScreenMessage" in localStorage ? JSON.parse(localStorage["IsShownSmallScreenMessage"]) : false;

    //check if screen is too small
    var isTooSmall = window.innerWidth < 720;


    if (isTooSmall && !isShownBefore) {
        //show special message
        Swal.fire('Screen Little Small!', 'Use device with <strong>larger screen</strong> for best experience', 'info');

        //don't bother user too much, note that message shown, so next time no need to show
        localStorage["IsShownSmallScreenMessage"] = JSON.stringify(true);
    }

}

//allows to override browser default error handler
//todo move to tools
function handleErr(msg, url, line_no) {

    //gather the info to about the error
    var errorMsg = "Error: " + msg + "\n";
    errorMsg += "URL: " + url + "\n";
    errorMsg += "Line: " + line_no + "\n\n";

    //print out in console for atm debugging
    console.log(errorMsg);

    //send a copy to server for logging
    var errorXml = `<Error>${errorMsg}</Error>`;
    window.LogThread.postMessage(errorXml);

    return true;
}


//-------------------------------------------------------------------------
