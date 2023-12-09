
from vedastro.calculators import *
from vedastro.objects import *
from System.Collections.Generic import List
import VedAstro.Library as VedAstro


# Create a GeoLocation object for Tokyo, Japan
geolocation = GeoLocation(location="Tokyo", latitude=35.6895, longitude=139.6917).geolocation

# Define the birth date, time, and time offset
time = "06:42"
time_offset = "+08:00"

# Create a Time object for the birth date
start_time = Time("01/01/2020", time, time_offset, geolocation).time_object
end_time = Time("02/01/2020", time, time_offset, geolocation).time_object

# Define the person's ID, user ID, notes, name, and gender
id = ""
user_id = ""
notes = ""
name = "John Doe"
gender = Gender.Male

# Create a Person object for John Doe with the provided details
john_doe = Person(id=id, user_id=user_id, name=name, gender=gender, birth_time=start_time, notes=notes).person

# select category of events to find and calculate
# note: each category can have multiple events (Muhurtha Events/Good Time Finder tool)
tagList = List[VedAstro.EventTag]()
tagList.Add(VedAstro.EventTag.General)
tagList.Add(VedAstro.EventTag.Medical)

# set how accurately the start & end time of each event is calculated
# exp: setting 1 hour, means given in a time range of 1 day, it will be checked 24 times 
precisionInHours = 1;
event_list = VedAstro.EventManager.CalculateEvents(precisionInHours, start_time, end_time, geolocation,john_doe,tagList).GetAwaiter().GetResult()

# print out each event
for event in event_list:
    print(event.ToString())