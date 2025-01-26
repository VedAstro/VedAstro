//-----------------------------> ADD PERSON PAGE

new PageHeader("PageHeader");
//new InfoBox("InfoBox_EasyImport_AddPerson");
new InfoBox("InfoBox_Private_AddPerson");
//new InfoBox("InfoBox_ForgotenTime_AddPerson");
new IconButton("IconButton_Advanced");
new IconButton("IconButton_Save_AddPerson");
let timeLocationInput = new TimeLocationInput("TimeLocationInput_AddPerson");
new PersonListViewer("PersonListViewer");

//update name input on loose focus to not be all caps
//make sure name is not fully capitalized, make it look nice
$(document).ready(function () {
    // Attach the function to the input element with id 'nameInput'
    $('#NameInput_AddPerson').on('blur', function () {
        // Get the current value of the input element
        var currentValue = $(this).val();

        // Convert the value to Pascal Case
        var convertedValue = CommonTools.convertNameToPascalCase(currentValue);

        // Update the input element with the converted value
        $(this).val(convertedValue);
    });
});


function OnClickAdvanced() {
    smoothSlideToggle('#NotesInputHolder');
    smoothSlideToggle(`#${timeLocationInput.TimezoneOffsetInputHolderID}`);
}

async function OnClickSave() {

    // if not logged in tell user what the f he is doing
    //if (VedAstro.IsGuestUser()) {
    //    const loginLink = `<a href="./Login.html" target="_blank" style="text-decoration-line: none;" class="link-primary fw-bold">logged in</a>`;
    //    const result = await Swal.fire({
    //        icon: 'info',
    //        title: 'Remember!',
    //        html: `You have not ${loginLink}, continue as <strong>Guest</strong>?`,
    //        showCancelButton: true,
    //        confirmButtonColor: '#3085d6',
    //        cancelButtonColor: '#d33',
    //        confirmButtonText: 'Yes, continue!'
    //    });
    //    if (!result.isConfirmed) return;
    //}

    // only continue if passed input field validation
    if (!(await isValidationPassed())) {
        Swal.close();
        return;
    }

    // show loading
    CommonTools.ShowLoading();

    // make a new person from the details in the input
    const person = await getPersonInstanceFromInput();

    // send newly created person to API server (server gives back unique ID 🆕)
    const newPersonId = await AddPersonViaApi(person);

    // update new id, before saving into browser storage
    person.PersonId = newPersonId;

    // After adding new person, set person as selected to make life easier for user (UX)
    // Use sessionStorage key set by caller in URL if any,
    // this allows each new person to be auto selected on a multi selector page 
    const selectedPersonStorageKey = new URL(window.location.href).searchParams.get('SelectedPersonStorageKey');
    if (selectedPersonStorageKey) {
        sessionStorage.setItem(selectedPersonStorageKey, JSON.stringify(person));
    }

    //clear cached person list (will cause person drop down to fetch new)
    PersonSelectorBox.ClearPersonListCache('private');

    // hide loading
    Swal.close();

    //play sound for better UX
    playBakingDoneSound();

    // show done message
    await Swal.fire({
        icon: 'success',
        title: 'Done ✅',
        text: 'Person added successfully!',
        timer: 2000,
        showConfirmButton: false
    });

    // send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
    navigateToPreviousPage();
}

// Add a person to the Vedastro API
// returns newly created ID for person
async function AddPersonViaApi(person) {

    // convert to birth time to correct URL format
    var timeUrl = person.BirthTime.ToUrl(); // sample: Location/Singapore/Time/00:00/24/06/2024/

    //construct URL to save Person in API Server 
    const apiUrl = [
        `${VedAstro.ApiDomain}/Calculate/AddPerson/`,
        `OwnerId/${VedAstro.IsGuestUser() ? VedAstro.VisitorId : VedAstro.UserId}`, //if guest use visitor id, else use user id
        `/${timeUrl}`, // exp: Location/Singapore/Time/00:00/24/06/2024/
        `PersonName/${person.Name}`, //NOTE: time URL has trailing, so we skip '/' before
        `/Gender/${person.Gender}`,
        `/Notes/${CommonTools.toUrlSafe(person.Notes)}`
    ].join('');

    // Make the API call to add the person
    // NOTE: server generates unique id based on existing db
    var jsonReply = await CommonTools.GetAPIPayload(apiUrl);

    //get first object (API call name)
    var newPersonId = jsonReply[Object.keys(jsonReply)[0]];

    return newPersonId;
}

//brings together all the individual data for making person 
//profile from page into 1 parsed Person instance object
async function getPersonInstanceFromInput() {
    const nameInput = document.getElementById("NameInput_AddPerson");
    const genderInput = document.getElementById("GenderInput_AddPerson");
    const notesInput = document.getElementById("NotesInput_AddPerson");

    //person ID is not filled, so Server can intelligently generate one
    const person = new Person({
        PersonId: "",
        Name: nameInput.value,
        Notes: notesInput.value,
        BirthTime: await timeLocationInput.getTimeJson(),
        Gender: genderInput.value,
        OwnerId: "", //not needed here since added later when making API call
        LifeEventList: []
    });

    return person;
}

/**
 * Validates the input fields.
 * @returns {Promise<boolean>} - True if validation passes.
 */
async function isValidationPassed() {
    // Prepare view components for checking
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
    const isValidTime = await timeLocationInput.isValid();
    if (!isValidTime) {
        await Swal.fire({
            icon: 'error',
            title: 'Check Location',
            text: 'Location name is missing or invalid!' //Note: though it checks both only location can go invalid, time has defaults so...yeah
        });
        return false;
    }

    // TEST 4: Check if user is sleeping by letting time be set as current year and date and month
    const tempTime = await timeLocationInput.getDateTimeOffset();
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