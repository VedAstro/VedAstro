
import vedastro # install via pip
from VedAstro.Library import * # reference full library

# set planet
planetName = PlanetName.Sun

# set location
geolocation = GeoLocation("Tokyo, Japan", 139.83, 35.65)

# set time hh:mm dd/mm/yyyy zzz
time = Time("23:40 31/12/2010 +08:00", geolocation)

# run calculator to get result
calcResult = Calculate.PlanetAvasta(planetName, time)

# display results
Tools.Print(calcResult)