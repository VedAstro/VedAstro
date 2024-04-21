# █───█ █──█ █──█ 　 ▀▀█▀▀ █──█ █▀▀ 　 █──█ █▀▀█ █▀▀▀ ─▀─ █▀▀ 　 █▀▀▄ █▀▀█ █▀▀▄ █ ▀▀█▀▀ 　 ▀▀█▀▀ █▀▀█ █── █─█
# █▄█▄█ █▀▀█ █▄▄█ 　 ──█── █▀▀█ █▀▀ 　 █▄▄█ █──█ █─▀█ ▀█▀ ▀▀█ 　 █──█ █──█ █──█ ─ ──█── 　 ──█── █▄▄█ █── █▀▄
# ─▀─▀─ ▀──▀ ▄▄▄█ 　 ──▀── ▀──▀ ▀▀▀ 　 ▄▄▄█ ▀▀▀▀ ▀▀▀▀ ▀▀▀ ▀▀▀ 　 ▀▀▀─ ▀▀▀▀ ▀──▀ ─ ──▀── 　 ──▀── ▀──▀ ▀▀▀ ▀─▀

# ▀▀█▀▀ █▀▀█ 　 █──█ █▀▀ ▀█
# ──█── █──█ 　 █──█ ▀▀█ █▀
# ──▀── ▀▀▀▀ 　 ─▀▀▀ ▀▀▀ ▄─

# █▀▀▄ █▀▀ █▀▀ █▀▀█ █──█ █▀▀ █▀▀ 　 ░█▀▀█ █▀▀█ █▀▀▄ 　 █▀▀▄ █▀▀█ █▀▀ █▀▀ 　 █▀▀▄ █▀▀█ ▀▀█▀▀ 　 ▀▀█▀▀ █▀▀█ █── █─█
# █▀▀▄ █▀▀ █── █▄▄█ █──█ ▀▀█ █▀▀ 　 ░█─▄▄ █──█ █──█ 　 █──█ █──█ █▀▀ ▀▀█ 　 █──█ █──█ ──█── 　 ──█── █▄▄█ █── █▀▄
# ▀▀▀─ ▀▀▀ ▀▀▀ ▀──▀ ─▀▀▀ ▀▀▀ ▀▀▀ 　 ░█▄▄█ ▀▀▀▀ ▀▀▀─ 　 ▀▀▀─ ▀▀▀▀ ▀▀▀ ▀▀▀ 　 ▀──▀ ▀▀▀▀ ──▀── 　 ──▀── ▀──▀ ▀▀▀ ▀─▀

# ▀▀█▀▀ █▀▀█ 　 █──█ ─▀─ █▀▄▀█ █▀▀ █▀▀ █── █▀▀ ─
# ──█── █──█ 　 █▀▀█ ▀█▀ █─▀─█ ▀▀█ █▀▀ █── █▀▀ ▄
# ──▀── ▀▀▀▀ 　 ▀──▀ ▀▀▀ ▀───▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀── █

import os
import time
from llama_index.core import get_response_synthesizer
from llama_index.core import VectorStoreIndex, get_response_synthesizer
from llama_index.core.retrievers import VectorIndexRetriever
from llama_index.core.query_engine import RetrieverQueryEngine
from llama_index.core import get_response_synthesizer
from llama_index.core.query_engine import RetrieverQueryEngine
from llama_index.core import QueryBundle
from llama_index.core.retrievers import VectorIndexRetriever
from llama_index.core.schema import NodeWithScore
from llama_index.core import SummaryIndex
from llama_index.core.tools import QueryEngineTool
from llama_index.core.query_engine import SubQuestionQueryEngine
from llama_index.agent.openai import OpenAIAgentWorker
from llama_index.agent.openai import OpenAIAgent

# Retrievers
from llama_index.core.retrievers import VectorIndexRetriever
from langchain.prompts import PromptTemplate
from llama_index.core import Settings

from llama_index.core import PromptTemplate
from llama_index.core.node_parser import SentenceSplitter

import os
from llama_index.legacy.embeddings import AzureOpenAIEmbedding

# from llama_index.legacy.llms import AzureOpenAI
from llama_index.llms.azure_openai import AzureOpenAI
from chat_objects import ChatTools
from llama_index.readers.vedastro import *
import os

from vedastro import *
from chat_objects import *
from llama_index.core import load_index_from_storage, StorageContext
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

