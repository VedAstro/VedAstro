import json
from typing import Optional
from pydantic import BaseModel
from datetime import datetime
from vedastro import *
import asyncio

class ChatPayload(BaseModel):
    """Payload coming from client in POST request"""
    variation_name: Optional[str] = None
    query: Optional[str] = None
    text: Optional[str] = None
    topic: Optional[str] = None # birth time url or book name
    name: Optional[str] = None
    llm_model_name: Optional[str] = None
    chat_model_name: Optional[str] = None
    search_type: Optional[str] = None
    temperature: Optional[float] = None
    max_tokens: Optional[float] = None
    top_p: Optional[float] = None
    stop: Optional[str] = None
    fetch_k: Optional[float] = None
    lambda_mult: Optional[float] = None
    
    def __init__(self, json_string: str = None):
        if json_string:
            payload = json.loads(json_string)
            self.__dict__.update({key.lower(): value for key, value in payload.items()})
            try:
                super().__init__(**self.__dict__)
            except Exception as e:
                print("Validation Error:", e)
        else:
            super().__init__()

    #marked for oblivion
    def get_birth_time(self) -> Time:
        # raw time string with location is parsed into correct astro time instance
        time_string = self.topic
        parsed_birth_time = Time.FromUrl(time_string).GetAwaiter().GetResult()

        return parsed_birth_time