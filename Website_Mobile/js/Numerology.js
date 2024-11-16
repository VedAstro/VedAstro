updateHistory();


new PageHeader("PageHeader");


let nameInputChangeTimeout = null;

//every time the user types something, it clears the existing timeout and sets a
//new timeout to make the API call after a 500ms delay.If the user types something 
//else before the 500ms delay is over, it clears the existing timeout and sets a new one,
//effectively delaying the API call until the user has finished typing.
async function onNameNumberInputChange(inputElement) {

    // Get text from input
    let nameText = inputElement.value.trim();

    // Don't make API calls for empty input & hide output from previous
    if (!nameText) {
        // hide the output holder
        $('#OutputHolder').hide();

        return;
    }; 

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

        // Show the output holder
        $('#OutputHolder').show();
        // hide loading icon
        $('#InPageLoadingIcon').hide();

    }, 500);
}
