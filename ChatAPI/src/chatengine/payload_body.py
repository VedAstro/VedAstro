from pydantic import BaseModel
from vedastro import *

# for easy async code
import asyncio

class PayloadBody(BaseModel):
    """Payload coming from client in POST request"""
    query: str
    birth_time: str = None
    name: str = None

    def get_birth_time(self) -> Time:
        # raw time string with location is parsed into correct astro time instance
        birth_time = Time.FromUrl(self.birth_time).GetAwaiter().GetResult()

        return birth_time
