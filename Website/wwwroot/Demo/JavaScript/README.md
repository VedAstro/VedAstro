## Easy to use JS library help your app or website to talk with VedAstro API

## 🛜 LIVE DEMO SITE --> https://vedastro.org/Demo/JavaScript/index.html

## 🧩 JS CDN --> https://vedastro.org/js/VedAstro.js

## 🏎️ Quick Start

HTML
```html
<html>
    <head>
        <title>Astro Table Demo</title>
        <script src="https://cdn.jsdelivr.net/npm/jquery/dist/jquery.min.js"></script>
        <script src="https://vedastro.org/js/VedAstro.js"></script>
    </head>
    <body>
        <div id="PlanetDataTable"></div>
    </body>
</html>
```

JS
```javascript
//define the columns names and the underling API call
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
![image](https://github.com/VedAstro/VedAstro/assets/43817262/419954b6-43ec-4a5b-b106-0c8f303b5a9d)
![image](https://github.com/VedAstro/VedAstro/assets/43817262/cb0da81b-6ee8-44e1-a635-9e83a3912947)
![image](https://github.com/VedAstro/VedAstro/assets/43817262/6c33a27a-83cb-4bf8-a346-35090daea106)



