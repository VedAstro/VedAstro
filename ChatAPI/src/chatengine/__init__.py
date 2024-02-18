# __init__.py

# Import classes from your package so they can
# be directly imported from the package
from .chat_tools import ChatTools
from .chat_engine import ChatEngine
from .embed_vectors import EmbedVectors
from .payload_body import PayloadBody
from .xml_loader import XMLLoader
from .local_huggingface_embeddings import LocalHuggingFaceEmbeddings


# Initialize a package variable
package_var = "Hello, World!"