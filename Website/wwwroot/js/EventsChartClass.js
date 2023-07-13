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

 class ID {
    static CursorLineLegendTemplate = `#CursorLineLegendTemplate`;
    static CursorLineLegendCloneCls = ".CursorLineLegendClone";
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
        this.$SvgChartElm = $(`#${chartId}`);
        this.Id = chartId;
        this.$CursorLine = this.$SvgChartElm.find(ID.CursorLine);
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

        //we pump the current EventChart instance into handler
        this.$SvgChartElm.mousemove((mouse) => EventsChart.onMouseMoveHandler(mouse, this));
        this.$SvgChartElm.mouseleave((mouse) => EventsChart.onMouseLeaveEventChart(mouse, this));

        //update once now
        EventsChart.updateNowLine(this);

        //setup to auto update every 1 minute
        setInterval(() => EventsChart.updateNowLine(this), 60 * 1000); // 60 seconds
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

    //fired when mouse moves over dasa view box
    //used to auto update cursor line & time legend
    static onMouseMoveHandler(mouse, instance) {

        //get relative position of mouse in Dasa view
        var mousePosition = getMousePositionInElement(mouse);

        //if cursor is out of chart view hide cursor and end here
        if (mousePosition === 0) { SVG(instance.$CursorLine[0]).hide(); return; }
        else { SVG(instance.$CursorLine[0]).show(); }

        //move cursor line 1st for responsiveness
        autoMoveCursorLine(mousePosition.xAxis);

        //update time legend
        generateTimeLegend(mousePosition);


        //-------------------------LOCAL FUNCS--------------------------

        //Gets a mouses x axis relative inside the given element
        //used to get mouse location on Dasa view
        //returns 0 when mouse is out
        function getMousePositionInElement(mouseEventData) {
            //gets the measurements of the dasa view holder
            //the element where cursor line will be moving
            let chartMeasurements = instance.$SvgChartElm[0]?.getBoundingClientRect();

            //if holder measurements invalid then end here
            if (!chartMeasurements) { return 0; }

            //calculate mouse X relative to dasa view box
            let relativeMouseX = mouseEventData.clientX - chartMeasurements.left;
            let relativeMouseY = mouseEventData.clientY - chartMeasurements.top; //when mouse leaves top
            let relativeMouseYb = mouseEventData.clientY - chartMeasurements.bottom; //when mouse leaves bottom

            //if mouse out of element element, set 0 as indicator
            let mouseOut = relativeMouseY < 0 || relativeMouseX < 0 || relativeMouseYb > 0;

            if (mouseOut) {
                return 0;
            } else {
                var mouse = {
                    xAxis: relativeMouseX,
                    yAxis: relativeMouseY
                };
                return mouse;
            }
        }

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
            //x axis is rounded because axis value in rect is whole numbers
            //and it has to be exact match to get it
            var mouseRoundedX = Math.round(mousePosition.xAxis);
            var mouseRoundedY = Math.round(mousePosition.yAxis);

            //use the mouse position to get all event rects
            //dasa elements at same X position inside the dasa svg
            //note: faster and less erroneous than using mouse.path (fruits of time)
            //- get only with event name
            var allElementsAtX = instance.$SvgChartElm.children().find(`[x=${mouseRoundedX}]`);
            var allEventRectsAtX = [];
            allElementsAtX.each(
                (index, element) => {
                    var eventName = element.getAttribute("eventname");
                    //if no "eventname" exist, wrong elm skip it
                    if (eventName) { allEventRectsAtX.push(element); }
                });

            //delete previously generated legend rows
            var previousClones = instance.$SvgChartElm.find(ID.CursorLineLegendCloneCls); //get latest ones by direct call
            previousClones.remove();

            //count good and bad events for summary row
            var goodCount = 0;
            var badCount = 0;
            instance.showDescription = false;//default description not shown

            //extract event data out and place it in legend
            $(allEventRectsAtX).each((index, element) => drawEventRow(element, mouseRoundedY, allElementsAtX));

            //auto show/hide description box based on mouse position
            if (instance.showDescription) {
                SVG(instance.$CursorLineLegendDescriptionHolder[0]).show();
            } else {
                SVG(instance.$CursorLineLegendDescriptionHolder[0]).hide();
            }

            //5 GENERATE LAST SUMMARY ROW
            //generate summary row at the bottom show count of good & bad
            //make a copy of template for this event
            var newSummaryRow = instance.$CursorLineLegendTemplate.clone();
            newSummaryRow.removeAttr('id'); //remove the clone template id
            newSummaryRow.addClass(ID.CursorLineLegendClone); //to delete it on next run
            newSummaryRow.appendTo(instance.$CursorLineLegendHolder); //place new legend into parent
            SVG(newSummaryRow[0]).show();//make cloned visible

            //position summary at bottom
            var lastEvent = allEventRectsAtX[allEventRectsAtX.length - 1];
            var lastEvtRowYAxis = parseInt(lastEvent.getAttribute("y"));
            var summaryRowYAxis = lastEvtRowYAxis + EventsChart.RowHeight - 1;
            newSummaryRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${summaryRowYAxis})`);//minus 1 for perfect alignment

            //set event name text & color element
            var textElm = newSummaryRow.children("text");
            textElm.text(` Good : ${goodCount}   Bad : ${badCount}`); //spacing set nicely
            //change icon to summary icon
            newSummaryRow.children("use").attr("xlink:href", ID.CursorLineSumIcon);


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
        }
    }
}

//LOGIC FUNCTIONS TO





