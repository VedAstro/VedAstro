import os
from langchain_core.documents import Document
import xmltodict

class XMLLoader:
    def __init__(self, filepath):
        dir_path = os.path.dirname(os.path.realpath(__file__))+'/..'
        print(dir_path)
        self.filepath = filepath
        self.embeddings_dir = dir_path+"/embeddings"

    # gets xml from file and returns parsed list
    def parse_xml(self):
        with open(self.filepath, 'r') as file:
            xml_content = file.read()
            json_content = xmltodict.parse(xml_content)
            # Get all elements inside the root
            root_elements = json_content['Root']['Event']
        return root_elements
    def load(self):
        # Parse the XML file, get all elements <Event> out as list
        allElements = self.parse_xml()
        
        # Iterate through each child element in <Root>
        docs = []
        for child in allElements:
            doc = self.create_document(child)
            docs.append(doc)

        return docs

    
    def create_document(self, xmlElement):
        # Create a new Document type
        text = xmlElement['Description'] if 'Description' in xmlElement else ''
        metadata={
            "name": xmlElement['Name'],
            "nature": xmlElement['Nature'],
            #"tag": xmlElement['Tag']
        }
        doc = Document(page_content=text, metadata=metadata)
        return doc



