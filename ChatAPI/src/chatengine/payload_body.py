from typing import Optional
from pydantic import BaseModel
from datetime import datetime
from vedastro import *
import asyncio

class PayloadBody(BaseModel):
    """Payload coming from client in POST request"""
    query: Optional[str] = None
    birth_time: Optional[str] = None
    name: Optional[str] = None
    llm_model_name: Optional[str] = None

    def get_birth_time(self) -> Time:
        # raw time string with location is parsed into correct astro time instance
        parsed_birth_time = Time.FromUrl(self.birth_time).GetAwaiter().GetResult()

        return parsed_birth_time