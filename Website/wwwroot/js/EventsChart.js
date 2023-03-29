
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
    static EventChartHolder = ".EventChartHolder";
    static CursorLine = '#CursorLine';
    static CursorLineLegendClone = "CursorLineLegendClone";
    static CursorLineLegendHolder = "#CursorLineLegendHolder";
    static CursorLineSumIcon = "#CursorLineSumIcon";
    static NowVerticalLine = '#NowVerticalLine';
    static EventListHolder = ".EventListHolder";
    static CursorLineClockIcon = "#CursorLineClockIcon";
}

class EventsChart {

    //inputed SVG chart can also be flag "URL"
    constructor($SvgChartElm) {
        console.log(`Creating new Events Chart...`);

        this.$SvgChartElm = $SvgChartElm;

        //make available for access from blazor
        window.EventsChart = this;

    }

    //ctor to call when need to make chart from data in URL (easy charts)
    static async ChartFromUrl($ChartParentElm) {

        console.log(`From URL`);

        //this.ChartMode = "URL"; //save for later when animating
        //get data to generate chart from URL
        var chartData = EventsChart.getDataFromUrl();

        var svgChartString = await EventsChart.getEventsChartFromApi(chartData);

        var $SvgChartElm = EventsChart.injectIntoElement($ChartParentElm[0], svgChartString);

        var newChart = new EventsChart($SvgChartElm);

        newChart.animateChart();
    }

    //used by blazor pages
    static async chartFromSvgString(svgChartString, $ChartParentElm) {

        console.log(`From SVG String`);

        $ChartParentElm = $($ChartParentElm);

        var $SvgChartElm = EventsChart.injectIntoElement($ChartParentElm[0], svgChartString);

        var newChart = new EventsChart($SvgChartElm);

        newChart.animateChart();
    }


    //INSTANCE METHODS
    //brings the chart to live, call after constructor
    async animateChart() {

        //set title for easy multi-tabbing (not chart related)
        //document.title = `${window?.ChartType} | ${window?.PersonName}`;

        //set toolbar data (not chart related)
        //$("#PersonNameBox").text("Person : " + window?.PersonName);

        //if URL based chart then load from API first
        //if (this.ChartMode === "URL") {

        //    var svgChartString = await EventsChart.getEventsChartFromApi(this.ChartData);

        //    this.$SvgChartElm = EventsChart.injectIntoElement(this.$ChartParentElm[0], svgChartString);
        //}

        await this.attachEventHandlers();

        //make toolbar visible
        //$("#ToolBar").removeClass("visually-hidden");

        //EventsChart.hideLoading();
    }

