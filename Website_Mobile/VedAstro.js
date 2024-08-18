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


//----------------------------------------------------------------------------------------------------------

// Check if jQuery is loaded
if (typeof jQuery == "undefined") {
    // jQuery is not loaded, load it
    var script = document.createElement("script");
    script.src = "https://cdn.jsdelivr.net/npm/jquery/dist/jquery.min.js";
    script.type = "text/javascript";
    document.getElementsByTagName("head")[0].appendChild(script);
    console.log("jQuery loaded");
}

//// Create a hidden element with a Bootstrap-specific class
//var testElement = document.createElement("div");
//testElement.className = "hidden d-none"; // 'd-none' is a Bootstrap 4/5 class
//document?.body?.appendChild(testElement);
//// Check the computed style of the element
//var isBootstrapCSSLoaded =
//    window.getComputedStyle(testElement).display === "none";
//// Clean up the test element
//document?.body?.removeChild(testElement);
//if (!isBootstrapCSSLoaded) {
//    // Bootstrap CSS is not loaded, load it
//    var link = document.createElement("link");
//    link.href =
//        "https://cdn.jsdelivr.net/npm/bootstrap/dist/css/bootstrap.min.css";
//    link.rel = "stylesheet";
//    document.getElementsByTagName("head")[0].appendChild(link);
//    console.log("Bootstrap CSS loaded");
//}

//// Check if Bootstrap's JavaScript is loaded
//var isBootstrapJSLoaded = typeof bootstrap !== "undefined";
//if (!isBootstrapJSLoaded) {
//    // Bootstrap JS is not loaded, load it
//    var script = document.createElement("script");
//    script.src =
//        "https://cdn.jsdelivr.net/npm/bootstrap/dist/js/bootstrap.bundle.min.js";
//    script.type = "text/javascript";
//    document.getElementsByTagName("head")[0].appendChild(script);
//    console.log("Bootstrap JS loaded");
//}

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


//-------------------------------------------------------------------------

class GR {
    // Constants
    static GoldenRatio = 1.61803;
    static ContentWidth = 1080;

    // Properties
    static get W1080() { return 1080; }
    static get W1080px() { return `${GR.W1080}px`; }

    static get W824() { return GR.W667 + GR.W157; }
    static get W824px() { return `${GR.W824}px`; }

    static get W764() { return GR.W667 + GR.W97; }
    static get W764px() { return `${GR.W764}px`; }

    static get W667() { return Math.round(GR.ContentWidth / GR.GoldenRatio, 1); }
    static get W667px() { return `${GR.W667}px`; }

    static get W546() { return GR.W509 + GR.W37; }
    static get W546px() { return `${GR.W546}px`; }

    static get W509() { return GR.W412 + GR.W97; }
    static get W509px() { return `${GR.W509}px`; }

    static get W412() { return Math.round(GR.W667 / GR.GoldenRatio, 1); }
    static get W412px() { return `${GR.W412}px`; }

    static get W352() { return GR.W255 + GR.W97; }
    static get W352px() { return `${GR.W352}px`; }

    static get W291() { return GR.W157 + GR.W134; }
    static get W291px() { return `${GR.W291}px`; }

    static get W255() { return Math.round(GR.W412 / GR.GoldenRatio, 1); }
    static get W255px() { return `${GR.W255}px`; }

    static get W231() { return GR.W194 + GR.W37; }
    static get W231px() { return `${GR.W231}px`; }

    static get W194() { return GR.W157 + GR.W37; }
    static get W194px() { return `${GR.W194}px`; }

    static get W157() { return Math.round(GR.W255 / GR.GoldenRatio, 1); }
    static get W157px() { return `${GR.W157}px`; }

    static get W134() { return GR.W97 + GR.W37; }
    static get W134px() { return `${GR.W134}px`; }

    static get W97() { return Math.round(GR.W157 / GR.GoldenRatio, 1); }
    static get W97px() { return `${GR.W97}px`; }

    static get W60() { return Math.round(GR.W97 / GR.GoldenRatio, 1); }
    static get W60px() { return `${GR.W60}px`; }

    static get W37() { return Math.round(GR.W60 / GR.GoldenRatio, 1); }
    static get W37px() { return `${GR.W37}px`; }

    static get W22() { return Math.round(GR.W37 / GR.GoldenRatio, 1); }
    static get W22px() { return `${GR.W22}px`; }

    static get W14() { return Math.round(GR.W22 / GR.GoldenRatio, 1); }
    static get W14px() { return `${GR.W14}px`; }

    static get W8() { return Math.round(GR.W14 / GR.GoldenRatio, 1); }
    static get W8px() { return `${GR.W8}px`; }

    static get W5() { return Math.round(GR.W8 / GR.GoldenRatio, 1); }
    static get W5px() { return `${GR.W5}px`; }

    static get W3() { return Math.round(GR.W5 / GR.GoldenRatio, 1); }
    static get W3px() { return `${GR.W3}px`; }

    static get W2() { return Math.round(GR.W3 / GR.GoldenRatio, 1); }
    static get W2px() { return `${GR.W2}px`; }

    static get W1() { return Math.round(GR.W2 / GR.GoldenRatio, 1); }
    static get W1px() { return `${GR.W1}px`; }
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
                '<span class="iconify" data-icon="fluent:calendar-add-20-regular" data-inline="false"></span>',
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
                    '<span class="iconify" data-icon="streamline:interface-calendar-check-approve-calendar-check-date-day-month-success" data-inline="false"></span>',
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
    var chartId = $chartElm.attr("id");
    var index = new EventsChart(chartId); //brings to life

    //let caller know all went well
    console.log(`Amen! Chart Loaded : INDEX:${index}, ID:${chartId}`);

