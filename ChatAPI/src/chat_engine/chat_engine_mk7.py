import os
import time
from llama_index.core import get_response_synthesizer
from llama_index.core import VectorStoreIndex, get_response_synthesizer
from llama_index.core.retrievers import VectorIndexRetriever
from llama_index.core.query_engine import RetrieverQueryEngine
from llama_index.core import get_response_synthesizer
from llama_index.core.query_engine import RetrieverQueryEngine

# Retrievers
from llama_index.core.retrievers import (
    VectorIndexRetriever, )
from langchain.prompts import PromptTemplate
from llama_index.core import Settings

from llama_index.core import PromptTemplate
from llama_index.core.node_parser import SentenceSplitter

# from llama_index.llms.azure_openai import AzureOpenAI
from llama_index.core import (
    
    load_index_from_storage,
    StorageContext,
)
import os
from llama_index.legacy.embeddings import AzureOpenAIEmbedding

# from llama_index.legacy.llms import AzureOpenAI
from llama_index.llms.azure_openai import AzureOpenAI
from chat_objects import ChatTools
from llama_index.readers.vedastro import SimpleBirthTimeReader
import os

from vedastro import *
from chat_objects import *

from llama_index.core import (
    load_index_from_storage, )
import numpy as np

custom_prompt = PromptTemplate("""\
Given a conversation (between Human and Astrologer) and a follow up message from Human, \

<Chat History>
{chat_history}

<Follow Up Message>
{question}

<Standalone question>
""")


