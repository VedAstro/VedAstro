//take note on order
import { URLS } from '/js/URLS.js';
import * as Tools from '/js/Tools.js';
import * as Interop from '/js/Interop.js';


//URL specific to this instance
//interop depends on this big time
window.URLS = new URLS();

//make interop available to blazor
window.Interop = Interop;

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
