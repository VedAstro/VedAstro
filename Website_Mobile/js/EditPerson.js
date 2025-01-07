//-----------------------------> EDIT PERSON PERSON PAGE

// Initialize components
new PageHeader("PageHeader");
new InfoBox("InfoBox_Private");
new InfoBox("InfoBox_ForgotenTime");
new IconButton("IconButton_Advanced");
new IconButton("IconButton_Save");
new IconButton("IconButton_Delete");
let timeLocationInput = new TimeLocationInput("TimeLocationInput");
new PersonListViewer("PersonListViewer");

// Variable to hold the original person data
let originalPerson;


$(document).ready(function () {
    // attach handler Update name input on losing focus to not be all caps
    $('#NameInput').on('blur', function () {
        var currentValue = $(this).val();
        var convertedValue = CommonTools.convertNameToPascalCase(currentValue);
        $(this).val(convertedValue);
    });

    // Load existing person data
    loadExistingPersonData();

});


/**
 * Loads the existing person's data into the input fields.
 */
function loadExistingPersonData() {
    // Get the selected person's storage key from the URL parameter
    const selectedPersonStorageKey = new URL(window.location.href).searchParams.get('SelectedPersonStorageKey');

    if (selectedPersonStorageKey) {
        const personJson = sessionStorage.getItem(selectedPersonStorageKey);
        if (personJson) {
            originalPerson = new Person(JSON.parse(personJson));

            // Populate input fields with person's data
            $('#NameInput').val(originalPerson.Name);
            $('#GenderInput').val(originalPerson.Gender);
            $('#NotesInput').val(originalPerson.Notes);

            // Set time and location into TimeLocationInput
            timeLocationInput.setInputDateTime(originalPerson.BirthTime);
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Person not found',
                text: 'Could not find person data in storage.'
            });
        }
    } else {
        Swal.fire({
            icon: 'error',
            title: 'No person selected',
            text: 'Please use the Edit button from the person list.'
        });
    }
}

/**
 * Handles the Advanced button click event to show/hide advanced options.
 */
function OnClickAdvanced() {
    smoothSlideToggle('#NotesInputHolder');
    smoothSlideToggle(`#${timeLocationInput.TimezoneOffsetInputHolderID}`);
}

/**
 * Handles the Save button click event to validate and save the updated person data.
 */
async function OnClickSave() {
    // Check if the user is a guest
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

    // Validate input fields
    if (!(await isValidationPassed())) {
        Swal.close();
        return;
    }

    // Show loading indicator
    CommonTools.ShowLoading();

    // Create a new person instance from input
    const person = await getPersonInstanceFromInput();

    // Copy PersonId and OwnerId from the original person
    person.PersonId = originalPerson.PersonId;
    person.OwnerId = originalPerson.OwnerId;

    // Update the person on the server
    const updateResult = await UpdatePersonViaApi(person);

    if (updateResult) {
        //clear cached person list (will cause person drop down to fetch newly updated data from API)
        PersonSelectorBox.ClearPersonListCache('private');

        // Update selected person in session storage
        const selectedPersonStorageKey = new URL(window.location.href).searchParams.get('SelectedPersonStorageKey');
        if (selectedPersonStorageKey) {
            sessionStorage.setItem(selectedPersonStorageKey, JSON.stringify(person));
        }

        // Hide loading indicator
        Swal.close();

        // Play sound for better UX
        playBakingDoneSound();

        // Show success message
        await Swal.fire({
            icon: 'success',
            title: 'Done ✅',
            text: 'Person updated successfully!',
            timer: 2300,
            showConfirmButton: false
        });

        // send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
        navigateToPreviousPage();

    } else {
        // Hide loading indicator
        Swal.close();

        // Show error message
        await Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Failed to update person. Please try again later.'
        });
    }

}

/**
 * Handles the Delete button click event to delete the person.
 */