# █▀█ █░█ █▀▀ █▀█ █▄█   █▀▀ █░█ █▄░█ █▀▀ █▀
# ▀▀█ █▄█ ██▄ █▀▄ ░█░   █▀░ █▄█ █░▀█ █▄▄ ▄█
# this is where the query starts its long journey to heaven and back ✝️

# RAW LLM CALL

	def query_level_0(self, user_question, topic_text):
		print("################ START: query_level_0  ################")
		# STAGE 2 : HASH FOR CACHING
		# create hash of topic given (birth time/book name)
		topic_hash = ChatTools.generate_hash(topic_text)

		# if exist in memory, then ready to use! You pass to the next level, collect 200 points 🪙
		result = self.retrieve_vector_query_llm(topic_hash=topic_hash, user_question=user_question, topic_text=topic_text)
		return result

	# LEVEL 1
	# A Response Synthesizer is what generates a response from an LLM, using a user query and a given set of text chunks.
	# When used in a query engine, the response synthesizer is used after nodes are retrieved from a retriever, and after any node-postprocessors are ran.
	def query_level_1(self, user_question, topic_text, include_response=False):
		print("################ START: query_level_0  ################")
		# STAGE 2 : HASH FOR CACHING
		# create hash of topic given (birth time/book name)
		# topic_hash = ChatTools.generate_hash(topic_text)

		# initialize response synthesizer
		# configure how the query will be processed
		# TODO : stack multiple simpler systesizer back to back
		# notes :- SIMPLE_SUMMARIZE : works good (suspect loss of nodes)
		#        - TREE_SUMMARIZE : works best (very accurate readings) ~5 calls
		#        - COMPACT : works good (good comparison)
		#        - COMPACT_ACCUMULATE, ACCUMULATE : not needed here, checks each prediction on own

		#########################################################

		text_qa_template_str = ("Context information is"
		                        " below. Context is my life description.\n---------------------\n{context_str}\n---------------------\nUsing"
		                        " only the context information, answer"
		                        " the question: {query_str}\nUsing the"
		                        " context if needed, update or repeat the existing answer.\n")
		text_qa_template = PromptTemplate(text_qa_template_str)

		refine_template_str = ("The original question is as follows: {query_str}\nWe have provided an"
		                       " existing answer: {existing_answer}\nWe have the opportunity to refine"
		                       " the existing answer with some more context"
		                       " below. Context is my life description.\n------------\n{context_msg}\n------------\nUsing the new"
		                       " context, update or repeat the existing answer.\n"
		                       "Always answer the query using the provided context information, "
		                       "and not prior knowledge.\n"
		                       "Some rules to follow:\n"
		                       "1. Avoid disclaimers like 'astrology is not a definitive predictor'.\n"
		                       "2. Avoid statements like 'Based on the context, ...' or "
		                       "'The context information ...' or anything along "
		                       "those lines.")
		refine_template = PromptTemplate(refine_template_str)

		new_summary_tmpl_str = ("Context information is below.\n"
		                        "---------------------\n"
		                        "{context_str}\n"
		                        "---------------------\n"
		                        "Given the context information and not prior knowledge, "
		                        "answer the query in a descriptively explanation.\n"
		                        "Query: {query_str}\n"
		                        "Answer: ")
		new_summary_tmpl = PromptTemplate(new_summary_tmpl_str)

		astro_scored_nodes = SimpleBirthTimeReader().load_node_with_score(topic_text)

		response_synthesizer = get_response_synthesizer(
		    llm=self.llm,
		    #callback_manager=self.callback_manager,
		    text_qa_template=text_qa_template,
		    refine_template=refine_template,
		    summary_template=new_summary_tmpl,
		    verbose=True,
		    response_mode="tree_summarize",
		    structured_answer_filtering=True)

		#make LLM call with only synthesizer ONLY if specified else none (Teacher Mode)
		reponse_data = {"response":""} if include_response == False else response_synthesizer.synthesize(user_question, nodes=astro_scored_nodes)

		return (response_synthesizer, reponse_data)

	def query_level_2(self, response_synthesizer, user_question, topic_text, include_response=False):
		print("################ START: query_level_0  ################")

		# summon the mongrel pups! 🐶
		# get response synthesizer
		#response_synthesizer, response = self.query_level_1(user_question=user_question, topic_text=topic_text, include_response=include_response)

		############### HOROSCOPE SUMMARY INDEX
		documents = SimpleBirthTimeReader().load_data(topic_text)
		nodes = Settings.node_parser.get_nodes_from_documents(documents)

		# initialize storage context (by default it's in-memory) CLEAN
		storage_context = StorageContext.from_defaults()
		storage_context.docstore.add_documents(nodes)

		# make indec based on summary
		summary_index = SummaryIndex(nodes, storage_context=storage_context)

		# define query engines
		summary_query_engine = summary_index.as_query_engine(response_mode="tree_summarize")

		# wrap with some metadata, used later for auto calling by SubQ engine
		summary_tool = QueryEngineTool.from_defaults(
		    query_engine=summary_query_engine,
		    name="summary_tool",
		    description=("Useful for summarization questions related to the author's life"),
		)

		############### HOROSCOPE PREDICTIONS INDEX
		topic_hash = ChatTools.generate_hash(topic_text)
		index = self.get_index(topic_hash)
		index.show_progress = True
		vector_retriever = VectorIndexRetriever(index=index, similarity_top_k=40)

		custom_query_engine = RetrieverQueryEngine(retriever=vector_retriever, response_synthesizer=response_synthesizer) # todo check this

		vector_tool = QueryEngineTool.from_defaults(
		    query_engine=custom_query_engine,
		    name="vector_tool",
		    description=("Useful for retrieving specific context to answer specific questions about the author's life"),
		)

		# place all engines as tools in a list for dynamic use by the "Mother" engine
		query_engine_tools = [vector_tool, summary_tool]

		base_sub_query_engine = SubQuestionQueryEngine.from_defaults(
		    query_engine_tools=query_engine_tools,
			response_synthesizer=response_synthesizer,
		    #question_gen = LLMQuestionGenerator.from_defaults(llm=llm) # now using OpenAI Func API
		    use_async=False,
		)

		# FINAL FANTASY
		#make LLM call with only synthesizer ONLY if specified else none (Teacher Mode)
		reponse_data = None if include_response == False else base_sub_query_engine.query(user_question)

		return (query_engine_tools, reponse_data)

	def query_level_3(self, query_engine_tools,  user_question, topic_text, include_response=False):
		print("################ START: query_level_0  ################")
		# STAGE 2 : HASH FOR CACHING
		# create hash of topic given (birth time/book name)
		# topic_hash = ChatTools.generate_hash(topic_text)


		# ############### HOROSCOPE SUMMARY INDEX
		# documents = SimpleBirthTimeReader().load_data(topic_text)
		# nodes = Settings.node_parser.get_nodes_from_documents(documents)

		# # initialize storage context (by default it's in-memory) CLEAN
		# storage_context = StorageContext.from_defaults()
		# storage_context.docstore.add_documents(nodes)

		# # make indec based on summary
		# summary_index = SummaryIndex(nodes, storage_context=storage_context)

		# # define query engines
		# summary_query_engine = summary_index.as_query_engine(response_mode="tree_summarize")

		# # wrap with some metadata, used later for auto calling by SubQ engine
		# summary_tool = QueryEngineTool.from_defaults(
		#     query_engine=summary_query_engine,
		#     name="summary_tool",
		#     description=("Useful for summarization questions related to the author's life"),
		# )


		# ############### HOROSCOPE PREDICTIONS INDEX
		# topic_hash = ChatTools.generate_hash(topic_text)
		# index = self.get_index(topic_hash)
		# index.show_progress = True
		# vector_retriever = VectorIndexRetriever(index=index, similarity_top_k=40)

		# # get response synthesizer
		# response_synthesizer, response = self.query_level_1(user_question=user_question, topic_text=topic_text, include_response=False)

		# custom_query_engine = RetrieverQueryEngine(retriever=vector_retriever, response_synthesizer=response_synthesizer) # todo check this

		# vector_tool = QueryEngineTool.from_defaults(
		#     query_engine=custom_query_engine,
		#     name="vector_tool",
		#     description=("Useful for retrieving specific context to answer specific questions about the author's life"),
		# )


		# # place all engines as tools in a list for dynamic use by the "Mother" engine
		# query_engine_tools = [vector_tool, summary_tool]

		agent = OpenAIAgent.from_tools(
		    query_engine_tools = query_engine_tools,
		    llm=self.llm,
		    verbose=True,
		)

		reponse_data = None if include_response == False else agent.chat(user_question)

		return (agent, reponse_data)





