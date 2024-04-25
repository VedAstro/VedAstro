import VedAstro # install via pip
from VedAstro.Library import * # reference full library

#PART 1 : PREPARE NEEDED DATA
#-----------------------------------

# set birth location
geolocation = GeoLocation("Tokyo, Japan", 139.83, 35.65)

# group all birth time data together
birth_time = Time("23:40 31/12/2010 +08:00", geolocation)


#PART 2 : SET CUSTOM AYANAMSA
# note : must be set before making calling calculator
#-----------------------------------

print(f"LAHIRI AYANAMSA : 285 AD") 
Calculate.YearOfCoincidence = int(Ayanamsa.Lahiri);

#PART 3 : MAKE CALCULATION
#-----------------------------------
moon_constellation = Calculate.PlanetConstellation(birth_time, PlanetName.Sun)
planet_longitude = Calculate.PlanetNirayanaLongitude(birth_time, PlanetName.Sun)
print(f"Sun Constellation : {moon_constellation}")  
print(f"Nirayana Longitude : {planet_longitude}")


#PART 4 : CHANGE CUSTOM AYANAMSA
# note : must be set before making calling calculator
#-----------------------------------

print(f"\nRAMAN AYANAMSA : 397 AD")  
Calculate.YearOfCoincidence = int(Ayanamsa.Raman);

#PART 5 : MAKE CALCULATION
#-----------------------------------
moon_constellation = Calculate.PlanetConstellation(birth_time, PlanetName.Sun)
planet_longitude = Calculate.PlanetNirayanaLongitude(birth_time, PlanetName.Sun)
print(f"Sun Constellation : {moon_constellation}") 
print(f"Nirayana Longitude : {planet_longitude}") 
