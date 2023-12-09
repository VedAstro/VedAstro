//DEFAULT COLUMNS FOR PLANET TABLE
PlanetColumns = [
    { API: "PlanetZodiacSign", Enabled: true, Name: "Sign" },
    { API: "PlanetConstellation", Enabled: true, Name: "Star" },
    { API: "HousePlanetOccupiesKP", Enabled: false, Name: "Occupies" },
    { API: "HousesOwnedByPlanetKP", Enabled: false, Name: "Owns" },
    { API: "PlanetLordOfZodiacSign", Enabled: true, Name: "Sign Lord" },
    { API: "PlanetLordOfConstellation", Enabled: true, Name: "Star Lord" },
    { API: "PlanetSubLordKP", Enabled: false, Name: "Sub Lord" },
    { API: "Empty", Enabled: false, Name: "Empty" },
    { API: "Empty", Enabled: false, Name: "Empty" },
];

class AstroTable {

    Ayanamsa = "KRISHNAMURTI"
    //# LOCAL <--> LIVE Switch
    APIDomain = "https://vedastroapi.azurewebsites.net/api";
    //APIDomain = "http://localhost:7071/api";

    // Class fields
    TableId = ""; //ID of table set in HTML, injected during init
    KeyColumn = ""; //Planet or House
    ColumnData = []; //data on selected columns
    EnableSorting = false; //sorting disabled by default
    APICalls = []; //list of API calls that can be used in table (filled on load)

    constructor(tableId, keyColumn, columnData, enableSorting = false) {

        this.TableId = tableId;

        //based on table ID try get any settings if saved from before
        var savedTableSettings = false; //TODO TEMP HARD SET DEFAULTS ON REFRESH --> localStorage.getItem(this.TableId);

        if (savedTableSettings) {
            //parse the data
            let jsonObject = JSON.parse(savedTableSettings);

            //set back all the exact settings from before
            this.KeyColumn = jsonObject["KeyColumn"];
            this.ColumnData = jsonObject["ColumnData"];
            this.EnableSorting = jsonObject["EnableSorting"];
        }
        //if null use data pumped in via constructor (defaults, when click Reset)
        else {
            this.KeyColumn = keyColumn;
            this.ColumnData = columnData;
            this.EnableSorting = enableSorting;
        }
    }

    async ShowEditTableOptions() {

        //pump in data about table settings to show in popup
        var htmlPopup = await AstroTable.GenerateTableEditorHtml(this.ColumnData, this.KeyColumn, this.APIDomain);

        //used to "Hoist" table reference for later event handlers firing
        var instance = this;

        var swalSettings = {
            width: 'auto',
            title: 'Edit Table',
            html: htmlPopup,
            focusConfirm: false,

            //after User clicks OK
            //get value from dialog box & save it for later use
            preConfirm: () => {
                //parses data from popup and saved it for later
                AstroTable.UpdateDateColumns(this.ColumnData);

                //update enable sorting switch
                this.EnableSorting = $("#TableSortingEnableSwitch").is(':checked');

                //get value from Key Column selector & save it
                this.KeyColumn = $("#KeyColumnInput").val();

                //clone all setting to Local Storage for future use under TableID which should be unique
                localStorage.setItem(this.TableId, this.ToJsonString());
            },
            //load saved values into view before showing to user
            //note: not all after load is done here, some data is fed into HTML maker
            didOpen: (popupElm) => {

                //SORT SWITCH
                //set switch based on what was set before
                $("#TableSortingEnableSwitch").prop('checked', instance.EnableSorting);

                //KEY COLUMN
                //attach one 1 time event reload popup if key column was changed
                //because API calls are different for different key columns
                $("#KeyColumnInput").one("change",
                    async (eventObj) => {

                        instance.KeyColumn = $(eventObj.target).val(); //save value

                        //tell user API calls need to be updated
                        await Swal.fire('Update API Calls', `You've changed the Key Column to <strong>${instance.KeyColumn}</strong>, update the API calls to match.`, 'info');

                        instance.ShowEditTableOptions(); //reload panel
                    });

                //RESET BUTTON
                //attach one 1 time event reload popup if Reset button clicked
                $("#EditTableResetButton").one("click",
                    async (eventObj) => {

                        //clear saved browser settings, this will make defaults to load in constructor
                        localStorage.setItem(instance.TableId, "");

                        //tell user API calls need to be updated
                        await Swal.fire('Reset done!', 'Please standby for auto page <strong>Refresh</strong>', 'success');

                        //reload page
                        location.reload();
                    });

            }
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
            theme: 'bootstrap',

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
                if (this.getValue().length == 0 && this.getValue() != this.lastValidValue) {
                    this.setValue(this.lastValidValue);
                }
            },
        };

