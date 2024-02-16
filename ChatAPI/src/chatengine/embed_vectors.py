import time

from .xml_loader import XMLLoader
# from langchain_community.document_loaders import ReadTheDocsLoader
# from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_community.vectorstores import FAISS
from embeddings import LocalHuggingFaceEmbeddings
import json
from langchain_core.documents import Document
import os

# Define constants
FAISS_INDEX_PATH = "faiss_index"
# model set in docker so can change at runtime, default to below
LLM_MODEL = os.getenv('LLM_MODEL', "sentence-transformers/all-MiniLM-L6-v2") 


# represents the vectors from file in memory
class EmbedVectors:
    def __init__(self, vectorStorePath):
        # Load the data from faiss
        st = time.time()
        self.embeddings = LocalHuggingFaceEmbeddings(LLM_MODEL) # works via llama.cpp on CPU (custom glue code)
        self.db = FAISS.load_local(vectorStorePath, self.embeddings)
        et = time.time() - st
        print(f"Loading database took {et} seconds.")

    def search(self, query, k=4, filter=None):
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