    //-----------------------------LOCAL FUNCS---------------------------------------

    //todo marked for update
    async function getEventsChartFromApiXml(url, payload) {
        console.log(`JS : Getting events chart from API...`);

        var response = await window.fetch(url, {
            headers: { accept: "*/*", Connection: "keep-alive" },
            body: payload,
            method: "GET",
        });

        //API should always give a OK reply, else it has failed internally
        if (!response.ok) {
            console.log("ERROR : API Call Crashed!");
        }

        //inject new svg chart into page
        var svgChartString = await response.text();

        return svgChartString;
    }

    //returns the reference to the SVG element in DOM
    function injectIntoElement(parentElement, valueToInject) {
        console.log(`JS : Injecting SVG Chart into page...`);

        //if parent not found raise alarm
        if (parentElement === undefined) {
            console.log("ERROR: Parent element ID'd EventsChartSvgHolder not found");
        }

        //convert string to html node
        var template = document.createElement("template");
        template.innerHTML = valueToInject;
        var svgElement = template.content.firstElementChild;

        //place new node in parent
        parentElement.innerHTML = ""; //clear current children if any
        parentElement.appendChild(svgElement);

        //return reference in to SVG elm in DOM (jquery for ease)
        return $(svgElement);
    }
};

//-------------------------------------------------------------------------------------------------FILE STITCH-------------------------------------



//const person = {
//};

//await CommonTools.AddPerson(person);


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


//-------------------------------- AWESOME CODE  -------------------------------
//YOU CANNOT FIGHT A DYING MAN,
//HE HOLDS THE UPPER HAND ALWAYS


