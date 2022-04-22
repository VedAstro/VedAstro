[![Azure Static Web Apps CI/CD](https://github.com/gen-so/Genso.Astrology/actions/workflows/azure-static-web-apps-purple-flower-03ae64d1e.yml/badge.svg)](https://github.com/gen-so/Genso.Astrology/actions/workflows/azure-static-web-apps-purple-flower-03ae64d1e.yml)

# Vedic Astrolger
A non-profit, opensource project to provide Vedic astrological tools for the public.
Check it out @ [VedAstro.org](https://purple-flower-03ae64d1e.1.azurestaticapps.net/)

# How To Use
Coming soon...

# Donate/Support
- [Donate](https://purple-flower-03ae64d1e.1.azurestaticapps.net/donate) to support programming & server costs
- [Report](https://purple-flower-03ae64d1e.1.azurestaticapps.net/reportbugs) problems you find when using software
- [Share](https://purple-flower-03ae64d1e.1.azurestaticapps.net/featurerequest) your ideas for new or better features 
- [Fix](https://purple-flower-03ae64d1e.1.azurestaticapps.net/featurerequestlist) fix bugs & implement features

# Project Architecture

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
              - complicated, neesd documentation
              - easy touch screen implimentation
              - very fast

Thus Option 3 was choosen.

## Astrology Library: Use of Struct vs Class
- structs are used to reduce overhead from large collections

## Person Get Hash uses MD5
- default hashing is inconsistent, MD5 is used
- many class's get hash overrides still use default hashing (in cache mechanism),
  could result in errors, needs to be updated
- NOTE : all default hashing is instance specific (FOR STRINGS ONLY so far), works as id in 1 enviroment, 
but with Client + Server config, hashes become different, needs changing to MD5