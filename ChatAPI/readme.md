# image build,
when docker file is run to create the image
it is done locally, then image is manually pushed to "azure image registry"
during build process, the linux image is modified as needed

# server startup
server starts reads env and starts a sh file that will start a server app.
```START_FILE_NAME=start-llm-server.sh```

# build image
```docker build -t chat-api .```

# run image
use local env, and port 8000 and 80
```docker run -p 8000:8000 -p 80:80 --env-file .env -d chat-api```

visit ```http://localhost``` with default password (admin) 


`pip freeze > /src/requirements.txt`

# set correct sub first
` az account set --subscription c4a47a40-5870-4dbc-91b8-6c967556bfeb`

# push code and build in azure (sometimes faster)
` az acr build --registry vedastrochatapi --image chat-api .`

# build local then push to Azure

 credentials from Access keys blade.

`docker login vedastrochatapi.azurecr.io`

link local image with online URI

`tag chat-api vedastrochatapi.azurecr.io/chat-api`

upload image to Azure

`docker push vedastrochatapi.azurecr.io/hello-world`
# prompts 
craft a response that incorporates the strengths of Response A and Response B, addresses their shortcomings based on the ratings, and leverages insights from the provided ratings: