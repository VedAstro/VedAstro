import time

from .xml_loader import XMLLoader
from .local_huggingface_embeddings import LocalHuggingFaceEmbeddings
from langchain_community.vectorstores import FAISS
import json
from langchain_core.documents import Document
import os

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


# import time

# from .xml_loader import XMLLoader
# from .local_huggingface_embeddings import LocalHuggingFaceEmbeddings
# from langchain_community.vectorstores import FAISS
# import json
# from langchain_core.documents import Document
# import os


# # represents the vectors from file in memory
# class EmbedVectors:
#     def __init__(self, vectorStorePath):
#         try:
#             # Load the data from faiss
#             st = time.time()
#             self.embeddings = LocalHuggingFaceEmbeddings(LLM_MODEL) # works via llama.cpp on CPU (custom glue code)
#             self.db = FAISS.load_local(vectorStorePath, self.embeddings)
#             et = time.time() - st
#             print(f"Loading vector database {vectorStorePath} took {et} seconds.")
#         except Exception as e:
#             raise Exception("Failed to load the model vector.") from e

#     def search(self, query, k=4, filter=None, search_type="similarity", return_scores=False, **kwargs):
#         """
#         Searches the FAISS database with different strategies.
#         :param query: The query string
#         :param k: Number of docs to return
#         :param filter: Optional metadata filter
#         :param search_type: "similarity", "mmr", "relevance"
#         :param return_scores: Whether to return scores
#         :param kwargs: Extra params like fetch_k, lambda_mult etc
#         :return: List of Documents or (Document, Score) tuples
#         """
#         #no filter when not supplied
#         if filter is None:
#             filter = {}
        
    
#         if search_type == "similarity":
#             #similarity_search_with_score, which allows you to return not only the documents 
#             #but also the distance score of the query to them. 
#             #The returned distance score is L2 distance. Therefore, a lower score is better.
#             if return_scores:
#                 method = "similarity_search_with_score"
#             else:
#                 method = "similarity_search"
#         elif search_type == "mmr":
#             method = "max_marginal_relevance_search"
#         elif search_type == "relevance":
#             method = "similarity_search_with_relevance_scores"
#         else:
#             raise ValueError("Invalid search type")
        
#         search_fn = getattr(self.db, method)
#         results = search_fn(query, filter=filter, k=k, **kwargs)
#         return results
