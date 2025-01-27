updateHistory();

new PageHeader("PageHeader");

// Initialize help text
new HelpTextIcon(`#NameTextHelp`);

let nameInputChangeTimeout = null;

// Function to calculate color based on percentage
function getColorFromScore(score) {
    const green = Math.round((score / 100) * 255);
    const red = 255 - green;
    return `rgb(${red}, ${green}, 0)`;
}

// Every time the user types something, it clears the existing timeout and sets a
// new timeout to make the API call after a 700ms delay. If the user types something 
// else before the 700ms delay is over, it clears the existing timeout and sets a new one,
// effectively delaying the API call until the user has finished typing.
async function onNameNumberInputChange(inputElement) {

    // Get text from input
    let nameText = inputElement.value.trim();

    // Don't make API calls for empty input & hide output from previous
    if (!nameText) { $('#OutputHolder').hide(); return; };

    // Clear existing timeout
    clearTimeout(nameInputChangeTimeout);

    // Set new timeout to make API call after 700ms delay
    nameInputChangeTimeout = setTimeout(async () => {
        // Show in-page loading icon to let user know something is happening
        $('#InPageLoadingIcon').show();
        // Hide the output holder
        $('#OutputHolder').hide();

        // Construct API URLs
        const namePredictionAPI = `${VedAstro.ApiDomain}/Calculate/NameNumberPrediction/FullName/${nameText}`;

        // Make API calls
        const [predictionResponse] = await Promise.all([
            fetch(namePredictionAPI).then(response => response.json()),
        ]);

        // Extract data from responses
        const prediction = predictionResponse.Payload.NameNumberPrediction.Prediction;
        const totalNumber = predictionResponse.Payload.NameNumberPrediction.Number;
        const planet = predictionResponse.Payload.NameNumberPrediction.Planet;
        const predictionSummary = predictionResponse.Payload.NameNumberPrediction.PredictionSummary;

        // Inject data into HTML
        $('#NameNumberPredictionHolder').text(prediction);
        $('#NameTotalNumberHolder').text(totalNumber);

        // Check if planet is "Empty" and prediction summary is empty
        if (planet === "Empty" && Object.keys(predictionSummary).length === 0) {
            // Show the output holder without processing the summary and planet
            $('#NameTotalPlanetHolder').empty();
            $('#PredictionSummary').empty();
            $('#OutputHolder').show();
            $('#InPageLoadingIcon').hide();
            return;
        }

        $('#NameTotalPlanetHolder').text(planet);

        // Update prediction summary
        $('#PredictionSummary').html(`
            <!-- Finance -->
            <div class="vstack">
                <div class="hstack d-flex justify-content-center">
                    <iconify-icon icon="mdi:currency-usd" width="32" height="32"></iconify-icon>
                    <div style="font-size: 32px; color: ${getColorFromScore(predictionSummary.Finance)};">${predictionSummary.Finance}%</div>
                </div>
                <div class="d-flex justify-content-center">Finance</div>
            </div>

            <!-- Romance -->
            <div class="vstack">
                <div class="hstack d-flex justify-content-center">
                    <iconify-icon icon="mdi:heart" width="32" height="32"></iconify-icon>
                    <div style="font-size: 32px; color: ${getColorFromScore(predictionSummary.Romance)};">${predictionSummary.Romance}%</div>
                </div>
                <div class="d-flex justify-content-center">Romance</div>
            </div>

            <!-- Education -->
            <div class="vstack">
                <div class="hstack d-flex justify-content-center">
                    <iconify-icon icon="mdi:school" width="32" height="32"></iconify-icon>
                    <div style="font-size: 32px; color: ${getColorFromScore(predictionSummary.Education)};">${predictionSummary.Education}%</div>
                </div>
                <div class="d-flex justify-content-center">Education</div>
            </div>

            <!-- Health -->
            <div class="vstack">
                <div class="hstack d-flex justify-content-center">
                    <iconify-icon icon="mdi:medical-bag" width="32" height="32"></iconify-icon>
                    <div style="font-size: 32px; color: ${getColorFromScore(predictionSummary.Health)};">${predictionSummary.Health}%</div>
                </div>
                <div class="d-flex justify-content-center">Health</div>
            </div>
        `);

        // Sometimes, text is cleared but API just finishes calculating,
        // so double-check if there is no text anymore then hide now (happens for last character)
        let currentNameText = inputElement.value.trim();
        if (!currentNameText) {
            $('#OutputHolder').hide(); // Hide the output holder
            $('#InPageLoadingIcon').hide(); // Hide loading icon
            return;
        };

        // Show the output holder
        $('#OutputHolder').show();
        // Hide loading icon
        $('#InPageLoadingIcon').hide();

    }, 700);
}
