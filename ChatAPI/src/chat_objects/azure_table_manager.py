from azure.data.tables import TableClient
import os
from .chat_tools import ChatTools
import time
import json

class AzureTableManager:
    connection_string = os.getenv("CENTRAL_API_STORAGE_CONNECTION_STRING")
    table_name = "ChatMessage"

    # chat_raw_input :topic, text, session_id, rating, message_number
    @staticmethod
    def log_message(chat_raw_input)-> str:
        
        #timestamp = time.ctime(time.time())
        #timestamp_now_hash = ChatTools.generate_hash(timestamp)
        sender = chat_raw_input["sender"]
        message_text = chat_raw_input["text"]
        message_text_hash = ChatTools.generate_hash(message_text)
        topic_text = chat_raw_input["topic"]
        topic_text_hash = ChatTools.generate_hash(topic_text)
        chat_session_id = chat_raw_input["session_id"]

        # question hash + topic hash (time url/book name)
        ques_topic_hash = f"{message_text_hash}-{topic_text_hash}"

        entity = {
            "PartitionKey": chat_session_id, #chat session id (random id)
            "RowKey": ques_topic_hash, # question hash + topic hash (time url/book name)
            "sender": sender,
            "text": chat_raw_input["text"],
            "rating": int(chat_raw_input["rating"]),
            "message_number" : int(chat_raw_input["message_number"]), # to quickly find answer given a question, with 1 query instead of comparison
            "object": json.dumps(chat_raw_input),
            "vector_store_hash" : "$$$" # coming soon!
        }
        
        AzureTableManager.write_to_table(entity)

        # we send this back to client so that can support rating system
        return ques_topic_hash

    # will return updated score
    @staticmethod
    def increase_contribution_score(user_id, new_contribution_score)->int:
        table_client = TableClient.from_connection_string(AzureTableManager.connection_string, "ContributionScore")
        try:
            # Create a new entity object with the given user ID and new contribution score
            new_entity = {
                "PartitionKey": user_id,
                "RowKey": "",
                "score": int(new_contribution_score),
            }
            
            # Check if an entity already exists for the given user ID
            entities = table_client.query_entities(partition_key=user_id)
            existing_entity = next((ent for ent in entities if ent["RowKey"] == ""), None)
            
            if existing_entity is not None:
                # If an entity already exists, increment its score by the new contribution score
                existing_entity["score"] += int(new_contribution_score)
                new_entity = existing_entity
                
            # Upsert the new or modified entity into the ContributionScore table
            table_client.upsert_entity(new_entity)
            
            print("Contribution Score updated successfully.")
            return new_entity["score"]
        except Exception as e:
            print(f"Failed to update Contribution Score: {e}")
            return None            
   

    @staticmethod
    def rate_message(session_id, ques_topic_hash, new_rating):
        table_client = TableClient.from_connection_string(AzureTableManager.connection_string, "ChatMessage")

        entity = None
        partition_query = f"PartitionKey eq '{session_id}'"
        row_query = f"RowKey eq '{ques_topic_hash}'"

        for item in table_client.query_entities(partition_key=session_id, row_key=""):
            if ques_topic_hash == item['RowKey']:
                entity = item
                break

        if not entity:
            print('Could not RATE! The specified message was not found')

        current_rating = entity['rating']
        updated_rating = current_rating + new_rating

        entity['rating'] = updated_rating
        entity['message_number'] += 1

        table_client.update_entity(entity, overwrite=True)

        print("Entity rated successfully.")


    @staticmethod
    def write_to_table(entity):
        table_client = TableClient.from_connection_string(AzureTableManager.connection_string, AzureTableManager.table_name)
        try:
            inserted_entity = table_client.create_entity(entity=entity)
            print("Entity inserted successfully.")
        except Exception as e:
            print(f"Failed to insert entity: {e}")

    @staticmethod
    def read_from_table(partition_key, row_key):
        table_client = TableClient.from_connection_string(AzureTableManager.connection_string, AzureTableManager.table_name)
        try:
            entity = table_client.get_entity(partition_key, row_key)
            return entity
        except Exception as e:
            print(f"Failed to read entity: {e}")
