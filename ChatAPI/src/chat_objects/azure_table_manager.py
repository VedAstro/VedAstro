from azure.data.tables import TableClient
import os

class AzureTableManager:
    connection_string = os.getenv("CENTRAL_API_STORAGE_CONNECTION_STRING")
    table_name = "ChatMessage"

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
