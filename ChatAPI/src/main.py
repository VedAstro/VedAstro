

# required for API
from fastapi import Body, FastAPI

import json  # for JSON parsing and packing

# for easy async code
import asyncio

# for absolute project paths
import os

# astro chat engines
from chatengine import *

from vedastro import *  # install via pip


# instances of chat engines 
# NOTE: keep multiple in global for lazy init & for easy experimentation
# on server cold start, will help speed other faster calls
all_horoscope_vectors = EmbedVectors(vectorStorePath="faiss_index") # load the horoscope vectors

# init app to handle HTTP requests
app = FastAPI(title="Chat API")


@app.get("/")
def home():
    return {"Welcome to Chat API"}

# receives HTTP request, processes and returns response
@app.post('/llmsearch')
async def horoscopellmsearch(payload: PayloadBody):
    global all_horoscope_vectors

    # # get all predictions for given birth time
    # run calculator to get list of prediction names for given birth time
    birthTime = payload.get_birth_time()
    calcResult = Calculate.HoroscopePredictionNames(birthTime)

    # format list nicely so LLM can swallow 
    birthPredictions = {"name": [item for item in calcResult]}

    # do LLM search on found predictions
    results = all_horoscope_vectors.search(payload.query, 100, birthPredictions)

    # convert found with score to nice format for sending
    results_formated = ChatTools.doc_with_score_to_dict(results)
    return results_formated




