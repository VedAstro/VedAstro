

new InfoBox("InfoBox_Privacy_Login");
new InfoBox("InfoBox_Storage_Login");
new InfoBox("InfoBox_Secure_Login");

//fill login helper text, to make user remember previous login method
fillLoginHelperText();

//due to some possibilities, the user can be in Login page even after login
//so check every 20s and redirect user to Home page once logged in
const intervalId = setInterval(() => {
    // Check if the user is already logged in
    if (!VedAstro.IsGuestUser()) {
        // Clear the interval to prevent further checks
        clearInterval(intervalId);
        // Redirect the user to Home.html
        window.location.href = './Home.html';
    }
}, 20000);

//called by custom FB login button on page
function onClickFacebookLoginButton() {
    FB.login(OnFacebookSignInHandler, { scope: 'email' });
}

//based on previous login method, remind user, else show message to encourage users to login
//name of element holding message is "LoginHelperTextHolder"
function fillLoginHelperText() {
    //see if previous login method exist
    const previousLoginMethod = localStorage.getItem('PreviousLoginMethod');

    if (previousLoginMethod) {
        // Display a message reminding the user of their previous login method
        $('#LoginHelperTextHolder').html(`On your last visit, you used <strong>"${previousLoginMethod}"</strong>`);
    } else {
        // Display a random funny text encouraging the user to log in
        const funnyTexts = [
            'To prove you\'re a human from Earth 🌍',
            'Don\'t be shy, log in and join the party! 🥳',
            'To prove you\'re not a robot 🤖',
            'To prove you\'re not from the Machine World 🤖',
            'To authenticate yourself as a biological entity 🧬'
        ];
        const randomIndex = Math.floor(Math.random() * funnyTexts.length);
        $('#LoginHelperTextHolder').text(funnyTexts[randomIndex]);

    }
}

//sends JWT token to API server for validation
//will save user id and name to storage
//will only be called on successful login by Google
async function OnGoogleSignInSuccessHandler(afterLoginResponse) {

    try {
        const jwtToken = afterLoginResponse.credential;
        const url = `${VedAstro.ApiDomain}/SignInGoogle/Token/${jwtToken}`;
        const response = await fetch(url, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        //extract reply, contains user ID & name only
        const userData = await response.json();

        //save to local storage for access by other components later
        VedAstro.UserId = userData.Payload.Id;
        VedAstro.UserName = userData.Payload.Name;

        //make note login method, to help user remember on return 🧠
        localStorage.setItem('PreviousLoginMethod', 'Google');

        //clear cached person list (will cause person drop down to fetch new)
        PersonSelectorBox.ClearPersonListCache('private');

        //tell user login was success
        await Swal.fire({ icon: 'success', title: 'Login Success ✅', timer: 1500, showConfirmButton: false });

        // wait a little and send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
        setTimeout(() => { navigateToPreviousPage(); }, 1500);

        console.log("Google Login Success ✅");

    } catch (error) {
        console.error(error);
    }

}

//sends access token to API server for validation
//will save user id and name to storage
//will be called on successful AND failed login by Facebook.js
//NOTE : has to be non-async for FB to work
function OnFacebookSignInHandler(afterLoginResponse) {

    //if login fail then end here! (will be null)
    if (afterLoginResponse.authResponse == null) { return; }

    try {
        const accessToken = afterLoginResponse.authResponse.accessToken;
        const url = `${VedAstro.ApiDomain}/SignInFacebook/Token/${accessToken}`;

        fetch(url, { method: 'GET', headers: { 'Content-Type': 'application/json' } })
            .then(response => response.json())
            .then(userData => {
                //save to local storage for access by other components later
                VedAstro.UserId = userData.Payload.Id;
                VedAstro.UserName = userData.Payload.Name;

                //make note login method, to help user remember on return 
                localStorage.setItem('PreviousLoginMethod', 'Facebook');

                //clear cached person list (will cause person drop down to fetch new)
                PersonSelectorBox.ClearPersonListCache('private');

                //tell user login was success
                Swal.fire({ icon: 'success', title: 'Login Success ', timer: 1500, showConfirmButton: false })
                    .then(() => {
                        // wait a little and send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
                        navigateToPreviousPage();
                    });
            })
            .catch(error => console.error(error));

        console.log("Facebook Login Success ");

    } catch (error) {
        console.error(error);
    }

}
