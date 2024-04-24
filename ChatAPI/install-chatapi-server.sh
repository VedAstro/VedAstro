#!/bin/bash

echo "*****************Installing Chat API Server*****************"

# Use pip to install the required packages
# requirements.txt stored in root not src, for easy detection
#pip install -r requirements.txt

#INSTALL .NET SO VEDASTRO CAN WORK NATIVE
# Install necessary dependencies
apt-get install -y apt-transport-https gpg ca-certificates wget apt-utils

# Before you install .NET, run the following commands to add the Microsoft package signing key to your list of trusted keys and add the package repository.
wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install the runtime set version number here
apt-get update
apt-get install -y aspnetcore-runtime-7.0

# Verify the installation
dotnet --list-runtimes

echo "*****************CHAT SERVER INSTALL COMPLETE*****************"
