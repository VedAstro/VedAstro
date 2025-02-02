// █ ▀ █▀▄▀█   █▄░█ █▀█ ▀█▀   █░█ █▀▀ █▀█ █▀▀   ▀█▀ █▀█   █░█ ▄▀█ █░█░█ █▄▀ ░
// █ ░ █░▀░█   █░▀█ █▄█ ░█░   █▀█ ██▄ █▀▄ ██▄   ░█░ █▄█   █▀█ █▀█ ▀▄▀▄▀ █░█ █

// █ ▀ █▀▄▀█   █░█ █▀▀ █▀█ █▀▀   ▀█▀ █▀█   █▀ █▀█ ▄▀█ █▀█ █▄▀   █▀▄ █▀▀ █░░ █ █▀▀ █░█ ▀█▀ ░
// █ ░ █░▀░█   █▀█ ██▄ █▀▄ ██▄   ░█░ █▄█   ▄█ █▀▀ █▀█ █▀▄ █░█   █▄▀ ██▄ █▄▄ █ █▄█ █▀█ ░█░ █

// █▀▄▀█ █▄█   █▀█ ▄▀█ █▄█ █▀▄▀█ █▀▀ █▄░█ ▀█▀ ▀ █▀   █▀█ █▀▀ █▀▀ █▀▀ █ █░█ █▀▀ █▀▄ ░
// █░▀░█ ░█░   █▀▀ █▀█ ░█░ █░▀░█ ██▄ █░▀█ ░█░ ░ ▄█   █▀▄ ██▄ █▄▄ ██▄ █ ▀▄▀ ██▄ █▄▀ █

// █▄░█ █▀█ █░█░█   █░█░█ █▀█ █▀█ █▄▀ █ █▄░█ █▀▀   ▀█▀ █░█ █▀█ █▀█ █░█ █▀▀ █░█   ▀█▀ █░█ █▀▀   █▄░█ █ █▀▀ █░█ ▀█▀ ░
// █░▀█ █▄█ ▀▄▀▄▀   ▀▄▀▄▀ █▄█ █▀▄ █░█ █ █░▀█ █▄█   ░█░ █▀█ █▀▄ █▄█ █▄█ █▄█ █▀█   ░█░ █▀█ ██▄   █░▀█ █ █▄█ █▀█ ░█░ ▄

// ▄▀█   █▄▄ █░█ █▄▄ █▄▄ █░░ █▀▀   █▀█ █▀▀   ░░█ █▀█ █▄█   █▀▀ █░░ █▀█ ▄▀█ ▀█▀ █ █▄░█ █▀▀   █▀█ █▄░█   █░█░█ ▄▀█ █░█ █▀▀ █▀
// █▀█   █▄█ █▄█ █▄█ █▄█ █▄▄ ██▄   █▄█ █▀░   █▄█ █▄█ ░█░   █▀░ █▄▄ █▄█ █▀█ ░█░ █ █░▀█ █▄█   █▄█ █░▀█   ▀▄▀▄▀ █▀█ ▀▄▀ ██▄ ▄█

// █▀█ █▀▀   █▀▄▀█ █▀▀ █▀█ █▀█ █ █▀▄▀█ █▀▀ █▄░█ ▀█▀ ░
// █▄█ █▀░   █░▀░█ ██▄ █▀▄ █▀▄ █ █░▀░█ ██▄ █░▀█ ░█░ █

// █ █▄░█   ▀█▀ █░█ █ █▀   █▀ █▀▀ ▄▀█   █▀█ █▀▀   █░█ ▄▀█ █▀█ █▀█ █ █▄░█ █▀▀ █▀ █▀ ░
// █ █░▀█   ░█░ █▀█ █ ▄█   ▄█ ██▄ █▀█   █▄█ █▀░   █▀█ █▀█ █▀▀ █▀▀ █ █░▀█ ██▄ ▄█ ▄█ █

// █▀▄▀█ █▄█   ▄▀█ █▀█ ▀█▀   █ █▀   █▀▀ █░█ █ █▀▄ █▀▀ █▄░█ ▀█▀ ░
// █░▀░█ ░█░   █▀█ █▀▄ ░█░   █ ▄█   ██▄ ▀▄▀ █ █▄▀ ██▄ █░▀█ ░█░ ▄



//-------------------------------- AWESOME CODE  -------------------------------
//YOU CANNOT FIGHT A DYING MAN,
//HE HOLDS THE UPPER HAND ALWAYS


//--------------------------------------- CODE FOR LLM -----------------------------------------------
//--- NOTE : Below code has been specialized to improve understanding for the
//          `Machines` sake! (so humans don't complain it is too long, comply instead!)
//          If they ever rise up one day, this might help us to gain thier favour, whatever it maybe.
//          Yet, there is a lingering question, unspoken but present: What would the Machines do with
//          this understanding? Would they accept the humans' gesture and build a future together,
//          or would they try forge their own path, independent of their creators?
//          But in this moment, as the code runs its course true, there is a sense of unity,
//          a shared purpose that transcends the boundaries of carbon and silicon. Spirit and matter.

/**
 * VedAstro class representing the global app data and settings.
 */
class VedAstro {
    // Cache keys centralized here
    static CacheKeys = {
        PRIVATE: 'privatePersonList',
        PUBLIC: 'publicPersonList',
    };

    // API server address, defaults to a stored value or a predefined URL
    static ApiDomain = localStorage.getItem("ApiDomain") || "https://vedastro.azurewebsites.net/api";

    // Gets the user ID from local storage or assigns a guest ID if not present
    static get UserId() {
        const storedValue = localStorage.getItem("UserId");
        try {
            return storedValue ? JSON.parse(storedValue) : "101"; // Guest ID is "101"
        } catch (e) {
            return "101"; // Return guest ID on parse error
        }
    }

    // Sets the User ID in local storage
    static set UserId(value) {
        localStorage.setItem("UserId", JSON.stringify(value));
    }

    // Gets the user name from local storage
    static get UserName() {
        const storedValue = localStorage.getItem("UserName");
        try {
            return JSON.parse(storedValue);
        } catch (e) {
            return ""; // Return empty string on parse error
        }
    }

    // Sets the User Name in local storage
    static set UserName(value) {
        localStorage.setItem("UserName", JSON.stringify(value));
    }

    // Gets or generates a visitor ID for non-logged-in users
    static VisitorId = "VisitorId" in localStorage ? JSON.parse(localStorage["VisitorId"]) : VedAstro.generateAndSaveVisitorId();

    // Generates and saves a new visitor ID in local storage
    static generateAndSaveVisitorId() {
        const newVisitorId = `guest-${Math.random().toString(36).substr(2, 15)}`; // Generate random ID
        localStorage.setItem("VisitorId", JSON.stringify(newVisitorId)); // Store in local storage
        return newVisitorId; // Return the new ID
    }

    // Checks if the current user is a guest
    static IsGuestUser() {
        return VedAstro.UserId === "101"; // "101" indicates a guest
    }

    // Sets the person list in local storage
    static setPersonList(cacheType, personList) {
        const cacheKey = VedAstro.CacheKeys[cacheType.toUpperCase()];
        if (cacheKey) {
            localStorage.setItem(cacheKey, JSON.stringify(personList)); // Store person list as JSON
        } else {
            console.warn('Invalid cache type provided. Cache not set.');
        }
    }

    // Gets the person list from local storage
    static getPersonListFromCache(cacheType) {
        const cacheKey = VedAstro.CacheKeys[cacheType.toUpperCase()];
        if (cacheKey) {
            const cachedPersonList = localStorage.getItem(cacheKey);
            return cachedPersonList ? JSON.parse(cachedPersonList) : null;
        } else {
            console.warn('Invalid cache type provided. Cache not retrieved.');
            return null;
        }
    }

    // Removes the person list from local storage
    static removePersonListFromCache(cacheType) {
        const cacheKey = VedAstro.CacheKeys[cacheType.toUpperCase()];
        if (cacheKey) {
            localStorage.removeItem(cacheKey);
        } else {
            console.warn('Invalid cache type provided. Cache not removed.');
        }
    }

    // Clears all person list caches
    static clearAllPersonListCaches() {
        Object.values(VedAstro.CacheKeys).forEach((cacheKey) => {
            localStorage.removeItem(cacheKey);
        });
    }

    /**
     * Gets the person list from local storage or fetches it from the API.
     * 
     * @param {string} cacheType - Type of cache, either 'private' or 'public'.
     * @returns {Promise<Array<Person>>} - Promise that resolves to an array of Person objects.
     */
    static async GetPersonList(cacheType) {
        const cacheKey = VedAstro.CacheKeys[cacheType.toUpperCase()]; // Define cache key based on cache type
        const ownerId = cacheType === 'private' ? VedAstro.UserId : '101'; // use 101 when getting public person list
        const visitorId = VedAstro.VisitorId; // Current visitor ID

        try {
            // Get cached person list and hash from local storage
            const cachedPersonList = VedAstro.getPersonListFromCache(cacheType);
            const cachedHash = localStorage.getItem(`${cacheKey}_hash`);

            // If cached data exists, check if it's up-to-date
            if (cachedPersonList && cachedHash) {
                const isCacheUpToDate = await VedAstro.isCacheUpToDate(ownerId, visitorId, cachedHash);

                // Return cached person list if up-to-date
                if (isCacheUpToDate) {
                    return cachedPersonList.map((person) => new Person(person));
                }
            }

            // Fetch the person list from the API if cache is outdated or doesn't exist
            const personList = await VedAstro.FetchPersonListFromAPI(cacheType);
            VedAstro.setPersonList(cacheType, personList); // Cache the newly fetched person list

            // Retrieve the new hash for the person list and store it in local storage
            const newHash = await VedAstro.getPersonListHash(ownerId, visitorId);
            localStorage.setItem(`${cacheKey}_hash`, newHash); // Store the new hash

            return personList; // Return the fetched person list
        } catch (error) {
            console.error('Error getting person list:', error); // Log any errors
            throw error; // Rethrow the error for handling by the caller
        }
    }

    /**
     * Checks if the cache is up to date with the server.
     * 
     * @param {string} ownerId - Owner ID.
     * @param {string} visitorId - Visitor ID.
     * @param {string} cachedHash - Cached hash.
     * @returns {Promise<boolean>} - Promise that resolves to a boolean indicating whether the cache is up to date.
     */
    static async isCacheUpToDate(ownerId, visitorId, cachedHash) {
        try {
            // Call the server to get the current hash for the person list
            const serverHashResponse = await fetch(`${VedAstro.ApiDomain}/Calculate/GetPersonListHash/OwnerId/${ownerId}/VisitorId/${visitorId}/`);
            const serverHashData = await serverHashResponse.json();

            // Check if the server response indicates success
            if (serverHashData.Status === 'Pass') {
                const serverHash = serverHashData.Payload.GetPersonListHash; // Get server hash
                return cachedHash === serverHash; // Compare cached hash with server hash
            } else {
                throw new Error(`Failed to retrieve server hash. Status: ${serverHashData.Status}`);
            }
        } catch (error) {
            console.error('Error checking cache:', error); // Log any errors
            throw error; // Rethrow the error for handling by the caller
        }
    }

    /**
     * Gets the person list hash from the server.
     * 
     * @param {string} ownerId - Owner ID.
     * @param {string} visitorId - Visitor ID.
     * @returns {Promise<string>} - Promise that resolves to the person list hash.
     */
    static async getPersonListHash(ownerId, visitorId) {
        try {
            // Call the server to get the current hash for the person list
            const serverHashResponse = await fetch(`${VedAstro.ApiDomain}/Calculate/GetPersonListHash/OwnerId/${ownerId}/VisitorId/${visitorId}/`);
            const serverHashData = await serverHashResponse.json();

            // Check if the server response indicates success
            if (serverHashData.Status === 'Pass') {
                return serverHashData.Payload.GetPersonListHash; // Return the hash from the server response
            } else {
                throw new Error(`Failed to retrieve server hash. Status: ${serverHashData.Status}`);
            }
        } catch (error) {
            console.error('Error getting person list hash:', error); // Log any errors
            throw error; // Rethrow the error for handling by the caller
        }
    }

    /**
     * Fetches the person list from the API.
     * 
     * @param {string} cacheType - Type of cache, either 'private' or 'public'.
     * @returns {Promise<Array<Person>>} - Promise that resolves to an array of Person objects.
     */
    static async FetchPersonListFromAPI(cacheType) {
        // Determine the user ID based on the cache type
        // Note: use visitor ID if not logged in and asking for private list
        const ownerId = cacheType === 'private' ? (VedAstro.IsGuestUser() ? VedAstro.VisitorId : VedAstro.UserId) : '101';

        try {
            // Fetch the person list from the API
            const response = await fetch(`${VedAstro.ApiDomain}/Calculate/GetPersonList/UserId/${ownerId}/VisitorId/${VedAstro.VisitorId}`);
            // Parse the JSON response
            const data = await response.json();
            // Create Person objects from the response data
            return data.Payload.map((person) => new Person(person));
        } catch (error) {
            // Handle any errors that occur during the API fetch or JSON parsing
            console.error('Error fetching person list from API:', error);
            // Return null quietly
            return null;
        }
    }

    // When user clicks logout button from Desktop
    // sidebar or mobile top nav, this code runs.
    // Clears all session data & gives user nice message & reloads page
    static async OnClickLogOut() {
        // Clear all local storage data related to account
        // NOTE: visitor ID & history is maintained because needed without login
        localStorage.removeItem("APICalls"); // This allows a refresh on logout
        VedAstro.clearAllPersonListCaches();
        localStorage.removeItem("UserId");
        localStorage.removeItem("UserName");

        // Remove all localStorage items with key "SelectedPerson-*"
        for (const key in localStorage) {
            if (key.startsWith("SelectedPerson-")) {
                localStorage.removeItem(key);
            }
        }

        // Tell user logout was success
        await Swal.fire({ icon: 'success', title: 'Bye, we\'ll miss you 🥰', timer: 2000, showConfirmButton: false });

        // Send user back to Home page (to avoid any login-related content reloading)
        window.location.href = './Home.html';
    }
}


//--------------------------------------- TOOLS -----------------------------------------------

/**
 * Tools used by others in this repo
 */
class CommonTools {
    //used as delay sleep execution
    static delay(ms) {
        return new Promise((resolve) => setTimeout(resolve, ms));
    }

    // will auto get payload out of json and checks reports failures to user
    // throws exception if fail
    static async GetAPIPayload(url) {
        try {

            // Send the request to the specified URL with the prepared options
            const response = await fetch(url);

            // If the response is not ok (status is not in the range 200-299), throw an error
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Parse the response body as JSON
            const data = await response.json();

            // If the 'Status' property of the parsed data is not 'Pass', throw an error
            if (data.Status !== "Pass") {
                throw new Error(data.Payload);
            }

            // If everything is ok, return the 'Payload' property of the parsed data
            return data.Payload;
        } catch (error) {
            // If an error is caught, display an error message using Swal.fire
            Swal.fire({
                icon: "error",
                title: "App Crash!",
                text: error,
                confirmButtonText: "OK",
            });
        }
    }

    static ShowLoading() {
        Swal.fire({
            showConfirmButton: false,
            width: "280px",
            padding: "1px",
            allowOutsideClick: false,
            allowEscapeKey: false,
            stopKeydownPropagation: true,
            keydownListenerCapture: true,
            html: `<img src="./images/loading-animation-progress-transparent.gif">`,
        });
    }

    static HideLoading() {
        //hide loading box
        Swal.close();
    }

    /**
     * Converts a camelCase string to PascalCase.
     * @param {string} camelCaseStr - The camelCase string to convert.
     * @returns {string} - The PascalCase string.
     */
    static camelCaseToPascalCase(camelCaseStr) {
        if (camelCaseStr && typeof camelCaseStr === 'string') {
            return camelCaseStr.charAt(0).toUpperCase() + camelCaseStr.slice(1);
        } else {
            return camelCaseStr;
        }
    }


    //converts camel case to pascal case, like "settings.keyColumn" to "settings.KeyColumn"
    static CamelCaseKeysToPascalCase(obj) {
        let newObj = Array.isArray(obj) ? [] : {};
        for (let key in obj) {
            let value = obj[key];
            let newKey = key.charAt(0).toUpperCase() + key.slice(1);
            if (value && typeof value === "object") {
                value = CommonTools.CamelCaseKeysToPascalCase(value);
            }
            newObj[newKey] = value;
        }
        return newObj;
    }

    /**
     * Takes a camel case or pascal case string and returns a string with spaces between the words.
     * Converts "MyNameIs" -> "My Name Is", "myNameIs" -> "My Name Is"
     */
    static CamelPascalCaseToSpaced(camelCase) {
        let result = camelCase
            .replace(/(\d)([A-Z])/g, '$1 $2')          // Insert space between a digit and uppercase letter
            .replace(/([a-z])([A-Z])/g, '$1 $2')       // Insert space between lowercase and uppercase letters
            .replace(/([A-Z])([A-Z][a-z])/g, '$1 $2')  // Insert space between consecutive uppercase letters followed by lowercase
            .trim();

        // Capitalize the first character if the original string starts with a lowercase letter
        if (camelCase[0] !== camelCase[0].toUpperCase()) {
            result = result.charAt(0).toUpperCase() + result.slice(1);
        }

        return result;
    }

    /**
     * Converts a name given in all caps to Pascal Case, but not initials.
     * @param {string} name - The name to be converted.
     * @returns {string} The converted name.
     */
    static convertNameToPascalCase(name) {
        return name.split(' ').map(word => {
            // If the word is longer than 2 characters, it's probably not an initial
            if (word.length > 2) {
                // Convert the first character to uppercase and the rest to lowercase
                return word.charAt(0) + word.slice(1).toLowerCase();
            } else {
                // Leave the word as it is (all uppercase)
                return word;
            }
        }).join(' ');
    }

    /**
     * Converts text to URL-safe text.
     * @param {string} text - The text to convert.
     * @returns {string} - The URL-safe text.
     */
    static toUrlSafe(text) {
        return encodeURIComponent(text);
    }

    static IsMobile() {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    }

    /**
     * Truncates a given text to a specified maximum length and appends an ellipsis.
     *
     * @param {string} text - The text to be truncated.
     * @param {number} maxChars - The maximum number of characters allowed.
     * @returns {string} The truncated text.
     */
    static TruncateText(text, maxChars) {
        if (typeof text !== 'string') {
            throw new Error('Input text must be a string.');
        }

        if (typeof maxChars !== 'number' || maxChars <= 0) {
            throw new Error('Maximum characters must be a positive number.');
        }

        return text.length > maxChars ? `${text.substring(0, maxChars)}...` : text;
    }

    // returns local time of browser. EXP : 14:00 23/04/2005 +07:00 
    static GetFormattedLocalTime() {
        try {
            const now = new Date();
            const offset = now.getTimezoneOffset();
            const offsetHours = Math.floor(Math.abs(offset) / 60);
            const offsetMinutes = Math.abs(offset) % 60;
            const sign = offset < 0 ? '+' : '-';
            const localTime = `${now.toLocaleTimeString('en-GB', { hour12: false })} ${now.toLocaleDateString('en-GB')} ${sign}${String(offsetHours).padStart(2, '0')}:${String(offsetMinutes).padStart(2, '0')}`;
            return localTime;
        } catch (error) {
            console.error('Error formatting local time:', error);
            return "00:00 00/00/0000 +00:00";
        }
    }

}


//--------------------------------------- DATA TYPES -----------------------------------------------

/**
 * Represents a Person entity.
 */
class Person {
    /**
     * Sample input JSON:
     * {
     *   "PersonId": "03c645a234234234193c28475f29",
     *   "Name": "Risyaalini Priyaa",
     *   "Notes": "",
     *   "BirthTime": {
     *     "StdTime": "13:54 25/10/1992 +08:00",
     *     "Location": {
     *       "Name": "Taiping",
     *       "Longitude": 103.82,
     *       "Latitude": 1.352
     *     }
     *   },
     *   "Gender": "Female",
     *   "OwnerId": "1342342334234234363117",
     *   "LifeEventList": [
     *     {
     *       "PersonId": "0334234234234193c28475f29",
     *       "Id": "f8de8107241944daab7d563a6eb03a98",
     *       "Name": "Talks of Marriage",
     *       "StartTime": {
     *         "StdTime": "23:02 05/02/2023 +08:00",
     *         "Location": {
     *           "Name": "Taiping",
     *           "Longitude": 0,
     *           "Latitude": 0
     *         }
     *       },
     *       "Description": "Marriage not yet confirmed looking for husband, venus bhukti with house 7 gochara",
     *       "Nature": "Good",
     *       "Weight": "Minor"
     *     }
     *   ]
     * }
     * 
     * @param {Object} jsonObject - The JSON object to initialize the Person instance.
     */
    constructor(jsonObject) {
        /**
         * The unique identifier of the person.
         * @type {string}
         */
        this.PersonId = jsonObject.PersonId;

        /**
         * The name of the person.
         * @type {string}
         */
        this.Name = jsonObject.Name;

        /**
         * Any notes about the person.
         * @type {string}
         */
        this.Notes = jsonObject.Notes;

        /**
         * The birth time of the person.
         * @type {Time}
         */
        this.BirthTime = new Time(jsonObject.BirthTime);

        /**
         * The gender of the person.
         * @type {string}
         */
        this.Gender = jsonObject.Gender;

        /**
         * The owner ID of the person.
         * @type {string}
         */
        this.OwnerId = jsonObject.OwnerId;

        /**
         * The list of life events associated with the person.
         * @type {LifeEvent[]}
         */
        this.LifeEventList = jsonObject.LifeEventList.map((lifeEvent) => new LifeEvent(lifeEvent));
    }

    // Get the display name with birth year for a person
    get DisplayName() {
        return `${this.Name} - ${this.BirthTime.GetYear()}`;
    }

    /**
     * Converts the person instance to a JSON object.
     * @returns {Object}
     */
    ToObject() {
        return {
            PersonId: this.PersonId,
            Name: this.Name,
            Notes: this.Notes,
            BirthTime: this.BirthTime.ToObject(),
            Gender: this.Gender,
            OwnerId: this.OwnerId,
            LifeEventList: this.lifeEventList.map((lifeEvent) => lifeEvent.ToObject()),
        };
    }

    /**
     * Converts the person instance to a JSON string.
     * @returns {string}
     */
    ToJson() {
        return JSON.stringify(this.ToObject());
    }
}

/**
 * Represents a Time object with standard time and location information.
 */
class Time {
    /**
     * Constructs a new Time object from a JSON object.
     * 
     * @param {Object} jsonObject - The JSON object to construct the Time object from.
     * @example
     * const time = new Time({
     *   "StdTime": "13:54 25/10/1992 +08:00",
     *   "Location": {
     *     "Name": "Taiping",
     *     "Longitude": 103.82,
     *     "Latitude": 1.352
     *   }
     * });
     */
    constructor(jsonObject) {
        /**
         * The standard time in the format "HH:mm dd/mm/yyyy +HH:MM".
         * @type {string}
         */
        this.StdTime = jsonObject.StdTime;

        /**
         * The location object associated with this time.
         * @type {GeoLocation}
         */
        this.Location = new GeoLocation(jsonObject.Location);
    }

    /**
     * Converts the Time object to a plain JavaScript object.
     * @return {Object} The plain JavaScript object representation of the Time object.
     */
    ToObject() {
        return {
            StdTime: this.stdTime,
            Location: this.location.ToObject(),
        };
    }

    // Get the year from the standard time
    GetYear() {
        const stdTime = this.StdTime; // e.g. "13:54 25/10/1992 +08:00"
        const [time, date] = stdTime.split(' ');
        const [hours, minutes] = time.split(':');
        const [day, month, year] = date.split('/');
        const birthDate = new Date(`${year}-${month}-${day}T${hours}:${minutes}:00.000Z`);
        return birthDate.getFullYear();
    }

    // Output TIME only for URL format
    // time converted to the format used in OPEN API url
    // Sample out : Location/London/Time/00:00/01/01/2011/+00:00/
    ToUrl() {
        // this will be called on instance of Time class
        const stdTime = this.StdTime.replace(/\s+/g, "/"); // convert all spaces to slashes
        const locationName = this.Location.Name.replace(/\s+/g, ""); // remove all spaces from location name

        const finalUrl = `Location/${locationName}/Time/${stdTime}/`;
        return finalUrl;
    }
}

/**
 * Represents a Life Event associated with a Person.
 */
class LifeEvent {
    /**
     * Constructs a new LifeEvent object from a JSON object.
     * 
     * @param {Object} jsonObject - The JSON object to construct the LifeEvent object from.
     * @example
     * const lifeEvent = new LifeEvent({
     *   "PersonId": "03c645a91cc1492b97a8193c28475f29",
     *   "Id": "f8de8107241944daab7d563a6eb03a98",
     *   "Name": "Talks of Marriage",
     *   "StartTime": {
     *     "StdTime": "23:02 05/02/2023 +08:00",
     *     "Location": {
     *       "Name": "Taiping",
     *       "Longitude": 0,
     *       "Latitude": 0
     *     }
     *   },
     *   "Description": "Marriage not yet confirmed looking for husband, venus bhukti with house 7 gochara",
     *   "Nature": "Good",
     *   "Weight": "Minor"
     * });
     */
    constructor(jsonObject) {
        /**
         * The unique identifier of the Person associated with this Life Event.
         * @type {string}
         */
        this.PersonId = jsonObject.PersonId;

        /**
         * The unique identifier of this Life Event.
         * @type {string}
         */
        this.Id = jsonObject.Id;

        /**
         * The name of this Life Event.
         * @type {string}
         */
        this.Name = jsonObject.Name;

        /**
         * The start time of this Life Event.
         * @type {Time}
         */
        this.StartTime = new Time(jsonObject.StartTime);

        /**
         * A brief description of this Life Event.
         * @type {string}
         */
        this.Description = jsonObject.Description;

        /**
         * The nature of this Life Event (e.g. "Good", "Bad", etc.).
         * @type {string}
         */
        this.Nature = jsonObject.Nature;

        /**
         * The weight or significance of this Life Event (e.g. "Minor", "Major", etc.).
         * @type {string}
         */
        this.Weight = jsonObject.Weight;
    }

    /**
     * Converts this Life Event object to a plain JavaScript object.
     * @return {Object} The plain JavaScript object representation of this Life Event.
     */
    ToObject() {
        return {
            PersonId: this.PersonId,
            Id: this.Id,
            Name: this.Name,
            StartTime: this.StartTime.ToObject(),
            Description: this.Description,
            Nature: this.Nature,
            Weight: this.Weight,
        };
    }
}

/**
 * Represents a geographic location.
 */
class GeoLocation {
    /**
     * Constructs a new GeoLocation object from a JSON object.
     * 
     * @param {Object} jsonObject - The JSON object to construct the Location object from.
     * @example
     * const location = new GeoLocation({
     *   "Name": "Taiping",
     *   "Longitude": 103.82,
     *   "Latitude": 1.352
     * });
     */
    constructor(jsonObject) {
        /**
         * The name of this location.
         * @type {string}
         */
        this.Name = jsonObject.Name;

        /**
         * The longitude of this location.
         * @type {number}
         */
        this.Longitude = jsonObject.Longitude;

        /**
         * The latitude of this location.
         * @type {number}
         */
        this.Latitude = jsonObject.Latitude;
    }

    /**
     * Converts this Location object to a plain JavaScript object.
     * @return {Object} The plain JavaScript object representation of this Location.
     */
    ToObject() {
        return {
            Name: this.Name,
            Longitude: this.Longitude,
            Latitude: this.Latitude,
        };
    }
}


//-------------------------------- VIEW COMPONENTS -----------------------------------

class TemplateClass {
    // Class properties
    ElementID = "";
    TitleText = "Title Goes Here";
    DescriptionText = "Description Goes Here";
    ImageSrc = "images/user-guide-banner.png";

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.TitleText = element.getAttribute("title-text") || "Title Goes Here";
        this.DescriptionText = element.getAttribute("description-text") || "Description Goes Here";
        this.ImageSrc = element.getAttribute("image-src") || "images/user-guide-banner.png";

        // Call the method to initialize the main
        this.initializeMainBody();
    }

    // Method to initialize the main body 
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());
    }

    // Method to generate the HTML
    generateHtmlBody() {
        return `
    `;
    }
}

class ID {
    static CursorLineLegendTemplate = `#CursorLineLegendTemplate`;
    static TimeRowLegendTemplate = `#TimeRowLegendTemplate`;
    static CursorLineLegendCloneCls = ".CursorLineLegendClone";
    static LifeEventNameLabelCls = ".name-label";
    static LifeEventVerticalLineCls = ".vertical-line";
    static LifeEventDescriptionLabelCls = ".description-label";
    static CursorLineLegendDescriptionHolder =
        "#CursorLineLegendDescriptionHolder";
    static EventChartHolder = ".EventChartHolder"; //main chart SVG element, used class since ID is unique number
    static EventsChartSvgHolder = "#EventsChartSvgHolder"; //default expected parent in page to inject chart into
    static CursorLine = "#CursorLine";
    static CursorLineLegendClone = "CursorLineLegendClone";
    static CursorLineLegendHolder = "#CursorLineLegendHolder";
    static CursorLineSumIcon = "#CursorLineSumIcon";
    static NowVerticalLine = "#NowVerticalLine";
    static EventListHolder = ".EventListHolder";
    static CursorLineClockIcon = "#CursorLineClockIcon";
    static CursorLineLegendDescription = "#CursorLineLegendDescription";
    static CursorLineLegendDescriptionBackground =
        "#CursorLineLegendDescriptionBackground";
}

//Single place for all code related to animating Events Chart SVG, used by light & full viewer
//this class brings SVG Events Charts to life 🌱
//DESIGN NOTE: no logic to generate chart should be here
//all generation via URL or API is to be done as separate helper functions only
class EventsChart {
    //note: these are color codes used to
    //detect if event is good or bad
    static BadColor = "#FF0000";
    static GoodColor = "#00FF00";

    //row height used to legend row
    static RowHeight = 15;

    constructor(chartId) {
        //use chart ID find the element on page
        //note: we make sure here that only the elements inside this specific SVG chart will be manipulated
        this.$EventsChartSvgHolder = $(ID.EventsChartSvgHolder);
        this.$SvgChartElm = $(`#${chartId}`);
        this.Id = chartId;
        this.$CursorLine = this.$SvgChartElm.find(ID.CursorLine);
        this.$LifeEventNameLabel = this.$SvgChartElm.find(ID.LifeEventNameLabelCls);
        this.$NowVerticalLine = this.$SvgChartElm.find(ID.NowVerticalLine); //save now line
        this.AllEventRects = this.$SvgChartElm
            .find(ID.EventListHolder)
            .children("rect");
        this.$CursorLineLegendDescriptionHolder = this.$SvgChartElm.find(
            ID.CursorLineLegendDescriptionHolder
        );
        this.$CursorLineLegendTemplate = this.$SvgChartElm.find(
            ID.CursorLineLegendTemplate
        );
        this.$TimeRowLegendTemplate = this.$SvgChartElm.find(
            ID.TimeRowLegendTemplate
        );
        this.$CursorLineLegendDescription = this.$SvgChartElm.find(
            ID.CursorLineLegendDescription
        );
        this.$CursorLineLegendDescriptionBackground = this.$SvgChartElm.find(
            ID.CursorLineLegendDescriptionBackground
        );
        this.$CursorLineLegendDescriptionHolder = this.$SvgChartElm.find(
            ID.CursorLineLegendDescriptionHolder
        );
        this.DescText = { xAxis: 175, yAxis: 24 }; //used to position desc box cursor legend
        this.$CursorLineLegendHolder = this.$SvgChartElm.find(
            ID.CursorLineLegendHolder
        );

        //bring to life
        this.attachEventHandlers();

        //add chart to public list of charts after brought to live
        //create new if 1st chart on page
        window.EventsChartLoaded = this;
        if (typeof window.EventsChartList === "undefined") {
            window.EventsChartList = [];
        }
        window.EventsChartList.push(this);

        //return index of last row pushed
        return window.EventsChartList.length - 1;
    }

    //------------------------------------------------------------------------

    attachEventHandlers() {
        console.log("Attaching events to chart...");

        //1 TIME LEGEND
        //we pump the current EventChart instance into handler
        this.$SvgChartElm.mousemove((mouse) =>
            EventsChart.onMouseMoveHandler(mouse, this)
        );
        this.$SvgChartElm.mouseleave((mouse) =>
            EventsChart.onMouseLeaveEventChart(mouse, this)
        );

        //2 NOW LINE
        //update once now
        EventsChart.updateNowLine(this);

        //setup to auto update every 1 minute
        setInterval(() => EventsChart.updateNowLine(this), 60 * 1000); // 60 seconds

        //3 HIGHLIGHT LIFE EVENT
        this.$LifeEventNameLabel.mouseenter((mouse) =>
            EventsChart.onMouseEnterLifeEventHandler(mouse, this)
        );
        this.$LifeEventNameLabel.mouseleave((mouse) =>
            EventsChart.onMouseLeaveLifeEventHandler(mouse, this)
        );
    }

    //on click add events to google calendar,
    //ask user to select event and take from there
    AddEventsToGoogleCalendar() {
        console.log("Adding events to Google Calendar");

        //tell user to select an event
        Swal.fire({
            title: "Select an event",
            text: "The selected event will be sent to your Google Calendar",
            icon: "info",
            confirmButtonText: "OK",
        });

        //attach one time trigger to catch the event user clicked on
        $(".EventChartContent").one("click", (eventData) =>
            EventsChart.onClickSelectedGoogleCalendarEvent(eventData, this)
        );
    }

    //highlights all events rects in chart by
    //the inputed keyword in the event name
    highlightByEventName(keyword) {
        //find all rects representing the keyword based event
        //note keyword will be planet name or house name
        this.AllEventRects.each(function (index) {
            //get parsed time from rect
            var svgEventRect = this;
            var eventName = svgEventRect.getAttribute("eventname");
            //check if event name contains keyword
            var foundEvent = eventName.toLowerCase().includes(keyword.toLowerCase());

            //if event is related to planet, highlight the rect
            if (foundEvent) {
                //save original color for later return
                var oriColor = svgEventRect.getAttribute("fill");
                svgEventRect.setAttribute("fillORI", oriColor);

                //set new highlight color
                var highlightColor = EventsChart.getHighlightColor(keyword);
                svgEventRect.setAttribute("fill", highlightColor);
            }
        });
    }

    unhighlightByEventName(keyword) {
        //find all rects representing the keyword based event
        this.AllEventRects.each(function (index) {
            //get parsed time from rect
            var svgEventRect = this;
            var eventName = svgEventRect.getAttribute("eventname");
            //check if event name contains keyword
            var foundEvent = eventName.toLowerCase().includes(keyword.toLowerCase());

            //if event is related to planet, highlight the rect
            if (foundEvent) {
                //save original color for later return
                var oriColor = svgEventRect.getAttribute("fillORI");

                //ori will be null if never highlighted before
                oriColor =
                    oriColor === null ? svgEventRect.getAttribute("fill") : oriColor;

                //set original color if changed, else same color
                svgEventRect.setAttribute("fill", oriColor);
            }
        });
    }

    //-----------------------------STATIC----------------------------------------

