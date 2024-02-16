import json
from langchain_core.documents import Document
from .xml_loader import XMLLoader
from typing import List
from .payload_body import PayloadBody

class ChatTools:        
    # given a list of documents will convert to JSON
    @staticmethod
    def doc_with_score_to_dict(docs: List[Document]) -> str:
        docs_list = []
        for doc in docs:
            doc_dict = {
                "page_content": doc[0].page_content,
                "metadata": doc[0].metadata,
                "score": float(doc[1]),
            }
            docs_list.append(doc_dict)
        #json_str = json.dumps(docs_list)
        return docs_list
