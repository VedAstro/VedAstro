import time

from XMLLoader import *
# from langchain_community.document_loaders import ReadTheDocsLoader
# from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_community.vectorstores import FAISS
from embeddings import LocalHuggingFaceEmbeddings

# Define constants

FAISS_INDEX_PATH = "faiss_index"

class Tools:
    @staticmethod
    def static_method1():
        loader = XMLLoader(filepath="embeddings/vedastro/data/HoroscopeDataList.xml")

        return "This is static method 1"

    @staticmethod
    def static_method2():
        return "This is static method 2"