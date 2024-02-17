from datasets import load_dataset

# Specify the name of the repository on the Hugging Face Hub
dataset_name = "vedastro-org/all-planet-data-london"

# Load the dataset
dataset = load_dataset(dataset_name)

# Specify the local directory where you want to save the dataset
local_directory = r"C:\Users\ASUS\Desktop\Projects\VedAstro\HuggingFace\all-planet-data"

# Save the dataset to the local directory
dataset.save_to_disk(local_directory)

# Verify that the dataset was saved to the local directory
local_dataset = load_dataset(local_directory)
print(local_dataset)