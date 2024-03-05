from typing import Optional
from pydantic import BaseModel
from datetime import datetime
import asyncio

class ChatInstancePayload(BaseModel):
    """Payload coming from client in POST request"""
    input_text: Optional[str] = None
    temperature: Optional[float] = None
    instruction_text: Optional[str] = None
    password: Optional[str] = None
