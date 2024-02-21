import json
from langchain_core.documents import Document
from .xml_loader import XMLLoader
from typing import List
from .payload_body import PayloadBody
from typing import List, Dict, Union

class ChatTools:        
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

