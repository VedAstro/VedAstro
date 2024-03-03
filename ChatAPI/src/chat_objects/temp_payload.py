import json
from typing import Optional
from pydantic import BaseModel
from datetime import datetime
from vedastro import *
import asyncio

class TempPayload(BaseModel):
    """Payload coming from client in POST request"""
    variation_name: Optional[str] = None
    query: Optional[str] = None
    birth_time: Optional[str] = None
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
    password: Optional[str] = None

    
    def get_birth_time(self) -> Time:
        # raw time string with location is parsed into correct astro time instance
        parsed_birth_time = Time.FromUrl(self.birth_time).GetAwaiter().GetResult()

        return parsed_birth_time