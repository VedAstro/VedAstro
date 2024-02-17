#!/bin/bash

echo "Installing Chat API Server..."

# # Use pip to install the required packages
# # requirements.txt stored in root not src, for easy detection
# pip install -r requirements.txt

# #INSTALL .NET 8 SO VEDASTRO CAN WORK NATIVE
# # Update package lists
# sudo apt-get update

# # Install pre-requisites
# sudo apt-get install -y apt-transport-https
# sudo apt-get install -y dotnet-sdk-8.0

# # Enable Microsoft package feed
# wget https://packages.microsoft.com/config/debian/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
# sudo dpkg -i packages-microsoft-prod.deb

# # Install the .NET Runtime
# sudo apt-get update
# sudo apt-get install -y aspnetcore-runtime-8.0

# # Verify the installation
# dotnet --list-runtimes 