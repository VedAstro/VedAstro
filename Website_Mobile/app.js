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

//navigate to page artificially exp :  onclick="showSection('Home')"
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
async function OnClickSave_AddPerson() {

    // if not logged in tell user what the f he is doing
    if (window.vedastro.IsGuestUser) {
        const loginLink = `<a target="_blank" style="text-decoration-line: none;" onclick="showSection('Login')" class="link-primary fw-bold">logged in</a>`;
        const result = await Swal.fire({
            icon: 'info',
            title: 'Remember!',
            html: `You have not ${loginLink}, continue as <strong>Guest</strong>?`,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, continue!'
        });
        if (!result.isConfirmed) return;
    }

    // show loading
    CommonTools.ShowLoading();

    // only continue if passed input field validation
    if (!(await isValidationPassed_AddPerson())) {
        Swal.close();
        return;
    }

    // make a new person from the details in the input
    const person = await getPersonInstanceFromInput();

    // send newly created person to API server
    const newPersonId = await CommonTools.AddPerson(person);

    // update new id, before saving into browser storage
    person.Id = newPersonId;

    // after adding new person set person, as selected to make life easier for user (UX)
    localStorage.setItem('selectedPerson', JSON.stringify(person));

    // hide loading
    Swal.close();

    // show done message
    Swal.fire({
        icon: 'success',
        title: 'Done!',
        text: 'Person added successfully!',
        timer: 1500
    });

    // wait a little and send user back to previous page
    setTimeout(() => {
        navigateToPreviousPage();
    }, 1500);
}

//brings together all the individual data for making person profile from page into 1 object that looks like
//Sample JSON : {"PersonId":"xxxxx","Name":"Risyaalini Priyaa","Notes":"","BirthTime":{"StdTime":"13:54 25/10/1992 +08:00","Location":{"Name":"Taiping","Longitude":103.82,"Latitude":1.352}},"Gender":"Female","OwnerId":"xxxxxx","LifeEventList":[{"PersonId":"xxxxx","Id":"xxxxxx","Name":"Talks of Marriage","StartTime":{"StdTime":"23:02 05/02/2023 +08:00","Location":{"Name":"Taiping","Longitude":0,"Latitude":0}},"Description":"Marriage not yet confirmed looking for husband, venus bhukti with house 7 gochara","Nature":"Good","Weight":"Minor"}]}
function getPersonInstanceFromInput() {
    const nameInput = document.getElementById("NameInput_AddPerson");
    const genderInput = document.getElementById("GenderInput_AddPerson");
    const timeLocationInput = window.vedastro.TimeLocationInputInstances["TimeLocationInput_AddPerson"];


    const person = new Person({
        PersonId: "",
        Name: nameInput.value,
        Notes: "",
        BirthTime: timeLocationInput.getTimeJson(),
        Gender: genderInput.value,
        OwnerId: "",
        LifeEventList: []
    });

    return person;
}

async function isValidationPassed_AddPerson() {
    // Prepare view components for checking
    var timeInput = window.vedastro.TimeLocationInputInstances["TimeLocationInput_AddPerson"];
    const nameInput = document.getElementById("NameInput_AddPerson");
    const genderInput = document.getElementById("GenderInput_AddPerson");

    // TEST 1: Name
    if (nameInput.value.trim() === "") {
        await Swal.fire({
            icon: 'error',
            title: 'Name is Required',
            text: 'Please enter a valid name. This will help you identify the correct person later.'
        });
        return false;
    }

    // TEST 2: Gender
    if (genderInput.value.trim() === "") {
        await Swal.fire({
            icon: 'error',
            title: 'Gender is Required',
            text: 'Please select a valid gender. Necessary for accurate calculations and predictions.'
        });
        return false;
    }


    // TEST 3: Time & Location
    const isValidTime = await timeInput.isValid();
    if (!isValidTime) {
        await Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Location missing or invalid!' //Note: though it checks both only location can go invalid, time has defaults so...yeah
        });
        return false;
    }

    // TEST 4: Check if user is sleeping by letting time be set as current year and date and month
    const tempTime = await timeInput.getDateTimeOffset();
    const thisYear = tempTime.year === new Date().getFullYear();
    const thisMonth = tempTime.month === new Date().getMonth();
    const thisDate = tempTime.date === new Date().getDate();
    const isSameYear = thisYear;
    const isSameMonth = thisYear && thisMonth;
    const isSameDate = thisYear && thisMonth && thisDate;
    if (isSameYear || isSameMonth || isSameDate) {
        const tempText = isSameDate ? 'today' : isSameMonth ? 'this month' : 'this year';
        const result = await Swal.fire({
            icon: 'question',
            title: 'Are you sure?',
            html: `You set <strong>${tempText}</strong> as your birth date, is this correct?`,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, correct!'
        });
        if (!result.isConfirmed) {
            return false;
        }
    }


    // TEST 5: Possible missing TIME 00:00
    const isTime0 = tempTime.minute === 0 && tempTime.hour === 0; // Possible user left it out
    if (isTime0) {
        const result = await Swal.fire({
            icon: 'question',
            title: 'Born exactly at 00:00 AM?',
            text: 'Looks like you did not fill birth time. Are you sure this is correct?',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, correct!'
        });
        if (!result.isConfirmed) {
            return false;
        }
    }

    // TEST 6: No single alphabet names please
    const tooShort = nameInput.value.length <= 3;
    if (tooShort) {
        const result = await Swal.fire({
            icon: 'question',
            title: 'Such a short name? Suspicious',
            text: `Only machines use short names like ${nameInput.value}, are you a machine?`,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'No, I\'m human!'
        });
        if (!result.isConfirmed) {
            return false;
        }
    }

    // TEST 7: No numbers please
    const isDigitPresent = /\d/.test(nameInput.value);
    if (isDigitPresent) {
        const result = await Swal.fire({
            icon: 'question',
            title: 'Are you a machine?',
            text: `Only machines have names with numbers like ${nameInput.value}, are you a machine?`,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'No, I\'m human!'
        });
        if (!result.isConfirmed) {
            return false;
        }
    }

    // If control reaches here, it's valid
    return true;
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