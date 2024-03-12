import numpy as np
import json
from fastapi import HTTPException, FastAPI, websockets

import json  # for JSON parsing and packing

# for easy async code

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
# logging.basicConfig(stream=sys.stdout, level=logging.DEBUG, force=True)

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

    variation_name = "MK7"

    print("Loaded Vectors go!")

    ####################################
    # prepare the LLM that will answer the query
    # select the correct engine variation
    wrapper = ChatEngine("MK7")
    # load the modal shards (heavy compute)
    chat_engines["MK7"] = wrapper.create_instance()

    print("Chat Model LLMs go!")

    ####################################
    # STEP 1 : Prepare Query Map
    # this will be used to find preset user query intentions, with standard answers
    # preset_queries, embeddings_creator = ChatTools.get_parsed_query_map()

    # print("Query Mapper go!")

    ####################################
    print("All systems are go")
    print("Astronauts, start your engines")
    print("Main engine start")
    print("Ignition sequence start")
    print("All engines running")
    print("We have lift off!")


# brings together the answering mechanism
# marked for OBLIVION
async def answer_to_reply(chat_input):
    print("################ START: ANSWER_TO_REPLY  ################")
    global chat_engines
    global preset_queries
    global embeddings_creator

    # TODO
    # do query mapping if possible
    # if is_query:
    #     auto_reply = ChatTools.map_query_by_similarity(
    #         payload.query, payload.llm_model_name, preset_queries, embeddings_creator)

    # time to call the in big guns...GPU infantry firing LLM "chat/completion" shells!
        # STEP 2: COMBINE CONTEXT AND QUESTION AND SEND TO CHAT LLM
        # Query the chat engine and send the results to the client
    llm_response = chat_engines["MK7"].query(
        text=chat_input["text"],
        topic=chat_input["topic"],  # birth time/book name
        # Controls the trade-off between randomness and determinism in the response
        # A high value (e.g., 1.0) makes the model more random and creative
        # temperature=chat_input["temperature"],
        # # Controls diversity of the response
        # # A high value (e.g., 0.9) allows for more diversity
        # top_p=chat_input["top_p"],
        # # Limits the maximum length of the generated text
        # max_tokens=chat_input["max_tokens"],
        # # Specifies sequences that tell the model when to stop generating text
        # stop=chat_input["stop"],
        # # Returns debug data like usage statistics
        # return_debug_data=False  # Set to True to see detailed information about model usage
    )

    # note: metadata and source nodes used is all here
    #       but we only take AI text response
    text_response = llm_response.response
    return text_response


