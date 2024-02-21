import os
from starlette.requests import Request
from typing import List, Optional, Any
from langchain.llms.utils import enforce_stop_tokens
from langchain_community.vectorstores import FAISS

# from langchain.llms import OpenAI
from langchain.chains.qa_with_sources import load_qa_with_sources_chain
from langchain.chains.question_answering import load_qa_chain
# from langchain import HuggingFacePipeline
from langchain.prompts import PromptTemplate
from langchain.chains import RetrievalQA
from transformers import pipeline as hf_pipeline
from transformers import (
    AutoModelForCausalLM,
    AutoModelForSeq2SeqLM,
    AutoTokenizer,
)
import torch
import time
from local_huggingface_embeddings import LocalHuggingFaceEmbeddings

from langchain_community.chat_models.anyscale import ChatAnyscale
from langchain_community.llms.anyscale import Anyscale
import asyncio
import async_generator
import aiohttp.helpers

SYSTEM_PROMPT = """use only text in context, context is a person's life description"""
LLAMA_TEMPLATE = "[INST]<<SYS>>\n" + SYSTEM_PROMPT + "{context}<</SYS>>\n\n{question}[/INST] "
PROMPT = PromptTemplate(template=LLAMA_TEMPLATE, input_variables=["context", "question"])


class ChatEngine2:
    def __init__(self, model_name):
        try:
            # 1 : load access to huggingface LLM via Anyscale
            st = time.time()

            # 2 : Set the most suitable LLM by name
            self.llm = ChatAnyscale(model_name=model_name, anyscale_api_key=os.environ["ANYSCALE_API_KEY"])

            # create how the data flows
            self.chain = load_qa_chain(llm=self.llm, chain_type="stuff", prompt=PROMPT)

            # Measure the time it took
            et = time.time() - st
            print(f"Loading HF model took {et} seconds.")
        except Exception as e:
            raise Exception("Failed to load the Chat Engine.") from e
    

    async def query(self, **kwargs):
        async def stream_handler():
            stream = self.chain.stream(
                input={"input_documents": kwargs["input_documents"], "question": kwargs["query"]},
                temperature=kwargs["temperature"],
                top_p=kwargs["top_p"],
                max_tokens=kwargs["max_tokens"],
                stop=kwargs["stop"],
            )
            for chunk in stream:
                yield chunk

        return stream_handler()

    async def process_and_print_stream(self, **kwargs):
        stream = await self.query(**kwargs)
        async for chunk in stream:
            print(chunk['text'])