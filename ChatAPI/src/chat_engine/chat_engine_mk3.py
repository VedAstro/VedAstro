import os
import time
from langchain.chains.question_answering import load_qa_chain
from langchain.prompts import PromptTemplate
from langchain_community.chat_models.anyscale import ChatAnyscale

from langchain.chains import LLMChain
from langchain.prompts import PromptTemplate

SYSTEM_PROMPT = """use only text in context, context is my life description, answer in 70 words or less, avoid disclaimers"""
LLAMA_TEMPLATE = "[INST]<<SYS>>\n" + SYSTEM_PROMPT + \
    "{context}<</SYS>>\n\n{question}[/INST] "
PROMPT = PromptTemplate(template=LLAMA_TEMPLATE,
                        input_variables=["context", "question"])


DEFAULT_LLAMA_SEARCH_PROMPT = PromptTemplate(
    input_variables=["question"],
    template="""<<SYS>> \n You are an assistant tasked with improving Google search \
results. \n <</SYS>> \n\n [INST] Generate THREE Google search queries that \
are similar to this question. The output should be a numbered list of questions \
and each should have a question mark at the end: \n\n {question} [/INST]""")

DEFAULT_SEARCH_PROMPT = PromptTemplate(
    input_variables=["question"],
    template="""You are an assistant tasked with improving Google search \
results. Generate THREE Google search queries that are similar to \
this question. The output should be a numbered list of questions and each \
should have a question mark at the end: {question}""")


class ChatEngine3:
    
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
            raise Exception(f"Failed to load the Chat Engine.\n{e}") from e
    

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