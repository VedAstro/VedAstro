using System;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;

namespace VedAstro.Library;

public class ChatMessageEntity : ITableEntity
{

    public ChatMessageEntity()
    {
        
    }

    public ChatMessageEntity(string sessionId, Time birthTime, string text, string sender,  string userId)
    {
        PartitionKey = sessionId;

        //generate row key
        var textHash = Tools.GetStringHashCodeMD5(text, 15);
        var birthTimeSimple = birthTime.ToUrl().Replace("/","-");
        var rawRowKey = $"{textHash}{birthTimeSimple}-{Tools.GenerateId(5)}";
        string cleanRowKey = System.Text.RegularExpressions.Regex.Replace(rawRowKey, @"[^a-zA-Z0-9\-\.\/_]", "");
        RowKey = cleanRowKey;

        UserId = userId;
        Sender = sender;
        Text = text;

        //NOTE: - internal ease use value, calculated and handled within
        //      - if new message will return 0
        int messageNumber = ChatAPI.GetLastMessageNumberNumberFromSessionId(sessionId); //autofill new session id

        MessageNumber = messageNumber + 1; // add 1 for next message
    }

    /// <summary>
    /// session id
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// text hash + user birth
    /// </summary>
    public string RowKey { get; set; }
    
    
    public string Sender { get; set; }
    
    public string Text { get; set; }
    public int Rating { get; set; }
    public int MessageNumber { get; set; }
    public string UserId { get; set; }


    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }



}