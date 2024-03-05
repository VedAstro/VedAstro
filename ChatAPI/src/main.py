

from typing import List, Dict
import json
from fastapi import HTTPException, FastAPI, websockets

import json  # for JSON parsing and packing

# for easy async code
import asyncio

# for absolute project paths
import os

# astro chat engines
from chat_objects import *
from chat_engine import ChatEngine

from vedastro import *  # install via pip

import time  # for performance measurements

# make sure KEY has been supplied
# exp use : api_key = os.environ["ANYSCALE_API_KEY"]
# load API keys from .env file
import os
if "ANYSCALE_API_KEY" not in os.environ:
    raise RuntimeError("KEY MISSING DINGUS")


FAISS_INDEX_PATH = "faiss_index"

# instances embedded vector stored here for speed, shared between all calls
loaded_vectors = {}
chat_engines = {}
preset_queries = {}
embeddings_creator = {}

# init app to handle HTTP requests
app = FastAPI(title="Chat API")


# ..𝕗𝕠𝕣 𝕚𝕗 𝕪𝕠𝕦 𝕒𝕣𝕖 the 𝕠𝕗 𝕨𝕠𝕣𝕤𝕥 the worst
# 𝕒𝕟𝕕 𝕪𝕠𝕦 𝕝𝕠𝕧𝕖 𝔾𝕠𝕕, 𝕪𝕠𝕦❜𝕣𝕖 𝕗𝕣𝕖𝕖❕
# Yogananda

# prepares server, run once on startup
def initialize_chat_api():
    global chat_engines  # set cache
    global loaded_vectors  # set cache
    global preset_queries  # set cache
    global embeddings_creator  # set cache

    print("T-minus countdown")
    print("Go/No-Go Poll")

    llm_model_name = "sentence-transformers/all-MiniLM-L6-v2"
    chat_model_name = "meta-llama/Llama-2-70b-chat-hf"
    variation_name = "MK3"

    # load full vector DB (contains all predictions text as numbers)
    savePathPrefix = "horoscope"  # use file path as id for dynamic LLM modal support
    
    # use modal name for multiple modal support
    filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{llm_model_name}"


    ####################################
    if loaded_vectors.get(filePath) is None:  # lazy load for speed
        # load the horoscope vectors (heavy compute)
        loaded_vectors[filePath] = EmbedVectors(filePath, llm_model_name)

    print("Loaded Vectors go!")


    ####################################
    # prepare the LLM that will answer the query
    if chat_engines.get(filePath) is None:  # lazy load for speed
        # select the correct engine variation
        wrapper = ChatEngine(variation_name)
        chat_engines[filePath] = wrapper.create_instance(chat_model_name)  # load the modal shards (heavy compute)

    print("Chat AI LLMs go!")


    ####################################
    # STEP 1 : Prepare Query Map 
    preset_queries, embeddings_creator = ChatTools.get_parsed_query_map(llm_model_name)
    
    print("Query Mapper go!")



    ####################################
    print("All systems are go")
    print("Astronauts, start your engines")
    print("Main engine start")
    print("Ignition sequence start")
    print("All engines running")
    print("We have lift off!")


# brings together the answering mechanism
async def answer_to_reply(chat_instance_data):
    global preset_queries
    global embeddings_creator

    # parse incoming data
    # Parse the message
    payload = ChatPayload(chat_instance_data)

    is_query = payload.query != ""
    
    # do query mapping if possible
    if is_query:
        auto_reply = ChatTools.map_query_by_similarity(payload.query, payload.llm_model_name, preset_queries, embeddings_creator)


    # if no reply than time to call the in big guns...GPU infantry firing LLMs shells!
    need_llm = auto_reply == None
    if need_llm & is_query:
        # STEP 1: GET NATIVE'S HOROSCOPE DATA (PREDICTIONS)
        # get all predictions for given birth time (aka filter)
        # run calculator to get list of prediction names for given birth time
        birth_time = payload.get_birth_time()
        calc_result = Calculate.HoroscopePredictionNames(birth_time)
        # format list nicely so LLM can swallow (dict)
        all_predictions = {"name": [item for item in calc_result]}

        # STEP 2: GET PREDICTIONS THAT RELATES TO QUESTION
        # load full vector DB (contains all predictions text as numbers)
        savePathPrefix = "horoscope"  # use file path as id for dynamic LLM modal support
        # use modal name for multiple modal support
        filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}"
        # do LLM search on found predictions
        found_predictions = loaded_vectors[filePath].search(payload.query, payload.search_type, all_predictions)


        # STEP 3: COMBINE CONTEXT AND QUESTION AND SEND TO CHAT LLM
        # Query the chat engine and send the results to the client
        async for chunk in await chat_engines[filePath].query(query=payload.query,
                                                                input_documents=found_predictions,
                                                                # Controls the trade-off between randomness and determinism in the response
                                                                # A high value (e.g., 1.0) makes the model more random and creative
                                                                temperature=payload.temperature,
                                                                # Controls diversity of the response
                                                                # A high value (e.g., 0.9) allows for more diversity
                                                                top_p=payload.top_p,
                                                                # Limits the maximum length of the generated text
                                                                max_tokens=payload.max_tokens,
                                                                # Specifies sequences that tell the model when to stop generating text
                                                                stop=payload.stop,
                                                                # Returns debug data like usage statistics
                                                                return_debug_data=False  # Set to True to see detailed information about model usage
                                                                ):
            return chunk['output_text']
    else:
        time.sleep(1.5) # little delay else, no time for "thinkin" msg to make it
        return auto_reply


