

# required for API
from fastapi import Body, FastAPI

import json  # for JSON parsing and packing

# for easy async code
import asyncio

# for absolute project paths
import os

from api_types import *

# astro chat engines
from chat_engine import Tools


# instances of chat engines 
# NOTE: keep multiple in global for lazy init & for easy experimentation
# on server cold start, will help speed other faster calls
chat_engine = None
chat_engine2 = None

# init app to handle HTTP requests
app = FastAPI(title="Chat API")


@app.get("/")
def home():
    return {"Welcome to Chat API"}

# receives HTTP request, processes and returns response
@app.post('/llmsearch')
async def chat(payload: PayloadBody):
    global chat_engine

    # lazy load for speed
    if chat_engine is None: chat_engine = ChatEngine()
    
    # get response from chat engine
    result = chat_engine.chat(payload)

    # Return JSON response
    return result




