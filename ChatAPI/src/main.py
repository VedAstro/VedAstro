

# make sure KEY has been supplied
#exp use : api_key = os.environ["ANYSCALE_API_KEY"]
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

# load API keys from .env file
import os
if "ANYSCALE_API_KEY" not in os.environ:
    raise RuntimeError("Please add the ANYSCALE_API_KEY environment variable to run this script. Run the following in your terminal `export OPENAI_API_KEY=...`")


FAISS_INDEX_PATH = "faiss_index"

# instances embedded vector stored here for speed, shared between all calls
loaded_vectors = {}
chat_engines = {}

# init app to handle HTTP requests
app = FastAPI(title="Chat API")


async def TextChunksToEmbedingVectors(payload, docs, savePathPrefix):

    # 0 : measure time to regenerate
    st = time.time()

    # 2 : embed the horoscope texts, using CPU LLM
    embeddings = LocalHuggingFaceEmbeddings(payload.llm_model_name)

    # 3 : save to local folder, for future use
    db = FAISS.from_documents(docs, embeddings)
    filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}" #use modal name for multiple modal support
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
        savePathPrefix = "horoscope"
        filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}" #use modal name for multiple modal support
        if loaded_vectors.get(filePath) is None:
            loaded_vectors[filePath] = EmbedVectors(filePath, payload.llm_model_name) # load the horoscope vectors (heavy compute)

        # # get all predictions for given birth time (aka filter)
        # run calculator to get list of prediction names for given birth time
        birthTime = payload.get_birth_time()
        calcResult = Calculate.HoroscopePredictionNames(birthTime)

        # format list nicely so LLM can swallow 
        birthPredictions = {"name": [item for item in calcResult]}

        # do LLM search on found predictions
        results_formated = loaded_vectors[filePath].search(payload.query, payload.search_type, birthPredictions)        
        return results_formated
    
    # if fail, fall gracefully
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


## RAG
    
# query chat
@app.post('/HoroscopeChat')
async def Horoscope_Chat(payload: PayloadBody):
    global chat_engines

    global loaded_vectors

    # lazy load for speed
    # use file path as id for dynamic LLM modal support
    savePathPrefix = "horoscope"
    filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}" #use modal name for multiple modal support
    if loaded_vectors.get(filePath) is None:
        loaded_vectors[filePath] = EmbedVectors(filePath, payload.llm_model_name) # load the horoscope vectors (heavy compute)

    savePathPrefix = "horoscope"
    filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}" #use modal name for multiple modal support
    if chat_engines.get(filePath) is None:
        chat_engines[filePath] = ChatEngine(filePath, payload.llm_model_name) # load the horoscope vectors (heavy compute)

    # do LLM search on found predictions
    # # get all predictions for given birth time (aka filter)
    # run calculator to get list of prediction names for given birth time
    birthTime = payload.get_birth_time()
    calcResult = Calculate.HoroscopePredictionNames(birthTime)

    # format list nicely so LLM can swallow 
    birthPredictions = {"name": [item for item in calcResult]}
    results_formated = loaded_vectors[filePath].search(payload.query, payload.search_type, birthPredictions)        
    
    results = chat_engines[filePath].qa(payload.query, results_formated)

    return results

## TRAINING

# REGENERATE HOROSCOPE EMBEDINGS
# takes all horoscope predictions text and converts them into LLM embedding vectors
# which will be used later to run queries for search & AI chat
@app.post('/HoroscopeRegenerateEmbeddings')
async def Horoscope_RegenerateEmbeddings(payload: PayloadBody):
    from langchain_core.documents import Document
    
    # 1 : get all horoscope texts direct from VedAstro library
    horoscopeDataList = HoroscopeDataListStatic.Rows

    # repackage all horoscope data so LLM can understand (docs)  
    docs = [Document(page_content=horoscope.Description, metadata={ "name": horoscope.Name.ToString(), "nature": horoscope.Nature.ToString() }) for horoscope in horoscopeDataList]

    # 2 : embed the horoscope texts, using CPU LLM (also saves to local disk under modal name)
    time_minutes = await TextChunksToEmbedingVectors(payload, docs, "horoscope")

    # tell call all went well
    return {"Status": "Pass",
            "Payload":f"Amen ✝️ complete, it took {time_minutes} min"}

## SERVER STUFF
