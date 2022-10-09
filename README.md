<h3 align="center">

  ![](GithubImages/header-logo.png)

</h3>

<h3 align="center">
  
  ![GitHub last commit](https://img.shields.io/github/last-commit/gen-so/Genso.Astrology)
  ![GitHub commit activity](https://img.shields.io/github/commit-activity/m/gen-so/Genso.Astrology)
  [![Stars](https://img.shields.io/github/stars/gen-so/Genso.Astrology?color=brightgreen)](https://github.com/gen-so/Genso.Astrology/stargazers)
  [![Gitter](https://badges.gitter.im/VedAstro/community.svg)](https://gitter.im/VedAstro/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
  [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
  
</h3>


<h1 align="center"><a href="https://www.vedastro.org">VedAstro.org</a></h1>

# Links
- [Quick Guide](https://vedastro.org/quickguide) learn how to use
- [Report](https://vedastro.org/contact) problems you find when using software
- [Share](https://vedastro.org/contact) your ideas for new or better features 
- [Fix](https://vedastro.org/contact) bugs & implement features
- [Donate](https://vedastro.org/donate) to support programming & server costs


# Unexpected errors?
There are new updates to the VedAstro WebApp almost everyday! Your browsers caches the old version,
which causes errors as it fails to work with the updated API Server. Do a hard refresh & clear your cache.
This will solve 99% of the errors you face.


# Project Architecture
#### Key design notes to understand the internals of the program better.


## Core Library
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
3. Add the prediction/event details PredictionDataList.xml


### To add a new Event Tag
1. Edit in Genso.Astrology.Library EventTag enum. Change here reflects even in GUI




# Design Decision Notes
#### These are randomly ordered notes on why a feature was implemented in a certain way. Will prove usefull when debugging & upgrading code

## Website: Dasa Notes APR 2022
- This feature is to store notes on the dasa report
- The notes are actualy Events converted to XML and stored inside each person's record
- When rendering these events are placed on top dasa report view

## WEBSITE : Why astrological calculation done on API server and not in client (browser) via webassmebly?
- The calculations tested on Intel Xeon with parallel procesing takes about 1GB RAM & 30% CPU.
With these loads browsers with mobile CPU's are going to be probelmatic for sure.
So as not to waste time, the API route has been decided since it has been proven to work.
- There are places where all Astronomical computation is done in client, exp. Planet Info Box 


## MUHURTHA : Notes On Gochara Prediction (from book ) - 11/2/2022

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
