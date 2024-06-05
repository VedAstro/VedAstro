using System.Collections.Generic;

public class LlamaReplyJson
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public List<Choice> Choices { get; set; }
    public Usages Usage { get; set; }

    public LlamaReplyJson(string json)
    {
        var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
        this.Id = jsonObject["id"].ToString();
        this.Object = jsonObject["object"].ToString();
        this.Created = jsonObject["created"].ToObject<long>();
        this.Model = jsonObject["model"].ToString();
        this.Choices = jsonObject["choices"].ToObject<List<Choice>>();
        var completionTokens = jsonObject["usage"]["completion_tokens"].ToObject<int>();
        var promptTokens = jsonObject["usage"]["prompt_tokens"].ToObject<int>();
        var totalTokens = jsonObject["usage"]["total_tokens"].ToObject<int>();
        this.Usage = new Usages() { CompletionTokens = completionTokens, PromptTokens = promptTokens, TotalTokens = totalTokens };
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string FinishReason { get; set; }
        public object Logprobs { get; set; }
        public object StopReason { get; set; }
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
        public object ToolCalls { get; set; }
    }

    public class Usages
    {
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
        public int CompletionTokens { get; set; }
    }
}