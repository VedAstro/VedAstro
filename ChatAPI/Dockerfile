# Use the latest Debian image as the base
FROM debian:11-slim

# Update the system and install Python,
# curl --> for file server
RUN apt-get update && apt-get install -y python3 python3-pip python3-venv curl

# We need to set the host to 0.0.0.0 to allow outside access
ENV HOST 0.0.0.0

# Create a virtual environment
RUN python3 -m venv /opt/venv

# Activate the virtual environment
ENV PATH="/opt/venv/bin:$PATH"

#**********************COPY STUFF***********************
#script that starts server
COPY start.sh /

#settings for web file server
COPY filebrowser.json /

#scripts to install server dependencies
COPY install-file-server.sh /
COPY install-chatapi-server.sh /

#scripts to run server
COPY start-file-server.sh /
COPY start-chatapi-server.sh /


#**********************INSTALL STUFF***********************

#copy chatapi app python files before pip install (maintain same structure)
COPY src /src
COPY requirements.txt /
COPY nvidia-requirements.txt /

# install chatapi server
RUN ["/bin/bash", "install-chatapi-server.sh"]

RUN pip3 install -r requirements.txt

#clean up uneeded pip files
#NVIDIA is see you!
#RUN pip3 uninstall -r nvidia-requirements.txt -y

#RUN rm /path/to/large/unused/wheelfile.whl


# install file browser server (won't run till called via start-file-server.sh)
# Download and install File Browser Server
RUN ["/bin/bash", "install-file-server.sh"]


#**********************PORT STUFF***********************

#for fileserver
EXPOSE 80
#for chatapi
EXPOSE 8000


#**********************START STUFF***********************

# needs this else won't work in Azure
CMD ["/bin/bash", "start.sh"]



