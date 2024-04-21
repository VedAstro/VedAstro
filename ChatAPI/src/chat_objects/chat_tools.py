import json
import os
import random
from typing import List, Dict, Union
import string
from vedastro import *  # install via pip
import numpy as np

FAISS_INDEX_PATH = "faiss_index"

import json


class ChatTools:

	@staticmethod
	def convert_to_kwargs(json_obj):
		return "**" + str(json_obj) + "**"

	@staticmethod
	def list_file_names_with_paths(directory, prefix):
		file_dict = {}
		for root, dirs, files in os.walk(directory):
			for file in files:
				file_blob_name = f"{prefix}-{file}"
				local_file_path = os.path.join(root, file)
				file_dict[file_blob_name] = local_file_path
		return file_dict

	@staticmethod
	def list_file_paths(directory) -> str:
		file_paths = []
		for root, dirs, files in os.walk(directory):
			for file in files:
				file_paths.append(os.path.join(root, file))
		return file_paths

	@staticmethod
	def remove_spaces(input_string) -> str:
		return input_string.replace(' ', '')

	# removes all chars that can't go into file name
	@staticmethod
	def sanitize_filename(filename):
		import re
		# Remove invalid characters
		filename = re.sub('[\\\\/:*?"<>|]', '', filename)
		return filename

	@staticmethod
	def random_id(length=8):
		"""Generate a random ID of the given length."""
		letters = string.ascii_letters + string.digits
		return ''.join(random.choice(letters) for _ in range(length))

	# given a couple of dynamic inputs, will format to message for client (AZURE LLM)
	@staticmethod
	def highlight_relevant_keywords_llm(question_text: str, answer_text: str):
		print("################ START: highlight_relevant_keywords_llm  ################")

		import os
		from openai import AzureOpenAI

		# create connetion to azure
		client = AzureOpenAI(azure_endpoint="https://openaimodelserver.openai.azure.com/", api_key=os.getenv("AZURE_OPENAI_KEY"), api_version="2024-02-15-preview")

		#stack question perfectly, to get perfect response
		messages = [
		    {
		        "role": "system",
		        "content": """Follow rules:\n1. Output ANSWER text in HTML format for use between <p> tag element\n2. Highlight relevant words and phrases in ANSWER that is related to QUESTION\n3 . Break long text and organize ANSWER text structure for easy readability. 4. All html element must be valid to be placed inside <p> tag element\n
"""
		    },
		    # NOTE: insert sample for AI to see and learn
		    # {
		    #     "role": "user",
		    #     "content": f"QUESTION:\n\nTell me something about my life\n\nANSWER:\n\nYour life predictions is special and unique to you."
		    # },
		    # {
		    #     "role": "assistant",
		    #     "content": f"Your <b>life predictions</b> is special and <br> unique to you."
		    # },
		    {
		        "role": "user",
		        "content": f"QUESTION:\n\n{question_text}\n\nANSWER:\n\n{answer_text}"
		    },
		]

		for x in messages:
			print(x)

		# call LLM (GPU power to the rescue!)
		#TODO adjust below knobs
		chat_completion = client.chat.completions.create(
		    model="vedastro",  # model = "deployment_name"
		    messages=messages,
		    temperature=0,
		    max_tokens=4096,
		    top_p=1,
		    frequency_penalty=0,
		    presence_penalty=0,
		    stop=None)

		# go through the chaos, and bring forth the answer!🕍🪼
		choices = chat_completion.choices
		message = choices[0].message

		#this should be raw text with html bold tags here and there
		text_html = message.content

		return text_html

	@staticmethod
	def generate_followup_questions_llm(keywords_text: str, main_text: str) -> List[str]:
		print("################ START: generate_followup_questions_llm  ################")

		#TODO TEMP!!!
		#below prompt engineering needs manual work
		#output is not perfect reply with bolded words in html

		return ["When?", "Why?", "How?", "Tell me more..."]

		import os
		from openai import AzureOpenAI

		client = AzureOpenAI(azure_endpoint="https://openaimodelserver.openai.azure.com/", api_key=os.getenv("AZURE_OPENAI_KEY"), api_version="2024-02-15-preview")

		messages = [{"role": "system", "content": "Repeat text in 'Answer' with keywords related to text in 'Question' bolded. Output as HTML."}, {"role": "user", "name": "keywords_text", "content": f"{keywords_text}"}, {"role": "assistant", "name": "main_text", "content": f"<p><b>{keywords_text}</b>: {main_text}</p>"}]

		chat_completion = client.chat.completions.create(
		    model="vedastro",  # model = "deployment_name"
		    messages=messages,
		    temperature=0.0,
		    max_tokens=4096,
		    top_p=1,
		    frequency_penalty=0,
		    presence_penalty=0,
		    stop=None)

		# go through the chaos, and bring forth the answer!🕍🪼
		choices = chat_completion.choices
		message = choices[0].message

		#this should be raw text with html bold tags here and there
		text_html = message.content

		return text_html

	@staticmethod
	def answer_followup_questions_llm(**kwargs) -> List[str]:
		print("################ START: answer_followup_questions_llm  ################")

		import os
		from openai import AzureOpenAI

		# create connetion to azure
		client = AzureOpenAI(azure_endpoint="https://openaimodelserver.openai.azure.com/", api_key=os.getenv("AZURE_OPENAI_KEY"), api_version="2024-02-15-preview")

		# prepare follow up question in correct format
		# NOTE: here is important to obey chat like speech for best answers
		primary_question = kwargs["primary_question"]  # base question to ask against
		primary_answer = kwargs["primary_answer"]  # base question to ask against
		horoscope_predictions = kwargs["horoscope_predictions"]
		followup_question = kwargs["followup_question"]  # single question sent by client

		combined_horscope = ' '.join([node.node.text for node in horoscope_predictions])

		#SPECIAL CASE:

		#stack question perfectly, to get perfect response
		# NOTES : system is solid at understanding "follow up question"
		#       - beware blowing the rule context, rules beyond certain points corrupt whole call
        #       - combined horoscope needs cleaning, with metadata for predictions, so "why?" "how?" can be answered even better
		messages_design_1 = [
		    {
		        "role": "system",
		        "content": f"""Answer with minimum 500 words. \nRules to follow:\n
				1.Avoid disclaimers like 'astrology is not a definitive predictor'.\n
				2.Avoid statements like 'Based on the context, ...' or 'The context information ...' or 'However, it's important to consider ...' or 'However, it's important to note ...'\n\n
				CONTEXT:\n{combined_horscope}"""
		    },
		    {
		        "role": "user",
		        "content": f"Your CONTEXT is a description of a person's life.\n"
		    },
		    {
		        "role": "assistant",
		        "content": f"Okay, I have read and understood this person's life description."
		    },
		    {
		        "role": "user",
		        "content": f"Based only on knowledge from given CONTEXT answer all questions."
		    },
		    {
		        "role": "assistant",
		        "content": f"Okay, ask your question and I'll give a an easy to understand answer."
		    },
		    {
		        "role": "user",
		        "content": f"{primary_question}"
		    },
		    {
		        "role": "assistant",
		        "content": f"{primary_answer}"
		    },
		    {
		        "role": "user",
		        "content": f"{followup_question}"
		    },
		]

        # debug print to improve PROMPTS live
		for x in messages_design_1:
			print(x)

		
		# call LLM (GPU power to the rescue!)
		#TODO adjust below knobs
		chat_completion = client.chat.completions.create(
		    model="vedastro",  # model = "deployment_name"
		    messages=messages_design_1,
		    temperature=0.1,
		    max_tokens=4096,
		    top_p=0.95,
		    frequency_penalty=0,
		    presence_penalty=0.15,
		    stop=None)

		# go through the chaos, and bring forth the answer!🕍🪼
		choices = chat_completion.choices
		message = choices[0].message

		#this should be raw text with html bold tags here and there
		text_html = message.content

		return text_html

	@staticmethod
	def package_reply_for_shippment(**kwargs) -> str:

		cardboard_box = {}

		# Add any additional keyword arguments to the cardboard box
		for key, value in kwargs.items():
			# Check if value is a list or an array and convert to list if necessary
			if isinstance(value, (list, np.ndarray)):
				value = list(value)
			cardboard_box[key] = value

		return json.dumps(cardboard_box)

	# given a string will output sha256, used for id purposes

	@staticmethod
	def generate_hash(input_string):
		import hashlib
		sha_signature = hashlib.sha256(input_string.encode()).hexdigest()
		return sha_signature

	@staticmethod
	def split_camel_case(s):
		import re
		return re.sub('((?<=[a-z])[A-Z]|(?<!\\A)[A-Z](?=[a-z]))', ' \\1', s)

	# just gets map from file and loads it
	# with vectors just loaded from CPU (RAM index)
	# maybe put in disk, but with better index quality (foreign🌍 LLM made)
	@staticmethod
	def get_parsed_query_map():
		from local_huggingface_embeddings import LocalHuggingFaceEmbeddings

		# Initialize the LLM modal to make vectors (local CPU powered)
		embeddings_creator = LocalHuggingFaceEmbeddings("sentence-transformers/all-MiniLM-L6-v2")

		# get pre-defined "query-map" (store in file)
		preset_queries = ChatTools.get_query_map_from_file()

		# NOTE : at this stage the "map" only contains topic text, no vectors for it
		# fill standard "vector" property
		preset_queries = [ChatTools.fill_vector(level1, "sentence-transformers/all-MiniLM-L6-v2", embeddings_creator) for level1 in preset_queries]

		return preset_queries, embeddings_creator

	# just gets map from file and loads it

	@staticmethod
	def map_query_by_similarity(query, llm_model_name, preset_queries, embeddings_creator):
		import numpy as np

		# Embed the user query
		user_vector = embeddings_creator.embed_query(query)
		user_vector_expanded = np.expand_dims(user_vector, axis=0)  # Make the vectors match

		# STEP 2 : Do search on map

		# fill query map scores for user's query
		preset_queries = [ChatTools.fill_similarity_score(user_vector_expanded, level1, llm_model_name, embeddings_creator) for level1 in preset_queries]

		# fill standard "total_score" property at end of parent child chain (to find winner)
		preset_queries = [ChatTools.fill_total_score(level1, 0) for level1 in preset_queries]

		# extract out query map path (leading to highest total score)
		map_route = ChatTools.find_max_scoring_topic(preset_queries)

		from chat_objects import AutoReply

		# send map route to auto reply engine
		reply_engine = AutoReply(map_route)
		reply_text = reply_engine.try_generate_auto_reply()  # no reply is empty

		# Return the most similar main query and sub-query (if any)
		return reply_text

	# just gets map from file and loads it

	@staticmethod
	def get_query_map_from_file():
		file_path = "query_map.json"
		with open(file_path, 'r') as json_file:
			data = json.load(json_file)
		return data

	@staticmethod
	def find_max_scoring_topic(preset_queries):
		max_score = -1
		max_path = []

		def traverse_query_tree(queries, path=[]):
			nonlocal max_score, max_path
			for query in queries:
				# Include the whole object, not just the 'topic'
				current_path = path + [query]
				if 'queries' not in query or not query['queries']:
					score = query.get('total_score', 0)
					if score > max_score:
						max_score = score
						max_path = current_path
				else:
					traverse_query_tree(query['queries'], current_path)

		traverse_query_tree(preset_queries)
		return max_path

	@staticmethod
	def fill_total_score(query_map, parent_score):

		# take text called "vector" out of main object
		score = query_map["score"]

		# sum the available scores
		total_so_far = score + parent_score

		# do recursive if has chilren
		if "queries" in query_map:
			for sub_q in query_map["queries"]:
				sub_q = ChatTools.fill_total_score(sub_q, total_so_far)
		else:
			# end of line add total property
			query_map["total_score"] = total_so_far

		return query_map

	@staticmethod
	def fill_similarity_score(user_query_vector, vector_object, llm_model_name, embeddings_creator):
		from sklearn.metrics.pairwise import cosine_similarity

		# take text called "vector" out of main object
		vector = vector_object["vector"]

		sub_query_similarities = cosine_similarity(user_query_vector, vector)[0]  # note how we dig 1 level down

		# place data inside package
		vector_object["score"] = sub_query_similarities[0]

		# do recursive if has chilren
		if "queries" in vector_object:
			for sub_q in vector_object["queries"]:
				sub_q = ChatTools.fill_similarity_score(user_query_vector, sub_q, llm_model_name, embeddings_creator)

		return vector_object

	@staticmethod
	def fill_vector(vector_object, llm_model_name, embeddings_creator):
		import numpy as np

		# take text called "topic" out of main object
		topic = vector_object["topic"]

		# make vector from the text
		vector = embeddings_creator.embed_query(topic)

		user_vector_expanded = np.expand_dims(vector, axis=0)  # Make the vectors match

		# place data inside package
		vector_object["vector"] = user_vector_expanded

		# do recursive if has chilren
		if "queries" in vector_object:
			for sub_q in vector_object["queries"]:
				sub_q = ChatTools.fill_vector(sub_q, llm_model_name, embeddings_creator)

		return vector_object

	@staticmethod
	def get_highest_scoring_topic(preset_queries):
		# Initialize a dictionary to hold the sum of scores for each topic
		topic_scores = {}

		# Iterate over each category in the preset queries
		for category in preset_queries:
			# Iterate over each topic in the category
			for topic_info in preset_queries[category]:
				topic = topic_info['topic']
				# Sum the scores for the current topic
				total_score = sum(query['score'] for query in topic_info['queries'])
				# Add or update the total score for the topic
				if topic in topic_scores:
					topic_scores[f"{category}_{topic}"] += total_score
				else:
					topic_scores[f"{category}_{topic}"] = total_score

		# Find the topic with the highest total score
		highest_scoring_topic = max(topic_scores, key=topic_scores.get)

		return highest_scoring_topic

	@staticmethod
	def extract_vectors(data):
		return [item['vector'] for item in data]

	@staticmethod
	def extract_texts(data):
		return [item['text'] for item in data]

	# checks given password with one stored in .env, if fail end here
	@staticmethod
	def password_protect(input_string):
		if input_string != os.environ["PASSWORD"]:
			raise ValueError(f"The password is Spotty")

	# given a list of documents puts into dictionary for Fast API output
	# from langchain_core.documents import Document
	# @staticmethod
	# def doc_with_score_to_dict(
	#         docs: List[Document]) -> List[Dict[str, Union[str, float]]]:
	#     docs_list = []
	#     for doc in docs:
	#         doc_dict = {
	#             "page_content": doc[0].page_content,
	#             "metadata": doc[0].metadata,
	#             "score": float(doc[1]),
	#         }
	#         docs_list.append(doc_dict)
	#     return docs_list

	# # given a list of documents with score, moves scores into metadata
	# # needed else error, since Document is wrapped with value array
	# @staticmethod
	# def doc_with_score_to_doc(
	#         input_documents: List[Document]) -> List[Document]:
	#     docs_list = []
	#     for doc in input_documents:
	#         doc_dict = Document(page_content=doc[0].page_content,
	#                             metadata={
	#                                 **doc[0].metadata, "score": float(doc[1])
	#                             })
	#         docs_list.append(doc_dict)
	#     return docs_list

	# @staticmethod
	# async def TextChunksToEmbedingVectors(payload, docs, savePathPrefix):
	#     #note keep here for production thining
	#     from local_huggingface_embeddings import LocalHuggingFaceEmbeddings
	#     from langchain_community.vectorstores import FAISS

	#     # 0 : measure time to regenerate
	#     st = time.time()

	#     # 2 : embed the horoscope texts, using CPU LLM
	#     embeddings = LocalHuggingFaceEmbeddings(payload.llm_model_name)

	#     # 3 : save to local folder, for future use
	#     db = FAISS.from_documents(docs, embeddings)
	#     # use modal name for multiple modal support
	#     filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}"
	#     db.save_local(filePath)

	#     # 4 : measure time to regenerate
	#     time_seconds = time.time() - st
	#     # convert to minutes
	#     time_minutes = time_seconds / 60

	#     return time_minutes
