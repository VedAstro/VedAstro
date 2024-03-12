# import os
# import time
# # from langchain.chains.question_answering import load_qa_chain
# # from langchain.prompts import PromptTemplate
# # from langchain_community.chat_models.anyscale import ChatAnyscale

# # from langchain.chains import LLMChain
# # from langchain.prompts import PromptTemplate

# # import os module & the OpenAI Python library for calling the OpenAI API
# import openai
# import json
# import os
# from langchain_openai import AzureChatOpenAI
# from langchain.schema import HumanMessage
# from langchain.chains import LLMChain
# from langchain.prompts import PromptTemplate

# from langchain.prompts.chat import (
#     ChatPromptTemplate,
#     SystemMessagePromptTemplate,
#     HumanMessagePromptTemplate,
# )
# from llama_index.core import Settings

# from llama_index.core import PromptTemplate
# from llama_index.core.llms import ChatMessage, MessageRole
# from llama_index.core.chat_engine import CondenseQuestionChatEngine

# custom_prompt = PromptTemplate(
#     """\
# Given a conversation (between Human and Astrologer) and a follow up message from Human, \
# rewrite the message to be a standalone question that captures all relevant context \
# from the conversation.

# <Chat History>
# {chat_history}

# <Follow Up Message>
# {question}

# <Standalone question>
# """
# )


# class ChatEngine4:

#     def __init__(self, model_name):
#         try:
#             from llama_index.embeddings.openai import OpenAIEmbedding
#             from llama_index.core.node_parser import SentenceSplitter
#             from llama_index.llms.openai import OpenAI
#             from local_huggingface_embeddings import LocalHuggingFaceEmbeddings
#             # from llama_index.llms.azure_openai import AzureOpenAI
#             from llama_index.core import (
#                 SimpleDirectoryReader,
#                 load_index_from_storage,
#                 VectorStoreIndex,
#                 StorageContext,
#             )
#             from llama_index.vector_stores.faiss import FaissVectorStore
#             from IPython.display import Markdown, display
#             import os
#             from llama_index.legacy.embeddings import AzureOpenAIEmbedding
#             # from llama_index.legacy.llms import AzureOpenAI
#             from llama_index.llms.azure_openai import AzureOpenAI
#             # 1 : load access to huggingface LLM via Anyscale
#             st = time.time()

#             # Initialize the Azure OpenAI embedding model
#             # NOTE: using a special legacy version so can access Azure's version
#             #       go to portal to get below details
#             Settings.embed_model = AzureOpenAIEmbedding(
#                 model="text-embedding-ada-002",
#                 deployment_name="text-embedder",
#                 api_key=os.environ["AZURE_OPENAI_API_KEY"],
#                 azure_endpoint="https://openaimodelserver.openai.azure.com",
#                 api_version="2024-02-15-preview",
#             )

#             Settings.llm = AzureOpenAI(
#                 engine="vedastro",
#                 model="gpt-35-turbo",
#                 api_version="2024-02-15-preview",
#                 azure_endpoint="https://openaimodelserver.openai.azure.com",
#                 api_key=os.environ["AZURE_OPENAI_API_KEY"]
#             )

#             self.llm = Settings.llm  # make copy other's use

#             Settings.node_parser = SentenceSplitter(
#                 chunk_size=512, chunk_overlap=20)
#             Settings.num_output = 512
#             Settings.context_window = 3900

#             # use modal name for multiple modal support
#             self.filePath = """vector_store/horoscope_data"""

#             # rebuild storage context
#             storage_context = StorageContext.from_defaults(
#                 persist_dir=self.filePath)

#             # # load index
#             self.index = load_index_from_storage(storage_context)

#             # CHAT ENGINE STUFF

#             # starting chat message aka PROMPT engineering
#             self.custom_chat_history = [
#                 ChatMessage(
#                     role=MessageRole.USER,
#                     content="Hi astrologer, we are having a insightful discussion on text describing my life.",
#                 ),
#                 ChatMessage(role=MessageRole.ASSISTANT,
#                             content="Okay, sounds good."),
#             ]

#             query_engine = self.index.as_query_engine()
#             self.chat_engine = CondenseQuestionChatEngine.from_defaults(
#                 llm=self.llm,
#                 query_engine=query_engine,
#                 condense_question_prompt=custom_prompt,
#                 chat_history=self.custom_chat_history,
#                 verbose=True,
#             )

#             # Measure the time it took
#             et = time.time() - st
#             print(f"Loading HF model took {et} seconds.")
#         except Exception as e:
#             print(e)
#             raise Exception(f"Failed to load the Chat Engine.\n{e}") from e    

#     def query(self, **kwargs):
#         from llama_index.core.data_structs import Node
#         from llama_index.core.schema import NodeWithScore
#         from llama_index.core import get_response_synthesizer

#         # configure how the query will be processed
#         # notes : - SIMPLE_SUMMARIZE : works good (suspect loss of nodes)
#         #        - COMPACT : works good (good comparison)
#         #        - COMPACT_ACCUMULATE, ACCUMULATE : not needed here, checks each prediction on own
#         response_synthesizer = get_response_synthesizer(response_mode="tree_summarize")

#         user_question = kwargs["query"]
        
#         # get response based on nodes as context
#         response = response_synthesizer.synthesize(
#             f"{user_question}, use only text in context, context is a person's life description",
#             nodes=kwargs["input_documents"]
#         )

#         return response
