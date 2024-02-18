

# required for API
from fastapi import HTTPException, FastAPI

import json  # for JSON parsing and packing

# for easy async code
import asyncio

# for absolute project paths
import os

# astro chat engines
from chatengine import *

from vedastro import *  # install via pip

import time # for performance measurements

from langchain_community.vectorstores import FAISS


FAISS_INDEX_PATH = "faiss_index"

# instances of chat engines 
# NOTE: keep multiple in global for lazy init & for easy experimentation
# after 1st slow load, will help speed other faster calls
loaded_vectors = {}

# init app to handle HTTP requests
app = FastAPI(title="Chat API")

def clean_string(s):
    # Define the characters that can't be used in dictionary keys
    invalid_chars = ["{", "}", ":", "'"]

    # Remove invalid characters from the string
    for char in invalid_chars:
        s = s.replace(char, "")

    return s

async def TextChunksToEmbedingVectors(payload, docs):

    # 0 : measure time to regenerate
    st = time.time()

    # 2 : embed the horoscope texts, using CPU LLM
    embeddings = LocalHuggingFaceEmbeddings(payload.llm_model_name)

    # 3 : save to local folder, for future use
    db = FAISS.from_documents(docs, embeddings)
    filePath = f"{FAISS_INDEX_PATH}/{payload.llm_model_name}" #use modal name for multiple modal support
    db.save_local(filePath)

    # 4 : measure time to regenerate
    time_seconds = time.time() - st
    #convert to minutes
    time_minutes = time_seconds / 60

    return time_minutes



@app.get("/")
def home():
    return {"Welcome to Chat API"}

## SEARCH

# receives HTTP request, processes and returns response
@app.post('/HoroscopeLLMSearch')
async def Horoscope_LLMSearch(payload: PayloadBody):
    try:
        global loaded_vectors

        # lazy load for speed
        # use file path as id for dynamic LLM modal support
        filePath = f"{FAISS_INDEX_PATH}/{payload.llm_model_name}" #use modal name for multiple modal support
        if loaded_vectors.get(filePath) is None:
            loaded_vectors[filePath] = EmbedVectors(filePath) # load the horoscope vectors (heavy compute)

        # # get all predictions for given birth time
        # run calculator to get list of prediction names for given birth time
        birthTime = payload.get_birth_time()
        calcResult = Calculate.HoroscopePredictionNames(birthTime)

        # format list nicely so LLM can swallow 
        birthPredictions = {"name": [item for item in calcResult]}

        # do LLM search on found predictions
        results = loaded_vectors[filePath].search(payload.query, 100, birthPredictions)

        # convert found with score to nice format for sending
        results_formated = ChatTools.doc_with_score_to_dict(results)
        return results_formated
    
    # if fail, fall gracefully
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

# receives HTTP request, processes and returns response
@app.post('/OpenAPIMetadataLLMSearch')
async def OpenAPIMetadata_LLMSearch(payload: PayloadBody):
    global loaded_vectors

    # # get all predictions for given birth time
    # run calculator to get list of prediction names for given birth time
    birthTime = payload.get_birth_time()
    calcResult = Calculate.HoroscopePredictionNames(birthTime)

    # format list nicely so LLM can swallow 
    birthPredictions = {"name": [item for item in calcResult]}

    # do LLM search on found predictions
    results = loaded_vectors.search(payload.query, 100, birthPredictions)

    # convert found with score to nice format for sending
    results_formated = ChatTools.doc_with_score_to_dict(results)
    return results_formated


## RAG


## TRAINING

# REGENERATE HOROSCOPE EMBEDINGS
# takes all horoscope predictions text and converts them into LLM embedding vectors
# which will be used later to run queries for search & AI chat
@app.post('/HoroscopeRegenerateEmbeddings')
async def Horoscope_RegenerateEmbeddings(payload: PayloadBody):
    from langchain_community.vectorstores import FAISS
    from langchain_core.documents import Document
    
    # 1 : get all horoscope texts direct from VedAstro library
    horoscopeDataList = HoroscopeDataListStatic.Rows

    # repackage all horoscope data so LLM can understand (docs)  
    docs = [Document(page_content=horoscope.Description, metadata={ "name": horoscope.Name.ToString(), "nature": horoscope.Nature.ToString() }) for horoscope in horoscopeDataList]

    # 2 : embed the horoscope texts, using CPU LLM (also saves to local disk under modal name)
    time_minutes = await TextChunksToEmbedingVectors(payload, docs)

    # tell call all went well
    return {"Status": "Pass",
            "Payload":f"Amen ✝️ complete, it took {time_minutes} min"}

## SERVER STUFF
