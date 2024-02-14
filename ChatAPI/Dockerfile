FROM python:3.11

# We need to set the host to 0.0.0.0 to allow outside access
ENV HOST 0.0.0.0

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

# install chatapi server (basically pip install)
RUN ["/bin/bash", "install-chatapi-server.sh"]

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



