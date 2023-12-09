import VedAstro # install via pip
from VedAstro.Library import * # reference full library



#PART I : PREPARE NEEDED DATA
#-----------------------------------

# set birth location
geolocation = GeoLocation(location="Tokyo", latitude=35.6895, longitude=139.6917).geolocation

# set birth time
date = "31/12/2010" # day/month/year
date2 = "04/01/1992" # day/month/year
time = "23:40" # 24 Hour
time_offset = "+08:00" # standard timezone at birth location

# group all birth time data together
birth_romeo = Time(date, time, time_offset, geolocation).time_object
birth_juliet = Time(date2, time, time_offset, geolocation).time_object

# person profile (optional)
id = "" # random unique ID
user_id = "" # owner of Person profile (API Key/User ID)

# group person data together
romeo = Person(id=id, user_id=user_id, name="Romeo", gender=Gender.Male, birth_time=birth_romeo, notes="").person
juliet = Person(id=id, user_id=user_id, name="Juliet", gender=Gender.Female, birth_time=birth_juliet, notes="").person


#PART II : MAKE CALCULATION
#-----------------------------------

# option 1 : calculate if an astrological event is occuring
saturn_aries = VedAstro.HoroscopeCalculatorMethods.SunInLeo(birth_romeo)

# print the results
occurrence = saturn_aries.Occuring
print(f"Saturn In Aries occuring on {date} : {occurrence}")


# option 2 : get zodiac sign behind a planet (astronomical) 
lordOfHouse1 = VedAstro.Calculate.LordOfHouse(VedAstro.HouseName.House1, birth_romeo)
print(f"House 1 Lord : {lordOfHouse1}")  # Outputs: Mercury


# option 3 : check match using kuta system (16 astro factors)
test = GetNewMatchReport(romeo,juliet,"101")

print(f"Total Match Score : {test.KutaScore}%")
