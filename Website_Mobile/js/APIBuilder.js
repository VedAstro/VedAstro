updateHistory();

new PageHeader("PageHeader");

const apiMethodData = {
          "Signature": "DasaAtRange(TimebirthTime,TimestartTime,TimeendTime,intlevels,intprecisionHours)",
          "Description": " Calculates dasa for a specific time frame ",
          "ExampleOutput": "",
          "SelectedParams": {},
          "MethodInfo": {
              "Name": "DasaAtRange",
              "DeclaringType": "VedAstro.Library.Calculate",
              "Parameters": [
                  {
                      "Name": "birthTime",
                      "DefaultValue": "",
                      "IsOptional": false,
                      "Description": "",
                      "ParameterType": "VedAstro.Library.Time"
                  },
                  {
                      "Name": "startTime",
                      "DefaultValue": "",
                      "IsOptional": false,
                      "Description": "start of time range to show dasa",
                      "ParameterType": "VedAstro.Library.Time"
                  },
                  {
                      "Name": "endTime",
                      "DefaultValue": "",
                      "IsOptional": false,
                      "Description": "end of time range to show dasa",
                      "ParameterType": "VedAstro.Library.Time"
                  },
                  {
                      "Name": "levels",
                      "DefaultValue": "3",
                      "IsOptional": true,
                      "Description": "range 1 to 7coresponds to bhukti antaram etc... lower is faster",
                      "ParameterType": "System.Int32"
                  },
                  {
                      "Name": "precisionHours",
                      "DefaultValue": "100",
                      "IsOptional": true,
                      "Description": "defaults to 21 days higher is faster set how accurately the start end time of each event is calculated exp setting 1 hour means given in a time range of 100 years it will be checked 876600 times",
                      "ParameterType": "System.Int32"
                  }
              ]
          }
      };


var apiMethodViewer = new ApiMethodViewer("ApiMethodViewer", apiMethodData);

//var horoscopePersonSelector = new PersonSelectorBox("PersonSelectorBox");
//var ayanamsaSelector = new AyanamsaSelectorBox("AyanamsaSelectorBox", "RAMAN");


//new IconButton("IconButton_Calculate_Horoscope");
//new IconButton("IconButton_Advanced_Horoscope");

//new InfoBox("InfoBox_AskAI_Horoscope");
//new InfoBox("InfoBox_EasyImport_Horoscope");
//new InfoBox("InfoBox_ForgotenTime_Horoscope");


//----------------------------------- FUNCS ------------------------------------------


