# import os
# from starlette.requests import Request
# from typing import List, Optional, Any
# from langchain.llms.utils import enforce_stop_tokens

# from langchain.chains.qa_with_sources import load_qa_with_sources_chain
# from langchain.chains.question_answering import load_qa_chain
# # from langchain import HuggingFacePipeline
# from langchain.prompts import PromptTemplate
# from langchain.chains import RetrievalQA
# from transformers import pipeline as hf_pipeline
# from transformers import (
#     AutoModelForCausalLM,
#     AutoModelForSeq2SeqLM,
#     AutoTokenizer,
# )
# import torch
# import time
# from local_huggingface_embeddings import LocalHuggingFaceEmbeddings

# from langchain_community.chat_models.anyscale import ChatAnyscale
# from langchain_community.llms.anyscale import Anyscale


# template = """
# <|SYSTEM|># StableLM Tuned (Alpha version)
# - You are a helpful, polite, fact-based agent for answering questions about Ray. 
# - Your answers include enough detail for someone to follow through on your suggestions. 
# <|USER|>
# If you don't know the answer, just say that you don't know. Don't try to make up an answer.
# Please answer the following question using the context provided. 

# CONTEXT: 
# {context}
# =========
# QUESTION: {question} 
# ANSWER: <|ASSISTANT|>"""
# PROMPT = PromptTemplate(template=template, input_variables=["context", "question"])


# class ChatEngine1:
#     def __init__(self, model_name):
#         try:
#             # 2 : load access to huggingface LLM via Anyscale
#             st = time.time()

#             # 'BAAI/bge-large-en-v1.5' - A large-scale English language model developed by BAAI.
#             # 'codellama/CodeLlama-70b-Instruct-hf' - A code generation model by CodeLlama with 70 billion parameters.
#             # 'mlabonne/NeuralHermes-2.5-Mistral-7B' - A model by mlabonne, part of the NeuralHermes series.
#             # 'meta-llama/Llama-2-7b-chat-hf' - A chatbot model by Meta-Llama with 7 billion parameters.
#             # 'Meta-Llama/Llama-Guard-7b' - A model by Meta-Llama, possibly used for content moderation.
#             # 'mistralai/Mixtral-8x7B-Instruct-v0.1' - An instruction following model by MistralAI.
#             # 'codellama/CodeLlama-34b-Instruct-hf' - A code generation model by CodeLlama with 34 billion parameters.
#             # 'HuggingFaceH4/zephyr-7b-beta' - A beta version of the Zephyr model by Hugging Face.
#             # 'meta-llama/Llama-2-70b-chat-hf' - A chatbot model by Meta-Llama with 70 billion parameters.
#             # 'mistralai/Mistral-7B-Instruct-v0.1' - An instruction following model by MistralAI.
#             # 'Open-Orca/Mistral-7B-OpenOrca' - A model by Open-Orca, possibly part of the Mistral series.
#             # 'thenlper/gte-large' - A large-scale model by TheNLP'er, possibly for general text generation.
#             # 'meta-llama/Llama-2-13b-chat-hf' - A chatbot model by Meta-Llama with 13 billion parameters.
#             self.llm = ChatAnyscale(model_name=model_name, anyscale_api_key=os.environ["ANYSCALE_API_KEY"])

#             # self.llm = StableLMPipeline.from_model_id(
#             #     model_id="stabilityai/stablelm-tuned-alpha-7b",
#             #     task="text-generation",
#             #     model_kwargs={"device_map": "auto", "torch_dtype": torch.float16},
#             # )

#             # create how the data flows
#             self.chain = load_qa_chain(llm=self.llm, chain_type="stuff", prompt=PROMPT)

#             et = time.time() - st
#             print(f"Loading HF model took {et} seconds.")
#         except Exception as e:
#             raise Exception("Failed to load the Chat Engine.") from e

#     def query(self, **kwargs):

#         result = self.chain.invoke({"input_documents": kwargs["input_documents"], "question": kwargs["query"]})

#         print(f"Result is: {result}")
#         return result["output_text"]