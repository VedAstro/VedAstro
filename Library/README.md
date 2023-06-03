
<a href="https://www.vedastro.org">
<img
  src="https://www.vedastro.org/images/website-header-screenshot.png"
  alt="VedAstro.org"
  title="VedAstro.org">
</a>


<h3 align="center">
  
  ![GitHub last commit](https://img.shields.io/github/last-commit/gen-so/Genso.Astrology)
  ![GitHub commit activity](https://img.shields.io/github/commit-activity/m/gen-so/Genso.Astrology)
  [![Stars](https://img.shields.io/github/stars/gen-so/Genso.Astrology?color=brightgreen)](https://github.com/gen-so/Genso.Astrology/stargazers)
  [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)

</h3>


<h2 align="center"><a href="https://www.vedastro.org">VedAstro</a></h2>
<h4 align="center">
  Astrology enables one to see subtle phenomena across time.</br>
       Which to the mortal man is invisible in action,
       and only visible in its reaction.</h4>

# Links
- [Quick Guide](https://vedastro.org/Docs/QuickGuide) learn how to use
- [Open API](https://vedastro.org/Docs/API) easily get astrology data for your app or website
- [Swiss Ephemris API](https://vedastro.org/Docs/API) easily get astrology data for your app or website
- [Calculators](https://vedastro.org/Calculator) each calculator shows an aspect of vedic astrology
- [Docker](https://github.com/orgs/VedAstro/discussions/8) supported for easy deployement anywhere
- [Contact](https://www.vedastro.org/Contact) us and we'll get back to you ASAP.
- [Donate](https://www.vedastro.org/Donate) to keep this project going
- [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/S6S0ELZGR)

<!--

# Examples of Accurate Predictions
The Dasa Life Calculator is the most advanced and amazingly accurate prediction tool we've ever seen.
Take a look at this example charts of Napoleon Bonaparte and Queen Elizabeth II. Every major event in their life
can be clearly seen predicted by the chart in the "Summary Row". Red for a bad event and green for a good life event.
Using a combination of Dasa, Bhukti, Antaram and Gocharam of 9 planets this complex chart can give perfect predictions!
The chart takes alot of computational power to generate, so please be patient after you click the Calculate button.

[![](GithubImages/napoleon-dasa-chart.png)](https://www.vedastro.org)
[![](GithubImages/elizabeth-dasa-chart.png)](https://www.vedastro.org)
 -->


# About : reason and goal for this project
Anybody who has studied Vedic Astrology knows well how accurate it can be.
But also how complex it can get to make accurate predictions.
It takes **decades of experience** to be able make accurate prediction.
As such this knowledge only reaches a limited people.
This project is an effort to change that.
</br>

Our goal is to make Vedic Astrology easily accessible to anybody.
So that people can use it in their daily lives for their benefit.
Vedic Astrology in Sanskrit mean "**Light**". And that is exactly what it is.
It lights our future so we can change it.
And it lights our past, to understand our mistakes.
</br>

By using modern computational technologies &amp; methods we can simplify the
complexity of Vedic Astrology. For example, calculating planet strength (Bhava Bala)
used to take hours, now with computers we can calculate it in milliseconds.
And using databases &amp; innovative programming methods, there is no need to remember
thousands of planetary combinations, allowing you to make accurate predictions
with little to no knowledge.
    
# 8 Years Old
The first line of code for this project was written in 2014.
Back then it was a simple desktop software, with no UI and only text display.
With continued support from investors &amp; users, this project has steadily developed to what it is today.
Helping people from all over the world.
    
# Credits &amp; Reference
<img
  src="https://www.vedastro.org/images/bv-raman-pic.jpg"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto; max-width: 300px">
<img
  src="https://www.vedastro.org/images/b-suryanarain-rao-pic.jpg"
  alt="Alt text"
  title="Optional title"
  style="display: inline-block; margin: 0 auto; max-width: 300px">
  </br>
Thanks to [B.V. Raman](https://en.wikipedia.org/wiki/B._V._Raman) and his grandfather B. Suryanarain Rao. Information from their books is the source
material & inspiration for this project. Please buy their books to show your support.
Astronomical calculation was made possible by "SwissEphNet" &amp; "SWISS EPHEMERIS".
Last but not least, we thank users like you who keep this project going.
<!--
# Help us and get paid!
Complete tasks for this project and get paid instantly! Each task has a different value based on complexity.
To get started fork the repo, make your edits and submit pull request. Contact us for task details.

Task | Value | Status
--- | --- | ---
Improve API chart generator performance by 2x | $10 | ![](https://img.shields.io/static/v1?label=&message=Open&color=blue)
Show name of planet state in Horoscope page (combust, exalted, debilited) | $2 | ![](https://img.shields.io/static/v1?label=&message=Open&color=blue)
Generate traditional Rasi, Navamsa South/North style chart | $5 | ![](https://img.shields.io/static/v1?label=&message=Open&color=blue)
Find a spelling mistake in VedAstro.org content | $1 | ![](https://img.shields.io/static/v1?label=&message=Open&color=blue)
Show name of planet motion in Horoscope page (retrograde, direct, stationary) | $1.5 | ![](https://img.shields.io/static/v1?label=&message=Done&color=green)
 -->

 # Unexpected errors?
- Clear browser cache & reload webapp
- Switch to Beta version

# Project Architecture : For Coders
Key design notes to understand the internals of the program better.


## Core Function Explained
The main part of the program is the prediction/event generator.
It works by combining **logic** on how to calculate a prediction with **data** about that prediction.
This is done everytime a "Calculate" button is clicked. Below you will see a brief explanation of this process.
This method was choosen to easily accommodate the thousands of astrological calculation possibilities.

## event prediction = (data + logic) + time

```

            CREATION OF AN EVENT/PREDICTION

STEP 1

Hard coded event data like name is stored in XML file.
A copy of the event name is stored as Enum to link
Calculator Methods with data from XML.
These static methods are the logic to check
if an event occured. No astro calculation done at this stage.
This is the linking process of the logic and data.

                      -------+
                             |
     +-----------------+     |
     | Event Data (xml)|     |
     +-----------------+     |
              +              |
     +------------------+    |
     |Event Names (Enum)|    +-----> Event Data (Instance)
     +------------------+    |
              +              |
     +------------------+    |
     |Calculator Methods|    |
     +------------------+    |
                             |
                       ------+


STEP 2

From the above step, list of Event Data is generated.
Is occuring logic of each Event Data is called with time slices,
generated from a start time & end time (inputed at runtime).
An Event is created if IsOccuring is true.
This's a merger of Time and EventData to create an
Event at a specific time. This Event is then used
throughout the progam.



              Event Data    +    Time Range
                 List               List
                            |
                            |
                            |
                            v

                        Event List


```

## Website
```
+--------+          +------------------------+                +------------------+
|  User  | <------+ |        Website         | -------------> |        API       |
|        | +------> | - Blazor WebAssembly   | <------------- | -Azure Functions |
+--------+   GUI    | - Azure Static WebApp  |      XML       |                  |
                    |                        |                |                  |
                    +------------------------+                +------------------+
```

# Code Edit Guide
### To add a new prediction/event
1. Create a method in EventCalculatorMethods.cs
2. Add the name in EventNames.cs
3. Add the prediction/event details HoroscopeDataList.xml

### To add a new Event Tag
1. Edit in VedAstro.Library EventTag enum. Change here reflects even in GUI


# Design Decision Notes
These are randomly ordered notes on why a feature was implemented in a certain way.<br/>
Will prove usefull when debugging & upgrading code.

## Handling SEO bots via Rules
Shows only clean & nice html index for bots from best known SEs

## Stop 404 error
for direct access Blazor page via static storage without 404 error
since no page acctually exists at page url, blazor takes url and runs the page as app
using rule engine this is possible
rules also make sure not to redirect file & api access only page access
- not begins with "/api/"
- has a path
- Sec-Fetch-Mode = navigate
- Sec-Fetch-Dest = document

## Domain redirection #06/03/2023
web : vedastro.org -> domain registra -> azure DNS -> azure cdn -> web blob storage
api **stable** : api.vedastro.org -> domain registra -> azure DNS -> azure cdn -> stable api server (render)
api **beta** : beta.api.vedastro.org -> domain registra -> azure DNS -> azure cdn -> beta api server (azure)
domain cert managed by lets encyrpt acme bot azure func

## API domain is routed # MAR 2023
via Azure CDN Rules Engine, this allows the use of ```api.vedastro.org``` & ```beta.api.vedastro.org```

## Antaram > Sukshma > Prana > Avi Prana > Viprana  # FEB 2023
Since not documented by BV. Raman, code here is created through experimentation by
repeating relationship between Dasa planet & Bhukti planet.

## Skipping EventDataList.xml # FEB 2023
Not all data regarding an event is hardwired. Generating gochara, antaram, sukshma and others is more effcient if description was created by Astronomical calculator
At the moment EventDataList.xml is the source of truth, meaning if an event exists in xml file, then it must exist in code.

## Direct Events Chart # NOV 2022
- Accessing events chart directly via API generated html
- CORS in Azure Website Storage needs to be disabled for this to work, outside of vedastro.org

## Events Chart default timezone # NOV 2022
The default timezone generated for all svg charts will be based on client timezone.
Timezone does not matter when full life charts are made, but will matter alot when
short term muhurtha charts are generated. Since most users are not living where they were born,
it is only logical to default it client browser's timezone.
This timezone must be visible/changeable to users who need to use otherwise.

## Event Chart Notes : Life Events # APR 2022
- This feature is to store notes on the dasa report
- The notes are actualy Events converted to XML and stored inside each person's record
- When rendering these events are placed on top dasa report view

## WEBSITE : Why astrological calculation done on API server and not in client (browser) via webassmebly?
- The calculations tested on Intel Xeon with parallel procesing takes about 1GB RAM & 30% CPU.
With these loads browsers with mobile CPU's are going to be probelmatic for sure.
So as not to waste time, the API route has been decided since it has been proven to work.
- There are places where all Astronomical computation is done in client, exp. Planet Info Box 


## MUHURTHA : Notes On Gochara Prediction (from book ) # FEB 2022

- Built on reference to, Hindu Predictive Astrology pg. 254

- Asthavarga bindus are different from shadbala and it is to be implemented soon.
- Asthavarga bindus are not yet account for, asthavarga good or bad nature of the planet.
  It is assumed that Shadbala system can compensate for it.

- This passage on page 255 needs to be clarified
"It must be noted that when passing through the first 10
degrees of a sign, Mars and the Sun produce results."

- It's intepreted that Vendha is an obstruction and not a reversal of the Gochara results
  So as for now the design is that if a vedha is present than the result is simply nullified.

- In Horoscope predictions methods have "time" & "person" arguments available, 
  obvioulsy "time" is not needed, but for sake of semantic similarity 
  with Muhurtha methods this is maintained.


## Zoom functionality in Dasa View Box

  - Option 1 : generate a high res image (svg/html) and zoom horitontally into it
              - very fast
              - image gets blurry

  - Option 2 : Regenerate whole component in Blazor
              - very slow
              - hard to implement with touch screen

  - Option 3 : Generate multiple preset zooms, than place them on top of each other,
               and only make visible what is needed via selector
              - complicated, needs documentation
              - easy touch screen implimentation
              - very fast

Thus Option 3 was choosen.

## Astrology Library: Use of Struct vs Class
-   Structs are used to reduce overhead from large collections, exp. List<>
-   When structs are part of a class, they are stored in the heap. An additional benefit is that structs need less memory than a class because they have no ObjectHeader or MethodTable. You should consider using a
    struct when the size of the struct will be minimal (say around 16 bytes), the struct will be short-lived, or the struct will be immutable.


## Person Get Hash uses MD5
- default hashing is inconsistent, MD5 is used
- many class's get hash overrides still use default hashing (in cache mechanism),
  could result in errors, needs to be updated
- NOTE : all default hashing is instance specific (FOR STRINGS ONLY so far), works as id in 1 enviroment, 
but with Client + Server config, hashes become different, needs changing to MD5


## Not Obvious Code Conventions
-   In class/struct that only represent data and not computation, use direct property naming without modifiers like "Get" or "Set".
    Example: Person struct should be "Person.BirthTime" and not "Person.GetBirthTime()"
        

## EventDataList.xml
-   3 files exist now, azure storage, desktop, wwwroot (TODO delete all but wwwroot)
-   2 of these files exist, 1 local in MuhurthaCore for desktop version.
    The other online in VedAstro Azure storage for use by API.
    Both files need to be in sync, if forgot to sync. Use file with latest update.
-   Future todo simplify into 1 file. Local MuhurthaCore can be deprecated.

## Event/Prediction Multiple Tags
-   Generally 1 tag for 1 event, add only when needed.
-   Multiple tags can be used by 1 event, separated by "," in in the Tag element
-   Done so that event can be accessed for multiple uses.
    Example, Tarabala Events is taged for Personal & Tarabala.
-   Needs to be added with care and where absolutely needed,
    else could get very confusing.



![GitHub stats](https://github-readme-stats.vercel.app/api?username=sengiv&count_private=true&theme=dark&show_icons=true&hide_border=true&hide=contribs,prs,issues)
![GitHub stats](https://github-readme-stats.vercel.app/api/top-langs/?username=sengiv&theme=dark&hide_border=true&langs_count=4)