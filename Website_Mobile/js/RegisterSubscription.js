// RegisterSubscription.js

updateHistory(); //so login page can redirect back here

//STEP 1:
// if not logged in tell user that login is required,
// and auto direct to login page
if (VedAstro.IsGuestUser()) {
    Swal.fire({
        icon: 'info',
        title: 'Please login',
        text: 'You will be redirected to the login page...',
        timer: 3000,
        showConfirmButton: false
    }).then(() => {
        // Redirect to login page after the delay
        window.location.href = './Login.html';
    });

}
else {
    // Proceed to Step 2 and Step 3

    // STEP 2:
    // Check if URL contains API Key (as param)
    const urlParams = new URLSearchParams(window.location.search);
    let apiKey = urlParams.get('APIKey');

    // If not, then generate new API key
    if (!apiKey) {
        apiKey = generateApiKey();
    }

    // STEP 3:
    // Register API key in server
    const registerUrl = `${VedAstro.ApiDomain}/RegisterSubscription/OwnerId/${VedAstro.UserId}/APIKey/${apiKey}`;

    fetch(registerUrl)
        .then(response => response.json())
        .then(data => {
            if (data.Status === 'Pass') {
                // thank user & say registration complete
                Swal.fire({
                    icon: 'success',
                    title: 'Thank You 🙏',
                    text: 'Your API key is now ready, test it in API Builder',
                    confirmButtonText: 'OK'
                }).then(() => {
                    // Redirect user to ./APIBuilder.html
                    window.location.href = './APIBuilder.html';
                });
            } else {
                // tell user registration failed
                Swal.fire({
                    icon: 'error',
                    title: 'Registration Failed',
                    text: 'There was an issue registering your API key. Please contact us.',
                    confirmButtonText: 'OK'
                }).then(() => {
                    // Redirect user to ./APIBuilder.html
                    window.location.href = './APIBuilder.html';
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            // tell user registration failed
            Swal.fire({
                icon: 'error',
                title: 'Registration Failed',
                text: 'There was an issue registering your API key. Please contact us.',
                confirmButtonText: 'OK'
            }).then(() => {
                // Redirect user to ./APIBuilder.html
                window.location.href = './APIBuilder.html';
            });
        });
}


function generateApiKey(length = 10) {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let apiKey = '';
    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * characters.length);
        apiKey += characters[randomIndex];
    }
    return apiKey;
}