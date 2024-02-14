from pydantic import BaseModel

# for easy async code
import asyncio

class PayloadBody(BaseModel):
    """Payload coming from client in POST request"""
    query: str
    birth_time: str = None
    name: str = None
