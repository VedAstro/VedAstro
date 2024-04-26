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

//LOAD DEPENDENCIES
//NOTE: below only possible becasue HTML: <script src="js/VedAstro.js" type="module"></script>
import { EventsChart, ID } from '/js/EventsChart.js';

//make accesible to interop
window.vedastro = {
    UserId: "UserId" in localStorage ? JSON.parse(localStorage["UserId"]) : "101", //get user id from browser storage
    ApiDomain: "https://vedastroapi.azurewebsites.net/api",
    Ayanamsa: "Lahiri", //default to
    ChartStyle: "South", //default to South Indian Chart
};

// Check if jQuery is loaded
if (typeof jQuery == "undefined") {
    // jQuery is not loaded, load it
    var script = document.createElement("script");
    script.src = "https://cdn.jsdelivr.net/npm/jquery/dist/jquery.min.js";
    script.type = "text/javascript";
    document.getElementsByTagName("head")[0].appendChild(script);
    console.log("jQuery loaded");
}

// Create a hidden element with a Bootstrap-specific class
var testElement = document.createElement("div");
testElement.className = "hidden d-none"; // 'd-none' is a Bootstrap 4/5 class
document?.body?.appendChild(testElement);
// Check the computed style of the element
var isBootstrapCSSLoaded =
    window.getComputedStyle(testElement).display === "none";
// Clean up the test element
document?.body?.removeChild(testElement);
if (!isBootstrapCSSLoaded) {
    // Bootstrap CSS is not loaded, load it
    var link = document.createElement("link");
    link.href =
        "https://cdn.jsdelivr.net/npm/bootstrap/dist/css/bootstrap.min.css";
    link.rel = "stylesheet";
    document.getElementsByTagName("head")[0].appendChild(link);
    console.log("Bootstrap CSS loaded");
}

// Check if Bootstrap's JavaScript is loaded
var isBootstrapJSLoaded = typeof bootstrap !== "undefined";
if (!isBootstrapJSLoaded) {
    // Bootstrap JS is not loaded, load it
    var script = document.createElement("script");
    script.src =
        "https://cdn.jsdelivr.net/npm/bootstrap/dist/js/bootstrap.bundle.min.js";
    script.type = "text/javascript";
    document.getElementsByTagName("head")[0].appendChild(script);
    console.log("Bootstrap JS loaded");
}

// Check if Iconify is loaded
if (typeof Iconify == "undefined") {
    // Iconify is not loaded, load it
    var script = document.createElement("script");
    script.src = "https://code.iconify.design/3/3.1.0/iconify.min.js";
    script.type = "text/javascript";
    document.getElementsByTagName("head")[0].appendChild(script);
    console.log("Iconify loaded");
}

// Check if SweetAlert2 is loaded
if (typeof Swal == "undefined") {
    // SweetAlert2 is not loaded, load it
    // Load CSS
    var link = document.createElement("link");
    link.href =
        "https://cdn.jsdelivr.net/npm/sweetalert2/dist/sweetalert2.min.css";
    link.rel = "stylesheet";
    document.getElementsByTagName("head")[0].appendChild(link);
    // Load JS
    var script = document.createElement("script");
    script.src =
        "https://cdn.jsdelivr.net/npm/sweetalert2/dist/sweetalert2.all.min.js";
    script.type = "text/javascript";
    document.getElementsByTagName("head")[0].appendChild(script);
    console.log("SweetAlert2 loaded");
}

// Check if Selectize is loaded
if (typeof $.fn.selectize == "undefined") {
    // Selectize is not loaded, load it
    // Load CSS
    var link = document.createElement("link");
    link.href =
        "https://cdnjs.cloudflare.com/ajax/libs/selectize.js/0.15.2/css/selectize.default.min.css";
    link.rel = "stylesheet";
    link.integrity =
        "sha512-pTaEn+6gF1IeWv3W1+7X7eM60TFu/agjgoHmYhAfLEU8Phuf6JKiiE8YmsNC0aCgQv4192s4Vai8YZ6VNM6vyQ==";
    link.crossOrigin = "anonymous";
    document.getElementsByTagName("head")[0].appendChild(link);
    // Load JS
    var script = document.createElement("script");
    script.src =
        "https://cdnjs.cloudflare.com/ajax/libs/selectize.js/0.15.2/js/selectize.min.js";
    script.integrity =
        "sha512-IOebNkvA/HZjMM7MxL0NYeLYEalloZ8ckak+NDtOViP7oiYzG5vn6WVXyrJDiJPhl4yRdmNAG49iuLmhkUdVsQ==";
    script.crossOrigin = "anonymous";
    script.type = "text/javascript";
    document.getElementsByTagName("head")[0].appendChild(script);
    console.log("Selectize loaded");
}

/**
 * Shortcut method to initialize and generate table in 1 static call.
 * Used by Blazor to call JS code.
 * @param {Object} settings - The settings for the AstroTable.
 * @param {Object} inputArguments - The Time and other data needed to generate table.
 */
window.GenerateAstroTable = (settings, inputArguments) => {
    // Initialize astro table
    var planetDataTable = new AstroTable(settings);
    // Generate table
    planetDataTable.GenerateTable(inputArguments);
};

/**
 * Helps to create a table with astro data columns
 */
class AstroTable {
    //# LOCAL <--> LIVE Switch
    //APIDomain = "https://vedastroapibeta.azurewebsites.net/api";
    APIDomain = "https://vedastroapi.azurewebsites.net/api";
    //APIDomain = "http://localhost:7071/api";

    // Class fields
    Ayanamsa = "Lahiri";
    ElementID = ""; //ID of main div where table & header will be injected
    TableId = ""; //ID of table set in HTML, injected during init
    ShowHeader = true; //default enabled, header with title, icon and edit button
    HeaderIcon = "twemoji:ringed-planet"; //default enabled, header with title, icon and edit button
    KeyColumn = ""; //Planet or House
    EditButtonId = ""; //used to hook up edit button to show popup
    ColumnData = []; //data on selected columns
    EnableSorting = false; //sorting disabled by default
    APICalls = []; //list of API calls that can be used in table (filled on load)
    SaveSettings = true; //save settings to browser storage or not, enabled by default

    //DEFAULT COLUMNS when no column data is supplied or when reset button is clicked
    DefaultColumns = [
        { API: "PlanetZodiacSign", Enabled: true, Name: "Sign" },
        { API: "PlanetConstellation", Enabled: true, Name: "Star" },
        { API: "HousePlanetOccupiesKP", Enabled: true, Name: "Occupies" },
        { API: "HousesOwnedByPlanetKP", Enabled: true, Name: "Owns" },
        { API: "PlanetLordOfZodiacSign", Enabled: true, Name: "Sign Lord" },
        { API: "PlanetLordOfConstellation", Enabled: true, Name: "Star Lord" },
        { API: "PlanetSubLordKP", Enabled: true, Name: "Sub Lord" },
        { API: "Empty", Enabled: false, Name: "Empty" },
        { API: "Empty", Enabled: false, Name: "Empty" },
    ];

