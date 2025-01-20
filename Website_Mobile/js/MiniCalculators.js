
new PageHeader("PageHeader");

async function calculateLmtToStd() {
    //get data inputed by user
    let lmtString = document.getElementById("localMeanTimeString").value; //example output : "19:23 08/08/1912"
    let lmtLongitude = document.getElementById("lmtLongitude").value;
    let outputStdOffset = document.getElementById("outputStdOffset").value;

    // Extract date and time from lmtString
    const [time, date] = lmtString.split(' ');
    const [hour, minute] = time.split(':');
    const [day, month, year] = date.split('/');

    // Construct API URL
    const apiUrl = `${VedAstro.ApiDomain}/Calculate/LmtToStd/Time/${hour}:${minute}/${day}/${month}/${year}/Longitude/${lmtLongitude}/STDOffset/${outputStdOffset}`;

    // Make API call and handle response
    const response = await fetch(apiUrl);

    const data = await response.json();
    if (data.Status === "Pass") {
        document.getElementById("lmtToStdTimeOutput").innerHTML = data.Payload.LmtToStd;
        $("#lmtToStdTimeOutput").parent().parent().parent().show(); //make holder visible
    } 
}

async function calculatelongToLmtOffset() {
    //get data inputed by user
    let lmtLongitude = document.getElementById("longToLmtOffsetLongitude").value;

    // Construct API URL
    const apiUrl = `${VedAstro.ApiDomain}/Calculate/LongitudeToLMTOffset/Longitude/${lmtLongitude}`;

    // Make API call and handle response
    const response = await fetch(apiUrl);

    const data = await response.json();
    if (data.Status === "Pass") {
        document.getElementById("longToLmtOffsetOutput").innerHTML = data.Payload.LongitudeToLMTOffset;
        $("#longToLmtOffsetOutput").parent().parent().parent().show(); //make holder visible
    } 

}

async function calculateCoordinatesToGeoLocation() {
    // Get data inputed by user
    let latitude = document.getElementById("coordinatesLatitude").value;
    let longitude = document.getElementById("coordinatesLongitude").value;

    // Construct API URL
    const apiUrl = `${VedAstro.ApiDomain}/Calculate/CoordinatesToGeoLocation/Latitude/${latitude}/Longitude/${longitude}`;

    // Make API call and handle response
    const response = await fetch(apiUrl);
    const data = await response.json();
    if (data.Status === "Pass") {
        document.getElementById("coordinatesToGeoLocationOutput").innerHTML = data.Payload.CoordinatesToGeoLocation.Name;
        $("#coordinatesToGeoLocationOutput").parent().parent().parent().show(); // Make holder visible
    }
}

async function calculateGeoLocationToTimezone() {
    //get data inputed by user
    let location = document.getElementById("geoLocationToTimezoneLocation").value;
    let latitude = document.getElementById("geoLocationToTimezoneLatitude").value;
    let longitude = document.getElementById("geoLocationToTimezoneLongitude").value;
    let time = document.getElementById("geoLocationToTimezoneTime").value;
    let date = document.getElementById("geoLocationToTimezoneDate").value;

    // Extract day, month and year from date
    const [day, month, year] = date.split('/');

    // Construct API URL
    const apiUrl = `${VedAstro.ApiDomain}/Calculate/GeoLocationToTimezone/Location/${location}/Coordinates/${latitude},${longitude}/Time/${time}/${day}/${month}/${year}/+00:00`;

    // Make API call and handle response
    const response = await fetch(apiUrl);

    const data = await response.json();
    if (data.Status === "Pass") {
        document.getElementById("geoLocationToTimezoneOutput").innerHTML = data.Payload.GeoLocationToTimezone;
        $("#geoLocationToTimezoneOutput").parent().parent().parent().show(); //make holder visible
    }
}

let searchTimeout = null;

function searchLocation() {
    const locationInput = document.getElementById("locationSearchInput").value;
    if (locationInput.length < 3) {
        // do not search for very short inputs
        return;
    }
    if (searchTimeout) {
        clearTimeout(searchTimeout);
    }
    searchTimeout = setTimeout(async () => {
        const apiUrl = `${VedAstro.ApiDomain}/Calculate/SearchLocation/Address/${locationInput}`;
        const response = await fetch(apiUrl);
        const data = await response.json();
        if (data.Status === "Pass") {
            const locationList = document.getElementById("locationSearchList");
            locationList.innerHTML = "";
            data.Payload.SearchLocation.forEach(location => {
                const listItem = document.createElement("li");
                listItem.textContent = `${location.Name} (${location.Latitude}, ${location.Longitude})`;
                locationList.appendChild(listItem);
            });
            document.getElementById("locationSearchResults").style.display = "block";
        }
    }, 500);
}


