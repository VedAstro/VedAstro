//-----------------------------> MAIN APP CODE
//stored in an array(up to 10 pages deep) in local storage, and the `updateHistory` function is used to update the history array 

// create an array to store the navigation history (up to 10 pages deep)
const history = JSON.parse(localStorage.getItem('history')) || [];

//when errors occur allow user to reset all memory and restart
const handleError = () => {
    Swal.fire({
        title: 'App Crashed 🤕',
        text: 'Do you want to reset the app?',
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Reset App',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.value) {
            localStorage.clear();
            window.location.reload();
        }
    });
};

//handle all types of errors
window.onerror = handleError;
window.onunhandledrejection = handleError;

// function to update the navigation history, with current url
function updateHistory() {
    // Get the current URL
    let currentUrl = window.location.href;

    // add the new page to the history array
    history.push(currentUrl);

    // limit the history array to 10 pages deep
    if (history.length > 10) {
        history.shift();
    }

    // save the history array to local storage
    localStorage.setItem('history', JSON.stringify(history));
}

//similar to JQuery's .slideToggle("slow")
// note that this function uses CSS transitions for the
//sliding effect, which are smoother than jQuery’s animations but might not be supported in all browsers
function smoothSlideToggle(elementSelector, speed = 1000) {

    // Select the element
    let el = document.querySelector(elementSelector);

    // Check if the element is currently not displayed
    //SHOW
    if (window.getComputedStyle(el).display === 'none') {
        // Set the initial display to block
        el.style.display = 'block';

        // Capture the height of the element
        let height = el.offsetHeight;

        // Set the initial height to 0 and overflow to hidden
        el.style.height = 0;
        el.style.overflow = 'hidden';

        // Set the transition property for smooth animation
        el.style.transition = 'height 1s ease-in-out';

        // After a short delay, set the height to the element's original height
        setTimeout(() => {
            return el.style.height = height + 'px';
        }, 0);

        //once animation complete, make "help text" not cut when exceed div width
        //this is done by removing overflow property, which needs to be "hidden" during animation 
        el.addEventListener('transitionend', function transitionEnd(event) {
            // Remove the event listener
            event.target.removeEventListener('transitionend', transitionEnd);

            // Set the overflow to visible
            el.style.removeProperty('overflow');
        });
    }

    //HIDE
    else {

        //before animation starts, set overflow back to "hidden", for beautiful UX animation
        el.style.overflow = 'hidden';

        // If the element is currently displayed, set the transition property
        el.style.transition = 'height 1s ease-in-out';

        // Animate the height to 0
        el.style.height = 0;

        // After the transition is complete, set the display to none and remove the added styles
        setTimeout(() => {
            el.style.display = 'none';
            el.style.removeProperty('height');
            el.style.removeProperty('overflow');
            el.style.removeProperty('transition');
        }, speed);
    }
}

//goes to previous page URL that's saved to history, without using browsers' "back" feature
function navigateToPreviousPage() {
    // check if there is a previous page in the history array
    if (history.length > 1) {
        // remove the current page from the history array
        history.pop();

        // get the previous page from the history array
        let previousPage = history[history.length - 1];

        // navigate to the href
        window.location.href = previousPage;

        // update the navigation history in local storage
        localStorage.setItem('history', JSON.stringify(history));
    }
}


//-----------------------------> NUMEROLOGY PAGE
async function getNameNumber(fullName) {
    try {
        const response = await fetch(`https://vedastroapi.azurewebsites.net/api/Calculate/NameNumber/FullName/${fullName}`);
        const data = await response.json();
        if (data.Status === 'Pass') {
            const nameNumber = parseInt(data.Payload.NameNumber, 10);
            return nameNumber;
        } else {
            throw new Error(`API returned error: ${data.Status}`);
        }
    } catch (error) {
        console.error(error);
        return null; // or throw error, depending on your use case
    }
}

function updateNumerologyPrediction(number, text) {
    const numberOutput = document.getElementById('NumerologyPredictionNumberOutput');
    const textOutput = document.getElementById('NumerologyPredictionTextOutput');

    numberOutput.textContent = number;
    textOutput.textContent = text;

    //make visible, since default start hidden
    document.getElementById('NumerologyPredictionOutputHolder').style.display = 'block';
}


