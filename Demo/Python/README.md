# VedAstro Python
[![License](https://img.shields.io/github/license/VedAstro/VedAstro.Python)](https://github.com/VedAstro/VedAstro.Python/blob/main/LICENSE)
[![GitHub Issues](https://img.shields.io/github/issues/VedAstro/VedAstro.Python)](https://github.com/VedAstro/VedAstro.Python/issues)

This is a Python wrapper VedAstro for [VedAstro](https://github.com/VedAstro/VedAstro). A powerful tool for astronomical calculations and data analysis. It provides a collection of functions and classes to perform various astronomical calculations, such as celestial object positions, time conversions, coordinate transformations, and more.


## Features
- Calculate the position of celestial objects (planets, stars, etc.) at a given date and time.
- Calculate Dasas
- Calculate Charts ( Rasi , Dasa .. etc)
- Many more ...


## Install
Step 1: [Download .NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.400-windows-x64-installer)

Step 2: Install VedAstro using pip:

```shell
pip install vedastro
```

[Watch Video Guide](https://youtu.be/chEeF-xEQ48?si=KBaLD-8dX1_NGr67)

## Demo Usage

Here's a simple example.
```python
from vedastro.calculators import SaturnInAries
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
```

Other Example/Demo Code
- [Calculate Events](https://github.com/VedAstro/VedAstro.Python/blob/main/demo-calculate-events.py) calculate Muhurtha events for a person in a time range
- [Set Custom Ayanamsa](https://github.com/VedAstro/VedAstro.Python/blob/main/demo-custom-ayanamsa.py) change Ayanamsa to Lahiri, Krishnamurti or Yukteswar
- [Planet & House Data](https://github.com/VedAstro/VedAstro.Python/blob/main/demo-general-calculators.py) calculate astrological data for a house or planet, exp: House Strenght, Planet Longitude, House Sign, etc.. 


## Contributing

Contributions to VedAstro Python are welcome! If you find a bug, have a feature request, or want to contribute code, please open an issue or submit a pull request. Make sure to read our [contribution guidelines](https://github.com/VedAstro/VedAstro.Python/CONTRIBUTING.md) before getting started.

## License

VedAstro Python is released under the MIT License. See [LICENSE](https://github.com/VedAstro/VedAstro.Python/LICENSE) for more information.


