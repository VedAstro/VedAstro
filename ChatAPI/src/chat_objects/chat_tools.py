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

