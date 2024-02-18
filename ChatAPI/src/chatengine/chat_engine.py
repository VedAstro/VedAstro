import os
from starlette.requests import Request
from typing import List, Optional, Any
from langchain.llms.utils import enforce_stop_tokens

from langchain.vectorstores import FAISS
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
from .local_huggingface_embeddings import LocalHuggingFaceEmbeddings

from langchain_community.chat_models.anyscale import ChatAnyscale
from langchain_community.llms.anyscale import Anyscale

FAISS_INDEX_PATH = "faiss_index_fast"


template = """
<|SYSTEM|># StableLM Tuned (Alpha version)
- You are a helpful, polite, fact-based agent for answering questions about Ray. 
- Your answers include enough detail for someone to follow through on your suggestions. 
<|USER|>
If you don't know the answer, just say that you don't know. Don't try to make up an answer.
Please answer the following question using the context provided. 

CONTEXT: 
{context}
=========
QUESTION: {question} 
ANSWER: <|ASSISTANT|>"""
PROMPT = PromptTemplate(template=template, input_variables=["context", "question"])


class ChatEngine:
    def __init__(self, vectorStorePath, model_name):
        try:
            # 1 : Load the data from faiss
            st = time.time()
            self.embeddings = LocalHuggingFaceEmbeddings(model_name) # works via llama.cpp on CPU (custom glue code)
            self.db = FAISS.load_local(vectorStorePath, self.embeddings)
            et = time.time() - st
            print(f"Loading vector database {vectorStorePath} took {et} seconds.")
        except Exception as e:
            raise Exception("Failed to load the model vector.") from e
        
        # 2 : load LLM huggingface modal
        st = time.time()

        # 'BAAI/bge-large-en-v1.5' - A large-scale English language model developed by BAAI.
        # 'codellama/CodeLlama-70b-Instruct-hf' - A code generation model by CodeLlama with 70 billion parameters.
        # 'mlabonne/NeuralHermes-2.5-Mistral-7B' - A model by mlabonne, part of the NeuralHermes series.
        # 'meta-llama/Llama-2-7b-chat-hf' - A chatbot model by Meta-Llama with 7 billion parameters.
        # 'Meta-Llama/Llama-Guard-7b' - A model by Meta-Llama, possibly used for content moderation.
        # 'mistralai/Mixtral-8x7B-Instruct-v0.1' - An instruction following model by MistralAI.
        # 'codellama/CodeLlama-34b-Instruct-hf' - A code generation model by CodeLlama with 34 billion parameters.
        # 'HuggingFaceH4/zephyr-7b-beta' - A beta version of the Zephyr model by Hugging Face.
        # 'meta-llama/Llama-2-70b-chat-hf' - A chatbot model by Meta-Llama with 70 billion parameters.
        # 'mistralai/Mistral-7B-Instruct-v0.1' - An instruction following model by MistralAI.
        # 'Open-Orca/Mistral-7B-OpenOrca' - A model by Open-Orca, possibly part of the Mistral series.
        # 'thenlper/gte-large' - A large-scale model by TheNLP'er, possibly for general text generation.
        # 'meta-llama/Llama-2-13b-chat-hf' - A chatbot model by Meta-Llama with 13 billion parameters.
        self.llm = ChatAnyscale(model_name="meta-llama/Llama-2-70b-chat-hf", anyscale_api_key="")

        # Initialize Anyscale model
        #self.llm = Anyscale(model=ANYSCALE_MODEL, api_key=ANYSCALE_ENDPOINT_TOKEN)

        # self.llm = StableLMPipeline.from_model_id(
        #     model_id="stabilityai/stablelm-tuned-alpha-7b",
        #     task="text-generation",
        #     model_kwargs={"device_map": "auto", "torch_dtype": torch.float16},
        # )
        et = time.time() - st
        print(f"Loading HF model took {et} seconds.")

        self.chain = load_qa_chain(llm=self.llm, chain_type="stuff", prompt=PROMPT)

    def qa(self, query, input_documents):
        from langchain_core.documents import Document
        # input_documents = [
        #     Document(page_content="Hello world!"),
        #     Document(page_content="This is some sample text") 
        # ]
        result = self.chain({"input_documents": input_documents, "question": query})

        print(f"Result is: {result}")
        return result["output_text"]