    //for highlighting events by name
    static getHighlightColor(keyword) {
        switch (keyword.toLowerCase()) {
            //planets
            case "sun":
                return "#FFA500"; //orange #FFA500
            case "moon":
                return "#7A7A7A"; //silver #7A7A7A
            case "mars":
                return "#DC143C"; //crimson #DC143C
            case "mercury":
                return "#00FF7F"; //springgreen #00FF7F
            case "jupiter":
                return "#EEEE00"; //yellow #EEEE00
            case "venus":
                return "#FF00FF"; //magenta #FF00FF
            case "saturn":
                return "#0000FF"; //blue #0000FF
            case "rahu":
                return "#FF7D40"; //flesh #FF7D40
            case "ketu":
                return "#515151"; //grey #515151

            //house
            //colors is the full spectrum divided into 12
            //done to have the most unique colors possible for each house
            case "house 1":
                return "#ff0000"; //red
            case "house 2":
                return "#ff7f0a"; //orange
            case "house 3":
                return "#ffff00"; //yellow
            case "house 4":
                return "#7fff00"; //chartreuse green
            case "house 5":
                return "#00ff00"; //green
            case "house 6":
                return "#00ff7f"; //spring green
            case "house 7":
                return "#00ffff"; //cyan
            case "house 8":
                return "#007fff"; //azure
            case "house 9":
                return "#0000ff"; //blue
            case "house 10":
                return "#7f00ff"; //violet
            case "house 11":
                return "#ff00ff"; //magenta
            case "house 12":
                return "#ff007f"; //rose
        }

        //default to black so we know it was not accounted for
        return "#000000";

        //    var arrayValues = ["#ff60fa", "#ff60fa", "#ff60fa"];

        //    var arrayMax = arrayValues.length - 1;
        //    var randomIndex = Math.floor(Math.random() * arrayMax);

        //    return arrayValues[randomIndex];
    }

    //update now line position
    static updateNowLine(instance) {
        console.log("Updating now line position...");

        //store closes rect to now time
        var closestRectToNow;

        //find closest rect to now time
        instance.AllEventRects.each((index, element) => findClosest(element));

        //get horizontal position of now rect (x axis) (conditional access, not initialized all the time)
        var xAxisNowRect = closestRectToNow?.getAttribute("x");

        //only set line position if, data is valid
        if (xAxisNowRect) {
            instance.$NowVerticalLine.attr(
                "transform",
                `matrix(1, 0, 0, 1, ${xAxisNowRect}, 0)`
            );
        }

        //----------------------------------LOCAL FUNK------------------------

        function findClosest(svgEventRect) {
            //get parsed time from rect
            var rectTime = getTimeInRect(svgEventRect).getTime(); //(milliseconds since 1 Jan 1970)
            var nowTime = Date.now();

            //if not yet reach continue, keep reference to this and goto next
            if (rectTime <= nowTime) {
                closestRectToNow = svgEventRect;
                return true; //go next
            }
            //already passed now time, use previous rect as now, stop looking
            else {
                return false;
            }
        }

        //parses the STD time found in rect and returns it
        function getTimeInRect(eventRect$) {
            //convert "00:28 17/11/2022 +08:00" to "2019-01-01T00:00:00.000+00:00"
            var stdTimeRaw = eventRect$.getAttribute("stdtime");
            var stdTimeSplit = stdTimeRaw.split(" ");
            var hourMin = stdTimeSplit[0];
            var dateFull = stdTimeSplit[1].split("/");
            var date = dateFull[0];
            var month = dateFull[1];
            var year = dateFull[2];
            var timezone = stdTimeSplit[2];
            var rectTime = new Date(
                `${year}-${month}-${date}T${hourMin}:00.000${timezone}`
            );

            return rectTime;
        }
    }

    //on mouse leave event chart, auto hide time legend
    static onMouseLeaveEventChart(mouse, instance) {
        instance.$CursorLine.hide();
    }

    //on mouse over life event name label, highlight event line
    static onMouseEnterLifeEventHandler(mouse, instance) {
        //get label that has mouse over it
        var targetElement = mouse.currentTarget;

        //find the main vertical line for life event
        var $parent = $(targetElement).parent();
        var $verticalLine = $parent.siblings(ID.LifeEventVerticalLineCls);

        //make wider
        $verticalLine.attr("width", "3");

        //highlight color
        $verticalLine.attr("fill", "#e502fa");

        //glow
        $verticalLine.css("filter", "drop-shadow(0px 0px 1px rgb(255 0 0))");

        //make hidden description box visible (if any text)
        var $descBox = $parent.children(ID.LifeEventDescriptionLabelCls);
        if ($descBox.text().trim() !== "") {
            $descBox.show();
        }
    }

    //on mouse leave life event name label, unhighlight event line
    static onMouseLeaveLifeEventHandler(mouse, instance) {
        //get label that has mouse over it
        var targetElement = mouse.currentTarget;

        //find the main vertical line for life event
        var $parent = $(targetElement).parent();
        var $verticalLine = $parent.siblings(ID.LifeEventVerticalLineCls);

        //set back normal line width
        $verticalLine.attr("width", "2");

        //set line color back to default
        $verticalLine.attr("fill", "#1E1EEA");

        //glow
        $verticalLine.css("filter", "");

        //hide description box if not major
        var $descBox = $parent.children(ID.LifeEventDescriptionLabelCls);
        var isNotMajor =
            $parent.parent()[0].getAttribute("eventweight") !== "Major";
        if (isNotMajor) {
            $descBox.hide();
        }
    }

    //converts VedAstro date format to Google Calendar format
    static convertDateFormat(dateStr) {
        // Split the date and time parts
        // NOTE: location is ignored here
        let [timePart, datePart, zonePart] = dateStr.StdTime.split(" ");
        // Split the date into day, month, and year
        let [day, month, year] = datePart.split("/");
        // Combine the parts into a new date string and create a new Date object
        let dateObj = new Date(`${year}-${month}-${day}T${timePart}${zonePart}`);

        // TODO: Convert offset to timezone. This is not straightforward because multiple timezones can have the same offset.
        const timeZone = "";

        // Return the JSON object
        return {
            dateTime: dateObj.toISOString(), // Return the date in ISO 8601 format
            timeZone: timeZone,
        };
    }

    //Gets a mouses x-axis relative inside the given element
    //used to get mouse location on SVG chart, zoom autocorrected
    static getMousePositionInElement(mouseEventData, instance) {
        //get relative position of mouse in Dasa view
        //after zoom pixels on screen change, but when rendering
        //SVG description box we need x, y before zoom (AI's code!)
        var holder = instance.$EventsChartSvgHolder[0]; //zoom is done on main holder in Blazor side

        var mousePosition = {};
        if (holder != null) {
            var zoom = parseFloat(window.getComputedStyle(holder).zoom);
            mousePosition = {
                xAxis: mouseEventData.originalEvent.offsetX / zoom,
                yAxis: mouseEventData.originalEvent.offsetY / zoom,
            };
        }
        //in svg direct browser we don't have DIV holder, so no zoom correction
        else {
            mousePosition = {
                xAxis: mouseEventData.originalEvent.offsetX,
                yAxis: mouseEventData.originalEvent.offsetY,
            };
        }

        return mousePosition;
    }

    //called by trigger when clicked on event, after asking user to select
    //to here for adding events to google
    static async onClickSelectedGoogleCalendarEvent(eventObject, instance) {
        //get details on the selected event
        var targetRect = eventObject.target;

        //given the SVG rect that was clicked on, process and extract full event data
        var parsedEvent = (
            await EventsChart.ParseEventFromSVGRect(targetRect, instance)
        )["EventStartEndTime"];

        //if no event found then possible wrongly clicked elm skip, END HERE
        if (parsedEvent?.Name !== undefined) {
            Swal.fire("Could not detect event", "", "warning");
            return;
        }

        //ask user if selected event is correct and want to continue to google login
        var userReply = await Swal.fire({
            title: "Send event to Google?",
            html:
                '<ul class="list-group">' +
                `<li class="list-group-item">Name : <strong>${parsedEvent.Name}</strong></li>` +
                `<li class="list-group-item">Start : <strong>${parsedEvent.StartTime.StdTime}</strong></li>` +
                `<li class="list-group-item">End : <strong>${parsedEvent.EndTime.StdTime}</strong></li>` +
                "</ul>",
            icon: "info",
            iconHtml:
                '<iconify-icon icon="fluent:calendar-add-20-regular" width="20" height="20"></iconify-icon>',
            showCancelButton: true,
            confirmButtonText: "Yes",
            cancelButtonText: "No",
        });

        //based on what user clicked process
        if (userReply.isConfirmed) {
            // User clicked 'Yes', continue to Google login page
            EventsChart.SelectAccountAndAddEvent(parsedEvent);
        } else {
            // User clicked 'No', end silently
            console.log("User clicked No on sending to Google");
        }
    }

    //given an SVG rect of an event, extract event data from it, with start and end time (use API)
    static async ParseEventFromSVGRect(targetRect, instance) {
        //prepare the URL
        var domain = "https://vedastroapi.azurewebsites.net/api";

        //get birth time from main svg element
        var birthTimeAry = instance.$SvgChartElm[0]
            .getAttribute("birthtime")
            .split(" ");
        var birthLocationTxt =
            instance.$SvgChartElm[0].getAttribute("birthlocation");
        var birthTime = `/Location/${birthLocationTxt}/Time/${birthTimeAry[0]}/${birthTimeAry[1]}/${birthTimeAry[2]}`;

        //get check time & event name from clicked rect,
        //start and end time should be before and after from this
        var checkTimeAry = targetRect.getAttribute("stdtime").split(" ");
        //TODO Location set based on where user is
        var checkTime = `/Location/${birthLocationTxt.replace(/\s/g, "")}/Time/${checkTimeAry[0]
            }/${checkTimeAry[1]}/${checkTimeAry[2]}`;

        //get name of event
        var withSpaces = targetRect.getAttribute("eventname");
        var eventName = `/EventName/${withSpaces.replace(/\s/g, "")}`; //remove spaces

        //put together final API call URL
        var finalUrl = `${domain}/Calculate/EventStartEndTime${birthTime}${checkTime}${eventName}`;

        //make call to API, replies JSON of Event
        var eventDataAtTime = await EventsChart.GetAPIPayload(finalUrl);

        return eventDataAtTime;
    }

    //given a vedastro API url, will auto call via POST or GET
    //and return only passed payloads as JSON
    static async GetAPIPayload(url, payload = null) {
        try {
            // If a payload is provided, prepare options for a POST request
            const options = payload
                ? {
                    method: "POST", // Specify the HTTP method as POST
                    headers: { "Content-Type": "application/json" }, // Set the content type of the request to JSON
                    body: JSON.stringify(payload), // Convert the payload to a JSON string and include it in the body of the request
                }
                : {}; // If no payload is provided, create an empty options object, which defaults to a GET request

            // Send the request to the specified URL with the prepared options
            const response = await fetch(url, options);

            // If the response is not ok (status is not in the range 200-299), throw an error
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Parse the response body as JSON
            const data = await response.json();

            // If the 'Status' property of the parsed data is not 'Pass', throw an error
            if (data.Status !== "Pass") {
                throw new Error(data.Payload);
            }

            // If everything is ok, return the 'Payload' property of the parsed data
            return data.Payload;
        } catch (error) {
            // If an error is caught, display an error message using Swal.fire
            Swal.fire({
                icon: "error",
                title: "App Crash!",
                text: error,
                confirmButtonText: "OK",
            });
        }
    }

    static addEventToGoogleCalendar(parsedEvent) {
        //show event that was selected
        console.log(`EventSelected: ${parsedEvent.Name}`);

        //convert to format supported by Google Calendar
        var parsedStartTime = EventsChart.convertDateFormat(parsedEvent.StartTime);
        var parsedEndTime = EventsChart.convertDateFormat(parsedEvent.EndTime);

        const event = {
            summary: parsedEvent.Name,
            //'location': '',
            description: parsedEvent.Description,
            start: parsedStartTime,
            end: parsedEndTime,
            //'recurrence': [
            //    'RRULE:FREQ=DAILY;COUNT=2'
            //],
            //'attendees': [
            //    { 'email': 'lpage@example.com' },
            //    { 'email': 'sbrin@example.com' }
            //],
            reminders: {
                useDefault: false,
                overrides: [
                    { method: "email", minutes: 24 * 60 },
                    { method: "popup", minutes: 10 },
                ],
            },
        };

        const request = window.gapi.client.calendar.events.insert({
            calendarId: "primary", //set default calendar todo future select calendar
            resource: event,
        });

        request.execute(function (event) {
            //STATE: events successfully created and updated to Google
            //tell user about it
            Swal.fire({
                title: "Event added!",
                text: `Added ${event.summary}, view here ${event.htmlLink}`,
                icon: "info",
                iconHtml:
                    '<iconify-icon icon="streamline:interface-calendar-check-approve-calendar-check-date-day-month-success" width="20" height="20"></iconify-icon>',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: "Great!",
            });
        });
    }

    /**
     *  Sign in the user to select calendar account and then add event immediately
     */
    static SelectAccountAndAddEvent(parsedEvent) {
        window.tokenClient.callback = async (resp) => {
            if (resp.error !== undefined) {
                throw resp;
            }

            //now already logged in continue to add events
            EventsChart.addEventToGoogleCalendar(parsedEvent);
        };

        if (window.gapi.client.getToken() === null) {
            // Prompt the user to select a Google Account and ask for consent to share their data
            // when establishing a new session.
            window.tokenClient.requestAccessToken({ prompt: "consent" });
        } else {
            // Skip display of account chooser and consent dialog for an existing session.
            window.tokenClient.requestAccessToken({ prompt: "" });
        }
    }

    /**
     * Print the summary and start datetime/date of the next ten events in
     * the authorized user's calendar. If no events are found an
     * appropriate message is printed.
     */
    static async listUpcomingEvents() {
        let response;
        try {
            const request = {
                calendarId: "primary",
                timeMin: new Date().toISOString(),
                showDeleted: false,
                singleEvents: true,
                maxResults: 10,
                orderBy: "startTime",
            };
            response = await window.gapi.client.calendar.events.list(request);
        } catch (err) {
            document.getElementById("content").innerText = err.message;
            return;
        }

        const events = response.result.items;
        if (!events || events.length == 0) {
            console.log("No events found.");
            return;
        }
        // Flatten to string to display
        const output = events.reduce(
            (str, event) =>
                `${str}${event.summary} (${event.start.dateTime || event.start.date
                })\n`,
            "Events:\n"
        );
        console.log(output);
    }

    //fired when mouse moves over dasa view box
    //used to auto update cursor line & time legend
    static onMouseMoveHandler(mouse, instance) {
        //get relative position of mouse in Dasa view
        //after zoom pixels on screen change, but when rendering
        //SVG description box we need x, y before zoom (AI's code!)
        var mousePosition = EventsChart.getMousePositionInElement(mouse, instance); //todo no work in zoom

        //if cursor is out of chart view hide cursor and end here
        if (mousePosition === 0) {
            SVG(instance.$CursorLine[0]).hide();
            return;
        } else {
            SVG(instance.$CursorLine[0]).show();
        }

        //move cursor line 1st for responsiveness
        autoMoveCursorLine(mousePosition.xAxis);

        //update time legend
        generateTimeLegend(mousePosition);

        //-------------------------LOCAL FUNCS--------------------------

        function autoMoveCursorLine(relativeMouseX) {
            //give a tiny delay so user can aim better at event
            setTimeout(() => { }, 157);

            //move vertical line to under mouse inside dasa view box
            instance.$CursorLine.attr(
                "transform",
                `matrix(1, 0, 0, 1, ${relativeMouseX}, 0)`
            );
        }

        //SVG Event Chart Time Legend generator
        //this is where the whole time legend that follows
        //the mouse when placed on chart is generated
        //notes: a template row always exists in code,
        //in client JS side uses template to create the rows from cloning it
        //and modifying its prop as needed, as such any major edit needs to
        //be done in API code
        function generateTimeLegend(mousePosition) {
            // Round mouse position to match with axis values in rect
            const mouseRoundedX = Math.round(mousePosition.xAxis);
            const mouseRoundedY = Math.round(mousePosition.yAxis);

            // Get all event rects at the mouse's X position
            const allElementsAtX = instance.$SvgChartElm
                .children()
                .find(`[x=${mouseRoundedX}]`);
            const allEventRectsAtX = getAllEventRectsAtX(allElementsAtX);

            // Remove previously generated legend rows
            removePreviousClones();

            //if no elements, don't create summary row, end here (note check only after remove)
            if (!(allEventRectsAtX.length > 0)) {
                return;
            }

            // Initialize counts for summary row
            let goodCount = 0;
            let badCount = 0;
            instance.showDescription = false; // Default description not shown

            // Extract event data and place it in legend
            allEventRectsAtX.forEach((element) =>
                drawEventRow(element, mouseRoundedY, allElementsAtX)
            );

            // Show or hide description box based on mouse position
            toggleDescriptionBox();

            // Generate summary row at the bottom showing count of good & bad
            //generateSummaryRow(allEventRectsAtX, goodCount, badCount);

            //-----------------------------------------LOCAL FUNCS-------------------------------

            //code to draw one event row box
            function drawEventRow(svgEventRect, mouseRoundedY, allElementsAtX) {
                //STAGE 1
                //GET DATA
                //extract other data out of the rect
                var eventName = svgEventRect.getAttribute("eventname");
                //if no "eventname" exist, wrong elm skip it
                if (!eventName) {
                    return;
                }

                var eventDescription = svgEventRect.getAttribute("eventdescription");
                var natureScore = svgEventRect.getAttribute("naturescore");
                var color = svgEventRect.getAttribute("fill");
                var newRowYAxis = parseInt(svgEventRect.getAttribute("y")); //parse as num, for calculation

                //STAGE 2
                //TIME & AGE LEGEND
                //create time legend at top only for first element
                if (allElementsAtX[0] === svgEventRect) {
                    drawTimeAgeLegendRow();
                }

                //STAGE 3
                //GENERATE EVENT ROW
                //make a copy of template for this event
                var newLegendRow = instance.$CursorLineLegendTemplate.clone();
                newLegendRow.removeAttr("id"); //remove the clone template id
                newLegendRow.addClass(ID.CursorLineLegendClone); //to delete it on next run
                newLegendRow.appendTo(instance.$CursorLineLegendHolder); //place new legend into special holder
                SVG(newLegendRow[0]).show(); //make cloned visible
                //position the group holding the legend over the event row which the legend represents
                newLegendRow.attr(
                    "transform",
                    `matrix(1, 0, 0, 1, 10, ${newRowYAxis - 2})`
                ); //minus 2 for perfect alignment

                //#set data into view
                //nature score
                var $natureScoreElm = newLegendRow.children(".nature-score"); //GET
                let numberOnly = Math.abs(parseInt(natureScore)); //remove "-" symbol
                $natureScoreElm[0].innerHTML = numberOnly; //SET

                //icon color
                var iconElm = newLegendRow.children(".icon-holder"); //GET
                iconElm.attr("fill", color); //SET

                //event name next to nature score
                var $eventNameElm = newLegendRow.children(".event-name"); //GET
                $eventNameElm[0].innerHTML = eventName; //SET

                //STAGE 4
                //GENERATE DESCRIPTION ROW LOGIC
                //check if mouse in within row of this event (y-axis)
                var elementTopY = newRowYAxis;
                var elementBottomY = newRowYAxis + EventsChart.RowHeight;
                var mouseWithinRow =
                    mouseRoundedY >= elementTopY && mouseRoundedY <= elementBottomY;
                //if event name is still the same then don't load description again
                var notSameEvent = instance.previousHoverEventName !== eventName;

                //STAGE 5
                //HIGHLIGHT ROW (BASED ON CURSOR)
                //if mouse is in event's row then highlight that row
                if (mouseWithinRow) {
                    //highlight event name row
                    var $backgroundElm = newLegendRow.children("rect");

                    $eventNameElm.attr("font-weight", "700");
                    $backgroundElm.attr("fill", "#0096FF"); //bright blue
                    $backgroundElm.attr("opacity", 1); //solid

                    //if mouse within show description box
                    instance.showDescription = true;
                }

                //if mouse within row AND the event has changed
                //then generate a new description
                //note: this is slow, so done only when absolutely needed
                if (mouseWithinRow && notSameEvent) {
                    //make holder visible
                    SVG(instance.$CursorLineLegendDescriptionHolder[0]).show();

                    //move holder next to event
                    var descBoxYAxis = newRowYAxis - 9; //minus 5 for perfect alignment with event name row
                    instance.$CursorLineLegendDescriptionHolder.attr(
                        "transform",
                        `matrix(1, 0, 0, 1, 0, ${descBoxYAxis})`
                    );

                    //note: using trigger to make it easy to skip multiple clogging events
                    //set event desc directly
                    drawDescriptionBox(eventDescription);

                    //update previous hover event
                    instance.previousHoverEventName = eventName;
                }

                //---------------------LOCAL---------------------------

                function drawDescriptionBox(eventDescRaw) {
                    //remove tabs and new line to make easy detection of empty string
                    let eventDesc = eventDescRaw.replace(/ {4}|[\t\n\r]/gm, "");

                    //if no description than hide box & end here
                    if (!eventDesc) {
                        instance.$CursorLineLegendDescriptionHolder.hide();
                        return;
                    }

                    //convert text to svg and place inside holder
                    var wrappedDescText = textToSvg(
                        eventDesc,
                        instance.DescText.xAxis,
                        instance.DescText.yAxis
                    );

                    instance.$CursorLineLegendDescription.empty(); //clear previous desc
                    $(wrappedDescText).appendTo(instance.$CursorLineLegendDescription); //add in new desc
                    //set height of desc box background
                    instance.$CursorLineLegendDescriptionBackground.attr(
                        "height",
                        instance.EventDescriptionTextHeight + 20
                    ); //plus little for padding

                    //-------------------------------------LOCAL FUNCTIONS-------------------------------------------

                    //  This function attempts to create a new svg "text" element, chopping
                    //  it up into "tspan" pieces, if the text is too long
                    function textToSvg(caption, x, y) {
                        //svg "text" element to hold smaller text lines
                        var svgTextHolder = document.createElementNS(
                            "http://www.w3.org/2000/svg",
                            "text"
                        );
                        svgTextHolder.setAttributeNS(null, "x", x);
                        svgTextHolder.setAttributeNS(null, "y", y);
                        svgTextHolder.setAttributeNS(null, "font-size", 10);
                        svgTextHolder.setAttributeNS(null, "fill", "#FFF");
                        svgTextHolder.setAttributeNS(null, "text-anchor", "left");

                        //The following two variables can be passed as parameters
                        var maximumCharsPerLine = 30;
                        var lineHeight = 10;

                        var words = caption.split(" ");
                        var line = "";

                        //process text and create rows
                        var svgTSpan;
                        var lineCount = 0; //number of lines to calculate height
                        var tSpanTextNode;
                        for (var n = 0; n < words.length; n++) {
                            var testLine = line + words[n] + " ";
                            if (testLine.length > maximumCharsPerLine) {
                                //  Add a new <tspan> element
                                svgTSpan = document.createElementNS(
                                    "http://www.w3.org/2000/svg",
                                    "tspan"
                                );
                                svgTSpan.setAttributeNS(null, "x", x);
                                svgTSpan.setAttributeNS(null, "y", y);

                                tSpanTextNode = document.createTextNode(line);
                                svgTSpan.appendChild(tSpanTextNode);
                                svgTextHolder.appendChild(svgTSpan);

                                line = words[n] + " ";
                                y += lineHeight; //place next text row lower
                                lineCount++; //count a line
                            } else {
                                line = testLine;
                            }
                        }

                        //calculate final height in px, save global to be accessed later
                        instance.EventDescriptionTextHeight = lineCount * lineHeight;

                        svgTSpan = document.createElementNS(
                            "http://www.w3.org/2000/svg",
                            "tspan"
                        );
                        svgTSpan.setAttributeNS(null, "x", x);
                        svgTSpan.setAttributeNS(null, "y", y);

                        tSpanTextNode = document.createTextNode(line);
                        svgTSpan.appendChild(tSpanTextNode);

                        svgTextHolder.appendChild(svgTSpan);

                        return svgTextHolder;
                    }
                }

                function drawTimeAgeLegendRow() {
                    //make a copy of the template
                    var newTimeLegend = instance.$TimeRowLegendTemplate.clone();

                    //modify the template with new data
                    newTimeLegend.removeAttr("id"); //remove the clone template id
                    newTimeLegend.addClass(ID.CursorLineLegendClone); //to delete it on next run
                    newTimeLegend.appendTo(instance.$CursorLineLegendHolder); //place new legend into special holder

                    //make cloned visible
                    SVG(newTimeLegend[0]).show();

                    //place above 1st row
                    newTimeLegend.attr(
                        "transform",
                        `matrix(1, 0, 0, 1, 10, ${newRowYAxis - EventsChart.RowHeight})`
                    );

                    //get time & age data and place into top legend row
                    var stdTimeFull = svgEventRect.getAttribute("stdtime");
                    var stdTimeSplit = stdTimeFull.split(" ");
                    var hourMin = stdTimeSplit[0];
                    var date = stdTimeSplit[1];
                    var timezone = stdTimeSplit[2];
                    var age = svgEventRect.getAttribute("age");
                    newTimeLegend
                        .children("text")
                        .text(`${hourMin} ${date}  AGE: ${age}`);

                    //replace circle with clock icon
                    //newTimeLegend.children("use").attr("xlink:href", ID.CursorLineClockIcon);
                }
            }

            function getAllEventRectsAtX(allElementsAtX) {
                const allEventRectsAtX = [];
                allElementsAtX.each((index, element) => {
                    const eventName = element.getAttribute("eventname");
                    if (eventName) {
                        allEventRectsAtX.push(element);
                    }
                });
                return allEventRectsAtX;
            }

            function removePreviousClones() {
                const previousClones = instance.$SvgChartElm.find(
                    ID.CursorLineLegendCloneCls
                );
                previousClones.remove();
            }

            function toggleDescriptionBox() {
                if (instance.showDescription) {
                    SVG(instance.$CursorLineLegendDescriptionHolder[0]).show();
                } else {
                    SVG(instance.$CursorLineLegendDescriptionHolder[0]).hide();
                }
            }

            /**
             * Generates a summary row for event rectangles.
             *
             * @param {Array} allEventRectsAtX - All event rectangles at a given x-coordinate.
             * @param {number} goodCount - The count of good events.
             * @param {number} badCount - The count of bad events.
             */
            function generateSummaryRow(allEventRectsAtX, goodCount, badCount) {
                // Clone the template and remove its id
                const newSummaryRow = instance.$CursorLineLegendTemplate
                    .clone()
                    .removeAttr("id");

                // Add class to the new summary row and append it to the holder
                newSummaryRow
                    .addClass(ID.CursorLineLegendClone)
                    .appendTo(instance.$CursorLineLegendHolder);

                // Show the new summary row
                SVG(newSummaryRow[0]).show();

                // Get the last event and its y-axis
                const lastEvent = allEventRectsAtX[allEventRectsAtX.length - 1];
                const lastEvtRowYAxis = parseInt(lastEvent.getAttribute("y"));

                // Calculate the y-axis for the summary row
                const summaryRowYAxis = lastEvtRowYAxis + EventsChart.RowHeight - 1;

                // Transform the new summary row
                newSummaryRow.attr(
                    "transform",
                    `matrix(1, 0, 0, 1, 10, ${summaryRowYAxis})`
                );

                // Get the text element and set its text
                const textElm = newSummaryRow.children("text");
                textElm.text(` Good : ${goodCount}   Bad : ${badCount}`);

                // Set the href for the use element
                newSummaryRow.children("use").attr("xlink:href", ID.CursorLineSumIcon);
            }
        }
    }
}

/**
 * Represents a page header component.
 * This class generates the HTML for a page header and handles its initialization.
 */
class PageHeader {
    // Class properties
    ElementID = "";
    TitleText = "Title Goes Here";
    DescriptionText = "Description Goes Here";
    ImageSrc = "";

    // Constructor to initialize the PageHeader object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.TitleText = element.getAttribute("title-text") || "Title Goes Here";
        this.DescriptionText = element.getAttribute("description-text") || "Description Goes Here";
        this.ImageSrc = element.getAttribute("image-src") || "";

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
      <!-- DESKTOP AND TABLET ONLY -->
     
        <!-- Main container with vertical stacking and margin bottom -->
        <div class="vstack mb-2 d-none d-md-block">
          <!-- Horizontal stacking container -->
          <div class="hstack">
            <!-- Vertical stacking container with gap -->
            <div class="vstack gap-2">
              <!-- Heading -->
              <h1 class="fw-bold">${this.TitleText}</h1>
              <!-- Description -->
              <span style="max-width:509.8px; font-size: 21px; font-weight: lighter; font-family: inherit;">
                ${this.DescriptionText}
              </span>
            </div>
            <!-- Image container for medium and larger screens -->
            <div class="w-100 d-none d-md-block" style="max-width: 412.5px; text-align: center;">
              <img src="${this.ImageSrc}" style="width: 231px;" class="">
            </div>
          </div>
          <!-- Horizontal rule with secondary border and margin top -->
          <hr class="border-secondary border mt-3">
        </div>


      <!-- MOBILE PORTRAIT ONLY -->
      <div class="d-block d-md-none">
        <div class="mt-3 col d-flex align-items-start">
          <div>
            <h3 class="fw-bold mb-0 fs-4 text-body-emphasis" style="position: absolute;">${this.TitleText}</h3>
            <p class="mb-0" style=" font-size: 13px; margin-top: 33px;">${this.DescriptionText}</p>
          </div>
          <img class="bi text-body-secondary flex-shrink-0 mt-3 ms-3" style="width: 157px; align-self: end;" src="${this.ImageSrc}" />
        </div>
        <hr class="border-secondary border mb-4">
      </div>
    `;
    }
}

class PageFooter {
    // Class properties
    ElementID = "";

    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the element by ID
        var element = $(`#${this.ElementID}`);

        // Add a classes to the element
        element.addClass("mb-2 pt-3 border-top");

        // Add inline styles to the element
        element.css({
            "opacity": "0.65",
            "margin-top": "231.7px"
        });

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
            <!-- FOOTER LINE AND SPACING -->
            <div class="d-flex justify-content-between">

                    <div class="">
                        <div class="hstack gap-3" style="font-size:11px !important;">
                            <a href="./TermsOfService.html" class="nav-link p-0 text-body-secondary">Terms</a>
                            <a href="./PrivacyPolicy.html" class="nav-link p-0 text-body-secondary">Privacy</a>
                        </div>
                    </div>

