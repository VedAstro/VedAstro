

import sys
import logging
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


# ログレベルの設定 (make server output more fun to watch 😁 📺)
logging.basicConfig(stream=sys.stdout, level=logging.DEBUG, force=True)

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
    variation_name = "MK4"

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
        chat_engines[llm_model_name] = wrapper.create_instance(chat_model_name)  # load the modal shards (heavy compute)

    print("Chat Model LLMs go!")

    ####################################
    # STEP 1 : Prepare Query Map
    preset_queries, embeddings_creator = ChatTools.get_parsed_query_map(
        llm_model_name)

    print("Query Mapper go!")

    ####################################
    print("All systems are go")
    print("Astronauts, start your engines")
    print("Main engine start")
    print("Ignition sequence start")
    print("All engines running")
    print("We have lift off!")


# converts vedastro horoscope predictions (JSON) to_llama-index's NodeWithScore
# so that llama index can understand vedastro predictions
def vedastro_predictions_to_llama_index_weight_nodes(birth_time, predictions_list_json):
    from llama_index.core.schema import NodeWithScore
    from llama_index.core.schema import TextNode

    # Initialize an empty list
    prediction_nodes = []
    for prediction in predictions_list_json:

        related_bod_json = prediction['RelatedBody']

        # shadbala_score = Calculate.PlanetCombinedShadbala()
        rel_planets = related_bod_json["Planets"]
        parsed_list = []
        for planet in rel_planets:
            parsed_list.append(PlanetName.Parse(planet))

        # TODO temp use 1st planet, house, zodiac
        planet_tags = []
        shadbala_score = 0
        if parsed_list:  # This checks if the list is not empty
            shadbala_score = Calculate.PlanetShadbalaPinda(
                parsed_list[0], birth_time).ToDouble()
            planet_tags = Calculate.GetPlanetTags(parsed_list[0])

        predict_node = TextNode(
            text=prediction["Description"],
            metadata={
                "name": ChatTools.split_camel_case(prediction['Name']),
                "related_body": prediction['RelatedBody'],
                "planet_tags": planet_tags,
            },
            metadata_seperator="::",
            metadata_template="{key}=>{value}",
            text_template="Metadata: {metadata_str}\n-----\nContent: {content}",
        )

        # add in shadbala to give each prediction weights
        parsed_node = NodeWithScore(
            node=predict_node, score=shadbala_score)  # add in shabala score
        prediction_nodes.append(parsed_node)  # add to main list

    return prediction_nodes


def vedastro_predictions_to_llama_index_nodes(birth_time, predictions_list_json):
    from llama_index.core.schema import NodeWithScore
    from llama_index.core.schema import TextNode

    # Initialize an empty list
    prediction_nodes = []
    for prediction in predictions_list_json:

        related_bod_json = prediction['RelatedBody']

        # shadbala_score = Calculate.PlanetCombinedShadbala()
        rel_planets = related_bod_json["Planets"]
        parsed_list = []
        for planet in rel_planets:
            parsed_list.append(PlanetName.Parse(planet))

        # TODO temp use 1st planet, house, zodiac
        planet_tags = []
        shadbala_score = 0
        if parsed_list:  # This checks if the list is not empty
            shadbala_score = Calculate.PlanetShadbalaPinda(
                parsed_list[0], birth_time).ToDouble()
            planet_tags = Calculate.GetPlanetTags(parsed_list[0])

        predict_node = TextNode(
            text=prediction["Description"],
            metadata={
                "name": ChatTools.split_camel_case(prediction['Name']),
                "related_body": prediction['RelatedBody'],
                "planet_tags": planet_tags,
            },
            metadata_seperator="::",
            metadata_template="{key}=>{value}",
            text_template="Metadata: {metadata_str}\n-----\nContent: {content}",
        )

        # add in shadbala to give each prediction weights
        prediction_nodes.append(predict_node)  # add to main list

    return prediction_nodes


