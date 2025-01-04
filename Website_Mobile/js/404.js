// 404.js

(function () {
    // Get the current URL
    var url = window.location.href;

    // Create a URL object to parse the URL
    var urlObj = new URL(url);

    // Get the pathname from the URL object
    var path = urlObj.pathname;

    // Check if the path does not end with ".html" and we haven't already redirected
    if (!path.endsWith('.html') && !urlObj.searchParams.has('redirected')) {
        // Remove any trailing slash from the path
        if (path.endsWith('/')) {
            path = path.slice(0, -1);
        }

        // Append '.html' to the path
        var newPath = path + '.html';

        // Create a new URL object for the redirect
        var redirectUrl = new URL(url);
        redirectUrl.pathname = newPath;

        // Add a search parameter to indicate we've redirected
        redirectUrl.searchParams.set('redirected', 'true');

        // Redirect to the new URL
        window.location.replace(redirectUrl.toString());
    } else {
        // Do not redirect; prevent infinite loop
        console.warn('Redirect to .html version unsuccessful; avoiding infinite redirect loop.');
    }
})();