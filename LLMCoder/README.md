# LLMCodes
LLMCodes is a simple graphical user interface (GUI) application designed 
to facilitate code generation using Large Language Models (LLMs).
This project allows users to easily switch between multiple LLMs,
providing a versatile coding assistant experience.

## Features

- **Multiple LLM Support**: Easily switch between different LLMs like GPT-4, Phi3, MistralNemo, MetaLlama, and Cohere.
- **Local Execution**: Run the application locally with unlimited timeout HTTP calls, offering more flexibility and control than online alternatives.
- **Past Prompts**: Load and reuse previous prompts from your chat history.
- **Simple User Interface**: A straightforward and user-friendly interface for a smooth experience.

![image](https://github.com/user-attachments/assets/0678763d-5a82-4130-a7bd-7517ed68d329)
![image](https://github.com/user-attachments/assets/b268c97c-5f8e-403b-a935-cd02ae5af204)
![image](https://github.com/user-attachments/assets/fd0e6286-53a4-428a-a03b-46cacced2f41)

## Installation

1. **Clone the Repository**:

```sh
Copy code
git clone https://github.com/your-username/LLMCodes.git
cd LLMCodes
```

2. Set Up Configuration:

Create a secrets.json file in the root directory with your API keys and endpoints. Use the following structure:
```json
Copy code
{
  "GPT4oEndpoint": "your-gpt4o-endpoint",
  "GPT4oApiKey": "your-gpt4o-api-key",
  "Phi3medium128kinstructEndpoint": "your-phi3medium128kinstruct-endpoint",
  "Phi3medium128kinstructApiKey": "your-phi3medium128kinstruct-api-key",
  "MistralNemo128kEndpoint": "your-mistralnemo128k-endpoint",
  "MistralNemo128kApiKey": "your-mistralnemo128k-api-key",
  "MetaLlama31405BEndpoint": "your-metallama31405b-endpoint",
  "MetaLlama31405BApiKey": "your-metallama31405b-api-key",
  "CohereCommandRPlusEndpoint": "your-coherecommandrplus-endpoint",
  "CohereCommandRPlusApiKey": "your-coherecommandrplus-api-key"
}
```

## Build and Run:

Open the solution in Visual Studio or your preferred .NET IDE.
Build the project and run the application.

## Usage
Selecting an LLM:

Use the dropdown menu to select your preferred LLM from the available options.
Entering a Prompt:

Type your code-related question or prompt in the input box and click "Send".
Viewing Responses:

The LLM's response will appear in the output box.
Managing Chat History:

Previous user prompts can be selected from the dropdown to reuse or edit.
Customizing Settings:

The settings in secrets.json can be adjusted to update API keys and endpoints.

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.