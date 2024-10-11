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

//TODO MARKED FOR OBLIVION
//navigate to page href set in clicked element
//function navigateToPage(clickedLink) {

//    // update the navigation history
//    updateHistory(clickedLink);

//    // get the href attribute from the clicked element
//    const href = clickedLink.getAttribute('href');

//    // navigate to the href
//    window.location.href = href;
//}

//goes to previous page that saved to history, without using browsers' "back" feature
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


//RUN CODE
// sidebar show/hide
//const buttontoggle = document.getElementById('sidebartoggle');
//const desktopSidebar = document.getElementById('desktopsidebar');
//buttontoggle.addEventListener('click',
//    () => {
//        desktopSidebar.classList.toggle('d-md-block');
//    });