/**
* Plays done "baked ding" a sound using HTML5 element on page.
*/
function playBakingDoneSound() {
    const audio = new Audio("./sound/positive-notification.mp3");
    audio.play().catch((error) => console.error('Error playing sound:', error));
}


//scrolls to div on page and flashes div using JS
//can input both Element ref or CSS selector
//note: just ID without hashtag
async function scrollToDivById(id) {
    const element = document.getElementById(id);
    if (element) {
        await element.scrollIntoView();
    }
}

//flashes div using JS
//can input both Element ref or CSS selector
function animateHighlightElement(elmInput) {

    var $elm = $(elmInput);

    //use JS to attract attention to div
    $elm.fadeOut(200).fadeIn(200).fadeOut(200).fadeIn(200);

    $elm.fadeTo(100, 0.4, function () { $(this).fadeTo(500, 1.0); });

}
function printConsoleMessage() {
    $.get("./data/ConsoleGreeting.txt")
        .done((result) => {
            console.log(result);
        });
}


//-----------------CODE TO RUN

//print console greeting (file in wwwroot)
printConsoleMessage();


new PageTopNavbar("VedAstro","PageTopNavbar", [
    { icon: "mdi:book-open-page-variant-outline", text: "Guide", href: "/" },
    { icon: "carbon:gateway-api", text: "Open API", href: "/" },
    { icon: "openmoji:love-letter", text: "Donate", href: "/" },
], [
    { text: "Contact Us", href: "./Contact.html" },
    { text: "About", href: "./About.html" },
    { text: "Video Guides", href: "https://www.youtube.com/@vedastro/videos", target: "_blank" },
    { text: "Join Us", href: "./JoinOurFamily.html" },
    { text: "Calculators", href: "./Calculator.html" },
    { text: "Person List", href: "./PersonList.html" },
    { text: "Train AI", href: "./TrainAIAstrologer.html" },
    { text: "Remedy", href: "./Remedy.html" },
    { text: "Download", href: "./Download.html" },
    { text: "API Live Status", href: "https://vedastroapi.azurewebsites.net/api" },
    { text: "Table Generator", href: "./TableGenerator.html" },
    { text: "Body Types", href: "./BodyTypes.html" },
    { text: "Import Person", href: "./ImportPerson.html" },
], [
    { icon: "mdi:home", text: "Home", href: "./Home.html" },
    //{ icon: "mage:we-chat", text: "AI Chat", href: "./AIChat.html" },
    { icon: "fluent:book-star-20-filled", text: "Horoscope", href: "./Horoscope.html" },
    { icon: "bi:arrow-through-heart-fill", text: "Match", href: "./MatchChecker.html" },
    { icon: "mdi:numbers", text: "Numerology", href: "./Numerology.html" },
    { icon: "game-icons:lovers", text: "Find Match", href: "./MatchFinder.html" },
    { icon: "gis:map-time", text: "Life Predictor", href: "./LifePredictor.html" },
    { icon: "svg-spinners:clock", text: "Good Time Finder", href: "./GoodTimeFinder.html" },
]);

const links = [
    {
        url: 'Home',
        icon: 'ant-design:home-twotone',
        text: 'Home'
    },
    {
        url: 'MatchChecker',
        icon: 'bi:arrow-through-heart-fill',
        text: 'Match Checker'
    },
    //{
    //    url: 'AIChat',
    //    icon: 'fluent-mdl2:chat-bot',
    //    text: 'AI Chat'
    //},
    {
        url: 'LifePredictor',
        icon: 'gis:map-time',
        text: 'Life Predictor'
    },
    {
        url: 'MatchFinder',
        icon: 'game-icons:lovers',
        text: 'Match Finder'
    },
    {
        url: 'Horoscope',
        icon: 'fluent:book-star-20-filled',
        text: 'Horoscope'
    },
    {
        url: 'GoodTimeFinder',
        icon: 'svg-spinners:clock',
        text: 'Good Time Finder'
    },
    {
        url: 'APIBuilder',
        icon: 'mdi:cloud-tags',
        text: 'API Builder'
    },
    {
        url: 'Numerology',
        icon: 'fluent:text-number-format-20-filled',
        text: 'Numerology'
    },
    //{
    //    url: 'StarsAboveMe',
    //    icon: 'solar:moon-stars-bold',
    //    text: 'Stars Above Me'
    //}
];

new DesktopSidebar("DesktopSidebarHolder", links);

new PageFooter("PageFooter");
