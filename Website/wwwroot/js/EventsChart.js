/*

███████╗██╗░░░██╗███████╗███╗░░██╗████████╗░██████╗    ░█████╗░██╗░░██╗░█████╗░██████╗░████████╗  
██╔════╝██║░░░██║██╔════╝████╗░██║╚══██╔══╝██╔════╝    ██╔══██╗██║░░██║██╔══██╗██╔══██╗╚══██╔══╝  
█████╗░░╚██╗░██╔╝█████╗░░██╔██╗██║░░░██║░░░╚█████╗░    ██║░░╚═╝███████║███████║██████╔╝░░░██║░░░  
██╔══╝░░░╚████╔╝░██╔══╝░░██║╚████║░░░██║░░░░╚═══██╗    ██║░░██╗██╔══██║██╔══██║██╔══██╗░░░██║░░░  
███████╗░░╚██╔╝░░███████╗██║░╚███║░░░██║░░░██████╔╝    ╚█████╔╝██║░░██║██║░░██║██║░░██║░░░██║░░░  
╚══════╝░░░╚═╝░░░╚══════╝╚═╝░░╚══╝░░░╚═╝░░░╚═════╝░    ░╚════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░  

░░░░░██╗░██████╗    ░█████╗░███╗░░██╗██╗███╗░░░███╗░█████╗░████████╗░█████╗░██████╗░
░░░░░██║██╔════╝    ██╔══██╗████╗░██║██║████╗░████║██╔══██╗╚══██╔══╝██╔══██╗██╔══██╗
░░░░░██║╚█████╗░    ███████║██╔██╗██║██║██╔████╔██║███████║░░░██║░░░██║░░██║██████╔╝
██╗░░██║░╚═══██╗    ██╔══██║██║╚████║██║██║╚██╔╝██║██╔══██║░░░██║░░░██║░░██║██╔══██╗
╚█████╔╝██████╔╝    ██║░░██║██║░╚███║██║██║░╚═╝░██║██║░░██║░░░██║░░░╚█████╔╝██║░░██║
░╚════╝░╚═════╝░    ╚═╝░░╚═╝╚═╝░░╚══╝╚═╝╚═╝░░░░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝

*/
//Single place for all code related to animating Events Chart SVG, used by light & full viewer

export class ID {
    static CursorLineLegendTemplate = `#CursorLineLegendTemplate`;
    static CursorLineLegendCloneCls = ".CursorLineLegendClone";
    static LifeEventNameLabelCls = ".name-label";
    static LifeEventVerticalLineCls = ".vertical-line";
    static CursorLineLegendDescriptionHolder = "#CursorLineLegendDescriptionHolder";
    static EventChartHolder = ".EventChartHolder"; //main chart SVG element, used class since ID is unique number
    static EventsChartSvgHolder = "#EventsChartSvgHolder"; //default expected parent in page to inject chart into
    static CursorLine = '#CursorLine';
    static CursorLineLegendClone = "CursorLineLegendClone";
    static CursorLineLegendHolder = "#CursorLineLegendHolder";
    static CursorLineSumIcon = "#CursorLineSumIcon";
    static NowVerticalLine = '#NowVerticalLine';
    static EventListHolder = ".EventListHolder";
    static CursorLineClockIcon = "#CursorLineClockIcon";
    static CursorLineLegendDescription = "#CursorLineLegendDescription";
    static CursorLineLegendDescriptionBackground = "#CursorLineLegendDescriptionBackground";
}

//this class is a living version SVG Events Chart
//DESIGN NOTE: no logic to generate chart should be here
//all generation via URL or API is to be done as separate helper functions only
export class EventsChart {
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
        this.AllEventRects = this.$SvgChartElm.find(ID.EventListHolder).children("rect");
        this.$CursorLineLegendDescriptionHolder = this.$SvgChartElm.find(ID.CursorLineLegendDescriptionHolder);
        this.$CursorLineLegendTemplate = this.$SvgChartElm.find(ID.CursorLineLegendTemplate);
        this.$CursorLineLegendDescription = this.$SvgChartElm.find(ID.CursorLineLegendDescription);
        this.$CursorLineLegendDescriptionBackground = this.$SvgChartElm.find(ID.CursorLineLegendDescriptionBackground);
        this.$CursorLineLegendDescriptionHolder = this.$SvgChartElm.find(ID.CursorLineLegendDescriptionHolder);
        this.DescText = { xAxis: 175, yAxis: 24 }; //used to position desc box cursor legend
        this.$CursorLineLegendHolder = this.$SvgChartElm.find(ID.CursorLineLegendHolder);

