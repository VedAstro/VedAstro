

//█▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
//█▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█
//PRODUCTIONS FUNCTION IN USE CALLED FROM BLAZOR CODE

import { URLS } from '/js/URLS.js';
import { VedAstro } from '/js/VedAstro.js';
import * as Tools from '/js/Tools.js';
import * as Interop from '/js/Interop.js';



//make interop available to blazor
window.Interop = Interop;

//URL specific to this instance
window.URLS = new URLS();

//options for printing PDF, interop uses it
window.PDFOptions  = {
    margin: 3,
    filename: `match-report.pdf`,
    //    image: { type: 'jpeg', quality: 0.98 },
    html2canvas: { scale: 3 },
    jsPDF: { unit: 'cm', format: 'A4', orientation: 'portrait' }
};

function handleErr(msg, url, line_no) {
    var errorMsg = "Error: " + msg + "\n";
    errorMsg += "URL: " + url + "\n";
    errorMsg += "Line: " + line_no + "\n\n";

    alert(errorMsg);

    return true;
}

// Set the global onerror; 
onerror = handleErr;

var apiKey = "089J89JF9W8JFJN49"; //copy from account page
//this will be called by blazor as window.API.GetChart()
window.API = new VedAstro(apiKey);

//hardcoded list of countries
//used for easy life event location
window.countries = ["Afghanistan", "Aland Islands", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bonaire, Sint Eustatius and Saba", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, Democratic Republic of the Congo", "Cook Islands", "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Curacao", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Island and Mcdonald Islands", "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran, Islamic Republic of", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kosovo", "Kuwait", "Kyrgyzstan", "Lao People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macao", "Macedonia, the Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Palestinian Territory, Occupied", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Barthelemy", "Saint Helena", "Saint Kitts and Nevis", "Saint Lucia", "Saint Martin", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Serbia and Montenegro", "Seychelles", "Sierra Leone", "Singapore", "Sint Maarten", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Timor-Leste", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Viet Nam", "Virgin Islands, British", "Virgin Islands, U.s.", "Wallis and Futuna", "Western Sahara", "Yemen", "Zambia", "Zimbabwe"];

//initialize separate worker thread to handle all logging
window.LogThread = new Worker('js/LogThread.js');

//print console greeting (file in wwwroot)
Tools.printConsoleMessage();

/*
      Customized JS for initializing Darkmode.js
 */

const options = {
    mixColor: '#fff', // default: '#fff'
    backgroundColor: '#fff',  // default: '#fff'
    buttonColorDark: '#100f2c',  // default: '#100f2c'
    buttonColorLight: '#fff', // default: '#fff'
    saveInCookies: true, // default: true,
    autoMatchOsTheme: false // default: true
}

//makes dark mode toggle available to Blazor
window.DarkMode = new Darkmode(options);




//█░░ █▀█ █▀▀ █ █▄░█   █▀▀ █▀█ █▀▄ █▀▀
//█▄▄ █▄█ █▄█ █ █░▀█   █▄▄ █▄█ █▄▀ ██▄

//makes a reference to SignInButton instance, to be used when user clicks sign in
//called in Blazor, after component render
window.SetSignInButtonInstance = (instance) => window.SignInButtonInstance = instance;
//wrapper function to forward call to blazor (hardwired in Blazor HTML)
window.OnGoogleSignInSuccessHandler = (response) => window.SignInButtonInstance.invokeMethodAsync('OnGoogleSignInSuccessHandler', response);

//called from Blazor when custom login button clicked
window.facebookLogin = () => FB.login(callBackFB, { scope: 'email' });
//wrapper function to forward call to blazor (hardwired in Blazor HTML)
var callBackFB = (response) => window.SignInButtonInstance.invokeMethodAsync('OnFacebookSignInSuccessHandler', response);

//-------------------------------------------------------------------------


//█▀ █▀█ █▀▀ █▀▀ █ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ █▀
//▄█ █▀▀ ██▄ █▄▄ █ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ▄█

//makes sure every function needed by blazor is ready to be called
//this allows full scale gonzo development pattern

window.GetInteropFuncList = () => {
    var list = Object.keys(Interop);
    return list;
}


//-------------------------------------------------------------------------
