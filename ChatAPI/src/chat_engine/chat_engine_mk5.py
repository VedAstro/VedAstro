import os
import time
# from langchain.chains.question_answering import load_qa_chain
# from langchain.prompts import PromptTemplate
# from langchain_community.chat_models.anyscale import ChatAnyscale

# from langchain.chains import LLMChain
# from langchain.prompts import PromptTemplate

# import os module & the OpenAI Python library for calling the OpenAI API
import os
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


class ChatEngine5:

    def __init__(self, model_name):
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

            Settings.llm = AzureOpenAI(
                engine="vedastro",
                model="gpt-35-turbo",
                api_version="2024-02-15-preview",
                azure_endpoint="https://openaimodelserver.openai.azure.com",
                api_key=os.environ["AZURE_OPENAI_API_KEY"]
            )

            self.llm = Settings.llm  # make copy other's use

            Settings.node_parser = SentenceSplitter(
                chunk_size=512, chunk_overlap=20)
            Settings.num_output = 512
            Settings.context_window = 3900

            # use modal name for multiple modal support
            self.filePath = """vector_store/horoscope_data"""

            # rebuild storage context
            storage_context = StorageContext.from_defaults(
                persist_dir=self.filePath)

            # # load index
            self.index = load_index_from_storage(storage_context)

            # CHAT ENGINE STUFF

            # starting chat message aka PROMPT engineering
            self.custom_chat_history = [
                ChatMessage(
                    role=MessageRole.USER,
                    content="Hi astrologer, we are having a insightful discussion on text describing my life.",
                ),
                ChatMessage(role=MessageRole.ASSISTANT,
                            content="Okay, sounds good."),
            ]

            query_engine = self.index.as_query_engine()
            self.chat_engine = CondenseQuestionChatEngine.from_defaults(
                llm=self.llm,
                query_engine=query_engine,
                condense_question_prompt=custom_prompt,
                chat_history=self.custom_chat_history,
                verbose=True,
            )

            # Measure the time it took
            et = time.time() - st
            print(f"Loading HF model took {et} seconds.")
        except Exception as e:
            print(e)
            raise Exception(f"Failed to load the Chat Engine.\n{e}") from e

    def query(self, **kwargs):
        from llama_index.core.data_structs import Node
        from llama_index.core.schema import NodeWithScore
        from llama_index.core import get_response_synthesizer
        from llama_index.core import VectorStoreIndex, get_response_synthesizer
        from llama_index.core.retrievers import VectorIndexRetriever
        from llama_index.core.query_engine import RetrieverQueryEngine
        from chat_objects import VedastroRetriever
        from llama_index.core import get_response_synthesizer
        from llama_index.core.query_engine import RetrieverQueryEngine
        # Retrievers
        from llama_index.core.retrievers import (
            BaseRetriever,
            VectorIndexRetriever,
            KeywordTableSimpleRetriever,
        )



        from llama_index.core import PromptTemplate
        from llama_index.core.response_synthesizers import TreeSummarize

        # NOTE: we add an extra tone_name variable here
        qa_prompt_tmpl = (
            "Context information is below.\n"
            "---------------------\n"
            "{context_str}\n"
            "---------------------\n"
            "Given the context information and not prior knowledge, "
            "answer the query.\n"
            "Please also write the answer in the tone of {tone_name}.\n"
            "Query: {query_str}\n"
            "Answer: "
        )
        qa_prompt = PromptTemplate(qa_prompt_tmpl)

        # initialize response synthesizer
        summarizer = TreeSummarize(verbose=True, summary_template=qa_prompt)
        # configure how the query will be processed
        # notes : - SIMPLE_SUMMARIZE : works good (suspect loss of nodes)
        #        - COMPACT : works good (good comparison)
        #        - COMPACT_ACCUMULATE, ACCUMULATE : not needed here, checks each prediction on own
        response_synthesizer = get_response_synthesizer(
            response_mode="tree_summarize", structured_answer_filtering=True)

        user_question = kwargs["query"]

        # configure retriever
        # define custom retriever
        vector_retriever = VectorIndexRetriever(
            index=self.index, similarity_top_k=2)
        keyword_retriever = KeywordTableSimpleRetriever(index=self.index)
        custom_retriever = VedastroRetriever(kwargs["input_documents"])

        # define response synthesizer
        response_synthesizer = get_response_synthesizer()

        # assemble query engine
        custom_query_engine = RetrieverQueryEngine(
            retriever=custom_retriever,
            response_synthesizer=response_synthesizer,
        )

        self.chat_engine._query_engine = custom_query_engine


        #remove
        ghgh = self.custom_chat_history

        result = self.chat_engine.chat(user_question)

        return result