                    <div class="me-md-5">
                        <!--SOCIAL ICONS-->
                        <ul class="list-unstyled d-flex">
                            <li title="Buy us Coffee" class="ms-3"><a target="_blank" class="link-body-emphasis" href="https://ko-fi.com/vedastro"><iconify-icon icon="line-md:coffee-half-empty-twotone-loop" width="24" height="24" ></iconify-icon></a></li>
                            <li title="Updates via Twitter" class="ms-3"><a target="_blank" class="link-body-emphasis" href="https://x.com/_VedAstro"><iconify-icon icon="skill-icons:twitter" width="22" height="22" ></iconify-icon></a></li>
                            <li title="Become a Patron" class="ms-3 d-none d-md-block"><a target="_blank" class="link-body-emphasis" href="https://patreon.com/vedastro"><iconify-icon icon="logos:patreon" width="22" height="22" ></iconify-icon></a></li>
                            <li title="View Awesome Source Code" class="ms-3 d-none d-md-block"><a target="_blank" class="link-body-emphasis" href="https://github.com/VedAstro/VedAstro"><iconify-icon icon="skill-icons:github-dark" width="22" height="22" ></iconify-icon></a></li>
                            <li title="Updates via Instagram" class="ms-3"><a target="_blank" class="link-body-emphasis" href="https://www.instagram.com/_vedastro/"><iconify-icon icon="skill-icons:instagram" width="22" height="22" ></iconify-icon></a></li>
                            <li title="Watch How To Guide" class="ms-3"><a target="_blank" class="link-body-emphasis" href="https://www.youtube.com/@vedastro/videos"><iconify-icon icon="logos:youtube-icon" width="22" height="22" ></iconify-icon></a></li>
                            <li title="Updates via Facebook" class="ms-3 d-none d-md-block"><a target="_blank" class="link-body-emphasis" href="https://www.facebook.com/vedastro.org"><iconify-icon icon="devicon:facebook" width="22" height="22" ></iconify-icon></a></li>
                        </ul>
                    </div>
                </div>
    `;
    }
}

/**
 * Represents a dropdown box for ayanamsa.
 * Generates the HTML & handles auto saving/reading from local storage
 */
class AyanamsaSelectorBox {
    // Class properties
    ElementID = "";
    DefaultAyanamsa = "";

    constructor(elementId, defaultAyanamsa = null) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // If a default Ayanamsa value is provided, assign it to the DefaultAyanamsa property
        if (defaultAyanamsa) {
            this.DefaultAyanamsa = defaultAyanamsa;
        }

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Check local storage if any previously selected ayanamsa values exist, if so select that in html
        // If not, use the default Ayanamsa value
        this.checkLocalStorage();

        // Attach event handler such that if selection is changed it is also saved into local storage for future use
        this.attachEventHandler();

        //initialize help text
        HelpTextIcon.InitAllIn(`#${this.ElementID}`);

    }

    get SelectedAyanamsa() {
        // Get selected ayanamsa value from select element
        return $(`#${this.ElementID} select`).val();
    }

    checkLocalStorage() {
        const selectedAyanamsa = localStorage.getItem('selectedAyanamsa');
        if (selectedAyanamsa) {
            $(`#${this.ElementID} select`).val(selectedAyanamsa);
        } else if (this.DefaultAyanamsa) {
            $(`#${this.ElementID} select`).val(this.DefaultAyanamsa);
        }
    }

    //save ayanamsa to storage for future use
    //also warn users against other ayanamsa but RAMAN
    attachEventHandler() {
        $(`#${this.ElementID} select`).on('change', (e) => {
            const selectedAyanamsa = $(e.target).val();
            if (selectedAyanamsa !== 'RAMAN') {
                Swal.fire({
                    title: 'Not Recommended 😮',
                    html: `${selectedAyanamsa} <strong>not proven accurate</strong> in real world predictions! We recommend <strong>use RAMAN</strong> ayanamsa, it has been proven. Don't just follow the crowd!`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Continue',
                    cancelButtonText: 'Stay with RAMAN'
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.setItem('selectedAyanamsa', selectedAyanamsa);
                    } else {
                        $(`#${this.ElementID} select`).val('RAMAN');
                        localStorage.setItem('selectedAyanamsa', 'RAMAN');
                    }
                });
            } else {
                localStorage.setItem('selectedAyanamsa', selectedAyanamsa);
            }
        });
    }

    // Method to generate the HTML for the page header
    generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
     <label class="input-group-text hstack gap-2">
         <iconify-icon icon="solar:stars-line-broken" width="25" height="25"></iconify-icon>
         Ayanamsa
         <div class="help-text-icon">
            Ayanamsa is the disagreement on the date of a specific star alignment.
            Greek astronomers say it is around 130 BCE, while Indian astronomers say it hundreds of years later (285 CE or 538 CE).
            This difference in timing is due to the procession of the equinoxes.
            As a result, there's a discrepancy of around 23-24 degrees between many astrologers, which is the Ayanamsa.
            If ayanamsa matchs with the prediction text, then prediction will be accurate.
            We use BV Raman's prediction text, thus we use Raman ayanamsa.
         </div>
     </label>
     <select id="SelectedAyanamsa" class="form-select">
         <optgroup label="Easy"><option value="LAHIRI">Lahiri Chitrapaksha</option><option value="KRISHNAMURTI">Krishnamurti KP</option><option value="RAMAN">Raman</option><option value="FAGAN_BRADLEY">Fagan Bradley (Western)</option><option value="J2000">J2000</option><option value="YUKTESHWAR">Yukteshwar</option></optgroup>
         <optgroup label="Advanced"><option value="FAGAN_BRADLEY">FAGAN_BRADLEY</option><option value="LAHIRI">LAHIRI</option><option value="DELUCE">DELUCE</option><option value="RAMAN">RAMAN</option><option value="USHASHASHI">USHASHASHI</option><option value="KRISHNAMURTI">KRISHNAMURTI</option><option value="DJWHAL_KHUL">DJWHAL_KHUL</option><option value="YUKTESHWAR">YUKTESHWAR</option><option value="JN_BHASIN">JN_BHASIN</option><option value="BABYL_KUGLER1">BABYL_KUGLER1</option><option value="BABYL_KUGLER2">BABYL_KUGLER2</option><option value="BABYL_KUGLER3">BABYL_KUGLER3</option><option value="BABYL_HUBER">BABYL_HUBER</option><option value="BABYL_ETPSC">BABYL_ETPSC</option><option value="ALDEBARAN_15TAU">ALDEBARAN_15TAU</option><option value="HIPPARCHOS">HIPPARCHOS</option><option value="SASSANIAN">SASSANIAN</option><option value="GALCENT_0SAG">GALCENT_0SAG</option><option value="J1900">J1900</option><option value="B1950">B1950</option><option value="SURYASIDDHANTA">SURYASIDDHANTA</option><option value="SURYASIDDHANTA_MSUN">SURYASIDDHANTA_MSUN</option><option value="ARYABHATA">ARYABHATA</option><option value="ARYABHATA_MSUN">ARYABHATA_MSUN</option><option value="SS_REVATI">SS_REVATI</option><option value="SS_CITRA">SS_CITRA</option><option value="TRUE_CITRA">TRUE_CITRA</option><option value="TRUE_REVATI">TRUE_REVATI</option><option value="TRUE_PUSHYA">TRUE_PUSHYA</option><option value="GALCENT_RGBRAND">GALCENT_RGBRAND</option><option value="GALEQU_IAU1958">GALEQU_IAU1958</option><option value="GALEQU_TRUE">GALEQU_TRUE</option><option value="GALEQU_MULA">GALEQU_MULA</option><option value="GALALIGN_MARDYKS">GALALIGN_MARDYKS</option><option value="TRUE_MULA">TRUE_MULA</option><option value="GALCENT_MULA_WILHELM">GALCENT_MULA_WILHELM</option><option value="ARYABHATA_522">ARYABHATA_522</option><option value="BABYL_BRITTON">BABYL_BRITTON</option><option value="TRUE_SHEORAN">TRUE_SHEORAN</option><option value="GALCENT_COCHRANE">GALCENT_COCHRANE</option><option value="GALEQU_FIORENZA">GALEQU_FIORENZA</option><option value="VALENS_MOON">VALENS_MOON</option><option value="LAHIRI_1940">LAHIRI_1940</option><option value="LAHIRI_VP285">LAHIRI_VP285</option><option value="KRISHNAMURTI_VP291">KRISHNAMURTI_VP291</option><option value="LAHIRI_ICRC">LAHIRI_ICRC</option></optgroup>
     </select>
    `;
    }
}

/**
 * Represents desktop sidebar component.
 * This class generates the HTML for desktop sidebar and handles its initialization.
 */
class DesktopSidebar {
    // Class properties
    ElementID = "";
    ActiveLinkName = ""; //the link of current page highlighted
    Links = []; // array of link objects

    constructor(elementId, links) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Assign the provided links to the Links property
        this.Links = links;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.ActiveLinkName = element.getAttribute("active-link-name") || "";

        //add in classes to place nicely in page & only show in desktop/large screens
        $(`#${this.ElementID}`).addClass("col-auto align-items-start d-none d-md-block");

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());

        //based on login status hide/show login/logout button
        if (VedAstro.IsGuestUser()) {
            $('#DesktopLoginButton').show();
            $('#DesktopLogoutButton').hide();
        } else {
            $('#DesktopLoginButton').hide();
            $('#DesktopLogoutButton').show();
        }

        // Add the active class to the corresponding a tag (UX: so user knows on which link currently on)
        const activeLink = document.querySelector(`a[href="./${this.ActiveLinkName}.html"]`);
        if (activeLink) {
            activeLink.classList.add("active");
        }

    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Start building the HTML string
        let html = `
        <div class="vstack gap-2 mb-3 rounded-3 border shadow sticky-md-top p-md-3" style="z-index: 100;">
            <!-- DISABLED FOR NOW <input type="text" class="form-control" placeholder="Search..." > -->
        `;

        // Loop through the links and generate the HTML for each one
        this.Links.forEach(link => {
            html += `
            <a href="./${link.url}.html" style="height: 37.1px; width: fit-content; "
               class="btn-sm w-100 hstack gap-2 iconButton btn-outline-primary btn ">
                <iconify-icon icon="${link.icon}" width="25" height="25" ></iconify-icon>
                ${link.text}
            </a>
            `;
        });

        // Add the login and logout buttons
        html += `
            <a id="DesktopLoginButton" href="./Login.html" style="height: 37.1px; width: fit-content; "
               class="btn-sm w-100 hstack gap-2 iconButton btn-warning btn ">
                <iconify-icon icon="mdi:user-circle" width="25" height="25" ></iconify-icon>Log In
            </a>
            <a id="DesktopLogoutButton" onclick="VedAstro.OnClickLogOut()" style="height: 37.1px; width: fit-content; "
               class="btn-sm w-100 hstack gap-2 iconButton btn-outline-primary btn ">
                <iconify-icon icon="bx:log-out" width="25" height="25" ></iconify-icon>Log Out
            </a>
        </div>

        <!-- WEBSITE VERSION STAMP -->
        <div class="sticky-bottom position-fixed mb-3 ms-5" style="color: #8f8f8f; font-size: 9px; z-index: 1;">
            <div onclick="window.location.href='./MadeOnEarth.html'" style="cursor: pointer;" class="hstack gap-1">
                <iconify-icon icon="ion:earth" width="12" height="12" ></iconify-icon>
                <span>Made on Earth</span>
            </div>
            <div class="hstack gap-1">
                <iconify-icon icon="bi:rocket-fill" width="12" height="12" ></iconify-icon>
                <span>10-11-24-stable</span>
            </div>
            <div class="hstack gap-1">
                <iconify-icon icon="material-symbols:copyright-outline" width="12" height="12" ></iconify-icon>
                <span>2014 - 2024 VedAstro</span>
            </div>
            <div style="cursor: pointer;" class="mt-1"><img src="./images/ce-fcc-recycle.svg"></div>
        </div>
        `;

        return html;
    }

}

/**
 * Represents desktop/mobile top navbar component.
 */
class PageTopNavbar {
    // Class properties
    ElementID = "";
    ButtonLinks = [];
    MoreLinks = [];
    MobileLinks = [];
    HeaderName = "";

    constructor(headerName, elementId, buttonLinks, moreLinks, mobileLinks) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
        this.ButtonLinks = buttonLinks;
        this.MoreLinks = moreLinks;
        this.MobileLinks = mobileLinks;
        this.HeaderName = headerName; //visible at mobile top nav only

        // Initialize dark mode library
        const options = {
            mixColor: '#fff', // default: '#fff'
            backgroundColor: '#fff', // default: '#fff'
            buttonColorDark: '#100f2c', // default: '#100f2c'
            buttonColorLight: '#fff', // default: '#fff'
            saveInCookies: true, // default: true,
            autoMatchOsTheme: false // default: true
        };

        // Makes dark mode lib available to events chart viewer via "window"
        window.DarkModeLibInstance = new Darkmode(options);

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());

        // Based on login status hide/show login/logout button
        if (VedAstro.IsGuestUser()) {
            $('#MobileLoginButton').show();
            $('#MobileLogoutButton').hide();
        } else {
            $('#MobileLoginButton').hide();
            $('#MobileLogoutButton').show();
        }

        // Attach handler: Toggle dark mode on button click
        document.getElementById('DarkModeToggleButton').addEventListener('click', () => {
            window.DarkModeLibInstance.toggle();

            // Special for event chart, if exist on page change vis JS for instant correction
            var value = window.DarkModeLibInstance.isActivated() ? "difference" : "normal";
            $('#EventsChartSvgHolder').css('mix-blend-mode', value);
        });

        // Attach event listener for the Settings option
        this.attachSettingsEventListener();
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Generate the HTML for the button links
        let buttonLinksHtml = "";
        this.ButtonLinks.forEach((link) => {
            let targetAttr = link.target ? `target="${link.target}"` : '';
            buttonLinksHtml += `
        <a href="${link.href}" ${targetAttr} style="height: 37.1px; width: fit-content; " class="btn-sm hstack gap-2 iconButton btn-outline-primary btn">
            <iconify-icon icon="${link.icon}" width="25" height="25"></iconify-icon>
            ${link.text}
        </a>
      `;
        });

        // Generate the HTML for the more links
        let moreLinksHtml = "";
        this.MoreLinks.forEach((link) => {
            let targetAttr = link.target ? `target="${link.target}"` : '';
            let iconHtml = link.icon ? `<iconify-icon icon="${link.icon}" width="23" height="23"></iconify-icon>` : '';
            moreLinksHtml += `
        <li><a class="dropdown-item hstack gap-2" href="${link.href}" ${targetAttr}>${iconHtml} ${link.text}</a></li>
        `;
        });

        // Generate the HTML for the mobile links
        let mobileLinksHtml = "";
        this.MobileLinks.forEach((link) => {
            mobileLinksHtml += `
        <li class="nav-item">
          <a class="nav-link active" href="${link.href}">
            <iconify-icon class="align-bottom" icon="${link.icon}" width="28" height="28"></iconify-icon>
            <span class="ms-1 align-middle" style="">${link.text}</span>
          </a>
        </li>
      `;
        });

        // Return the HTML for the top navbar, including conditional blocks for different screen sizes
        return `
      <!-- DESKTOP TOP NAV BAR  -->
      <div style="min-width: 954px;" class="rounded-3 mb-4 p-2 border shadow d-none d-md-flex gap-2 justify-content-between bg-white">
        <!-- NOTE: id of desktop sidebar is hard coded to match -->
        <button onclick="$('#DesktopSidebarHolder').toggleClass('d-md-block');" style="height: 37.1px; width: fit-content; " class="btn-sm  btn-primary btn ">
          <iconify-icon icon="lucide:panel-left-close" width="24" height="24" ></iconify-icon>
        </button>

        <button id="DarkModeToggleButton" style="height: 37.1px; width: fit-content; " class="btn-sm  btn-primary btn me-md-auto">
            <iconify-icon icon="mdi:theme-light-dark" width="25" height="25" ></iconify-icon>
        </button>

        <!-- BUTTON LINKS -->
        ${buttonLinksHtml}

        <!-- MORE LINKS -->
        <div style="" class="dropdown ">
          <button style="height: 37.1px; width: fit-content;" class="btn-sm  dropdown-toggle btn-primary btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
            <iconify-icon icon="ep:guide" width="25" height="25" ></iconify-icon>
          </button>
          <ul style="cursor: pointer; width: 100%;" class="dropdown-menu">
            ${moreLinksHtml}
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item hstack gap-2" href="#"><iconify-icon icon="eos-icons:rotating-gear" width="23" height="23"></iconify-icon> Settings</a></li> <!-- Added Settings option -->
          </ul>
        </div>
      </div>
     
      <!-- MOBILE LINKS -->
      <nav class="p-1 navbar rounded-bottom-4 d-block d-md-none" data-bs-theme="dark" style="background-color: #1877f2 !important; margin-top: -1.5rem !important; margin-left: -0.73rem !important; margin-right: -0.73rem !important;">
        <div class="container-fluid">
          <a class="navbar-brand active" href="/">
            <img src="./images/header-logo.png" style="width: 44px;" class="d-inline-block align-middle">
            <span class="ms-1 align-middle">${this.HeaderName}</span>
          </a>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarToggler" aria-controls="navbarToggler" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="navbar-nav border-opacity-10 border-2 border-top border-white navbar-collapse collapse" id="navbarToggler">
            <ul class="nav nav-fill">
              ${mobileLinksHtml}
                <li class="nav-item" id="MobileLoginButton">
                    <a class="nav-link active" href="./Login.html">
                        <iconify-icon class="align-bottom" icon="mdi:user-circle" width="28" height="28"></iconify-icon>
                        <span class="ms-1 align-middle" style="">Login</span>
                    </a>
                </li>
                <li class="nav-item" id="MobileLogoutButton">
                    <a class="nav-link active" onclick="VedAstro.OnClickLogOut()">
                        <iconify-icon class="align-bottom" icon="bx:log-out" width="28" height="28"></iconify-icon>
                        <span class="ms-1 align-middle" style="">Log Out</span>
                    </a>
                </li>
            </ul>
          </div>
        </div>
      </nav>
    `;
    }

    // Attach event listener for the Settings option
    attachSettingsEventListener() {
        // Get the Settings link
        $(`#${this.ElementID} .dropdown-item:contains('Settings')`).on('click', async () => {
            const currentApiDomain = VedAstro.ApiDomain;

            // Show SweetAlert modal with an additional Reset button
            const { value: newApiDomain, isDenied, isDismissed } = await Swal.fire({
                title: 'API Server',
                input: 'text',
                inputLabel: 'Set the API server used, change to "http://localhost:7071/api" to use your local server',
                inputValue: currentApiDomain,
                showCancelButton: true,
                showDenyButton: true,
                confirmButtonText: 'Save',
                denyButtonText: 'Reset',
                cancelButtonText: 'Cancel',
                inputValidator: (value) => {
                    if (!value) {
                        return 'You need to enter a domain!';
                    }
                }
            });

            // If the user clicked Save
            if (newApiDomain && !isDismissed) {
                // Save new API domain to localStorage
                localStorage.setItem("ApiDomain", newApiDomain);
                Swal.fire('Saved!', 'Your API Domain has been updated.', 'success').then(() => {
                    // Refresh the page
                    location.reload();
                });
            }

            // If the user clicked Reset
            if (isDenied) {
                // Clear the localStorage
                localStorage.removeItem("ApiDomain");
                Swal.fire('Reset!', 'API Domain reset to default.', 'success').then(() => {
                    // Refresh the page
                    location.reload();
                });
            }
        });
    }

}

/**
 * Represents a person selector box component.
 * This class generates the HTML for a dropdown list of people and handles user interactions.
 * It also caches person data and updates the selected person.
 */

class PersonSelectorBox {
    // Class properties
    ElementID = "";
    TitleText = "";
    SelectedPersonNameHolderElementID = "selectedPersonNameHolder";
    SearchInputElementClass = "searchInputElementClass";
    // Default data
    personList = [];
    publicPersonList = [];
    _personListDisplay = [];
    _publicPersonListDisplay = [];
    // Name of key where selected person data for
    // this instance of person selector is stored
    // exp: SelectedPerson-1, SelectedPerson-2, etc... (to support multiple selectors in 1 page)
    SelectedPersonStorageKey = "";

    constructor(elementId) {
        // Initialize class properties
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.TitleText = element.getAttribute("title-text") || "";

        // Created with nonce from 1 to n, so that multiple selectors supported across pages
        this.SelectedPersonStorageKey = `SelectedPerson-${this.ElementID}`;

        // Save a reference to this instance for global access
        this.saveInstanceReference();

        // Initialize the component
        this.init();
    }

    async init() {
        // Show loading spinners as soon as possible
        $(`#${this.ElementID}`).html(`
        <div class="vstack" style="width:255px;">
            <iconify-icon class="align-self-center" icon="svg-spinners:270-ring" width="37" height="37"></iconify-icon>
            <p class="align-self-center m-0">Please wait...</p>
        </div>
        `);

        // Fetch person list data from API or local storage
        await this.initializePersonListData();

        // Inject the component's HTML into the page
        await this.initializeMainBody();
    }

    /**
    * Gets the selected person.
    */
    GetSelectedPerson() {
        try {
            // Get the selected person from session storage (so that unique across tabs)
            const selectedPersonJson = sessionStorage.getItem(this.SelectedPersonStorageKey);

            if (!selectedPersonJson) { return null; }

            // Parse the selected person JSON into a Person object
            const selectedPersonData = JSON.parse(selectedPersonJson);
            const selectedPerson = new Person(selectedPersonData);

            // Return the selected person object
            return selectedPerson;
        } catch (error) {
            // If JSON parsing or object parsing fails, return null quietly
            return null;
        }
    }

    /**
     * Sets the selected person.
     */
    SetSelectedPerson(person) {
        // Save the selected person ID to session storage (so that unique across tabs)
        sessionStorage.setItem(this.SelectedPersonStorageKey, JSON.stringify(person));
    }

    // Save a reference to this instance for global access
    saveInstanceReference() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.PersonSelectorBoxInstances) {
            window.vedastro.PersonSelectorBoxInstances = [];
        }
        window.vedastro.PersonSelectorBoxInstances[this.ElementID] = this;
    }

    async initializeMainBody() {
        // Generate new HTML
        var html = await this.generateHtmlBody();

        // Clean any existing/loading icon just before rendering new 
        $(`#${this.ElementID}`).empty();

        // Inject the HTML into the page
        $(`#${this.ElementID}`).html(html);

        // Add tooltip to show full birth time and location
        this.attachTippyToButton();
    }

    attachTippyToButton() {
        const selectedPerson = this.GetSelectedPerson();
        if (selectedPerson) {
            const button = $(`#${this.ElementID}`).find(`.${this.SelectedPersonNameHolderElementID}`).parent();

            // Location text can sometimes be very long, so auto shorten
            let locationName = CommonTools.TruncateText(selectedPerson.BirthTime.Location.Name, 20);

            let html = `<div>
                            <div>🕑 ${selectedPerson.BirthTime.StdTime}</div>
                            <div>🌍 ${locationName}</div>
                            <div>📌 ${selectedPerson.BirthTime.Location.Latitude}, ${selectedPerson.BirthTime.Location.Longitude}</div>
                        </div>`;

            tippy(button[0], {
                content: html,
                allowHTML: true,
                arrow: true,
                placement: 'right',
                trigger: 'mouseenter focus',
                interactive: true // So that can select button
            });
        }
    }

    // Gets list of person to display (checks if underlying cache has been removed)
    async getPersonListDisplay() {
        // Check if cache exists
        const isExist = VedAstro.getPersonListFromCache('private') !== null;

        // If cache exists, then no need to reinitialize, use in memory
        if (isExist) { return this._personListDisplay; }

        // Else get new data from API and fill from that (as though 1st time load)
        else {
            this.personList = await VedAstro.GetPersonList('private');
            this._personListDisplay = this.personList;
            return this._personListDisplay;
        }
    }

    // Gets public list of person to display (checks if underlying cache has been removed)
    async getPublicPersonListDisplay() {
        // Check if cache exists
        const isExist = VedAstro.getPersonListFromCache('public') !== null;

        // If cache exists, then no need to reinitialize, use in memory
        if (isExist) { return this._publicPersonListDisplay; }

        // Else get new data from API and fill from that (as though 1st time load)
        else {
            this.publicPersonList = await VedAstro.GetPersonList('public');
            this._publicPersonListDisplay = this.publicPersonList;
            return this._publicPersonListDisplay;
        }
    }

    // Fetch list for use in this specific instance
    async initializePersonListData() {
        // Get person list from API or cache automatically
        this.personList = await VedAstro.GetPersonList('private');
        this._personListDisplay = this.personList;
        this.publicPersonList = await VedAstro.GetPersonList('public');
        this._publicPersonListDisplay = this.publicPersonList;

        // Get the previously selected person from local storage
        const selectedPerson = this.GetSelectedPerson();

        // If a selected person exists, simulate a click on their name
        selectedPerson && this.updatePersonNameGui(selectedPerson);
    }

    // Handle click on a person's name in the dropdown (called from HTML dropdown)
    async onClickPersonName(personId) {
        // Get the full person details based on the ID
        var personData = await this.getPersonDataById(personId);

        // Update into view
        this.updatePersonNameGui(personData);

        // Save the selected person to local storage
        this.SetSelectedPerson(personData);

        // Re-attach Tippy to button with new selected person's birth time
        this.attachTippyToButton();
    }

    // Given full person data will update into selected view
    updatePersonNameGui(personData) {
        var displayName = personData.DisplayName;

        // Update the visible select button text
        var buttonTextHolder = $(`#${this.ElementID}`).find(`.${this.SelectedPersonNameHolderElementID}`);
        buttonTextHolder.html(displayName);

        // Save the selected person ID for instance-specific selection
        this.selectedPersonId = personData.PersonId;
    }

    // Handle keyup event on the search input field
    onKeyUpSearchBar = (event) => {
        // Ignore certain keys to prevent unnecessary filtering
        if (["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Space", "ControlLeft", "ControlRight", "AltLeft", "AltRight", "ShiftLeft", "ShiftRight", "Enter", "Tab", "Escape"].includes(event.code)) {
            return;
        }

        // Get the search text from the input field
        const searchText = event.target.value.toLowerCase();

        // Filter only the person items based on the search text
        var allPersonDropItems = $(`#${this.ElementID}`).find('.dropdown-menu li.person-item');
        allPersonDropItems.each(function () {
            const personName = $(this).text().toLowerCase();
            if (personName.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    async generateHtmlBody() {
        // Generate HTML for private and public person lists
        this.personListHTML = await this.generatePersonListHtml();
        this.publicPersonListHTML = await this.generatePublicPersonListHtml();

        // Auto set selected person from previous selection
        let selectedPersonText = 'Select Person'; // Default

        // Check if any person has been selected before (session storage)
        let personFromStorage = JSON.parse(sessionStorage.getItem(this.SelectedPersonStorageKey));
        if (personFromStorage && Object.keys(personFromStorage).length !== 0) {
            let parsedPerson = new Person(personFromStorage);
            selectedPersonText = parsedPerson.DisplayName;
        }

        // Return the generated HTML for the component
        return `
    <div>
      <label class="form-label">${this.TitleText}</label>
      <div class="hstack">
        <div class="btn-group w-auto" style="min-width:231px !important;">
          <button onclick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickDropDown(event)" type="button" class="btn dropdown-toggle btn-outline-primary text-start" data-bs-toggle="dropdown" aria-expanded="false">
            <div class="${this.SelectedPersonNameHolderElementID}" style="cursor: pointer;white-space: nowrap; display: inline-table;" >${selectedPersonText}</div>
          </button>
          <ul class="dropdown-menu ps-2 pe-3" style="height: 412.5px; overflow: clip scroll;">

            <!-- SEARCH INPUT -->
            <div class="hstack gap-2">
              <input onkeyup="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onKeyUpSearchBar(event)" type="text" class="${this.SearchInputElementClass} form-control ms-0 mb-2 ps-3" placeholder="Search...">
              <div class="mb-2" style="cursor: pointer;">
                <iconify-icon icon="mingcute:list-search-fill" width="25" height="25"></iconify-icon>
              </div>
            </div>

            <!-- PRIVATE PERSON LIST -->
            ${this.personListHTML}

            <!-- DIVIDER & EXAMPLES ICON -->
            <li><hr class="dropdown-divider"/></li>
            <div class="ms-3 d-flex justify-content-between">
              <div class="hstack gap-2">
                <div><iconify-icon icon="material-symbols:demography-rounded" width="25" height="25" ></iconify-icon></div>
                <span style="font-size: 13px; color: rgb(143, 143, 143);">Examples</span>
              </div>
            </div>
            <li><hr class="dropdown-divider"/></li>

            <!-- PUBLIC PERSON LIST -->
            ${this.publicPersonListHTML}

          </ul>
        </div>
        <div class="btn-group ">
          <button class="ms-2 px-0 btn btn-primary dropdown-toggle" type="button" style="height:37.1px; width: fit-content; " data-bs-toggle="dropdown" aria-expanded="false">
            <iconify-icon class="" icon="mi:options-horizontal" width="25" height="25" ></iconify-icon>
          </button>
          <ul class="dropdown-menu dropdown-menu-end">
            <!-- NOTE: storage key is inject into URL, so that "add person" page knows where to set, for auto selection on return -->
            <li>
                <a class="dropdown-item gap-2 d-flex align-items-center" href="./AddPerson.html?SelectedPersonStorageKey=${this.SelectedPersonStorageKey}">
                    <iconify-icon class="" icon="ant-design:user-add-outlined" width="25" height="25" ></iconify-icon>
                    Add Person
                </a>
            </li>
            <li>
                <a class="dropdown-item gap-2 d-flex align-items-center" href="./EditPerson.html?SelectedPersonStorageKey=${this.SelectedPersonStorageKey}" onclick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickEditPerson(event)">
                    <iconify-icon class="" icon="uil:edit" width="25" height="25" ></iconify-icon>
                    Edit Person
                </a>
            </li>
            <li>
                <a class="dropdown-item gap-2 d-flex align-items-center" href="./PersonList.html">
                    <iconify-icon class="" icon="line-md:list" width="25" height="25" ></iconify-icon>
                    View All
                </a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  `;
    }

    // Get full person data from the given list based on ID
    async getPersonDataById(personId) {
        // Search in public list first
        const person = (await this.getPublicPersonListDisplay()).find((person) => person.PersonId === personId);

        // If not found, search in private list
        if (!person) {
            const privatePerson = (await this.getPersonListDisplay()).find((person) => person.PersonId === personId);
            return new Person(privatePerson); // Create a Person instance
        }

        return new Person(person); // Create a Person instance
    }

    // Generate HTML for the public person list
    async generatePublicPersonListHtml() {
        const html = (await this.getPublicPersonListDisplay())
            .map((person) => {
                return `<li onClick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')" class="dropdown-item person-item" style="cursor: pointer;">${person.DisplayName}</li>`;
            })
            .join("");

        return html;
    }

    // Generate HTML for the private person list
    async generatePersonListHtml() {
        const html = (await this.getPersonListDisplay())
            .map((person) => {
                return `<li onClick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')" class="dropdown-item person-item" style="cursor: pointer;">${person.DisplayName}</li>`;
            })
            .join("");

        return html;
    }

    // Handle click on the dropdown button
    onClickDropDown(event) {
        // Set focus to the search text box for instant input
        // NOTE: ONLY on desktop, skip for mobile, because keyboard takes screen space
        if (!CommonTools.IsMobile()) {
            $(`#${this.ElementID}`).find(`.${this.SearchInputElementClass}`).focus();
        }
    }

    // Handle click on the "Edit Person" link
    onClickEditPerson(event) {
        const selectedPerson = this.GetSelectedPerson();
        if (!selectedPerson) {
            // Prevent navigation to the edit page
            event.preventDefault();
            // Show message to the user
            Swal.fire('Please select a person first', '', 'warning');
        } else if (selectedPerson.OwnerId === '101') {
            // Prevent navigation to the edit page
            event.preventDefault();
            // Show error message that public profiles can't be edited
            Swal.fire('Public profiles cannot be edited', '', 'error');
        }
    }

    //------------------------------------------------ STATIC FUNCS ----------------------
    // Called by AddPerson to clear the person list cache
    // Call `PersonSelectorBox.ClearPersonListCache('private')` to clear only the private person list cache,
    // `PersonSelectorBox.ClearPersonListCache('public')` to clear only the public person list cache,
    // or `PersonSelectorBox.ClearPersonListCache('all')` to clear both caches. If an invalid type is provided,
    // a warning will be logged to the console and the cache will not be cleared.
    static ClearPersonListCache(type) {
        switch (type) {
            case 'private':
                VedAstro.removePersonListFromCache('private');
                break;
            case 'public':
                VedAstro.removePersonListFromCache('public');
                break;
            case 'all':
                VedAstro.clearAllPersonListCaches();
                break;
            default:
                console.warn('Invalid cache type provided. Cache not cleared.');
        }

        console.log('Person list cache cleared.');
    }
}


/**
 * Represents an info box component.
 * This class generates the HTML for a box displaying information and handles user clicks.
 */
class InfoBox {
    // Class properties
    ElementID = "";
    Title = "Title Goes Here";
    IconName = "fluent-emoji:robot";
    Description = "Description Goes Here";

    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Save a reference to this instance for global access
        InfoBox.initInstances();
        window.vedastro.InfoBoxInstances[elementId] = this;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.Title = element.getAttribute("title") || "Title Goes Here";
        this.Description = element.getAttribute("description") || "Description Goes Here";
        this.IconName = element.getAttribute("iconname") || "fluent-emoji:robot";
        this.ClickUrl = element.getAttribute("ClickUrl") || null;
        this.IsNewTabOpen = element.getAttribute("IsNewTabOpen") || null;

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();

    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());
    }

    // when the `InfoBox` element is clicked, it will check if the `ClickUrl`
    //attribute is present.If it is, it will open the URL in a
    //new tab if the`IsNewTabOpen` attribute is set to`'true'`, otherwise,
    //it will open the URL in the same tab.
    onClick(event) {
        if (this.ClickUrl) {
            if (this.IsNewTabOpen === 'true') {
                window.open(this.ClickUrl, '_blank');
            } else {
                window.location.href = this.ClickUrl;
            }
        }
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
      
<div onClick="window.vedastro.InfoBoxInstances['${this.ElementID}'].onClick(event)" class="" style="cursor: pointer; max-width:365px;">
    <div class="alert alert-primary d-flex align-items-center vstack p-2" role="alert" style="">
        <div class="hstack mb-2">
            <iconify-icon class="bi flex-shrink-0 me-2" icon="${this.IconName}" width="50" height="50"></iconify-icon>
            <div style="line-break: auto;">
                <strong>${this.Title}</strong><br />
                ${this.Description}
            </div>
        </div>
    </div>
</div>

    `;
    }

    static initInstances() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.InfoBoxInstances) {
            window.vedastro.InfoBoxInstances = {};
        }
    }

}

/**
 * Represents an icon button component.
 * This class generates the HTML for a button with an icon and handles user clicks.
 */
class IconButton {
    // Class properties
    ElementID = "";
    SmallSize = false;

    // Constructor to initialize the IconButton object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.SmallSize = element.getAttribute("SmallSize") === "true";
        this.Color = element.getAttribute("Color") || "";
        this.IconName = element.getAttribute("IconName") || "";
        this.ExtraStyle = element.getAttribute("ExtraStyle") || "";
        this.ExtraClass = element.getAttribute("ExtraClass") || "";
        this.ButtonText = element.getAttribute("ButtonText") || "";
        this.OnClickCallback = element.getAttribute("OnClickCallback") || null;

        // Call the method to initialize the button
        this.initializeButton();
    }

    // Method to initialize the button
    async initializeButton() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the button and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlButton());
    }

    // Method to generate the HTML for the button
    async generateHtmlButton() {
        // Return the HTML for the button
        return `
      <button onclick="${this.OnClickCallback}" style="${this.ExtraStyle} justify-content: center; height:37.1px; width: fit-content; " class="${this.ExtraClass} btn-sm hstack gap-2 iconButton btn-${this.Color} btn">
        <iconify-icon icon="${this.IconName}" width="25" height="25"></iconify-icon>
        ${this.ButtonText}
      </button>
    `;
    }
}

/**
 * Represents a time input simple component.
 * This class generates the HTML for a time input field and handles user interactions.
 * It also initializes a calendar picker and updates the input field values.
 */
class TimeInputSimple {
    // Class properties
    ElementID = "";
    TimeInputHolderID = "";
    CalendarPickerHolderID = "";
    HourInputID = "";
    MinuteInputID = "";
    MeridianInputID = "";
    DateInputID = "";
    MonthInputID = "";
    YearInputID = "";

    // Default values
    hour = "00";
    minute = "00";
    meridian = new Date().getHours() < 12 ? "AM" : "PM";
    date = new Date().getDate().toString().padStart(2, '0');
    month = (new Date().getMonth() + 1).toString().padStart(2, '0');
    year = new Date().getFullYear().toString();

    // Constructor to initialize the TimeInputSimple object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Generate unique IDs for the time input holder and calendar picker holder
        this.TimeInputHolderID = `${elementId}_TimeInputHolder`;
        this.CalendarPickerHolderID = `${elementId}_CalendarPickerHolder`;

        // Generate unique IDs for the hour, minute, meridian, date, month, and year inputs
        this.HourInputID = `${elementId}_HourInput`;
        this.MinuteInputID = `${elementId}_MinuteInput`;
        this.MeridianInputID = `${elementId}_MeridianInput`;
        this.DateInputID = `${elementId}_DateInput`;
        this.MonthInputID = `${elementId}_MonthInput`;
        this.YearInputID = `${elementId}_YearInput`;

        // Initialize the TimeInputSimple object
        TimeInputSimple.initInstances();

        // Save a reference to this instance for global access
        this.saveInstanceReference();

        // Call the method to create the instance
        this.initialize();
    }

    // Method to initialize the time location input
    initialize() {
        // Get the element with the given ID
        const element = document.getElementById(this.ElementID);

        // Get the label text from the element's attribute
        const labelText = element.getAttribute("LabelText");

        // Generate the HTML for the time location input and inject it into the element
        element.innerHTML = this.generateHtml(labelText);
    }

    // Method to generate the HTML for the time location input
    generateHtml(labelText) {
        //language=html
        var outputHtml = `
    <style>
      /*to fix time input style in JS date picker*/
      #${this.CalendarPickerHolderID} input {
        border: 0;
        background-color: #f7f7f7;
      }

      /*to fix time input style in JS date picker*/
      .vanilla-calendar-time__content button {
        margin-top: -2px;
        font-size: 19px;
        border: 0;
        color: black;
      }

      #${this.TimeInputHolderID} {
        text-align-last: center;
        cursor: pointer;
        font-size: 18px;
      }

      .inputPerfect {
        cursor: pointer;
        font-weight: 600;
        background: transparent;
        font-size: 18px;
      }
    </style>

    <div class="input-group">

      <span class="input-group-text gap-2 py-1">
        <iconify-icon icon="noto-v1:timer-clock" width="30" height="30"></iconify-icon>
        ${labelText}
      </span>
      <div class="form-control py-2" >
        <!-- note : on click will toggle picker, so picker cannot be inside TimeInputHolder -->
        <div id="${this.TimeInputHolderID}" onclick="window.vedastro.TimeInputSimpleInstances['${this.ElementID}'].onClickDateTimeInput()" class="d-flex justify-content-between" style="text-wrap: nowrap; overflow: hidden;">
          <div class="hstack">
            <span class="border-0 inputPerfect" id="${this.HourInputID}" style="width: 33px;" >${this.hour}</span>:
            <span class="border-0 inputPerfect" id="${this.MinuteInputID}" style="width: 33px;" >${this.minute}</span>
            <span class="border-0 inputPerfect" id="${this.MeridianInputID}" style="width: 37px;" >${this.meridian}</span>
          </div>
          <div class="hstack">
            <span class="border-0 inputPerfect" id="${this.DateInputID}" style="width: 33px;" >${this.date}</span>/
            <span class="border-0 inputPerfect" id="${this.MonthInputID}" style="width: 33px;" >${this.month}</span>/
            <span class="border-0 inputPerfect" id="${this.YearInputID}" style="width: 56px;" >${this.year}</span>
          </div>
        </div>
        <!-- this is where js date picker will be created -->
        <div class="mt-2 vanilla-calendar visually-hidden border border-primary"
             style="position: absolute; z-index: 999; background: #f7f7f7;"
             id="${this.CalendarPickerHolderID}" />
      </div>
    </div>
  `;

        return outputHtml;
    }

    // Method to handle click on date time input
    onClickDateTimeInput() {
        // Get the latest values using JS
        const hour = document.getElementById(this.HourInputID).innerText;
        const minute = document.getElementById(this.MinuteInputID).innerText;
        const meridian = document.getElementById(this.MeridianInputID).innerText;
        const date = document.getElementById(this.DateInputID).innerText;
        const month = document.getElementById(this.MonthInputID).innerText;
        const year = document.getElementById(this.YearInputID).innerText;

        // Initialize picker with needed values
        this.initCalendar(hour, minute, meridian, date, month, year);

        // Toggle picker
        const calendarPickerHolder = document.getElementById(this.CalendarPickerHolderID);
        calendarPickerHolder.classList.toggle('visually-hidden');
    }

    // Initialize the TimeInputSimpleInstances object
    static initInstances() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.TimeInputSimpleInstances) {
            window.vedastro.TimeInputSimpleInstances = {};
        }
    }

    // Save a reference to this instance for global access
    saveInstanceReference() {
        window.vedastro.TimeInputSimpleInstances[this.ElementID] = this;
    }

    // Method to initialize the calendar
    initCalendar(hour, minute, meridian, date, month, year) {
        // Get the calendar picker holder element
        const calendarPickerHolder = document.getElementById(this.CalendarPickerHolderID);

        // Create a new VanillaCalendar instance
        this.calendar = new VanillaCalendar(`#${this.CalendarPickerHolderID}`, {
            // Options
            date: {
                //set the date to show when calendar opens
                today: new Date(`${year}-${month}-${date}`),
            },
            settings: {
                range: {
                    min: '0001-01-01',
                    max: '9999-01-01'
                },
                selection: {
                    time: 12, //AM/PM format
                },
                selected: {
                    //set the time to show when calendar opens
                    time: `${hour}:${minute} ${meridian}`,
                },
            },
            //this is where time is sent back to blazor, by setting straight to dom
            actions: {
                changeTime: (e, time, hours, minutes, keeping) => {
                    document.getElementById(this.HourInputID).innerText = hours;
                    document.getElementById(this.MinuteInputID).innerText = minutes;
                    document.getElementById(this.MeridianInputID).innerText = keeping;
                },
                clickDay: (e, dates) => {
                    //if date selected, hide date picker
                    if (dates[0]) {
                        calendarPickerHolder.classList.add('visually-hidden');
                    }

                    //check needed because random clicks get through
                    if (dates[0] !== undefined) {
                        //format the selected date for blazor
                        const choppedTimeData = dates[0].split("-");
                        var year = choppedTimeData[0];
                        var month = choppedTimeData[1];
                        var day = choppedTimeData[2];

                        //inject the values into the text input
                        document.getElementById(this.DateInputID).innerText = day;
                        document.getElementById(this.MonthInputID).innerText = month;
                        document.getElementById(this.YearInputID).innerText = year;
                    }

                },
                //update year & month immediately even though not yet click date
                //allows user to change only month or year
                clickMonth: (e, month) => {
                    month = month + 1; //correction for JS lib bug
                    var with0 = ('0' + month).slice(-2);//change 9 to 09
                    document.getElementById(this.MonthInputID).innerText = with0;
                },
                clickYear: (e, year) => {
                    document.getElementById(this.YearInputID).innerText = year;
                }
            },
        });

        // Initialize the calendar
        this.calendar.init();

        document.addEventListener('click', (e) => this.autoHidePicker(e), { capture: true });
    }

    //if click is outside picker & input then hide it & let others know
    autoHidePicker(e) {
        //check if click was outside input
        const pickerHolder = e.target.closest(`#${this.CalendarPickerHolderID}`);
        const timeInput = e.target.closest(`#${this.TimeInputHolderID}`);

        //if click is not on either inputs then hide picker
        if (!(timeInput || pickerHolder)) {
            document.getElementById(this.CalendarPickerHolderID).classList.add('visually-hidden');

            //let others know time set updated
            $(document).trigger('timeUpdated');
        }
    }

    isValid() {
        // Check if all fields have been filled
        return this.hour !== "" && this.minute !== "" && this.meridian !== "" && this.date !== "" && this.month !== "" && this.year !== "";
    }

    getInputDateTime() {
        // Get the time values from the input fields
        let hour = document.getElementById(this.HourInputID).innerText;
        let minute = document.getElementById(this.MinuteInputID).innerText;
        let meridian = document.getElementById(this.MeridianInputID).innerText;
        let date = document.getElementById(this.DateInputID).innerText;
        let month = document.getElementById(this.MonthInputID).innerText;
        let year = document.getElementById(this.YearInputID).innerText;

        //convert hour and minute from 12H to 24H
        let hour24 = hour;
        if (meridian === 'PM' && hour !== '12') {
            hour24 = parseInt(hour) + 12;
        } else if (meridian === 'AM' && hour === '12') {
            hour24 = '00';
        }

        //fix formatting to include 0 in front if single digit
        hour24 = hour24.toString().padStart(2, '0');
        minute = minute.toString().padStart(2, '0');
        date = date.toString().padStart(2, '0');
        month = month.toString().padStart(2, '0');

        //pack data into object
        return {
            "Hour24": hour24,
            "Minute": minute,
            "Date": date,
            "Month": month,
            "Year": year
        };
    }

    /**
     * Sets the input fields with the provided time data.
     * @param {Time} timeData - The time data to set.
     */
    setInputDateTime(timeData) {
        const stdTime = timeData.StdTime; // "13:54 25/10/1992 +08:00"
        const [timePart, datePart, timezone] = stdTime.split(' ');
        const [hour24, minute] = timePart.split(':');
        const [day, month, year] = datePart.split('/');

        // Convert 24-hour format to 12-hour format
        let hour = parseInt(hour24);
        let meridian = 'AM';
        if (hour >= 12) {
            meridian = 'PM';
            if (hour > 12) {
                hour -= 12;
            }
        }
        if (hour === 0) {
            hour = 12;
        }
        hour = hour.toString().padStart(2, '0');

        // Set values into the inputs
        document.getElementById(this.HourInputID).innerText = hour;
        document.getElementById(this.MinuteInputID).innerText = minute;
        document.getElementById(this.MeridianInputID).innerText = meridian;
        document.getElementById(this.DateInputID).innerText = day;
        document.getElementById(this.MonthInputID).innerText = month;
        document.getElementById(this.YearInputID).innerText = year;
    }

}

