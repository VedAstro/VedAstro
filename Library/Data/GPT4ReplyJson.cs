using System.Collections.Generic;

public class Gpt4ReplyJson
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public List<Choice> Choices { get; set; }
    public List<PromptFilterResult> PromptFilterResults { get; set; }
    public string SystemFingerprint { get; set; }
    public Usages Usage { get; set; }

    public Gpt4ReplyJson(string json)
    {
        var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
        this.Id = jsonObject["id"].ToString();
        this.Object = jsonObject["object"].ToString();
        this.Created = jsonObject["created"].ToObject<long>();
        this.Model = jsonObject["model"].ToString();
        this.Choices = jsonObject["choices"].ToObject<List<Choice>>();
        this.PromptFilterResults = jsonObject["prompt_filter_results"].ToObject<List<PromptFilterResult>>();
        this.SystemFingerprint = jsonObject["system_fingerprint"].ToString();
        var completionTokens = jsonObject["usage"]["completion_tokens"].ToObject<int>();
        var promptTokens = jsonObject["usage"]["prompt_tokens"].ToObject<int>();
        var totalTokens = jsonObject["usage"]["total_tokens"].ToObject<int>();
        this.Usage = new Usages() { CompletionTokens = completionTokens, PromptTokens = promptTokens, TotalTokens = totalTokens };
    }

    public class Choice
    {
        public ContentFilterResults ContentFilterResults { get; set; }
        public string FinishReason { get; set; }
        public int Index { get; set; }
        public object Logprobs { get; set; }
        public Message Message { get; set; }
    }

    public class ContentFilterResults
    {
        public Hate Hate { get; set; }
        public SelfHarm SelfHarm { get; set; }
        public Sexual Sexual { get; set; }
        public Violence Violence { get; set; }
    }

    public class Hate
    {
        public bool Filtered { get; set; }
        public string Severity { get; set; }
    }

    public class SelfHarm
    {
        public bool Filtered { get; set; }
        public string Severity { get; set; }
    }

    public class Sexual
    {
        public bool Filtered { get; set; }
        public string Severity { get; set; }
    }

    public class Violence
    {
        public bool Filtered { get; set; }
        public string Severity { get; set; }
    }

    public class Message
    {
        public string Content { get; set; }
        public string Role { get; set; }
    }

    public class PromptFilterResult
    {
        public int PromptIndex { get; set; }
        public ContentFilterResults ContentFilterResults { get; set; }
    }

    public class Usages
    {
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
        public int CompletionTokens { get; set; }
    }
}