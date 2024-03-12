# import os
# import time
# # from langchain.chains.question_answering import load_qa_chain
# # from langchain.prompts import PromptTemplate
# # from langchain_community.chat_models.anyscale import ChatAnyscale

# # from langchain.chains import LLMChain
# # from langchain.prompts import PromptTemplate
# from llama_index.core import get_response_synthesizer
# from llama_index.core import VectorStoreIndex, get_response_synthesizer
# from llama_index.core.retrievers import VectorIndexRetriever
# from llama_index.core.query_engine import RetrieverQueryEngine
# from llama_index.core import get_response_synthesizer
# from llama_index.core.query_engine import RetrieverQueryEngine
# # Retrievers
# from llama_index.core.retrievers import (
#     VectorIndexRetriever,
# )
# from llama_index.core import PromptTemplate

# # import os module & the OpenAI Python library for calling the OpenAI API
# import os
# from langchain.prompts import PromptTemplate

# from llama_index.core import Settings

# from llama_index.core import PromptTemplate
# from llama_index.core.llms import ChatMessage, MessageRole
# from llama_index.core.chat_engine import CondenseQuestionChatEngine
# from llama_index.core.node_parser import SentenceSplitter
# # from llama_index.llms.azure_openai import AzureOpenAI
# from llama_index.core import (
#     load_index_from_storage,
#     VectorStoreIndex,
#     StorageContext,
# )
# import os
# from llama_index.legacy.embeddings import AzureOpenAIEmbedding
# # from llama_index.legacy.llms import AzureOpenAI
# from llama_index.llms.azure_openai import AzureOpenAI
# from llama_index.core.prompts import SelectorPromptTemplate
# from llama_index.core.prompts.base import PromptTemplate
# from llama_index.core.prompts.prompt_type import PromptType
# from llama_index.core import PromptTemplate
# from llama_index.core.response_synthesizers import TreeSummarize
# from llama_index.core.retrievers import RecursiveRetriever
# from llama_index.core.query_engine import RetrieverQueryEngine
# from llama_index.core import get_response_synthesizer

# custom_prompt = PromptTemplate(
#     """\
# Given a conversation (between Human and Astrologer) and a follow up message from Human, \

# <Chat History>
# {chat_history}

# <Follow Up Message>
# {question}

# <Standalone question>
# """
# )


# class ChatEngine6:

#     def __init__(self, model_name):
#         try:
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
#                 api_key=os.environ["AZURE_OPENAI_API_KEY"],
#                 # SETTINGS
#                 temperature=0.25,
#                 max_tokens=4000
#             )

#             self.llm = Settings.llm  # make copy other's use

#             Settings.node_parser = SentenceSplitter(
#                 chunk_size=512, chunk_overlap=20)
#             Settings.num_output = 512
#             Settings.context_window = 3900

#             # use modal name for multiple modal support
#             self.filePath = """vector_store/horoscope_data"""

#             # rebuild storage context
#             self.storage_context = StorageContext.from_defaults(
#                 persist_dir=self.filePath)

#             # # load index
#             self.index = load_index_from_storage(self.storage_context)

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
#             print(f"Loading MK6 took {et} seconds.")
#         except Exception as e:
#             print(e)
#             raise Exception(f"Failed to load the Chat Engine.\n{e}") from e

#     def query(self, **kwargs):

#         # docs that can be converted to index
#         docs = kwargs["input_documents"]

#         # Create index from documents
#         index = VectorStoreIndex.from_documents(docs, show_progress=True)

#         ## response_synthesizer
#         # templates need to be set here

#         text_qa_template_str = (
#             "Context information is"
#             " below. Context is my life description.\n---------------------\n{context_str}\n---------------------\nUsing"
#             " only the context information, answer"
#             " the question: {query_str}\nUsing the"
#             " context if needed, update or repeat the existing answer.\n"
#         )
#         text_qa_template = PromptTemplate(text_qa_template_str)

#         refine_template_str = (
#             "The original question is as follows: {query_str}\nWe have provided an"
#             " existing answer: {existing_answer}\nWe have the opportunity to refine"
#             " the existing answer with some more context"
#             " below. Context is my life description.\n------------\n{context_msg}\n------------\nUsing the new"
#             " context, update or repeat the existing answer.\n"
#         "Always answer the query using the provided context information, "
#         "and not prior knowledge.\n"
#         "Some rules to follow:\n"
#         "1. Avoid disclaimers like 'astrology is not a definitive predictor'.\n"
#         "2. Avoid statements like 'Based on the context, ...' or "
#         "'The context information ...' or anything along "
#         "those lines."
#         )
#         refine_template = PromptTemplate(refine_template_str)

#         new_summary_tmpl_str = (
#             "Context information is below.\n"
#             "---------------------\n"
#             "{context_str}\n"
#             "---------------------\n"
#             "Given the context information and not prior knowledge, "
#             "answer the query in a descriptively explanation.\n"
#             "Query: {query_str}\n"
#             "Answer: "
#         )
#         new_summary_tmpl = PromptTemplate(new_summary_tmpl_str)

#         #based on response mode, the template is selected
#         response_synthesizer = get_response_synthesizer(
#             response_mode="refine",
#             text_qa_template=text_qa_template,
#             refine_template=refine_template,
#             summary_template=new_summary_tmpl,
#             verbose=True
#         )

#         # configure retriever
#         # TODO can be upgraded
#         retriever = VectorIndexRetriever(
#             index=index,
#             similarity_top_k=10,
#         )

#         # assemble query engine
#         query_engine = RetrieverQueryEngine(
#             retriever=retriever,
#             response_synthesizer=response_synthesizer,
#         )

#         # query
#         user_question = kwargs["query"]
#         result = query_engine.query(user_question)

#         return result