        //bring to life
        this.attachEventHandlers();

        //add chart to public list of charts after brought to live
        //create new if 1st chart on page
        window.EventsChartLoaded = this;
        if (typeof window.EventsChartList === 'undefined') { window.EventsChartList = []; }
        window.EventsChartList.push(this);

        //return index of last row pushed
        return window.EventsChartList.length - 1;
    }

    //------------------------------------------------------------------------

    attachEventHandlers() {
        console.log("Attaching events to chart...");

        //1 TIME LEGEND
        //we pump the current EventChart instance into handler
        this.$SvgChartElm.mousemove((mouse) => EventsChart.onMouseMoveHandler(mouse, this));
        this.$SvgChartElm.mouseleave((mouse) => EventsChart.onMouseLeaveEventChart(mouse, this));

        //2 NOW LINE
        //update once now
        EventsChart.updateNowLine(this);

        //setup to auto update every 1 minute
        setInterval(() => EventsChart.updateNowLine(this), 60 * 1000); // 60 seconds

        //3 HIGHLIGHT LIFE EVENT
        this.$LifeEventNameLabel.mouseenter((mouse) => EventsChart.onMouseEnterLifeEventHandler(mouse, this));
        this.$LifeEventNameLabel.mouseleave((mouse) => EventsChart.onMouseLeaveLifeEventHandler(mouse, this));

    }

    //on click add events to google calendar,
    //ask user to select event and take from there
    AddEventsToGoogleCalendar() {
        console.log("Adding events to Google Calendar");

        //tell user to select an event
        Swal.fire({
            title: 'Select an event',
            text: 'The selected event will be sent to your Google Calendar',
            icon: 'info',
            confirmButtonText: 'OK'
        });

        //get selected event by user
        $(".EventChartContent rect").one("click", (eventData) => EventsChart.onClickSelectedGoogleCalendarEvent(eventData, this));

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
                oriColor = oriColor === null ? svgEventRect.getAttribute("fill") : oriColor;

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
            case "sun": return "#FFA500";  //orange #FFA500
            case "moon": return "#7A7A7A"; //silver #7A7A7A
            case "mars": return "#DC143C"; //crimson #DC143C
            case "mercury": return "#00FF7F"; //springgreen #00FF7F
            case "jupiter": return "#EEEE00"; //yellow #EEEE00
            case "venus": return "#FF00FF";//magenta #FF00FF
            case "saturn": return "#0000FF";//blue #0000FF
            case "rahu": return "#FF7D40"; //flesh #FF7D40
            case "ketu": return "#515151"; //grey #515151

            //house
            //colors is the full spectrum divided into 12
            //done to have the most unique colors possible for each house
            case "house 1": return "#ff0000"; //red
            case "house 2": return "#ff7f0a"; //orange
            case "house 3": return "#ffff00"; //yellow
            case "house 4": return "#7fff00"; //chartreuse green
            case "house 5": return "#00ff00"; //green
            case "house 6": return "#00ff7f"; //spring green
            case "house 7": return "#00ffff"; //cyan
            case "house 8": return "#007fff"; //azure
            case "house 9": return "#0000ff"; //blue
            case "house 10": return "#7f00ff";//violet
            case "house 11": return "#ff00ff";//magenta
            case "house 12": return "#ff007f";//rose

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
        if (xAxisNowRect) { instance.$NowVerticalLine.attr('transform', `matrix(1, 0, 0, 1, ${xAxisNowRect}, 0)`); }

        //----------------------------------LOCAL FUNK------------------------

        function findClosest(svgEventRect) {
            //get parsed time from rect
            var rectTime = getTimeInRect(svgEventRect).getTime();//(milliseconds since 1 Jan 1970)
            var nowTime = Date.now();

            //if not yet reach continue, keep reference to this and goto next
            if (rectTime <= nowTime) {
                closestRectToNow = svgEventRect;
                return true; //go next
            }
            //already passed now time, use previous rect as now, stop looking
            else { return false; }
        }

        //parses the STD time found in rect and returns it
        function getTimeInRect(eventRect$) {
            //convert "00:28 17/11/2022 +08:00" to "2019-01-01T00:00:00.000+00:00"
            var stdTimeRaw = eventRect$.getAttribute("stdtime");
            var stdTimeSplit = stdTimeRaw.split(" ");
            var hourMin = stdTimeSplit[0];
            var dateFull = stdTimeSplit[1].split('/');
            var date = dateFull[0];
            var month = dateFull[1];
            var year = dateFull[2];
            var timezone = stdTimeSplit[2];
            var rectTime = new Date(`${year}-${month}-${date}T${hourMin}:00.000${timezone}`);

            return rectTime;
        }
    }

    //on mouse leave event chart, auto hide time legend
    static onMouseLeaveEventChart(mouse, instance) { instance.$CursorLine.hide(); }

    //on mouse over life event name label, highlight event line
    static onMouseEnterLifeEventHandler(mouse, instance) {

        //get label that has mouse over it
        var targetElement = mouse.currentTarget;

        //find the main vertical line for life event
        var $parent = $(targetElement).parent();
        var $verticalLine = $parent.siblings(ID.LifeEventVerticalLineCls);

        //make wider
        $verticalLine.attr('width', '3');

        //highlight color
        $verticalLine.attr('fill', '#e502fa');

        //glow
        $verticalLine.css('filter', 'drop-shadow(0px 0px 1px rgb(255 0 0))');

    }

    //on mouse leave life event name label, unhighlight event line
    static onMouseLeaveLifeEventHandler(mouse, instance) {

        //get label that has mouse over it
        var targetElement = mouse.currentTarget;

        //find the main vertical line for life event
        var $parent = $(targetElement).parent();
        var $verticalLine = $parent.siblings(ID.LifeEventVerticalLineCls);

        //set back normal line width
        $verticalLine.attr('width', '2');

        //set line color back to default
        $verticalLine.attr('fill', '#1E1EEA');

        //glow
        $verticalLine.css('filter', '');
    }

    //Gets a mouses x axis relative inside the given element
    //used to get mouse location on SVG chart, zoom auto corrected 
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
                yAxis: mouseEventData.originalEvent.offsetY / zoom
            };
        }
        //in svg direct browser we don't have DIV holder, so no zoom correction
        else {
            mousePosition = {
                xAxis: mouseEventData.originalEvent.offsetX,
                yAxis: mouseEventData.originalEvent.offsetY
            };
        }

        return mousePosition;
    }

    static onClickSelectedGoogleCalendarEvent(eventObject, instance) {

        //get details on the selected event
        var targetRect = eventObject.target;
        var eventName = targetRect.getAttribute("eventname");
        var eventDescription = targetRect.getAttribute("eventdescription");
        var eventStdTime = targetRect.getAttribute("stdtime");

        //show event that was selected
        console.log(`EventSelected: ${eventName}`);

        //debugger;
        //EventsChart.LoginGoogleCalendarUser(instance);
        EventsChart.handleAuthClick();
    }



    static addEventToGoogleCalendar() {

        const event = {
            'summary': 'Google I/O 2015',
            'location': '800 Howard St., San Francisco, CA 94103',
            'description': 'A chance to hear more about Google\'s developer products.',
            'start': {
                'dateTime': '2015-05-28T09:00:00-07:00',
                'timeZone': 'America/Los_Angeles'
            },
            'end': {
                'dateTime': '2015-05-28T17:00:00-07:00',
                'timeZone': 'America/Los_Angeles'
            },
            'recurrence': [
                'RRULE:FREQ=DAILY;COUNT=2'
            ],
            'attendees': [
                { 'email': 'lpage@example.com' },
                { 'email': 'sbrin@example.com' }
            ],
            'reminders': {
                'useDefault': false,
                'overrides': [
                    { 'method': 'email', 'minutes': 24 * 60 },
                    { 'method': 'popup', 'minutes': 10 }
                ]
            }
        };

        const request = window.gapi.client.calendar.events.insert({
            'calendarId': 'primary',
            'resource': event
        });

        request.execute(function (event) {
            alert('Event created: ' + event.htmlLink);
        });
    }

    /**
       *  Sign in the user upon button click.
       */
    static handleAuthClick() {
        window.tokenClient.callback = async (resp) => {
            if (resp.error !== undefined) {
                throw (resp);
            }
            //document.getElementById('signout_button').style.visibility = 'visible';
            //document.getElementById('authorize_button').innerText = 'Refresh';

            EventsChart.addEventToGoogleCalendar();

            //await listUpcomingEvents();
        };

        if (window.gapi.client.getToken() === null) {
            // Prompt the user to select a Google Account and ask for consent to share their data
            // when establishing a new session.
            window.tokenClient.requestAccessToken({ prompt: 'consent' });
        } else {
            // Skip display of account chooser and consent dialog for an existing session.
            window.tokenClient.requestAccessToken({ prompt: '' });
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
                'calendarId': 'primary',
                'timeMin': (new Date()).toISOString(),
                'showDeleted': false,
                'singleEvents': true,
                'maxResults': 10,
                'orderBy': 'startTime',
            };
            response = await window.gapi.client.calendar.events.list(request);
        } catch (err) {
            document.getElementById('content').innerText = err.message;
            return;
        }

        const events = response.result.items;
        if (!events || events.length == 0) {
            console.log('No events found.');
            return;
        }
        // Flatten to string to display
        const output = events.reduce(
            (str, event) => `${str}${event.summary} (${event.start.dateTime || event.start.date})\n`,
            'Events:\n');
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
        if (mousePosition === 0) { SVG(instance.$CursorLine[0]).hide(); return; }
        else { SVG(instance.$CursorLine[0]).show(); }

        //move cursor line 1st for responsiveness
        autoMoveCursorLine(mousePosition.xAxis);

        //update time legend
        generateTimeLegend(mousePosition);


        //-------------------------LOCAL FUNCS--------------------------


        function autoMoveCursorLine(relativeMouseX) {
            //give a tiny delay so user can aim better at event
            setTimeout(() => { }, 157);

            //move vertical line to under mouse inside dasa view box
            instance.$CursorLine.attr('transform', `matrix(1, 0, 0, 1, ${relativeMouseX}, 0)`);
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
            const allElementsAtX = instance.$SvgChartElm.children().find(`[x=${mouseRoundedX}]`);
            const allEventRectsAtX = getAllEventRectsAtX(allElementsAtX);

            // Remove previously generated legend rows
            removePreviousClones();

            //if no elements, don't create summary row, end here (note check only after remove)
            if (!(allEventRectsAtX.length > 0)) { return; }

            // Initialize counts for summary row
            let goodCount = 0;
            let badCount = 0;
            instance.showDescription = false; // Default description not shown

            // Extract event data and place it in legend
            allEventRectsAtX.forEach(element => drawEventRow(element, mouseRoundedY, allElementsAtX));

            // Show or hide description box based on mouse position
            toggleDescriptionBox();

            // Generate summary row at the bottom showing count of good & bad
            generateSummaryRow(allEventRectsAtX, goodCount, badCount);


            //-----------------------------------------LOCAL FUNCS-------------------------------

            //code to draw one event row
            function drawEventRow(svgEventRect, mouseRoundedY, allElementsAtX) {

                //STAGE 1
                //GET DATA
                //extract other data out of the rect
                var eventName = svgEventRect.getAttribute("eventname");
                //if no "eventname" exist, wrong elm skip it
                if (!eventName) { return; }

                var eventDescription = svgEventRect.getAttribute("eventdescription");
                var color = svgEventRect.getAttribute("fill");
                var newRowYAxis = parseInt(svgEventRect.getAttribute("y"));//parse as num, for calculation

                //count good and bad events for summary row
                var eventNatureName = "";// used later for icon color
                //NOTE: this could fail if change in generator
                if (color === EventsChart.BadColor) { eventNatureName = "Bad", badCount++; }
                if (color === EventsChart.GoodColor) { eventNatureName = "Good", goodCount++; }

                //STAGE 2
                //TIME & AGE LEGEND
                //create time legend at top only for first element
                if (allElementsAtX[0] === svgEventRect) { drawTimeAgeLegendRow(); }

                //STAGE 3
                //GENERATE EVENT ROW
                //make a copy of template for this event
                var newLegendRow = instance.$CursorLineLegendTemplate.clone();
                newLegendRow.removeAttr('id'); //remove the clone template id
                newLegendRow.addClass(ID.CursorLineLegendClone); //to delete it on next run
                newLegendRow.appendTo(instance.$CursorLineLegendHolder); //place new legend into special holder
                SVG(newLegendRow[0]).show();//make cloned visible
                //position the group holding the legend over the event row which the legend represents
                newLegendRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis - 2})`);//minus 2 for perfect alignment

                //set event name text & color element
                var $textElm = newLegendRow.children("text");
                var iconElm = newLegendRow.children("use");
                $textElm[0].innerHTML = eventName;
                iconElm.attr("xlink:href", `#CursorLine${eventNatureName}Icon`); //set icon color based on nature

                //STAGE 4
                //GENERATE DESCRIPTION ROW LOGIC
                //check if mouse in within row of this event (y axis)
                var elementTopY = newRowYAxis;
                var elementBottomY = newRowYAxis + EventsChart.RowHeight;
                var mouseWithinRow = mouseRoundedY >= elementTopY && mouseRoundedY <= elementBottomY;
                //if event name is still the same then don't load description again
                var notSameEvent = instance.previousHoverEventName !== eventName;

                //STAGE 5
                //HIGHLIGHT ROW (BASED ON CURSOR)
                //if mouse is in event's row then highlight that row
                if (mouseWithinRow) {
                    //highlight event name row
                    var $backgroundElm = newLegendRow.children("rect");

                    $textElm.attr('font-weight', '700');
                    $backgroundElm.attr("fill", "#0096FF"); //bright blue
                    $backgroundElm.attr("opacity", 1);//solid

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
                    var descBoxYAxis = newRowYAxis - 9;//minus 5 for perfect alignment with event name row
                    instance.$CursorLineLegendDescriptionHolder.attr("transform", `matrix(1, 0, 0, 1, 0, ${descBoxYAxis})`);

                    //note: using trigger to make it easy to skip multiple clogging events
                    //set event desc directly
                    drawDescriptionBox(eventDescription);

                    //update previous hover event
                    instance.previousHoverEventName = eventName;
                }

                //---------------------LOCAL---------------------------

                function drawDescriptionBox(eventDescRaw) {
                    //remove tabs and new line to make easy detection of empty string
                    let eventDesc = eventDescRaw.replace(/ {4}|[\t\n\r]/gm, '');

                    //if no description than hide box & end here
                    if (!eventDesc) { instance.$CursorLineLegendDescriptionHolder.hide(); return; }

                    //convert text to svg and place inside holder
                    var wrappedDescText = textToSvg(eventDesc, instance.DescText.xAxis, instance.DescText.yAxis);

                    instance.$CursorLineLegendDescription.empty(); //clear previous desc
                    $(wrappedDescText).appendTo(instance.$CursorLineLegendDescription); //add in new desc
                    //set height of desc box background
                    instance.$CursorLineLegendDescriptionBackground.attr("height", instance.EventDescriptionTextHeight + 20); //plus little for padding


                    //-------------------------------------LOCAL FUNCTIONS-------------------------------------------

                    //  This function attempts to create a new svg "text" element, chopping
                    //  it up into "tspan" pieces, if the text is too long
                    function textToSvg(caption, x, y) {
                        //svg "text" element to hold smaller text lines
                        var svgTextHolder = document.createElementNS('http://www.w3.org/2000/svg', 'text');
                        svgTextHolder.setAttributeNS(null, 'x', x);
                        svgTextHolder.setAttributeNS(null, 'y', y);
                        svgTextHolder.setAttributeNS(null, 'font-size', 10);
                        svgTextHolder.setAttributeNS(null, 'fill', '#FFF');
                        svgTextHolder.setAttributeNS(null, 'text-anchor', 'left');

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
                                svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
                                svgTSpan.setAttributeNS(null, 'x', x);
                                svgTSpan.setAttributeNS(null, 'y', y);

                                tSpanTextNode = document.createTextNode(line);
                                svgTSpan.appendChild(tSpanTextNode);
                                svgTextHolder.appendChild(svgTSpan);

                                line = words[n] + " ";
                                y += lineHeight; //place next text row lower
                                lineCount++; //count a line
                            }
                            else {
                                line = testLine;
                            }
                        }

                        //calculate final height in px, save global to be accessed later
                        instance.EventDescriptionTextHeight = lineCount * lineHeight;

                        svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
                        svgTSpan.setAttributeNS(null, 'x', x);
                        svgTSpan.setAttributeNS(null, 'y', y);

                        tSpanTextNode = document.createTextNode(line);
                        svgTSpan.appendChild(tSpanTextNode);

                        svgTextHolder.appendChild(svgTSpan);

                        return svgTextHolder;
                    }

                }

                function drawTimeAgeLegendRow() {
                    //make a copy of the template
                    var newTimeLegend = instance.$CursorLineLegendTemplate.clone();

                    //modify the template with new data
                    newTimeLegend.removeAttr('id'); //remove the clone template id
                    newTimeLegend.addClass(ID.CursorLineLegendClone); //to delete it on next run
                    newTimeLegend.appendTo(instance.$CursorLineLegendHolder); //place new legend into special holder

                    //make cloned visible
                    SVG(newTimeLegend[0]).show();

                    //place above 1st row
                    newTimeLegend.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis - EventsChart.RowHeight})`);

                    //get time & age data and place into top legend row
                    var stdTimeFull = svgEventRect.getAttribute("stdtime");
                    var stdTimeSplit = stdTimeFull.split(" ");
                    var hourMin = stdTimeSplit[0];
                    var date = stdTimeSplit[1];
                    var timezone = stdTimeSplit[2];
                    var age = svgEventRect.getAttribute("age");
                    newTimeLegend.children("text").text(`${hourMin} ${date}  AGE: ${age}`);

                    //replace circle with clock icon
                    newTimeLegend.children("use").attr("xlink:href", ID.CursorLineClockIcon);
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
                const previousClones = instance.$SvgChartElm.find(ID.CursorLineLegendCloneCls);
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
                const newSummaryRow = instance.$CursorLineLegendTemplate.clone().removeAttr('id');

                // Add class to the new summary row and append it to the holder
                newSummaryRow.addClass(ID.CursorLineLegendClone).appendTo(instance.$CursorLineLegendHolder);

                // Show the new summary row
                SVG(newSummaryRow[0]).show();

                // Get the last event and its y-axis
                const lastEvent = allEventRectsAtX[allEventRectsAtX.length - 1];
                const lastEvtRowYAxis = parseInt(lastEvent.getAttribute("y"));

                // Calculate the y-axis for the summary row
                const summaryRowYAxis = lastEvtRowYAxis + EventsChart.RowHeight - 1;

                // Transform the new summary row
                newSummaryRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${summaryRowYAxis})`);

                // Get the text element and set its text
                const textElm = newSummaryRow.children("text");
                textElm.text(` Good : ${goodCount}   Bad : ${badCount}`);

                // Set the href for the use element
                newSummaryRow.children("use").attr("xlink:href", ID.CursorLineSumIcon);
            }
        }
    }
}





