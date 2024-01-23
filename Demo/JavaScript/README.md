## Easy to use JS library help your app or website to talk with VedAstro API

## 🛜 JS CDN -> https://vedastro.org/js/VedAstro.js

## 🏎️ Quick Start

HTML
```html
<html>
    <head>
        <title>Astro Table Demo</title>
    </head>
    <body>
        <div id="PlanetDataTable"></div>
    </body>
</html>
```

JS
```javascript
//define the columns
var planetColumns = [
    { Api: "PlanetZodiacSign", Enabled: true, Name: "Sign" },
    { Api: "PlanetConstellation", Enabled: true, Name: "Star" },
    { Api: "PlanetLordOfZodiacSign", Enabled: true, Name: "Sign Lord" },
    { Api: "PlanetLordOfConstellation", Enabled: true, Name: "Star Lord" },
    { Api: "PlanetSubLordKP", Enabled: true, Name: "Sub Lord" },
    { Api: "Empty", Enabled: false, Name: "Empty" },
];

//initialize astro table preferences
var settings = {
    ElementID : "PlanetDataTable",
    KeyColumn : "Planet", //can be "House"
    ShowHeader : true,
    HeaderIcon : "twemoji:ringed-planet",
    ColumnData : planetColumns, //columns names to create table
    EnableSorting : false,
    SaveSettings : false, //save to browser storage
};

//make new astro table
var planetDataTable = new AstroTable(settings);

//done, easy!
```

## 🚀 What's Possible?
