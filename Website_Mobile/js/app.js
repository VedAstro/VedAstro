//-----------------------------> MAIN APP CODE
//stored in an array(up to 10 pages deep) in local storage, and the `updateHistory` function is used to update the history array 

// create an array to store the navigation history (up to 10 pages deep)
const history = JSON.parse(localStorage.getItem('history')) || [];

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




////-----------------------------> CONTACT PAGE
//function sendMessage() {

//}





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
