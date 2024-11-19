updateHistory();


new PageHeader("PageHeader");

//initialize help text
new HelpTextIcon(`#NameTextHelp`);


let nameInputChangeTimeout = null;

//every time the user types something, it clears the existing timeout and sets a
//new timeout to make the API call after a 500ms delay.If the user types something 
//else before the 500ms delay is over, it clears the existing timeout and sets a new one,
//effectively delaying the API call until the user has finished typing.
async function onNameNumberInputChange(inputElement) {

    // Get text from input
    let nameText = inputElement.value.trim();

    // Don't make API calls for empty input & hide output from previous
    if (!nameText) { $('#OutputHolder').hide(); return; };

    // Clear existing timeout
    clearTimeout(nameInputChangeTimeout);

    // Set new timeout to make API call after 500ms delay
    nameInputChangeTimeout = setTimeout(async () => {
        // show in page loading icon let user know something is happening
        $('#InPageLoadingIcon').show();
        // hide the output holder
        $('#OutputHolder').hide();

        // Construct API URLs
        const namePredictionAPI = `${VedAstro.ApiDomain}/Calculate/NameNumberPrediction/FullName/${nameText}`;
        const nameTotalNumberAPI = `${VedAstro.ApiDomain}/Calculate/NameNumber/FullName/${nameText}`;

        // Make API calls
        const [predictionResponse, totalNumberResponse] = await Promise.all([
            fetch(namePredictionAPI).then(response => response.json()),
            fetch(nameTotalNumberAPI).then(response => response.json())
        ]);

        // Extract data from responses
        const prediction = predictionResponse.Payload.NameNumberPrediction;
        const totalNumber = totalNumberResponse.Payload.NameNumber;

        // Inject data into HTML
        $('#NameNumberPredictionHolder').text(prediction);
        $('#NameTotalNumberHolder').text(totalNumber);

        // sometimes, text is cleared but API just finish calculating,
        // so double check if there is no text any more than hide now (happens for last character)
        let currentNameText = inputElement.value.trim();
        if (!currentNameText) {
            $('#OutputHolder').hide();// hide the output holder
            $('#InPageLoadingIcon').hide();// hide loading icon
            return;
        };

        // Show the output holder
        $('#OutputHolder').show();
        // hide loading icon
        $('#InPageLoadingIcon').hide();

    }, 500);
}
