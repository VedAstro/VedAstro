using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json.Linq;
using VedAstro.Library;

namespace VedAstro.Library;

/// <summary>
/// </summary>
public class PresetQuestionEmbeddingsEntity : ITableEntity
{

    /// <summary>
    /// input query
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// category
    /// </summary>
    public string RowKey { get; set; }


    public string Embeddings { get; set; }


    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }

    public double[] GetEmbeddingsArray()
    {
        var docEmbedding = JArray.Parse(this.Embeddings);
        var newQueryEmbedsgg = docEmbedding.Select(jv => (double)jv).ToArray();

        return newQueryEmbedsgg;
    }
}