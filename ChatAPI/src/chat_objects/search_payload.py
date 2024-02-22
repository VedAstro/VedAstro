from typing import Optional
from pydantic import BaseModel
from datetime import datetime
from vedastro import *
import asyncio


class SearchPayload(BaseModel):
    """Payload coming from client in POST request"""
    query: Optional[str] = None
    birth_time: Optional[str] = None
    llm_model_name: Optional[str] = None
    search_type: Optional[str] = None
    lambda_mult: Optional[float] = None
    
    def get_birth_time(self) -> Time:
        # raw time string with location is parsed into correct astro time instance
        parsed_birth_time = Time.FromUrl(self.birth_time).GetAwaiter().GetResult()

        return parsed_birth_time