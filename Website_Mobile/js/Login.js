

new PageTopNavbar("PageTopNavbar");
new DesktopSidebar("DesktopSidebarHolder");


//send JWT token to API server for validation
//will return user id and name only
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
        Swal.fire({ icon: 'success', title: 'Login Success ✅', timer: 1500, showConfirmButton: false });

        // wait a little and send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
        setTimeout(() => { navigateToPreviousPage(); }, 1500);

        console.log("Google Login Success ✅");

    } catch (error) {
        console.error(error);
    }

}

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
        Swal.fire({ icon: 'success', title: 'Login Success ✅', timer: 1500, showConfirmButton: false });

        // wait a little and send user back to previous page (reloaded & not via "Back" functionality to avoid caching)
        setTimeout(() => { navigateToPreviousPage(); }, 1500);

        console.log("Facebook Login Success ✅");

    } catch (error) {
        console.error(error);
    }

}