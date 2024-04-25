from vedastro.objects import GeoLocation, Time, Person, Gender
import VedAstro.Library as VedAstro

# Create a GeoLocation object for Tokyo, Japan
geolocation = GeoLocation(location="Tokyo", latitude=35.6895, longitude=139.6917).geolocation

# Define the birth date, time, and time offset
date = "07/05/2010"
time = "06:42"
time_offset = "+09:00"

# Create a Time object for the birth date, time, and time offset
time_ob = Time(date, time, time_offset, geolocation).time_object

# Define the person's ID, user ID, notes, name, and gender
id = "1234"
user_id = "123"
notes = ""
name = "John Doe"
gender = Gender.Male

# Create a Person object for John Doe with the provided details
john_doe = Person(id=id, user_id=user_id, name=name, gender=gender, birth_time=time_ob, notes=notes).person

# do calculation to check if saturn is in aries at a given time
saturn_aries = VedAstro.HoroscopeCalculatorMethods.SaturnInAries(time_ob)

# data if the astro event occured
occurrence = saturn_aries.Occuring

# get the planets or houses related to this astro event
related_body = saturn_aries.RelatedBody

# Print the results
print("Occurrence of Saturn in Aries:", occurrence)
print("Related celestial body:", related_body)