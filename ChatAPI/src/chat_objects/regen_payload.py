from typing import Optional
from pydantic import BaseModel
from datetime import datetime
from vedastro import *
import asyncio

class RegenPayload(BaseModel):
    """Payload coming from client in POST request"""
    llm_model_name: Optional[str] = None