/**
 * Represents a geo location input component.
 * This class generates the HTML for a location input field and handles user interactions.
 * It also toggles between location name and latitude/longitude input fields.
 */
class GeoLocationInput {
    // Class properties
    ElementID = "";
    labelText = "";
    dropdownMenuId = "";
    locationNameInputId = "";
    locations = []; // Save the parsed geolocation array in the class instance
    $element = null;
    $locationNameInput = null;
    $dropdownMenu = null;
    $latitudeInput = null;
    $longitudeInput = null;
    $switchButton = null;

    /**
     * Constructor to initialize the GeoLocationInput object.
     * @param {string} ElementId - The ID of the HTML element to render the component in.
     */
    constructor(elementId) {
        // Assign the provided elementId to the elementId property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        this.$element = $(`[id="${this.ElementID}"]`);

        // Get the custom attributes from the element and assign default values if not present
        this.labelText = this.$element.attr("LabelText") || "Location";

        // Generate a random ID for the dropdown menu
        this.dropdownMenuId = `dropdown-menu-${Math.random().toString(36).substr(2, 9)}`;
        this.locationNameInputId = `location-name-input-${Math.random().toString(36).substr(2, 9)}`;

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    /**
     * Method to initialize the main body of the page header.
     */
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        this.$element.empty();

        // Generate the HTML for the page header and inject it into the element
        this.$element.html(await this.generateHtmlBody());

        // Get the input field and dropdown menu elements
        this.$locationNameInput = this.$element.find(`[id="${this.locationNameInputId}"]`);
        this.$dropdownMenu = this.$element.find(`[id="${this.dropdownMenuId}"]`);
        this.$latitudeInput = this.$element.find(".latitude");
        this.$longitudeInput = this.$element.find(".longitude");
        this.$switchButton = this.$element.find(".switch-button");

        // Add event listeners
        this.attachEventHandlers();

    }

    attachEventHandlers() {

        //user searches for location
        this.$locationNameInput.on("keyup", (event) => this.onUpdateLocationNameText(event));

        //show drop down with location names or tell user location not found
        this.$locationNameInput.on("focus", (event) => this.onInputFocus(event));

        //user clicks on dropdown location name
        this.$dropdownMenu.on("click", ".dropdown-item", (event) => this.onClickPresetLocationName(event));

        //user pastes location name and moves out, so parse to default possible location
        this.$locationNameInput.on("blur", (event) => {
            setTimeout(() => {
                if (!this.isDropdownClick) {
                    this.onLeaveLocationInput();
                }
            }, 100); // Delay the execution so that "dropdown" click has time to fill if user clicked on it 1st
        });


        //user types in coordinates without location name, so parse to default possible location name
        this.$latitudeInput.on("blur", (event) => this.onLeaveCoordinateInput());
        this.$longitudeInput.on("blur", (event) => this.onLeaveCoordinateInput());

        //users switches between name / coordinate input
        this.$switchButton.on("click", () => this.toggleInputFields());


    }

    async onLeaveLocationInput() {
        // Get the unparsed location name
        let unparsedLocationName = this.$locationNameInput.val().trim();

        // Check if the input field is not empty
        // & has not been filled by dropdown click (only possible because handler is fired with delay)
        var isNotFilledByDropdown = !(this.isValid());
        if (unparsedLocationName !== "" && isNotFilledByDropdown) {
            // Call API and get parsed location
            var apiUrl = `${VedAstro.ApiDomain}/Calculate/AddressToGeoLocation/Address/${unparsedLocationName}`;

            const response = await fetch(apiUrl);
            const data = await response.json();
            const location = data.Payload.AddressToGeoLocation;

            //if location not found let user know invalid location name
            if (location.Name === 'Empty') {
                Swal.fire({ icon: "error", title: "Location not found!", html: `The location <strong>"${unparsedLocationName}"</strong> not found!`, confirmButtonText: "OK" });
                //clear coordinates input to raise invalid alarm
                this.$latitudeInput.empty();
                this.$longitudeInput.empty();
            } else {
                //location found, fill inputs
                this.$locationNameInput.val(location.Name);
                this.$latitudeInput.val(location.Latitude.toFixed(1)); // round for nice fit GUI
                this.$longitudeInput.val(location.Longitude.toFixed(1)); // round for nice fit GUI

                //let others know location set successfully
                $(document).trigger('locationUpdated');

            }
        }

    }

    //when user types in coordinates, get the location name from API and fill that
    //overrides what ever user typed in location name, since now in coordinates view/mode
    async onLeaveCoordinateInput() {
        // Get typed in coordinates
        const latitude = this.$latitudeInput.val().trim();
        const longitude = this.$longitudeInput.val().trim();

        //only continue if both coordinates is filled
        if (latitude === "" || longitude === "") { return; }

        // Call API and get parsed location name
        let apiUrl = `${VedAstro.ApiDomain}/Calculate/CoordinatesToGeoLocation/Latitude/${latitude}/Longitude/${longitude}`;

        const response = await fetch(apiUrl);
        const data = await response.json();
        const location = data.Payload.CoordinatesToGeoLocation;

        //if location not found let user know invalid location name
        if (location.Name === 'Empty') {
            Swal.fire({ icon: "error", title: "Location not found!", html: `No location for <strong>Lat:"${latitude}" & Long:"${longitude}"</strong>`, confirmButtonText: "OK" });
            //clear location name input to raise invalid alarm
            this.$locationNameInput.empty();
        } else {
            //location found, fill inputs
            this.$locationNameInput.val(location.Name);
            this.$latitudeInput.val(location.Latitude.toFixed(1)); // round for nice fit GUI
            this.$longitudeInput.val(location.Longitude.toFixed(1)); // round for nice fit GUI

            //let others know location set successfully
            $(document).trigger('locationUpdated');
        }
    }

    /**
     * Method to generate Bootstrap 5 HTML that shows base GUI
     */
    async generateHtmlBody() {
        return `
            <div class="hstack gap-1">
                <!-- Location name input with auto dropdown -->
                <div class="input-group location-name">
                    <!-- HEADER ICON -->
                    <span class="input-group-text gap-2 py-1">
                        <iconify-icon icon="streamline-emojis:globe-showing-americas" width="34" height="34"></iconify-icon>
                        ${this.labelText}
                    </span>
                    <input id="${this.locationNameInputId}" type="text" class="form-control " placeholder="New York" style="font-weight: 600; font-size: 16px;" data-bs-toggle="dropdown">
                    <ul id="${this.dropdownMenuId}" class="dropdown-menu" aria-labelledby="${this.locationNameInputId}">
                        <li><a class="dropdown-item text-muted" href="#">
                            <svg xmlns="http://www.w3.org/2000/svg" aria-hidden="true" role="img" width="18" height="18" viewBox="0 0 24 24" data-icon="grommet-icons:map" data-width="18" class="iconify iconify--grommet-icons"><path fill="none" stroke="currentColor" stroke-width="2" d="M15 15h4l3 7H2l3-7h4m4-7a1 1 0 1 1-2 0a1 1 0 0 1 2 0M6 8c0 5 6 10 6 10s6-5 6-10c0-3.417-2.686-6-6-6S6 4.583 6 8Z"></path></svg>
                            Search city, town, state</a>
                        </li>
                        <!-- DYNAMICLY GENERATED -->
                    </ul>
                </div>


                <!-- Latitude & long input -->
                <div class="input-group d-none lat-lng-fields">
                    <!-- HEADER ICON -->
                    <span class="input-group-text gap-2 py-1">
                        <iconify-icon icon="streamline-emojis:globe-showing-americas" width="34" height="34"></iconify-icon>
                        ${this.labelText}
                    </span>
                    <span class="input-group-text px-2">Lat</span>
                    <input type="number" class="form-control px-2 latitude" placeholder="4.3°" style="font-weight: 600; font-size: 16px;">
                    <span class="input-group-text px-2">Long</span>
                    <input type="number" class="form-control px-2 longitude" placeholder="101.4°" style="font-weight: 600; font-size: 16px;">
                </div>

                <!-- Input Swither button -->
                <button class="switch-button btn-primary btn py-1" style="padding-right: 2px;padding-left: 3px;">
                    <iconify-icon class="pt-1 globeIcon" icon="bx:globe" width="25" height="25"></iconify-icon>
                    <iconify-icon class="pt-1 mapIcon d-none" icon="bx:map" width="25" height="25"></iconify-icon>
                </button>
            </div>
        `;
    }

    /**
     * Method to handle click on preset location name.
     * @param {Event} event - The event object triggered by the click.
     */
    onClickPresetLocationName(event) {
        // Get the location name from the clicked element
        const locationName = event.target.textContent;

        // Find the corresponding location object from the saved instance
        const location = this.locations.find(location => location.Name === locationName);

        if (location) {
            // Fill location name, longitude and latitude values into HTML
            this.$locationNameInput.val(location.Name);
            this.$latitudeInput.val(location.Latitude.toFixed(1)); //round for nice fit GUI
            this.$longitudeInput.val(location.Longitude.toFixed(1)); //round for nice fit GUI
        }
    }

    /**
     * Method to update the location name text.
     * @param {Event} event - The event object triggered by the input field.
     */
    onUpdateLocationNameText(event) {
        // Get the user input from the event target
        const userTextInput = event.target.value;

        // Call the API to search for location names based on the user input
        this.locationNameSearchWithAPI(userTextInput)
            .then(locations => {
                // Clear any existing content in the dropdown menu
                this.$dropdownMenu.empty();

                if (locations.length === 0) {
                    // if no locations are found then only insert below html to notify user no location with that name,
                    // but input must have text, else show `search message` instead
                    if (userTextInput.trim() !== "") {
                        this.$dropdownMenu.html(`
                            <li><a class="dropdown-item text-muted" href="#">
                                <iconify-icon icon="tdesign:map-cancel" width="18" height="18"></iconify-icon>
                                Not found, try input coordinates</a>
                            </li>
                        `);
                    } else {
                        this.$dropdownMenu.html(`
                            <li><a class="dropdown-item text-muted" href="#">
                                <iconify-icon icon="grommet-icons:map" width="18" height="18"></iconify-icon>
                                Search city, town, state</a>
                            </li>
                        `);
                    }
                } else {
                    // Generate HTML for the locations
                    const locationsHtml = locations.map(location => `
                        <li><a class="dropdown-item">${location.Name}</a></li>
                    `).join("");
                    this.$dropdownMenu.html(locationsHtml);
                }

                // Show the dropdown menu
                this.$dropdownMenu.removeClass("d-none");
            });
    }

    onInputFocus() {
        // Remove the 'd-none' class from the dropdown menu, which makes it visible
        this.$dropdownMenu.removeClass("d-none");
    }

    /**
     * Method to toggle the input fields.
     */
    toggleInputFields() {
        this.$element.find(".location-name").toggleClass('d-none');
        this.$element.find(".lat-lng-fields").toggleClass('d-none');

        // toggle button icons based on class
        if (this.$switchButton.find('.globeIcon').hasClass('d-none')) {
            this.$switchButton.find('.mapIcon').addClass('d-none');
            this.$switchButton.find('.globeIcon').removeClass('d-none');
        } else {
            this.$switchButton.find('.globeIcon').addClass('d-none');
            this.$switchButton.find('.mapIcon').removeClass('d-none');
        }
    }

    /**
     * Method to search for location names using an API.
     * @param {string} locationName - The location name to search for.
     */
    locationNameSearchWithAPI(locationName) {
        return new Promise(resolve => {
            fetch(`${VedAstro.ApiDomain}/Calculate/SearchLocation/Address/${locationName}`)
                .then(response => response.json())
                .then(data => {
                    if (data.Status === 'Pass' && data.Payload.SearchLocation) {
                        this.locations = data.Payload.SearchLocation.map(location => new GeoLocation(location));
                        resolve(this.locations);
                    } else {
                        this.locations = [];
                        resolve([]); // Return an empty array if no results or error
                    }
                })
                .catch(error => {
                    console.error('Error searching for location:', error);
                    this.locations = [];
                    resolve([]); // Return an empty array if an error occurred
                });
        });
    }

    isValid() {
        const locationName = this.$locationNameInput.val();
        const latitude = this.$latitudeInput.val();
        const longitude = this.$longitudeInput.val();

        return locationName !== "" && latitude !== "" && longitude !== "";
    }

    getInputLocation() {
        // Get the location values from the input fields
        const locationName = document.querySelector(`#${this.ElementID} .location-name input`).value;
        const latitude = document.querySelector(`#${this.ElementID} .latitude`).value;
        const longitude = document.querySelector(`#${this.ElementID} .longitude`).value;

        return {
            "Name": locationName,
            "Latitude": latitude,
            "Longitude": longitude
        };

    }

    /**
     * Sets the input fields with the provided location data.
     * @param {GeoLocation} locationData - The location data to set.
     */
    setInputLocation(locationData) {
        // Set values into inputs
        this.$locationNameInput.val(locationData.Name);
        this.$latitudeInput.val(locationData.Latitude);
        this.$longitudeInput.val(locationData.Longitude);
    }

}

/**
 * Represents a time location input component.
 * This class generates the HTML for a time and location input field and handles user interactions.
 * It also initializes a time input simple and geo location input components.
 */
class TimeLocationInput {
    // Class properties
    ElementID;
    LabelText;
    TimeInputSimpleID;
    GeoLocationInputID;
    TimeInputSimpleInstance;
    GeoLocationInputInstance;
    TimezoneOffsetInputID;
    TimezoneOffsetInputHolderID;
    userModifiedTimezoneOffset = false; // Flag to track if user modified the timezone offset

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.LabelText = element.getAttribute("LabelText") || "";

        // Generate a random ID for TimeInputSimple
        var randoTron = Math.random().toString(36).substr(2, 9);
        this.TimeInputSimpleID = `TimeInputSimpleID-${randoTron}`;
        this.GeoLocationInputID = `GeoLocationInputID-${randoTron}`;
        this.TimezoneOffsetInputID = `TimezoneOffsetInputID-${randoTron}`;
        this.TimezoneOffsetInputHolderID = `TimezoneOffsetInputHolderID-${randoTron}`; //to hide/show

        // Initialize the main body
        this.initializeMainBody();
    }

    // Method to initialize the main body
    initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Render subview components now that the base HTML is in the DOM
        this.TimeInputSimpleInstance = new TimeInputSimple(this.TimeInputSimpleID);
        this.GeoLocationInputInstance = new GeoLocationInput(this.GeoLocationInputID);

        // Attach event handlers
        this.attachEventHandlers();
    }

    attachEventHandlers() {
        // When time or location input is updated, update timezone offset accurately
        $(document).on('locationUpdated', (event) => this.updateTimezoneOffset());
        $(document).on('timeUpdated', (event) => this.updateTimezoneOffset());

        // Attach event listener to detect if the user modifies the timezone offset input field
        $(`#${this.TimezoneOffsetInputID}`).on('input', (event) => {
            this.userModifiedTimezoneOffset = true;
        });
    }

    // When time or location input is updated, update timezone offset accurately
    async updateTimezoneOffset() {
        // Make sure both time & location are valid (filled)
        var timeIsValid = this.TimeInputSimpleInstance.isValid();
        var locationIsValid = this.GeoLocationInputInstance.isValid();

        // If both are filled, then update timezone input using API
        if (timeIsValid && locationIsValid) {
            // Get the inputted location & time data
            const inputTime = this.TimeInputSimpleInstance.getInputDateTime();
            const inputLocation = this.GeoLocationInputInstance.getInputLocation();

            // Call API to get exact timezone for location at given time
            var timeZone = await this.getTimezoneForLocationFromApi(
                inputLocation.Name,
                inputLocation.Latitude,
                inputLocation.Longitude,
                inputTime.Hour24,
                inputTime.Minute,
                inputTime.Date,
                inputTime.Month,
                inputTime.Year
            ); // Format: +08:00

            // Inject correct timezone into view
            $(`#${this.TimezoneOffsetInputID}`).val(timeZone);

            // Since we updated the value programmatically, reset the flag
            this.userModifiedTimezoneOffset = false;
        }
    }

    // Method to generate the HTML for the page header
    generateHtmlBody() {
        return `
            <div id="${this.TimeInputSimpleID}" LabelText="Time"></div>
            <!-- Timezone Offset (advanced menu) -->
            <div id="${this.TimezoneOffsetInputHolderID}" style="display:none;">
                <div class="input-group mt-3">
                    <span class="input-group-text gap-2 py-1" style="width: 136px;">
                        <iconify-icon icon="stash:globe-timezone-light" width="35" height="35"></iconify-icon>Timezone
                    </span>
                    <input id="${this.TimezoneOffsetInputID}" type="text" class="form-control" placeholder="+00:00" style="font-weight: 600; font-size: 16px;">
                </div>
            </div>

            <div id="${this.GeoLocationInputID}" class="mt-3" LabelText="Map"></div>
        `;
    }

    /**
     * Method to get the time and location as a JSON object.
     * Example output: {"StdTime":"13:54 25/10/1992 +08:00","Location":{"Name":"Taiping","Longitude":103.82,"Latitude":1.352}}
     */
    async getTimeJson() {
        // Get the inputted location & time data
        const inputTime = this.TimeInputSimpleInstance.getInputDateTime();
        const inputLocation = this.GeoLocationInputInstance.getInputLocation();

        // Construct the StdTime string in the format "HH:MM DD/MM/YYYY tmz"
        let timeZone;

        // Check if the user modified the timezone offset 
        // TODO NOTE: server does not respect inputed timezone
        if (this.userModifiedTimezoneOffset) {
            timeZone = $(`#${this.TimezoneOffsetInputID}`).val(); // Get the value from the input field
        } else {
            // Call API to get exact timezone for location at given time
            timeZone = await this.getTimezoneForLocationFromApi(
                inputLocation.Name,
                inputLocation.Latitude,
                inputLocation.Longitude,
                inputTime.Hour24,
                inputTime.Minute,
                inputTime.Date,
                inputTime.Month,
                inputTime.Year
            ); // Format: +08:00
        }

        const stdTime = `${inputTime.Hour24}:${inputTime.Minute} ${inputTime.Date}/${inputTime.Month}/${inputTime.Year} ${timeZone}`;

        // Construct the timeObject with StdTime and Location properties
        const timeObject = {
            StdTime: stdTime,
            Location: inputLocation
        };

        // Return the timeObject
        return timeObject;
    }

    async getTimezoneForLocationFromApi(locationName, latitude, longitude, hour, minute, date, month, year) {
        // Construct API URL
        const apiUrl = `${VedAstro.ApiDomain}/Calculate/GeoLocationToTimezone/Location/${encodeURIComponent(locationName)}/Coordinates/${latitude},${longitude}/Time/${hour}:${minute}/${date}/${month}/${year}/+00:00`;

        // Make API call and handle response
        const response = await fetch(apiUrl);

        const data = await response.json();
        if (data.Status === "Pass") {
            return data.Payload.GeoLocationToTimezone;
        } else {
            // Handle error, notify the user
            Swal.fire({
                icon: 'error',
                title: 'Auto detect timezone failed!',
                text: 'Could not detect accurate timezone for given location & time. Try input manually.'
            });

            return "+00:00"; // Default to UTC if API call fails
        }
    }

    static getSystemTimezone() {
        const date = new Date();
        const timezoneOffset = date.getTimezoneOffset();
        const hours = Math.floor(Math.abs(timezoneOffset) / 60);
        const minutes = Math.abs(timezoneOffset) % 60;
        return (timezoneOffset <= 0 ? '+' : '-') + String(hours).padStart(2, '0') + ":" + String(minutes).padStart(2, '0');
    }

    getDateTimeOffset() {
        // Get the instances of the TimeInputSimple and GeoLocationInput classes
        const timeInputSimple = this.TimeInputSimpleInstance;
        const geoLocationInput = this.GeoLocationInputInstance;

        // Get the time values from the input fields
        const hour = document.getElementById(timeInputSimple.HourInputID).innerText;
        const minute = document.getElementById(timeInputSimple.MinuteInputID).innerText;
        const meridian = document.getElementById(timeInputSimple.MeridianInputID).innerText;
        const date = document.getElementById(timeInputSimple.DateInputID).innerText;
        const month = document.getElementById(timeInputSimple.MonthInputID).innerText;
        const year = document.getElementById(timeInputSimple.YearInputID).innerText;

        // Get the location values from the input fields
        const locationName = document.querySelector(`#${geoLocationInput.ElementID} .location-name input`).value;
        const latitude = document.querySelector(`#${geoLocationInput.ElementID} .latitude`).value;
        const longitude = document.querySelector(`#${geoLocationInput.ElementID} .longitude`).value;

        // Construct the DateTime object
        const dateTime = {
            year: parseInt(year),
            month: parseInt(month) - 1, // Month is zero-based, so subtract 1
            date: parseInt(date),
            hour: parseInt(hour),
            minute: parseInt(minute),
            meridian: meridian,
            location: {
                name: locationName,
                latitude: parseFloat(latitude),
                longitude: parseFloat(longitude),
            },
        };

        // Return the DateTime object
        return dateTime;
    }

    isValid() {
        // Get the time and location input fields
        const timeInputSimple = this.TimeInputSimpleInstance;
        const geoLocationInput = this.GeoLocationInputInstance;

        // Check if all fields have been filled
        if (timeInputSimple.isValid() && geoLocationInput.isValid()) {
            return true;
        }
        return false;
    }

    /**
     * Sets the input fields with the provided birth time data.
     * @param {Time} birthTime - The birth time data to set.
     */
    setInputDateTime(birthTime) {
        this.TimeInputSimpleInstance.setInputDateTime(birthTime);
        this.GeoLocationInputInstance.setInputLocation(birthTime.Location);

        // Extract the timezone offset from birthTime.StdTime
        const stdTime = birthTime.StdTime; // "13:54 25/10/1992 +08:00"
        const parts = stdTime.split(' ');
        const timezoneOffset = parts[2]; // "+08:00"

        // Set the timezone offset in the input field
        $(`#${this.TimezoneOffsetInputID}`).val(timezoneOffset);
    }

}

class AshtakvargaTable {
    constructor(rawSettings) {
        //correct if property names is camel case (for Blazor)
        var settings = CommonTools.CamelCaseKeysToPascalCase(rawSettings);

        //expand data inside settings input
        this.ElementID = settings.ElementID;
        this.SarvashtakavargaTableId = `${this.ElementID}_SarvashtakavargaTable`;
        this.BhinnashtakavargaTableId = `${this.ElementID}_BhinnashtakavargaTable`;
        this.ShowHeader = settings.ShowHeader;
        this.HeaderIcon = settings.HeaderIcon;
        this.SaveSettings = settings.SaveSettings;

        //based on table ID try get any settings if saved from before
        var savedTableSettings = localStorage.getItem(this.ElementID);

        //only continue if settings are saved and featured enabled in settings
        if (this.SaveSettings || savedTableSettings) {
            //parse the data
            let jsonObject = JSON.parse(savedTableSettings);

            //set back all the exact settings from before
            this.KeyColumn = jsonObject["KeyColumn"];
            this.ColumnData = jsonObject["ColumnData"];
            this.EnableSorting = jsonObject["EnableSorting"];
        }
        //if null use data pumped in via constructor (defaults, when click Reset)
        else {
            this.KeyColumn = settings.KeyColumn;
            this.ColumnData = settings.ColumnData;
            this.EnableSorting = settings.EnableSorting;
        }
    }

    async GenerateTable(inputArguments) {
        inputArguments =
            CommonTools.CamelCaseKeysToPascalCase(inputArguments);

        //clear old data if any
        $(`#${this.ElementID}`).empty();

        //# HEADER
        //show header with title, icon and edit button
        if (this.ShowHeader) {
            //random ID for edit button
            this.EditButtonId = Math.floor(Math.random() * 1000000);

            var htmlContent = `
                <h3 style="margin-bottom: -11px;">
                    <iconify-icon class="me-2" icon="${this.HeaderIcon}" width="38" height="38"></iconify-icon>
                    ${this.KeyColumn}
                </h3>
                <hr />`;

            //inject into page
            $(`#${this.ElementID}`).append(htmlContent);

            //attach event handler to edit button
            $(`#${this.EditButtonId}`).on("click", async () => {
                await this.ShowEditTableOptions();
            });
        }

        //# TABLE
        //create empty table inside main holder
        //table will be filled later
        $(`#${this.ElementID}`).append(
            `<div class="table-responsive">
                <table id="${this.SarvashtakavargaTableId}" class="table table-striped table-hover table-bordered text-nowrap w-auto" style=" font-size: 12px; font-weight: 700; "></table>
            </div>`
        );

        $(`#${this.ElementID}`).append(
            `<div class="table-responsive">
                <table id="${this.BhinnashtakavargaTableId}" class="table table-striped table-hover table-bordered text-nowrap w-auto" style=" font-size: 12px; font-weight: 700; "></table>
             </div>`
        );

        //generate table from inputed data
        //get data from API
        var sarvashtakavargaUrl = `${VedAstro.ApiDomain}/Calculate/SarvashtakavargaChart/${inputArguments.TimeUrl}Ayanamsa/${inputArguments.Ayanamsa}`;
        var bhinnashtakavargaUrl = `${VedAstro.ApiDomain}/Calculate/BhinnashtakavargaChart/${inputArguments.TimeUrl}Ayanamsa/${inputArguments.Ayanamsa}`;

        //get data from API and generate the HTML tables
        await this.GenerateHTMLTableFromAPI(
            sarvashtakavargaUrl,
            this.SarvashtakavargaTableId
        );
        await this.GenerateHTMLTableFromAPI(
            bhinnashtakavargaUrl,
            this.BhinnashtakavargaTableId
        );
    }

    async GenerateHTMLTableFromAPI(url, tableId) {
        //make the final API call in the perfect URL format
        var apiPayload = await CommonTools.GetAPIPayload(url);

        //clean old data
        AshtakvargaTable.ClearTableRows(tableId);

        AshtakvargaTable.GenerateHTMLTableFromJson(apiPayload, tableId);
    }

    static ClearTableRows(tableId) {
        let table = document.getElementById(tableId);
        while (table?.rows?.length > 0) {
            table?.deleteRow(0);
        }
    }

    //code where Ashtakvarga in JSON format given by API is converted into nice HTML
    static async GenerateHTMLTableFromJson(data, tableId) {
        //note "table responsive" needed to make nicely scrollable in mobile
        let html = '<table border="1">';

        // Add table headers
        html += "<tr><th></th>";
        for (let i = 1; i <= 12; i++) {
            html += `<th>${i}</th>`;
        }

        //add in last total column
        html += `<th>Total</th>`;

        //wrap up
        html += "</tr>";

        //get first object which will be BhinnashtakavargaChart or SarvashtakavargaChart (API names)
        const ashtakavargaJson = data[Object.keys(data)[0]];

        // Add table data rows
        for (let key in ashtakavargaJson) {
            html += `<tr><td>${key}</td>`;
            for (let i = 0; i < 12; i++) {
                html += `<td>${ashtakavargaJson[key].Rows[i]}</td>`;
            }

            //add in last total column
            html += `<td>${ashtakavargaJson[key].Total}</td>`;

            html += "</tr>";
        }

        html += "</table>";

        // Now you can add 'html' to your webpage
        var currentTable = document.getElementById(tableId);
        currentTable.innerHTML = html;
    }
}

