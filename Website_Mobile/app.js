//-----------------------------> MAIN APP CODE
//code handles a single-page app with sections 
//and a navigation menu that allows users to switch between them.The navigation history is
//stored in an array(up to 10 pages deep) in local storage, and the `updateHistory` function is used to update the history array 
//whenever the user navigates to a new page.The`showSection` function is used to show the specified section(page) and hide all others

// get all sections (pages)
const sections = document.querySelectorAll('section');

// create an array to store the navigation history (up to 10 pages deep)
const history = JSON.parse(localStorage.getItem('history')) || [];

// function to update the navigation history
function updateHistory(newPage) {
    // add the new page to the history array
    history.push(newPage);
    // limit the history array to 10 pages deep
    if (history.length > 10) {
        history.shift();
    }
    // save the history array to local storage
    localStorage.setItem('history', JSON.stringify(history));
}

//navigate to page artificially
function showSection(sectionId) {

    // update the navigation history
    updateHistory(sectionId);

    // Selection of all section HTML elements
    const sections = document.querySelectorAll('section');

    // Looping through every selected section node.
    sections.forEach((section) => {
        // Removing 'active' class from each section node.
        section.classList.remove('active');

        // Addition of 'active' class to match the specific sectionID being displayed.
        if (section.id === sectionId) {
            section.classList.add('active');
        }
    });
}

function navigateToPreviousPage() {
    // check if there is a previous page in the history array
    if (history.length > 1) {
        // remove the current page from the history array
        history.pop();
        // get the previous page from the history array
        const previousPage = history[history.length - 1];
        // show the previous page
        showSection(previousPage);
        // update the navigation history in local storage
        localStorage.setItem('history', JSON.stringify(history));
    }
}

// show the first section (page) by default
showSection('Home');





//-----------------------------> HOROSCOPE PAGE
new PageHeader("HoroscopePageHeader");
new PersonSelectorBox("PersonSelectorBox_Horoscope");
new InfoBox("InfoBox_AskAI_Horoscope");
new InfoBox("InfoBox_EasyImport_Horoscope");
new InfoBox("InfoBox_ForgotenTime_Horoscope");


//-----------------------------> CONTACT PAGE
function sendMessage() {

}


//-----------------------------> ADD PERSON PAGE
new PageHeader("AddPersonPageHeader");
new PersonSelectorBox("PersonSelectorBox_Horoscope");
new InfoBox("InfoBox_EasyImport_AddPerson");
new InfoBox("InfoBox_Private_AddPerson");
new InfoBox("InfoBox_ForgotenTime_AddPerson");
new IconButton("IconButton_Back_AddPerson");
new IconButton("IconButton_Save_AddPerson");
new TimeLocationInput("TimeLocationInput_AddPerson");


function OnClickBack_AddPerson() {
    navigateToPreviousPage();
}



function OnClickSave_AddPerson(parameters) {
    alert(parameters);
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