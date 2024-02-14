# image build,
when docker file is run to create the image
it is done locally, then image is manually pushed to "azure image registry"
during build process, the linux image is modified as needed

# server startup
server starts reads env and starts a sh file that will start a server app.
```START_FILE_NAME=start-llm-server.sh```

### build image
```docker build -t chat-api .```

### run image
use local env, and port 8000 and 80
```docker run -p 8000:8000 -p 80:80 --env-file .env -d chat-api```

visit ```http://localhost``` with default password (admin) 


`pip freeze > /src/requirements.txt`