def vedastro_predictions_to_llama_index_documents(predictions_list_json):
    from llama_index.core import Document
    from llama_index.core.schema import MetadataMode

    # Initialize an empty list
    prediction_nodes = []
    for prediction in predictions_list_json:

        prediction = json.loads(prediction.ToJson().ToString())


        predict_node = Document(
            text=prediction["Name"],
            metadata=prediction,
            metadata_seperator="::",
            metadata_template="{key}=>{value}",
            text_template="Metadata: {metadata_str}\n-----\nContent: {content}",
        )

        # this is shows difference for understanding output of Documents
        print(
            "The LLM sees this: \n",
            predict_node.get_content(metadata_mode=MetadataMode.LLM),
        )
        print(
            "The Embedding model sees this: \n",
            predict_node.get_content(metadata_mode=MetadataMode.EMBED),
        )

        # add in shadbala to give each prediction weights
        prediction_nodes.append(predict_node)  # add to main list

    return prediction_nodes


# brings together the answering mechanism
async def answer_to_reply(chat_instance_data):
    global preset_queries
    global embeddings_creator
    from llama_index.core import Document, VectorStoreIndex

    # parse incoming data
    # Parse the message
    payload = ChatPayload(chat_instance_data)

    is_query = payload.query != ""

    # do query mapping if possible
    if is_query:
        auto_reply = ChatTools.map_query_by_similarity(
            payload.query, payload.llm_model_name, preset_queries, embeddings_creator)

    # if no reply than time to call the in big guns...GPU infantry firing LLMs shells!
    need_llm = auto_reply == None
    if need_llm & is_query:

        # STEP 1: GET NATIVE'S HOROSCOPE DATA (PREDICTIONS)
        # get all predictions for given birth time (aka filter)
        # run calculator to get list of prediction names for given birth time
        birth_time = payload.get_birth_time()
        all_predictions_raw = Calculate.HoroscopePredictions(birth_time)

        # show the number of horo records found
        print(f"Predictions Found : {len(all_predictions_raw)}")

        # format list nicely so LLM can swallow (llama_index nodes)
        # so that llama index can understand vedastro predictions
        all_predictions_json = json.loads(HoroscopePrediction.ToJsonList(all_predictions_raw).ToString())
        #prediction_nodes = vedastro_predictions_to_llama_index_documents(all_predictions_json)
        prediction_nodes = vedastro_predictions_to_llama_index_weight_nodes(birth_time, all_predictions_json)

        # build index

        # Define your criteria in this function

        def criteria(predictionNode):
            return predictionNode.score > 0
        # This will create a new array with elements that meet the criteria
        prediction_nodes_sorted = [
            element for element in prediction_nodes if criteria(element)]
        # place higest weight at top so doesn't get chopped off
        prediction_nodes_sorted = sorted(
            prediction_nodes_sorted, key=lambda predictionNode: predictionNode.score, reverse=True)

        # Take the top 10 elements
        top_10_array = prediction_nodes_sorted[:20]

        # STEP 2: COMBINE CONTEXT AND QUESTION AND SEND TO CHAT LLM
        # use file path as id for dynamic LLM modal support
        # Query the chat engine and send the results to the client
        llm_response = chat_engines[payload.llm_model_name].query(query=payload.query,
                                                    input_documents=prediction_nodes_sorted,
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
                                                    )

        # note: metadata and source nodes used is all here
        #       but we only take AI text response
        text_response = llm_response.response
        return text_response
    else:
        # little delay else, no time for "thinkin" msg to make it
        time.sleep(1.5)
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
    global chat_engines  # use cache
    global loaded_vectors  # use cache

    await websocket.accept()
    await websocket.send_text("Welcome to VedAstro!")

    # connection has been made, now keep connection alive in infinite loop,
    # with fail safe to catch it midway
    try:

        while True:
            
            
            #STAGE 1:
            # Receive a message from the client
            # control is held here while waiting for user input
            client_input = await websocket.receive_text()

            input_parsed = json.loads(client_input)
            entity = {
            "PartitionKey": ChatTools.generate_hash(input_parsed["query"]), #message will be main id
            "RowKey": ChatTools.generate_hash(time.ctime(time.time())), # time of msg will be 2n id 
            "sender": "user",
            "text": client_input,
            "chat_session_hash": "comming soon!" # hash as id leading to data of chat instance metadata
            }
            AzureTableManager.write_to_table(entity)

            # STAGE 2 : THINK
            # Let caller process started
            await websocket.send_text("Thinking....")

            # answer machine (send all needed data)
            ai_reply = await answer_to_reply(client_input)


            # STAGE 3 : REPLY
            entity = {
            "PartitionKey": ChatTools.generate_hash(input_parsed["query"]), #message will be main id
            "RowKey": ChatTools.generate_hash(time.ctime(time.time())), # time of msg will be 2n id 
            "sender": input_parsed["chat_model_name"], # specify modal's name, so we know who made what
            "text": ai_reply,
            "chat_session_hash": "comming soon!" # hash as id leading to data of chat instance metadata
            }
            AzureTableManager.write_to_table(entity)

            # send ans to caller
            await websocket.send_text(ai_reply)

    # Handle failed gracefully
    except Exception as e:
        print(e)