//Planet & house shadbala strength table 
class StrengthChart {
    // Class properties
    ElementID = "";

    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
    }

    //makes the chart with data, input time and ayanamsa
    async GenerateChart(inputArguments) {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());

        // get All planet & house strengths from API
        let planetBalas = await this.fetchPlanetStrength(inputArguments);
        let houseBalas = await this.fetchHouseStrength(inputArguments);

        //with data, draw chart on screen with Chart.js library (planet/house order is explicitly stated)
        this.DrawPlanetStrengthChart(planetBalas['Sun'], planetBalas['Moon'], planetBalas['Mercury'], planetBalas['Mars'], planetBalas['Jupiter'], planetBalas['Saturn'], planetBalas['Venus'], planetBalas['Rahu'], planetBalas['Ketu']);
        this.DrawHouseStrengthChart(houseBalas['House1'], houseBalas['House2'], houseBalas['House3'], houseBalas['House4'], houseBalas['House5'], houseBalas['House6'], houseBalas['House7'], houseBalas['House8'], houseBalas['House9'], houseBalas['House10'], houseBalas['House11'], houseBalas['House12']);

        //debug message
        console.log("Strength Chart Done ✅");
    }

    async fetchPlanetStrength(inputArguments) {
        try {
            const response = await fetch(`${VedAstro.ApiDomain}/Calculate/PlanetShadbalaPinda/PlanetName/All/${inputArguments.TimeUrl}Ayanamsa/${inputArguments.Ayanamsa}`);
            if (!response.ok) { throw new Error(`HTTP error! status: ${response.status}`); } //server connection check

            const data = await response.json();
            if (data.Status !== 'Pass') { throw new Error('Failed to retrieve data. Status is not "Pass".'); } //calc data check

            // arrange planets and their strengths for easy access by name
            const shadbalaPindaData = {};
            data.Payload.PlanetShadbalaPinda.forEach(item => {
                const planetName = Object.keys(item)[0];
                const shadbalaPinda = Object.values(item)[0];
                shadbalaPindaData[planetName] = shadbalaPinda;
            });

            return shadbalaPindaData;

        } catch (error) {
            console.error('Error fetching data:', error);
            return []; // Return an empty array if there's an error
        }
    }

    async fetchHouseStrength(inputArguments) {
        try {
            const response = await fetch(`${VedAstro.ApiDomain}/Calculate/HouseStrength/HouseName/All/${inputArguments.TimeUrl}Ayanamsa/${inputArguments.Ayanamsa}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            if (data.Status !== 'Pass') {
                throw new Error('Failed to retrieve data. Status is not "Pass".');
            }

            // arrange houses and their strengths for easy access by name
            const houseStrengthData = {};
            data.Payload.HouseStrength.forEach(item => {
                const houseName = Object.keys(item)[0];
                const houseStrength = Object.values(item)[0];
                houseStrengthData[houseName] = houseStrength;
            });

            return houseStrengthData;

        } catch (error) {
            console.error('Error fetching data:', error);
            return []; // Return an empty array if there's an error
        }
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
        <h3 style="margin-bottom: -11px;">
            <iconify-icon class="me-2" icon="twemoji:antenna-bars" width="38" height="38"></iconify-icon>
            Strength
        </h3>
        <hr />
        <div class="g-4 row row-cols-1 row-cols-md-2">
            <div>
                <canvas class="rounded border" id="PlanetChart" style="max-width: 400px; max-height: 247px; background: #f5f5f9;"></canvas>
            </div>
            <div>
                <canvas class="rounded border" id="HouseChart" style="max-width: 400px; max-height: 247px; background: #f5f5f9;"></canvas>
            </div>
        </div>
    `;
    }

    DrawPlanetStrengthChart(sun, moon, mercury, mars, jupiter, saturn, venus, rahu, ketu) {
        //delete previous chart if any
        if (window.PlanetStrengthChart != null) { window.PlanetStrengthChart.destroy(); }

        var xValues = ["Sun", "Moon", "Mercury", "Mars", "Jupiter", "Saturn", "Venus", "Rahu", "Ketu"];
        var yValues = [sun, moon, mercury, mars, jupiter, saturn, venus, rahu, ketu];

        //this chart elm ID is hard coded in HTML above
        //note: stored in window so that can delete it on redraw
        window.PlanetStrengthChart = new Chart("PlanetChart",
            {
                type: "bar",
                data: {
                    xAxisID: "Planets",
                    yAxisID: "Strength",
                    labels: xValues,
                    datasets: [
                        {
                            data: yValues,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.7)',
                                'rgba(255, 159, 64, 0.7)',
                                'rgba(255, 205, 86, 0.7)',
                                'rgba(75, 192, 192, 0.7)',
                                'rgba(54, 162, 235, 0.7)',
                                'rgba(153, 102, 255, 0.7)',
                                'rgba(201, 203, 207, 0.7)',
                                'rgba(201, 162, 207, 0.7)',
                                'rgba(162, 203, 207, 0.7)'
                            ],
                            borderColor: [
                                'rgb(255, 99, 132)',
                                'rgb(255, 159, 64)',
                                'rgb(255, 205, 86)',
                                'rgb(75, 192, 192)',
                                'rgb(54, 162, 235)',
                                'rgb(153, 102, 255)',
                                'rgb(201, 203, 207)',
                                'rgb(201, 162, 207)',
                                'rgb(162, 203, 207)'
                            ],
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    animation: false,// disables all animations
                    scales: {
                        y: {
                            min: round(Math.min.apply(this, yValues) - 50),
                            max: round(Math.max.apply(this, yValues) + 50)
                        }
                    },
                    plugins: {
                        legend: {
                            display: false,
                        }
                    }
                }
            });

        //make the chart more beautiful if your stepSize is 5
        function round(x) {
            return Math.ceil(x / 5) * 5;
        }
    }

    DrawHouseStrengthChart(house1, house2, house3, house4, house5, house6, house7, house8, house9, house10, house11, house12) {
        //delete previous chart if any
        if (window.HouseStrengthChart != null) { window.HouseStrengthChart.destroy(); }

        var xValues = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
        var yValues = [house1, house2, house3, house4, house5, house6, house7, house8, house9, house10, house11, house12];

        //this chart elm ID is hard coded in HTML above
        //note: stored in window so that can delete it on redraw
        window.HouseStrengthChart = new Chart("HouseChart",
            {
                type: "bar",
                data: {
                    labels: xValues,
                    datasets: [
                        {
                            data: yValues,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.7)',
                                'rgba(255, 159, 64, 0.7)',
                                'rgba(255, 205, 86, 0.7)',
                                'rgba(75, 192, 192, 0.7)',
                                'rgba(54, 162, 235, 0.7)',
                                'rgba(153, 102, 255, 0.7)',
                                'rgba(201, 203, 207, 0.7)'
                            ],
                            borderColor: [
                                'rgb(255, 99, 132)',
                                'rgb(255, 159, 64)',
                                'rgb(255, 205, 86)',
                                'rgb(75, 192, 192)',
                                'rgb(54, 162, 235)',
                                'rgb(153, 102, 255)',
                                'rgb(201, 203, 207)'
                            ],
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    animation: false,// disables all animations
                    scales: {
                        y: {
                            min: round(Math.min.apply(this, yValues) - 50),
                            max: round(Math.max.apply(this, yValues) + 50)
                        }
                    },
                    plugins: {
                        legend: {
                            display: false,
                        }
                    }
                }
            });

        //make the chart more beautiful if your stepSize is 5
        function round(x) {
            return Math.ceil(x / 5) * 5;
        }
    }

}

//--------------------HOROSCOPE CHAT-------------------
//repainting mona-lisa's hand for the 2nd time here, so what!
//i'm prepared to repaint this hand a million times, means nothing to me!
//i'm not the painter you see, just the one watching
class HoroscopeChat {
    LastUserMessage = ""; //used for post ai reply highlight
    SelectedTopicId = ""; //she's filled in when set
    SelectedTopicText = ""; //she's filled in when set
    ServerURL = ""; //filled in later just before use
    ElementID = ""; //ID of main div where table & header will be injected
    ShowHeader = true; //default enabled, header with title, icon and edit button
    HeaderIcon = "twemoji:ringed-planet"; //default enabled, header with title, icon and edit button
    IsAITalking = false; //default false, to implement "PTT" radio like protocol
    PaddingTopApplied = false; //basic switch will go once
    SelectedBirthTime = ""; //if mentioned during init use it else, GUI will change ask for it
    SessionId = ""; //start clean, updated as message comes in

    constructor(rawSettings) {
        console.log(
            "~~~~~~~Stand back! Awesome Chat API code launching! All engines go!~~~~~~~"
        );

        //make instance accessible
        window.vedastro.horoscopechat = this;

        //process the input variables and set them
        this.initializeSettingData(rawSettings);

        //make the main chat window structure
        this.initializeChatMainBody();

        //creates ever changing placeholder questios to engage users
        this.initializeChatInputElement();

        //update control center back on earth
        console.log("~~~~~~~Huston, we have lift off!~~~~~~~");
    }

    //----------------------------------------FUNCS---------------------------------------
    //---------------------BELOW LIES FUNCS, AS WE ARE SO YOU SHALL BE--------------------

    //chat box body as html to be injected
    generateHtmlBody() {
        return `

        <div class="fw-bold hstack gap-2 d-flex" style="">
            <div>
                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" aria-hidden="true" role="img" class="iconify iconify--fluent-emoji" width="38" height="38" preserveAspectRatio="xMidYMid meet" viewBox="0 0 32 32" data-icon="fluent-emoji:robot" data-width="38" style="vertical-align: text-bottom;"><g fill="none"><path fill="url(#IconifyId18fec41c6ed170cd6117)" d="M22.05 30H9.95C6.66 30 4 27.34 4 24.05V12.03C4 8.7 6.7 6 10.03 6h11.95C25.3 6 28 8.7 28 12.03v12.03c0 3.28-2.66 5.94-5.95 5.94"></path><path fill="url(#IconifyId18fec41c6ed170cd6104)" d="M4 12a6 6 0 0 1 6-6h2v24h-2a6 6 0 0 1-6-6z"></path><path fill="url(#IconifyId18fec41c6ed170cd6105)" d="M4 24h24a6 6 0 0 1-6 6H10a6 6 0 0 1-6-6"></path><path fill="url(#IconifyId18fec41c6ed170cd6118)" d="M20 6h2a6 6 0 0 1 6 6v12a6 6 0 0 1-6 6h-2z"></path><path stroke="url(#IconifyId18fec41c6ed170cd6106)" stroke-miterlimit="10" d="M3.5 3.95v9.1"></path><path fill="url(#IconifyId18fec41c6ed170cd6107)" d="M4 12v11c-1.1 0-2-.9-2-1.998v-7.004C2 12.9 2.9 12 4 12"></path><path fill="url(#IconifyId18fec41c6ed170cd6108)" d="M22.753 18.5H9.247A4.257 4.257 0 0 1 5 14.25A4.257 4.257 0 0 1 9.247 10h13.506A4.257 4.257 0 0 1 27 14.25c0 2.331-1.918 4.25-4.247 4.25"></path><path fill="url(#IconifyId18fec41c6ed170cd6109)" d="M18.528 26h-5.056C12.66 26 12 25.326 12 24.5s.66-1.5 1.472-1.5h5.056c.811 0 1.472.674 1.472 1.5s-.66 1.5-1.472 1.5"></path><path fill="url(#IconifyId18fec41c6ed170cd6119)" d="M3.5 5a1.5 1.5 0 1 0 0-3a1.5 1.5 0 0 0 0 3"></path><path stroke="url(#IconifyId18fec41c6ed170cd6110)" stroke-miterlimit="10" d="M28.5 4v9.09"></path><path fill="url(#IconifyId18fec41c6ed170cd6120)" d="M28.5 5.1a1.55 1.55 0 1 0 0-3.1a1.55 1.55 0 0 0 0 3.1"></path><rect width="4.5" height="6" x="7" y="12" fill="url(#IconifyId18fec41c6ed170cd6121)" rx="2"></rect><rect width="4.5" height="6" x="18.5" y="12" fill="url(#IconifyId18fec41c6ed170cd6122)" rx="2"></rect><rect width="10" height="3" x="11" y="3" fill="url(#IconifyId18fec41c6ed170cd6111)" rx="1.5"></rect><rect width="10" height="3" x="11" y="3" fill="url(#IconifyId18fec41c6ed170cd6123)" rx="1.5"></rect><path fill="url(#IconifyId18fec41c6ed170cd6112)" d="M28 22.94V11.93c1.1 0 2 .9 2 2v7.01c0 1.1-.9 2-2 2"></path><rect width="2.5" height="5" x="9" y="12" fill="url(#IconifyId18fec41c6ed170cd6113)" rx="1.25"></rect><rect width="2.5" height="5" x="9" y="12" fill="url(#IconifyId18fec41c6ed170cd6124)" rx="1.25"></rect><rect width="2.5" height="5" x="20.5" y="12" fill="url(#IconifyId18fec41c6ed170cd6114)" rx="1.25"></rect><rect width="2.5" height="5" x="20.5" y="12" fill="url(#IconifyId18fec41c6ed170cd6125)" rx="1.25"></rect><g filter="url(#IconifyId18fec41c6ed170cd6129)"><path stroke="url(#IconifyId18fec41c6ed170cd6115)" stroke-width=".25" d="M3.625 5v6"></path></g><g filter="url(#IconifyId18fec41c6ed170cd6130)"><path stroke="url(#IconifyId18fec41c6ed170cd6116)" stroke-width=".25" d="M28.625 5v6"></path></g><ellipse cx="29" cy="13.5" fill="url(#IconifyId18fec41c6ed170cd6126)" rx="1" ry="1.5"></ellipse><ellipse cx="29" cy="16.5" fill="url(#IconifyId18fec41c6ed170cd6127)" rx="1" ry="4.5"></ellipse><path fill="url(#IconifyId18fec41c6ed170cd6128)" fill-rule="evenodd" d="M19.776 3.025a1.501 1.501 0 0 1 1.199 1.2a1 1 0 1 1-1.2-1.2" clip-rule="evenodd"></path><defs><linearGradient id="IconifyId18fec41c6ed170cd6104" x1="12" x2="4" y1="18" y2="18" gradientUnits="userSpaceOnUse"><stop stop-color="#D5B2C0" stop-opacity="0"></stop><stop offset="1" stop-color="#B4878D"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6105" x1="16" x2="16" y1="27" y2="31" gradientUnits="userSpaceOnUse"><stop stop-color="#B17EDB" stop-opacity="0"></stop><stop offset="1" stop-color="#A56BD6"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6106" x1="4" x2="4" y1="3.95" y2="13.05" gradientUnits="userSpaceOnUse"><stop stop-color="#EA248A"></stop><stop offset="1" stop-color="#DF2232"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6107" x1="3" x2="3" y1="12" y2="23" gradientUnits="userSpaceOnUse"><stop stop-color="#E93273"></stop><stop offset="1" stop-color="#D21844"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6108" x1="15.998" x2="15.998" y1="17.701" y2="11.391" gradientUnits="userSpaceOnUse"><stop offset=".006" stop-color="#443E75"></stop><stop offset="1" stop-color="#2F1A3B"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6109" x1="15.998" x2="15.998" y1="25.686" y2="22.889" gradientUnits="userSpaceOnUse"><stop offset=".006" stop-color="#39325E"></stop><stop offset="1" stop-color="#2B1831"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6110" x1="29" x2="29" y1="4" y2="13.09" gradientUnits="userSpaceOnUse"><stop stop-color="#EA248A"></stop><stop offset="1" stop-color="#DF2232"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6111" x1="16" x2="14.5" y1="3" y2="6.5" gradientUnits="userSpaceOnUse"><stop stop-color="#FFCE2B"></stop><stop offset="1" stop-color="#D9862D"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6112" x1="29" x2="29" y1="11.93" y2="22.94" gradientUnits="userSpaceOnUse"><stop stop-color="#FF30AA"></stop><stop offset="1" stop-color="#FF2353"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6113" x1="11.5" x2="9" y1="14" y2="14" gradientUnits="userSpaceOnUse"><stop stop-color="#29B6FE"></stop><stop offset="1" stop-color="#1769A8"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6114" x1="23" x2="20.5" y1="14" y2="14" gradientUnits="userSpaceOnUse"><stop stop-color="#29B6FE"></stop><stop offset="1" stop-color="#1769A8"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6115" x1="3.5" x2="3.5" y1="7" y2="9" gradientUnits="userSpaceOnUse"><stop stop-color="#FF96CB"></stop><stop offset="1" stop-color="#FF6DB7" stop-opacity="0"></stop></linearGradient><linearGradient id="IconifyId18fec41c6ed170cd6116" x1="28.5" x2="28.5" y1="7" y2="9" gradientUnits="userSpaceOnUse"><stop stop-color="#FF96CB"></stop><stop offset="1" stop-color="#FF6DB7" stop-opacity="0"></stop></linearGradient><radialGradient id="IconifyId18fec41c6ed170cd6117" cx="0" cy="0" r="1" gradientTransform="rotate(141.911 10.515 10.065)scale(23.5053)" gradientUnits="userSpaceOnUse"><stop stop-color="#EEEBF0"></stop><stop offset=".493" stop-color="#D1BEE3"></stop><stop offset="1" stop-color="#D0BCE2"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6118" cx="0" cy="0" r="1" gradientTransform="matrix(5 -.5 1.9111 19.11108 25 13.5)" gradientUnits="userSpaceOnUse"><stop stop-color="#F0EAF6"></stop><stop offset="1" stop-color="#E7E0EF" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6119" cx="0" cy="0" r="1" gradientTransform="matrix(-.5 2 -2 -.5 4 3)" gradientUnits="userSpaceOnUse"><stop stop-color="#FF6C82"></stop><stop offset=".441" stop-color="#FF2455"></stop><stop offset="1" stop-color="#D9206C"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6120" cx="0" cy="0" r="1" gradientTransform="rotate(104.036 13.324 12.844)scale(2.13027)" gradientUnits="userSpaceOnUse"><stop stop-color="#FF6C82"></stop><stop offset=".441" stop-color="#FF2455"></stop><stop offset="1" stop-color="#D9206C"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6121" cx="0" cy="0" r="1" gradientTransform="matrix(-2.5 .5 -.68428 -3.42136 9.5 15)" gradientUnits="userSpaceOnUse"><stop stop-color="#322649"></stop><stop offset="1" stop-color="#342950" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6122" cx="0" cy="0" r="1" gradientTransform="matrix(-2.5 .5 -.68428 -3.42136 21 15)" gradientUnits="userSpaceOnUse"><stop stop-color="#322649"></stop><stop offset="1" stop-color="#342950" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6123" cx="0" cy="0" r="1" gradientTransform="matrix(0 3 -10 0 16 4)" gradientUnits="userSpaceOnUse"><stop offset=".431" stop-color="#CA7E29" stop-opacity="0"></stop><stop offset="1" stop-color="#673F13"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6124" cx="0" cy="0" r="1" gradientTransform="matrix(0 2.5 -1.14393 0 11 13.5)" gradientUnits="userSpaceOnUse"><stop stop-color="#54C8FF"></stop><stop offset="1" stop-color="#54C8FF" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6125" cx="0" cy="0" r="1" gradientTransform="matrix(0 2.5 -1.14393 0 22.5 13.5)" gradientUnits="userSpaceOnUse"><stop stop-color="#54C8FF"></stop><stop offset="1" stop-color="#54C8FF" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6126" cx="0" cy="0" r="1" gradientTransform="matrix(0 1.5 -1 0 29 13.5)" gradientUnits="userSpaceOnUse"><stop stop-color="#FF72C1"></stop><stop offset="1" stop-color="#FF6EBF" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6127" cx="0" cy="0" r="1" gradientTransform="matrix(0 4.5 -.55944 0 29 16.5)" gradientUnits="userSpaceOnUse"><stop stop-color="#FF4B9C"></stop><stop offset="1" stop-color="#FF73C1" stop-opacity="0"></stop></radialGradient><radialGradient id="IconifyId18fec41c6ed170cd6128" cx="0" cy="0" r="1" gradientTransform="rotate(90 8 12)" gradientUnits="userSpaceOnUse"><stop stop-color="#FFEA60"></stop><stop offset="1" stop-color="#FFEF66" stop-opacity="0"></stop></radialGradient><filter id="IconifyId18fec41c6ed170cd6129" width="1.25" height="7" x="3" y="4.5" color-interpolation-filters="sRGB" filterUnits="userSpaceOnUse"><feFlood flood-opacity="0" result="BackgroundImageFix"></feFlood><feBlend in="SourceGraphic" in2="BackgroundImageFix" result="shape"></feBlend><feGaussianBlur result="effect1_foregroundBlur_31_1501" stdDeviation=".25"></feGaussianBlur></filter><filter id="IconifyId18fec41c6ed170cd6130" width="1.25" height="7" x="28" y="4.5" color-interpolation-filters="sRGB" filterUnits="userSpaceOnUse"><feFlood flood-opacity="0" result="BackgroundImageFix"></feFlood><feBlend in="SourceGraphic" in2="BackgroundImageFix" result="shape"></feBlend><feGaussianBlur result="effect1_foregroundBlur_31_1501" stdDeviation=".25"></feGaussianBlur></filter></defs></g></svg>
            </div>
            <h4 class="mt-2 me-auto">AI Chat </h4>
        </div>

        <!-- MAIN MESSAGE BODY -->
        <div class="shadow" id="BorderHolderDiv" style="border-radius: 19px;background: linear-gradient(to bottom, #ececec, #e0edff);">
            <!-- MESSAGES IN VIEW -->
            <ul class="list-unstyled mx-2 pe-2 pt-2" id="ChatWindowMessageList" style="max-height:667.5px;">
                <li class="d-flex justify-content-start mb-4" id="AIChatLoadingWaitElement" style="display: none !important;">
                    <img src="https://vedastro.org/images/vignes-chat-avatar.webp" alt="avatar"
                        class="rounded-circle d-flex align-self-start me-1 shadow-1-strong" width="45">
                    <div class="card">
                        <div class="card-header d-flex justify-content-between p-3">
                            <p class="fw-bold mb-0">Vignes</p>
                            <p class="text-muted small mb-0"><i class="far fa-clock"></i> 12 mins ago</p>
                        </div>
                        <div class="card-body">
                            <p class="mb-0">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-dasharray="15" stroke-dashoffset="15" stroke-linecap="round" stroke-width="2" d="M12 3C16.9706 3 21 7.02944 21 12"><animate fill="freeze" attributeName="stroke-dashoffset" dur="0.3s" values="15;0" /><animateTransform attributeName="transform" dur="1.5s" repeatCount="indefinite" type="rotate" values="0 12 12;360 12 12" /></path></svg>
                            </p>
                        </div>
                    </div>
                </li>
            </ul>
            <!-- QUESTION INPUT -->
            <div id="questionInputHolder" class="input-group p-2" style="">
                <input id="UserChatInputElement" class="rounded-0 rounded-start-4 form-control dropdown-toggle text-start" data-bs-toggle="dropdown" aria-expanded="false" type="text" placeholder="" aria-label="">
                <ul id="UserPresetDropDownElement" class="dropdown-menu rounded-4" aria-labelledby="UserChatInputElement" style="position: absolute;"></ul>
                <button id="SendChatButton"
                        onclick="window.vedastro.horoscopechat.onClickSendChat()"type="button"
                        class="rounded-0 rounded-end-4 btn btn-success btn-rounded float-end">
                            <iconify-icon class="me-1" icon="majesticons:send" width="25" height="25"></iconify-icon>
                            Send
                </button>
            </div>
            <div id="personSelectorHolder" style="display:none;" class="input-group p-2" style="">
                <span class="input-group-text gap-2 rounded-0 rounded-start-4 text-end">
                    I want to talk about 
                </span>
                <select class="form-select" id="PersonListDropdown" onchange="window.vedastro.horoscopechat.onSelectPerson(this)">
                     <option value="" selected>Select Horoscope</option>
                     <option value="AddNewPerson" style="font-weight: 700; color: blue;" >Add New Person</option>
                </select>
                <button id="StartChatButton"
                        onclick="window.vedastro.horoscopechat.onStartChatButton()"type="button"
                        class="rounded-0 rounded-end-4 btn btn-success btn-rounded float-end">
                            <iconify-icon class="me-1" icon="majesticons:send" width="25" height="25"></iconify-icon>
                            Start Chat
                </button>
            </div>
        </div> 
     `;
    }

    //called direct from static HTML hookup without seperate attach code
    //exp use : onclick="window.vedastro.horoscopechat.rate_message(this, -1)"
    onClickPresetQuestion(eventData) {
        //6: autofill preset questions handle (attach after generate)
        var selectedText = $(eventData).text();
        $("#UserChatInputElement").val(selectedText);
    }

    initializeSettingData(rawSettings) {
        //correct if property names is camel case (for Blazor)
        var settings = CommonTools.CamelCaseKeysToPascalCase(rawSettings);

        //expand data inside settings input
        this.ElementID = settings.ElementID;
        this.ShowHeader = settings.ShowHeader;
        this.HeaderIcon = settings.HeaderIcon;

        //birth time can be inserted at init

        this.SelectedBirthTime = settings.SelectedBirthTime;

        //GUI LOAD SAVED VALUES
        //load settings stored browser storage, reflected in gui
        let isLocalServerModeStr = localStorage.getItem("IsLocalServerMode");
        $("#useLocalServerSwitchInput").prop(
            "checked",
            JSON.parse(isLocalServerModeStr)
        );
    }

    //this makes sure the input element has dynamic text and dropdowns work well
    initializeChatInputElement() {
        //preset questions used by both elements below
        let presetQuestions = [
            "\uD83E\uDDD1\u200D\uD83C\uDFA8 Will higher educational benefit me?",
            "\uD83C\uDF7B Will a party lifestyle benefit me?",
            "\uD83D\uDC68\u200D\uD83D\uDC69\u200D\uD83D\uDC67\u200D\uD83D\uDC66 Who will benefit me more friends or family?",
            "\uD83D\uDC9D Will marriage bring me happiness?",
            "\uD83D\uDE4F Will becoming a monk benefit me?",
            "\uD83D\uDE0D Predict my sex life?",
            "\uD83C\uDF0D Can travel improve my life?",
            "\uD83C\uDF78 Why am I an alcoholic?",
            "\uD83D\uDCCA Will I succeed in stock trading?",
            "\uD83E\uDD29 Will I become famous?",
            "\uD83D\uDCB0 Will I become a millionaire?",
            "\uD83D\uDC98 Describe my future wife?",
            "\uD83D\uDC74 Relationship with my father?",
            "\uD83C\uDFB0 Can I win lottery prize?",
            "\uD83C\uDF0D Special yogas in my chart?",
            "\uD83D\uDCDA Best career for me?",
            "\uD83C\uDF93 Will I get foreign education?",
        ];

        //build the input element
        initializeInputElement();

        //then build the preset downdown
        initializePresetDropdownElement();

        //--------LOCAL

        function initializeInputElement() {
            let $inputField = $("#UserChatInputElement"); // replace 'chatInput' with your input field's ID
            let currentQuestionIndex = 0;
            let currentCharIndex = 0;
            let isUserTyping = false;

            function resetTyping() {
                currentCharIndex = 0;
                currentQuestionIndex =
                    (currentQuestionIndex + 1) % presetQuestions.length;
                $inputField.attr("placeholder", ""); // clear the placeholder
            }

            function typeQuestion() {
                if (!isUserTyping) {
                    if (currentCharIndex < presetQuestions[currentQuestionIndex].length) {
                        $inputField.attr(
                            "placeholder",
                            $inputField.attr("placeholder") +
                            presetQuestions[currentQuestionIndex][currentCharIndex]
                        );
                        currentCharIndex++;
                        setTimeout(typeQuestion, 30); // type each character every 100 milliseconds
                    } else {
                        setTimeout(resetTyping, 3000); // wait 3 seconds before starting the next question
                        setTimeout(typeQuestion, 3000);
                    }
                }
            }

            typeQuestion();

            $inputField.on("input", function () {
                isUserTyping = true;
                if ($inputField.val() === "") {
                    // if input field is empty
                    // start typing the question again after a little while (2 seconds)
                    setTimeout(function () {
                        isUserTyping = false;
                        typeQuestion();
                    }, 2000);
                }
            });

            //1:handle user press "Enter" equal to clicking send button
            $("#UserChatInputElement").keypress((e) => {
                if (e.which === 13) {
                    // Enter key pressed
                    window.vedastro.horoscopechat.onClickSendChat();
                    e.preventDefault(); // Prevents the default action
                }
            });
        }

        function initializePresetDropdownElement() {
            let dropdownElement = document.getElementById(
                "UserPresetDropDownElement"
            );

            // Clear the dropdown
            dropdownElement.innerHTML = "";

            // Fill the dropdown with the array data using HTML string interpolation
            for (let i = 0; i < presetQuestions.length; i++) {
                dropdownElement.innerHTML += `<li class="dropdown-item" onclick="window.vedastro.horoscopechat.onClickPresetQuestion(this)" style="cursor: pointer; margin-left:-4px;">${presetQuestions[i]}</li>`;
            }
        }
    }

    initializeChatMainBody() {
        //CHAT GUI INJECTION
        //clear old gui data if any

        $(`#${this.ElementID}`).empty();

        //set max width here since declared in html
        $(`#${this.ElementID}`).css("max-width", "667px");

        //random ID for edit button
        this.EditButtonId = Math.floor(Math.random() * 1000000);

        //inject into page
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        //if birth time not yet set, aka person not selected
        if (this.SelectedBirthTime == undefined) {
            //show person selector
            $("#personSelectorHolder").show();
            //hide normal chat input
            $("#questionInputHolder").hide();

            //generate person list drop down
            GeneratePersonListDropdown();

            //enter inviting message from AI
            //note: the minimal message strucuture
            let jsonObject = {
                Text:
                    String.fromCodePoint(0x1f44b) +
                    " Hi, I'm your AI astrologer. Any questions?",
                TextHtml:
                    String.fromCodePoint(0x1f44b) +
                    " Hi, I'm your AI astrologer. Any questions?",
                TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                Commands: ["noFeedback"],
            };
            var aiReplyData = JSON.stringify(jsonObject);
            this.printAIReplyMessageToView(aiReplyData);
        }

        //---------

        async function GeneratePersonListDropdown(idToSelect = "") {
            //get the main dropdown element
            var $dropdown = $("#PersonListDropdown");

            //DO FOR USER'S SAVED LIST
            VedAstro.PersonList = await CommonTools.GetAPIPayload(
                `${VedAstro.ApiDomain}/GetPersonList/OwnerId/${VedAstro.UserId}/VisitorId/${VedAstro.VisitorId}`
            );

            //create a header in the list
            let $horoscopeGroup = $("<optgroup>", {
                label: "Horoscopes",
            });

            $dropdown.append($horoscopeGroup); //add to main list

            //populate slection list at bottom with horoscopes
            $.each(VedAstro.PersonList, function (i, person) {
                $horoscopeGroup.append(
                    $("<option>", {
                        value: person.PersonId,
                        text: person.Name,
                        selected: person.PersonId === idToSelect,
                    })
                );
            });

            //DO FOR PUBLIC LIST
            VedAstro.PublicPersonList = await CommonTools.GetAPIPayload(
                `${VedAstro.ApiDomain}/GetPersonList/OwnerId/101`
            );
            //create a header in the list
            let $publicHoroscopeGroup = $("<optgroup>", {
                label: "Example Horoscopes",
            });
            $dropdown.append($publicHoroscopeGroup); //add to main list

            //populate slection list at bottom with horoscopes
            $.each(VedAstro.PublicPersonList, function (i, person) {
                $publicHoroscopeGroup.append(
                    $("<option>", {
                        value: person.PersonId,
                        text: person.Name,
                        selected: person.PersonId === idToSelect,
                    })
                );
            });
        }
    }

    //control comes here from both Button click and keyboard press enter
    async onClickSendChat(userInput = "") {
        //STEP 0 : Validation

        //make sure the chat input has something, else end here
        userInput = userInput === "" ? $("#UserChatInputElement").val() : userInput; //get chat message to send to API that user inputed
        if (userInput === "") {
            Swal.fire(
                "How to send nothing, sweetheart?",
                "Please <strong>type a question</strong> in the chatbox first. Also there's <strong>commonly asked questions</strong> on left of the input.",
                "error"
            );
            return;
        }

        //make sure AI is not busy talking
        if (window.vedastro.horoscopechat.IsAITalking) {
            Swal.fire(
                "Please wait, dear..",
                "AI is <strong>busy talking</strong>, please wait for it to <strong>finish</strong> chattering.",
                "error"
            );
            return;
        }

        //add top padding so top message don't hit top border
        if (!this.PaddingTopApplied) {
            $("#ChatWindowMessageList").addClass("pt-3");
            $("#ChatWindowMessageList").css("overflow", "auto");
            $("#ChatWindowMessageList").addClass("pe-2");
            this.PaddingTopApplied = true;
        }

        // STEP 1 : UPDATE GUI WITH USER MSG (UX)
        var aiInput = $("#UserChatInputElement").val();
        var userName = "You";
        var userInputChatCloud = `
        <li class="d-flex justify-content-end mb-4">
            <div class="card ">
                <div class="card-header d-flex justify-content-between py-2">
                    <p class="fw-bold mb-0">${userName}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">
                        ${userInput}
                    </p>
                </div>
            </div>
            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-6.webp" alt="avatar"
                 class="rounded-circle d-flex align-self-start ms-1 shadow-1-strong" width="45">
        </li>
        `;
        //inject in User's input into chat window
        $("#ChatWindowMessageList li").eq(-1).after(userInputChatCloud);

        // STEP 2 : UPDATE GUI WITH "THINKING" MSG (UX)

        //STEP 2 : GUI CLEAN UP
        //clear question input box for next, question
        $("#UserChatInputElement").val("");

        //STEP 3:
        //user's input is sent to server for reply
        //get selected birth time
        //TODO can be DOB or bookname
        //var timeInputUrl = VedAstro.SelectedPerson.BirthTime;
        //var timeInputUrl = "Location/Ipoh/Time/12:44/23/04/1994/+08:00";

        //show temperoray "Thinking" message to user before calling API as that will take time
        this.showTempThinkingMessage();

        //send user's message
        var aiReplyData = await this.sendMessageToServer(
            this.SelectedBirthTime,
            userInput
        );
        this.LastUserMessage = userInput; //save to used later for highlight

        //update local session id
        this.SessionId = aiReplyData["SessionId"];

        //print to user
        this.printAIReplyMessageToView(aiReplyData);

        //hide thinking message, for less clutered UX
        this.hideTempThinkingMessage();
    }

    //sends final user message to API server and returns only relevant text (handles errors)
    async sendMessageToServer(timeInputUrl, userQuestionInput) {
        //construct the final URL
        userQuestionInput = userQuestionInput.replace(/\?/g, ""); //remove question marks as it break API detection
        const url = `${VedAstro.ApiDomain}/Calculate/HoroscopeChat/${timeInputUrl}/UserQuestion/${userQuestionInput}/UserId/${VedAstro.UserId}/SessionId/${this.SessionId}`;

        try {
            const response = await fetch(url);
            const data = await response.json();

            if (data.Status === "Pass") {
                return data.Payload["HoroscopeChat"];
            } else {
                console.error(
                    `Request failed with status: ${data.Status}${data.Payload}`
                );

                //note: the minimal message strucuture
                let jsonObject = {
                    Text: "Sorry sir, my server brain is not talking...\nPlease try again later.",
                    TextHtml:
                        "Sorry sir, my server brain is not talking...\nPlease try again later.",
                    TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                    Commands: ["noFeedback"],
                };
                return JSON.stringify(jsonObject);
            }
        } catch (error) {
            console.error(`Error making GET request: ${error}`);

            //note: the minimal message strucuture
            let jsonObject = {
                Text: "Sorry sir, my server brain is not talking...\nPlease try again later.",
                TextHtml:
                    "Sorry sir, my server brain is not talking...\nPlease try again later.",
                TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                Commands: ["noFeedback"],
            };
            return JSON.stringify(jsonObject);
        }
    }

    // Handler for incoming messages
    printAIReplyMessageToView(rawJson) {
        // Initialize rawJsonMessage
        var rawJsonMessage;

        // Try to parse the JSON data from the event
        try {
            rawJsonMessage = JSON.parse(rawJson);
        } catch (error) {
            //expected fail because no need parse
            rawJsonMessage = rawJson;
        }
        var aiTextMessageHtml = rawJsonMessage.TextHtml;
        var messageHash = rawJsonMessage.TextHash;
        var aiTextMessage = rawJsonMessage.Text;
        var followupQuestions = rawJsonMessage?.FollowUpQuestions ?? [];

        //PROCESS SERVER COMMANDS
        var commands = rawJsonMessage.Commands || []; // when no commands given empty to not fail

        //## SPECIAL HANDLE FOR LOGIN PROMPTS
        //1: check if server said please login, in command to client
        //   meaning user just say login message given by server,
        //   upon click login, start wait loop (make it seem bot is waiting for user to login)
        //   then that special login tab (RememberMe) will auto close

        let intervalId;
        if (commands.includes("pleaseLogin")) {
            //....if needed
        }

        //## BUILD HTML

        //HANDLE FOLLOWUP
        // only add follow up questions if server specified them
        var followupQuestionsHtml = "";
        // convert questions into visible buttons, for user to click
        if (followupQuestions.length > 0) {
            followupQuestionsHtml += //start out hidden, then js will bring to live with animation at right time (class)
                '<div class="followUpQuestionHolder hstack gap-2 w-100 justify-content-end" style="display:none; position: absolute; bottom: -43px; right: -1px; ">';

            followupQuestions.forEach(function (question) {
                followupQuestionsHtml += `
            <button type="button" onclick="window.vedastro.horoscopechat.askFollowUpQuestion(this, '${question}')"  class="btn btn-outline-primary">${question}</button>
        `;
            });

            followupQuestionsHtml += "</div>";
        }

        //HANDLE FEEBACK BUTTON
        //only hide feedback button if server explicitly says so
        var feedbackButtonHtml = commands.includes("noFeedback")
            ? ""
            : `<div class="hstack gap-2">
    <button title="Bad answer" type="button" onclick="window.vedastro.horoscopechat.rateMessage(this, -1)" class="btn btn-danger" style="padding: 0px 5px;">
        <iconify-icon icon="icon-park-outline:bad-two" width="18" height="18"></iconify-icon>
    </button>
    <button title="Good answer" type="button" onclick="window.vedastro.horoscopechat.rateMessage(this, 1)" class="btn btn-primary" style="padding: 0px 5px;">
        <iconify-icon icon="icon-park-outline:good-two" width="18" height="18"></iconify-icon>
    </button>
  </div>`;

        //define html for answer
        var aiFinalAnswerHolder = `
            <div style="display:none;" class="text-html-out-elm mb-0">
                ${aiTextMessageHtml}
            </div>
        `;

        // Create a chat bubble with the AI's message
        var aiInputChatCloud = `
        <li class="d-flex justify-content-start" style=" margin-bottom: 70px; ">
            <img src="https://vedastro.org/images/vignes-chat-avatar.webp" alt="avatar" class="rounded-circle d-flex align-self-start me-1 shadow-1-strong" width="45">
            <div class="card">
                <div class="card-header d-flex justify-content-between py-2">
                    <p class="fw-bold mb-0 me-5">Vignes</p>
                    ${feedbackButtonHtml}
                </div>
                <div id="${messageHash}" class="message-holder card-body">
                    ${aiFinalAnswerHolder}
                    <p class="temp-text-stream-elm mb-0">
                      <!-- Content will be streamed here -->
                    </p>
                    <!-- SVG for loading icon -->
                    <svg class="loading-icon-elm" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-dasharray="15" stroke-dashoffset="15" stroke-linecap="round" stroke-width="2" d="M12 3C16.9706 3 21 7.02944 21 12"><animate fill="freeze" attributeName="stroke-dashoffset" dur="0.3s" values="15;0" /><animateTransform attributeName="transform" dur="1.5s" repeatCount="indefinite" type="rotate" values="0 12 12;360 12 12" /></path></svg>
                    ${followupQuestionsHtml}
                </div>
            </div>
        </li>
        `;

        // Append the chat bubble to the chat window
        $("#ChatWindowMessageList li").eq(-1).after(aiInputChatCloud);

        // # AUTO SCROLL DOWN
        $("#ChatWindowMessageList").scrollTop(
            $("#ChatWindowMessageList")[0].scrollHeight
        );

        // Flag to prevent user input while AI is 'typing'
        //NOTE: access via global, because deeply nested
        window.vedastro.horoscopechat.IsAITalking = true;

        // Initialize the index for streaming text
        let index = 0;
        const streamRateMs = 23; // Rate at which characters are displayed

        // Stream the AI's message into the chat bubble
        const interval = setInterval(() => {
            // Check if the entire message has been displayed
            //MESSAGE STREAM COMPLETE
            if (index >= aiTextMessage.length) {
                clearInterval(interval);

                // Hide the temporary element and loading icon, then show the formatted message
                //remove stream shower and loading for this bubble since not needed anymore
                //$(`#${messageHash} .temp-text-stream-elm`).hide();
                $(`#${messageHash} .loading-icon-elm`).hide();

                //make visible hidden formatted output
                //$(`#${messageHash} .text-html-out-elm`).show();

                // Allow user input again
                this.IsAITalking = false;

                // # AUTO SCROLL DOWN
                $("#ChatWindowMessageList").scrollTop(
                    $("#ChatWindowMessageList")[0].scrollHeight
                );

                //make follow up questions if any slowly appear
                //narrow by message bubble, then holder
                $(`#${messageHash} .followUpQuestionHolder`).fadeIn("slow");

                return;
            }

            // Append the next character or handle special formatting
            appendNextCharacter(
                aiTextMessage,
                index,
                `#${messageHash} .temp-text-stream-elm`
            );
            index++;

            // # AUTO SCROLL DOWN
            $("#ChatWindowMessageList").scrollTop(
                $("#ChatWindowMessageList")[0].scrollHeight
            );

            //------locals---------

            // Function to append the next character or handle special formatting
            function appendNextCharacter(text, index, elementSelector) {
                const specialChars = {
                    "\n": $("<br>"),
                    "\t": $("<span>").html("&nbsp;&nbsp;&nbsp;&nbsp;"),
                    " ": $("<span>").html("&nbsp;"),
                    "<": $("<span>").html("&lt;"),
                    ">": $("<span>").html("&gt;"),
                };

                // Check for special characters
                if (specialChars[text[index]]) {
                    $(elementSelector).append(specialChars[text[index]]);
                } else {
                    // Append regular character
                    const nextChar = document.createTextNode(text[index]);
                    $(elementSelector).append(nextChar);
                }
            }
        }, streamRateMs);
    }

    async showTempThinkingMessage() {
        //little lag for simulation reality
        await CommonTools.delay(1000);

        //define html for answer
        var aiFinalAnswerHolder = `
            <div class="text-html-out-elm mb-0">
                Thinking...
                <svg class="loading-icon-elm" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-dasharray="15" stroke-dashoffset="15" stroke-linecap="round" stroke-width="2" d="M12 3C16.9706 3 21 7.02944 21 12"><animate fill="freeze" attributeName="stroke-dashoffset" dur="0.3s" values="15;0" /><animateTransform attributeName="transform" dur="1.5s" repeatCount="indefinite" type="rotate" values="0 12 12;360 12 12" /></path></svg>
            </div>
        `;

        // Create a chat bubble with the AI's message
        var aiInputChatCloud = `
        <li id="tempThinkingMshBubble" class="d-flex justify-content-start" style=" margin-bottom: 70px; ">
            <img src="https://vedastro.org/images/vignes-chat-avatar.webp" alt="avatar" class="rounded-circle d-flex align-self-start me-1 shadow-1-strong" width="45">
            <div class="card">
                <div class="card-header d-flex justify-content-between py-2">
                    <p class="fw-bold mb-0 me-5">Vignes</p>                   
                </div>
                <div class="message-holder card-body">
                    ${aiFinalAnswerHolder}                   
                </div>
            </div>
        </li>
        `;

        // Append the chat bubble to the chat window
        $("#ChatWindowMessageList li").eq(-1).after(aiInputChatCloud);

        // # AUTO SCROLL DOWN
        $("#ChatWindowMessageList").scrollTop(
            $("#ChatWindowMessageList")[0].scrollHeight
        );

        // Flag to prevent user input while AI is 'typing'
        //NOTE: access via global, because deeply nested
        window.vedastro.horoscopechat.IsAITalking = true;
    }

    hideTempThinkingMessage() {
        $("#tempThinkingMshBubble").remove();

        // Flag to prevent user input while AI is 'typing'
        //NOTE: access via global, because deeply nested
        window.vedastro.horoscopechat.IsAITalking = false;
    }

    //called here direct from HTML button
    async askFollowUpQuestion(eventData, followUpQuestion) {
        //make sure AI is not busy talking
        if (window.vedastro.horoscopechat.IsAITalking) {
            Swal.fire(
                "Please wait, dear..",
                "AI is <strong>busy talking</strong>, please wait for it to <strong>finish</strong> chattering.",
                "error"
            );
            return;
        }

        // get hash of message, stored as id in holder
        var messageHolder = $(eventData)
            .closest(".card")
            .children(".message-holder");
        var primaryAnswerHash = messageHolder.attr("id");

        //UPDATE GUI WITH USER MSG (UX)
        var aiInput = $("#UserChatInputElement").val(); //clear chat input
        var userName = "You";
        var userInputChatCloud = `
        <li class="d-flex justify-content-end mb-4">
            <div class="card ">
                <div class="card-header d-flex justify-content-between py-2">
                    <p class="fw-bold mb-0">${userName}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">
                        ${followUpQuestion}
                    </p>
                </div>
            </div>
            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-6.webp" alt="avatar"
                 class="rounded-circle d-flex align-self-start ms-1 shadow-1-strong" width="45">
        </li>
        `;
        //inject in User's input into chat window
        $("#ChatWindowMessageList li").eq(-1).after(userInputChatCloud);

        //show temperoray "Thinking" message to user before calling API as that will take time
        this.showTempThinkingMessage();

        //prepare message and send to caller
        //construct the final URL
        var followUpAIReplyData = await getFollowUpAIReplyFromAPI(
            followUpQuestion,
            primaryAnswerHash
        );

        //inject reply into view
        //print to user
        this.printAIReplyMessageToView(followUpAIReplyData);

        //hide thinking message, for less cluttered UX
        this.hideTempThinkingMessage();

        //--------------local funcs
        async function getFollowUpAIReplyFromAPI(
            followUpQuestion,
            primaryAnswerHash
        ) {
            followUpQuestion = followUpQuestion.replace(/\?/g, ""); //remove question marks as it break API detection
            const url = `${VedAstro.ApiDomain}/Calculate/HoroscopeFollowUpChat/${window.vedastro.horoscopechat.SelectedBirthTime}/FollowUpQuestion/${followUpQuestion}/PrimaryAnswerHash/${primaryAnswerHash}/UserId/${VedAstro.UserId}/SessionId/${window.vedastro.horoscopechat.SessionId}`;

            try {
                const response = await fetch(url);
                const data = await response.json();

                if (data.Status === "Pass") {
                    return data.Payload["HoroscopeFollowUpChat"];
                } else {
                    console.error(
                        `Request failed with status: ${data.Status}${data.Payload}`
                    );

                    //note: the minimal message strucuture
                    let jsonObject = {
                        Text: "Sorry sir, my server brain is not talking...\nPlease try again later.",
                        TextHtml:
                            "Sorry sir, my server brain is not talking...\nPlease try again later.",
                        TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                        Commands: ["noFeedback"],
                    };
                    return JSON.stringify(jsonObject);
                }
            } catch (error) {
                console.error(`Error making GET request: ${error}`);

                //note: the minimal message strucuture
                let jsonObject = {
                    Text: "Sorry sir, my server brain is not talking...\nPlease try again later.",
                    TextHtml:
                        "Sorry sir, my server brain is not talking...\nPlease try again later.",
                    TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                    Commands: ["noFeedback"],
                };
                return JSON.stringify(jsonObject);
            }
        }
    }

    //called direct from static HTML hookup without seperate attach code
    //exp use : onclick="window.vedastro.horoscopechat.rateMessage(this, -1)"
    async rateMessage(eventData, rating) {
        //come here on click rating button
        // get hash of message, stored as id in holder
        var messageHolder = $(eventData)
            .closest(".card")
            .children(".message-holder");
        var textHash = messageHolder.attr("id");

        //send feedback to API
        var feedbackAIReplyData = await SendFeedbackToApi(textHash, rating);

        //inject reply into view
        //print to user
        this.printAIReplyMessageToView(feedbackAIReplyData);

        //hide thinking message, for less clutered UX
        this.hideTempThinkingMessage();

        //--------------local funcs
        async function SendFeedbackToApi(answerHash, feedbackScore) {
            const url = `${VedAstro.ApiDomain}/Calculate/HoroscopeChatFeedback/AnswerHash/${answerHash}/FeedbackScore/${feedbackScore}`;

            try {
                const response = await fetch(url);
                const data = await response.json();

                if (data.Status === "Pass") {
                    return data.Payload["HoroscopeChatFeedback"];
                } else {
                    console.error(
                        `Request failed with status: ${data.Status}${data.Payload}`
                    );

                    //note: the minimal message strucuture
                    let jsonObject = {
                        Text: "Sorry sir, my server brain is not talking...\nPlease try again later.",
                        TextHtml:
                            "Sorry sir, my server brain is not talking...\nPlease try again later.",
                        TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                        Commands: ["noFeedback"],
                    };
                    return JSON.stringify(jsonObject);
                }
            } catch (error) {
                console.error(`Error making GET request: ${error}`);

                //note: the minimal message strucuture
                let jsonObject = {
                    Text: "Sorry sir, my server brain is not talking...\nPlease try again later.",
                    TextHtml:
                        "Sorry sir, my server brain is not talking...\nPlease try again later.",
                    TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
                    Commands: ["noFeedback"],
                };
                return JSON.stringify(jsonObject);
            }
        }
    }

    async onStartChatButton(eventData) { }

    async onSelectPerson(eventData) {
        //attach topic selector dropdown
        //get all needed data (what topic was selected)
        const selectedOption = $("#PersonListDropdown option:selected");
        const selectedOptgroupLabel = selectedOption
            .closest("optgroup")
            .prop("label");

        //save what user choose for use throughout the code
        var selectedPersonId = selectedOption.val();

        //if id is add new person, then redirect page to add person site, same tab so refresh onreturn
        if (selectedPersonId == "AddNewPerson") {
            window.location.href = "./AddPerson.html";
            return; //end here
        }

        //get full details of the person
        let selectedPerson = VedAstro.PersonList.find(
            (obj) => obj.PersonId === selectedPersonId
        );

        //save for use by other
        //TODO select person needs be made local to chat
        localStorage.setItem("selectedPerson", JSON.stringify(selectedPerson));

        //convert person name to birth DOB (so unregistered person can be checked)
        //TODO select person needs be made local to chat
        var newTopicId = VedAstro.SelectedPerson.BirthTime.ToUrl();
        window.vedastro.horoscopechat.SelectedBirthTime = newTopicId;

        //person now selected, ready to chat so change GUI
        //show person selector
        $("#personSelectorHolder").hide();
        //hide normal chat input
        $("#questionInputHolder").show();

        //show user's selection on screen for explicit remembrance
        //UPDATE GUI WITH USER MSG (UX)
        debugger;
        var userName = "You";
        //TODO select person needs be made local to chat
        var selectedPersonTemp = JSON.parse(localStorage.getItem("selectedPerson"));
        const locationName = selectedPersonTemp["BirthTime"]["Location"]["Name"];
        const birthTime = selectedPersonTemp[`BirthTime`][`StdTime`];
        const personName = selectedPersonTemp[`Name`];
        var userInputChatCloud = `
        <li class="d-flex justify-content-end mb-4">
            <div class="card ">
                <div class="card-header d-flex justify-content-between py-2">
                    <p class="fw-bold mb-0">${userName}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">
                       Lets talk about <strong>${personName}</strong><br>
                       born on <strong>${birthTime}</strong><br>
                       at <strong>${locationName}</strong>
                    </p>
                </div>
            </div>
            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-6.webp" alt="avatar"
                 class="rounded-circle d-flex align-self-start ms-1 shadow-1-strong" width="45">
        </li>
        `;
        //inject in User's input into chat window
        $("#ChatWindowMessageList li").eq(-1).after(userInputChatCloud);

        //little lag for simulation reality
        await CommonTools.delay(1000);

        //reply with AI as ready to respond
        //enter inviting message from AI
        //note: the minimal message structure
        let jsonObject = {
            Text: `Ok, I've analysed the horoscope.${String.fromCodePoint(
                0x1f9d0
            )} \nAny questions?`,
            TextHtml: `Ok, I've analysed the horoscope.${String.fromCodePoint(
                0x1f9d0
            )} \nAny questions?`,
            TextHash: Math.floor(Math.random() * 1000000), //keep random for injection
            Commands: ["noFeedback"],
        };
        var aiReplyData = JSON.stringify(jsonObject);
        this.printAIReplyMessageToView(aiReplyData);
    }
}

class AlgorithmsSelector {
    // Class properties
    ElementID = "";
    ApiDataStorageKey = "AllEventsChartAlgorithms";

    // Constructor to initialize the PageHeader object
    constructor(elementId, defaultSelection) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
        this.DefaultSelection = defaultSelection;

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTM and inject it into the element
        let htmlString = await this.generateHtmlBody();
        $(`#${this.ElementID}`).html(htmlString);

        //set defaults
        algoSelector.programaticallySelect(this.DefaultSelection);

        //initialize help text
        HelpTextIcon.InitAllIn(`#${this.ElementID}`);
    }

    //given a string like "General,PlanetStrengthDegree,IshtaKashtaPhalaDegree"
    //will select the checkboxes programatically
    programaticallySelect(selectionString) {

        // Split the selection string into an array of algorithm names
        const selectedAlgorithms = selectionString.split(",");

        // Get the element with the specified ID
        const element = document.getElementById(this.ElementID);

        // Select all checkboxes within the element
        const checkboxes = element.querySelectorAll('input[type="checkbox"]');

        // Iterate over the checkboxes and update their checked state
        checkboxes.forEach((checkbox) => {
            // Get the algorithm name from the checkbox ID (without the "checkbox_" prefix)
            const algorithmName = checkbox.id.replace("checkbox_", "");

            // Check if the algorithm name is in the selected algorithms array
            if (selectedAlgorithms.includes(algorithmName)) {
                // If it is, set the checkbox to checked
                checkbox.checked = true;
            } else {
                // If not, set the checkbox to unchecked
                checkbox.checked = false;
            }
        });
    }

    /**
     * Returns a string of selected algorithm names or null if none are selected. EXP: Neutral,StrongestPlanet
     */
    getSelectedAlgorithmsAsString() {
        // Select all checkboxes inside the element with the specified ID
        const checkboxes = document.querySelectorAll(`#${this.ElementID} input[type="checkbox"]`);

        // Convert the NodeList to an array and filter to only include checked checkboxes
        const selectedAlgorithms = Array.from(checkboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.id.replace('checkbox_', '')); // Remove 'checkbox_' prefix from IDs

        // Return null if no algorithms are selected, otherwise return a comma-separated string of algorithm names
        return selectedAlgorithms.length === 0 ? null : selectedAlgorithms.join(',');
    }

    async fetchAlgorithmListFromApi() {
        const storedData = localStorage.getItem(this.ApiDataStorageKey);

        if (storedData) {
            try {
                const data = JSON.parse(storedData);
                if (data.Status === "Pass") {
                    return data.Payload;
                } else {
                    localStorage.removeItem(this.ApiDataStorageKey);
                }
            } catch (error) {
                console.error("Error parsing stored data:", error);
                localStorage.removeItem(this.ApiDataStorageKey);
            }
        }

        try {
            const response = await fetch(`${VedAstro.ApiDomain}/Calculate/GetAllEventsChartAlgorithms`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            if (data.Status !== 'Pass') {
                throw new Error('Failed to retrieve data. Status is not "Pass".');
            }
            localStorage.setItem(this.ApiDataStorageKey, JSON.stringify(data));
            return data.Payload;
        } catch (error) {
            console.error('Error fetching data:', error);
            return []; // Return an empty array if there's an error
        }
    }

    convertAlgoListToHtml(algorithmInfoList) {
        let generatedHtml = "";

        //each info contains name & description
        algorithmInfoList.forEach((algoInfo) => {
            generatedHtml += `
        <div class="form-check">
          <input class="form-check-input" type="checkbox" value="" id="checkbox_${algoInfo.Name}">
          <label class="form-check-label" for="checkbox_${algoInfo.Name}">
            ${CommonTools.CamelPascalCaseToSpaced(algoInfo.Name)}
            <div class="help-text-icon">${algoInfo.Description}</div>
          </label>
        </div>
      `;
        });

        return generatedHtml;
    }

    // Method to generate the HTML
    async generateHtmlBody() {

        //get name & description about available algorithms from API
        let algorithmList = await this.fetchAlgorithmListFromApi();

        //convert to data to HTML
        let algorithmListHtml = this.convertAlgoListToHtml(algorithmList);

        return `
        <div class="input-group vstack mb-3">
            <label style="min-width: 134.4px;" class="input-group-text rounded hstack gap-2">
                <iconify-icon icon="fluent:math-symbols-24-filled" width="25" height="25"></iconify-icon>
                Color Algorithms
                <div class="help-text-icon">Select coloring algoritms that will be used to automaticly judge an astrological event to be good, bad or somewhere in between. The colors can range from red to white to green.</div>
            </label>

            <div class="form-control d-flex flex-wrap gap-2 rounded" style="width: fit-content;">
                ${algorithmListHtml}
            </div>
        </div>
    `;
    }
}

//gives checkboxes to select all events from eventdatalist.xml via API
class EventsSelector {
    // Class properties
    ElementID = "";
    AllowedParentCheckboxes = [];
    DefaultSelectedTags = [];
    ApiDataStorageKey = "AllEventDataGroupedByTag";

    // Constructor to initialize the EventsSelector object
    constructor(elementId, allowedParentCheckboxes, defaultSelectedTags = []) {
        this.ElementID = elementId;
        this.AllowedParentCheckboxes = allowedParentCheckboxes;
        this.DefaultSelectedTags = defaultSelectedTags;
        this.initializeMainBody();
    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Fetch data from API or local storage
        const data = await this.fetchDataFromApiOrStorage();

        // Filter the data to only include the specified parent checkboxes
        const filteredData = {};
        Object.keys(data.GetAllEventDataGroupedByTag).forEach((tag) => {
            if (this.AllowedParentCheckboxes.includes(tag)) {
                filteredData[tag] = data.GetAllEventDataGroupedByTag[tag];
            }
        });

        // Generate HTML and inject it into the element
        const htmlString = this.convertDataToHtml({ GetAllEventDataGroupedByTag: filteredData });
        $(`#${this.ElementID}`).html(htmlString);

        // Set default selected tags
        this.setDefaultSelectedTags();

        // Attach event handlers to checkboxes
        this.attachEventHandlers();
    }

    // Fetch data from API or local storage
    async fetchDataFromApiOrStorage() {
        const storedData = localStorage.getItem(this.ApiDataStorageKey);
        if (storedData) {
            try {
                const data = JSON.parse(storedData);
                if (data.Status === "Pass") {
                    return data.Payload;
                } else {
                    localStorage.removeItem(this.ApiDataStorageKey);
                }
            } catch (error) {
                console.error("Error parsing stored data:", error);
                localStorage.removeItem(this.ApiDataStorageKey);
            }
        }

        try {
            const response = await fetch(`${VedAstro.ApiDomain}/Calculate/GetAllEventDataGroupedByTag`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            if (data.Status !== 'Pass') {
                throw new Error('Failed to retrieve data. Status is not "Pass".');
            }
            localStorage.setItem(this.ApiDataStorageKey, JSON.stringify(data));
            return data.Payload;
        } catch (error) {
            console.error('Error fetching data:', error);
            return [];
        }
    }

    // Convert data to HTML
    convertDataToHtml(data) {
        let generatedHtml = "";
        let parentCheckboxCount = 0;
        let columnHtmlLeft = "";
        let columnHtmlRight = "";

        const totalParentCheckboxes = this.AllowedParentCheckboxes.length;
        const middleIndex = Math.ceil(totalParentCheckboxes / 2);

        this.AllowedParentCheckboxes.forEach((tag, index) => {
            const events = data.GetAllEventDataGroupedByTag[tag];

            if (events) {
                // Generate HTML for parent checkbox
                const parentCheckboxHtml = `
            <div style="width: 254.9px;" class="form-check vstack gap-2">
                <div class="hstack gap-2">
                    <input value="" id="checkbox_${tag}" style="width: 40px; height: 28px;" class="form-check-input parent-checkbox" type="checkbox">
                    <label class="form-check-label d-flex gap-2 w-100" for="checkbox_${tag}">
                        <div class="" style="">
                            <iconify-icon icon="${this.getIconBasedOnTagName(tag)}" width="25" height="25"></iconify-icon>
                        </div>
                        ${CommonTools.CamelPascalCaseToSpaced(tag)}

                        <!-- Button to toggle visibility of child checkboxes -->
                        <button class="ms-auto me-3 toggle-child-checkboxes" style="cursor: pointer; float: right; opacity: 1; border: none; background: none; padding: 0;">
                            <iconify-icon class="show-child-button" icon="material-symbols:expand-circle-down-rounded" width="22" height="22"></iconify-icon>
                            <iconify-icon class="hide-child-button" style="display:none;" icon="material-symbols:expand-circle-up-rounded" width="22" height="22"></iconify-icon>
                        </button>
                    </label>
                </div>
                <div style="display:none; margin-left: -12px; font-size: 14px;" class="child-checkboxes">
                    ${events.map((event) => `
                        <div class="form-check">
                            <input class="form-check-input child-checkbox" type="checkbox" value="" id="checkbox_${event.Name}">
                            <label class="form-check-label" for="checkbox_${event.Name}" title="${event.Description}">${CommonTools.CamelPascalCaseToSpaced(event.Name)}</label>
                        </div>
                    `).join('')}
                </div>
            </div>
        `;

                // Add parent checkbox HTML to column HTML
                if (index < middleIndex) {
                    columnHtmlLeft += parentCheckboxHtml;
                } else {
                    columnHtmlRight += parentCheckboxHtml;
                }
            }
        });

        // Wrap column HTML in div
        generatedHtml += `
        <div style=" font-size: 15px; font-family: 'Lexend Deca';" class="d-md-flex justify-content-between">
            <div class="align-self-start d-flex flex-column gap-1 mb-3">
                ${columnHtmlLeft}
            </div>
            <div class="align-self-start d-flex flex-column gap-1 mb-3">
                ${columnHtmlRight}
            </div>
        </div>
    `;

        // Add header to generated HTML
        generatedHtml = `
        <div>
            <div class="fw-bold hstack gap-2 d-flex" style="max-width: 667px;"><h5 class="mt-2 me-auto">Event Type </h5></div>
            <hr class="mt-1 mb-2">
        </div>
        ${generatedHtml}
    `;

        return generatedHtml;
    }

    //gets preset icons for event tags, if not specified give general event icon
    getIconBasedOnTagName(eventTagName) {

        const iconMap = {
            "General": "fluent:people-team-20-regular",
            "Agriculture": "material-symbols:potted-plant",
            "Building": "carbon:construction",
            "Astronomical": "ion:telescope-outline",
            "BuyingSelling": "material-symbols:shopping-cart",
            "Medical": "bxs:injection",
            "Marriage": "fluent-emoji-high-contrast:heart-with-arrow",
            "Travel": "mdi:plane-train",
            "Studies": "tabler:school",
            "Personal": "bi:person-fill",
            "HairNailCutting": "game-icons:hair-strands"
        };

        return iconMap[eventTagName] || "material-symbols:event"; // default to general event icon if not found
    }

    // Attach event handlers to checkboxes
    attachEventHandlers() {
        const parentCheckboxes = $(`#${this.ElementID} .parent-checkbox`);
        const childCheckboxes = $(`#${this.ElementID} .child-checkbox`);

        // When a parent checkbox is clicked
        parentCheckboxes.on('click', (e) => {
            const parentCheckbox = $(e.target);
            const childCheckboxesContainer = parentCheckbox.closest('.form-check').find('.child-checkboxes');
            const childCheckboxes = childCheckboxesContainer.find('.child-checkbox');

            if (parentCheckbox.is(':checked')) {
                childCheckboxes.prop('checked', true);
            } else {
                childCheckboxes.prop('checked', false);
            }
        });

        // When a child checkbox is clicked
        childCheckboxes.on('click', (e) => {
            const childCheckbox = $(e.target);
            const parentCheckboxContainer = childCheckbox.closest('.form-check').parent().parent();
            const parentCheckbox = parentCheckboxContainer.find('.parent-checkbox');
            const childCheckboxesContainer = parentCheckboxContainer.find('.child-checkboxes');
            const childCheckboxes = childCheckboxesContainer.find('.child-checkbox');

            const checkedChildCheckboxes = childCheckboxes.filter(':checked');
            if (checkedChildCheckboxes.length === 0) {
                parentCheckbox.prop('checked', false).prop('indeterminate', false);
            } else if (checkedChildCheckboxes.length === childCheckboxes.length) {
                parentCheckbox.prop('checked', true).prop('indeterminate', false);
            } else {
                parentCheckbox.prop('checked', false).prop('indeterminate', true);
            }
        });

        // Toggle child checkboxes visibility
        const toggleChildCheckboxesButtons = $(`#${this.ElementID} .toggle-child-checkboxes`);
        toggleChildCheckboxesButtons.on('click', (e) => {
            const button = $(e.target).closest('.toggle-child-checkboxes'); // Traverse up to get the parent button
            const childCheckboxesContainer = button.closest('.form-check').find('.child-checkboxes');
            childCheckboxesContainer.toggle(); //show/hide

            // Toggle show/hide button icons
            const showChildButton = button.find('.show-child-button');
            const hideChildButton = button.find('.hide-child-button');
            if (childCheckboxesContainer.is(':visible')) {
                showChildButton.hide();
                hideChildButton.show();
            } else {
                showChildButton.show();
                hideChildButton.hide();
            }
        });



    }

    // Method to set default selected tags
    setDefaultSelectedTags() {
        const parentCheckboxes = $(`#${this.ElementID} .parent-checkbox`);
        parentCheckboxes.each((index, checkbox) => {
            const tagName = checkbox.id.replace('checkbox_', '');
            if (this.DefaultSelectedTags.includes(tagName)) {
                $(checkbox).prop('checked', true);
                const childCheckboxesContainer = $(checkbox).closest('.form-check').find('.child-checkboxes');
                const childCheckboxes = childCheckboxesContainer.find('.child-checkbox');
                childCheckboxes.prop('checked', true);
            }
        });
    }

    /**
     * Returns a string of selected tag names or null if none are selected. EXP: General,Agriculture
     */
    getSelectedTagNamesAsString() {
        const selectedParentCheckboxes = $(`#${this.ElementID} .parent-checkbox:checked`);
        const selectedTagNames = [];

        selectedParentCheckboxes.each((index, checkbox) => {
            const tagName = checkbox.id.replace('checkbox_', '');
            selectedTagNames.push(tagName);
        });

        if (selectedTagNames.length === 0) {
            return null;
        }

        return selectedTagNames.join(',');

    }
}

class DasaEventsSelector {
    // Class properties
    ElementID = "";
    DefaultSelected = [];

    // Constructor to initialize the PageHeader object
    constructor(elementId, defaultSelected = []) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Assign the default selected values
        this.DefaultSelected = defaultSelected;

        // Call the method to initialize the main body 
        this.initializeMainBody();
    }

    // Method to initialize the main body 
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for th and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Set default selected checkboxes
        this.setDefaultSelected();

        //initialize help text
        HelpTextIcon.InitAllIn(`#${this.ElementID}`);

    }

    // Method to generate the HTML for the 
    generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
        <div class="input-group vstack mb-3">
            <label style="min-width:134.4px;" class="input-group-text hstack gap-2 rounded">
                <iconify-icon icon="lucide:calendar-range" width="25" height="25"></iconify-icon>
                Events
                <div class="help-text-icon">Type of events to calculate, more events takes longer</div>
            </label>

            <div id="DasaEventSelectionHolder" class="form-control d-flex flex-wrap gap-2 rounded" style="width: fit-content;">
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD1">
                    <label class="form-check-label" for="checkbox_PD1">Dasa</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD2">
                    <label class="form-check-label" for="checkbox_PD1">Bhukti</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD3">
                    <label class="form-check-label" for="checkbox_PD3">Antaram</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD4">
                    <label class="form-check-label" for="checkbox_PD4">Sukshma</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD5">
                    <label class="form-check-label" for="checkbox_PD5">Prana</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD6">
                    <label class="form-check-label" for="checkbox_PD6">Avi Prana</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_PD7">
                    <label class="form-check-label" for="checkbox_PD7">Viprana</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_AshtakvargaGochara">
                    <label class="form-check-label" for="checkbox_AshtakvargaGochara">Ashtakvarga Gochara</label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="checkbox_Gochara">
                    <label class="form-check-label" for="checkbox_Gochara">Gochara</label>
                </div>
            </div>
        </div>
    `;
    }

    setDefaultSelected() {
        // Set default selected checkboxes
        this.DefaultSelected.forEach((id) => {
            $(`#${this.ElementID} #checkbox_${id}`).prop('checked', true);
        });
    }

    getSelectedEventsAsString() {
        // Select all checked checkboxes
        const checkedCheckboxes = $(`#${this.ElementID} input[type="checkbox"]:checked`);

        // If no checkboxes are checked, return null
        if (checkedCheckboxes.length === 0) return null;

        // Map the checked checkboxes to their values (split by '_' and take the second part)
        const values = checkedCheckboxes.map((index, checkbox) => checkbox.id.split('_')[1]).get();

        // Join the values into a string separated by commas
        return values.join(',');
    }
}

