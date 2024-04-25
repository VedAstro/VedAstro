import requests
import json
import pandas as pd

# print data nicely
def print_json_as_table(json_array):
    df = pd.DataFrame(json_array)
    print(df)

# combines 2 arrays with common column House
def combine_json_arrays(json_array1, json_array2):
    combined_json_array = []
    for obj1 in json_array1:
        for obj2 in json_array2:
            if obj1["House"] == obj2["House"]:
                combined_obj = {**obj1, **obj2}
                combined_json_array.append(combined_obj)
    return combined_json_array

# set time & location
domainName = "https://api.vedastro.org"
timeLocation = "Location/Kanpur,UttarPradesh,India/Time/17:15/19/11/2002/+05:30"

# get needed data
url = f'{domainName}/Calculate/AllHousePlanetsInHouseBasedOnSign/{timeLocation}'
rawData = requests.get(url).json()["Payload"]
allPlanetHousePositionsBasedOnSignList = next(iter(rawData.values()))

url2 = f'{domainName}/Calculate/AllHouseSignName/{timeLocation}'
rawData2 = requests.get(url2).json()["Payload"]
allHouseSignNameList = next(iter(rawData2.values()))

# combine data 
combined_json_array = combine_json_arrays(allPlanetHousePositionsBasedOnSignList, allHouseSignNameList)

# print data
print_json_as_table(combined_json_array)