# REGENERATE HOROSCOPE EMBEDINGS
# takes all horoscope predictions text and converts them into LLM embedding vectors
# which will be used later to run queries for search & AI chat
@app.post('/HoroscopeRegenerateEmbeddings')
async def horoscope_regenerate_embeddings(payload: RegenPayload):
    from llama_index.core import Document, VectorStoreIndex
    from llama_index.core import Settings

    ChatTools.password_protect(payload.password)  # password is Spotty

    from langchain_core.documents import Document
    import chromadb
    from llama_index.core import VectorStoreIndex, SimpleDirectoryReader
    from llama_index.vector_stores.chroma import ChromaVectorStore
    from llama_index.core import StorageContext

    # 1 : get all horoscope texts direct from VedAstro library
    horoscopeDataList = HoroscopeDataListStatic.Rows

    # repackage all horoscope data so LLM can understand (docs)
    # format list nicely so LLM can swallow (llama_index nodes)
    # so that llama index can understand vedastro predictions
    #all_predictions_json = json.loads(HoroscopePrediction.ToJsonList(horoscopeDataList).ToString())
    prediction_nodes = vedastro_predictions_to_llama_index_documents(horoscopeDataList)

    # build index
    index = VectorStoreIndex.from_documents(prediction_nodes, show_progress=True)
    
    filePath = """vector_store/horoscope_data"""

    index.storage_context.persist(persist_dir=filePath)

    # todo commit to GitHub repo


    # tell call all went well
    return {"Status": "Pass",
            "Payload": f"Amen ✝️ complete, it took {11} min"}


# NOTE: below is another methood generating vectors used up till MK3
#       benefit is based on CPU, via FAISS
@app.post('/HoroscopeRegenerateEmbeddingsLegacy')
async def horoscope_regenerate_embeddingsLegacy(payload: RegenPayload):

    ChatTools.password_protect(payload.password)  # password is Spotty

    # LlamaIndexのインポート
    from llama_index.core import VectorStoreIndex, SummaryIndex, SimpleDirectoryReader

    # dataフォルダ内の学習データを使い、インデックスを生成する
    documents = SimpleDirectoryReader('data').load_data()
    VSindex = VectorStoreIndex.from_documents(documents, show_progress=True)
    Sindex = SummaryIndex.from_documents(documents, show_progress=True)

    # 質問を実行
    VSquery_engine = VSindex.as_query_engine()
    Squery_engine = Sindex.as_query_engine()

    # tell call all went well
    return {"Status": "Pass",
            "Payload": f"Amen ✝️ complete, it took {11} min"}


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

    auto_reply = ChatTools.map_query_by_similarity(
        payload.query, payload.llm_model_name, preset_queries, embeddings_creator)

    return auto_reply

# SERVER STUFF

# do init
initialize_chat_api()
