const vedAstroJsHash = "f0f351f8777dcd61cf8638e8b5fe1ec516cc0672ae8ebeb8ee502c00f38552e8";


//-----------------------------> MAIN APP CODE
// create an array to store the navigation history (up to 10 pages deep)
const history = JSON.parse(localStorage.getItem('history')) || [];

//when errors occur send to server for logging
const handleError = (error) => {
    // Get error details
    const errorDetails = {
        message: error.message,
        stack: error.stack,
        url: window.location.href,
        userAgent: navigator.userAgent,
        ipAddress: ''
    };

    // Get client's IP address using an external service
    fetch('https://api.ipify.org?format=json')
        .then(response => response.json())
        .then(data => {
            errorDetails.ipAddress = data.ip;

            // Send error details to server
            fetch('https://api.vedastro.org/api/LogError', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(errorDetails)
            });
        });
};

//handle all types of errors
window.onerror = handleError;
window.onunhandledrejection = handleError;

// Call the function to check the hash and refresh if necessary
checkAndUpdateVedAstroJs();

// Optionally, you can set an interval to periodically check for updates
setInterval(checkAndUpdateVedAstroJs, 60 * 60 * 1000); // Check every hour


// gets latest hash from server & compares with local placed during build
async function checkAndUpdateVedAstroJs() {
    async function fetchLatestHash() {
        try {
            const response = await fetch('https://api.vedastro.org/api/GetVedAstroJSHash');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            if (data.Status === "Pass") {
                return data.Payload;
            } else {
                console.error('Failed to get a valid hash:', data);
                return null;
            }
        } catch (error) {
            console.error('Failed to fetch the latest hash:', error);
            return null;
        }
    }

    const latestHash = await fetchLatestHash();

    if (latestHash && latestHash !== vedAstroJsHash) {
        console.log('New version detected. Refreshing the page...');
        location.reload(true); // Reload the page from the server
    } else {
        console.log('No new version detected. Current version is up-to-date.');
    }
}


//similar to JQuery's .slideToggle("slow")
//note that this function uses CSS transitions for the
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
    // Parse the history from localStorage
    let history = JSON.parse(localStorage.getItem('history')) || [];

    // Check if there is a previous page in the history array
    if (history.length > 1) {
        // Get the previous page from the history array
        let previousPage = history[history.length - 1];

        // Update the navigation history in local storage before navigation
        localStorage.setItem('history', JSON.stringify(history));

        // Navigate to the previous page
        window.location.href = previousPage;
    } else {
        // Optional: Handle the case where there's no previous page
        console.warn('No previous page found in history.');
    }
}

// function to update the navigation history, with current url
function updateHistory() {
    // Parse the history from localStorage or initialize it
    let history = JSON.parse(localStorage.getItem('history')) || [];

    // Get the current URL
    let currentUrl = window.location.href;

    // Check if the current URL is different from the last one in the history
    if (history.length === 0 || history[history.length - 1] !== currentUrl) {
        // Add the new page to the history array
        history.push(currentUrl);

        // Limit the history array to 10 pages deep
        if (history.length > 10) {
            history.shift();
        }

        // Save the history array to localStorage
        localStorage.setItem('history', JSON.stringify(history));
    }
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
    //{ icon: "mdi:book-open-page-variant-outline", text: "Guide", href: "/" },
    { icon: "carbon:gateway-api", text: "Open API", href: "./APIBuilder.html" },
    { icon: "openmoji:love-letter", text: "Donate", href: "./Donate.html" },
], [
    { icon: "heroicons-outline:mail", text: "Contact Us", href: "./ContactUs.html" },
    { icon: "ix:about", text: "About", href: "./About.html" },
    { icon: "octicon:video-16", text: "Video Guides", href: "https://www.youtube.com/@vedastro/videos", target: "_blank" },
    /*{ text: "Join Us", href: "./JoinOurFamily.html" },*/
    /*{ text: "Calculators", href: "./Calculator.html" },*/
    { icon: "line-md:list", text: "Person List", href: "./PersonList.html" },
    /*{ text: "Train AI", href: "./TrainAIAstrologer.html" },*/
    /*{ text: "Remedy", href: "./Remedy.html" },*/
    /*{ text: "Download", href: "./Download.html" },*/
    /*{ text: "API Live Status", href: "https://vedastroapi.azurewebsites.net/api/Home" },*/
    /*{ text: "Table Generator", href: "./TableGenerator.html" },*/
    /*{ text: "Body Types", href: "./BodyTypes.html" },*/
    /*{ text: "Import Person", href: "./ImportPerson.html" },*/
], [
    { icon: "mdi:home", text: "Home", href: "./Home.html" },
    //{ icon: "mage:we-chat", text: "AI Chat", href: "./AIChat.html" },
    { icon: "fluent:book-star-20-filled", text: "Horoscope", href: "./Horoscope.html" },
    { icon: "bi:arrow-through-heart-fill", text: "Match", href: "./MatchChecker.html" },
    { icon: "mdi:numbers", text: "Numerology", href: "./Numerology.html" },
    /*{ icon: "game-icons:lovers", text: "Find Match", href: "./MatchFinder.html" },*/
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
    //{
    //    url: 'MatchFinder',
    //    icon: 'game-icons:lovers',
    //    text: 'Match Finder'
    //},
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
    {
        url: 'MiniCalculators',
        icon: 'fluent:calculator-multiple-20-regular',
        text: 'Mini Calculators'
    },

    //{
    //    url: 'StarsAboveMe',
    //    icon: 'solar:moon-stars-bold',
    //    text: 'Stars Above Me'
    //}
];

new DesktopSidebar("DesktopSidebarHolder", links);

new PageFooter("PageFooter");