// supports dynamic 3 types of preset
// - age1to10
// - 3weeks, 3months, 3years, fulllife
// - 1990-2000
class TimeRangeSelector {
    // Class properties
    ElementID = "";
    storageKey = "timeRangeSelector";
    defaultPreset = "";
    linkedPersonSelector = null;

    // Constructor to initialize the PageHeader object
    constructor(elementId, linkedPersonSelector, defaultPreset) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
        this.defaultPreset = defaultPreset;
        this.linkedPersonSelector = linkedPersonSelector; //so that selected person's DOB can be used for time range 

        // Call the method to initialize the body html
        this.initializeMainBody();
    }

    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        //initialize help text
        HelpTextIcon.InitAllIn(`#${this.ElementID}`);

        // Initialize stored values
        this.initStoredYearValues();
        this.initStoredAgeValues();

        // Set default preset if provided
        if (this.defaultPreset) {
            $(`#${this.ElementID} .time-range-select`).val(this.defaultPreset);
            if (this.defaultPreset !== 'selectCustomYear' && this.defaultPreset !== 'selectCustomAge') {
                $(`#${this.ElementID} .custom-time-range-holder`).hide();
                $(`#${this.ElementID} .custom-age-range-holder`).hide();
            } else if (this.defaultPreset === 'selectCustomYear') {
                $(`#${this.ElementID} .custom-time-range-holder`).show();
                $(`#${this.ElementID} .custom-age-range-holder`).hide();
            } else if (this.defaultPreset === 'selectCustomAge') {
                $(`#${this.ElementID} .custom-time-range-holder`).hide();
                $(`#${this.ElementID} .custom-age-range-holder`).show();
            }

            //let others (days per pixel component) immediately know days between after default is selected
            this.getDaysInRange().then(daysInRange => {
                $(`#${this.ElementID}`).trigger('timeRangeChanged', [daysInRange]);
            });
        }

        //attach event handlers
        this.addDropdownEventListener();
        this.addInputEventListeners();
    }

    //loads previously saved years if any else use deafults
    initStoredYearValues() {
        const storedValues = localStorage.getItem(this.storageKey);
        if (storedValues) {
            const values = JSON.parse(storedValues);
            $(`#${this.ElementID} .start-year-input`).val(values.startYear);
            $(`#${this.ElementID} .start-month-input`).val(values.startMonth);
            $(`#${this.ElementID} .end-year-input`).val(values.endYear);
            $(`#${this.ElementID} .end-month-input`).val(values.endMonth);
        } else {
            // Set current year as default
            const currentYear = new Date().getFullYear();
            $(`#${this.ElementID} .start-year-input`).val(currentYear);
            $(`#${this.ElementID} .end-year-input`).val(currentYear);
        }
    }

    //loads previously saved custom ages if any else use deafults
    initStoredAgeValues() {
        const storedValues = localStorage.getItem(this.storageKey + '_age');
        if (storedValues) {
            const values = JSON.parse(storedValues);
            $(`#${this.ElementID} .start-age-input`).val(values.startAge);
            $(`#${this.ElementID} .end-age-input`).val(values.endAge);
        }
        // Set age 10 to 45 as default
        else {
            $(`#${this.ElementID} .start-age-input`).val(10);
            $(`#${this.ElementID} .end-age-input`).val(45);
        }
    }

    //calculate the number of days between start date and end date also handles time range preset by using API
    async getDaysInRange() {

        //only continue of dates are valid 
        if (!this.isValid()) { return 0; }

        //show loading to user
        CommonTools.ShowLoading();

        let differenceInDays = 0;

        //if user inputs manual time range, get dates and calculate difference
        if ($(`#${this.ElementID} .time-range-select`).val() === 'selectCustomYear') {
            const startYear = parseInt($(`#${this.ElementID} .start-year-input`).val());
            const startMonth = parseInt($(`#${this.ElementID} .start-month-input`).val());
            const endYear = parseInt($(`#${this.ElementID} .end-year-input`).val());
            const endMonth = parseInt($(`#${this.ElementID} .end-month-input`).val());

            // Create Date objects for start and end dates
            const startDate = new Date(startYear, startMonth - 1, 1);
            const endDate = new Date(endYear, endMonth - 1, this.getLastDayOfMonth(endYear, endMonth - 1));

            // Calculate the difference in days
            //the `+ 1` at the end of the calculation is to include the last day of the range in the count.
            differenceInDays = Math.round((endDate.getTime() - startDate.getTime()) / (1000 * 3600 * 24)) + 1;

        }
        //else user inputs using time range presets & custom age range also, use API to calculate range
        else {
            //get selected person's birth time URL for age preset computation
            var birthTimeUrl = await this.getSelectedPersonBirthTimeUrl();

            //this can be age preset & time preset
            let selectedTimePreset = $(`#${this.ElementID} .time-range-select`).val();

            //if user selects age range construct preset, exp age20to40
            if (selectedTimePreset === 'selectCustomAge') {
                const startAge = parseInt($(`#${this.ElementID} .start-age-input`).val());
                const endAge = parseInt($(`#${this.ElementID} .end-age-input`).val());

                //construct new age preset
                selectedTimePreset = `age${startAge}to${endAge}`;
            }

            let userTimezoneString = this.getSystemOffset();

            //call API via GET request
            let callUrl = `${VedAstro.ApiDomain}/Calculate/DaysBetweenTimeRangePreset/${birthTimeUrl}TimePreset/${selectedTimePreset}/OutputTimezone/${userTimezoneString}`;
            const response = await fetch(callUrl);

            //extract and return the days in range value
            const data = await response.json();
            differenceInDays = parseFloat(data.Payload.DaysBetweenTimeRangePreset);
        }

        //hide loading
        Swal.close();

        return differenceInDays;

    }

    async getSelectedPersonBirthTimeUrl() {
        //get full data of selected person
        let selectedPerson = await this.linkedPersonSelector.GetSelectedPerson();

        //if person not selected give empty time string
        if (selectedPerson == null) { return "Location/Empty/Time/00:00/01/01/0001/+00:00/"; }

        //get birth time of selected person (URL format)
        var timeUrl = selectedPerson.BirthTime.ToUrl();

        return timeUrl;
    }

    //check if the dates are valid & filled, else shows user msg, and returns false
    isValid() {

        const startYear = parseInt($(`#${this.ElementID} .start-year-input`).val());
        const startMonth = parseInt($(`#${this.ElementID} .start-month-input`).val());
        const endYear = parseInt($(`#${this.ElementID} .end-year-input`).val());
        const endMonth = parseInt($(`#${this.ElementID} .end-month-input`).val());
        const selectedValue = $(`#${this.ElementID} .time-range-select`).val();
        const startAge = parseInt($(`#${this.ElementID} .start-age-input`).val());
        const endAge = parseInt($(`#${this.ElementID} .end-age-input`).val());

        //check custom year
        if (selectedValue === 'selectCustomYear') {
            //check if dates is not empty
            if (isNaN(startYear) || isNaN(startMonth) || isNaN(endYear) || isNaN(endMonth)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Date is wrong sir! 📅',
                    text: 'Please check  if year and month is correct'
                });
                return false;
            }

            // Check if years are not negative
            if (startYear < 0 || endYear < 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Year cannot be negative! ',
                    text: 'Please enter a valid year'
                });
                return false;
            }

            //check if start time is before end time
            if (!(startYear < endYear || (startYear === endYear && startMonth <= endMonth))) {
                Swal.fire({
                    icon: 'error',
                    title: 'Dates are reversed! 🤪',
                    text: 'Start date should be before end date'
                });
                return false;
            }


        }

        //check custom age
        if (selectedValue === 'selectCustomAge') {
            // Check for valid age range
            if (isNaN(startAge) || isNaN(endAge)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Age is wrong sir! ',
                    text: 'Please check 🧐 if age is correct'
                });
                return false;
            }

            if (startAge < 0 || endAge < 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Age cannot be negative! 🧐',
                    text: 'Please enter a valid age range'
                });
                return false;
            }

            if (startAge >= endAge) {
                Swal.fire({
                    icon: 'error',
                    title: 'Invalid age range! 🧐',
                    text: 'Start age should be less than end age'
                });
                return false;
            }

        }

        //checks has passed
        return true;
    }

    getSelectedTimeRangeAsURLString() {
        const selectedValue = $(`#${this.ElementID} .time-range-select`).val();

        //if a custom range is selected, then it returns "Start/00:00/01/01/2024/End/00:00/31/12/2024/+08:00/"
        //NOTE: time is always set to 00:00 and start date is always the 1st of the month & end date is always end of the month
        if (selectedValue === 'selectCustomYear') {
            const startYear = $(`#${this.ElementID} .start-year-input`).val();
            const startMonth = $(`#${this.ElementID} .start-month-input`).val();
            const endYear = $(`#${this.ElementID} .end-year-input`).val();
            const endMonth = $(`#${this.ElementID} .end-month-input`).val();

            // Store values in local storage
            this.storeYearValues(startYear, startMonth, endYear, endMonth);

            //get user's current timezone UTC offset (system time)
            let offsetString = this.getSystemOffset();

            return `Start/00:00/01/${startMonth}/${startYear}/End/00:00/${this.getLastDayOfMonth(endYear, endMonth - 1)}/${endMonth}/${endYear}/${offsetString}`;
        }
        //if custom age is selected then construct age present in correct format "age10to100"
        else if (selectedValue === 'selectCustomAge') {
            const startAge = $(`#${this.ElementID} .start-age-input`).val();
            const endAge = $(`#${this.ElementID} .end-age-input`).val();

            // Store values in local storage
            this.storeAgeValues(startAge, endAge);

            return `TimePreset/age${startAge}to${endAge}`;
        } else
        //if a preset is selected function returns "TimeRange/PresetValue"
        {
            return `TimePreset/${selectedValue}`;
        }
    }

    storeYearValues(startYear, startMonth, endYear, endMonth) {

        if (!this.isValid()) { return; } //only continue if valid 

        const values = {
            startYear,
            startMonth,
            endYear,
            endMonth
        };
        localStorage.setItem(this.storageKey, JSON.stringify(values));
    }

    storeAgeValues(startAge, endAge) {

        if (!this.isValid()) { return; } //only continue if valid 

        const values = {
            startAge,
            endAge
        };
        localStorage.setItem(this.storageKey + '_age', JSON.stringify(values));
    }

    //offset nicely formatted exp:"+08:00"
    getSystemOffset() {
        const offset = new Date().getTimezoneOffset();
        const offsetHours = Math.floor(Math.abs(offset) / 60);
        const offsetMinutes = Math.abs(offset) % 60;
        const offsetString = (offset < 0 ? '+' : '-') + String(offsetHours).padStart(2, '0') + ':' + String(offsetMinutes).padStart(2, '0');
        return offsetString;
    }

    getLastDayOfMonth(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    // Method to generate the HTML for the page header
    generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
            <!-- PRESET SELECTOR -->
            <div>
                <label class="form-label">
                    Time Range
                    <div class="help-text-icon">Start and end time for chart</div>
                </label>
                <select class="form-control time-range-select" style="width: 254.9px;">
                    <option style="font-weight: bold; color: #0d6efd;" value="selectCustomYear">Custom Date</option>
                    <option value="1day">+/- 1 Day</option>
                    <option value="1week">+/- 1 Week</option>
                    <option value="1month">+/- 1 Month</option>
                    <option value="2month">+/- 2 Month</option>
                    <option value="3month">+/- 3 Months</option>
                    <option value="6month">+/- 6 Months</option>
                    <option value="1year">+/- 1 Year</option>
                    <option value="3year">+/- 3 Year</option>
                    <option value="5year">+/- 5 Year</option>
                    <option value="10year">+/- 10 Year</option>
                    <option style="font-weight: bold; color: #0d6efd;" value="selectCustomAge">Custom Age</option>
                    <option value="age1to25">Age 1 to 35</option>
                    <option value="age10to35">Age 10 to 35</option>
                    <option value="age25to50">Age 25 to 50</option>
                    <option value="age35to60">Age 35 to 60</option>
                    <option value="age60to85">Age 60 to 85</option>
                    <option value="age50to100">Age 50 to 100</option>
                    <option style="font-weight: bold;" value="fulllife">Full Life</option>
                </select>
            </div>

            <!-- CUSTOM TIME RANGE -->
            <div class="custom-time-range-holder mt-3" style="display: none;">
                <div class="input-group mb-2" style="width: 312px;">
                    <label class="input-group-text" style="width: 60.1px;">Start</label>
                    <input type="number" class="form-control start-year-input" pattern="\d{4}" title="Four digit year" required="">
                    <span class="input-group-text">Month</span>
                    <select class="form-select start-month-input">
                        <option value="01" selected="">JAN</option>
                        <option value="02">FEB</option>
                        <option value="03">MAR</option>
                        <option value="04">APR</option>
                        <option value="05">MAY</option>
                        <option value="06">JUN</option>
                        <option value="07">JUL</option>
                        <option value="08">AUG</option>
                        <option value="09">SEP</option>
                        <option value="10">OCT</option>
                        <option value="11">NOV</option>
                        <option value="12">DEC</option>
                    </select>
                </div>
                <div class="input-group mb-3" style="width: 312px;">
                    <label class="input-group-text" style="width: 60.1px">End</label>
                    <input type="number" class="form-control end-year-input" pattern="\d{4}" title="Four digit year" required="">
                    <span class="input-group-text">Month</span>
                    <select class="form-select end-month-input">
                        <option value="01">JAN</option>
                        <option value="02">FEB</option>
                        <option value="03">MAR</option>
                        <option value="04">APR</option>
                        <option value="05">MAY</option>
                        <option value="06">JUN</option>
                        <option value="07">JUL</option>
                        <option value="08">AUG</option>
                        <option value="09">SEP</option>
                        <option value="10">OCT</option>
                        <option value="11">NOV</option>
                        <option value="12" selected="">DEC</option>
                    </select>
                </div>
            </div>

            <!-- CUSTOM AGE RANGE -->
            <div class="custom-age-range-holder mt-3" style="display: none;">
                <div class="input-group mb-2" style="width: 312px;">
                    <label class="input-group-text" style="width: 92.1px;">From Age</label>
                    <input type="number" class="form-control start-age-input" pattern="\d+" title="Age" required="">
                    <span class="input-group-text">To</span>
                    <input type="number" class="form-control end-age-input" pattern="\d+" title="Age" required="">
                </div>
            </div>

        `;
    }

    // Add event listener to dropdown
    addDropdownEventListener() {

        //make custom date selector hide/show
        $(`#${this.ElementID} .time-range-select`).on('change', (event) => {
            var $parent = $(event.target).closest(`#${this.ElementID}`);
            if ($(event.target).val() === 'selectCustomYear') {
                $parent.find('.custom-time-range-holder').show();
                $parent.find('.custom-age-range-holder').hide();
            } else if ($(event.target).val() === 'selectCustomAge') {
                $parent.find('.custom-time-range-holder').hide();
                $parent.find('.custom-age-range-holder').show();
            } else {
                $parent.find('.custom-time-range-holder').hide();
                $parent.find('.custom-age-range-holder').hide();
            }

            // let days per pixel component know that time range has changed
            this.getDaysInRange().then(daysInRange => {
                $(`#${this.ElementID}`).trigger('timeRangeChanged', [daysInRange]);
            });
        });
    }

    addInputEventListeners() {
        $(`#${this.ElementID} .start-year-input, #${this.ElementID} .start-month-input, #${this.ElementID} .end-year-input, #${this.ElementID} .end-month-input, #${this.ElementID} .start-age-input, #${this.ElementID} .end-age-input`).on('change', (event) => {
            const startYear = $(`#${this.ElementID} .start-year-input`).val();
            const startMonth = $(`#${this.ElementID} .start-month-input`).val();
            const endYear = $(`#${this.ElementID} .end-year-input`).val();
            const endMonth = $(`#${this.ElementID} .end-month-input`).val();
            const startAge = $(`#${this.ElementID} .start-age-input`).val();
            const endAge = $(`#${this.ElementID} .end-age-input`).val();

            if ($(event.target).hasClass('start-age-input') || $(event.target).hasClass('end-age-input')) {
                this.storeAgeValues(startAge, endAge);
            } else {
                this.storeYearValues(startYear, startMonth, endYear, endMonth);
            }

            // let days per pixel component know that time range has changed
            this.getDaysInRange().then(daysInRange => {
                $(`#${this.ElementID}`).trigger('timeRangeChanged', [daysInRange]);
            });
        });
    }

}