    //attaches all needed handlers to animate a chart
    async attachEventHandlers() {
        console.log("Attaching events to chart...");

        //attach mouse handler to auto move cursor line & update time legend
        //load event description file 1st
        await EventsChart.loadEventDataListFile();

        this.$SvgChartElm[0].addEventListener("mousemove", EventsChart.onMouseMoveHandler);
        this.$SvgChartElm[0].addEventListener("mouseleave", EventsChart.onMouseLeaveEventChart);


        //checkbox in toolbar
        $(".CheckBox").click((eventData) => EventsChart.handleCheckBoxClicks(eventData, this.$SvgChartElm));

        //save now line
        this.$NowVerticalLine = this.$SvgChartElm.find(ID.NowVerticalLine);

        //update once now
        var nowVerticalLine = this.$NowVerticalLine; //hack to make available in local below
        var svgChartElm = this.$SvgChartElm; //hack to make available in local below
        updateNowLine();

        //setup to auto update every 1 minute
        setInterval(function () { updateNowLine(); }, 60 * 1000); // 60*1000ms


        //update now line position
        function updateNowLine() {
            console.log("Updating now line position...");

            //get all rects
            var allEventRects = svgChartElm.find(".EventListHolder").children("rect");

            //find closest rect to now time
            var closestRectToNow;
            allEventRects.each(function (index) {
                //get parsed time from rect
                var svgEventRect = this;
                var rectTime = getTimeInRect(svgEventRect).getTime();//(milliseconds since 1 Jan 1970)
                var nowTime = Date.now();

                //if not yet reach continue, keep reference to this and goto next
                if (rectTime <= nowTime) {
                    closestRectToNow = svgEventRect;
                    return true; //go next
                }
                //already passed now time, use previous rect as now, stop looking
                else { return false; }
            });

            //get horizontal position of now rect (x axis) (conditional access, not initialized all the time)
            var xAxisNowRect = closestRectToNow?.getAttribute("x");

            //only set line position if, data is valid
            if (xAxisNowRect) { nowVerticalLine.attr('transform', `matrix(1, 0, 0, 1, ${xAxisNowRect}, 0)`); }

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

    async downloadChartFileSVG() {


        //chart name is file name
        var _fileName = "Chart";

        //change svg node to string, then add XML encoding info,
        //since it is going to be an independent SVG file 
        var svgElmString = this.$SvgChartElm[0].outerHTML;
        var encoding = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        var finalSvg = encoding + svgElmString;

        //do the download
        download(finalSvg, "image/svg+xml", _fileName);

        //turns given content to file for download
        function download(content, mimeType, filename) {
            const a = document.createElement('a') // Create "a" element
            const blob = new Blob([content], { type: mimeType }) // Create a blob (file-like object)
            const url = URL.createObjectURL(blob) // Create an object URL from blob
            a.setAttribute('href', url) // Set "a" element link
            a.setAttribute('download', filename) // Set download filename
            a.click() // Start downloading
        }
    }


    //STATIC FUNCTIONS

    static handleCheckBoxClicks(eventData, $SvgChartElm) {

        var checkbox = $(eventData.currentTarget);
        var tickElm = checkbox.find('.Tick');

        //toggle display of the tick mark
        //if tick visible, then tick on
        var tickOn = EventsChart.toggleElm(tickElm);

        //get text of check box
        var text = checkbox.find("text").text();

        //based on text handle the call appropriately
        switch (text) {
            case 'Life Events': EventsChart.toggleElm($("#LifeEventLinesHolder")); break;
            case 'Color Summary': EventsChart.toggleElm($("#ColorRow")); break;
            case 'Smart Summary': EventsChart.toggleElm($("#BarChartRowSmart")); break;
            case 'Bar Summary': EventsChart.toggleElm($("#BarChartRow")); break;
            default:
                {
                    if (tickOn) {
                        EventsChart.highlightByEventName(text, $SvgChartElm);
                    } else {
                        EventsChart.unhighlightByEventName(text, $SvgChartElm);
                    }
                }
        }

    }

    //needs to be run once before get event description method is used
    //loads xml file located in wwwroot to xml global data
    static async loadEventDataListFile() {

        console.log(`Getting event data file from API...`);

        var response = await window.fetch("https://www.vedastro.org/data/EventDataList.xml");

        var rawData = await response.text();
        var xmlDoc = $.parseXML(rawData);
        var $xml = $(xmlDoc);
        window.$EventDataListXml = $xml;

    }

    //highlights all events rects in chart by
    //the inputed keyword in the event name
    static highlightByEventName(keyword, $SvgChartElm) {

        //get all rects
        var allEventRects = $SvgChartElm.find(ID.EventListHolder).children("rect");

        //find all rects representing the keyword based event
        //note keyword will be planet name or house name
        allEventRects.each(function (index) {
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


    static unhighlightByEventName(keyword, $SvgChartElm) {

        //get all rects
        var allEventRects = $SvgChartElm.find(ID.EventListHolder).children("rect");

        //find all rects representing the keyword based event
        allEventRects.each(function (index) {
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

    //get screen width
    static getScreenWidth() {
        return Math.max(
            document.body.scrollWidth,
            document.documentElement.scrollWidth,
            document.body.offsetWidth,
            document.documentElement.offsetWidth,
            document.documentElement.clientWidth
        );
    }

    //toggle hide and show of elements via SVG.js lib
    //returns true if set to visible, false if set to hide
    static toggleElm(element) {

        var svgElm = SVG(element[0]);

        var isVisible = svgElm.visible();
        if (isVisible) {
            svgElm.hide();
        } else {
            svgElm.show();
        }

        //get updated is visibility
        return svgElm.visible();
    }


    //gets clients timezone offset, exp:"+08:00"
    static getParseableTimezone() {
        var date = new Date();
        var tzo = -date.getTimezoneOffset(),
            dif = tzo >= 0 ? '+' : '-',
            pad = function (num) {
                return (num < 10 ? '0' : '') + num;
            };

        return dif +
            pad(Math.floor(Math.abs(tzo) / 60)) +
            ':' +
            pad(Math.abs(tzo) % 60);
    }

    static getDataFromToolbar() {

        //get needed data out of selection boxes
        var chartData = {
            "timePreset": $("#TimePresetSelector").val(),
            "eventPreset": $("#EventPresetSelector").val(),
            "personId": window.PersonId,
            "maxWidth": EventsChart.getScreenWidth(),
            "timezone": EventsChart.getParseableTimezone(),
            "url": ""//empty string first
        };

        //reset parts of the URL with data from selection boxes
        var pathname = window.location.pathname.split("/");
        pathname[pathname.length - 1] = chartData.timePreset;
        pathname[pathname.length - 2] = chartData.eventPreset;
        pathname[pathname.length - 3] = chartData.personId;

        //construct the final URL back
        var newUrl = pathname.join('/');
        chartData.url = newUrl;
        return chartData;
    }

    //gets data needed to make chart from URL
    static getDataFromUrl() {
        console.log(`Getting chart data from URL...`);

        //get data out of url
        var pathname = window.location.pathname.split("/");
        //url pattern chart/{personId}/{eventPreset}/{timePreset}
        var returnVal = {
            "timePreset": pathname[pathname.length - 1],
            "eventPreset": pathname[pathname.length - 2],
            "personId": pathname[pathname.length - 3],
            "maxWidth": EventsChart.getScreenWidth(),
            "timezone": EventsChart.getParseableTimezone()
        };

        return returnVal;
    }

    //fired when mouse moves over dasa view box
    //used to auto update cursor line & time legend
    static async onMouseMoveHandler(mouse) {

        //get relative position of mouse in Dasa view
        var mousePosition = getMousePositionInElement(mouse, ID.EventChartHolder);

        //if mouse is out of dasa view hide cursor and end here
        if (mousePosition == 0) { SVG(ID.CursorLine).hide(); return; }
        else { SVG(ID.CursorLine).show(); }

        //move cursor line 1st for responsiveness
        autoMoveCursorLine(mousePosition.xAxis);

        //update time legend
        generateTimeLegend(mousePosition);

        //FUNCTIONS
        //Gets a mouses x axis relative inside the given element
        //used to get mouse location on Dasa view
        //returns 0 when mouse is out
        function getMousePositionInElement(mouseEventData, elementId) {

            //gets the measurements of the dasa view holder
            //the element where cursor line will be moving
            //TODO read val from global var
            let holderMeasurements = $(elementId)[0]?.getBoundingClientRect();

            //if holder measurements invalid then end here
            if (!holderMeasurements) { return 0; }

            //calculate mouse X relative to dasa view box
            let relativeMouseX = mouseEventData.clientX - holderMeasurements.left;
            let relativeMouseY = mouseEventData.clientY - holderMeasurements.top; //when mouse leaves top
            let relativeMouseYb = mouseEventData.clientY - holderMeasurements.bottom; //when mouse leaves bottom

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
            //move vertical line to under mouse inside dasa view box
            $(ID.CursorLine).attr('transform', `matrix(1, 0, 0, 1, ${relativeMouseX}, 0)`);
        }

        //SVG Event Chart Time Legend generator
        //this is where the whole time legend that follows
        //the mouse when placed on chart is generated
        //notes: a template row always exists in code,
        //in client JS side uses template to create the rows from cloning it
        //and modifying its prop as needed, as such any major edit needs to
        //be done in API code
        function generateTimeLegend(mousePosition) {
            //console.log(`JS: autoUpdateTimeLegend`);

            //x axis is rounded because axis value in rect is whole numbers
            //and it has to be exact match to get it
            var mouseRoundedX = Math.round(mousePosition.xAxis);
            var mouseRoundedY = Math.round(mousePosition.yAxis);

            //use the mouse position to get all dasa rect
            //dasa elements at same X position inside the dasa svg
            //note: faster and less erroneous than using mouse.path
            var children = $(ID.EventChartHolder).children();
            var allElementsAtX = children.find(`[x=${mouseRoundedX}]`);


            //delete previously generated legend rows
            $(ID.CursorLineLegendCloneCls).remove();


            //count good and bad events for summary row
            var goodCount = 0;
            var badCount = 0;
            var newRowYAxis = 0; //vertical position of the current event row
            window.showDescription = false;//default description not shown
            //extract event data out and place it in legend
            allElementsAtX.each(drawEventRow);

            //auto show/hide description box based on mouse position
            if (window.showDescription) {
                SVG(ID.CursorLineLegendDescriptionHolder).show();
            } else {
                SVG(ID.CursorLineLegendDescriptionHolder).hide();
            }



            //5 GENERATE LAST SUMMARY ROW
            //generate summary row at the bottom
            //make a copy of template for this event
            var newSummaryRow = $(ID.CursorLineLegendTemplate).clone();
            newSummaryRow.removeAttr('id'); //remove the clone template id
            newSummaryRow.addClass(ID.CursorLineLegendClone); //to delete it on next run
            newSummaryRow.appendTo(ID.CursorLineLegendHolder); //place new legend into parent
            SVG(newSummaryRow[0]).show();//make cloned visible
            //position the group holding the legend over the event row which the legend represents
            newSummaryRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis + 15 - 1})`);//minus 1 for perfect alignment

            //set event name text & color element
            var textElm = newSummaryRow.children("text");
            textElm.text(` Good : ${goodCount}   Bad : ${badCount}`);
            //change icon to summary icon
            newSummaryRow.children("use").attr("xlink:href", ID.CursorLineSumIcon);

            //FUNCTIONS

            //
            function drawEventRow() {

                //this is the rect generated by API containing data about the event
                var svgEventRect = this;

                //1 GET DATA
                //extract other data out of the rect
                var eventName = svgEventRect.getAttribute("eventname");
                //if no "eventname" exist, wrong elm skip it
                if (!eventName) { return; }

                var eventDescription = svgEventRect.getAttribute("eventdescription");
                var color = svgEventRect.getAttribute("fill");
                newRowYAxis = parseInt(svgEventRect.getAttribute("y"));//parse as num, for calculation

                //count good and bad events for summary row
                var eventNatureName = "";// used later for icon color
                //todo this could fail if change in generator
                if (color === "#FF0000") { eventNatureName = "Bad", badCount++; }
                if (color === "#00FF00") { eventNatureName = "Good", goodCount++; }


                //2 TIME & AGE LEGEND
                //create time legend at top only for first element
                if (allElementsAtX[0] === svgEventRect) { drawTimeAgeLegendRow(); }

                //3 GENERATE EVENT ROW
                //make a copy of template for this event
                var newLegendRow = $(ID.CursorLineLegendTemplate).clone();
                newLegendRow.removeAttr('id'); //remove the clone template id
                newLegendRow.addClass("CursorLineLegendClone"); //to delete it on next run
                newLegendRow.appendTo("#CursorLineLegendHolder"); //place new legend into special holder
                SVG(newLegendRow[0]).show();//make cloned visible
                //position the group holding the legend over the event row which the legend represents
                newLegendRow.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis - 2})`);//minus 2 for perfect alignment

                //set event name text & color element
                var textElm = newLegendRow.children("text")[0];
                var iconElm = newLegendRow.children("use");
                textElm.innerHTML = eventName;
                //textElm.text(`${eventName}`);
                iconElm.attr("xlink:href", `#CursorLine${eventNatureName}Icon`); //set icon color based on nature

                //4 GENERATE DESCRIPTION ROW LOGIC
                //check if mouse in within row of this event (y axis)
                var elementTopY = newRowYAxis;
                var elementBottomY = newRowYAxis + 15;
                var mouseWithinRow = mouseRoundedY >= elementTopY && mouseRoundedY <= elementBottomY;
                //if event name is still the same then don't load description again
                var notSameEvent = window.previousHoverEventName !== eventName;

                //HIGHLIGHT ROW
                //if mouse is in event's row then highlight that row
                if (mouseWithinRow) {
                    //highlight event name row
                    var backgroundElm = SVG(newLegendRow.children("rect")[0]);
                    backgroundElm.fill("#003e99");
                    backgroundElm.opacity(1);//solid
                    //textElm.css("fill", "#fff");
                    SVG(textElm).font('weight', '700');
                    //if mouse within show description box
                    window.showDescription = true;
                }

                //if mouse within row AND the event has changed
                //then generate a new description
                //note: this is slow, so done only when absolutely needed
                if (mouseWithinRow && notSameEvent) {

                    //make holder visible
                    SVG(ID.CursorLineLegendDescriptionHolder).show();

                    //move holder next to event
                    var descBoxYAxis = newRowYAxis - 9;//minus 5 for perfect alignment with event name row
                    $(ID.CursorLineLegendDescriptionHolder).attr("transform", `matrix(1, 0, 0, 1, 0, ${descBoxYAxis})`);

                    //note: using trigger to make it easy to skip multiple clogging events
                    //set event desc directly
                    drawDescriptionBox(eventDescription);
                    //$(document).trigger('loadEventDescription', eventName);

                    //update previous hover event
                    window.previousHoverEventName = eventName;
                }

                //---------------------LOCAL---------------------------

                function drawDescriptionBox(eventDescRaw) {

                    //remove tabs and new line to make easy detection of empty string
                    let eventDesc = eventDescRaw.replace(/ {4}|[\t\n\r]/gm, '');

                    //if no description than hide box & end here
                    if (!eventDesc) { $(ID.CursorLineLegendDescriptionHolder).hide(); return; }

                    //convert text to svg and place inside holder
                    var wrappedDescText = EventsChart.textToSvg(eventDesc, 175, 24);

                    $("#CursorLineLegendDescription").empty(); //clear previous desc
                    $(wrappedDescText).appendTo("#CursorLineLegendDescription"); //add in new desc
                    //set height of desc box background
                    $("#CursorLineLegendDescriptionBackground").attr("height", window.EventDescriptionTextHeight + 20); //plus little for padding

                }

                function drawTimeAgeLegendRow() {
                    var newTimeLegend = $(ID.CursorLineLegendTemplate).clone();
                    newTimeLegend.removeAttr('id'); //remove the clone template id
                    newTimeLegend.addClass(ID.CursorLineLegendClone); //to delete it on next run
                    newTimeLegend.appendTo(ID.CursorLineLegendHolder); //place new legend into special holder
                    SVG(newTimeLegend[0]).show();//make cloned visible
                    newTimeLegend.attr('transform', `matrix(1, 0, 0, 1, 10, ${newRowYAxis - 15})`); //above 1st row
                    //split time to remove timezone from event
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

    //on mouse leave event chart, auto hide time legend
    static async onMouseLeaveEventChart(mouse) {
        $("#CursorLine").hide();
    }

    //returns the reference to the SVG element in DOM
    static injectIntoElement(parentElement, valueToInject) {
        console.log(`Injecting SVG Chart into page...`);

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

    //gets chart from API & injects it into page
    //returns svg chart as string
    static async getEventsChartFromApi(chartData) {
        console.log(`Getting events chart from API...`);

        var payload = `<Root><PersonId>${chartData.personId}</PersonId><TimePreset>${chartData.timePreset}</TimePreset><EventPreset>${chartData.eventPreset}</EventPreset><Timezone>${chartData.timezone}</Timezone><MaxWidth>${chartData.maxWidth}</MaxWidth></Root>`;

        //TODO special URL for chart because timeout Azure CDN timeout >30s
        //GetEventsChartDirect = "https://vedastroapi.azurewebsites.net/api/geteventschart"; 

        var response = await window.fetch("https://vedastroapi.azurewebsites.net/api/geteventscharteasy", {
            "headers": { "accept": "*/*", "Connection": "keep-alive", "Content-Type": "text/plain" },
            "body": payload,
            "method": "POST",
            "mode": "no-cors"
        });


        //inject new svg chart into page
        var svgChartString = await response.text();

        return svgChartString;
    }


    static getScreenHeight() {
        return Math.max(
            document.body.scrollHeight,
            document.documentElement.scrollHeight,
            document.body.offsetHeight,
            document.documentElement.offsetHeight,
            document.documentElement.clientHeight
        );
    }

    static getHighlightColor(keyword) {

        switch (keyword.toLowerCase()) {
            case "sun": return "#FFFF00";
            case "moon": return "#00FFFF";
            case "mars": return "#ff609b";
            case "mercury": return "#005522";
            case "jupiter": return "#ff60fa";
            case "venus": return "#FF0099";
            case "saturn": return "#60aaff";
            case "rahu": return "#ffb460";
            case "ketu": return "#faff60";
        }

        //default
        return "ff60fa";

        //    var arrayValues = ["#ff60fa", "#ff60fa", "#ff60fa"];

        //    var arrayMax = arrayValues.length - 1;
        //    var randomIndex = Math.floor(Math.random() * arrayMax);

        //    return arrayValues[randomIndex];
    }

    //FUNCTION
    //  This function attempts to create a new svg "text" element, chopping
    //  it up into "tspan" pieces, if the text is too long
    static textToSvg(caption, x, y) {

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
        window.EventDescriptionTextHeight = lineCount * lineHeight;

        svgTSpan = document.createElementNS('http://www.w3.org/2000/svg', 'tspan');
        svgTSpan.setAttributeNS(null, 'x', x);
        svgTSpan.setAttributeNS(null, 'y', y);

        tSpanTextNode = document.createTextNode(line);
        svgTSpan.appendChild(tSpanTextNode);

        svgTextHolder.appendChild(svgTSpan);

        return svgTextHolder;
    }

    //this allows the life event icons to be moved up
    //or down to meet the changing rows
    static setLifeEventsIconYAxis() {
        //use svg.js
    }
}

//GLUE METHOD > KEEP IT CLEAN
async function ChartFromSvgString(rawSvgChart, chartParent) { EventsChart.chartFromSvgString(rawSvgChart, chartParent); }
async function DownloadChartFileSVG() { window.EventsChart.downloadChartFileSVG(); }








//ARCHIVE
////coming from direct/url access page
////the method called to start the JS code to animate an already loaded chart
//async animateEventsChart2() {
//    console.log(`Starting the engine...`);

//    //set title for easy multi-tabbing (not chart related)
//    document.title = `${window?.ChartType} | ${window?.PersonName}`;

//    //set toolbar data (not chart related)
//    $("#PersonNameBox").text("Person : " + window?.PersonName);

//    var notLoaded = !EventsChart.isSvgLoaded();

//    //try get svg from server if svg not loaded by now
//    //coming from live chart generate
//    if (notLoaded) {
//        //get data to generate chart from URL
//        var data = EventsChart.getDataFromUrl();

//        await EventsChart.getEventsChartFromApi(data);
//    }

//    //attach mouse handler to auto move cursor line & update time legend
//    //load event description file 1st
//    //this.svgChart$ = this.$ChartParentElm.children().first();
//    await EventsChart.attachEventHandlers();

//    //make toolbar visible
//    $("#ToolBar").removeClass("visually-hidden");

//    EventsChart.hideLoading();

//}

////animates a new chart from URL
////coming from live chart generate
//async animateChartFromUrl() {

//    ////set title for easy multi-tabbing (not chart related)
//    //document.title = `${window?.ChartType} | ${window?.PersonName}`;

//    ////set toolbar data (not chart related)
//    //$("#PersonNameBox").text("Person : " + window?.PersonName);

//    //---

//    var svgChartString = await EventsChart.getEventsChartFromApi(this.ChartData);

//    this.$SvgChartElm = EventsChart.injectIntoElement(this.$ChartParentElm[0], svgChartString);

//    await this.attachEventHandlers();

//    //---
//    //make toolbar visible
//    //    $("#ToolBar").removeClass("visually-hidden");

//    //    EventsChart.hideLoading();
//}

////returns true if svg loaded
//static isSvgLoaded() {
//    console.log(`Checking if SVG Chart is loaded...`);

//    //check if svg already loaded into page
//    var isLoaded = $("#EventsChartSvgHolder").children().first().is("svg");

//    return isLoaded;
//}
