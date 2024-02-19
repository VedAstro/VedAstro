import time

from .xml_loader import XMLLoader
from .local_huggingface_embeddings import LocalHuggingFaceEmbeddings
from langchain_community.vectorstores import FAISS
import json
from langchain_core.documents import Document
import os
from .chat_tools import ChatTools
from typing import List, Dict, Union

# Define constants
FAISS_INDEX_PATH = "faiss_index"


# represents the vectors from file in memory
class EmbedVectors:
    def __init__(self, vectorStorePath, model_name):
        try:
            # Load the data from faiss
            st = time.time()
            self.embeddings = LocalHuggingFaceEmbeddings(model_name) # works via llama.cpp on CPU (custom glue code)
            self.db = FAISS.load_local(vectorStorePath, self.embeddings)
            et = time.time() - st
            print(f"Loading vector database {vectorStorePath} took {et} seconds.")
        except Exception as e:
            raise Exception("Failed to load the model vector.") from e

    def similarity_search_with_score(self, query, k=4, filter=None):
        """
        Searches the database for documents relevant to the given query using the max-marginal relevance algorithm.

        :param query: The query string to search for.
        :param k: The number of top results to return. Default is 4.
        :param filter: An optional metadata filter to apply. Defaults to None.
        :return: A list of up to k Document objects ranked by their relevance to the query with score 1 high.
        """
        
        if filter is None:
            filter = {}

        # NOTE: Filter
        # only search in horoscope text related native,
        # with filter param of names of predictions that can be used   
        results = self.db.similarity_search_with_score(query, filter=filter, k=k)

        return results
    
    def max_marginal_relevance_search(self, query, k=4, filter=None):
        """
        Searches the database for documents relevant to the given query using the max-marginal relevance algorithm.

        :param query: The query string to search for.
        :param k: The number of top results to return. Default is 4.
        :param filter: An optional metadata filter to apply. Defaults to None.
        :return: A list of up to k Document objects ranked by their relevance to the query with score 1 high.
        """
        
        if filter is None:
            filter = {}

        # NOTE: Filter
        # only search in horoscope text related native,
        # with filter param of names of predictions that can be used   
        #results = self.db.similarity_search_with_score(query, filter=filter, k=k)
        results = self.db.max_marginal_relevance_search(query)

        return results

    def search(self, query, search_type, filter=None):
        # similarity
        if search_type == "similarity":
            results = self.similarity_search_with_score(query, 100, filter)
            # convert found with score to nice format for sending
            results_formated = ChatTools.doc_with_score_to_doc(results)

        # MMR
        # The MaxMarginalRelevanceExampleSelector selects examples
        # based on a combination of which examples are most similar to the inputs,
        # while also optimizing for diversity. It does this by finding the examples 
        # with the embeddings that have the greatest cosine similarity with the inputs,
        #and then iterativelyadding them while penalizing them for closeness to already selected examples.
        if search_type == "mmr":
            results_formated = self.max_marginal_relevance_search(query, 100, filter)

        return results_formated
    