@app.get("/")
def home():
    return {"Welcome to ChatAPI : open-source chat AI for vedic astrology"}


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
            loaded_vectors[filePath] = EmbedVectors(filePath, payload.llm_model_name)

        # # get all predictions for given birth time (aka filter)
        # run calculator to get list of prediction names for given birth time
        birthTime = payload.get_birth_time()
        calcResult = Calculate.HoroscopePredictionNames(birthTime)

        # format list nicely so LLM can swallow
        birthPredictions = {"name": [item for item in calcResult]}

        # do LLM search on found predictions
        results_formated = loaded_vectors[filePath].search(payload.query, payload.search_type, birthPredictions)
        return results_formated

    # if fail, fall gracefully my dear
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@app.websocket("/HoroscopeChat")
async def horoscope_chat(websocket: websockets.WebSocket):
    global chat_engines  # use cache
    global loaded_vectors  # use cache

    await websocket.accept()
    ai_reply = "👋 Welcome, how may i help?"
    await websocket.send_text(ChatTools.package_reply_for_shippment(command=np.array(["no_feedback"]), text_html=ai_reply, text=ai_reply, text_hash=ChatTools.random_id()))

    # connection has been made, now keep connection alive in infinite loop,
    # with fail safe to catch it midway
    try:
        session_id = ChatTools.random_id(23)
        message_count = 0
        # BEATING HEART 💓
        # this is the loop that keeps chat running at the highest level
        while True:  # beat forever....my sweet love

            # STAGE 1:
            # Receive a message from the client
            # control is held here while waiting for user input
            client_input_raw = await websocket.receive_text()

            # 1.1 : prepare needed data
            input_parsed = json.loads(client_input_raw)

            # command : can come from client as well (exp:follow-up question)
            if "command" in input_parsed and not hasattr(input_parsed["command"], 'tolist'):  # convert to numpy
                input_parsed["command"] = np.array(input_parsed["command"])
            if "command" not in input_parsed:
                # make new command
                input_parsed["command"] = np.array([""])
            # text
            if "text" not in input_parsed:
                input_parsed["text"] = ""  # easy detect if empty
            # topic
            if "topic" not in input_parsed:
                input_parsed["topic"] = ""  # easy detect if empty
            # session id
            if "session_id" not in input_parsed:
                # overwrite only if not specified
                input_parsed["session_id"] = session_id
            # rating
            if "rating" not in input_parsed:
                input_parsed["rating"] = 0  # overwrite only if not specified
            # text_hash : for user to give point to a message and rate it
            if "text_hash" not in input_parsed:
                # overwrite only if not specified
                input_parsed["text_hash"] = ""
            # user_id : internet fungus filter
            if "user_id" not in input_parsed or input_parsed["user_id"] == "101":
                input_parsed["user_id"] = ""  # overwrite only if not specified
            # sender
            if "sender" not in input_parsed:
                input_parsed["sender"] = "Human"  # incoming is always Human

            # STAGE 2 : THINK
            # standard 1st answer
            ai_html_reply = ai_reply = "Thinking....🤔"
            # 2.1 : user not logged in (could be a Machine!) beware internet fungus! (END HERE)
            if input_parsed["user_id"] == "":
                # add special command recognized by VedAstro.js to show handle login nicely
                input_parsed["command"] = np.append(input_parsed["command"], "please_login")
                ai_reply = """Please login sir...to verify you are not a robot 🤖\nEasy & secure login with Google or Facebook\n\n.....I understand this is annoying, but I have no choice!🤗\nthere are robots in the internet who target smart AI Chat agents like me.\nPlease login to start talking about astrology...💬
                """
                ai_html_reply = """
                    Please login sir...to verify you are not a robot 🤖<br>
                    Easy & secure login with <a target="_blank" style="text-decoration-line: none;" href="https://vedastro.org/Account/Login/RememberMe" class="link-primary fw-bold">Google</a> or <a target="_blank" style="text-decoration-line: none;" href="https://vedastro.org/Account/Login/RememberMe" class="link-primary fw-bold">Facebook</a><br><br>
                    .....I understand this is annoying, but I have no choice!🤗<br>
                    there are robots in the internet who target smart AI Chat agents like me.<br>
                    So please login to get started...<br>
                """

            # 2.2 : rating AND text_hash specified --> user is giving rating vote NOT QUERY (END HERE)
            if input_parsed["rating"] != 0 and input_parsed["text_hash"] != "":
                # memorize Human's rating
                AzureTableManager.rate_message(input_parsed["session_id"], input_parsed["text_hash"], input_parsed["rating"])

                # increse contribution score
                contribution_score = AzureTableManager.increase_contribution_score(input_parsed["user_id"], 1)

                # say thanks 🙏
                # NOTE: DO NOT tell the user explcitly to give more feedback
                # pysholocgy 101 : give them the sincere motivation to help instead -> better quality/quantity
                # if we tell the user explicitly, we increase the probablity of deterministic failure, pushing the user into 1 of 2 camps
                ai_reply = f"""Congratulation!🫡\n You have just helped improve astrology worldwide🌍\n I have now memorized your feedback,🧠\n now on all my answer will take your feedback into consideration.\n Thank you so much for the rating🙏\n"""
                ai_html_reply = f"""
                	<h5>🥇 AI Contributor Score : <b>{contribution_score*10}</b></h5>
                    Congratulation!🫡<br>
                    You have just helped improve astrology worldwide🌍<br><br>
                    I have now <b>memorized your feedback</b>,🧠<br>
                    now on all my answer will take your feedback into consideration.<br><br>
                    Thank you so much for the rating🙏<br><br>
                """

            # no feedback needed here
            input_parsed["command"] = np.append(input_parsed["command"], "no_feedback")
            # update caller with itermidiate message
            await websocket.send_text(ChatTools.package_reply_for_shippment(command=input_parsed["command"], text_html=ai_html_reply, text=ai_reply, text_hash=ChatTools.random_id()))

            # get start feedback beyond this point
            # this will START showing give feedback buttons 🗣️🙏
            index = np.where(input_parsed["command"] == "no_feedback")
            input_parsed["command"] = np.delete(input_parsed["command"], index)

            #             +> FOLLOW-UP --> specialiazed lite llm call
            #             |
            # QUESTION ---+
            #             |
            #             +> UNRELATED --> full llama raq synthesis

            # mark as follow up only if a follow up question is present
            is_followup = "followup_question" in input_parsed["command"]
            all_checks_pass = ai_reply == "" or ai_reply == "Thinking....🤔"

            # use later for highlight (UX improve)
            user_question = input_parsed["text"]  # user's question

            # message_number : needed for quick revisit answer lookup
            message_count += 1
            input_parsed["message_number"] = message_count

            # SAVE QUESTION
            # format & log message for inteligent past QA (id out is for reference)
            # chat_raw_input : text, session_id, rating, message_number
            human_question_hash = AzureTableManager.save_message_in_azure(input_parsed)

            # UNRELATED
            if not is_followup and all_checks_pass:  # only call LLM if all checks and balances are even

                #FOLLOW UP
                print("################ DETECTED: LLM UNRELATED REPLY  ################")

                # answer machine (send all needed data)
                raw_result = chat_engines["MK7"].query(user_question=input_parsed["text"], topic_text=input_parsed["topic"])
                ai_reply = raw_result.response


                # STAGE 3 : REPLY
                # log message for inteligent past QA
                # use later for highlight (UX improve)
                input_parsed["text"] = ai_reply
                input_parsed["sender"] = "AI"
                message_count += 1
                input_parsed["message_number"] = message_count
                ai_reply_hash = AzureTableManager.save_message_in_azure(input_parsed)

                # send ans to caller in JSON form, to support metadata
                # highlight text with relevant keywords relating to input query
                html_str_llm = ChatTools.highlight_relevant_keywords_llm(keywords_text=user_question, main_text=ai_reply)
                followup_questions = ChatTools.generate_followup_questions_llm(keywords_text=user_question, main_text=ai_reply)
                packed_box = ChatTools.package_reply_for_shippment(command=input_parsed["command"], text_html=html_str_llm, text=ai_reply, text_hash=ai_reply_hash, followup_questions=followup_questions)
                await websocket.send_text(packed_box)
            
            # FOLLOW-UP
            if is_followup and all_checks_pass: 
                print("################ DETECTED: FOLLOW UP QUESTION ################")

                #prepare needed data
                primary_answer_hash = input_parsed["primary_answer_hash"]  # base question to ask against
                #based on hash get full question as pure text
                primary_answer_data = AzureTableManager.read_from_table(session_id, primary_answer_hash)
                primary_answer = primary_answer_data["text"]

                #based on primary answer, back track to primary question
                primary_question_msg_number = int(primary_answer_data["message_number"]) - 1  # go up 1 step
                primary_question_data = AzureTableManager.read_from_table_message_number(session_id=session_id, message_number=primary_question_msg_number)
                primary_question = primary_question_data["text"]
                followup_question = input_parsed["text"]  # single question sent by client

                # NOTE: SPECIAL DOCS RETRIVEL
                # create a new systhetic query, coposing all possible data
                #to fetch more relevant context than 1st time question asked
                synthetic_user_question = f"{primary_question}\n\n{primary_answer}\n\n{followup_question}"
                horoscope_predictions = chat_engines["MK7"].vector_index_search(topic = input_parsed["topic"], user_question=synthetic_user_question)
                
                # get reply from LLM 🚅
                ai_reply = ChatTools.answer_followup_questions_llm(primary_question=primary_question, primary_answer=primary_answer, horoscope_predictions=horoscope_predictions, followup_question=followup_question)

                # SAVE AI REPLY
                # log message for inteligent past QA
                input_parsed["text"] = ai_reply
                input_parsed["sender"] = "AI"
                message_count += 1
                input_parsed["message_number"] = message_count
                ai_reply_hash = AzureTableManager.save_message_in_azure(input_parsed)

                ai_reply_html = ChatTools.highlight_relevant_keywords_llm(keywords_text=user_question, main_text=ai_reply)
                followup_questions = ChatTools.generate_followup_questions_llm(keywords_text=user_question, main_text=ai_reply)
                packed_box = ChatTools.package_reply_for_shippment(text_html=ai_reply_html, text=ai_reply, text_hash=ai_reply_hash, followup_questions=followup_questions)
                await websocket.send_text(packed_box)

            # end of line no conditions met
            print("END OF LINE!")

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
    # all_predictions_json = json.loads(HoroscopePrediction.ToJsonList(horoscopeDataList).ToString())
    prediction_nodes = ChatTools.vedastro_predictions_to_llama_index_documents(horoscopeDataList)

    # build index
    index = VectorStoreIndex.from_documents(prediction_nodes, show_progress=True)

    filePath = """vector_store/horoscope_data"""

    index.storage_context.persist(persist_dir=filePath)

    # todo commit to GitHub repo

    # tell call all went well
    return {"Status": "Pass", "Payload": f"Amen ✝️ complete, it took {11} min"}


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
    return {"Status": "Pass", "Payload": f"Amen ✝️ complete, it took {11} min"}


