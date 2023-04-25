//called by nav items to add active class to them self and remove nav pill from the previous
function NavMenuAutoActiveStyle(clickedNav) {
    //remove highlight from other
    $(clickedNav).parent('li').siblings().children().removeClass('active');

    //give highlight to clicked nav button
    $(clickedNav).addClass('active');
}


//Generates a table using Tabulator table library
//id to where table will be generated needs to be inputed
 function generatePersonListTable(tableId, tableData) {
    //set table data
    var table = new Tabulator(`#${tableId}`, {
        data: tableData,           //load row data from array
        //editable: false,
        layout: "fitColumns",      //fit columns to width of table
        responsiveLayout: "hide",  //hide columns that don't fit on the table
        //tooltips: true,            //show tool tips on cells
        addRowPos: "top",          //when adding a new row, add it to the top of the table
        history: false,             //allow undo and redo actions on the table
        pagination: "local",       //paginate the data
        paginationSize: 50,         //allow 7 rows per page of data
        paginationCounter: "rows", //display count of paginated rows in footer
        movableColumns: false,      //allow column order to be changed
        resizableRows: true,       //allow row order to be changed
        initialSort: [             //set the initial sort order of the data
            { column: "name", dir: "asc" },
        ],
        columns: [                 //define the table columns
            { title: "Name", field: "name", hozAlign: "center" },
            { title: "Gender", field: "genderString", hozAlign: "center" },
            { title: "Birth", field: "birthTimeString", hozAlign: "center" },
            { title: "Id", field: "id", hozAlign: "center" },
        ],
    });

    //handler when table row is clicked
    table.on("rowClick", function (e, row) {
        //get person name
        let personId = row._row.data.hash;
        //send user to person editor page with clicked person
        window.location.href = `/Account/Person/Editor/${personId}`;
    });

    //same as click handler but for touch
    table.on("rowTap", function (e, row) {
        //get person name
        let personId = row._row.data.hash;
        //send user to person editor page with clicked person
        window.location.href = `/Account/Person/Editor/${personId}`;
    });
}


//uses html2pdf.js to convert any html to PDF
//will prompt user to save file
//don't include .pdf in file name
//function htmlToPdf(elementKey, newPdfFileName) {
//    //find the element
//    var element = $(elementKey)[0];

//    var opt = {
//        filename: `${newPdfFileName}.pdf`
//    };

//    html2pdf(element, opt);
//}




//function setTitleWrapper(newTitle) {
//    console.log(`JS: setTitleWrapper`);
//    //this has to be done like this for it to work
//    $(function () {
//        $(document).attr("title", newTitle);
//    });
//};

//var setTitleWrapper = (title) => { document.title = title; };

//async sleep millisecond
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

//async delay for specified time in ms
//example: await delay(1000);
function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}

//Gets a mouses x axis relative inside the given element
//used to get mouse location on Dasa view
//returns 0 when mouse is out
function GetMousePositionInElement(mouseEventData, elementId) {
    //gets the measurements of the dasa view holder
    //the element where cursor line will be moving
    //TODO read val from global var
    let holderMeasurements = $(elementId)[0].getBoundingClientRect();

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

//TODO MARKED FOR DELETION ONCE SVG VERSION IMPLEMENTED
//gets all life event lines by class
//and attaches tooltip event on it to
//show data of the event on mouse hover
//uses Tippy js lib, needs to be called everytime
//new elements are created, because attach by direct
//element reference not class
//function InitLifeEventLineToolTip() {
//    $(".LifeEventLines").each(function () {
//        var evName = this.getAttribute("eventname");

//        tippy(this, {
//            content: evName,
//            placement: 'bottom',
//            arrow: true
//        });

//    });

//}



//█▀█ █▀█ █▀█ █▀▀ █▀█ █▀▀ █▀ █▀   █▄▄ ▄▀█ █▀█
//█▀▀ █▀▄ █▄█ █▄█ █▀▄ ██▄ ▄█ ▄█   █▄█ █▀█ █▀▄

function InitProgressBar(ms) {
    window.ProgressBarTempValue = 0;

    window.ProgressBarInstance = new ProgressBar.Line('#progressBar', {
        strokeWidth: 4,
        easing: 'linear',
        duration: 1400,
        color: '#2dd128',
        trailColor: '#eee',
        trailWidth: 1,
        svgStyle: { width: '100%', height: '100%' },
        text: {
            style: {
                // Text color.
                // Default: same as stroke color (options.color)
                color: '#999',
                position: 'absolute',
                right: '0',
                top: '30px',
                padding: 0,
                margin: 0,
                transform: null
            },
            autoStyleContainer: false
        },
        from: { color: '#FFEA82' },
        to: { color: '#ED6A5A' },
        step: (state, bar) => {
            bar.setText(Math.round(bar.value() * 100) + ' %');
        }
    });
}

//Adds input value to current progress bar
function AddToProgressBar(percentage) {
    var value = percentage / 100; //convert 50 to 0.5
    window.ProgressBarTempValue += value;

    //if above 100 end here & reset to 0
    if (window.ProgressBarTempValue > 1) {
        window.ProgressBarTempValue = 0;
        return;
    }

    ProgressBarInstance.animate(window.ProgressBarTempValue);
}

//Adds input value to current progress bar
function SetProgressBar(percentage) {
    var value = percentage / 100; //convert 50 to 0.5
    window.ProgressBarTempValue = value;
    ProgressBarInstance.animate(window.ProgressBarTempValue);
}

//auto updates progress bar till 100%
async function ProgressBarSlowAutoUpdate() {
    for (let percent = 0; percent < 100; percent++) {
        //set 0
        var progressVal = percent / 100; //convert exp: 50 to 0.5
        ProgressBarInstance.animate(progressVal);
        await delay(200);
    }
}

function ResetProgressBar() {
    //reset both view & data
    window.ProgressBarTempValue = 0;
    ProgressBarInstance.animate(0);
}

//Gets current value of progress bar
function GetProgressBarValue() {
    return window.ProgressBarTempValue;
}


//---------------------

//element passed in is the element where touch will be detected
function InitTouchLib(element) {
    //var myElement = document.getElementById('myElement')

    // create a simple instance
    // by default, it only adds horizontal recognizers
    window.hammerJs = new Hammer(element, { touchAction: 'auto' });

    // listen to events...
    window.hammerJs.on("panleft panright tap", function (ev) {
        //when dragging the dasa report, this will stop
        //from detecting as out of element
        if (ev.center.x == 0) { return; }

        //converting the touch event into a mouse event
        var mouse = {
            clientX: ev.center.x,
            clientY: ev.center.y,
            path: ev.srcEvent.path
        };

        //and calling the event handlers
        //that a mouse would normally fire
        // alert("todo touch not implemented!!!");
        autoMoveCursorLine(mouse);
        //todo call to this method is out dated
        autoUpdateTimeLegend(mouse);
    });

    //window.hammerJs = new Hammer(myElement, myOptions);

    //window.hammerJs.on('pan', function (ev) {
    //    console.log(ev);
    //});
}

