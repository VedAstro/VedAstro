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

RUN pip3 install -r requirements.txt

# Install .NET 7
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        apt-transport-https \
        dirmngr \
        gnupg \
        ca-certificates \
    && wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y dotnet-sdk-7.0

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



