

updateAge();

function updateAge() {
    // Get the current date and time
    const currentDate = new Date();

    // Set the hard-coded month and year
    const pastDate = new Date(2014, 8, 1); // September 2014

    // Calculate the difference in milliseconds
    const differenceInMilliseconds = currentDate.getTime() - pastDate.getTime();

    // Convert the difference to years
    const millisecondsInYear = 31536000000; // 1000 * 60 * 60 * 24 * 365
    const yearsDifference = differenceInMilliseconds / millisecondsInYear;

    // Format the years difference to two decimal places
    const formattedYearsDifference = yearsDifference.toFixed(2);

    // Get the div element to display the result
    const resultDiv = document.getElementById('ageOutputHeader');

    // Display the result in the div
    resultDiv.innerText = `${formattedYearsDifference} Years Old`;

}