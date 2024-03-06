import os
import time
# from langchain.chains.question_answering import load_qa_chain
# from langchain.prompts import PromptTemplate
# from langchain_community.chat_models.anyscale import ChatAnyscale

# from langchain.chains import LLMChain
# from langchain.prompts import PromptTemplate

# import os module & the OpenAI Python library for calling the OpenAI API
import openai
import json
import os
from langchain_openai import AzureChatOpenAI
from langchain.schema import HumanMessage
from langchain.chains import LLMChain
from langchain.prompts import PromptTemplate

from langchain.prompts.chat import (
    ChatPromptTemplate,
    SystemMessagePromptTemplate,
    HumanMessagePromptTemplate,
)


class ChatEngine4:

    def __init__(self, model_name):
        try:
            from llama_index.embeddings.openai import OpenAIEmbedding
            from llama_index.core.node_parser import SentenceSplitter
            from llama_index.llms.openai import OpenAI
            from llama_index.core import Settings

            # 1 : load access to huggingface LLM via Anyscale
            st = time.time()

            # The base URL for your Azure OpenAI resource. e.g. "https://<your resource name>.openai.azure.com"
            openai_api_base = "https://openaimodelserver.openai.azure.com/openai/deployments/vedastro"
            text_embedder_api_base = "https://openaimodelserver.openai.azure.com/openai/deployments/text-embedder"

            # API version e.g. "2023-07-01-preview"
            openai_api_version = "2024-02-15-preview"

            # The name of your Azure OpenAI deployment chat model. e.g. "gpt-35-turbo-0613"
            deployment_name = "vedastro"

            # The API key for your Azure OpenAI resource.
            openai_api_key = os.getenv("AZURE_OPENAI_API_KEY")

            # This is set to `azure`
            openai_api_type = "azure"

            # 2 : Set the most suitable LLM by name
            self.llm = AzureChatOpenAI(
                openai_api_base=openai_api_base,
                openai_api_version=openai_api_version,
                openai_api_key=openai_api_key,
                openai_api_type=openai_api_type,
            )

            #self.embed_model = OpenAIEmbedding(model="text-embedding-3-small")

            text_encoder = OpenAIEmbedding(model="text-embedding-ada-002",api_key=os.getenv("AZURE_OPENAI_API_KEY"), api_base=text_embedder_api_base)
            self.embed_model = text_encoder

            Settings.llm = self.llm
            Settings.embed_model = self.embed_model
            Settings.node_parser = SentenceSplitter(
                chunk_size=512, chunk_overlap=20)
            Settings.num_output = 512
            Settings.context_window = 3900

            # Measure the time it took
            et = time.time() - st
            print(f"Loading HF model took {et} seconds.")
        except Exception as e:
            raise Exception(f"Failed to load the Chat Engine.\n{e}") from e

    def query(self, **kwargs):
        from llama_index.core.data_structs import Node
        from llama_index.core.schema import NodeWithScore
        from llama_index.core import get_response_synthesizer

        # configure how the query will be processed
        # notes : - SIMPLE_SUMMARIZE : works good (suspect loss of nodes)
        #        - COMPACT : works good (good comparison)
        #        - COMPACT_ACCUMULATE, ACCUMULATE : not needed here, checks each prediction on own
        response_synthesizer = get_response_synthesizer(response_mode="compact")

        user_question = kwargs["query"]
        
        # get response based on nodes as context
        response = response_synthesizer.synthesize(
            f"{user_question}, use only text in context, context is a person's life description",
            nodes=kwargs["input_documents"]
        )

        return response
