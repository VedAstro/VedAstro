import re
import time
from datetime import datetime

import pandas as pd

import datetime
import json
import time

import requests
from bs4 import BeautifulSoup
import concurrent.futures


class AstroSeekWebScraper:
    def __init__(self, astro_seek_base_url):
        self._astro_seek_base_url = astro_seek_base_url

    def load_all_fam_ppl_roden_aa(self):

        # Define the range and decrement value
        start_number = 15950
        end_number = 8000
        decrement_value = 50

        # Execute the loop in parallel with a maximum of 3 threads
        with concurrent.futures.ThreadPoolExecutor(max_workers=10) as executor:
            executor.map(AstroSeekWebScraper.extract_and_add_profile, range(start_number, end_number, -decrement_value))


        # get links to all 15995 rows, 320 pages
        # last page is 15950
        # for page_num in range(10, 321):
        #     hrefs_for_all_people_url_AA = self._get_hrefs_for_famous_people_roden_AA(page_num)

        #     # read each url and extract need data and save to db
        #     for href in hrefs_for_all_people_url_AA:
        #         profile = self.get_astro_chart_data_for_famous_person(href)

        #         print(profile)
        # add astro profile data to VedAstro
        # self.add_new_person_to_vedastro(
        #     profile["person_name"],
        #     profile["gender"],
        #     profile["notes"],
        #     "102111269113114363117",
        #     profile["location_name"],
        #     profile["birth_time"],
        # )

    @staticmethod
    def extract_and_add_profile(num):
        print(num)
        hrefs_for_all_people_url_AA = (
            AstroSeekWebScraper._get_hrefs_for_famous_people_roden_AA(num)
        )

        # read each url and extract need data and save to db
        for href in hrefs_for_all_people_url_AA:
            profile = AstroSeekWebScraper.get_astro_chart_data_for_famous_person(
                href
            )

            # print(profile)
            # add astro profile data to VedAstro
            AstroSeekWebScraper.add_new_person_to_vedastro(
                profile["person_name"],
                profile["gender"],
                profile["notes"],
                "xxxxxx",
                profile["location_name"],
                profile["birth_time"],
            )

    def load_raw_celebrity_astro_seek_data(self):

        # get all accupation
        soup = self.get_beauiful_soup_object_from_base_url(self._astro_seek_base_url)
        occupation_type_urls = self._get_hrefs_for_occupation_types(soup)

        for occupation_type_url in occupation_type_urls:
            start_time = time.time()

            occupation = occupation_type_url.split("/")[-1]
            print(f"Collecting data for {occupation}...")

            hrefs_for_famous_people_in_occupation_url = (
                self._get_hrefs_for_famous_people_by_occupation_type(
                    occupation_type_url
                )
            )
            print(
                f"Number of {occupation}: {len(hrefs_for_famous_people_in_occupation_url)}"
            )

            for href in hrefs_for_famous_people_in_occupation_url:
                name_of_famous_person = re.search(
                    "birth-chart/(.*)-horoscope", href
                ).group(1)
                astro_chart_data_for_famous_person = (
                    self.get_astro_chart_data_for_famous_person(href)
                )
                if astro_chart_data_for_famous_person is not None:
                    astro_chart_data_for_famous_person["name"] = name_of_famous_person
                    astro_chart_data_for_famous_person["occupation"] = occupation.split(
                        "famous-"
                    )[-1]
                astro_chart_data_for_people_by_occupation_type.append(
                    astro_chart_data_for_famous_person
                )

            print(
                f"Time take to collect data: {round((time.time() - start_time) / 60, 2)} minutes"
            )
            print("-" * 20)

        astro_chart_data_for_people_by_occupation_type = list(
            filter(None, astro_chart_data_for_people_by_occupation_type)
        )
        astro_chart_data_df = pd.DataFrame(
            astro_chart_data_for_people_by_occupation_type,
            columns=list(astro_chart_data_for_people_by_occupation_type[0].keys()),
        )

        return astro_chart_data_df

    def _get_hrefs_for_occupation_types(self, soup):
        all_search_by_options = soup.find(id="tabs_content_container")
        all_search_by_options_elements = all_search_by_options.find_all(
            "div", class_="inv"
        )

        hrefs_for_search_by_option = []
        for search_by_option_element in all_search_by_options_elements:
            search_by_option_classes = search_by_option_element.find_all(class_="tenky")

            for search_by_option_class in search_by_option_classes:
                search_by_option_class_href = search_by_option_class["href"]
                if "occupation" in search_by_option_class_href:
                    hrefs_for_search_by_option.append(search_by_option_class_href)

        return hrefs_for_search_by_option

    def _get_hrefs_for_famous_people_by_occupation_type(self, occupation_type_url):
        soup = self.get_beauiful_soup_object_from_base_url(occupation_type_url)
        number_of_famous_people_with_occupation_type = len(
            soup.find_all(class_="w260_p5")
        )

        if number_of_famous_people_with_occupation_type >= 200:
            all_pages_for_occupation_type = soup.find_all(
                "a", href=re.compile("filter_occupation")
            )
            hrefs_for_all_pages = [
                page_class["href"] for page_class in all_pages_for_occupation_type
            ]

            all_hrefs_for_famous_people_with_occupation_type = []
            for href in hrefs_for_all_pages:
                href_soup = get_beauiful_soup_object_from_base_url(href)
                hrefs_for_famous_people = self._get_hrefs_from_soup(href_soup)

                all_hrefs_for_famous_people_with_occupation_type.extend(
                    hrefs_for_famous_people
                )
        else:
            all_hrefs_for_famous_people_with_occupation_type = (
                self._get_hrefs_from_soup(soup)
            )

        return all_hrefs_for_famous_people_with_occupation_type

    @staticmethod
    def _get_hrefs_for_famous_people_roden_AA(formated_pg_num):
        # get all people list start from page 0 to 320

        # get whole html page by url
        # page_num -= 1  # 0 index based pg numbring, by 50 a page
        # formated_pg_num = page_num * 50
        soup = AstroSeekWebScraper.get_beauiful_soup_object_from_base_url(
            f"https://famouspeople.astro-seek.com/calculate-advanced-astrology-search_{formated_pg_num})/?rodden=1"
        )

        # extract person profile links for page
        all_hrefs_for_famous_people_with_occupation_type = (
            AstroSeekWebScraper._get_hrefs_from_soup(soup)
        )

        return all_hrefs_for_famous_people_with_occupation_type

    @staticmethod
    def _get_hrefs_from_soup(href_soup):
        famous_people_with_occupation_type = href_soup.find_all(class_="w260_p5")
        hrefs_for_famous_poeple = [
            famous_person.a["href"]
            for famous_person in famous_people_with_occupation_type
        ]

        return hrefs_for_famous_poeple

    # get all basic astro data from astro-seek.com rodden AA in notes
    @staticmethod
    def get_astro_chart_data_for_famous_person(href):

        # all can fail, so do safe
        try:

            soup = AstroSeekWebScraper.get_beauiful_soup_object_from_base_url(href)
            tags = soup.find_all("em")
            astro_chart_raw_data = [tag.text for tag in tags]

            # Find the h2 tag containing the name
            name_tag = soup.find("h2")

            # Get the person's name
            name = name_tag.text.strip().split("-")[0].strip()

            info_div = soup.find("div", class_="detail-info")

            for elem in info_div.find_all("div"):
                if elem.get_text(strip=True).lower() == "occupation:":
                    occupation_elem = elem.find_next_sibling("div")
                    occupation = occupation_elem.get_text(strip=True)
                if elem.get_text(strip=True).lower() == "gender:":
                    gender_elem = elem.find_next_sibling("div")
                    gender = gender_elem.get_text(strip=True)
                if elem.get_text(strip=True).lower() == "birth place:":
                    location_elem = elem.find_next_sibling("div")
                    location_name = location_elem.get_text(strip=True)
                if elem.get_text(strip=True).lower() == "country:":
                    country_elem = elem.find_next_sibling("div")
                    country_name = country_elem.get_text(strip=True)

            formated_dob = AstroSeekWebScraper.convert_date_format(
                astro_chart_raw_data[2]
            )  # convert DOB STD format to sample 00:00/24/06/2024
            astro_chart_data_for_famous_person = {
                "person_name": name,
                "gender": gender,
                "birth_time": formated_dob,
                "location_name": f"{location_name}, {country_name}",
                "notes": {"rodden": "AA"},
            }

        except Exception as e:
            print(f"An error occurred: {e}")

            # Handle the exception (e.g., set a default value)
            astro_chart_data_for_famous_person = {
                "person_name": "Empty",
                "gender": "Male",
                "birth_time": "00:00/01/01/0001",
                "location_name": f"Singapore",
                "notes": {"rodden": "FF"},
            }

        return astro_chart_data_for_famous_person

    # given format from astro-seek.com convert from 15 March 1475 - 01:45 to 01:45/15/03/1475
    @staticmethod
    def convert_date_format(datetime_str):

        # Define a dictionary to map abbreviated month names to full month names (case-insensitive)
        month_mapping = {
            " jan ": " January ",
            " feb ": " February ",
            " mar ": " March ",
            " apr ": " April ",
            " may ": " May ",
            " jun ": " June ",
            " jul ": " July ",
            " aug ": " August ",
            " sep ": " September ",
            " oct ": " October ",
            " nov ": " November ",
            " dec ": " December ",
        }

        # Replace abbreviated month names (case-insensitive) with full month names
        for abbrev, full in month_mapping.items():
            datetime_str = datetime_str.lower().replace(abbrev, full)

        # Parse the input datetime string using strptime() with format A
        parsed_datetime = datetime.datetime.strptime(datetime_str, "%d %B %Y - %H:%M")

        # Format the parsed datetime object into format B using strftime()
        formatted_datetime = parsed_datetime.strftime("%H:%M/%d/%m/%Y")

        return formatted_datetime

    @staticmethod
    def get_beauiful_soup_object_from_base_url(base_url):
        try:
            page = requests.get(base_url)
            soup = BeautifulSoup(page.content, "html.parser")
        except ConnectionResetError:
            print("Connection Reset Error! Wait before next request.")
            time.sleep(120)
            page = requests.get(base_url)
            soup = BeautifulSoup(page.content, "html.parser")

        return soup

    # http://localhost:7071/api/Calculate/AddPerson/OwnerId/xxxx/Location/Singapore/Time/00:00/24/06/2024/+08:00/PersonName/James%20Brown/Gender/Male/Notes/%7Brodden:%22AA%22%7D
    @staticmethod
    def add_new_person_to_vedastro(
        person_name, gender, notes, owner_id, location_name, birth_time
    ):

        # Construct the URL with parameters
        full_url = (
            f"http://localhost:7071/api/Calculate/AddPerson/OwnerId/{owner_id}"
            f"/Location/{location_name}"
            f"/Time/{birth_time}/+00:00"  # 00:00/24/06/2024/+08:00 # add +00:00 for format sake, will be ignored since location name
            f"/PersonName/{person_name}"
            f"/Gender/{gender}"
            f"/Notes/{notes}"
            f"/FailIfDuplicate/True"
        )

        try:
            response = requests.get(full_url)
            if response.status_code == 200:
                # Process the response content here
                print(f"Response content: {response.text}")
            else:
                print(f"Error: Status code {response.status_code}")
        except requests.RequestException as e:
            print(f"Error: {e}")