# █▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▀ █░█ █▄░█ █▀▀ █▀
# █▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █▀░ █▄█ █░▀█ █▄▄ ▄█

# all access to index via get & set
# include "topic_text" only if want auto build index if not exist,
# else leave empty to only use stored

	def get_index(self, topic_hash, topic_text=""):
		directory_path = "vector_store/birth_time_predictions/"

		# check if index already exist in memory
		index_exist_in_memory = self.is_index_exist_in_memory(topic_text=topic_text, topic_hash=topic_hash, directory_path=directory_path)

		return self.index[topic_hash]

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
			print("Cached topic vector loaded from disk! Money in the bank 🏦")
			self.index[topic_hash] = temp_index  # only safe to RAM once confirmed no errors
			return True  # let caller know idex is ready in RAM

		except Exception as e:
			#failure here means, not found in disk
			return False

	# downloads & loads index into memory if exist in azure
	# then return true
	# include "topic_text" only if want auto build index if not exist,
	# else leave empty to only use stored
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
				return True  # let caller know idex is ready in RAM

			# possibility 2 : not found,
			# hence new topic, need to generate new index (CALL LLM) TODO for book, now only DOB
			else:
				
				print("Building new index with LLM 🚄")

				# depending on book or DOB call appropriate data loader
				is_birth_time = "Location" in topic_text
				documents =  SimpleBirthTimeReader().load_data(topic_text) if is_birth_time else SimpleAstrologyBookReader().load_data(topic_text) 

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

		# find local index
		topic_hash = kwargs["topic_hash"]
		topic_text = kwargs["topic_text"]
		index = self.get_index(topic_hash)
		index.show_progress = True
		vector_retriever = VectorIndexRetriever(index=index, similarity_top_k=40)

		documents = SimpleBirthTimeReader().load_data(topic_text)

		nodes = Settings.node_parser.get_nodes_from_documents(documents)

		# initialize storage context (by default it's in-memory)
		storage_context = StorageContext.from_defaults()
		storage_context.docstore.add_documents(nodes)

		summary_index = SummaryIndex(nodes, storage_context=storage_context)

		# define query engines
		summary_query_engine = summary_index.as_query_engine(response_mode="tree_summarize")

		summary_tool = QueryEngineTool.from_defaults(
		    query_engine=summary_query_engine,
		    name="summary_tool",
		    description=("Useful for summarization questions related to the author's life"),
		)

		# STAGE 5
		# assemble query engine
		custom_query_engine = RetrieverQueryEngine(retriever=vector_retriever, response_synthesizer=response_synthesizer)

		vector_tool = QueryEngineTool.from_defaults(
		    query_engine=custom_query_engine,
		    name="vector_tool",
		    description=("Useful for retrieving specific context to answer specific questions about the author's life"),
		)

		query_engine_tools = [vector_tool, summary_tool]

		base_sub_query_engine = SubQuestionQueryEngine.from_defaults(
		    query_engine_tools=query_engine_tools,
		    #question_gen = LLMQuestionGenerator.from_defaults(llm=llm)
		    use_async=False,
		)

		agent = OpenAIAgent.from_tools(
		    query_engine_tools,
		    llm=self.llm,
		    verbose=True,
		)
		result3 = agent.chat(kwargs["user_question"])
		print(str(result3))
		# query_engine = SubQuestionQueryEngine.from_defaults(
		#     query_engine_tools=query_engine_tools,
		#     use_async=True,
		# )

		print("###########################################################")
		result2 = base_sub_query_engine.query(kwargs["user_question"])
		print(str(result2))
		print("###########################################################")
		result = custom_query_engine.query(kwargs["user_question"])
		print(str(result))
		return result

	def vector_index_search(self, **kwargs):

		# Assuming vector_index is already defined
		topic = kwargs["topic"]
		user_question = kwargs["user_question"]
		topic_hash = ChatTools.generate_hash(topic)
		topic_index = self.get_index(topic_hash, topic)  #note we specify text, so can create index if don't have
		vector_retriever = VectorIndexRetriever(index=topic_index, similarity_top_k=30)

		# Define a query
		query_bundle = QueryBundle(query_str=user_question)

		# Retrieve nodes
		retrieved_nodes = vector_retriever.retrieve(query_bundle)

		# Print retrieved nodes
		# for node in retrieved_nodes:
		#     print(node)

		return retrieved_nodes
