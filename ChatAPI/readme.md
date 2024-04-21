
# ARCHITECTURE
```
                       simple RAG pipeline consisting of top-k retrieval + LLM synthesis.                                                                        
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                           FILTER PREDICTIONS                                                                                                    
                     ┌──────────────────────────────────────────────────────────────┐                                                                            
                     │                                                              │                                                                            
                     │ - FAISS similarity & mmr search                              │                                                                            
                     │                                                              │                                                                            
                     │ - Llama-Index :                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     │                                                              │                                                                            
                     └──────────────────────────────────────────────────────────────┘                                                                            
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                                                                                                                                                 
                                                                ~30 predictions             LLAMA-INDEX                                                          
                                                             with query match score    ┌───────────────────┐                                                     
             QUERY ─────────────────► FILTER PREDICTIONS  ───────────────────────────► │                   │                                                     
               ▲                               ▲                     OUT               │                   │                                                     
               │                               │                                       │                   │  ~5 CHAT/COMPLETION CALLS                           
               │                               │                                       │                   ├────────────────────────────►                        
USER ──────────┤                               │                                       │                   │                                LLM CALL TO          
               │                               │~40 predictions                        │                   ├────────────────────────────►   OPENAI/AZURE/ANYSCALE
               │                               │                                       │                   │                                                     
               │                               │                                       │                   ├────────────────────────────►                        
               ▼                               │                                       │                   │                                                     
           BIRTH TIME ───────────────► HOROSCOPE PREDICTIONS                           │                   │                                                     
                            via        - list of text                                  │                   │                                                     
                       VedAstro Lib                                                    └───────────────────┘                                                     
```


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

# OPTION 1
## push code and build in azure (sometimes faster)

`az acr build --registry vedastrochatapi --image chat-api . --verbose`
` az acr build --registry vedastrochatapi --image chat-api .`

NOTE: live ACA server needs to be restarted for new image to kick in

# OPTION 2
## build local then push to Azure

 credentials from Access keys blade.

`docker login vedastrochatapi.azurecr.io`

link local image with online URI

`docker tag chat-api vedastrochatapi.azurecr.io/chat-api`

upload image to Azure

`docker push vedastrochatapi.azurecr.io/chat-api`

# prompts 
craft a response that incorporates the strengths of Response A and Response B, addresses their shortcomings based on the ratings, and leverages insights from the provided ratings:


# start file server
1. change env start file
2. visit via browser at http://localhost/

# LLAMA-INDEX notes
- agents screw up vedic accuracy when answering direct questions, probably because to many absrtaction layer (loose control over fine detail)
- use agents for follow up questions where injecting previous question is crucial with proper prompt cleaning
- if use direct complete call for Azure some success, but prompts must be perfect and at times very bad answers (llama-index) will stop wrong answers
- as such stick with multiple query engine use

# Free AI chat, really?
Yes, we're doing this to improve vedic prediction quality for the common good of human race.
So we will focus on keeping this free and you focus on giving feedback.
Lets improve **Vedic Astrology AI Chat** together!