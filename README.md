[![Azure Static Web Apps CI/CD](https://github.com/gen-so/Genso.Astrology/actions/workflows/azure-static-web-apps-purple-flower-03ae64d1e.yml/badge.svg)](https://github.com/gen-so/Genso.Astrology/actions/workflows/azure-static-web-apps-purple-flower-03ae64d1e.yml)

# Vedic Astrolger
A non-profit, opensource project to provide Vedic astrological tools for the public.
Check it out @ [VedAstro.org](https://purple-flower-03ae64d1e.1.azurestaticapps.net/)

# Donate/Support
- [Donate](https://purple-flower-03ae64d1e.1.azurestaticapps.net/donate) to support programming & server costs
- [Report](https://purple-flower-03ae64d1e.1.azurestaticapps.net/reportbugs) problems you find when using software
- [Share](https://purple-flower-03ae64d1e.1.azurestaticapps.net/featurerequest) ideas you for new or better features 
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




# DESIGN NOTES

### Notes On Gochara Prediction in Muhurtha (from book ) - 11/2/2022

- Built on reference to, Hindu Predictive Astrology pg. 254

- Asthavarga bindus are not yet account for, asthavarga good or bad nature of the planet.
  It is assumed that Shadbala system can compensate for it.

- This passage on page 255 needs to be clarified
"It must be noted that when passing through the first 10
degrees of a sign, Mars and the Sun produce results."

- It's intepreted that Vendha is an obstruction and not a reversal of the Gochara results
  So as for now the design is that if a vedha is present than the result is simply nullified.

**In Horoscope predictions methods have "time" & "person" arguments available, 
  obvioulsy "time" is not needed, but for sake of semantic similarity 
  with Muhurtha methods this is maintained.