        //initialize Doped select options, with search for each dropdown
        for (var columnNumber = 0; columnNumber < this.ColumnData.length; columnNumber++) {
            $(`#SelecteAPI${columnNumber}Dropdown`).selectize(selectizeConfigSingle);
        }
    }

    //given the full column array, extract out only the filtered endpoint
    GetAllEnabledEndpoints() {

        // Filter the ColumnData array to get only the columns where Enabled is true
        let enabledColumns = this.ColumnData.filter(column => column.Enabled);

        // Map the enabledColumns to their respective API and return the result
        let apis = enabledColumns.map(column => column.API);

        return apis;
    }

    GetNiceColumnNameFromRawAPIName(rawApiName) {
        for (let i = 0; i < this.ColumnData.length; i++) {
            if (this.ColumnData[i].API === rawApiName) {
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
            this.APICalls = await AstroTable.GetAPIPayload(`${this.APIDomain}/ListCalls`);
        }

        var foundCalls = AstroTable.FindAPICallByName(this.APICalls, apiName);

        var selectedMethodInfo = foundCalls[0]?.MethodInfo;

        return selectedMethodInfo;
    }

    async GenerateHTMLTableFromAPI(timeUrlParam, horaryNumber, rotateDegrees) {

        //extract endpoints that have been enabled
        var endpoints = this.GetAllEnabledEndpoints();

        // load the needed data from API for each column based
        var keyColumnParam = `${this.KeyColumn}Name/All/`;

        //compile all user inputed params
        //NOTE: name of property must match API C# code
        var userInputParams = {
            "time": timeUrlParam,
            [this.KeyColumn]: keyColumnParam,
        };

        //only add horary if user inputed (defaults to 0)
        var horaryParam = `Number/${horaryNumber}/`;
        if (horaryNumber !== 0) { userInputParams["horary"] = horaryParam; }

        //only add rotate degrees if user inputed (defaults to 0)
        var rotateParam = `RotateDegrees/${rotateDegrees}/`;
        if (rotateDegrees !== 0) { userInputParams["rotate"] = rotateParam; }


        //each API calculator listed is called (parallel)
        var payloads = await Promise.all(endpoints.map(
            async (endpoint) => {
                var apiPayload = await AstroTable.GetPayLoad2(endpoint, userInputParams, this);
                return apiPayload;
            }));

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
            var th = document.createElement('th');
            th.textContent = cleanColumnName;
            headerRow.appendChild(th);
        }
        // Create the table body
        var tbody = document.createElement('tbody');
        table.appendChild(tbody);

        // Create the body rows
        for (var key in data) {
            var row = tbody.insertRow();
            var cell = document.createElement('td');
            cell.textContent = key;
            row.appendChild(cell);

            //each item here is the data that goes into cell
            for (var item of data[key]) {

                cell = document.createElement('td');

                //if the value inside column is complex type (not string)
                //exp : Zodiac Sign/Planet Name in JSON format
                if (typeof item === 'object' && item !== null) {

                    //SPECIAL handle to remove unwanted properties from JSON for special types
                    AstroTable.RemoveProperty(item, "TotalDegrees"); //Zodiac Sign

                    //place each value inside object into 1 string
                    cell.textContent = AstroTable.FlattenObjectValues(item).join(' ');
                }
                else {
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
            "TableId": this.TableId,
            "KeyColumn": this.KeyColumn,
            "ColumnData": this.ColumnData,
            "EnableSorting": this.EnableSorting
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

        if (selectedMethodInfo === undefined) {
            alert(`API call ${endpoint} not found!`);
        }

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
                if (typeof obj[prop] === 'object') {
                    if (obj[prop].hasOwnProperty(propToRemove)) {
                        delete obj[prop][propToRemove];
                    } else {
                        for (let subProp in obj[prop]) {
                            if (typeof obj[prop][subProp] === 'object' && obj[prop][subProp].hasOwnProperty(propToRemove)) {
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
            if (typeof obj[prop] === 'object' && obj[prop] !== null) {
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
        return items.filter(item => item.MethodInfo.Name === apiCalcName);

    }

    static CombineRawAPICallResults(inputArray) {
        return inputArray.reduce((acc, curr) => {
            curr?.forEach(obj => {
                const key = Object.keys(obj)[0];
                if (!acc[key]) {
                    acc[key] = [];
                }
                acc[key].push(obj[key]);
            });
            return acc;
        }, {});
    }

    // generate Table Editor column options popup panel
    static async GenerateTableEditorHtml(columnData, keyColumnName, apiDomain) {

        var formHtml = '';

        for (var columnNumber = 0; columnNumber < columnData.length; columnNumber++) {
            formHtml += `
                    <div class="input-group input-group-sm mb-3">
                        <div class="input-group-text">
                            <input class="form-check-input mt-0"  id="Enabled${columnNumber}" type="checkbox" value="" aria-label="Enable Column" ${columnData[columnNumber].Enabled ? 'checked' : ''}>
                        </div>
                        <input type="text" id="Name${columnNumber}" value="${columnData[columnNumber].Name}" class="form-control" aria-label="Text input with checkbox">
                        <span class="input-group-text">
                            <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 128 128"><path fill="#40c0e7" d="M108.58 64L62.47 97.81V76.72H19.42V51.49h43.04v-21.3L108.58 64z"/></svg>
                        </span>
                        <div class="w-50">
                            <select id="SelecteAPI${columnNumber}Dropdown"  class="mt-1">
                                <option value=""></option>
                                ${await AstroTable.GetAPICallsListSelectOptionHTML(columnData[columnNumber].API, keyColumnName, apiDomain)}
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
        $defaultKeyColumnSel.find('option[value="' + keyColumnName + '"]').attr('selected', 'selected');

        // Convert the jQuery object back to HTML string
        //saved as string to be injected later
        var keyColumnSelector = $defaultKeyColumnSel.prop('outerHTML');

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
    static async GetAPICallsListSelectOptionHTML(selectValue, keyColumnName, apiDomain) {

        //get raw API calls list from Server
        var apiCalls = await AstroTable.GetAPIPayload(`${apiDomain}/ListCalls`);

        //filter out call that can NOT be used in columns (make User's live easier)
        apiCalls = AstroTable.FilterOutIncompatibleAPICalls(apiCalls, keyColumnName);

        let options = "";
        $.each(apiCalls, function (i, item) {
            //if called specified selected value, than select it
            var isSelected = selectValue === item.MethodInfo.Name;
            options += `<option value='${item.MethodInfo.Name}' title='${item.Description}' ${isSelected ? "selected" : ""}>${item.MethodInfo.Name}</option>`;
        });

        return options;

    }

    //gets only API calls that can be used in Table, removes rest 
    static FilterOutIncompatibleAPICalls(items, keyColumnName) {
        return items.filter(item => {
            const parameters = item.MethodInfo.Parameters;
            return parameters.length >= 2 &&
                //NOTE: here hack to link Key Column to API library
                //make sure parameters to call API is supported
                parameters[0].ParameterType === `VedAstro.Library.${keyColumnName}Name` &&
                parameters[1].ParameterType === "VedAstro.Library.Time";
        });
    }

    // Function to update the array based on the Swal form
    static async UpdateDateColumns(dataColumns) {
        for (var i = 0; i < dataColumns.length; i++) {
            dataColumns[i].API = $(`#SelecteAPI${i}Dropdown`).val();
            dataColumns[i].Enabled = $('#Enabled' + i).is(':checked');
            dataColumns[i].Name = $('#Name' + i).val();
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
        if (!foundParam) { foundParam = ""; }
        return foundParam;
    }

    static async GetAPIPayload(url, payload = null) {
        try {
            // If a payload is provided, prepare options for a POST request
            const options = payload
                ? {
                    method: 'POST', // Specify the HTTP method as POST
                    headers: { 'Content-Type': 'application/json' }, // Set the content type of the request to JSON
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
                icon: 'error',
                title: 'App Crash!',
                text: error,
                confirmButtonText: 'OK'
            });
        }
    }

    static ClearTableRows(tableId) {
        let table = document.getElementById(tableId);
        while (table.rows.length > 0) {
            table.deleteRow(0);
        }
    }

}
