from typing import Optional
from pydantic import BaseModel
from datetime import datetime
import asyncio

class AutoReply():
    query_map_list = None

    def __init__(self, query_map_list):
        self.query_map_list = query_map_list
    
    @staticmethod
    def techinical_auto_reply():
        return """
        I'm the programmed conciousness of many Vedic Astrologers.
        You can call me Vignes, but honestly I don't have a name.
        I'm merely an a collection of ideas floating in the electrical pulses of silicon.
        I'm not just another AI modal. I do slowly analyse, learn and improve my prediction.
        So please give me feedback so I can better serve you.
        """

    def try_generate_auto_reply(self):
        #based on query map
        for q_object in self.query_map_list:
            #only process the last node that has functions to call
            if "call_functions" in q_object:
                #execute each func till end 1 by 1
                for func_name in q_object["call_functions"]:
                    bbb = getattr(AutoReply, func_name)()
                    return bbb
    
        

    