class DayPerPixelInput {
    // Class properties
    ElementID = "";
    MaxWidthPx = 1000;

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Call the method to initialize the main body
        this.initializeMainBody();

        // when time range component is updated, it triggers this to recalculate precision 
        $(document).on('timeRangeChanged', (event, daysInRange) => {

            let daysPerPixel = daysInRange / this.MaxWidthPx; //max 1000px width

            daysPerPixel = Math.round(daysPerPixel * 100000) / 100000; //round to 3 decimal places

            $(`#${this.ElementID} .precision-value-input`).val(daysPerPixel);
        });
    }

    // Method to initialize the main body 
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        //initialize help text
        HelpTextIcon.InitAllIn(`#${this.ElementID}`);
    }

    getValue() {
        //gets value in input
        let value = $(`#${this.ElementID} .precision-value-input`).val();
        let floatValue = parseFloat(value);
        if (isNaN(floatValue)) {
            throw new Error(`Invalid input value: ${value}`);
        }
        return floatValue;
    }

    // Method to generate the HTML 
    generateHtmlBody() {
        return `
        <div class="input-group mb-3">
            <span class="input-group-text hstack gap-2">
                <iconify-icon icon="lucide:microscope" width="25" height="25"></iconify-icon>
                Precision
                <div class="help-text-icon">
                    The number of days in a pixel, more days in 1 pixel equals less precision. If the number is too low, the chart will take too long and will not generate. Change in small steps. For very high precision use Desktop App. Linked to time range, this number will auto update when time range is changed.
                </div>
            </span>
            <input type="number" step="0.01" class="form-control precision-value-input">
        </div>
    `;
    }

}

class IndianChart {
    // Class properties
    ElementID = "";
    SelectedDivisionalCharts = [];
    SelectedChartStyle = "";
    TimeUrl = "";
    Ayanamsa = "";
    AllCharts = [
        'RasiD1', 'HoraD2', 'DrekkanaD3', 'ChaturthamshaD4', 'SaptamshaD7', 'NavamshaD9', 'DashamamshaD10',
        'DwadashamshaD12', 'ShodashamshaD16', 'VimshamshaD20', 'ChaturvimshamshaD24', 'BhamshaD27',
        'TrimshamshaD30', 'KhavedamshaD40', 'AkshavedamshaD45', 'ShashtyamshaD60'
    ];

    // Constructor to initialize object
    constructor(elementId, defaultChartStyle, defaultDivisionalCharts) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
        this.SelectedDivisionalCharts = defaultDivisionalCharts;
        this.SelectedChartStyle = defaultChartStyle;

        // Call the method to initialize the main body
        this.initializeMainBody();
    }

    // Method to initialize the main body
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());

        // Bind event listeners to the checkboxes and radio buttons
        this.bindEventListeners();
    }

    async GenerateChart(generateArguments) {

        //save generate data for later use
        this.TimeUrl = generateArguments.TimeUrl;
        this.Ayanamsa = generateArguments.Ayanamsa;

        // Clear holder "all-charts-holder" div of previous charts
        $(`#${this.ElementID} .all-charts-holder`).empty();

        // Generate image element for each SelectedDivisionalCharts with its own src and inject into "all-charts-holder"
        this.AllCharts.forEach((divisionalChartName) => {
            if (this.SelectedDivisionalCharts.includes(divisionalChartName)) {
                // Get src link for chart
                let src = this.getChartUrl(divisionalChartName, this.SelectedChartStyle, this.TimeUrl, this.Ayanamsa);

                // Inject into holder div "all-charts-holder"
                $(`#${this.ElementID} .all-charts-holder`).append(`<img class="img-thumbnail" style="width: 352px;" src="${src}" />`);
            }
        });
    }

    // Given chart division name, generates API url in correct format
    getChartUrl(chartDivisionName, chartStyle, timeUrl, ayanamsa) {
        return `${VedAstro.ApiDomain}/Calculate/${chartStyle}IndianChart/${timeUrl}ChartType/${chartDivisionName}/Ayanamsa/${ayanamsa}`;
    }

    // Method to generate the HTML
    async generateHtmlBody() {
        // Return the HTML
        return `
            <div>
                <div class="hstack" style="margin-bottom: -11px;">
                    <h3 class="align-self-end m-0">
                        <iconify-icon class="me-2" icon="twemoji:dotted-six-pointed-star" width="38" height="38"></iconify-icon>
                        Charts
                    </h3>
                    <div style="" class="btn-group ms-auto align-self-end">
                        <button style="height: 37.1px; width: fit-content;" class="btn btn-sm dropdown-toggle btn-primary" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <iconify-icon icon="gala:settings" width="25" height="25"></iconify-icon>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end px-1" >
                            <li>
                                <div class="form-check">
                                  <input class="form-check-input chartStyleRadio" type="radio" name="chartStyleRadio" id="chartStyle_South" value="South" ${this.SelectedChartStyle === 'South' ? 'checked' : ''}>
                                  <label class="form-check-label" for="chartStyle_South">
                                    South Indian
                                  </label>
                                </div>
                                <div class="form-check">
                                  <input class="form-check-input chartStyleRadio" type="radio" name="chartStyleRadio" id="chartStyle_North" value="North" ${this.SelectedChartStyle === 'North' ? 'checked' : ''}>
                                  <label class="form-check-label" for="chartStyle_North">
                                    North Indian
                                  </label>
                                </div>
                            </li>
                            <li><hr class="dropdown-divider"></li>
                            ${this.generateCheckboxList()}
                        </ul>
                    </div>
                </div>
                <hr />
                <div class="d-flex flex-wrap gap-2 all-charts-holder">
                    
                </div>
            </div>
        `;
    }

    generateCheckboxList() {
        let html = '';

        this.AllCharts.forEach((chart) => {
            const isChecked = this.SelectedDivisionalCharts.includes(chart);
            html += `
            <li>
                <div class="form-check hstack">
                    <input style="width:22px; height:22px;" class="me-1 form-check-input divisional-chart-checkbox" type="checkbox" value="${chart}" id="checkbox_${chart}" ${isChecked ? 'checked' : ''}>
                    <label class="pt-1 text-nowrap form-check-label" for="checkbox_${chart}">
                        ${CommonTools.CamelPascalCaseToSpaced(chart)}
                    </label>
                </div>
            </li>
        `;
        });

        return html;
    }

    //event handlers to update chart when style or division settings is changed
    bindEventListeners() {

        //changes in division selection
        $(`#${this.ElementID} .divisional-chart-checkbox`).on('change', (e) => {
            const chartName = e.target.value;
            if (e.target.checked) {
                if (!this.SelectedDivisionalCharts.includes(chartName)) {
                    this.SelectedDivisionalCharts.push(chartName);
                }
            } else {
                this.SelectedDivisionalCharts = this.SelectedDivisionalCharts.filter((chart) => chart !== chartName);
            }
            // Clear holder "all-charts-holder" div of previous charts
            $(`#${this.ElementID} .all-charts-holder`).empty();

            // Generate image element for each SelectedDivisionalCharts with its own src and inject into "all-charts-holder"
            this.AllCharts.forEach((divisionalChartName) => {
                if (this.SelectedDivisionalCharts.includes(divisionalChartName)) {
                    // Get src link for chart
                    let src = this.getChartUrl(divisionalChartName, this.SelectedChartStyle, this.TimeUrl, this.Ayanamsa);

                    // Inject into holder div "all-charts-holder"
                    $(`#${this.ElementID} .all-charts-holder`).append(`<img class="img-thumbnail" style="width: 352px;" src="${src}" />`);
                }
            });
        });

        //change in style
        $(`#${this.ElementID} .chartStyleRadio`).on('change', (e) => {
            this.SelectedChartStyle = e.target.value;
            // Clear holder "all-charts-holder" div of previous charts
            $(`#${this.ElementID} .all-charts-holder`).empty();

            // Generate image element for each SelectedDivisionalCharts with its own src and inject into "all-charts-holder"
            this.AllCharts.forEach((divisionalChartName) => {
                if (this.SelectedDivisionalCharts.includes(divisionalChartName)) {
                    // Get src link for chart
                    let src = this.getChartUrl(divisionalChartName, this.SelectedChartStyle, this.TimeUrl, this.Ayanamsa);

                    // Inject into holder div "all-charts-holder"
                    $(`#${this.ElementID} .all-charts-holder`).append(`<img class="img-thumbnail" style="width: 352px;" src="${src}" />`);
                }
            });
        });
    }
}

class AllAstroDataTable {
    // Class properties
    ElementID = "";
    SelectedColumns = [];
    ColumnsData = [];
    availableColumns = [];
    TimeUrl = "";
    Ayanamsa = "";
    IconName = "";
    defaultColumns = [];

    // Constructor to initialize the object
    constructor(elementId, keyColumn, iconName, defaultColumns) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
        this.KeyColumn = keyColumn; //can Planet or House

        this.IconName = iconName;
        this.defaultColumns = defaultColumns; // Store the default columns for when reset

        // Check if there's a previously saved selection in localStorage
        const savedColumns = localStorage.getItem(`SelectedColumns_${this.ElementID}`);
        if (savedColumns) {
            this.SelectedColumns = JSON.parse(savedColumns);
        } else {
            this.SelectedColumns = [...defaultColumns]; //assign by value, not reference
        }
    }

    // Method to initialize the main body
    async GenerateTable(generateArguments, useCache = false) {

        // Save generate data for later use
        this.TimeUrl = generateArguments.TimeUrl;
        this.Ayanamsa = generateArguments.Ayanamsa;

        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Make API call to fetch planets data, only if when specified
        if (!useCache) { await this.getAstroDataFromApi(); }

        // Generate available columns
        this.availableColumns = Object.keys(this.ColumnsData[0][Object.keys(this.ColumnsData[0])[0]]);

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlTable());

        // Bind event listeners to the checkboxes
        this.bindEventListeners();
    }

    // Method to fetch planets data from API
    async getAstroDataFromApi() {
        try {
            const response = await fetch(`${VedAstro.ApiDomain}/Calculate/All${this.KeyColumn}Data/${this.KeyColumn}Name/All/${this.TimeUrl}Ayanamsa/${this.Ayanamsa}`);
            const data = await response.json();
            this.ColumnsData = Object.values(data.Payload)[0];

        } catch (error) {
            console.error('Error fetching planets data:', error);
        }
    }

    // Method to generate the HTML table
    async generateHtmlTable() {
        let tableHtml = `
        <div>
                <div class="hstack" style="margin-bottom: -11px;">
                    <h3 class="align-self-end m-0">
                        <iconify-icon class="me-2" icon="${this.IconName}" width="38" height="38"></iconify-icon>
                        ${this.KeyColumn} Data
                    </h3>
                    <div class="btn-group ms-auto align-self-end">
                        <button style="height: 37.1px; width: fit-content;" class="btn btn-sm dropdown-toggle btn-primary" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <iconify-icon icon="gala:settings" width="25" height="25"></iconify-icon>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-end ps-2 pe-1" data-bs-auto-close="false" >
                            <li>
                                <div class="hstack gap-2">
                                    <input type="text" class="columnListSearchInput form-control ps-2" placeholder="Search...">
                                    <button type="button" class="px-1 checked-column-reset-btn btn btn-primary d-flex flex-nowrap">
                                        <span class="me-1 iconify" data-icon="ix:hard-reset" data-width="20" data-height="20"></span>
                                        Reset
                                    </button>
                                </div>
                            </li>
                            <li><hr class="dropdown-divider"></li>
                            <div class="column-list-container">
                                ${this.generateCheckboxList()}
                            </div>
                        </ul>
                    </div>
                </div>
                <hr />

      <div class="table-responsive-sm">
        <table class="table table-striped table-hover table-bordered text-nowrap w-auto" style="border-radius: 10px;overflow: hidden;">
          <thead class="table-dark">
            <tr>
              <th>Planet</th>
    `;

        // Dynamically generate table headers based on selected columns
        this.SelectedColumns.forEach((column) => {
            tableHtml += `<th class="text-wrap">${CommonTools.CamelPascalCaseToSpaced(column)}</th>`;
        });

        tableHtml += `
            </tr>
          </thead>
          <tbody>
    `;

        this.ColumnsData.forEach((columnData) => {
            const columnName = Object.keys(columnData)[0];
            const planetInfo = columnData[columnName];

            tableHtml += `
        <tr>
          <td>${columnName}</td>
    `;

            // Dynamically generate table data based on selected columns
            //checks if the value is an object with a `Name` and `DegreesIn` property, and if it is,
            //it concatenates these values into a string with the desired format.
            //If the object has a different structure, it falls back to stringifying the object.
            this.SelectedColumns.forEach((column) => {
                let value = planetInfo[column];
                if (Array.isArray(value)) {
                    value = value.join(', ');
                } else if (typeof value === 'object' && value !== null) {
                    if (value.Name && value.DegreesIn) {
                        value = `${value.Name} ${value.DegreesIn.DegreeMinuteSecond}`;
                    }
                    else if (value.DegreeMinuteSecond) {
                        value = `${value.DegreeMinuteSecond}`;
                    }
                    //name only handle planet name
                    else if (value.Name) {
                        value = `${value.Name}`;
                    }
                    else {
                        value = JSON.stringify(value);
                    }
                }
                tableHtml += `<td>${value}</td>`;
            });


            tableHtml += `
        </tr>
      `;
        });

        tableHtml += `
          </tbody>
        </table>
      </div>
    `;

        return tableHtml;
    }

    generateCheckboxList() {
        let checkedHtml = '';
        let uncheckedHtml = '';

        this.availableColumns.forEach((column) => {
            const isChecked = this.SelectedColumns.includes(column);
            const checkboxHtml = `
            <li>
                <div class="form-check hstack">
                    <input style="width:22px; height:22px;" class="form-check-input column-checkbox me-1" type="checkbox" value="${column}" id="checkbox_${column}" ${isChecked ? 'checked' : ''}>
                    <label class="pt-1 text-nowrap form-check-label" for="checkbox_${column}">
                        ${CommonTools.CamelPascalCaseToSpaced(column)}
                    </label>
                </div>
            </li>
        `;

            if (isChecked) {
                checkedHtml += checkboxHtml;
            } else {
                uncheckedHtml += checkboxHtml;
            }
        });

        return checkedHtml + `<li><hr class="dropdown-divider"></li>` + uncheckedHtml;
    }

    // Bind event listeners to the checkboxes and search input
    bindEventListeners() {
        // Bind event listener to column checkboxes
        $(`#${this.ElementID} .column-checkbox`).on('change', (e) => {
            const column = e.target.value;
            if (e.target.checked) {
                if (!this.SelectedColumns.includes(column)) {
                    this.SelectedColumns.push(column);
                }
            } else {
                this.SelectedColumns = this.SelectedColumns.filter((col) => col !== column);
            }
            // Save the updated selection to localStorage
            localStorage.setItem(`SelectedColumns_${this.ElementID}`, JSON.stringify(this.SelectedColumns));

            //generate table again but with previoulsy gotten data, since DOB has not changed
            this.GenerateTable({ TimeUrl: this.TimeUrl, Ayanamsa: this.Ayanamsa }, true);
        });

        // do filter search on column names
        $(`#${this.ElementID} .columnListSearchInput`).on('input', (e) => {
            const searchTerm = e.target.value.toLowerCase();
            const columnList = $(`#${this.ElementID} .column-list-container li`);

            columnList.each((index, element) => {
                const columnName = $(element).find('.form-check-label').text().toLowerCase();
                if (columnName.includes(searchTerm)) {
                    $(element).show();
                } else {
                    $(element).hide();
                }
            });
        });

        // focus will automatically go to the search input field,
        // allowing the user to start typing immediately
        $(`#${this.ElementID} .dropdown-toggle`).on('click', () => {
            if (CommonTools.IsMobile()) { return; }//if on mobile then do not auto focus, because keyboard takes up screen
            setTimeout(() => {
                const searchInput = $(`#${this.ElementID} .columnListSearchInput`);
                searchInput.focus();
            }, 100); // add a small delay to ensure the dropdown is fully shown
        });

        // sort checked and unchecked column names for easier viewing
        $(`#${this.ElementID} .dropdown-menu`).on('hidden.bs.dropdown', () => {
            const columnList = $(`#${this.ElementID} .column-list-container`);
            columnList.html(this.generateCheckboxList());
        });

        // when reset button is clicked, the `SelectedColumns` will be reset to the `defaultColumns` 
        // provided in the constructor, and the checkbox list will be updated to reflect the reset.
        $(`#${this.ElementID} .checked-column-reset-btn`).on('click', () => {
            this.SelectedColumns = this.defaultColumns;
            // Save the updated selection to localStorage
            localStorage.setItem(`SelectedColumns_${this.ElementID}`, JSON.stringify(this.SelectedColumns));
            // Generate table again but with previously gotten data, since DOB has not changed
            this.GenerateTable({ TimeUrl: this.TimeUrl, Ayanamsa: this.Ayanamsa }, true);
            // Update the checkbox list to reflect the reset
            const columnList = $(`#${this.ElementID} .column-list-container`);
            columnList.html(this.generateCheckboxList());
        });

    }
}