# SUMMARISE PREDICTION TEXT
# JSON summarise data
@app.post('/SummarizePrediction')
async def summarize_prediction(payload: SummaryPayload):

    ChatTools.password_protect(payload.password)  # password is Spotty

    from typing import List
    import openai
    from pydantic import BaseModel, Field
    from enum import Enum

    client = openai.OpenAI(base_url="https://api.endpoints.anyscale.com/v1", api_key=os.environ["ANYSCALE_API_KEY"])

    class SpecializedSummary(BaseModel):
        """The format of the answer."""
        Body: str = Field(description="related to physical body, health")
        Mind: str = Field(description="related to state of mind, emotional state")
        Family: str = Field(description="related to friends, family, people around us")
        Romance: str = Field(description="related to romantic relationships, love, marriage")
        Finance: str = Field(description="related to finances, money, income, and wealth")
        Education: str = Field(description="related to studies, learning, education, academic pursuits, knowledge acquisition")

    chat_completion = client.chat.completions.create(
        model="mistralai/Mistral-7B-Instruct-v0.1",  # check for more model
        response_format={
            "type": "json_object",
            "schema": SpecializedSummary.model_json_schema()
        },
        messages=[{
            "role": "system",
            "content": f"Output JSON. Only use context. \n CONTEXT:{{{payload.input_text}}} "
        }, {
            "role": "user",
            "content": payload.instruction_text
        }],
        temperature=payload.temperature)

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