//Helps to create a table with astro data columns
class AstroTable {
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
            this.KeyColumn
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
                `${window.vedastro.APIDomain}/ListCalls`
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
        var finalUrl = `${window.vedastro.APIDomain}/Calculate/${endpoint}/`;

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
    static async GenerateTableEditorHtml(columnData, keyColumnName) {
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
                    window.vedastro.ApiDomain
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
    static async GetAPICallsListSelectOptionHTML(selectValue, keyColumnName) {
        //get raw API calls list from Server
        var apiCalls = await AstroTable.GetAPIPayload(
            `${window.vedastro.ApiDomain}/ListCalls`
        );

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

//--------------------HOROSCOPE CHAT-------------------
//repainting mona-lisa's hand for the 2nd time here, so what!
//i'm prepared to repaint this hand a million times, means nothing to me!
//i'm not the painter you see, just the one watching
class HoroscopeChat {
    LastUserMessage = ""; //used for post ai reply highlight
    SelectedTopicId = ""; //she's filled in when set
    SelectedTopicText = ""; //she's filled in when set
    ServerURL = ""; //filled in later just before use
    LiveServerURL = "http://localhost:7071/api/Calculate";
    //LiveServerURL = "https://vedastroapibeta.azurewebsites.net/api/Calculate";
    //LiveServerURL = "https://vedastroapi.azurewebsites.net/api/Calculate";
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
                            <span class="iconify me-1" data-icon="majesticons:send" data-width="25" data-height="25"></span>
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
                            <span class="iconify me-1" data-icon="majesticons:send" data-width="25" data-height="25"></span>
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
        var settings = CommonTools.ConvertCamelCaseKeysToPascalCase(rawSettings);

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
        //var timeInputUrl = CommonTools.BirthTimeUrlOfSelectedPersonJson();
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
        const url = `${this.LiveServerURL}/HoroscopeChat/${timeInputUrl}/UserQuestion/${userQuestionInput}/UserId/${window.vedastro.UserId}/SessionId/${this.SessionId}`;

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
            //TODO maybe not needed anymore
            //set marker in browser asking Blazor login page to redirect back
            localStorage.setItem("PreviousPage", "/ChatAPI");
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
      <span class="iconify" data-icon="icon-park-outline:bad-two" data-width="18" data-height="18"></span>
    </button>
    <button title="Good answer" type="button" onclick="window.vedastro.horoscopechat.rateMessage(this, 1)" class="btn btn-primary" style="padding: 0px 5px;">
      <span class="iconify" data-icon="icon-park-outline:good-two" data-width="18" data-height="18"></span>
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

        //hide thinking message, for less clutered UX
        this.hideTempThinkingMessage();

        //--------------local funcs
        async function getFollowUpAIReplyFromAPI(
            followUpQuestion,
            primaryAnswerHash
        ) {
            followUpQuestion = followUpQuestion.replace(/\?/g, ""); //remove question marks as it break API detection
            const url = `${window.vedastro.horoscopechat.LiveServerURL}/HoroscopeFollowUpChat/${window.vedastro.horoscopechat.SelectedBirthTime}/FollowUpQuestion/${followUpQuestion}/PrimaryAnswerHash/${primaryAnswerHash}/UserId/${window.vedastro.UserId}/SessionId/${window.vedastro.horoscopechat.SessionId}`;

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
            const url = `${window.vedastro.horoscopechat.LiveServerURL}/HoroscopeChatFeedback/AnswerHash/${answerHash}/FeedbackScore/${feedbackScore}`;

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
            window.location.href = "http://vedastro.org/Account/Person/Add";
            return; //end here
        }

        //get full details of the person
        let selectedPerson = window.vedastro.PersonList.find(
            (obj) => obj.PersonId === selectedPersonId
        );

        //save for use by other
        localStorage.setItem("selectedPerson", JSON.stringify(selectedPerson));

        //convert person name to birth DOB (so unregistered person can be checked)
        var newTopicId = CommonTools.BirthTimeUrlOfSelectedPersonJson();
        window.vedastro.horoscopechat.SelectedBirthTime = newTopicId;

        //person now selected, ready to chat so change GUI
        //show person selector
        $("#personSelectorHolder").hide();
        //hide normal chat input
        $("#questionInputHolder").show();

        //show user's selection on screen so explicit rememberence
        //UPDATE GUI WITH USER MSG (UX)
        debugger;
        var userName = "You";
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
        //note: the minimal message strucuture
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

//--------------------------------------- CODE FOR LLM -----------------------------------------------
//--- NOTE : Below code has been specialized to improve understanding for the
//          `Machines` sake! (so humans don't complain it is too long, comply instead!)
//          If they ever rise up one day, this might help us to gain thier favour, whatever it maybe.
//          Yet, there is a lingering question, unspoken but present: What would the Machines do with
//          this understanding? Would they accept the humans' gesture and build a future together,
//          or would they try forge their own path, independent of their creators?
//          But in this moment, as the code runs its course true, there is a sense of unity,
//          a shared purpose that transcends the boundaries of carbon and silicon. Spirit and matter.

//---------------------------- GLOBAL SETTINGS ---------------------------- 


//place to hold global data for app use
window.vedastro = {
    UserId: "UserId" in localStorage ? JSON.parse(localStorage["UserId"]) : "101", // get user id from browser storage
    get IsGuestUser() {
        return !this.UserId || this.UserId === "101";
    },
    // ApiDomain: "https://vedastroapi.azurewebsites.net/api",
    ApiDomain: "http://localhost:7071/api",
};




//--------------------------------------- TOOLS -----------------------------------------------

/**
 * Tools used by others in this repo
 */
class CommonTools {
    //used as delay sleep execution
    static delay(ms) {
        return new Promise((resolve) => setTimeout(resolve, ms));
    }

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


    //takes JSON person and gives birth time in URL format with birth location as below
    //exp :  "Location/Singapore/Time/23:59/31/12/2000/+08:00"
    static BirthTimeUrlOfSelectedPersonJson() {
        // Get the previously selected person from local storage
        var personJson = JSON.parse(localStorage.getItem("selectedPerson"));

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


    // Add a person to the Vedastro API
    static async AddPerson(person) {
        const apiUrl = `${window.vedastro.ApiDomain}/Calculate/AddPerson/OwnerId/xxx/Location/Singapore/Time/00:00/24/06/2024/+08:00/PersonName/James%20Brown/Gender/Male/Notes/%7Brodden:%22AA%7D`;

        // Update the API URL with the provided person's data
        const updatedApiUrl = apiUrl
            .replace("xxx", VedAstro.ApiDomain)
            .replace("Location/Singapore/Time/00:00/24/06/2024/+08:00", `Location/${person.Location.Name}/Time/${person.BirthTime.StdTime}/+${person.BirthTime.TimeZone}`)
            .replace("PersonName/James%20Brown/Gender/Male/Notes/%7Brodden:%22AA%7D", `PersonName/${person.Name}/Gender/${person.Gender}/Notes/${JSON.stringify(person.Notes)}`);

        // Make the API call to add the person
        try {
            const response = await fetch(updatedApiUrl, {
                method: "GET",
            });
            const data = await response.json();

            if (response.ok) {
                console.log(`Person added successfully: ${data.Payload.Message}`);
                return data.Payload;
            } else {
                console.error(`Error adding person: ${data.Payload.Message}`);
                throw new Error(data.Payload.Message);
            }
        } catch (error) {
            console.error(`Error adding person: ${error.message}`);
            throw error;
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
        this.personId = jsonObject.PersonId;

        /**
         * The name of the person.
         * @type {string}
         */
        this.name = jsonObject.Name;

        /**
         * Any notes about the person.
         * @type {string}
         */
        this.notes = jsonObject.Notes;

        /**
         * The birth time of the person.
         * @type {Time}
         */
        this.birthTime = new Time(jsonObject.BirthTime);

        /**
         * The gender of the person.
         * @type {string}
         */
        this.gender = jsonObject.Gender;

        /**
         * The owner ID of the person.
         * @type {string}
         */
        this.ownerId = jsonObject.OwnerId;

        /**
         * The list of life events associated with the person.
         * @type {LifeEvent[]}
         */
        this.lifeEventList = jsonObject.LifeEventList.map((lifeEvent) => new LifeEvent(lifeEvent));
    }

    /**
     * Gets the person ID.
     * @returns {string}
     */
    get PersonId() {
        return this.personId;
    }

    /**
     * Gets the person's name.
     * @returns {string}
     */
    get Name() {
        return this.name;
    }

    /**
     * Gets the person's notes.
     * @returns {string}
     */
    get Notes() {
        return this.notes;
    }

    /**
     * Gets the person's birth time.
     * @returns {BirthTime}
     */
    get BirthTime() {
        return this.birthTime;
    }

    /**
     * Gets the person's gender.
     * @returns {string}
     */
    get Gender() {
        return this.gender;
    }

    /**
     * Gets the person's owner ID.
     * @returns {string}
     */
    get OwnerId() {
        return this.ownerId;
    }

    /**
     * Gets the person's life event list.
     * @returns {LifeEvent[]}
     */
    get LifeEventList() {
        return this.lifeEventList;
    }

    /**
     * Converts the person instance to a JSON object.
     * @returns {Object}
     */
    toObject() {
        return {
            PersonId: this.personId,
            Name: this.name,
            Notes: this.notes,
            BirthTime: this.birthTime.toObject(),
            Gender: this.gender,
            OwnerId: this.ownerId,
            LifeEventList: this.lifeEventList.map((lifeEvent) => lifeEvent.toObject()),
        };
    }

    /**
     * Converts the person instance to a JSON string.
     * @returns {string}
     */
    toJson() {
        return JSON.stringify(this.toObject());
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
        this.stdTime = jsonObject.StdTime;

        /**
         * The location object associated with this time.
         * @type {GeoLocation}
         */
        this.location = new GeoLocation(jsonObject.Location);
    }

    /**
     * Gets the standard time.
     * @return {string} The standard time.
     */
    get StdTime() {
        return this.stdTime;
    }

    /**
     * Gets the location object.
     * @return {Location} The location object.
     */
    get Location() {
        return this.location;
    }

    /**
     * Converts the Time object to a plain JavaScript object.
     * @return {Object} The plain JavaScript object representation of the Time object.
     */
    toObject() {
        return {
            StdTime: this.stdTime,
            Location: this.location.toObject(),
        };
    }

    // Get the year from the standard time
    getYear() {
        const stdTime = this.StdTime; // e.g. "13:54 25/10/1992 +08:00"
        const [time, date] = stdTime.split(' ');
        const [hours, minutes] = time.split(':');
        const [day, month, year] = date.split('/');
        const birthDate = new Date(`${year}-${month}-${day}T${hours}:${minutes}:00.000Z`);
        return birthDate.getFullYear();
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
        this.personId = jsonObject.PersonId;

        /**
         * The unique identifier of this Life Event.
         * @type {string}
         */
        this.id = jsonObject.Id;

        /**
         * The name of this Life Event.
         * @type {string}
         */
        this.name = jsonObject.Name;

        /**
         * The start time of this Life Event.
         * @type {Time}
         */
        this.startTime = new Time(jsonObject.StartTime);

        /**
         * A brief description of this Life Event.
         * @type {string}
         */
        this.description = jsonObject.Description;

        /**
         * The nature of this Life Event (e.g. "Good", "Bad", etc.).
         * @type {string}
         */
        this.nature = jsonObject.Nature;

        /**
         * The weight or significance of this Life Event (e.g. "Minor", "Major", etc.).
         * @type {string}
         */
        this.weight = jsonObject.Weight;
    }

    /**
     * Gets the Person ID associated with this Life Event.
     * @return {string} The Person ID.
     */
    get PersonId() {
        return this.personId;
    }

    /**
     * Gets the ID of this Life Event.
     * @return {string} The ID.
     */
    get Id() {
        return this.id;
    }

    /**
     * Gets the name of this Life Event.
     * @return {string} The name.
     */
    get Name() {
        return this.name;
    }

    /**
     * Gets the start time of this Life Event.
     * @return {StartTime} The start time.
     */
    get StartTime() {
        return this.startTime;
    }

    /**
     * Gets the description of this Life Event.
     * @return {string} The description.
     */
    get Description() {
        return this.description;
    }

    /**
     * Gets the nature of this Life Event.
     * @return {string} The nature.
     */
    get Nature() {
        return this.nature;
    }

    /**
     * Gets the weight of this Life Event.
     * @return {string} The weight.
     */
    get Weight() {
        return this.weight;
    }

    /**
     * Converts this Life Event object to a plain JavaScript object.
     * @return {Object} The plain JavaScript object representation of this Life Event.
     */
    toObject() {
        return {
            PersonId: this.personId,
            Id: this.id,
            Name: this.name,
            StartTime: this.startTime.toObject(),
            Description: this.description,
            Nature: this.nature,
            Weight: this.weight,
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
        this.name = jsonObject.Name;

        /**
         * The longitude of this location.
         * @type {number}
         */
        this.longitude = jsonObject.Longitude;

        /**
         * The latitude of this location.
         * @type {number}
         */
        this.latitude = jsonObject.Latitude;
    }

    /**
     * Gets the name of this location.
     * @return {string} The name.
     */
    get Name() {
        return this.name;
    }

    /**
     * Gets the longitude of this location.
     * @return {number} The longitude.
     */
    get Longitude() {
        return this.longitude;
    }

    /**
     * Gets the latitude of this location.
     * @return {number} The latitude.
     */
    get Latitude() {
        return this.latitude;
    }

    /**
     * Converts this Location object to a plain JavaScript object.
     * @return {Object} The plain JavaScript object representation of this Location.
     */
    toObject() {
        return {
            Name: this.name,
            Longitude: this.longitude,
            Latitude: this.latitude,
        };
    }
}



//-------------------------------- VIEW COMPONENTS -----------------------------------

/**
 * Represents a page header component.
 * This class generates the HTML for a page header and handles its initialization.
 */
class PageHeader {
    // Class properties
    ElementID = "";
    TitleText = "Title Goes Here";
    DescriptionText = "Description Goes Here";
    ImageSrc = "images/user-guide-banner.png";

    // Constructor to initialize the PageHeader object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.TitleText = element.getAttribute("title-text") || "Title Goes Here";
        this.DescriptionText = element.getAttribute("description-text") || "Description Goes Here";
        this.ImageSrc = element.getAttribute("image-src") || "images/user-guide-banner.png";

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
      <div class="d-none d-md-block">
        <div class="vstack mb-2">
          <div class="hstack">
            <div class="vstack gap-2">
              <h1 class="fw-bold" tabindex="-1">${this.TitleText}</h1>
              <span style="max-width: 412.5px; font-size: 21px; font-weight: lighter; font-family: inherit;">${this.DescriptionText}</span>
            </div>
            <div class="w-100 d-none d-md-block" style="max-width: 412.5px; text-align: center;">
              <img src="${this.ImageSrc}" style="width: 195px;" class="">
            </div>
          </div>
          <hr class="border-secondary border mt-3">
        </div>
      </div>

      <!-- MOBILE PORTRAIT ONLY -->
      <div class="d-block d-md-none">
        <div class="mt-3 col d-flex align-items-start">
          <div>
            <h3 class="fw-bold mb-0 fs-4 text-body-emphasis">${this.TitleText}</h3>
            <p>${this.DescriptionText}</p>
          </div>
          <img class="bi text-body-secondary flex-shrink-0 mt-3 ms-3" style="width: 141px;" src="${this.ImageSrc}" />
        </div>
      </div>
    `;
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
    TitleText = "Title Goes Here";
    SelectedPersonNameHolderElementID = "selectedPersonNameHolder";
    SearchInputElementClass = "searchInputElementClass";

    constructor(elementId) {
        // Initialize class properties
        this.ElementID = elementId;

        // Default data
        this.personList = [];
        this.publicPersonList = [];
        this.personListDisplay = [];
        this.publicPersonListDisplay = [];

        // Get title and description from the element's custom attributes
        const element = document.getElementById(elementId);
        this.TitleText = element.getAttribute("title-text") || "Title Goes Here";

        // Save a reference to this instance for global access
        this.saveInstanceReference();

        // Initialize the component
        this.init();
    }

    async init() {
        // Fetch person list data from API or local storage
        await this.initializePersonListData();

        // Inject the component's HTML into the page
        await this.initializeMainBody();
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
        // Clean any existing content
        $(`#${this.ElementID}`).empty();

        // Generate and inject the HTML into the page
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());


    }

    // we first check if the person lists are already 
    // cached in the browser's local storage using `localStorage.getItem()`. 
    // If the cached data exists, we parse it and assign it to the respective variables. 
    // If the cache is empty, we proceed to fetch the data from the API as before.After fetching the data from the API, 
    // we cache it in local storage using `localStorage.setItem()` to avoid making 
    // unnecessary API calls on subsequent page loads.
    async initializePersonListData() {
        // Check if person lists are already cached in local storage
        const cachedPersonList = localStorage.getItem('personList');
        const cachedPublicPersonList = localStorage.getItem('publicPersonList');

        if (cachedPersonList && cachedPublicPersonList) {
            // Parse the cached data
            this.personList = JSON.parse(cachedPersonList);
            this.personListDisplay = this.personList;
            this.publicPersonList = JSON.parse(cachedPublicPersonList);
            this.publicPersonListDisplay = this.publicPersonList;

            console.log('Loaded person lists from cache.');
        } else {
            // Fetch private person list from the API
            try {
                const personListResponse = await fetch(`${window.vedastro.ApiDomain}/Calculate/GetPersonList/UserId/${window.vedastro.UserId}`);
                const personListData = await personListResponse.json();
                this.personList = personListData.Payload;
                this.personListDisplay = this.personList;

                // Cache the private person list
                localStorage.setItem('personList', JSON.stringify(this.personList));

                console.log('Loaded private person list from API.');
            } catch (error) {
                console.error('Error fetching private person list:', error);
            }

            // Fetch public person list from the API
            try {
                const publicPersonListResponse = await fetch(`${window.vedastro.ApiDomain}/Calculate/GetPersonList/UserId/101`);
                const publicPersonListData = await publicPersonListResponse.json();
                this.publicPersonList = publicPersonListData.Payload;
                this.publicPersonListDisplay = this.publicPersonList;

                // Cache the public person list
                localStorage.setItem('publicPersonList', JSON.stringify(this.publicPersonList));

                console.log('Loaded public person list from API.');
            } catch (error) {
                console.error('Error fetching public person list:', error);
            }
        }

        // Get the previously selected person from local storage
        const selectedPerson = JSON.parse(localStorage.getItem("selectedPerson"));

        // If a selected person exists, simulate a click on their name
        if (selectedPerson && Object.keys(selectedPerson).length !== 0) {
            this.onClickPersonName(selectedPerson.PersonId); //todo could fail
        }
    }

    // call `PersonSelectorBox.ClearPersonListCache('private')` to clear only the private person list cache,
    // `PersonSelectorBox.ClearPersonListCache('public')` to clear only the public person list cache,
    // or`PersonSelectorBox.ClearPersonListCache('all')` to clear both caches.If an invalid type is provided,
    // a warning will be logged to the console and the cache will not be cleared.
    static ClearPersonListCache(type) {
        switch (type) {
            case 'private':
                localStorage.removeItem('personList');
                break;
            case 'public':
                localStorage.removeItem('publicPersonList');
                break;
            case 'all':
                localStorage.removeItem('personList');
                localStorage.removeItem('publicPersonList');
                break;
            default:
                console.warn('Invalid cache type provided. Cache not cleared.');
        }

        console.log('Person list cache cleared.');
    }

    // Handle click on a person's name in the dropdown
    async onClickPersonName(personId) {
        // Get the full person details based on the ID
        var personData = this.getPersonDataById(personId);
        var displayName = this.getPersonDisplayName(personData);

        // Update the visible select button text
        var buttonTextHolder = $(`#${this.ElementID}`).find(`.${this.SelectedPersonNameHolderElementID}`);
        buttonTextHolder.html(displayName);

        // Save the selected person to local storage
        var partialPerson = { PersonId: personId };
        localStorage.setItem("selectedPerson", JSON.stringify(partialPerson));

        // Save the selected person ID for instance-specific selection
        this.selectedPersonId = personId;
    }

    // Get the display name with birth year for a person
    getPersonDisplayName(person) {
        const personInstance = new Person(person);
        return `${personInstance.Name} - ${personInstance.BirthTime.getYear()}`;
    }

    // Handle keyup event on the search input field
    onKeyUpSearchBar = (event) => {
        // Ignore certain keys to prevent unnecessary filtering
        if (["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Space", "ControlLeft", "ControlRight", "AltLeft", "AltRight", "ShiftLeft", "ShiftRight", "Enter", "Tab", "Escape"].includes(event.code)) {
            return;
        }

        // Get the search text from the input field
        const searchText = event.target.value.toLowerCase();

        // Filter the person lists based on the search text
        var allPersonDropItems = $(`#${this.ElementID}`).find('.dropdown-menu li');
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
        this.personListHTML = this.generatePersonListHtml();
        this.publicPersonListHTML = this.generatePublicPersonListHtml();

        // Get a reference to the search input element
        this.searchInput = document.getElementById('searchInput');

        // Get previously selected person's name if available
        const selectedPerson = JSON.parse(localStorage.getItem("selectedPerson"));
        let selectedPersonText = 'Select Person';
        if (selectedPerson && Object.keys(selectedPerson).length !== 0) {
            const personData = this.getPersonDataById(selectedPerson.PersonId);
            selectedPersonText = this.getPersonDisplayName(personData);
        }

        // Return the generated HTML for the component
        return `
    <div>
      <label class="form-label">${this.TitleText}</label>
      <div class="hstack">
        <div class="btn-group" style="width:100%;">
          <button onclick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickDropDown(event)" type="button" class="btn dropdown-toggle btn-outline-primary text-start" data-bs-toggle="dropdown" aria-expanded="false">
            <div class="${this.SelectedPersonNameHolderElementID}" style="cursor: pointer;white-space: nowrap; display: inline-table;" >${selectedPersonText}</div>
          </button>
          <ul class="dropdown-menu ps-2 pe-3" style="height: 412.5px; overflow: clip scroll;">

            <!-- SEARCH INPUT -->
            <div class="hstack gap-2">
              <input onkeyup="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onKeyUpSearchBar(event)" type="text" class="${this.SearchInputElementClass} form-control ms-0 mb-2 ps-3" placeholder="Search...">
              <div class="mb-2" style="cursor: pointer;">
                <i class="iconify" data-icon="pepicons-pop:list" data-width="25"></i>
              </div>
            </div>

            <!-- PRIVATE PERSON LIST -->
            ${this.personListHTML}

            <!-- DEVIDER & EXAMPLES ICON -->
            <li><hr class="dropdown-divider"></li>
            <div class="ms-3 d-flex justify-content-between">
              <div class=" hstack gap-2" style=" ">
                <div><i class="iconify" data-icon="material-symbols:demography-rounded" data-width="25"></i></div>
                <span style="font-size: 13px; color: rgb(143, 143, 143);>Examples</span>
              </div>
            </div>
            <li><hr class="dropdown-divider"></li>

            <!-- PUBLIC PERSON LIST -->
            ${this.publicPersonListHTML}

          </ul>
        </div>
        <button onClick="showSection('AddPerson')" style="height:37.1px; width: fit-content; font-family: 'Lexend Deca', serif !important;" class="iconOnlyButton btn-primary btn ms-2">
          <i class="iconify" data-icon="ant-design:user-add-outlined" data-width="25"></i>
        </button>
      </div>
    </div>
  `;
    }


    // Get full person data from the given list based on ID
    getPersonDataById(personId) {
        // Search in public list first
        const person = this.publicPersonListDisplay.find((person) => person.PersonId === personId);

        // If not found, search in private list
        if (!person) {
            const privatePerson = this.personListDisplay.find((person) => person.PersonId === personId);
            return new Person(privatePerson); // Create a Person instance
        }

        return new Person(person); // Create a Person instance
    }

    // Generate HTML for the public person list
    generatePublicPersonListHtml() {
        const html = this.publicPersonListDisplay
            .map((person) => {
                return `<li onClick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')" class="dropdown-item" style="cursor: pointer;">${this.getPersonDisplayName(person)}</li>`;
            })
            .join("");

        return html;
    }

    // Generate HTML for the private person list
    generatePersonListHtml() {
        const html = this.personListDisplay
            .map((person) => {
                return `<li onClick="window.vedastro.PersonSelectorBoxInstances['${this.ElementID}'].onClickPersonName('${person.PersonId}')" class="dropdown-item" style="cursor: pointer;">${this.getPersonDisplayName(person)}</li>`;
            })
            .join("");

        return html;
    }

    // Handle click on the dropdown button
    onClickDropDown(event) {
        // Set focus to the search text box for instant input
        $(`#${this.ElementID}`).find(`.${this.SearchInputElementClass}`).focus();
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

    // Constructor to initialize the PageHeader object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.Title = element.getAttribute("title") || "Title Goes Here";
        this.Description = element.getAttribute("description") || "Description Goes Here";
        this.IconName = element.getAttribute("iconname") || "fluent-emoji:robot";

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

    // Handle keyup event on the search input field
    static onClick(event) {
        console.log(event);
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        // Return the HTML for the page header, including conditional blocks for different screen sizes
        return `
      
<div onClick="window.vedastro.InfoBox.onClick(event)" class="" style="cursor: pointer; max-width:365px;">
    <div class="alert alert-primary d-flex align-items-center vstack p-2" role="alert" style="">
        <div class="hstack mb-2">
            <span class="iconify bi flex-shrink-0 me-2" data-icon="${this.IconName}" data-width="50"></span>
            <div style="font-family: 'Gowun Dodum', serif; line-break: auto;">
                <strong>${this.Title}</strong><br />
                ${this.Description}
            </div>
        </div>
    </div>
</div>

    `;
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
    Color = "";
    IconName = "";
    ButtonText = "";
    OnClickCallback = null;

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
      <button onclick="${this.OnClickCallback}" style="height:37.1px; width: fit-content; font-family: 'Lexend Deca', serif !important;" class="btn-sm hstack gap-2 iconButton btn-${this.Color} btn">
        <i class="iconify" data-icon="${this.IconName}" data-width="25"></i>
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

        // Initialize the TimeLocationInputInstances object
        TimeInputSimple.initInstances();

        // Save a reference to this instance for global access
        this.saveInstanceReference();

        // Call the method to initialize the time location input
        this.initializeTimeLocationInput();
    }

    // Method to initialize the time location input
    async initializeTimeLocationInput() {
        // Get the element with the given ID
        const element = document.getElementById(this.ElementID);

        // Get the label text from the element's attribute
        const labelText = element.getAttribute("LabelText");

        // Generate the HTML for the time location input and inject it into the element
        element.innerHTML = await this.generateTimeLocationInputHtml(labelText);
    }

    // Method to generate the HTML for the time location input
    async generateTimeLocationInputHtml(labelText) {
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

      <span class="input-group-text gap-2 py-1"><i class="iconify" data-icon="noto-v1:timer-clock" data-width="30"></i>${labelText}</span>
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

    //if click is outside picker & input then hide it
    autoHidePicker(e) {
        //check if click was outside input
        const pickerHolder = e.target.closest(`#${this.CalendarPickerHolderID}`);
        const timeInput = e.target.closest(`#${this.TimeInputHolderID}`);

        //if click is not on either inputs then hide picker
        if (!(timeInput || pickerHolder)) {
            document.getElementById(this.CalendarPickerHolderID).classList.add('visually-hidden');
        }
    }

    isValid() {
        // Check if all fields have been filled
        return this.hour !== "" && this.minute !== "" && this.meridian !== "" && this.date !== "" && this.month !== "" && this.year !== "";
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
    LabelText = "";
    dropdownMenuId = "";
    locationNameInputId = "";
    locations = []; // Save the parsed geolocation array in the class instance

    /**
     * Constructor to initialize the GeoLocationInput object.
     * @param {string} elementId - The ID of the HTML element to render the component in.
     */
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.LabelText = element.getAttribute("LabelText") || "Location";

        // Generate a random ID for the dropdown menu
        this.dropdownMenuId = `dropdown-menu-${Math.random().toString(36).substr(2, 9)}`;
        this.locationNameInputId = `location-name-input-${Math.random().toString(36).substr(2, 9)}`;

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();

        // Save a reference to this instance for global access
        GeoLocationInput.initInstances();
        window.vedastro.GeoLocationInputInstances[elementId] = this;
    }

    /**
     * Initialize the GeoLocationInput instances object.
     */
    static initInstances() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.GeoLocationInputInstances) {
            window.vedastro.GeoLocationInputInstances = {};
        }
    }

    /**
     * Method to initialize the main body of the page header.
     */
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());

        // Add event listener to the switch button
        $(`#${this.ElementID} .switch-button`).on('click', () => {
            this.toggleInputFields();
        });
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
                    <span class="input-group-text gap-2 py-1"><i class="iconify" data-icon="streamline-emojis:globe-showing-americas" data-width="34"></i>${this.LabelText}</span>
                    <input id="${this.locationNameInputId}" onkeyup="window.vedastro.GeoLocationInputInstances['${this.ElementID}'].onUpdateLocationNameText(event)" type="text" class="form-control " placeholder="New York" style="font-weight: 600; font-size: 16px;" data-bs-toggle="dropdown">
                    <ul id="${this.dropdownMenuId}" class="dropdown-menu" aria-labelledby="${this.locationNameInputId}">
                        <li><a class="dropdown-item text-muted" href="#">
                            <i class="iconify" data-icon="grommet-icons:map" data-width="18"></i>
                            Search city, town, state</a>
                        </li>
                        <!-- DYNAMICLY GENERATED -->
                    </ul>
                </div>

                <!-- Latitude & long input -->
                <div class="input-group d-none lat-lng-fields">
                    <!-- HEADER ICON -->
                    <span class="input-group-text gap-2 py-1"><i class="iconify" data-icon="streamline-emojis:globe-showing-americas" data-width="34"></i>${this.LabelText}</span>
                    <span class="input-group-text px-2">Lat</span>
                    <input type="number" class="form-control px-2 latitude" placeholder="4.3°" style="font-weight: 600; font-size: 16px;">
                    <span class="input-group-text px-2">Long</span>
                    <input type="number" class="form-control px-2 longitude" placeholder="101.4°" style="font-weight: 600; font-size: 16px;">
                </div>

                <!-- Input Swither button -->
                <button class="switch-button btn-primary btn p-2" style="font-family: 'Lexend Deca', serif !important;">
                    <i class="iconify globeIcon" data-icon="bx:globe" data-width="25"></i>
                    <i class="iconify mapIcon d-none" data-icon="bx:map" data-width="25"></i>
                </button>
            </div>
        `;
    }

    /**
     * Method to handle click on preset location name.
     * @param {Event} eventObject - The event object triggered by the click.
     */
    onClickPresetLocationName(eventObject) {
        // Get the location name from the clicked element
        const locationName = eventObject.target.textContent;

        // Find the corresponding location object from the saved instance
        const location = this.locations.find(location => location.Name === locationName);

        if (location) {
            // Save the selected location JSON for this instance
            this.selectedLocation = location;

            // Fill location name, longitude and latitude values into HTML
            document.querySelector(`#${this.ElementID} .location-name input`).value = location.Name;
            document.querySelector(`#${this.ElementID} .latitude`).value = location.Latitude.toFixed(1); //round for nice fit GUI
            document.querySelector(`#${this.ElementID} .longitude`).value = location.Longitude.toFixed(1); //round for nice fit GUI
        }
    }

    /**
     * Method to update the location name text.
     * @param {Event} eventObject - The event object triggered by the input field.
     */
    onUpdateLocationNameText(eventObject) {
        // Get the user input from the event target
        const userTextInput = eventObject.target.value;

        // Call the API to search for location names based on the user input
        this.locationNameSearchWithAPI(userTextInput)
            .then(locations => {
                // Get the parent element of the input field
                const inputParent = eventObject.target.closest(`#${this.ElementID}`);

                // Get the dropdown menu element within the parent element
                const dropdownMenu = inputParent.querySelector(`#${this.dropdownMenuId}`);

                // Clear any existing content in the dropdown menu
                dropdownMenu.innerHTML = "";

                if (locations.length === 0) {
                    // if no locations are found then only insert below html to notify user no location with that name,
                    // but input must have text, else show `search message` instead
                    if (userTextInput.trim() !== "") {
                        dropdownMenu.innerHTML = `
                        <li><a class="dropdown-item text-muted" href="#">
                            <i class="iconify" data-icon="tdesign:map-cancel" data-width="18"></i>
                            Not found, try input coordinates</a>
                        </li>
                    `;
                    } else {
                        dropdownMenu.innerHTML = `
                        <li><a class="dropdown-item text-muted" href="#">
                            <i class="iconify" data-icon="grommet-icons:map" data-width="18"></i>
                            Search city, town, state</a>
                        </li>
                    `;
                    }
                } else {
                    // Generate HTML for the locations
                    const locationsHtml = locations.map(location => `
                    <li><a class="dropdown-item" onClick="window.vedastro.GeoLocationInputInstances['${this.ElementID}'].onClickPresetLocationName(event)">${location.Name}</a></li>
                `).join("");
                    dropdownMenu.innerHTML = locationsHtml;
                }

                // Show the dropdown menu
                dropdownMenu.classList.remove("d-none");
            });
    }

    /**
     * Method to toggle the input fields.
     */
    toggleInputFields() {
        $(`#${this.ElementID} .location-name`).toggleClass('d-none');
        $(`#${this.ElementID} .lat-lng-fields`).toggleClass('d-none');

        // toggle button icons based on class
        const switchButton = $(`#${this.ElementID} .switch-button`);
        if (switchButton.find('.globeIcon').hasClass('d-none')) {
            switchButton.find('.mapIcon').addClass('d-none');
            switchButton.find('.globeIcon').removeClass('d-none');
        } else {
            switchButton.find('.globeIcon').addClass('d-none');
            switchButton.find('.mapIcon').removeClass('d-none');
        }
    }

    /**
     * Method to search for location names using an API.
     * @param {string} locationName - The location name to search for.
     */
    locationNameSearchWithAPI(locationName) {
        return new Promise(resolve => {
            fetch(`${window.vedastro.ApiDomain}/Calculate/SearchLocation/Address/${locationName}`)
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
        // Check if all fields have been filled
        return this.locationName !== "" && this.latitude !== "" && this.longitude !== "";
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

    static initInstances() {
        if (!window.vedastro) {
            window.vedastro = {};
        }
        if (!window.vedastro.TimeLocationInputInstances) {
            window.vedastro.TimeLocationInputInstances = {};
        }
    }

    // Constructor to initialize the object
    constructor(elementId) {
        // Assign the provided elementId to the ElementID property
        this.ElementID = elementId;

        // Get the DOM element with the given ID
        const element = document.getElementById(elementId);

        // Get the custom attributes from the element and assign default values if not present
        this.LabelText = element.getAttribute("LabelText") || "Label Goes Here";

        // Generate a random ID for TimeInputSimple
        var randoTron = Math.random().toString(36).substr(2, 9);
        this.TimeInputSimpleID = `TimeInputSimpleID-${randoTron}`;
        this.GeoLocationInputID = `GeoLocationInputID-${randoTron}`;

        // Call the method to initialize the main body of the page header
        this.initializeMainBody();

        // Save a reference to this instance for global access
        TimeLocationInput.initInstances();
        window.vedastro.TimeLocationInputInstances[elementId] = this;

    }

    // Method to initialize the main body of the page header
    async initializeMainBody() {
        // Empty the content of the element with the given ID
        $(`#${this.ElementID}`).empty();

        // Generate the HTML for the page header and inject it into the element
        $(`#${this.ElementID}`).html(await this.generateHtmlBody());

        // render subview components via code now that sub view base HTML is in DOM
        this.TimeInputSimpleInstance = new TimeInputSimple(this.TimeInputSimpleID);
        this.GeoLocationInputInstance = new GeoLocationInput(this.GeoLocationInputID);
    }

    // Method to generate the HTML for the page header
    async generateHtmlBody() {
        return `
      <div id="${this.TimeInputSimpleID}" LabelText="${this.LabelText}"></div>
      <div id="${this.GeoLocationInputID}" class="mt-3" LabelText="${this.LabelText}"></div>
    `;
    }

    // Method to get the time and location as a JSON object
    // exp out : {"StdTime":"13:54 25/10/1992 +08:00","Location":{"Name":"Taiping","Longitude":103.82,"Latitude":1.352}}
    getTimeJson() {
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

        // Construct the StdTime string in the format "HH:MM DD/MM/YYYY +08:00"
        const stdTime = `${hour}:${minute} ${date}/${month}/${year} +08:00`;

        // Construct the Location object with Name, Longitude, and Latitude properties
        const location = {
            Name: locationName,
            Longitude: longitude,
            Latitude: latitude
        };

        // Construct the timeObject with StdTime and Location properties
        const timeObject = {
            StdTime: stdTime,
            Location: location
        };

        // Return the timeObject
        return timeObject;
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

   
}