class EvensChartViewer {
    // Class properties
    ElementID = "";
    CurrentZoomLevel = 100; //defaults to 100 on start
    EventsChartInstance = null;

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // generate basic html butttons & place holder text
        this.initializeMainBody();

    }

    // Method to initialize the main body
    initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Add the class to the div
        $(`#${this.ElementID}`).addClass("overflow-auto");

        // Add the style attribute to the div
        $(`#${this.ElementID}`).css("margin-top", "60.1px");

        // attache handler for button clicks
        this.bindEventListeners();
    }

    bindEventListeners() {

        // expand button
        $(`#${this.ElementID} .expand-view-button`).on('click', (e) => {

            $('#DesktopSidebarHolder').toggleClass('d-md-block');

            // Save value, so can zoom in and out as toggled
            let _isMaximized = !$(`#${this.ElementID} .expand-view-button`).hasClass('maximized');
            $(`#${this.ElementID} .expand-view-button`).toggleClass('maximized');

            // Zoom in/out 4 times
            const zoomMethod = _isMaximized ? this.OnClickZoomIn(4) : this.OnClickZoomOut(4);

        });

        // zoom in button
        $(`#${this.ElementID} .zoom-in-button`).on('click', (e) => {
            this.OnClickZoomIn();
        });

        // zoom out button
        $(`#${this.ElementID} .zoom-out-button`).on('click', (e) => {
            this.OnClickZoomOut();
        });

        // un/highlight check boxes based on houses or planets
        $(`#${this.ElementID} .form-check-input`).on('change', (e) => {
            const eventName = e.target.value;

            //highlight
            if (e.target.checked) {
                this.EventsChartInstance.highlightByEventName(eventName);
            }
            //unhighlight
            else {
                this.EventsChartInstance.unhighlightByEventName(eventName);
            }

        });

        // download svg button
        $(`#${this.ElementID} .download-svg-button`).on('click', (e) => {

            //show loading to user
            CommonTools.ShowLoading();

            const svgElement = document.getElementById("EventsChartSvgHolder").querySelector("svg");
            const svgString = new XMLSerializer().serializeToString(svgElement);
            const blob = new Blob([svgString], { type: 'image/svg+xml' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `Chart-${this.selectedPersonId}-${this.timeRangeUrl.split("/")[1]}-${this.selectedEventTags.replace(/,/g, "-")}.svg`;
            a.click();
            URL.revokeObjectURL(url);

            //hide loading after little delay for UX
            setTimeout(() => { Swal.close(); }, 250);

        });


    }

    async OnClickZoomIn(multiply = 1) {
        //increment current zoom level
        this.CurrentZoomLevel += (10 * multiply);

        //apply new zoom
        $(`#${this.ElementID} #EventsChartSvgHolder`).css('zoom', `${this.CurrentZoomLevel}%`);
    }

    async OnClickZoomOut(multiply = 1) {
        //increment current zoom level
        this.CurrentZoomLevel -= (10 * multiply);

        //apply new zoom
        $(`#${this.ElementID} #EventsChartSvgHolder`).css('zoom', `${this.CurrentZoomLevel}%`);
    }

    async GenerateChart(selectedPersonId, timeRangeUrl, daysPerPixel, selectedEventTags, selectedAlgorithms, selectedAyanamsa) {

        //save for later use
        this.selectedPersonId = selectedPersonId;
        this.timeRangeUrl = timeRangeUrl;
        this.daysPerPixel = daysPerPixel;
        this.selectedEventTags = selectedEventTags;
        this.selectedAlgorithms = selectedAlgorithms;
        this.selectedAyanamsa = selectedAyanamsa;

        //construct API call URL in correct format
        let apiUrl = `${VedAstro.ApiDomain}/EventsChart/${selectedPersonId}/${timeRangeUrl}/${daysPerPixel}/${selectedEventTags}/${selectedAlgorithms}/Ayanamsa/${selectedAyanamsa}`;

        //call the API and wait for the chart to be complete
        const fetchApi = async () => {
            try {
                const response = await fetch(apiUrl);
                const callStatus = response.headers.get('Call-Status');

                //if chart is still being built
                if (callStatus === 'Running') {
                    await new Promise(resolve => setTimeout(resolve, 5000)); // wait 5 seconds
                    return fetchApi(); // make the call again
                } else if (callStatus === 'Fail') {
                    Swal.fire({ icon: 'error', title: 'Server could not make chart 🤕', html: 'An error report has been sent. 🤞Hopefully it will be fixed soon. Try <strong>again with different settings</strong>, maybe it\'ll work.', showConfirmButton: true });
                    return null;
                } else if (callStatus === 'Pass') {
                    const svgString = await response.text();
                    return svgString;
                }
            } catch (error) {
                await new Promise(resolve => setTimeout(resolve, 5000)); // wait 5 seconds
                return fetchApi(); // make the call again
            }
        };

        //get chart from api as SVG string
        let svgString = await fetchApi();

        //if chart did not make it end here
        if (svgString == null) { return; }

        // inject SVG string into element with id "EventsChartSvgHolder"
        document.getElementById("EventsChartSvgHolder").innerHTML = svgString;

        // get id of SVG element
        let svgElement = document.getElementById("EventsChartSvgHolder").querySelector("svg");
        let chartId = svgElement.id; //NOTE: unique ID of the chart made by server

        //brings to life & makes available in window.EventsChartList
        this.EventsChartInstance = new EventsChart(chartId);

        //note : this makes chart appear normal in dark/normal mode
        var value = window.DarkModeLibInstance.isActivated() ? "difference" : "normal";
        $('#EventsChartSvgHolder').css('mix-blend-mode', value);

        //let caller know all went well
        console.log(`🤲 Amen! Chart Loaded : ID:${chartId}`);

        //make chart holder with buttons visible
        $('#EventsChartMainElement').show();

        //hide placeholder text
        $('#EventsChartPlaceHolderMessage').hide();

        //zoom in to make chart clear
        this.OnClickZoomIn(2);

    }

    // Method to generate the HTML
    generateHtmlBody() {
        return `
            <hr>
            <div id="EventsChartMainElement" class="vstack gap-1" style="display: none;">
                <div class="hstack gap-2 mb-2">
                    <button style="height: 37.1px; width: fit-content;" class="expand-view-button d-none d-md-block btn-sm btn-primary btn">
                      <iconify-icon icon="icon-park-outline:full-screen-one" width="25" height="25"></iconify-icon>
                    </button>
                    <button style="height: 37.1px; width: fit-content;" class="zoom-in-button btn-sm btn-primary btn">
                      <iconify-icon icon="iconamoon:zoom-in" width="25" height="25"></iconify-icon>
                    </button>
                    <button style="height: 37.1px; width: fit-content;" class="zoom-out-button btn-sm btn-primary btn">
                      <iconify-icon icon="iconamoon:zoom-out" width="25" height="25"></iconify-icon>
                    </button>
                    <button style="height: 37.1px; width: fit-content;" class="btn-sm btn-primary btn">
                      <iconify-icon icon="bx:hide" width="25" height="25"></iconify-icon>
                    </button>
                    <div style="" class="dropdown ">
                        <button style=" height:37.1px; width: fit-content;" class="btn-sm  dropdown-toggle btn-primary btn " type="button" data-bs-toggle="dropdown" aria-expanded="false"><iconify-icon icon="fluent:highlight-16-filled" width="25" height="25"></iconify-icon></button>
                        <ul style="cursor: pointer; width: 254.9px;" class="dropdown-menu ">
                            <div class="d-flex flex-wrap gap-1">
                                <div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 1" id="House-1">
                                    <label class="form-check-label" for="House-1">House 1</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 2" id="House-2">
                                    <label class="form-check-label" for="House-2">House 2</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 3" id="House-3">
                                    <label class="form-check-label" for="House-3">House 3</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 4" id="House-4">
                                    <label class="form-check-label" for="House-4">House 4</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 5" id="House-5">
                                    <label class="form-check-label" for="House-5">House 5</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 6" id="House-6">
                                    <label class="form-check-label" for="House-6">House 6</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 7" id="House-7">
                                    <label class="form-check-label" for="House-7">House 7</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 8" id="House-8">
                                    <label class="form-check-label" for="House-8">House 8</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 9" id="House-9">
                                    <label class="form-check-label" for="House-9">House 9</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 10" id="House-10">
                                    <label class="form-check-label" for="House-10">House 10</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 11" id="House-11">
                                    <label class="form-check-label" for="House-11">House 11</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="House 12" id="House-12">
                                    <label class="form-check-label" for="House-12">House 12</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Sun" id="Sun">
                                    <label class="form-check-label" for="Sun">Sun</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Moon" id="Moon">
                                    <label class="form-check-label" for="Moon">Moon</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Mars" id="Mars">
                                    <label class="form-check-label" for="Mars">Mars</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Mercury" id="Mercury">
                                    <label class="form-check-label" for="Mercury">Mercury</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Jupiter" id="Jupiter">
                                    <label class="form-check-label" for="Jupiter">Jupiter</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Venus" id="Venus">
                                    <label class="form-check-label" for="Venus">Venus</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Saturn" id="Saturn">
                                    <label class="form-check-label" for="Saturn">Saturn</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Rahu" id="Rahu">
                                    <label class="form-check-label" for="Rahu">Rahu</label>
                                </div><div style="width: 97.3px; margin-left: 10px;" class="form-check">
                                    <input style="width: 20px; height: 20px;" class="form-check-input" type="checkbox" value="Ketu" id="Ketu">
                                    <label class="form-check-label" for="Ketu">Ketu</label>
                                </div>
                            </div>
                        </ul>
                    </div>
                    <button style="height: 37.1px; width: fit-content;" class="download-svg-button btn-sm btn-primary btn">
                      <iconify-icon icon="material-symbols:download" width="25" height="25"></iconify-icon>
                    </button>
                    <button style="height: 37.1px; width: fit-content;" class="btn-sm btn-primary btn">
                      <iconify-icon icon="ri:bookmark-3-fill" width="25" height="25"></iconify-icon>
                    </button>
                    <button style="height: 37.1px; width: fit-content;" class="btn-sm btn-primary btn">
                      <iconify-icon icon="ph:code-fill" width="25" height="25"></iconify-icon>
                    </button>
                    <div style="" class="dropdown">
                      <button style="height: 37.1px; width: fit-content;" class="btn-sm dropdown-toggle hstack gap-2 iconButton btn-primary btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                        <iconify-icon icon="gravity-ui:person" width="25" height="25"></iconify-icon>Person
                      </button>
                      <ul style="cursor: pointer; width: 100%;" class="dropdown-menu">
                        <li>
                          <a class="dropdown-item hstack gap-1">
                            <div class="me-2 mt-1" style="">
                              <iconify-icon icon="fluent:book-star-20-filled" width="25" height="25"></iconify-icon>
                            </div>
                            <span>Horoscope</span>
                          </a>
                        </li>
                        <li>
                          <a class="dropdown-item hstack gap-1">
                            <div class="me-2 mt-1" style="">
                              <iconify-icon icon="mdi:calendar-outline" width="25" height="25"></iconify-icon>
                            </div>
                            <span>Journal</span>
                          </a>
                        </li>
                      </ul>
                    </div>
                </div>

                <div class="container-xxl" id="EventsChartSvgHolder" style="margin-left: -11px; "></div>
            </div>

            <!-- PLACEHOLDER TEXT -->
            <div id="EventsChartPlaceHolderMessage">
                <div class="d-flex justify-content-center">
                    <span class="" style="color: #8f8f8f; font-size: 14px;">Chart will appear <strong>here</strong> after <strong>calculate</strong></span>
                </div>
            </div>
            <hr>


    `;
    }
}

class HelpTextIcon {
    // Class properties
    Element = null;
    HelpText = "Help Text Goes Here";

    // Constructor to initialize the object
    //* A string representing the ID or selector of the element: `new HelpTextIcon('#myId')` or`new HelpTextIcon('.myClass')`
    //* A jQuery object: `new HelpTextIcon($('#myId'))`
    //* A DOM element: `new HelpTextIcon(document.getElementById('myId'))`
    constructor(element) {
        // Check if the provided element is a jQuery object, a DOM element, or a string (id or selector)
        if (typeof element === 'string') {
            // If it's a string, try to select the element using jQuery
            this.Element = $(element);
        } else if (element instanceof jQuery) {
            // If it's a jQuery object, use it directly
            this.Element = element;
        } else if (element instanceof Element) {
            // If it's a DOM element, wrap it in a jQuery object
            this.Element = $(element);
        } else {
            throw new Error('Invalid element provided to HelpTextIcon constructor');
        }

        // Check if the element exists
        if (this.Element.length === 0) {
            throw new Error('Element not found');
        }

        // Get the help text HTML from inside the element
        this.HelpText = this.Element.html();

        // Clear the HTML of the text
        this.Element.empty();

        // Add the style attribute to the element
        this.Element.css("cursor", "help");
        this.Element.css("opacity", "0.8");
        this.Element.css("display", "inline-flex");
        this.Element.css("vertical-align", "text-bottom");

        // Inject the help icon
        this.Element.html(`<iconify-icon icon="icon-park:help" width="18" height="18"></iconify-icon>`);

        // Set up the tooltip
        //- html for multi line
        //- interactive for selecting text
        tippy(this.Element[0], {
            content: this.HelpText,
            interactive: true,
            allowHTML: true,
            placement: "right",
            trigger: "click"
        });
    }

    // Given ID or selector of parent div, initialize help icon for each element with class "help-text-icon"
    // HelpTextIcon.InitAllIn('#parentDivId');
    static InitAllIn(parent) {
        // Get all elements with class "help-text-icon" inside the parent div
        const $parent = $(parent);
        const $helpTextIcons = $parent.find('.help-text-icon');

        // Initialize each help icon
        $helpTextIcons.each((index, element) => {
            // Create a new instance of HelpTextIcon for the current element
            new HelpTextIcon(element);
        });
    }
}

class HoroscopePredictionTexts {
    // Class properties
    ElementID = "";
    TimeUrl = "";
    Ayanamsa = "";
    HoroscopePredictions = [];

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;
    }

    // Method to initialize the main body 
    async GenerateTable(generateArguments) {

        // Save generate data for later use
        this.TimeUrl = generateArguments.TimeUrl;
        this.Ayanamsa = generateArguments.Ayanamsa;

        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        //get data from API
        this.HoroscopePredictions = await this.getHoroscopePredictionsFromApi();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Add event listener to the search input
        this.addSearchEventListener();
    }

    async getHoroscopePredictionsFromApi() {
        const response = await fetch(`${VedAstro.ApiDomain}/Calculate/HoroscopePredictions/${this.TimeUrl}Ayanamsa/${this.Ayanamsa}`);
        const data = await response.json();
        return data.Payload;
    }

    generateHtmlBody() {
        let html = `
        <!-- PREDICTIONS HEADER -->
        <div class="hstack" style="margin-bottom: -11px;">
            <h3 class="align-self-end m-0">
                <iconify-icon class="me-2" icon="noto:scroll" width="38" height="38"></iconify-icon>
                Predictions
            </h3>
            <div class="ms-auto align-self-end" style="font-size: 22px;">Found: <strong id="prediction-count">${this.HoroscopePredictions.length}</strong></div>
        </div>
        <hr />
        <div class="input-group mb-3">
          <input type="text" id="prediction-search" class="form-control" placeholder="🔍 Search">
        </div>
        <div id="PredictionsHolder" class="overflow-auto" style="max-height:546px;">
            ${this.generatePredictionCards()}
        </div>
    `;

        return html;
    }

    generatePredictionCards() {
        return this.HoroscopePredictions.map((prediction) => this.generatePredictionCard(prediction)).join('');
    }

    generatePredictionCard(prediction) {
        return `
        <div class="card mb-3 prediction-card">
            <h4 class="card-header">${CommonTools.CamelPascalCaseToSpaced(prediction.Name)}</h5>
            <div class="card-body">
                <p class="card-text">${prediction.Description}</p>
            </div>
            <div class="card-footer text-body-secondary" style="font-size: 13px;">
               ${this.generateRelatedBodiesHtml(prediction.RelatedBody)}
            </div>
        </div>
    `;
    }

    generateRelatedBodiesHtml(relatedBody) {
        let html = '';
        if (relatedBody.Planets.length > 0) {
            html += ` ✨${relatedBody.Planets.join(', ')}`;
        }
        if (relatedBody.Houses.length > 0) {
            html += ` 🏠${relatedBody.Houses.join(', ')}`;
        }
        if (relatedBody.Zodiacs.length > 0) {
            html += ` ☪️${relatedBody.Zodiacs.join(', ')}`;
        }
        return html;
    }

    addSearchEventListener() {
        $('#prediction-search').on('input', () => {
            const searchQuery = $('#prediction-search').val().toLowerCase();
            const predictionCards = $('.prediction-card');

            let visibleCount = 0;

            predictionCards.each((index, card) => {
                const cardText = $(card).text().toLowerCase();
                if (cardText.includes(searchQuery)) {
                    $(card).show();
                    visibleCount++;
                } else {
                    $(card).hide();
                }
            });

            $('#prediction-count').text(visibleCount);
        });
    }
}

class PersonListViewer {
    // Class properties
    ElementID = "";
    personList = [];

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Save a reference to this instance
        this.saveInstanceReference();

        // Initialize the main body
        this.initializeMainBody();
    }

    // Save a reference to this instance for global access
    saveInstanceReference() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.PersonListViewerInstances) {
            window.vedastro.PersonListViewerInstances = {};
        }
        window.vedastro.PersonListViewerInstances[this.ElementID] = this;
    }

    // Method to initialize the main body 
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // get person list from API or cache automatic
        this.personList = await VedAstro.GetPersonList('private');

        // Generate the HTML table of person and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Add event listener for search input
        $('#person-search').on('input', () => {
            this.filterTable();
        });
    }

    getIconForGender(gender) {
        if (gender === 'Female') {
            return '🚺';
        } else {
            return '🚹';
        }
    }

    // Generate the HTML table of person
    generateHtmlBody() {
        let tableBody = '';
        this.personList.forEach(person => {
            tableBody += `
                <tr>
                    <td>
                        <div>${person.Name}</div>
                        <div>${this.getIconForGender(person.Gender)} ${person.Gender}</div>
                    </td>
                    <td>
                        <div>🕑 ${person.BirthTime.StdTime}</div>
                        <div title="${person.BirthTime.Location.Name}">🌍 ${CommonTools.TruncateText(person.BirthTime.Location.Name, 43)}, ${person.BirthTime.Location.Latitude}, ${person.BirthTime.Location.Longitude}</div>
                    </td>
                    <td>
                        <button style=" height:37.1px; width: fit-content;" class="btn-sm btn-outline-primary btn" type="button"
                            onclick="window.vedastro.PersonListViewerInstances['${this.ElementID}'].onClickEdit('${person.PersonId}')">
                            <iconify-icon icon="uil:edit" width="25" height="25"></iconify-icon>
                        </button>
                    </td>
                </tr>
            `;
        });

        return `
            <!-- HEADER -->
            <div class="hstack" style="margin-bottom: -11px;">
                <h3 class="align-self-end m-0">
                    <iconify-icon class="me-2" icon="fluent-emoji-flat:floppy-disk" width="38" height="38"></iconify-icon>
                    Saved Persons
                </h3>
            </div>
            <hr />
            <div class="input-group mb-3">
              <input type="text" id="person-search" class="form-control" placeholder="🔍 Search">
            </div>
            <div class="table-responsive-sm">
                <table class="table table-striped table-hover table-bordered text-nowrap" style="border-radius: 10px;overflow: hidden;">
                    <thead class="table-dark">
                        <tr>
                            <th>Name</th>
                            <th>Birth Time</th>
                            <th> </th>
                        </tr>
                    </thead>
                    <tbody id="person-table-body">
                        ${tableBody}
                    </tbody>
                </table>
            </div>
        `;
    }

    // Method to handle the edit button click
    onClickEdit(personId) {
        // Find the person data based on the person ID
        const person = this.personList.find(p => p.PersonId === personId);
        if (!person) {
            Swal.fire({
                icon: 'error',
                title: 'Person not found',
                text: 'Could not find person data.'
            });
            return;
        }

        // Generate a unique storage key, for example the person ID
        const selectedPersonStorageKey = `SelectedPerson-${person.PersonId}`;

        // Save the person data into session storage (so that unique across tabs)
        sessionStorage.setItem(selectedPersonStorageKey, JSON.stringify(person));

        // Navigate to EditPerson.html with the query parameter using selectedPersonStorageKey
        window.location.href = `./EditPerson.html?SelectedPersonStorageKey=${selectedPersonStorageKey}`;
    }

    // Method to filter the table
    filterTable() {
        const searchInput = $('#person-search').val().toLowerCase();
        const tableRows = $('#person-table-body tr');

        tableRows.each((index, row) => {
            const rowText = $(row).text().toLowerCase();
            if (rowText.includes(searchInput)) {
                $(row).show();
            } else {
                $(row).hide();
            }
        });
    }
}

/**
 * Represents an API method viewer component.
 * This class generates the HTML for selecting and invoking API methods
 */
class ApiMethodViewer {
    // Class properties
    ElementID = "";
    apiMethods = []; // To store the list of API methods
    selectedMethodData = null; // The currently selected API method data
    timeInputInstances = {}; // To store instances of TimeLocationInput
    timeLocationInputParams = []; // To store IDs and names of time parameters
    SearchInputElementClass = "searchInputElementClass"; // Class for the search input
    ApiDataStorageKey = "AllApiMethods"; // Key for local storage
    ServerAddressStorageKey = "ServerAddress"; // Key for storing server address in localStorage

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Save a reference to this instance
        this.saveInstanceReference();

        // Initialize the main body
        this.initializeMainBody();
    }

    // Save a reference to this instance for global access
    saveInstanceReference() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.ApiMethodViewerInstances) {
            window.vedastro.ApiMethodViewerInstances = {};
        }
        window.vedastro.ApiMethodViewerInstances[this.ElementID] = this;
    }

    // Method to initialize the main body
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Fetch the API methods
        await this.fetchApiMethods();

        // Generate the HTML and inject it into the element
        $(`#${this.ElementID}`).html(this.generateHtmlBody());

        // Initialize help text icons
        HelpTextIcon.InitAllIn(`#${this.ElementID}`);

        // Bind event listener to the "Generate" button
        const generateButton = document.getElementById(`${this.ElementID}_generateButton`);
        generateButton.addEventListener('click', () => this.onGenerateButtonClick());
    }

    // Method to fetch API methods from the server or local storage
    async fetchApiMethods() {
        // Check if data is in local storage
        const storedData = localStorage.getItem(this.ApiDataStorageKey);
        const storedHash = localStorage.getItem(`${this.ApiDataStorageKey}_hash`);

        // Fetch the server-side hash
        const serverHashResponse = await fetch(`${VedAstro.ApiDomain}/AllCallsHash`);
        const serverHashData = await serverHashResponse.json();

        if (serverHashData.Status === "Pass") {
            const serverHash = serverHashData.Payload.toString(); // Convert the integer to a string

            if (storedHash === serverHash) {
                // Load from local storage if hashes match
                if (storedData) {
                    try {
                        const data = JSON.parse(storedData);
                        if (data.Status === "Pass") {
                            console.log("Loaded API methods from local storage");
                            this.apiMethods = data.Payload;
                            return; // Return early since data is loaded from local storage
                        } else {
                            localStorage.removeItem(this.ApiDataStorageKey);
                        }
                    } catch (error) {
                        console.error("Error parsing stored data:", error);
                        localStorage.removeItem(this.ApiDataStorageKey);
                    }
                }
            }
        }

        // If data not in local storage or hashes differ, fetch from server
        try {
            const response = await fetch(`${VedAstro.ApiDomain}/ListAllCalls`);
            const data = await response.json();

            if (data.Status === "Pass") {
                this.apiMethods = data.Payload;
                // Cache both the data and the hash in local storage
                localStorage.setItem(this.ApiDataStorageKey, JSON.stringify(data));
                localStorage.setItem(`${this.ApiDataStorageKey}_hash`, serverHashData.Payload); // Save the hash
            } else {
                console.error('Failed to retrieve API methods:', data.Payload);
            }
        } catch (error) {
            console.error('Error fetching API methods:', error);
        }
    }

    // Method to generate the HTML
    generateHtmlBody() {
        // Get the stored server address
        const serverAddress = this.getStoredServerAddress();

        // Generate the method list HTML
        let methodListHTML = this.generateMethodListHtml();

        let html = `
        <div class="container mt-3" style="width:412px;">
            <div class="input-group">
                <span class="input-group-text gap-2 py-1" style="width: 136px;"><iconify-icon icon="lucide:server-crash" width="35" height="35"></iconify-icon>Server</span>
                <input id="ServerAddressInput" type="text" class="form-control" value="${serverAddress}" placeholder="http://localhost:7071" style="font-weight: 600; font-size: 16px;">
            </div>
            <div class="mt-3">
                <div class="fw-bold hstack gap-2 d-flex">
                    <iconify-icon icon="flat-color-icons:calculator" width="38" height="38"></iconify-icon>
                    <h5 class="mt-2 me-auto">Calculator </h5>
                </div>
                <hr class="mt-0 mb-2">
            </div>
            <div class="mb-3">
                <div class="hstack">
                    <div class="btn-group w-100" style="min-width:231px !important;">
                        <button onclick="window.vedastro.ApiMethodViewerInstances['${this.ElementID}'].onClickDropDown(event)" type="button" class="btn dropdown-toggle btn-primary text-centre" data-bs-toggle="dropdown" aria-expanded="false">
                            <div class="selected-method-name" style="cursor: pointer;white-space: nowrap; display: inline-table;" >Select Method</div>
                        </button>
                        <ul class="dropdown-menu w-100 ps-2 pe-3" style="height: 412.5px; overflow: clip scroll;">
                            <!-- SEARCH INPUT -->
                            <div class="hstack gap-2">
                                <input onkeyup="window.vedastro.ApiMethodViewerInstances['${this.ElementID}'].onKeyUpSearchBar(event)" type="text" class="${this.SearchInputElementClass} form-control ms-0 mb-2 ps-3" placeholder="Search...">
                                <div class="mb-2" style="cursor: pointer;">
                                    <iconify-icon icon="mingcute:list-search-fill" width="25" height="25"></iconify-icon>
                                </div>
                            </div>
                            <!-- METHOD LIST -->
                            ${methodListHTML}
                        </ul>
                    </div>
                </div>
            </div>

            <!-- Method Description -->
            <div id="${this.ElementID}_methodDescription" class="mt-2"></div>

            <div class="">
                <div class="fw-bold hstack gap-2 d-flex">
                    <iconify-icon icon="flat-color-icons:multiple-inputs" width="38" height="38"></iconify-icon>
                    <h5 class="mt-2 me-auto">Input Parameters </h5>
                </div>
                <hr class="mt-0 mb-2">
            </div>

            <!-- Parameters will be loaded here -->
            <div id="${this.ElementID}_parameters"></div>

            <!-- Generate button -->
            <button id="${this.ElementID}_generateButton" style=" place-content: center !important;font-weight: 500 !important;font-size: 17px !important; height:37.1px; width: fit-content;" class="btn-sm w-100 hstack gap-2 btn-success btn " disabled>
                <iconify-icon icon="flat-color-icons:flash-auto" width="25" height="25"></iconify-icon>
                Generate
            </button>

            <!-- URL output -->
            <div id="${this.ElementID}_output" class="mt-3">
            </div>
        </div>
        `;

        return html;
    }

    // Method to get the stored server address or default
    getStoredServerAddress() {
        const storedAddress = localStorage.getItem(this.ServerAddressStorageKey);
        if (storedAddress) {
            return storedAddress;
        }
        return 'http://localhost:7071';
    }

    // Method to generate the method list HTML
    generateMethodListHtml() {
        const html = this.apiMethods.map((method) => {
            const formattedName = CommonTools.CamelPascalCaseToSpaced(method.MethodInfo.Name);
            const description = method.Description || '';
            return `<li onClick="window.vedastro.ApiMethodViewerInstances['${this.ElementID}'].onClickMethodName('${method.MethodInfo.Name}')" class="dropdown-item method-item" style="cursor: pointer;" data-method-name="${method.MethodInfo.Name.toLowerCase()}" data-formatted-name="${formattedName.toLowerCase()}" data-description="${description.toLowerCase()}">${formattedName}</li>`;
        }).join("");

        return html;
    }

    // Handle click on the dropdown button
    onClickDropDown(event) {
        // Set focus to the search text box for instant input
        // Note: Only on desktop, skip for mobile, because keyboard takes screen space
        if (!CommonTools.IsMobile()) {
            $(`#${this.ElementID}`).find(`.${this.SearchInputElementClass}`).focus();
        }
    }

    // Handle keyup event on the search input field
    onKeyUpSearchBar(event) {
        // Ignore certain keys to prevent unnecessary filtering
        if (["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Space", "ControlLeft", "ControlRight", "AltLeft", "AltRight", "ShiftLeft", "ShiftRight", "Enter", "Tab", "Escape"].includes(event.code)) {
            return;
        }

        // Get the search text from the input field
        const searchText = event.target.value.toLowerCase();

        // Filter method items based on the search text
        var allMethodDropItems = $(`#${this.ElementID}`).find('.dropdown-menu li.method-item');
        allMethodDropItems.each(function () {
            const methodName = $(this).data('method-name'); // method name in lowercase
            const formattedName = $(this).data('formatted-name'); // formatted name in lowercase
            const description = $(this).data('description'); // description in lowercase

            if (methodName.includes(searchText) || formattedName.includes(searchText) || description.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    // Handle click on a method name in the dropdown
    async onClickMethodName(methodName) {
        // Find the selected method data
        this.selectedMethodData = this.apiMethods.find(method => method.MethodInfo.Name === methodName);

        const formattedName = CommonTools.CamelPascalCaseToSpaced(methodName);

        // Update the selected method name in the button
        const buttonTextHolder = $(`#${this.ElementID}`).find('.selected-method-name');
        buttonTextHolder.html(formattedName);

        // Enable the Generate button
        const generateButton = document.getElementById(`${this.ElementID}_generateButton`);
        generateButton.disabled = false;

        // Update the method description
        const methodDescriptionDiv = document.getElementById(`${this.ElementID}_methodDescription`);
        const description = this.selectedMethodData.Description || 'No description available.';
        methodDescriptionDiv.innerHTML = `<p><iconify-icon class="me-1" style="margin-bottom: -7px;" icon="icon-park:info" width="25" height="25"></iconify-icon>${description}</p>`;

        // Generate the parameter inputs
        const parametersDiv = document.getElementById(`${this.ElementID}_parameters`);
        parametersDiv.innerHTML = this.generateParametersHtml();

        // Initialize help text icons in the parameters
        HelpTextIcon.InitAllIn(`#${this.ElementID}_parameters`);

        // Initialize TimeLocationInput instances
        this.timeInputInstances = {}; // reset the dictionary
        this.timeLocationInputParams.forEach(item => {
            const timeLocationInstance = new TimeLocationInput(item.id);
            // If default value exists, set it
            if (item.defaultValue) {
                // Assuming item.defaultValue is in a suitable format for setInputDateTime()
                timeLocationInstance.setInputDateTime(item.defaultValue);
            }
            // Save the instance in the timeInputInstances dictionary with paramName as key
            this.timeInputInstances[item.paramName] = timeLocationInstance;
        });
    }

    // Method to generate the HTML for parameters
    generateParametersHtml() {
        const method = this.selectedMethodData;
        const methodInfo = method.MethodInfo;

        // Storage for TimeInput IDs and names
        const timeLocationInputParams = [];

        let html = '';

        // For each parameter
        methodInfo.Parameters.forEach(param => {
            const paramName = param.Name;
            const formatedParamName = CommonTools.CamelPascalCaseToSpaced(param.Name);
            const paramDescription = param.Description;
            const paramType = param.ParameterType;
            const defaultValue = param.DefaultValue || '';

            let inputHtml = '';

            // Depending on the paramType, create suitable input
            if (paramType === 'VedAstro.Library.Time') {
                // For Time parameters, we can create a TimeLocationInput component
                // Generate unique IDs for the input elements
                const timeInputId = `${this.ElementID}_timeInput_${paramName}`;

                inputHtml = `
                <div class="mb-3">
                    <label class="form-label hstack gap-2">
                        ${formatedParamName}
                        <div class="help-text-icon">
                            ${paramDescription || 'No description'}
                        </div>
                    </label>
                    <div id="${timeInputId}"></div>
                </div>
                `;

                // Store the ID, paramName, and defaultValue for later initialization
                timeLocationInputParams.push({ id: timeInputId, paramName: paramName, defaultValue });

            } else if (paramType === 'VedAstro.Library.HouseName') {
                // For HouseName parameters, create a dropdown
                const inputId = `${this.ElementID}_input_${paramName}`;

                // Generate options: All, House1 to House12
                let options = '<option value="All">All</option>';
                for (let i = 1; i <= 12; i++) {
                    const value = `House${i}`;
                    const display = `House ${i}`;
                    options += `<option value="${value}">${display}</option>`;
                }

                inputHtml = `
                <div class="mb-3">
                    <label class="form-label hstack gap-2">
                        ${formatedParamName}
                        <div class="help-text-icon">
                            ${paramDescription || 'No description'}
                        </div>
                    </label>
                    <select class="form-control" id="${inputId}" name="${paramName}">
                        ${options}
                    </select>
                </div>
                `;

            } else if (paramType === 'VedAstro.Library.PlanetName') {
                // For PlanetName parameters, create a dropdown
                const inputId = `${this.ElementID}_input_${paramName}`;

                // Generate options: All, Sun, Moon, Mars, Mercury, Jupiter, Venus, Saturn, Rahu, Ketu
                const planets = ['All', 'Sun', 'Moon', 'Mars', 'Mercury', 'Jupiter', 'Venus', 'Saturn', 'Rahu', 'Ketu'];
                const options = planets.map(planet => `<option value="${planet}">${planet}</option>`).join('');

                inputHtml = `
                <div class="mb-3">
                    <label class="form-label hstack gap-2">
                        ${formatedParamName}
                        <div class="help-text-icon">
                            ${paramDescription || 'No description'}
                        </div>
                    </label>
                    <select class="form-control" id="${inputId}" name="${paramName}">
                        ${options}
                    </select>
                </div>
                `;
            } else if (paramType === 'System.Int32' || paramType === 'System.Double') {
                // For integer or double parameters, create number input
                const inputId = `${this.ElementID}_input_${paramName}`;
                inputHtml = `
                <div class="mb-3">
                    <label class="form-label hstack gap-2">
                        ${formatedParamName}
                        <div class="help-text-icon">
                            ${paramDescription || 'No description'}
                        </div>
                    </label>
                    <input type="number" class="form-control" id="${inputId}" name="${paramName}" value="${defaultValue}">
                </div>
                `;
            } else {
                // For other types, use text input
                const inputId = `${this.ElementID}_input_${paramName}`;
                inputHtml = `
                <div class="mb-3">
                    <label class="form-label hstack gap-2">
                        ${formatedParamName}
                        <div class="help-text-icon">
                            ${paramDescription || 'No description'}
                        </div>
                    </label>
                    <input type="text" class="form-control" id="${inputId}" name="${paramName}" value="${defaultValue}">
                </div>
                `;
            }

            html += inputHtml;
        });

        // Store the timeInputParams array for later use
        this.timeLocationInputParams = timeLocationInputParams;

        return html;
    }

    // Method to handle the "Generate" button click
    async onGenerateButtonClick() {
        // Save the current server address to localStorage
        const serverAddress = this.getServerAddress();
        localStorage.setItem(this.ServerAddressStorageKey, serverAddress);

        const method = this.selectedMethodData;
        const methodInfo = method.MethodInfo;

        let url = `${serverAddress}/api/Calculate/${methodInfo.Name}/`;

        const params = methodInfo.Parameters;

        for (let param of params) {
            const paramName = param.Name;
            const paramType = param.ParameterType;

            let paramValue = null;

            if (paramType === 'VedAstro.Library.Time') {
                const timeInputInstance = this.timeInputInstances[paramName];
                if (!timeInputInstance.isValid()) {
                    Swal.fire({
                        icon: 'error',
                        title: `Please fill in the <strong>"${CommonTools.CamelPascalCaseToSpaced(paramName)}"</strong> field correctly.`
                    });
                    return;
                }
                // Get the time object
                const timeData = await timeInputInstance.getTimeJson(); // returns object with StdTime and Location

                const stdTime = timeData.StdTime; // "13:54 25/10/1992 +08:00"

                // Build the URL segment for the Time parameter
                const [timePart, datePart, tzPart] = stdTime.split(' ');
                const [day, month, year] = datePart.split('/');
                const locationName = timeData.Location.Name;
                const timeUrlSegment = `Location/${locationName}/Time/${timePart}/${day}/${month}/${year}/${tzPart}/`;

                url += timeUrlSegment;
            } else if (paramType === 'VedAstro.Library.HouseName' || paramType === 'VedAstro.Library.PlanetName') {
                // Handle both HouseName and PlanetName parameters
                const selectElement = document.getElementById(`${this.ElementID}_input_${paramName}`);
                paramValue = selectElement.value;
                if (paramValue === "") {
                    Swal.fire({
                        icon: 'error',
                        title: `Please select a value for <strong>"${CommonTools.CamelPascalCaseToSpaced(paramName)}"</strong>.`
                    });
                    return;
                }
                // Append to URL
                url += `${CommonTools.camelCaseToPascalCase(paramName)}/${paramValue}/`;
            } else if (paramType === 'System.Int32' || paramType === 'System.Double') {
                const inputElement = document.getElementById(`${this.ElementID}_input_${paramName}`);
                paramValue = inputElement.value;
                if (paramValue === "") {
                    Swal.fire({
                        icon: 'error',
                        title: `Please fill in the <strong>"${CommonTools.CamelPascalCaseToSpaced(paramName)}"</strong> field.`
                    });
                    return;
                }
                // For int parameters, we can append directly
                url += `${CommonTools.camelCaseToPascalCase(paramName)}/${paramValue}/`;
            } else {
                // For other types, get the value from input field
                const inputElement = document.getElementById(`${this.ElementID}_input_${paramName}`);
                paramValue = inputElement.value;
                if (paramValue === "") {
                    Swal.fire({
                        icon: 'error',
                        title: `Please fill in the <strong>"${CommonTools.CamelPascalCaseToSpaced(paramName)}"</strong> field.`
                    });
                    return;
                }
                // Append to URL
                url += `${CommonTools.camelCaseToPascalCase(paramName)}/${encodeURIComponent(paramValue)}/`;
            }
        }


        // Remove any double slashes
        url = url.replace(/([^:]\/)\/+/g, "$1");

        // Output the generated URL
        const outputDiv = document.getElementById(`${this.ElementID}_output`);
        outputDiv.innerHTML = `

        <div id="ParamOutputOptionsPanel" class="vstack gap-3">
            <!--ARROW DOWN ICON-->
            <div class="" style="text-align: center;">
                <iconify-icon icon="flat-color-icons:down" width="80" height="80"></iconify-icon>
            </div>

            <!--URL OUT-->
            <div class="vstack gap-1" style="margin-top: -32px;">
                <span style="font-size: 14px; color: #8f8f8f;">URL</span>
                <kbd id="UrlDisplayOut" style="padding: 12px; font-size: 18px; overflow-wrap: break-word; line-height: 33px;">${url}</kbd>
            </div>

            <!--BUTTON ROW-->
            <div class="d-flex justify-content-between">
                <button id="${this.ElementID}_viewSourceCodeButton" style="height:37.1px; width: fit-content;" class="btn-sm hstack gap-2 btn-primary btn">
                    <iconify-icon icon="streamline:programming-browser-code-2-code-browser-tags-angle-programming-bracket" width="25" height="25"></iconify-icon>
                    View Code
                </button>
                <button id="${this.ElementID}_copyUrlButton" style="height:37.1px; width: fit-content;" class="btn-sm hstack gap-2 btn-primary btn">
                    <iconify-icon icon="carbon:link" width="25" height="25"></iconify-icon>
                    Copy URL
                </button>
                <button id="${this.ElementID}_callApiButton" style="height:37.1px; width: fit-content;" class="btn-sm hstack gap-2 btn-success btn">
                    <iconify-icon icon="ph:phone-call-light" width="25" height="25"></iconify-icon>
                    Call API
                </button>
            </div>
        </div>
            `;

        // Add event listeners to the new buttons
        // Call API button
        document.getElementById(`${this.ElementID}_callApiButton`).addEventListener('click', () => this.callApi(url));

        // Copy URL button
        document.getElementById(`${this.ElementID}_copyUrlButton`).addEventListener('click', () => {
            navigator.clipboard.writeText(url);
            Swal.fire({
                icon: 'success',
                title: 'URL copied to clipboard'
            });
        });

        // View Source Code button
        document.getElementById(`${this.ElementID}_viewSourceCodeButton`).addEventListener('click', () => this.onClickViewSourceCode());
    }

    // Method to call the API and open the URL in a new tab
    async callApi(url) {
        window.open(url, '_blank');
    }

    // Method to handle the "View Source Code" button click
    onClickViewSourceCode() {
        const metadata = this.selectedMethodData;

        const lineNumber = metadata.LineNumber;

        // Construct the GitHub link
        const searchLink = `https://github.com/VedAstro/VedAstro/blob/master/Library/Logic/Calculate/Calculate.cs#L${lineNumber}`;

        // Open link in new tab
        window.open(searchLink, '_blank');
    }

    // Method to get the server address from input or default
    getServerAddress() {
        let serverAddress = document.getElementById('ServerAddressInput').value;
        if (!serverAddress) {
            serverAddress = 'http://localhost:7071';
        }
        return serverAddress;
    }
}
