import json
import os
from langchain_core.documents import Document
from .xml_loader import XMLLoader
from typing import List
from typing import List, Dict, Union
import time  # for performance measurements
from local_huggingface_embeddings import LocalHuggingFaceEmbeddings
from langchain_community.vectorstores import FAISS


FAISS_INDEX_PATH = "faiss_index"

class ChatTools:
    
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
    @staticmethod
    def get_parsed_query_map(llm_model_name):
        from local_huggingface_embeddings import LocalHuggingFaceEmbeddings

        # Initialize the LLM modal to make vectors (local CPU powered)
        embeddings_creator = LocalHuggingFaceEmbeddings(llm_model_name)

        # get pre-defined "query-map" (store in file)
        preset_queries = ChatTools.get_query_map_from_file()

        #fill standard "vector" property
        preset_queries = [ChatTools.fill_vector(level1, llm_model_name, embeddings_creator) for level1 in preset_queries]

        return preset_queries, embeddings_creator


    # just gets map from file and loads it
    @staticmethod
    def map_query_by_similarity(query, llm_model_name,preset_queries, embeddings_creator):
        import numpy as np

        # Embed the user query
        user_vector = embeddings_creator.embed_query(query)
        user_vector_expanded = np.expand_dims(user_vector, axis=0)  # Make the vectors match

        # STEP 2 : Do search on map

        # fill query map scores for user's query
        preset_queries = [ChatTools.fill_similarity_score(user_vector_expanded,level1, llm_model_name,embeddings_creator) for level1 in preset_queries]

        # fill standard "total_score" property at end of parent child chain (to find winner)
        preset_queries = [ChatTools.fill_total_score(level1,0) for level1 in preset_queries]

        # extract out query map path (leading to highest total score)
        map_route = ChatTools.find_max_scoring_topic(preset_queries)
        
        from chat_objects import AutoReply

        # send map route to auto reply engine
        reply_engine = AutoReply(map_route)
        reply_text = reply_engine.try_generate_auto_reply() # no reply is empty
        
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
                current_path = path + [query]  # Include the whole object, not just the 'topic'
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

        #take text called "vector" out of main object
        score = query_map["score"]
        
        #sum the available scores
        total_so_far = score + parent_score

        # do recursive if has chilren
        if "queries" in query_map:
            for sub_q in query_map["queries"]:
                sub_q = ChatTools.fill_total_score(sub_q, total_so_far)
        else:
            #end of line add total property
            query_map["total_score"] = total_so_far

        return query_map
    
    @staticmethod
    def fill_similarity_score(user_query_vector, vector_object, llm_model_name, embeddingsCreator):
        from sklearn.metrics.pairwise import cosine_similarity


        #take text called "vector" out of main object
        vector = vector_object["vector"]

        sub_query_similarities = cosine_similarity(user_query_vector, vector)[0]  # note how we dig 1 level down

        #place data inside package
        vector_object["score"] = sub_query_similarities[0]
        
        # do recursive if has chilren
        if "queries" in vector_object:
            for sub_q in vector_object["queries"]:
                sub_q = ChatTools.fill_similarity_score(user_query_vector, sub_q, llm_model_name, embeddingsCreator)

        return vector_object

    @staticmethod
    def fill_vector(vector_object, llm_model_name, embeddingsCreator):
        import numpy as np

        #take text called "topic" out of main object
        topic = vector_object["topic"]

        #make vector from the text
        vector = embeddingsCreator.embed_query(topic)

        user_vector_expanded = np.expand_dims(vector, axis=0)  # Make the vectors match

        #place data inside package
        vector_object["vector"] = user_vector_expanded
        
        # do recursive if has chilren
        if "queries" in vector_object:
            for sub_q in vector_object["queries"]:
                sub_q = ChatTools.fill_vector(sub_q, llm_model_name, embeddingsCreator)

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
    
    #checks given password with one stored in .env, if fail end here
    @staticmethod
    def password_protect(input_string):
        if input_string != os.environ["PASSWORD"]:
            raise ValueError(f"The password is Spotty")

    # given a list of documents puts into dictionary for Fast API output
    @staticmethod
    def doc_with_score_to_dict(docs: List[Document]) -> List[Dict[str, Union[str, float]]]:
        docs_list = []
        for doc in docs:
            doc_dict = {
                "page_content": doc[0].page_content,
                "metadata": doc[0].metadata,
                "score": float(doc[1]),
            }
            docs_list.append(doc_dict)
        return docs_list
    
    # given a list of documents with score, moves scores into metadata
    # needed else error, since Document is wrapped with value array
    @staticmethod
    def doc_with_score_to_doc(input_documents: List[Document]) -> List[Document]:
        docs_list = []
        for doc in input_documents:
            doc_dict = Document(
                page_content=doc[0].page_content, 
                metadata = {**doc[0].metadata, "score": float(doc[1])}
            )
            docs_list.append(doc_dict)
        return docs_list

    @staticmethod
    async def TextChunksToEmbedingVectors(payload, docs, savePathPrefix):
        # 0 : measure time to regenerate
        st = time.time()

        # 2 : embed the horoscope texts, using CPU LLM
        embeddings = LocalHuggingFaceEmbeddings(payload.llm_model_name)

        # 3 : save to local folder, for future use
        db = FAISS.from_documents(docs, embeddings)
        # use modal name for multiple modal support
        filePath = f"{FAISS_INDEX_PATH}/{savePathPrefix}/{payload.llm_model_name}"
        db.save_local(filePath)

        # 4 : measure time to regenerate
        time_seconds = time.time() - st
        # convert to minutes
        time_minutes = time_seconds / 60

        return time_minutes

