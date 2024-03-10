from azure.data.tables import TableClient, UpdateMode
import os
from .chat_tools import ChatTools
import time
import json
from typing import Any, Dict


class AzureTableManager:
    connection_string = os.getenv("CENTRAL_API_STORAGE_CONNECTION_STRING")

    # chat_raw_input :topic, text, session_id, rating, message_number
    @staticmethod
    def log_message(chat_raw_input) -> str:

        # timestamp = time.ctime(time.time())
        # timestamp_now_hash = ChatTools.generate_hash(timestamp)
        sender = chat_raw_input["sender"]
        message_text = chat_raw_input["text"]
        message_text_hash = ChatTools.generate_hash(message_text)[:20]
        topic_text = chat_raw_input["topic"]
        # topic_text_hash = ChatTools.remove_spaces(topic_text) # note: don't hash so can see for easy debug
        # note: don't hash so can see for easy debug
        topic_text_hash = ChatTools.generate_hash(topic_text)[:20]
        chat_session_id = chat_raw_input["session_id"]

        # question hash + topic hash (time url/book name)
        # NOTE:
        # always add a random number at back of each message hash,
        # this is done to allow same messages in same chat session to be treated uniquely
        randotron = ChatTools.random_id(4)
        # make the row key unique
        ques_topic_hash = f"{message_text_hash}-{topic_text_hash}-{randotron}"

        # make sure commands is properly converted if it exist
        if hasattr(chat_raw_input["command"], 'tolist'):
            chat_raw_input["command"] = chat_raw_input["command"].tolist()

        entity = {
            "PartitionKey": chat_session_id,  # chat session id (random id)
            # question hash + topic hash (time url/book name)
            "RowKey": ques_topic_hash,
            "sender": sender,
            "text": chat_raw_input["text"],
            "rating": int(chat_raw_input["rating"]),
            # to quickly find answer given a question, with 1 query instead of comparison
            "message_number": int(chat_raw_input["message_number"]),
            "object": json.dumps(chat_raw_input),
            "vector_store_hash": "$$$"  # coming soon!
        }


        # now add to table forever!
        AzureTableManager.write_to_table(entity)

        # we send this back to client so that can support rating system
        return ques_topic_hash

    # will return updated score
    @staticmethod
    def increase_contribution_score(user_id, new_contribution_score) -> int:
        table_client = TableClient.from_connection_string(
            AzureTableManager.connection_string, "ContributionScore")
        try:
            # Create a new entity object with the given user ID and new contribution score
            new_entity = {
                "PartitionKey": user_id,
                "RowKey": "",
                "score": int(new_contribution_score),
            }

            # check if user already has contributed before
            parameters = {"user_id": user_id}
            odata_query = "PartitionKey eq @user_id"
            msg_list = table_client.query_entities(
                query_filter=odata_query, parameters=parameters
            )

            # user already has record, so just add to existing score
            for contribution_score_row in msg_list:
                # If an entity already exists, increment its score by the new contribution score
                contribution_score_row["score"] += int(new_contribution_score)
                new_entity = contribution_score_row

                # Upsert the new or modified entity into the ContributionScore table
                table_client.upsert_entity(new_entity)

                print("Contribution Score updated successfully.")

                # return new total for user to see marks without waiting a week for it. Sejarah am I right people 😁
                return new_entity["score"]

            # control comes here only when not found
            print('No previous contribution. Adding new.')
            # Upsert the new or modified entity into the ContributionScore table
            table_client.upsert_entity(new_entity)

            return new_entity["score"]

        except Exception as e:
            print(f"Failed to update Contribution Score: {e}")
            return None

    @staticmethod
    def rate_message(session_id, ques_topic_hash, new_rating):
        table_client = TableClient.from_connection_string(
            AzureTableManager.connection_string, "ChatMessage")

        # find the exact message to rate, hence no need to break ques_topic hash
        parameters = {"session_id": session_id, "ques_topic_hash": ques_topic_hash}
        odata_query = "PartitionKey eq @session_id and RowKey eq @ques_topic_hash"
        msg_list = table_client.query_entities(
            query_filter=odata_query, parameters=parameters
        )

        for found_msg in msg_list:
            # update rating score with existing
            current_rating = found_msg['rating']
            updated_rating = current_rating + new_rating
            found_msg['rating'] = updated_rating

            # save back to DB
            table_client.upsert_entity(found_msg, UpdateMode.REPLACE)

            print("Message rated successfully.")
            return

        # control comes here only if no msg found
        print('Could not RATE! The specified message was not found')

    @staticmethod
    def write_to_table(entity):
        table_client = TableClient.from_connection_string(
            AzureTableManager.connection_string, "ChatMessage")
        try:
            inserted_entity = table_client.create_entity(entity=entity)
            print("Entity inserted successfully.")
        except Exception as e:
            print(f"Failed to insert entity: {e}")

    @staticmethod
    def is_same_question_in_same_session(partition_key, row_key):
        table_client = TableClient.from_connection_string(
            AzureTableManager.connection_string, "ChatMessage")

        # check if user already asked same question in same chat session
        # For example, if you have row keys that look like ‘KJV-C1-V1’, ‘KJV-C1-V2’,
        # ‘KJV-C1-V3’, ‘KJV-C2-V1’, ‘KJV-C2-V2’, ‘KJV-C2-V3’,
        # you can retrieve all entities with row keys that start with ‘KJV-C1’.
        # This is possible with a query like: PartitionKey eq 'your partition key' and (RowKey ge 'KJV-C1' and RowKey lt 'KJV-C2')
        parameters = {"partition_key": partition_key,
                      "comb_hash": row_key}  # todo needs testing

        # ge = greater than or equal
        odata_query2 = "PartitionKey eq @partition_key and RowKey ge @comb_hash and RowKey lt @comb_hash"
        # ge = greater than or equal
        odata_query = "PartitionKey eq @partition_key and RowKey ge @comb_hash"

        msg_list = table_client.query_entities(
            query_filter=odata_query, parameters=parameters
        )

        count = 0
        for index, value in enumerate(msg_list):
            print(value)
            count += 1

        # not first question
        if count >= 1:
            return True
        else:
            return False

    @staticmethod
    def read_from_table(partition_key, row_key):
        table_client = TableClient.from_connection_string(
            AzureTableManager.connection_string, AzureTableManager.table_name)
        try:
            entity = table_client.get_entity(partition_key, row_key)
            return entity
        except Exception as e:
            print(f"Failed to read entity: {e}")