async function OnClickDelete() {
    // Confirm with user before deleting
    const result = await Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        html: 'This will delete the person <strong>permanently</strong>.',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete!',
        cancelButtonText: 'Cancel',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6'
    });

    if (result.isConfirmed) {
        // Show loading indicator
        CommonTools.ShowLoading();

        // Call the API to delete the person
        const deleteResult = await DeletePersonViaApi(originalPerson.PersonId);

        if (deleteResult) {
            // Clear cached person list
            PersonSelectorBox.ClearPersonListCache('private');

            // Remove selected person from session storage
            const selectedPersonStorageKey = new URL(window.location.href).searchParams.get('SelectedPersonStorageKey');
            if (selectedPersonStorageKey) {
                sessionStorage.removeItem(selectedPersonStorageKey);
            }

            // Hide loading indicator
            CommonTools.HideLoading();

            // Play sound for better UX
            playBakingDoneSound();

            // Show success message
            await Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: 'Person has been deleted successfully.',
                timer: 2300,
                showConfirmButton: false
            });

            // Navigate back to the person list or previous page
            navigateToPreviousPage();

        } else {
            // Hide loading indicator
            CommonTools.HideLoading();

            // Show error message
            await Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to delete person. Please try again later.'
            });
        }
    }
}

/**
 * Deletes a person in the VedAstro API.
 * @param {string} personId - The ID of the person to delete.
 * @returns {Promise<boolean>} - True if delete is successful, false otherwise.
 */
async function DeletePersonViaApi(personId) {
    // Construct API URL
    const apiUrl = [
        `${VedAstro.ApiDomain}/Calculate/DeletePerson/`,
        `OwnerId/${VedAstro.IsGuestUser() ? VedAstro.VisitorId : VedAstro.UserId}`,
        `/PersonId/${personId}`
    ].join('');

    // Make the API call to delete the person
    try {
        await CommonTools.GetAPIPayload(apiUrl);
        // Assuming API returns a success status
        return true;
    } catch (error) {
        console.error('Error deleting person:', error);
        return false;
    }

}

/**
 * Updates a person in the VedAstro API.
 * @param {Person} person - The person object to update.
 * @returns {Promise<boolean>} - True if update is successful, false otherwise.
 */
async function UpdatePersonViaApi(person) {
    // Convert birth time to URL format
    var timeUrl = person.BirthTime.ToUrl();

    // Construct API URL
    const apiUrl = [
        `${VedAstro.ApiDomain}/Calculate/UpdatePerson/`,
        `OwnerId/${VedAstro.IsGuestUser() ? VedAstro.VisitorId : VedAstro.UserId}`,
        `/PersonId/${person.PersonId}`,
        `/${timeUrl}`,
        `PersonName/${person.Name}`,
        `/Gender/${person.Gender}`,
        `/Notes/${CommonTools.toUrlSafe(person.Notes)}`
    ].join('');

    // Make the API call to update the person
    try {
        await CommonTools.GetAPIPayload(apiUrl);
        // Assuming API returns a success status
        return true;
    } catch (error) {
        console.error('Error updating person:', error);
        return false;
    }
}

/**
 * Collects input from fields and creates a Person instance.
 * @returns {Promise<Person>} - The person instance.
 */
async function getPersonInstanceFromInput() {
    const nameInput = document.getElementById("NameInput");
    const genderInput = document.getElementById("GenderInput");
    const notesInput = document.getElementById("NotesInput");

    const person = new Person({
        PersonId: "", // Will be set later
        Name: nameInput.value,
        Notes: notesInput.value,
        BirthTime: await timeLocationInput.getTimeJson(),
        Gender: genderInput.value,
        OwnerId: "", // Will be set later
        LifeEventList: originalPerson.LifeEventList || []
    });

    return person;
}

/**
 * Validates the input fields.
 * @returns {Promise<boolean>} - True if validation passes.
 */
async function isValidationPassed() {
    const nameInput = document.getElementById("NameInput");
    const genderInput = document.getElementById("GenderInput");

    // Name validation
    if (nameInput.value.trim() === "") {
        await Swal.fire({
            icon: 'error',
            title: 'Name is Required',
            text: 'Please enter a valid name. This will help you identify the correct person later.'
        });
        return false;
    }

    // Gender validation
    if (genderInput.value.trim() === "") {
        await Swal.fire({
            icon: 'error',
            title: 'Gender is Required',
            text: 'Please select a valid gender. Necessary for accurate calculations and predictions.'
        });
        return false;
    }

    // Time & Location validation
    const isValidTime = await timeLocationInput.isValid();
    if (!isValidTime) {
        await Swal.fire({
            icon: 'error',
            title: 'Check Location',
            text: 'Location name is missing or invalid!'
        });
        return false;
    }

    // Additional validations can be added here

    return true;
}
