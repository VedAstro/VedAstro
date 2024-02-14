#!/bin/bash

echo "Starting Chat API Server..."

# goto the place where app files are stored,
# this was copied during image build
cd /src

# start forever running server
# 0.0.0.0 to allow all at port 8000
uvicorn main:app --host 0.0.0.0 --port 8000
