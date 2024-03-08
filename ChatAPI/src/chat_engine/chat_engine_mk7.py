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
    VectorIndexRetriever,
)
from langchain.prompts import PromptTemplate
from llama_index.core import Settings

from llama_index.core import PromptTemplate
from llama_index.core.llms import ChatMessage, MessageRole
from llama_index.core.chat_engine import CondenseQuestionChatEngine
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

custom_prompt = PromptTemplate(
    """\
Given a conversation (between Human and Astrologer) and a follow up message from Human, \

<Chat History>
{chat_history}

<Follow Up Message>
{question}

<Standalone question>
"""
)


class ChatEngine7:

    def __init__(self, model_name):
        try:
            # 1 : load access to huggingface LLM via Anyscale
            st = time.time()

            # Initialize the Azure OpenAI embedding model
            # NOTE: using a special legacy version so can access Azure's version
            #       go to portal to get below details
            # temperature: This can be any float between 0 and 1. For example, temperature: 0.7 would make the output somewhat random, while temperature: 0.2 would make it more deterministic.
            # max_tokens: This is an integer that sets the maximum length of the generated text. For example, max_tokens: 100 would limit the output to 100 tokens.
            # frequency_penalty: This can be any float between 0 and 1. For example, frequency_penalty: 0.5 would moderately penalize more frequent tokens.
            # presence_penalty: This can be any float between 0 and 1. For example, presence_penalty: 0.3 would slightly penalize new tokens.
            Settings.embed_model = AzureOpenAIEmbedding(
                model="text-embedding-ada-002",
                deployment_name="text-embedder",
                api_key=os.environ["AZURE_OPENAI_API_KEY"],
                azure_endpoint="https://openaimodelserver.openai.azure.com",
                api_version="2024-02-15-preview"
            )

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

            #If you increase the chunk_overlap value, there will be more overlap between
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




            # # load index
            self.index = None

            # Measure the time it took
            et = time.time() - st
            print(f"Loading HF model took {et} seconds.")
        except Exception as e:
            print(e)
            raise Exception(f"Failed to load the Chat Engine.\n{e}") from e

    def query(self, **kwargs):

        # initialize response synthesizer
        # configure how the query will be processed
        # notes : - SIMPLE_SUMMARIZE : works good (suspect loss of nodes)
        #        - COMPACT : works good (good comparison)
        #        - COMPACT_ACCUMULATE, ACCUMULATE : not needed here, checks each prediction on own
        response_synthesizer = get_response_synthesizer(
            response_mode="tree_summarize", structured_answer_filtering=True)

        user_question = kwargs["query"]

        # docs that can be converted to index
        docs = kwargs["input_documents"]

        # Create index from documents (if not created for given birth TODO)
        if self.index is None:
            self.index = VectorStoreIndex.from_documents(
                docs, show_progress=True)

        vector_retriever = VectorIndexRetriever(
            index=self.index, similarity_top_k=40)

        # assemble query engine
        custom_query_engine = RetrieverQueryEngine(
            retriever=vector_retriever,
            response_synthesizer=response_synthesizer,
        )

        result = custom_query_engine.query(user_question)

        return result