class ChatEngine7:

    def __init__(self):
        try:
            # 1 : load access to huggingface LLM via Anyscale
            st = time.time()

            # Initialize the Azure OpenAI embedding model
            # NOTE: using a special legacy version so can access Azure's version
            #       go to portal to get below details
            Settings.embed_model = AzureOpenAIEmbedding(
                model="text-embedding-ada-002",
                deployment_name="text-embedder",
                api_key=os.environ["AZURE_OPENAI_API_KEY"],
                azure_endpoint="https://openaimodelserver.openai.azure.com",
                api_version="2024-02-15-preview",
            )

            # temperature: This can be any float between 0 and 1. For example, temperature: 0.7 would make the output somewhat random, while temperature: 0.2 would make it more deterministic.
            # max_tokens: This is an integer that sets the maximum length of the generated text. For example, max_tokens: 100 would limit the output to 100 tokens.
            # frequency_penalty: This can be any float between 0 and 1. For example, frequency_penalty: 0.5 would moderately penalize more frequent tokens.
            # presence_penalty: This can be any float between 0 and 1. For example, presence_penalty: 0.3 would slightly penalize new tokens.
            Settings.llm = AzureOpenAI(
                engine="vedastro",
                model="gpt-35-turbo",
                api_version="2024-02-15-preview",
                azure_endpoint="https://openaimodelserver.openai.azure.com",
                api_key=os.environ["AZURE_OPENAI_API_KEY"],
                temperature=0.25,
                max_tokens=4096,
                # frequency_penalty=0.7,
                # presence_penalty=0.7
            )

            self.llm = Settings.llm  # make copy other's use

            # If you increase the chunk_overlap value, there will be more overlap between
            # consecutive chunks. This means that the same tokens may appear in multiple
            # chunks, which could potentially improve the context understanding
            # for those tokens at the cost of increased redundancy.
            Settings.text_splitter = SentenceSplitter(chunk_size=3000, chunk_overlap=1000)
            Settings.node_parser = SentenceSplitter(chunk_size=3000, chunk_overlap=1000)
            Settings.context_window = 4096

            # maximum input size to the LLM
            Settings.context_window = 4096

            # number of tokens to leave room for the LLM to generate
            # NOTE : this is the secret number in llama-index to increase output length
            #       who knew? God knew 😁
            Settings.num_output = 3000

            # place where index will be stored with by topic hash in RAM
            self.index = {}

            # Measure the time it took
            et = time.time() - st
            print(f"Loading HF model took {et} seconds.")
        except Exception as e:
            print(e)
            raise Exception(f"Failed to load the Chat Engine.\n{e}") from e

    # this is where the query starts,
    def query(self, **kwargs):
        print("################ START: query  ################")
        # STAGE 1 : DATA
        # user's question as text
        user_question = kwargs["text"]
        topic_text = kwargs["topic"]
        directory_path = "vector_store/birth_time_predictions/"

        # STAGE 2 : HASH FOR CACHING
        # create hash of topic given (birth time/book name)
        topic_hash = ChatTools.generate_hash(topic_text)
        # check if index already exist in memory
        index_exist_in_memory = self.is_index_exist_in_memory(topic_text=topic_text, topic_hash=topic_hash, directory_path=directory_path)

        # if exist in memory, then ready to use! You pass to the next level, collect 200 points 🪙
        if index_exist_in_memory:
            result = self.retrieve_vector_query_llm(topic_hash=topic_hash, user_question=user_question)
            return result

        # run query agains index and process calls from there
        result = self.retrieve_vector_query_llm(topic_hash=topic_hash, user_question=user_question)
        return result

    # downloads & loads index into memory if exist in azure
    # then return true
    def is_index_exist_in_disk(self, **kwargs) -> bool:
        print("################ START: is_index_exist_in_disk  ################")

        #try to load index with 50/50 failure expected
        try:
            directory_path = kwargs["directory_path"]
            topic_hash = kwargs["topic_hash"]
            vi_out_path = f"{directory_path}{topic_hash}"
            temp_index = load_index_from_storage(StorageContext.from_defaults(persist_dir=vi_out_path))
            print("Cached topic vector loaded! Money in the bank 🏦")
            self.index[topic_hash] = temp_index  # only safe to RAM once confirmed no errors
            return True  # let caller know idex is ready in RAM

        except Exception as e:
            #failure here means, not found in disk
            return False

    # downloads & loads index into memory if exist in azure
    # then return true
    def is_index_exist_in_memory(self, **kwargs) -> bool:
        print("################ START: is_index_exist_in_memory  ################")

        # get data out nice nice 😁
        directory_path = kwargs["directory_path"]
        topic_hash = kwargs["topic_hash"]
        topic_text = kwargs["topic_text"]

        # if no index in memory
        if self.index.get(topic_hash) is None:
            #if got copy in local files
            is_found = self.is_index_exist_in_disk(directory_path=directory_path, topic_hash=topic_hash)

            #if don't have then check & auto download from azure
            if not is_found:
                is_found = AzureTableManager.download_index_if_any(directory_path, topic_hash)

            # if found, load into memory please 🫡
            if is_found:
                vi_out_path = f"{directory_path}{topic_hash}" # standard path where indexes are placed by all methods
                self.index[topic_hash] = load_index_from_storage(StorageContext.from_defaults(persist_dir=vi_out_path))
                print("Cached topic vector loaded! Money in the bank 🏦")
                return True  # let caller know idex is ready in RAM

            # possibility 2 : not found,
            # hence new topic, need to generate new index (CALL LLM) TODO for book, now only DOB
            else:
                print("Building new index with LLM 🚄")
                # calculate new prediction text for birth time (topic) (using vedastro lib)
                documents = SimpleBirthTimeReader().load_data(topic_text)  # get predictions for given birth time

                # call LLM and embed predictions into vector index (stored in RAM)
                self.index[topic_hash] = VectorStoreIndex.from_documents(documents, show_progress=True)  # TODO checkout summaryindex and FAISS index

                # make copy in cache db to save future LLM calls 💰
                self.save_index_to_azure_db_and_local(topic_hash=topic_hash)
                return True  # let caller know idex is ready in RAM
        else:
            #index already exist Capitan...full speed ahead 🚄
            return True

    def save_index_to_azure_db_and_local(self, **kwargs):
        print("################ START: save_index_to_azure_db_and_local  ################")

        # save index to local temp storage
        directory_path = f"vector_store/birth_time_predictions/"
        topic_hash = kwargs["topic_hash"]
        filePath = f"{directory_path}{topic_hash}"  # note, hash sub dir here becomes blob name prefix

        # save to use back later
        self.index[topic_hash].storage_context.persist(persist_dir=filePath)

        # upload file to azure
        AzureTableManager.upload_directory_to_blob(filePath, topic_hash)

    # standard RAG operation
    def retrieve_vector_query_llm(self, **kwargs):
        print("################ START: retrieve_vector_query_llm  ################")
        # initialize response synthesizer
        # configure how the query will be processed
        # TODO : stack multiple simpler systesizer back to back
        # notes :- SIMPLE_SUMMARIZE : works good (suspect loss of nodes)
        #        - TREE_SUMMARIZE : works best (very accurate readings) ~5 calls
        #        - COMPACT : works good (good comparison)
        #        - COMPACT_ACCUMULATE, ACCUMULATE : not needed here, checks each prediction on own
        response_synthesizer = get_response_synthesizer(response_mode="tree_summarize", structured_answer_filtering=True)  # TODO test without SAF

        # find local index
        topic_hash = kwargs["topic_hash"]
        index = self.index[topic_hash]
        vector_retriever = VectorIndexRetriever(index=index, similarity_top_k=40)

        # STAGE 5
        # assemble query engine
        custom_query_engine = RetrieverQueryEngine(retriever=vector_retriever, response_synthesizer=response_synthesizer)

        result = custom_query_engine.query(kwargs["user_question"])
        return result
