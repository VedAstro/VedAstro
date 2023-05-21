/*SIMPLE WRAPPER LIB FOR ACCESSING VEDASTRO LIB*/

import { URLS } from '/js/URLS.js';
import { EventsChart, ID } from '/js/EventsChart.js';


export class VedAstro {

    constructor(apiKey) {
        //api key used when making calls
        this.APIKey = apiKey;

        this.URLS = new URLS();

        return this;
    }

    //special URL to access ready made chart by async
    async ChartFromGenerateDataXML(chartUrl) {

        //get raw chart from API
        var chartStr = await getEventsChartFromApiXml(chartUrl);

        //inject into default div on page to hold, "EventsChartSvgHolder"
        var $chartElm = injectIntoElement($(ID.EventsChartSvgHolder)[0], chartStr);

        //things done here:
        //- get the unique ID of the chart
        //- use ID to maintain clean code
        //- chart is available in window.EventsChartList
        var index = new EventsChart($chartElm.attr('id'));


        //-----------------------------LOCAL FUNCS---------------------------------------

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

}