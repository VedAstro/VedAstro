from vedastro import *

# set location
geolocation = GeoLocation("Tokyo, Japan", 139.83, 35.65)

# set time hh:mm dd/mm/yyyy zzz
time = Time("23:40 31/12/2010 +08:00", geolocation)

# set planet
planetName = PlanetName.Sun

# run calculator to get result
calcResult = Calculate.HousePlanetIsIn(time, planetName)

# display results
Tools.Print(calcResult)