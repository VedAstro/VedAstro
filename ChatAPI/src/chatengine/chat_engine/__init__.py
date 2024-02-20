# __init__.py

from .chat_engine_mk1 import ChatEngine1
from .chat_engine_mk2 import ChatEngine2

class ChatEngine:
    def __init__(self, variation_name):
        self.variation_name = variation_name
        self.variation_map = {
            "MK1": ChatEngine1,
            "MK2": ChatEngine2,
            # Add more variations as needed
        }

    def create_instance(self, *args):
        variation_class = self.variation_map.get(self.variation_name)
        if variation_class:
            new_instance = variation_class(*args)
            print(f"Created new instance of MK: {self.variation_name}")
            return new_instance
        else:
            raise ValueError(f"Unknown variation: {self.variation_name}")
