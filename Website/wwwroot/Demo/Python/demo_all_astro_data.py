from vedastro import *  # install via pip


#PART 1 : PREPARE NEEDED DATA
#-----------------------------------

# set birth location
geolocation = GeoLocation("Tokyo, Japan", 139.83, 35.65)

# group all birth time data together (day/month/year)
birth_time = Time("23:40 31/12/2010 +08:00", geolocation)


#PART 2 : CALCULATE ALL DATA
#-----------------------------------

#PLANETS
allPlanetDataList = Calculate.AllPlanetData(PlanetName.Sun, birth_time)
Tools.Print(allPlanetDataList)

#HOUSES
allHouseDataList = Calculate.AllHouseData(HouseName.House1, birth_time)
Tools.Print(allHouseDataList)

#ZODIAC SIGNS
allZodiacDataList = Calculate.AllZodiacSignData(ZodiacName.Gemini, birth_time)
Tools.Print(allZodiacDataList)