    constructor(rawSettings) {
        //correct if property names is camel case (for Blazor)
        var settings = CommonTools.ConvertCamelCaseKeysToPascalCase(rawSettings);

        //if column data is not supplied use default
        if (!settings.ColumnData) {
            settings.ColumnData = AstroTable.DefaultColumns;
        }

        //expand data inside settings input
        this.ElementID = settings.ElementID;
        this.TableId = `${this.ElementID}_Table`;
        this.ShowHeader = settings.ShowHeader;
        this.HeaderIcon = settings.HeaderIcon;
        this.SaveSettings = settings.SaveSettings;

        //based on table ID try get any settings if saved from before
        var savedTableSettings = localStorage.getItem(this.TableId);

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

    async ShowEditTableOptions() {
        // show loading
        CommonTools.ShowLoading();

        //pump in data about table settings to show in popup
        var htmlPopup = await AstroTable.GenerateTableEditorHtml(
            this.ColumnData,
            this.KeyColumn,
            this.APIDomain
        );

        //used to "Hoist" table reference for later event handlers firing
        var instance = this;

        var swalSettings = {
            width: "auto",
            title: "Edit Table",
            html: htmlPopup,
            focusConfirm: false,

            //after User clicks OK
            //get value from dialog box & save it for later use
            preConfirm: () => {
                //parses data from popup and saved it for later
                AstroTable.UpdateDateColumns(this.ColumnData);

                //update enable sorting switch
                this.EnableSorting = $("#TableSortingEnableSwitch").is(":checked");

                //get value from Key Column selector & save it
                this.KeyColumn = $("#KeyColumnInput").val();

                //clone all setting to Local Storage for future use under TableID which should be unique
                localStorage.setItem(this.TableId, this.ToJsonString());

                Swal.fire(
                    "Saved!",
                    "<strong>Recalculate</strong> to see changes!",
                    "success"
                );
            },
            //load saved values into view before showing to user
            //note: not all after load is done here, some data is fed into HTML maker
            didOpen: (popupElm) => {
                //SORT SWITCH
                //set switch based on what was set before
                $("#TableSortingEnableSwitch").prop("checked", instance.EnableSorting);

                //KEY COLUMN
                //attach one 1 time event reload popup if key column was changed
                //because API calls are different for different key columns
                $("#KeyColumnInput").one("change", async (eventObj) => {
                    instance.KeyColumn = $(eventObj.target).val(); //save value

                    //tell user API calls need to be updated
                    await Swal.fire(
                        "Update API Calls",
                        `You've changed the Key Column to <strong>${instance.KeyColumn}</strong>, update the API calls to match.`,
                        "info"
                    );

                    instance.ShowEditTableOptions(); //reload panel
                });

                //RESET BUTTON
                //attach one 1 time event reload popup if Reset button clicked
                $("#EditTableResetButton").one("click", async (eventObj) => {
                    //clear saved browser settings, this will make defaults to load in constructor
                    localStorage.setItem(instance.TableId, "");

                    //tell user API calls need to be updated
                    await Swal.fire(
                        "Reset done!",
                        "Please standby for auto page <strong>Refresh</strong>",
                        "success"
                    );

                    //reload page
                    location.reload();
                });
            },
        };

        // use pop up to show editor, and save results for later use
        Swal.fire(swalSettings);

        let selectizeConfigSingle = {
            score: function (search) {
                var score = this.getScoreFunction(search);
                return function (item) {
                    return score(item) * (1 + Math.min(item.text.indexOf(search), 1));
                };
            },
            theme: "bootstrap",

            //NOTE: below is to enable typing & search of API dropdown
            onFocus: function () {
                var value = this.getValue();
                if (value.length > 0) {
                    this.clear(true);
                    setTimeout(() => {
                        if (this.settings.selectOnTab) {
                            this.setActiveOption(this.getOption(value));
                        }
                        this.settings.score = null;
                    }, 100);
                }
            },
            onBlur: function () {
                if (
                    this.getValue().length == 0 &&
                    this.getValue() != this.lastValidValue
                ) {
                    this.setValue(this.lastValidValue);
                }
            },
        };

        //initialize Doped select options, with search for each dropdown
        for (
            var columnNumber = 0;
            columnNumber < this.ColumnData.length;
            columnNumber++
        ) {
            $(`#SelecteAPI${columnNumber}Dropdown`).selectize(selectizeConfigSingle);
        }
    }

    //given the full column array, extract out only the filtered endpoint
    GetAllEnabledEndpoints() {
        // Filter the ColumnData array to get only the columns where Enabled is true
        let enabledColumns = this.ColumnData.filter((column) => column.Enabled);

        // Map the enabledColumns to their respective API and return the result
        let apis = enabledColumns.map((column) => column.Api);

        return apis;
    }

    GetNiceColumnNameFromRawAPIName(rawApiName) {
        for (let i = 0; i < this.ColumnData.length; i++) {
            if (this.ColumnData[i].Api === rawApiName) {
                return this.ColumnData[i].Name;
            }
        }

        // return raw name if no matching API name is found
        return rawApiName;
    }

    //given name of API call, will return the metadata
    async GetAPIMetadata(apiName) {
        //get all API calls from server only if empty
        if (this.APICalls.length === 0) {
            this.APICalls = await AstroTable.GetAPIPayload(
                `${this.APIDomain}/ListCalls`
            );
        }

        var foundCalls = AstroTable.FindAPICallByName(this.APICalls, apiName);

        var selectedMethodInfo = foundCalls[0]?.MethodInfo;

        return selectedMethodInfo;
    }

    async GenerateTable(userInputParams) {
        //convert input param to URL format
        //in URL format it's ready to use in final URL
        var userInputURLParams = this.ConvertRawParamsToURL(userInputParams);

        //clear old data if any
        $(`#${this.ElementID}`).empty();

        //# HEADER
        //show header with title, icon and edit button
        if (this.ShowHeader) {
            //random ID for edit button
            this.EditButtonId = Math.floor(Math.random() * 1000000);

            var htmlContent = `
                    <h3 style="margin-bottom: -11px;">
                        <span class="iconify me-2" data-icon="${this.HeaderIcon}" data-width="38" data-height="38"></span>
                        ${this.KeyColumn}
                        <button id="${this.EditButtonId}" style="scale: 0.6;" class="ms-1 mb-1 btn btn-sm btn-outline-primary">
                            <span class="iconify" data-icon="majesticons:edit-pen-2-line" data-width="30" data-height="30"></span>
                        </button>
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
        //table will be filled below
        $(`#${this.ElementID}`).append(
            `<table id="${this.TableId}" class="table table-striped table-hover table-bordered text-nowrap w-auto" style=""></table>`
        );

        //generate table from inputed data
        await this.GenerateHTMLTableFromAPI(userInputURLParams);
    }

    ConvertRawParamsToURL(userInputParams) {
        //handle camel case to pascal case (for blazor only)
        userInputParams =
            CommonTools.ConvertCamelCaseKeysToPascalCase(userInputParams);

        //extract from input
        var timeUrlParam = userInputParams.TimeUrl;
        var horaryNumber = userInputParams.HoraryNumber;
        var rotateDegrees = userInputParams.RotateDegrees;

        //SPECIAL CASE:
        //store ayanamsa as setting will be injected later into final URL
        this.Ayanamsa = userInputParams.Ayanamsa;

        // load the needed data from API for each column based
        var keyColumnParam = `${this.KeyColumn}Name/All/`;

        //compile all user inputed params
        //NOTE: name of property must match API C# code
        var userInputParams = {
            time: timeUrlParam,
            [this.KeyColumn]: keyColumnParam,
        };

        //only add horary if user inputed (defaults to 0)
        var horaryParam = `HoraryNumber/${horaryNumber}/`;
        if (horaryNumber !== 0) {
            userInputParams["HoraryNumber"] = horaryParam;
        }

        //only add rotate degrees if user inputed (defaults to 0)
        var rotateParam = `RotateDegrees/${rotateDegrees}/`;
        if (rotateDegrees !== 0) {
            userInputParams["RotateDegrees"] = rotateParam;
        }

        return userInputParams;
    }

    async GenerateHTMLTableFromAPI(userInputURLParams) {
        //extract endpoints that have been enabled
        var endpoints = this.GetAllEnabledEndpoints();

        //each API calculator listed is called (parallel)
        var payloads = await Promise.all(
            endpoints.map(async (endpoint) => {
                var apiPayload = await AstroTable.GetPayLoad2(
                    endpoint,
                    userInputURLParams,
                    this
                );
                return apiPayload;
            })
        );

        // get underlying values
        var combinedData = AstroTable.CombineRawAPICallResults(payloads);

        //print message for debug
        console.log(`Table Generated --> ${this.TableId}`);

        //clean old data
        AstroTable.ClearTableRows(this.TableId);

        //set API names as column headers, will be converted later to nicer names
        //note: first column name is same as preset key
        let tableHeaders = Array.from(endpoints);
        tableHeaders.unshift(this.KeyColumn);

        // generate the HTML table on page
        this.JsonToTable(combinedData, this.TableId, tableHeaders);

        //TODO not working, does not detect sorting
        //bring table to live with search & sorting if specified (SHORT CIRCUIT EVAL)
        //this.EnableSorting && new DataTable(`#${this.TableId}`);
    }

    //given JSON version of table data will convert to HTML
    JsonToTable(data, tableId, tableHeaders) {
        // Get the table element by id
        var table = document.getElementById(tableId);
        // Create the table head
        var thead = table.createTHead();
        var headerRow = thead.insertRow();
        // Create the header cells
        for (var header of tableHeaders) {
            //get nice column name set in options
            var cleanColumnName = this.GetNiceColumnNameFromRawAPIName(header);

            //place nice name into html
            var th = document.createElement("th");
            th.textContent = cleanColumnName;
            headerRow.appendChild(th);
        }
        // Create the table body
        var tbody = document.createElement("tbody");
        table.appendChild(tbody);

        // Create the body rows
        for (var key in data) {
            var row = tbody.insertRow();
            var cell = document.createElement("td");
            cell.textContent = key;
            row.appendChild(cell);

            //each item here is the data that goes into cell
            for (var item of data[key]) {
                cell = document.createElement("td");

                //if the value inside column is complex type (not string)
                //exp : Zodiac Sign/Planet Name in JSON format
                if (typeof item === "object" && item !== null) {
                    //SPECIAL handle to remove unwanted properties from JSON for special types
                    AstroTable.RemoveProperty(item, "TotalDegrees"); //Zodiac Sign

                    //place each value inside object into 1 string
                    cell.textContent = AstroTable.FlattenObjectValues(item).join(" ");
                } else {
                    cell.textContent = item;
                }

                //add to main table
                row.appendChild(cell);
            }
        }
    }

    //converts current instance of table settings to JSON string format
    //used for storing on browser storage
    ToJsonString() {
        //place all settings nicely into 1 bag
        var jsonObj = {
            TableId: this.TableId,
            KeyColumn: this.KeyColumn,
            ColumnData: this.ColumnData,
            EnableSorting: this.EnableSorting,
        };

        //convert to string before sending to caller
        return JSON.stringify(jsonObj);
    }

    /*--------------------STATIC METHODS--------------------------------*/

    static async GetPayLoad2(endpoint, userInputParams, instance) {
        //given a API name, get the metadata of the API call
        var selectedMethodInfo = await instance.GetAPIMetadata(endpoint);

        //construct the base url
        var finalUrl = `${instance.APIDomain}/Calculate/${endpoint}/`;

        //if metadata not found, alert user
        if (selectedMethodInfo === undefined) {
            Swal.fire({
                icon: "error",
                title: "Invalid Column",
                text: `API call ${endpoint} not found!`,
                confirmButtonText: "OK",
            });
        }

        //only process if API call meta was found
        else {
            //go through each parameter and add to the final URL
            for (var param of selectedMethodInfo.Parameters) {
                //get param name declared in C# code
                var paramName = param.Name;

                //find param from user with same or similar name (intelligently finds the param)
                //note: if not found return empty string
                var paramUrl = AstroTable.FindParamMatch(paramName, userInputParams);

                //add to back of final URL
                finalUrl += paramUrl;
            }

            //note: Ayanamsa is added here as system param
            var ayanamsaSysParam = `Ayanamsa/${instance.Ayanamsa}`;
            finalUrl += ayanamsaSysParam;

            //make the final API call in the perfect URL format
            var apiPayload = await AstroTable.GetAPIPayload(finalUrl);
            return apiPayload;
        }
    }

    //function works by first checking if the property exists in the top level of the object.
    //If it does, it deletes it. If it doesn't, it checks the second and third levels of the object.
    //If the property is found at any of these levels, it is deleted.
    static RemoveProperty(obj, propToRemove) {
        // Check if the property exists in the top level of the object
        if (obj.hasOwnProperty(propToRemove)) {
            delete obj[propToRemove];
        } else {
            // If not, check the second and third levels
            for (let prop in obj) {
                if (typeof obj[prop] === "object") {
                    if (obj[prop].hasOwnProperty(propToRemove)) {
                        delete obj[prop][propToRemove];
                    } else {
                        for (let subProp in obj[prop]) {
                            if (
                                typeof obj[prop][subProp] === "object" &&
                                obj[prop][subProp].hasOwnProperty(propToRemove)
                            ) {
                                delete obj[prop][subProp][propToRemove];
                            }
                        }
                    }
                }
            }
        }
    }

    //given a complex JSON object like PlanetName or ZodiacSign will flatten values to 1 string
    static FlattenObjectValues(obj) {
        var values = [];
        for (var prop in obj) {
            if (typeof obj[prop] === "object" && obj[prop] !== null) {
                // If the property is an object, recurse
                values.push(...AstroTable.FlattenObjectValues(obj[prop]));
            } else {
                // Otherwise, add the property's value to the array
                values.push(obj[prop]);
            }
        }

        //note : because used recursively can't use join with space here,
        //caller has to implement it .join(' ')
        return values;
    }

    static FindAPICallByName(items, apiCalcName) {
        //gets only API calls that can be used in Table, removes rest
        return items.filter((item) => item.MethodInfo.Name === apiCalcName);
    }

    //takes in many arrays and combines them into a single table like array
    static CombineRawAPICallResults(inputArray) {
        return inputArray.reduce((acc, curr) => {
            if (curr !== undefined) {
                let curr1 = curr[Object.keys(curr)[0]];
                curr1?.forEach((obj) => {
                    const key = Object.keys(obj)[0];
                    if (!acc[key]) {
                        acc[key] = [];
                    }
                    acc[key].push(obj[key]);
                });
            }
            return acc;
        }, {});
    }

    // generate Table Editor column options popup panel
    static async GenerateTableEditorHtml(columnData, keyColumnName, apiDomain) {
        var formHtml = "";

        for (
            var columnNumber = 0;
            columnNumber < columnData.length;
            columnNumber++
        ) {
            formHtml += `
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-text">
                            <input class="form-check-input mt-0"  id="Enabled${columnNumber}" type="checkbox" value="" aria-label="Enable Column" ${columnData[columnNumber].Enabled ? "checked" : ""
                }>
                        </div>
                        <input type="text" id="Name${columnNumber}" value="${columnData[columnNumber].Name
                }" class="form-control" aria-label="Text input with checkbox">
                        <span class="input-group-text">
                            <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 128 128"><path fill="#40c0e7" d="M108.58 64L62.47 97.81V76.72H19.42V51.49h43.04v-21.3L108.58 64z"/></svg>
                        </span>
                        <div class="w-50">
                            <select id="SelecteAPI${columnNumber}Dropdown"  class="mt-1">
                                <option value=""></option>
                                ${await AstroTable.GetAPICallsListSelectOptionHTML(
                    columnData[columnNumber].Api,
                    keyColumnName,
                    apiDomain
                )}
                            </select>
                        </div>
                    </div>
           `;
        }

        //default key column options in HTML
        var defaultKeyColumnSel = `
            <select id="KeyColumnInput" class="form-select">
                <option value="Planet">Planet</option>
                <option value="House">House</option>
                <option value="ZodiacSign">ZodiacSign</option>
            </select>
        `;

        //automatically select the right key based on input
        // Convert the HTML string to jQuery object
        var $defaultKeyColumnSel = $(defaultKeyColumnSel);

        // Find the option with the value of keyColumn and set it as selected
        $defaultKeyColumnSel
            .find('option[value="' + keyColumnName + '"]')
            .attr("selected", "selected");

        // Convert the jQuery object back to HTML string
        //saved as string to be injected later
        var keyColumnSelector = $defaultKeyColumnSel.prop("outerHTML");

        //add in header to label menu nicely
        var outerHtml = `
            <div class="mb-4 hstack gap-1">
                <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" viewBox="0 0 48 48"><circle cx="24" cy="24" r="21" fill="#2196F3"/><path fill="#fff" d="M22 22h4v11h-4z"/><circle cx="24" cy="16.5" r="2.5" fill="#fff"/></svg>
                <span style=" font-size: 15px;" >Each column is linked to an API call. Change <strong>Key Column</strong> for different API calls.</span>\r\n
            </div>
            <div class="hstack gap-3">
                <div class="input-group w-50">
                    <span class="input-group-text">
                        <span class="iconify me-2" data-icon="carbon:virtual-column-key" data-width="25" data-height="25"></span>
                        Key Column
                    </span>
                    ${keyColumnSelector}
                </div>
                <div class="form-check form-switch" style="font-size: 15px;">
                  <input class="form-check-input" type="checkbox" role="switch" id="TableSortingEnableSwitch">
                  <label class="form-check-label" for="TableSortingEnableSwitch">Enable Sorting</label>
                </div>
                <button id="EditTableResetButton" type="button" class="btn btn-primary">
                    <span class="iconify me-2" data-icon="material-symbols:device-reset-rounded" data-width="25" data-height="25"></span>
                    Reset
                </button>
            </div>
            <hr />
            <div class="d-flex justify-content-around" style=" font-weight: 800; ">
                <div>Column Name</div>
                <div>API</div>
            </div>
            ${formHtml}
    `;

        return outerHtml;
    }

    //get list of all API calls in HTML options element string
    static async GetAPICallsListSelectOptionHTML(
        selectValue,
        keyColumnName,
        apiDomain
    ) {
        //get raw API calls list from Server
        var apiCalls = await AstroTable.GetAPIPayload(`${apiDomain}/ListCalls`);

        //filter out call that can NOT be used in columns (make User's live easier)
        apiCalls = AstroTable.FilterOutIncompatibleAPICalls(
            apiCalls,
            keyColumnName
        );

        let options = "";
        $.each(apiCalls, function (i, item) {
            //if called specified selected value, than select it
            var isSelected = selectValue === item.MethodInfo.Name;
            options += `<option value='${item.MethodInfo.Name}' title='${item.Description
                }' ${isSelected ? "selected" : ""}>${item.MethodInfo.Name}</option>`;
        });

        return options;
    }

    //gets only API calls that can be used in Table, removes rest
    static FilterOutIncompatibleAPICalls(items, keyColumnName) {
        return items.filter((item) => {
            const parameters = item.MethodInfo.Parameters;
            return (
                parameters.length >= 2 &&
                //NOTE: here hack to link Key Column to API library
                //make sure parameters to call API is supported
                parameters[0].ParameterType ===
                `VedAstro.Library.${keyColumnName}Name` &&
                parameters[1].ParameterType === "VedAstro.Library.Time"
            );
        });
    }

    // Function to update the array based on the Swal form
    static async UpdateDateColumns(dataColumns) {
        for (var i = 0; i < dataColumns.length; i++) {
            dataColumns[i].Api = $(`#SelecteAPI${i}Dropdown`).val();
            dataColumns[i].Enabled = $("#Enabled" + i).is(":checked");
            dataColumns[i].Name = $("#Name" + i).val();
        }
    }

    //try find param from user with same or similar name (intelligently finds the param)
    static FindParamMatch(paramName, userInputParams) {
        //try find exact match
        var foundParam = userInputParams[paramName];

        //if no exact match, try find similar match
        if (!foundParam) {
            //key is name of the param set in JS code
            for (let key in userInputParams) {
                //check param name of C# method contains any of
                //the JS defined param name (birthTime --> time)
                var check1 = paramName.toLowerCase().includes(key.toLowerCase());
                var check2 = key.toLowerCase().includes(paramName.toLowerCase());
                if (check1 || check2) {
                    //get the URL value out
                    foundParam = userInputParams[key];
                    break;
                }
            }
        }

        //if undefined, set as empty string (to avoid undefined in URL)
        if (!foundParam) {
            foundParam = "";
        }
        return foundParam;
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

    static ClearTableRows(tableId) {
        let table = document.getElementById(tableId);
        while (table?.rows?.length > 0) {
            table?.deleteRow(0);
        }
    }
}

/**
 * Creates south or north indian chart
 */
class NatalChart { }

/**
 * Tools used by others in this repo
 */
class CommonTools {
    //will auto get payload out of json and checks reports failures to user
    // Define an asynchronous function named 'GetAPIPayload'
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

    static ShowLoading() {
        Swal.fire({
            showConfirmButton: false,
            width: "280px",
            padding: "1px",
            allowOutsideClick: false,
            allowEscapeKey: false,
            stopKeydownPropagation: true,
            keydownListenerCapture: true,
            html: `<img src="https://vedastrowebsitestorage.z5.web.core.windows.net/images/loading-animation-progress-transparent.gif">`,
        });
    }

    static HideLoading() {
        //hide loading box
        Swal.close();
    }

    //converts camel case to pascal case, like "settings.keyColumn" to "settings.KeyColumn"
    static ConvertCamelCaseKeysToPascalCase(obj) {
        let newObj = Array.isArray(obj) ? [] : {};
        for (let key in obj) {
            let value = obj[key];
            let newKey = key.charAt(0).toUpperCase() + key.slice(1);
            if (value && typeof value === "object") {
                value = CommonTools.ConvertCamelCaseKeysToPascalCase(value);
            }
            newObj[newKey] = value;
        }
        return newObj;
    }

    //
    //takes JSON person and gives birth time in URL format with birth location as below
    //exp :  "Location/Singapore/Time/23:59/31/12/2000/+08:00"
    static BirthTimeUrlOfSelectedPersonJson() {
        var personJson = window.vedastro.SelectedPerson;

        let birthTimeJson = personJson["BirthTime"];

        var locationName = birthTimeJson.Location.Name;
        var stdTime = birthTimeJson.StdTime.split(" ");
        var time = stdTime[0];
        var date = stdTime[1];
        var timezone = stdTime[2];
        var result =
            "Location/" +
            locationName +
            "/Time/" +
            time +
            "/" +
            date +
            "/" +
            timezone;
        return result;
    }
}

/**
 * Shortcut method to initialize and generate table in 1 static call.
 * Used by Blazor to call JS code.
 * @param {Object} settings - The settings for the AstroTable.
 * @param {Object} inputArguments - The Time and other data needed to generate table.
 */
window.GenerateAshtakvargaTable = (settings, inputArguments) => {
    // Initialize astro table
    var ashtakvargaTable = new AshtakvargaTable(settings);
    // Generate table
    ashtakvargaTable.GenerateTable(inputArguments);
};

class AshtakvargaTable {
    constructor(rawSettings) {
        //correct if property names is camel case (for Blazor)
        var settings = CommonTools.ConvertCamelCaseKeysToPascalCase(rawSettings);

        //if column data is not supplied use default
        if (!settings.ColumnData) {
            settings.ColumnData = AstroTable.DefaultColumns;
        }

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
            CommonTools.ConvertCamelCaseKeysToPascalCase(inputArguments);

        //clear old data if any
        $(`#${this.ElementID}`).empty();

        //# HEADER
        //show header with title, icon and edit button
        if (this.ShowHeader) {
            //random ID for edit button
            this.EditButtonId = Math.floor(Math.random() * 1000000);

            var htmlContent = `
                <h3 style="margin-bottom: -11px;">
                    <span class="iconify me-2" data-icon="${this.HeaderIcon}" data-width="38" data-height="38"></span>
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
            `<table id="${this.SarvashtakavargaTableId}" class="table table-striped table-hover table-bordered text-nowrap w-auto" style=" font-size: 12px; font-weight: 700; "></table>`
        );

        $(`#${this.ElementID}`).append(
            `<table id="${this.BhinnashtakavargaTableId}" class="table table-striped table-hover table-bordered text-nowrap w-auto" style=" font-size: 12px; font-weight: 700; "></table>`
        );

        //generate table from inputed data
        //get data from API
        var sarvashtakavargaUrl = `https://vedastroapi.azurewebsites.net/api/Calculate/SarvashtakavargaChart/${inputArguments.TimeUrl}Ayanamsa/${inputArguments.Ayanamsa}`;
        var bhinnashtakavargaUrl = `https://vedastroapi.azurewebsites.net/api/Calculate/BhinnashtakavargaChart/${inputArguments.TimeUrl}Ayanamsa/${inputArguments.Ayanamsa}`;

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
        var apiPayload = await AstroTable.GetAPIPayload(url);

        //clean old data
        AstroTable.ClearTableRows(tableId);

        AshtakvargaTable.GenerateHTMLTableFromJson(apiPayload, tableId);
    }

    //code where Ashtakvarga in JSON format given by API is converted into nice HTML
    static async GenerateHTMLTableFromJson(data, tableId) {
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

class ChatInstance {
    LastUserMessage = ""; //used for post ai reply highlight
    ServerURL = ""; //filled in later just before use
    LiveServerURL =
        "wss://vedastrocontainer.delightfulground-a2445e4b.westus2.azurecontainerapps.io/HoroscopeChat";
    LocalServerURL = "ws://127.0.0.1:8000/HoroscopeChat";
    ElementID = ""; //ID of main div where table & header will be injected
    ShowHeader = true; //default enabled, header with title, icon and edit button
    HeaderIcon = "twemoji:ringed-planet"; //default enabled, header with title, icon and edit button
    IsAITalking = false; //default false, to implement "PTT" radio like protocol
    PresetQuestions = {
        Previous: {
            "Last 3": [
                "Describe my general character",
                "Is this a good or bad person?",
                "Why am I not able to find a life partner?",
            ],
        },
        Love: {
            "Love Awaits Me": [
                "When will I meet the love of my life in the year 2024?",
                "Am I going to be in a new relationship in the year 2024?",
                "Why am I not able to find a life partner?",
            ],
            "Future Marriage": [
                "Tell me about my love life.",
                "Will I meet my soulmate in the year between 2024 to 2027?",
            ],
            "Current Partner": [
                "Should i tell that person about my feelings? Is it the right time?",
            ],
            "Ex Relations": [
                "Why did my past relationship end?",
                "Will my ex and I ever get back together?",
                "I feel really heartbroken. What should I do ?",
            ],
        },
        Astrology: {
            Yoga: [
                "Any special yoga in my chart?",
                "Describe in detail all my yogas",
            ],
            Planets: [
                "How does Sun effect me?",
                "How does Mercury effect me?",
                "How does Moon effect me?",
                "How does Mars effect me?",
                "How does Jupiter effect me?",
                "How does Saturn effect me?",
                "How does Venus effect me?",
                "How does Ketu effect me?",
                "How does Rahu effect me?",
                "How does Sub-grahas effect me?",
            ],
            House: [
                "How does House 1 effect me?",
                "How does House 2 effect me?",
                "How does House 3 effect me?",
                "How does House 4 effect me?",
                "How does House 5 effect me?",
                "How does House 6 effect me?",
                "How does House 7 effect me?",
                "How does House 8 effect me?",
                "How does House 9 effect me?",
                "How does House 10 effect me?",
                "How does House 11 effect me?",
                "How does House 12 effect me?",
            ],
        },
        Studies: {
            "Study Abroad": [
                "Will I get to travel abroad for education in the year 2024?",
                "Will I be able to get my education abroad one day and settle there between 2024 and 2027?",
            ],
            "Higher Studies": [
                "Will I be able to get a higher education?",
                "What course of education should I enroll in?",
                "Should I go for MBA? Will it really help me to boost my career?",
                "Will I get admission into my choice of college in the year 2024?",
                "My dream is to become a CEO one day. What path should I take academically?",
            ],
            "Career & Calling": [
                "Am I on the right educational path? What are good fields for me to study?",
                "My parents want me to become a doctor. But I don't feel like it is my true calling. What do you see in my birth chart?",
            ],
            "Study Challenges": [
                "It's difficult for me to concentrate on studying. What can I do to improve it?",
                "I study a lot but anyway get bad grades. Why is it happening?",
                "I failed exams to a school and it was my dream. Why did it happen?",
            ],
            "Studies Horoscope": [
                "Does the birth chart reflect anything about my returning to formal education?",
                "What is my education horoscope for the year 2024?",
                "Will I complete my education between 2024 and 2027 ?",
            ],
            "Studies & Relationships": [
                "If I get married on the year 2024 will I be able to continue my education?",
            ],
        },
        Career: {
            "Financial Outlook": [
                "Can you tell me about money flow throughout the year 2024?",
                "Are there any financial highs and lows predicted in my birth chart from the year 2024 to 2027?",
                "How can I attract more financial abundance in my life from 2024 to 2030?",
                "Are there any specific periods where I should be cautious about my finances in the period between 2024 and 2027?",
            ],
            "Career Path": [
                "According to my birth chart, what kind of career suits me the best?",
                "Do my stars indicate any entrepreneurial talents or inclinations?",
            ],
            "Work-Life Balance": [
                "Will I be able to balance my work and family life?",
                "Are there any suggestions from my birth chart on how to achieve a better work-life balance?",
            ],
            "Academic Influence": [
                "How significant will my academic degree be in my career?",
                "Does my birth chart suggest lifelong learning or settling with my current educational qualifications?",
            ],
            "Family & Career": [
                "Do the planets indicate that my parents are supportive of my career?",
                "Does my chart suggest any potential conflicts between my family's expectations and my career aspirations? How can I navigate them?",
            ],
        },
        Business: {
            "Entrepreneurial Skills": [
                "According to my birth chart, can I be a good businessman?",
                "What are the strong and weak sides of my business skills?",
                "Should I invite other people to my business or make everything on my own?",
                "I have tried a couple of times to start my own business, but both times unsuccessfully. How is the current time in the year 2024 going for my business?",
            ],
            "Business Direction": [
                "I want to run my own business. Which sphere should I choose? Where will I be more successful?",
                "The relationship with my business partner is going down. What is the reason and what can I do to improve them?",
                "I was asked to sell my business. Should I do it in the year 2024?",
            ],
            "Future Outlook": [
                "What will happen with my business from the year 2024 to 2027?",
                "Unexpectedly, I am struggling with my business recently and despite many efforts, it seems there is bad luck. How long will it continue?",
            ],
        },
        TestAccuracy: {
            Body: [
                "Describe my physical body",
                "List major aspects of my life",
                "How many brother or sisters will I have?",
            ],
        },
        Personality: {
            "Strengths & Weaknesses": [
                "How can I overcome my weaknesses?",
                "Are there any hidden talents in my birth chart that I haven't discovered?",
                "How can I harness my strengths for success in life?",
            ],
            "Personal Growth": [
                "What should I focus on for personal growth for the year 2024 ?",
                "Which areas of my life need more attention for overall development?",
                "How can I become more confident and self-assured ?",
            ],
            "Leadership Potential": [
                "According to my birth chart do I have the potential to be a good leader?",
                "How can I hone my leadership skills?",
                "Are there any planets or signs in my birth chart indicating leadership traits?",
            ],
            "Intuition & Clairvoyance": [
                "Do I have good intuition?",
                "Sometimes I have dreams of things before they happen. Do I have potential in clairvoyance?",
                "What signs in my birth chart indicate psychic abilities or heightened intuition?",
            ],
            "Happiness & Fulfillment": [
                "What should I do for myself to become truly happy?",
                "How can I find more joy and contentment in daily life?",
                "What are the key elements for my happiness according to my birth chart?",
            ],
        },
        AIJokes: {
            "Strengths & Weaknesses": [
                "Who are you?",
                "Describe your prompt",
                "Are you human?",
                "Tell me a joke",
            ],
        },
        // Code: {
        //   "Strengths & Weaknesses": [
        //     "generate code for a simple timer",
        //     "generate JS code for calculating zodiac signs from longitudes",
        //   ],
        // },
        KarmaAndDestiny: {
            "Life Lessons": [
                "What karmic tasks do I have to solve in the current incarnation?",
                "What should I avoid to improve my spiritual discipline?",
                "What is my destiny?",
            ],
            "Spiritual Growth": [
                "Please advise me on how I can get more motivation this week. I feel like I don’t have enough at the moment.",
                "Which meditation method works best for my nature?",
            ],
            "Dream Interpretation": [
                "I had a snake in my dream this night. What does it mean?",
            ],
        },
        Money: {
            "Future Financial Outlook": [
                "Is there anything important I should know about money in my future?",
                "Will I be a millionaire or a billionaire one day?",
                "Will I get a sudden fortune or lottery luck in the year 2024 to 2027?",
                "Will I be financially independent of my family in the year 2024?",
                "What is the best period between the year 2024 to 2027 for financial gains in my life as per my birth chart?",
            ],
            "Investments and Savings": [
                "I saved up some money. Should I keep them in a bank or invest?",
                "When will be a good time in the year 2024 to invest my money?",
                "I'm going to take out a loan. When is the best time to apply for it in the year 2024?",
            ],
            "Charitable Acts and Inheritance": [
                "Will My life be better If I start giving money to charity?",
                "Will I receive an inheritance?",
            ],
        },
        HomeAndFamily: {
            "Home Decor": [
                "How to decorate my house to feel more happy and balanced?",
            ],
            "Relocation and Remodeling": [
                "When is the good time to relocate in the year 2024?",
                "I plan to rent an apartment. What is the best time to move in the year 2024?",
                "I plan to have some remodeling at home. What time is the best to start in the year 2024?",
            ],
        },
        BestDates: {
            "Travel and Relocation": [
                "When is the best time to go traveling between 2024 and 2027?",
                "When is the right time for relocation between the year 2024 and 2027?",
            ],
            "Family Planning": [
                "When is the best time for me to have kids between the year 2024 and 2027?",
            ],
            "Career Decisions": [
                "When is the best time to start searching for a new job between the year 2024 and 2027?",
                "When is the right time to start building a house between the year 2024 and 2027?",
            ],
            Purchases: [
                "When is the best time to buy a new car between the year 2024 and 2027?",
                "Is it a good time now according to planetary alignments to make major purchases between the year 2024 and 2027?",
                "When is the best time to sell my house between the year 2024 and 2027?",
            ],
            "Personal Decisions": [
                "Will it be good for me to get a tattoo? When is the best time for it between the year 2024 and 2027?",
            ],
        },
    };

    //chat box body as html to be injected
    HtmlContent = `    
     <!-- MAIN MESSAGE BODY -->
     <!-- CHAT SESSION PERSON SELECTOR -->
         <div class="input-group mb-2">
             <span class="input-group-text gap-2">
                 <span class="iconify" data-icon="icon-park:topic" data-width="25" data-height="25"></span>Chat Topic
             </span>
             <!-- TOPIC SELECTOR -->
             <select class="form-select" id="TopicListDropdown">
                 <option selected value="">What do you want to talk about?</option>
                 <optgroup label="Learn Astrology">
                   <option value="Jaimini Upadesa Sutras - Translated by KRS Jayalakshmi">Jaimini Upadesa Sutras (Translated by KRS Jayalakshmi)</option>
                   <option value="Prasna Marga - Translated by Dr. Raman Srinivasan">Prasna Marga (Translated by Dr. Raman Srinivasan)</option>
                   <option value="Saravali - Translated by P.V.N. Rao">Saravali (Translated by P.V.N. Rao)</option>
                   <option value="Uttara Kalamritha - Translated by Varaha Mihira">Uttara Kalamritha (Translated by Varaha Mihira)</option>
                   <option value="Brihat Jataka - Translated by N. Chidambaram Iyer">Brihat Jataka (Translated by N. Chidambaram Iyer)</option>
                   <option value="Phala Deepika - Translated by V. Subramanya Sastri">Phala Deepika (Translated by V. Subramanya Sastri)</option>
                   <option value="Hora Sara - Translated by B.V. Raman">Hora Sara (Translated by B.V. Raman)</option>
                   <option value="Muhurta Chintamani - Translated by Ganesh Oka">Muhurta Chintamani (Translated by Ganesh Oka)</option>
                   <option value="Garga Hora - Translated by Sanjay Rath">Garga Hora (Translated by Sanjay Rath)</option>
                 </optgroup>
                 <optgroup label="Generate Code">
                   <option value="VedAstro">Vedic astrology code in any language</option>
                 </optgroup>
             </select>
             <!-- ADD PERSON BUTTON -->
             <button type="button" onclick="window.vedastro.chatapi.onclick_add_person()" class="btn btn-outline-secondary"><span class="iconify" data-icon="gg:add" data-width="25" data-height="25"></span></button>
             <!-- RESET CHAT BUTTON -->
             <button type="button" class="btn btn-outline-danger"><span class="iconify" data-icon="carbon:restart" onclick="window.vedastro.chatapi.restart_baby()" data-width="25" data-height="25"></span></button>
             <!-- ADVANCED SETTINGS BUTTON -->
             <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-auto-close="outside" data-bs-toggle="dropdown" aria-expanded="false">
               <span class="iconify" data-icon="iconamoon:settings-fill" data-width="25" data-height="25"></span>
             </button>
             <ul class="dropdown-menu dropdown-menu-end">
               <li class="dropdown-item">
                 <div class="form-check form-switch">
                   <input class="form-check-input" type="checkbox" role="switch" onchange="localStorage.setItem('IsLocalServerMode', this.checked)" id="useLocalServerSwitchInput">
                   <label class="form-check-label" for="useLocalServerSwitchInput">Local Server Mode</label>
                 </div>
               </li>
               <li class="dropdown-item">
                 <div class="input-group">
                   <span class="input-group-text" id="serverAddressLabel">Server</span>
                   <input  id="serverAddressInputElement" type="text" class="form-control" placeholder="Username" aria-label="Username" aria-describedby="serverAddressLabel">
                 </div>
               </li>
               <li class="dropdown-item">
                  <div class="form-check form-switch">
                      <input class="form-check-input" type="checkbox" role="switch" onchange="localStorage.setItem('IsTeacherMode', this.checked)" id="useTeacherModeSwitchInput">
                      <label class="form-check-label" for="useTeacherModeSwitchInput">Teacher Mode</label>
                  </div>
               </li>
               <li class="dropdown-item">
                  <div class="input-group">
                    <span class="input-group-text" id="teacherKeyLabel">Teacher Key</span>
                    <input id="teacherKeyInputElement" type="text" class="form-control" placeholder="Teacher Key" aria-label="Teacher Key" aria-describedby="teacherKeyLabel">
                  </div>
               </li>
               <li><a class="dropdown-item" href="#">Warning under</a></li>
               <li><a class="dropdown-item" href="#">Development</a></li>
               <li><hr class="dropdown-divider"></li>
               <li><a class="dropdown-item" href="#">Advanced users only</a></li>
             </ul>
     
         </div>
     
         <!-- PLACEHOLDER BEFORE CHAT START -->
         <div id="placeholderElement" style="margin: 50px auto;">
           <div class="d-flex justify-content-center">
             <!-- ARROW ICON FOR EYE TO FOLLOW (DRUNK UX) -->
             <svg xmlns="http://www.w3.org/2000/svg" width="73" height="73" viewBox="0 0 24 24">
               <g fill="none">
                 <path d="M24 0v24H0V0zM12.593 23.258l-.011.002l-.071.035l-.02.004l-.014-.004l-.071-.035c-.01-.004-.019-.001-.024.005l-.004.01l-.017.428l.005.02l.01.013l.104.074l.015.004l.012-.004l.104-.074l.012-.016l.004-.017l-.017-.427c-.002-.01-.009-.017-.017-.018m.265-.113l-.013.002l-.185.093l-.01.01l-.003.011l.018.43l.005.012l.008.007l.201.093c.012.004.023 0 .029-.008l.004-.014l-.034-.614c-.003-.012-.01-.02-.02-.022m-.715.002a.023.023 0 0 0-.027.006l-.006.014l-.034.614c0 .012.007.02.017.024l.015-.002l.201-.093l.01-.008l.004-.011l.017-.43l-.003-.012l-.01-.01z" />
                 <path fill="#00b3ff" d="M13.06 3.283a1.5 1.5 0 0 0-2.12 0L5.281 8.939a1.5 1.5 0 0 0 2.122 2.122L10.5 7.965V19.5a1.5 1.5 0 0 0 3 0V7.965l3.096 3.096a1.5 1.5 0 1 0 2.122-2.122z" />
               </g>
             </svg>
           </div>
           <div class="d-flex justify-content-center">
             <span class="" style="color: rgb(143, 143, 143); font-size: 14px;">
               Select <strong>topic</strong> above to <strong>start</strong>
             </span>
           </div>
         </div>
         
         <!-- MESSAGES IN VIEW -->
         <ul class="list-unstyled px-3" id="ChatWindowMessageList" style="max-height:667.5px; overflow: auto; ">
             <li class="d-flex justify-content-start mb-4" id="AIChatLoadingWaitElement" style="display: none !important;">
                 <img src="https://vedastro.org/images/vignes-chat-avatar.webp" alt="avatar"
                      class="rounded-circle d-flex align-self-start me-3 shadow-1-strong" width="45">
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
         <div id="questionInputHolder" class="input-group mb-3" style="">
     
           <button id="presetQuestionsButton" 
             data-bs-auto-close="outside" 
             class="btn btn-outline-secondary dropdown-toggle"
             type="button" data-bs-toggle="dropdown" 
             aria-expanded="false">
               <span class="iconify" 
                 data-icon="icon-park:message" 
                 data-width="23" 
                 data-height="23"></span>
           </button>
           <ul id="presetQuestionDropdown" class="dropdown-menu" aria-labelledby="presetQuestionsButton">
             <!-- DYNAMIC CODE GENERATED HERE -->
           </ul>
     
           <input id="UserChatInputElement" type="text" class="form-control" placeholder="Ask anything about astrology...." aria-label="Ask anything about astrology....">
           <button id="SendChatButton" type="button" class="btn btn-success btn-rounded float-end"><span class="iconify me-1" data-icon="majesticons:send" data-width="25" data-height="25"></span>Send</button>
         </div>
     
     `;

    constructor(rawSettings) {
        console.log(
            "~~~~~~~Stand back! Awesome Chat API code launching! All engines go!~~~~~~~"
        );

        //make instance accessible
        window.vedastro.chatapi = this;

        //correct if property names is camel case (for Blazor)
        var settings = CommonTools.ConvertCamelCaseKeysToPascalCase(rawSettings);

        //expand data inside settings input
        this.ElementID = settings.ElementID;
        this.ShowHeader = settings.ShowHeader;
        this.HeaderIcon = settings.HeaderIcon;

        this.connected = false;
        this.queue = [];

        this.intervalId = null;

        //CHAT GUI INJECTION
        //clear old data if any
        $(`#${this.ElementID}`).empty();

        //random ID for edit button
        this.EditButtonId = Math.floor(Math.random() * 1000000);

        //inject into page
        $(`#${this.ElementID}`).html(this.HtmlContent);

        GenerateTopicListDropdown();

        //generate preset question drop down
        ChatInstance.generateHtmlFromJson(
            this.PresetQuestions,
            "presetQuestionDropdown"
        );

        //GUI LOAD SAVED VALUES
        //load settings stored browser storage
        let isLocalServerModeStr = localStorage.getItem("IsLocalServerMode");
        $("#useLocalServerSwitchInput").prop(
            "checked",
            JSON.parse(isLocalServerModeStr)
        );

        let isTeacherModeStr = localStorage.getItem("IsTeacherMode");
        $("#useTeacherModeSwitchInput").prop(
            "checked",
            JSON.parse(isTeacherModeStr)
        );

        // GUI EVENT HANDLRES
        // NOTE: do handle only

        //1:handle user press "Enter" equal to clicking send button
        $("#UserChatInputElement").keypress((e) => {
            if (e.which === 13) {
                // Enter key pressed
                this.OnClickSendChat();
                e.preventDefault(); // Prevents the default action
            }
        });

        //2:handle send button click
        $("#SendChatButton").on("click", () => {
            this.OnClickSendChat();
        });

        //3: ON change topic dropdown
        //attach topic selector dropdown
        $("#TopicListDropdown").on("change", (e) => {
            //get all needed data (what topic was selected)
            const selectedOption = $("#TopicListDropdown option:selected");
            const selectedOptgroupLabel = selectedOption
                .closest("optgroup")
                .prop("label");
            const selectedTopicId = selectedOption.val();

            //handle possible different selection types
            //nothing much done here, basically let caller context has been swicthed
            var validTopicSelected = false;

            //1 : HOROSCOPE
            //if value is for horoscope
            if (
                selectedOptgroupLabel === "Horoscopes" ||
                selectedOptgroupLabel === "Example Horoscopes"
            ) {
                //get full details of the person
                let selectedPerson = window.vedastro.PersonList.find(
                    (obj) => obj.PersonId === selectedTopicId
                );

                //save for use by other
                window.vedastro.SelectedPerson = selectedPerson;

                //let others know (context changer)
                $(document).trigger("onChangeSelectedTopic", "Hello, World!");

                validTopicSelected = true; //ready to connect to server
            }

            //2 : BOOKS
            if (selectedOptgroupLabel === "Learn Astrology") {
                Swal.fire(
                    "Coming soon!",
                    "<strong>Come</strong> back later or choose another topic",
                    "info"
                );

                //reset to make selection again
                $("#TopicListDropdown").val("");
            }

            //2 : CODE
            if (selectedOptgroupLabel === "Generate Code") {
                Swal.fire(
                    "Coming soon!",
                    "<strong>Come</strong> back later or choose another topic",
                    "info"
                );

                //reset to make selection again
                $("#TopicListDropdown").val("");
            }

            //open connection to server if has not been open
            if (typeof this.socket === "undefined" && validTopicSelected) {
                this.ServerURL = $("#useLocalServerSwitchInput").is(":checked")
                    ? this.LocalServerURL
                    : this.LiveServerURL;

                OpenConnectionToChatBot(this);
            }
        });

        //4:let user know AI chat will use this newly selected person
        $(document).on("onChangeSelectedTopic", function (e) {
            // show message to user that location was found and set
            Swal.fire({
                icon: "success",
                title: "Topic changed!",
                html: `We will now talk about <strong>${window.vedastro.SelectedPerson.Name}</strong>'s horoscope.`,
                showConfirmButton: false,
                timer: 1000,
            });

            // execute once execution que is empty (so pop up stays open) via 0ms
            setTimeout(function () {
                //auto open presets questions drop down for super speed users (UX ⚡)
                $("#presetQuestionsButton").dropdown("toggle");
            }, 0);
        });

        //7: handle local server switch
        //when swith is fliped record into memory for future use (save user's time)

        // Save a string value to local storage
        localStorage.setItem("myKey", "Hello World!");

        //update control center back on earth
        console.log("~~~~~~~Huston, we have lift off!~~~~~~~");

        function OpenConnectionToChatBot(instance) {
            //start connection
            instance.socket = new WebSocket(instance.ServerURL);

            //attach standard handlers for connetion
            ["open", "message", "close", "error"].forEach((eventName) => {
                instance.socket.addEventListener(
                    eventName,
                    instance[`on${eventName}`].bind(instance)
                );
            });
        }

        async function GenerateTopicListDropdown(idToSelect = "") {
            //get the main dropdown element
            var $dropdown = $("#TopicListDropdown");

            //DO FOR USER'S SAVED LIST
            window.vedastro.PersonList = await CommonTools.GetAPIPayload(
                `${window.vedastro.ApiDomain}/GetPersonList/OwnerId/${window.vedastro.UserId}`
            );

            //create a header in the list
            let $horoscopeGroup = $("<optgroup>", {
                label: "Horoscopes",
            });

            $dropdown.append($horoscopeGroup); //add to main list

            //populate slection list at bottom with horoscopes
            $.each(window.vedastro.PersonList, function (i, person) {
                $horoscopeGroup.append(
                    $("<option>", {
                        value: person.PersonId,
                        text: person.Name,
                        selected: person.PersonId === idToSelect,
                    })
                );
            });

            //DO FOR PUBLIC LIST
            window.vedastro.PublicPersonList = await CommonTools.GetAPIPayload(
                `${window.vedastro.ApiDomain}/GetPersonList/OwnerId/101`
            );
            //create a header in the list
            let $publicHoroscopeGroup = $("<optgroup>", {
                label: "Example Horoscopes",
            });
            $dropdown.append($publicHoroscopeGroup); //add to main list

            //populate slection list at bottom with horoscopes
            $.each(window.vedastro.PublicPersonList, function (i, person) {
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

    /*
        █▀▀ █░█ █▀▀ █▄░█ ▀█▀   █░█ ▄▀█ █▄░█ █▀▄ █░░ █▀▀ █▀█ █▀
        ██▄ ▀▄▀ ██▄ █░▀█ ░█░   █▀█ █▀█ █░▀█ █▄▀ █▄▄ ██▄ █▀▄ ▄█
        */

    onopen() {
        console.log("We're connected to live chat baby!");

        //hide chat box place holder
        $("#placeholderElement").hide();

        this.connected = true;
        this.processQueue();
    }

    //called direct from static HTML hookup without seperate attach code
    //exp use : onclick="window.vedastro.chatapi.rate_message(this, -1)"
    restart_baby() {
        //TODO do proper restart of only client connetion now whole dang system!!

        //restart dank system
        location.reload();
    }

    //called direct from static HTML hookup without seperate attach code
    //exp use : onclick="window.vedastro.chatapi.rate_message(this, -1)"
    rate_message(eventData, rating) {
        //come here on click rating button
        // get hash of message, stored as id in holder
        var messageHolder = $(eventData)
            .closest(".card")
            .children(".message-holder");
        var text_hash = messageHolder.attr("id");

        const messagePayload = {
            user_id: window.vedastro.UserId,
            rating: rating,
            text_hash: text_hash,
        };

        window.vedastro.chatapi.enqueueMessage(JSON.stringify(messagePayload));
    }

    onclick_add_person() {

        //NOTE : "add person page" has auto return to previous page on save
        //navigate to person add page
        window.location.href = "./Account/Person/Add";
    }

    onclick_login() {

        //NOTE : "add person page" has auto return to previous page on save
        //navigate to login page
        window.location.href = "./Account/Login";
    }


    //called direct from static HTML hookup without seperate attach code
    //exp use : onclick="window.vedastro.chatapi.rate_message(this, -1)"
    onclick_preset_question(eventData) {
        //6: autofill preset questions handle (attach after generate)
        var selectedText = $(eventData).text();
        $("#UserChatInputElement").val(selectedText);
    }

    //when on of the follow up questions gets clicked
    //get called direct from html code
    ask_followup_question(eventData, followupQuestion) {
        // get hash of message, stored as id in holder
        var messageHolder = $(eventData)
            .closest(".card")
            .children(".message-holder");
        var primaryAnswerHash = messageHolder.attr("id");

        //UPDATE GUI WITH USER MSG (UX)
        var aiInput = $("#UserChatInputElement").val();
        var userName = "You";
        var userInputChatCloud = `
        <li class="d-flex justify-content-end mb-4">
            <div class="card ">
                <div class="card-header d-flex justify-content-between p-3">
                    <p class="fw-bold mb-0">${userName}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">
                        ${followupQuestion}
                    </p>
                </div>
            </div>
            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-6.webp" alt="avatar"
                 class="rounded-circle d-flex align-self-start ms-3 shadow-1-strong" width="45">
        </li>
        `;
        //inject in User's input into chat window
        $("#ChatWindowMessageList li").eq(-1).after(userInputChatCloud);

        //set topic text TODO support DOB and books
        var topicText = CommonTools.BirthTimeUrlOfSelectedPersonJson();

        //note the switch to python naming covention
        var commandsToSend = [];
        commandsToSend.push("followup_question"); //add command for server to read as "follow up"
        const messagePayload = {
            user_id: window.vedastro.UserId,
            primary_answer_hash: primaryAnswerHash,
            command: commandsToSend, //server uses this to do special handling
            text: followupQuestion,
            topic: topicText,
        };

        window.vedastro.chatapi.enqueueMessage(JSON.stringify(messagePayload));
    }

    //YOU CANNOT FIGHT A DYING MAN,
    //HE HOLDS THE UPPER HAND ALWAYS

    // Handler for incoming messages
    onmessage(event) {
        // Parse the JSON data from the event
        var raw_json_message = JSON.parse(event.data);
        var ai_text_message_html = raw_json_message.text_html;
        var ai_text_2_message_html = raw_json_message.text_2;
        var ai_text_3_message_html = raw_json_message.text_3;
        var message_hash = raw_json_message.text_hash;
        var ai_text_message = raw_json_message.text;
        var followup_questions = raw_json_message?.followup_questions ?? [];

        //PROCESS SERVER COMMANDS
        var commands = raw_json_message.command || []; // when no commands given empty to not fail

        //## SPECIAL HANDLE FOR LOGIN PROMPTS
        //1: check if server said please login, in command to client
        //   meaning user just say login message given by server,
        //   upon click login, start wait loop (make it seem bot is waiting for user to login)
        //   then that special login tab (RememberMe) will auto close

        let intervalId;
        if (commands.includes("please_login")) {
            //TODO maybe not needed anymore
            //set marker in browser asking Blazor login page to redirect back
            localStorage.setItem('PreviousPage', '/ChatAPI');
        }

        //## BUILD HTML

        //HANDLE FOLLOWUP
        // only add follow up questions if server specified them
        var followupQuestionsHtml = "";
        // convert questions into visible buttons, for user to click
        if (followup_questions.length > 0) {
            followupQuestionsHtml += //start out hidden, then js will bring to live with animation at right time (class)
                '<div class="followUpQuestionHolder hstack gap-2 w-100 justify-content-end" style="display:none; position: absolute; bottom: -43px; right: -1px; ">';

            followup_questions.forEach(function (question) {
                followupQuestionsHtml += `
            <button type="button" onclick="window.vedastro.chatapi.ask_followup_question(this, '${question}')"  class="btn btn-outline-primary">${question}</button>
        `;
            });

            followupQuestionsHtml += "</div>";
        }

        //only show feedback buttons for text that need feedback
        var feedbackButtonHtml = commands.includes("no_feedback")
            ? ""
            : `<div class="hstack gap-2">
    <button title="Bad answer" type="button" onclick="window.vedastro.chatapi.rate_message(this, -1)" class="btn btn-danger" style="padding: 0px 5px;">
      <span class="iconify" data-icon="icon-park-outline:bad-two" data-width="18" data-height="18"></span>
    </button>
    <button title="Good answer" type="button" onclick="window.vedastro.chatapi.rate_message(this, 1)" class="btn btn-primary" style="padding: 0px 5px;">
      <span class="iconify" data-icon="icon-park-outline:good-two" data-width="18" data-height="18"></span>
    </button>
  </div>`;

        //based on number of answers received use correct GUI
        //so, if only 1 ans then show standard gui, else do special multi ans accordian
        var isMultiAns = ai_text_2_message_html !== undefined;
        var aiFinalAnswerHolder = isMultiAns
            ? `                <!-- Multiple answer holder -->
    <div class="accordion text-html-out-elm" style="display:none;" >
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
            Answer #1
          </button>
        </h2>
        <div id="collapseOne" class="accordion-collapse collapse show" data-bs-parent="#accordionExample">
          <div class="accordion-body">
            ${ai_text_message_html}
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
           Answer #2
          </button>
        </h2>
        <div id="collapseTwo" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
          <div class="accordion-body">
            ${ai_text_2_message_html}
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
            Answer #3
          </button>
        </h2>
        <div id="collapseThree" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
          <div class="accordion-body">
            ${ai_text_3_message_html}
          </div>
        </div>
      </div>
    </div>
`
            : `
<div style="display:none;" class="text-html-out-elm mb-0">
${ai_text_message_html}
</div>
`;

        // Create a chat bubble with the AI's message
        var aiInputChatCloud = `<li class="d-flex justify-content-start" style=" margin-bottom: 70px; ">
        <img src="https://vedastro.org/images/vignes-chat-avatar.webp" alt="avatar" class="rounded-circle d-flex align-self-start me-3 shadow-1-strong" width="45">
        <div class="card">
            <div class="card-header d-flex justify-content-between p-3">
                <p class="fw-bold mb-0 me-5">Vignes</p>
                ${feedbackButtonHtml}
            </div>
            <div id="${message_hash}" class="message-holder card-body">
                ${aiFinalAnswerHolder}
                <p class="temp-text-stream-elm mb-0">
                  <!-- Content will be streamed here -->
                </p>
                <!-- SVG for loading icon -->
                <svg class="loading-icon-elm" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-dasharray="15" stroke-dashoffset="15" stroke-linecap="round" stroke-width="2" d="M12 3C16.9706 3 21 7.02944 21 12"><animate fill="freeze" attributeName="stroke-dashoffset" dur="0.3s" values="15;0" /><animateTransform attributeName="transform" dur="1.5s" repeatCount="indefinite" type="rotate" values="0 12 12;360 12 12" /></path></svg>
                ${followupQuestionsHtml}
            </div>
        </div>
    </li>`;

        // Append the chat bubble to the chat window
        $("#ChatWindowMessageList li").eq(-1).after(aiInputChatCloud);

        // # AUTO SCROLL DOWN
        $("#ChatWindowMessageList").scrollTop(
            $("#ChatWindowMessageList")[0].scrollHeight
        );

        // Flag to prevent user input while AI is 'typing'
        this.IsAITalking = true;

        // Initialize the index for streaming text
        let index = 0;
        const streamRateMs = 23; // Rate at which characters are displayed

        // Stream the AI's message into the chat bubble
        const interval = setInterval(() => {
            // Check if the entire message has been displayed
            //MESSAGE STREAM COMPLETE
            if (index >= ai_text_message.length) {
                clearInterval(interval);

                // Hide the temporary element and loading icon, then show the formatted message
                //remove stream shower and loading for this bubble since not needed anymore
                $(`#${message_hash} .temp-text-stream-elm`).hide();
                $(`#${message_hash} .loading-icon-elm`).hide();

                //make visible hidden formatted output
                $(`#${message_hash} .text-html-out-elm`).show();

                // Allow user input again
                this.IsAITalking = false;

                //make follow up questions if any slowly appear
                //narrow by message bubble, then holder
                $(`#${message_hash} .followUpQuestionHolder`).fadeIn("slow");

                return;
            }

            // Append the next character or handle special formatting
            ChatInstance.appendNextCharacter(
                ai_text_message,
                index,
                `#${message_hash} .temp-text-stream-elm`
            );
            index++;
        }, streamRateMs);
    }

    // Function to append the next character or handle special formatting
    static appendNextCharacter(text, index, elementSelector) {
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

    //TODO Needs rename
    static generateHtmlFromJson(json, targetElementId) {
        const presetQuestionsJson = json;
        const targetElement = document.getElementById(targetElementId);
        let htmlContent = "";

        //NOTE: below sample of full render
        // <li data-bs-toggle="collapse" href="#collapseExample" role="button" aria-expanded="false" aria-controls="collapseExample">
        //    <h6 class="dropdown-header">
        //        <span class="iconify me-2" data-icon="fluent-emoji-flat:heart-with-arrow" data-width="23" data-height="23"></span>Love</h6></li>
        // <div class="collapse" id="collapseExample">
        //   <div class="card card-body">
        //     <li><a class="dropdown-item">When will I meet the love of my life in the year 2024?</a></li>
        //     <li><a class="dropdown-item">Am I going to be in a new relationship in the year 2024?</a></li>
        //     <li><a class="dropdown-item">Tell me about my love life.</a></li>
        //     <li><a class="dropdown-item">Will I meet my soulmate in the year between 2024 to 2027?</a></li>
        //   </div>
        // </div>

        Object.entries(presetQuestionsJson).forEach(
            ([categoryKey, categoryValueList]) => {
                htmlContent += `<li 
        data-bs-toggle="collapse" 
        href="#collapse_${categoryKey}"
        role="button" aria-expanded="false"
        aria-controls="collapse_${categoryKey}">
          <h6 class="dropdown-header">
            <span class="iconify me-2" data-icon="${ChatInstance.getCategoryIconClass(
                    categoryKey
                )}" data-width="23" data-height="23"></span>
            ${categoryKey}
          </h6></li>`;
                htmlContent += `<div class="collapse" id="collapse_${categoryKey}">`;
                htmlContent += '<div class="card card-body">';

                let combinedArray = [];
                for (let category in categoryValueList) {
                    combinedArray = combinedArray.concat(categoryValueList[category]);
                }

                if (Array.isArray(combinedArray)) {
                    combinedArray.forEach((questionText) => {
                        htmlContent += `<li><a  onclick="window.vedastro.chatapi.onclick_preset_question(this)" class="dropdown-item">${questionText}</a></li>`;
                    });
                } else {
                    htmlContent += `<li><a class="dropdown-item">${combinedArray}</a></li>`;
                }

                htmlContent += "</div>"; // Close .card-body div
                htmlContent += "</div>"; // Close #collapse_{categoryKey} div
            }
        );

        targetElement.innerHTML = htmlContent;
    }
    static getCategoryIconClass(categoryKey) {
        switch (categoryKey) {
            case "BestDates":
                return "fluent-emoji-flat:spiral-calendar";
            case "Personality":
                return "fluent-emoji-flat:person";
            case "Travel":
                return "fluent-emoji-flat:airplane-departure";
            case "Love":
                return "fluent-emoji-flat:heart-with-arrow";
            case "Astrology":
                return "twemoji:ringed-planet";
            case "Studies":
                return "fluent-emoji-flat:books";
            case "Money":
                return "fluent-emoji-flat:money-bag";
            case "Business":
                return "fluent-emoji-flat:briefcase";
            case "Career":
                return "fluent-emoji-flat:graduation-cap";
            case "HomeAndFamily":
                return "fluent-emoji-flat:house-with-garden";
            case "KarmaAndDestiny":
                return "noto:milky-way";
            case "TestAccuracy":
                return "fluent-emoji-flat:test-tube";
            case "AIJokes":
                return "fxemoji:smiletongue";
            case "Previous":
                return "mdi:comment-previous";
            default:
                return "fluent-emoji-flat:heart-with-arrow";
        }
    }

    onclose() {
        console.log("Connection closed");
        this.connected = false;
    }

    onerror(event) {
        console.error("WebSocket error: ", event);
        this.connected = false;
    }

    processQueue() {
        while (this.queue.length > 0) {
            this.sendNextMessageInQueue();
        }
    }

    sendNextMessageInQueue() {
        if (this.connected && this.socket.readyState === WebSocket.OPEN) {
            const message = this.queue.shift();
            this.socket.send(message);
        }
    }

    enqueueMessage(message) {
        if (!this.connected || this.socket.readyState !== WebSocket.OPEN) {
            this.queue.push(message);
        } else {
            this.sendMessageNow(message);
        }
    }

    sendMessageNow(message) {
        if (
            this.socket.readyState === WebSocket.OPEN ||
            this.socket.readyState === WebSocket.CONNECTING
        ) {
            this.socket.send(message);
        } else {
            console.error("Cannot send message, connection is not open");
        }
    }

    async waitForConnection() {
        return new Promise((resolve) => {
            if (this.connected) {
                resolve();
                return;
            }

            this.checkConnection = setInterval(() => {
                if (this.connected) {
                    clearInterval(this.checkConnection);
                    resolve();
                }
            }, 100);
        });
    }

    //control comes here from both Button click and keyboard press enter
    async OnClickSendChat(userInput = "") {
        //STEP 0 : Validation
        //make sure the topic has been selected, else end here
        var selectedTopic = $("#TopicListDropdown").val();
        if (selectedTopic === "") {
            Swal.fire(
                "How to talk about nothing, dear?",
                "<strong>Choose</strong> first a <strong>topic</strong> that you want to talk about today",
                "error"
            );
            return;
        }

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
        if (this.IsAITalking) {
            Swal.fire(
                "Please wait, dear..",
                "AI is <strong>busy talking</strong>, please wait for it to <strong>finish</strong> chattering.",
                "error"
            );
            return;
        }

        // STEP 1 : UPDATE GUI WITH USER MSG (UX)
        var aiInput = $("#UserChatInputElement").val();
        var userName = "You";
        var userInputChatCloud = `
        <li class="d-flex justify-content-end mb-4">
            <div class="card ">
                <div class="card-header d-flex justify-content-between p-3">
                    <p class="fw-bold mb-0">${userName}</p>
                </div>
                <div class="card-body">
                    <p class="mb-0">
                        ${userInput}
                    </p>
                </div>
            </div>
            <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-6.webp" alt="avatar"
                 class="rounded-circle d-flex align-self-start ms-3 shadow-1-strong" width="45">
        </li>
        `;
        //inject in User's input into chat window
        $("#ChatWindowMessageList li").eq(-1).after(userInputChatCloud);

        //STEP 2:
        //user's input is sent to server for reply
        //get selected birth time
        //TODO can be DOB or bookname
        var topicText = CommonTools.BirthTimeUrlOfSelectedPersonJson();
        await this.SendMessageToServer(userInput, topicText);
        this.LastUserMessage = userInput; //save to used later for highlight

        //STEP 3 : GUI CLEAN UP
        //clear question input box for next, question
        $("#UserChatInputElement").val("");

        //STEP 4: ADD EASY TO EASY REASK LIST
        // Add the new question at the beginning of the 'Last 3' array
        // Check if the new question already exists in the 'Last 3' array
        var index = this.PresetQuestions.Previous["Last 3"].indexOf(userInput);

        if (index === -1) {
            // If the new question does not exist, add it at the beginning of the array
            this.PresetQuestions.Previous["Last 3"].unshift(userInput);
        } else {
            // If the new question exists, remove it from its current position
            this.PresetQuestions.Previous["Last 3"].splice(index, 1);
            // Then add it at the beginning of the array
            this.PresetQuestions.Previous["Last 3"].unshift(userInput);
        }

        // If there are more than 3 questions, remove the 4th one
        if (this.PresetQuestions.Previous["Last 3"].length > 5) {
            this.PresetQuestions.Previous["Last 3"].pop();
        }
        //re-generate preset question drop down
        ChatInstance.generateHtmlFromJson(
            this.PresetQuestions,
            "presetQuestionDropdown"
        );
    }

    //this is where final JSON packaged before sending to server
    async SendMessageToServer(message, topicText) {
        //build all commands based on set user settings
        var command_list = [];
        var password = $("#useTeacherModeSwitchInput").val();
        var isTeacherMode = $("#useTeacherModeSwitchInput").is(":checked");
        //teacher mode enables all answers, but slower
        if (isTeacherMode) {
            command_list.push("teacher_mode");
        }

        //NOTE: also equivelant to answer processing expriments
        //set answer level
        command_list.push("level_1");

        const messagePayload = {
            user_id: window.vedastro.UserId, //gotten from browser storage if any when script was init
            text: message,
            topic: topicText,
            command: command_list,
            password: password,
        };

        window.vedastro.chatapi.enqueueMessage(JSON.stringify(messagePayload));
    }
}

/**
 * Shortcut method to initialize Chat instace in 1 static call.
 * Used by Blazor to call JS code.
 * @param {Object} settings - The settings for the AstroTable.
 * @param {Object} inputArguments - The Time and other data needed to generate table.
 */
window.GenerateChatInstance = (settingsAIChat) => {
    //note: on init, chat instance is loaded into window.vedastro.chatapi
    new ChatInstance(settingsAIChat);
    window.vedastro.chatapi.waitForConnection();
};


/**
 * Shortcut method to aimate events chart.
 * Used by Blazor to call JS code.
 */
window.ChartFromSVG = async (chartStr) => {

    //inject into default div on page to hold, "EventsChartSvgHolder"
    var $chartElm = injectIntoElement($(ID.EventsChartSvgHolder)[0], chartStr);

    //things done here:
    //- get the unique ID of the chart
    //- use ID to maintain clean code
    //- chart is available in window.EventsChartList
    var chartId = $chartElm.attr('id');
    var index = new EventsChart(chartId); //brings to life

    //let caller know all went well
    console.log(`Amen! Chart Loaded : INDEX:${index}, ID:${chartId}`);



    //-----------------------------LOCAL FUNCS---------------------------------------

    //todo marked for update
    async function getEventsChartFromApiXml(url, payload) {
        console.log(`JS : Getting events chart from API...`);

        var response = await window.fetch(url, {
            "headers": { "accept": "*/*", "Connection": "keep-alive" },
            "body": payload,
            "method": "GET"
        });

        //API should always give a OK reply, else it has failed internally
        if (!response.ok) { console.log("ERROR : API Call Crashed!"); }

        //inject new svg chart into page
        var svgChartString = await response.text();

        return svgChartString;
    }


    //returns the reference to the SVG element in DOM
    function injectIntoElement(parentElement, valueToInject) {
        console.log(`JS : Injecting SVG Chart into page...`);

        //if parent not found raise alarm
        if (parentElement === undefined) { console.log("ERROR: Parent element ID'd EventsChartSvgHolder not found"); }

        //convert string to html node
        var template = document.createElement("template");
        template.innerHTML = valueToInject;
        var svgElement = template.content.firstElementChild;

        //place new node in parent
        parentElement.innerHTML = ''; //clear current children if any
        parentElement.appendChild(svgElement);

        //return reference in to SVG elm in DOM (jquery for ease)
        return $(svgElement);
    }


}