@app.get("/")
def home():
    return {"Welcome to VedAstro"}


# SEARCH
@app.post('/HoroscopeLLMSearch')
async def horoscope_llmsearch(payload: SearchPayload):
    try:
        global loaded_vectors

        # lazy load for speed
        # use file path as id for dynamic LLM modal support
        savePathPrefix = "horoscope"
        # use modal name for multiple modal support
        filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}"
        if loaded_vectors.get(filePath) is None:
            # load the horoscope vectors (heavy compute)
            loaded_vectors[filePath] = EmbedVectors(
                filePath, payload.llm_model_name)

        # # get all predictions for given birth time (aka filter)
        # run calculator to get list of prediction names for given birth time
        birthTime = payload.get_birth_time()
        calcResult = Calculate.HoroscopePredictionNames(birthTime)

        # format list nicely so LLM can swallow
        birthPredictions = {"name": [item for item in calcResult]}

        # do LLM search on found predictions
        results_formated = loaded_vectors[filePath].search(
            payload.query, payload.search_type, birthPredictions)
        return results_formated

    # if fail, fall gracefully my dear
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@app.websocket("/HoroscopeChat")
async def horoscope_chat(websocket: websockets.WebSocket):
    # Import the ast module
    import ast
    
    global chat_engines  # use cache
    global loaded_vectors  # use cache

    await websocket.accept()
    await websocket.send_text("Welcome to VedAstro!")

    # variable data related to chat instance (filled during chat)
    chat_instance_data = {}
    prev_chat_instance_data = {}

    # connection has been made, now keep connection alive in infinite loop,
    # with fail safe to catch it midway
    try:
        
        while True:
            # Receive a message from the client
            # control is held here while waiting for user input
            client_input = await websocket.receive_text()
            
            # Let caller process started
            await websocket.send_text("Thinking....")
            
            # answer machine (send all needed data)
            ai_reply = await answer_to_reply(client_input)

            # send ans to caller
            await websocket.send_text(ai_reply)


    # Handle failed gracefully
    except Exception as e:
        print(e)




# RAG
@app.websocket("/HoroscopeChatold")
async def horoscope_chatold(websocket: websockets.WebSocket):
    global chat_engines  # use cache
    global loaded_vectors  # use cache

    await websocket.accept()
    await websocket.send_text("Welcome to VedAstro!")

    # connection has been made, now keep connection alive in infinite loop,
    # with fail safe to catch it midway
    try:
        
        while True:
            # Receive a message from the client
            # control is held here while waiting for user input
            raw_data = await websocket.receive_text()

            # Check if the client wants to exit
            if raw_data == "":
                break

            # Parse the message
            payload = ChatPayload(raw_data)

            # Let caller process started
            await websocket.send_text("Thinking....")

            # STEP 1: GET NATIVE'S HOROSCOPE DATA (PREDICTIONS)
            # get all predictions for given birth time (aka filter)
            # run calculator to get list of prediction names for given birth time
            birth_time = payload.get_birth_time()
            calc_result = Calculate.HoroscopePredictionNames(birth_time)
            # format list nicely so LLM can swallow (dict)
            all_predictions = {"name": [item for item in calc_result]}

            # STEP 2: GET PREDICTIONS THAT RELATES TO QUESTION
            # load full vector DB (contains all predictions text as numbers)
            savePathPrefix = "horoscope"  # use file path as id for dynamic LLM modal support
            # use modal name for multiple modal support
            filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}"
            # do LLM search on found predictions
            found_predictions = loaded_vectors[filePath].search(payload.query, payload.search_type, all_predictions)

            # STEP 3: COMBINE CONTEXT AND QUESTION AND SEND TO CHAT LLM
            # Query the chat engine and send the results to the client
            async for chunk in await chat_engines[filePath].query(query=payload.query,
                                                                  input_documents=found_predictions,
                                                                  # Controls the trade-off between randomness and determinism in the response
                                                                  # A high value (e.g., 1.0) makes the model more random and creative
                                                                  temperature=payload.temperature,
                                                                  # Controls diversity of the response
                                                                  # A high value (e.g., 0.9) allows for more diversity
                                                                  top_p=payload.top_p,
                                                                  # Limits the maximum length of the generated text
                                                                  max_tokens=payload.max_tokens,
                                                                  # Specifies sequences that tell the model when to stop generating text
                                                                  stop=payload.stop,
                                                                  # Returns debug data like usage statistics
                                                                  return_debug_data=False  # Set to True to see detailed information about model usage
                                                                  ):
                await websocket.send_text(chunk['output_text'])

    # Handle failed gracefully
    except Exception as e:
        print(e)


