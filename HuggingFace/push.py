from datasets import load_dataset

dataset = load_dataset("csv", data_files=r"C:\Users\ASUS\Desktop\Projects\VedAstro\HuggingFace\ml-table.csv")

dataset.push_to_hub("vedastro-org/all-planet-data-london")

