# __init__.py
# Set engines here to try out variations
from .chat_engine_mk1 import ChatEngine1
from .chat_engine_mk2 import ChatEngine2
from .chat_engine_mk3 import ChatEngine3
from .chat_engine_mk4 import ChatEngine4
from .chat_engine_mk5 import ChatEngine5
from .chat_engine_mk6 import ChatEngine6
from local_huggingface_embeddings import LocalHuggingFaceEmbeddings


class ChatEngine:
    def __init__(self, variation_name):
        self.variation_name = variation_name
        self.variation_map = {
            "MK1": ChatEngine1,
            "MK2": ChatEngine2,
            "MK3": ChatEngine3,
            "MK4": ChatEngine4,
            "MK5": ChatEngine5,
            "MK6": ChatEngine6,
            # Add more variations as needed
        }

    def create_instance(self, *args):
        variation_class = self.variation_map.get(self.variation_name)
        if variation_class:
            new_instance = variation_class(*args)
            print(f"Experiment Number: {self.variation_name}")
            return new_instance
        else:
            raise ValueError(f"Unknown variation: {self.variation_name}")
