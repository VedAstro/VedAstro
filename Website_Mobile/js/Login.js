
new PageTopNavbar("PageTopNavbar");
new DesktopSidebar("DesktopSidebarHolder");

//fill login helper text, to make user remember previous login method
fillLoginHelperText();



//based on previous login method, remind user, else show message to encourage users to login
//name of element holding message is "LoginHelperTextHolder"
function fillLoginHelperText() {
    //see if previous login method exist
    const previousLoginMethod = localStorage.getItem('PreviousLoginMethod');

    if (previousLoginMethod) {
        // Display a message reminding the user of their previous login method
        $('#LoginHelperTextHolder').text(`On your last visit, you used <strong>"${previousLoginMethod}"</strong>`);
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
//will return user id and name only
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
//will return user id and name only
//will only be called on successful login by Facebook
async function OnFacebookSignInSuccessHandler(afterLoginResponse) {

    try {
        const jwtToken = afterLoginResponse.authResponse;
        const url = `${VedAstro.ApiDomain}/SignInFacebook/Token/${jwtToken}`;
        const response = await fetch(url, { method: 'GET', headers: { 'Content-Type': 'application/json' } });

        //extract reply, contains user ID & name only
        const userData = await response.json();

        //save to local storage for access by other components later
        VedAstro.UserId = userData.Payload.Id;
        VedAstro.UserName = userData.Payload.Name;

        //make note login method, to help user remember on return 🧠
        localStorage.setItem('PreviousLoginMethod', 'Facebook');

        //tell user login was success
        await Swal.fire({ icon: 'success', title: 'Login Success ✅', timer: 1500, showConfirmButton: false });

        // wait a little and send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
        setTimeout(() => { navigateToPreviousPage(); }, 1500);

        console.log("Facebook Login Success ✅");

    } catch (error) {
        console.error(error);
    }

}