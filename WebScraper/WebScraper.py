
import requests
from AstroSeekWebScraper import AstroSeekWebScraper



if __name__ == "__main__":
    base_url = "https://famouspeople.astro-seek.com/"

    # create scraper
    astro_chart_loader = AstroSeekWebScraper(base_url)

    # ignite scraper engine!
    astro_chart_data_df = astro_chart_loader.load_all_fam_ppl_roden_aa()

    print("DONE!")

