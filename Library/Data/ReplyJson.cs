using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VedAstro.Library
{
    public class ReplyJson
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }

        public ReplyJson(string jsonString)
        {
            var chatCompletion = JsonConvert.DeserializeObject<ReplyJson>(jsonString);

            // Assign properties manually
            Id = chatCompletion?.Id;
            Object = chatCompletion?.Object;
            Created = chatCompletion?.Created ?? 0;
            Model = chatCompletion?.Model;
            Choices = chatCompletion?.Choices;
            Usage = chatCompletion?.Usage;
        }

        // Alternatively, you can use the DeserializeObject method to directly 
        // initialize the properties of the current instance
        public ReplyJson()
        {
        }

        public static ReplyJson FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<ReplyJson>(jsonString);
        }
    }

    public class Choice
    {
        public string FinishReason { get; set; }
        public int Index { get; set; }
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Content { get; set; }
        public string Role { get; set; }
        public List<object> ToolCalls { get; set; }
    }

    public class Usage
    {
        public int CompletionTokens { get; set; }
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}