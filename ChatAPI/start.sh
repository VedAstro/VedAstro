#!/bin/bash

# Print out "Starting VedAstro Server"
echo "Starting VedAstro Server"

# Check if START_FILE_NAME is empty
if [ -z "$START_FILE_NAME" ]
then
  # note: since azure app will auto kill container if no start
  echo "No Start File Exiting Bye!"
  exit 1
fi

# start file should be file name, like "start-file-server.sh"
source $START_FILE_NAME