# REGENERATE HOROSCOPE EMBEDINGS
# takes all horoscope predictions text and converts them into LLM embedding vectors
# which will be used later to run queries for search & AI chat
@app.post('/HoroscopeRegenerateEmbeddings')
async def horoscope_regenerate_embeddings(payload: RegenPayload):

    ChatTools.password_protect(payload.password)  # password is Spotty

    from langchain_core.documents import Document

    # 1 : get all horoscope texts direct from VedAstro library
    horoscopeDataList = HoroscopeDataListStatic.Rows

    # repackage all horoscope data so LLM can understand (docs)
    docs = [Document(page_content=horoscope.Description, metadata={"name": horoscope.Name.ToString(
    ), "nature": horoscope.Nature.ToString()}) for horoscope in horoscopeDataList]

    # 2 : embed the horoscope texts, using CPU LLM (also saves to local disk under modal name)
    time_minutes = await ChatTools.TextChunksToEmbedingVectors(payload, docs, "horoscope")

    # tell call all went well
    return {"Status": "Pass",
            "Payload": f"Amen ✝️ complete, it took {time_minutes} min"}


# SUMMARISE PREDICTION TEXT
# JSON summarise data
@app.post('/SummarizePrediction')
async def summarize_prediction(payload: SummaryPayload):

    ChatTools.password_protect(payload.password)  # password is Spotty

    from typing import List
    import openai
    from pydantic import BaseModel, Field
    from enum import Enum

    client = openai.OpenAI(
        base_url="https://api.endpoints.anyscale.com/v1",
        api_key=os.environ["ANYSCALE_API_KEY"]
    )

    class SpecializedSummary(BaseModel):
        """The format of the answer."""
        Body: str = Field(description="related to physical body, health")
        Mind: str = Field(
            description="related to state of mind, emotional state")
        Family: str = Field(
            description="related to friends, family, people around us")
        Romance: str = Field(
            description="related to romantic relationships, love, marriage")
        Finance: str = Field(
            description="related to finances, money, income, and wealth")
        Education: str = Field(
            description="related to studies, learning, education, academic pursuits, knowledge acquisition")

    chat_completion = client.chat.completions.create(
        model="mistralai/Mistral-7B-Instruct-v0.1",  # check for more model
        response_format={
            "type": "json_object",
            "schema": SpecializedSummary.model_json_schema()
        },
        messages=[
            {"role": "system",
             "content": f"Output JSON. Only use context. \n CONTEXT:{{{payload.input_text}}} "},
            {"role": "user", "content": payload.instruction_text}
        ],
        temperature=payload.temperature
    )

    return json.loads(chat_completion.choices[0].message.content)

# blind sighted on a monday afternoon in 2024
# brought my hand close to my nose only to smell the past
# winter in 2015 of a metal burnt with solid flower essence
#
# my beloved now stood then by me too,
# it is through her i see the past,
# and smile 😊

@app.post("/PresetQueryMatch")
async def preset_query_match(payload: TempPayload):
    global preset_queries
    global embeddings_creator

    ChatTools.password_protect(payload.password)  # password is Spotty

    auto_reply = ChatTools.map_query_by_similarity(payload.query, payload.llm_model_name, preset_queries, embeddings_creator)

    return auto_reply

# SERVER STUFF

# do init
initialize_chat_api()
