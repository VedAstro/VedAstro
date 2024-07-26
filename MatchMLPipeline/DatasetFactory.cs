using Azure;
using Azure.Data.Tables;
using MimeDetective.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScottPlot.Drawing.Colormaps;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using VedAstro.Library;

namespace MatchMLPipeline;

public static class DatasetFactory
{

    static string MetaLlama_3_70BEndpoint = "https://Meta-Llama-3-70B-Instruct-ydbrc-serverless.westus.inference.ai.azure.com/v1/chat/completions";

    //static TableClient personListClient = new TableClient(centralStorageConnection, "PersonList");
    public static TableClient personListClient_LocalEmulator = new TableClient(centralStorageConnection_LocalEmulator, "PersonList");
    public static TableClient marriageInfoDatasetClient_LocalEmulator = new TableClient(centralStorageConnection_LocalEmulator, "MarriageInfoDataset");
    public static TableClient bodyInfoDatasetClient_LocalEmulator = new TableClient(centralStorageConnection_LocalEmulator, "BodyInfoDataset");
    //static TableClient personNameEmbeddingsClient = new TableClient(centralStorageConnection, "PersonNameEmbeddings");
    public static TableClient personNameEmbeddingsClient_LocalEmulator = new TableClient(centralStorageConnection_LocalEmulator, "PersonNameEmbeddings");



    public static bool GeneratePersonLifeDataset()
    {
        var massiveList = DatasetFactory.AllFamousePeople15k();

        //marriage info
        Parallel.ForEach(massiveList, new ParallelOptions { MaxDegreeOfParallelism = 25 }, ConvertPersonToMarriageInfo);

        //body info
        //Parallel.ForEach(massiveList, new ParallelOptions { MaxDegreeOfParallelism = 20 }, ConvertPersonToBodyInfo);


        return true;
    }

    public static void ConvertPersonToMarriageInfo(PersonListEntity personProfile)
    {
        //loop all 15k profiles
        try
        {
            //get all marriages info for person by profile (via LLM 🛩️)
            var marriageInfoJson = GetRawMarriagesFromPersonProfileLLM_LLAMA3(personProfile).Result;

            var newDatasetRow = new MarriageInfoDatasetEntity() { PartitionKey = personProfile.RowKey, Info = marriageInfoJson };

            marriageInfoDatasetClient_LocalEmulator.UpsertEntityAsync(newDatasetRow);

            Console.WriteLine($"-----{personProfile.RowKey}------");
            Console.WriteLine(marriageInfoJson);
        }
        catch (Exception e)
        {
            Console.WriteLine("FAIL...Moving on 🛩️");
        }

    }

    public static void ConvertPersonToBodyInfo(PersonListEntity personProfile)
    {
        //loop all 15k profiles
        try
        {
            //get all body info for person by profile (via LLM 🛩️)
            var bodyInfoJson = GetRawBodyInfoFromPersonProfileLLM_LLAMA3(personProfile).Result;

            var newDatasetRow = new BodyInfoDatasetEntity() { PartitionKey = personProfile.RowKey, Info = bodyInfoJson };

            bodyInfoDatasetClient_LocalEmulator.UpsertEntityAsync(newDatasetRow);

            Console.WriteLine($"-----{personProfile.RowKey}------");
            Console.WriteLine(bodyInfoJson);
        }
        catch (Exception e)
        {
            Console.WriteLine("FAIL...Moving on 🛩️");
        }

    }


    /// <summary>
    /// Given a person profile entity will use person name and birthdate to get marriage info in json raw string
    /// </summary>
    public static async Task<string> GetRawMarriagesFromPersonProfileLLM_LLAMA3(PersonListEntity? personProfile)
    {
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
                       (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };


        using (var client = new HttpClient(handler))
        {
            var requestBodyObject = new
            {
                messages = new[]
                {
                    new { role = "system", content = "given person name & birth year output only JSON marriage data in valid ```json" +
                                                     @"{
  ""marriages"":[
    {
      ""type"": ""Arranged/Love/Pragmatic"",
      ""spouse"": ""John Doe"",
      ""marriageDate"": ""30/12/2000"",
      ""divorceDate"": ""30/12/2000"",
      ""outcome"": ""Happiness/Struggle/Dissolution"",
      ""dataCredibility"": ""low/medium/high""
    },
    {
      ""type"": ""Arranged/Love/Pragmatic"",
      ""spouse"": ""Mike Doe"",
      ""marriageDate"": ""30/12/2000"",
      ""divorceDate"": ""30/12/2000"",
      ""outcome"": ""Happiness/Struggle/Dissolution"",
      ""dataCredibility"": ""low/medium/high""
    }
  ]
}```" },
                    new { role = "user", content = $"'{personProfile.Name}' born '{personProfile.GetBirthYear()}'"}
                    },
                max_tokens = 4096,
                temperature = 0.1,
                top_p = 0.1,
                best_of = 1,
                presence_penalty = 0,
                use_beam_search = "false",
                ignore_eos = "false",
                skip_special_tokens = "false",
                logprobs = "false"
            };

            var requestBody = JsonConvert.SerializeObject(requestBodyObject);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMetaLlama3APIKey);
            client.BaseAddress = new Uri(MetaLlama_3_70BEndpoint);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("", content);

            //get full reply and parse it
            var fullReplyRaw = await response.Content.ReadAsStringAsync();
            var fullReply = new LlamaReplyJson(fullReplyRaw);

            //return only message text
            var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
            return replyMessage;
        }
    }

    public static async Task<string> GetRawBodyInfoFromPersonProfileLLM_LLAMA3(PersonListEntity? personProfile)
    {
        var handler = new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
                       (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
        };


        using (var client = new HttpClient(handler))
        {
            var requestBodyObject = new
            {
                messages = new[]
                {
                    new { role = "system", content = "given person name & birth year output only JSON body data in valid ```json" +
                                                     @"{
  ""body"": {
    ""type"": ""ectomorph/mesomorph/endomorph"",
    ""breastSize"": ""small/medium/large"",
    ""height"": ""short/average/tall"",
    ""weight"": ""light/medium/heavy"",
    ""dataCredibility"": ""low/medium/high"",
  }
}```" },

                    //new { role = "user", content = "'Marilyn Monroe' born '1926'" },
                    //new { role = "assistant", content = @"" },
                    new { role = "user", content = $"'{personProfile.Name}' born '{personProfile.GetBirthYear()}'"}
                    },
                max_tokens = 4096,
                temperature = 0.1,
                top_p = 0.1,
                best_of = 1,
                presence_penalty = 0,
                use_beam_search = "false",
                ignore_eos = "false",
                skip_special_tokens = "false",
                logprobs = "false"
            };

            var requestBody = JsonConvert.SerializeObject(requestBodyObject);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMetaLlama3APIKey);
            client.BaseAddress = new Uri(MetaLlama_3_70BEndpoint);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("", content);

            //get full reply and parse it
            var fullReplyRaw = await response.Content.ReadAsStringAsync();
            var fullReply = new LlamaReplyJson(fullReplyRaw);

            //return only message text
            var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
            return replyMessage;
        }
    }

    /// <summary>
    /// creates dataset for making marriage ml predictions and saves to Azure Tables
    /// </summary>
    public static Task<bool> GenerateMarriageKutaDataset()
    {

        // 1 :  


        // 4 : FINAL DATASET
        // PredictionTag | MaleNameYear | FemaleNameYear | 

        throw new Exception();

        //return true;
    }


    public static void CleanPersonList()
    {
        var emptyDetect = @"{
  ""StdTime"": ""00:00 01/01/2000 +08:00"",
  ""Location"": {
    ""Name"": ""Empty"",
    ""Longitude"": 101.0,
    ""Latitude"": 4.59
  }
}";
        string filter = $"BirthTime eq '{emptyDetect}'";

        // Query the table for matching records
        var queryResults = personListClient_LocalEmulator.Query<PersonListEntity>(filter);

        // Iterate through the query results in parallel and delete matching records
        Parallel.ForEach(queryResults, entity =>
        {
            var deleteResponse = personListClient_LocalEmulator.DeleteEntity(entity.PartitionKey, entity.RowKey);
            Console.WriteLine($"Deleted entity with PartitionKey: {entity.PartitionKey}, RowKey: {entity.RowKey}");
        });

    }

    public static void PrintDatasetHighDataCredibility<T>(TableClient tableClient) where T : class, ITableEntity
    {

        // get all
        var queryResults = tableClient.Query<T>();

        var count = 0;
        // Iterate through the query results in parallel and delete matching records
        Parallel.ForEach(queryResults, entity =>
        {
            ((dynamic)entity)?.PrintHighCredibilityMarriages();
            //var xxx = ccc[0];
            //if (xxx["dataCredibility"].Value<string>() == "high")
            //{
            //    count++;
            //    Console.WriteLine(entity.PartitionKey + "-" + ((dynamic)entity).InfoJson().ToString(Formatting.None));
            //}
        });

        Console.WriteLine($"records:{count}");
    }

    /// <summary>
    /// given table client, will clean input char from info field
    /// </summary>
    public static void CleanDatasetFromCharacter<T>(string targetCharacters, TableClient tableClient) where T : class, ITableEntity
    {
        // get all
        var queryResults = tableClient.Query<T>();

        // Iterate through the query results in parallel and delete matching records
        Parallel.ForEach(queryResults, new ParallelOptions { MaxDegreeOfParallelism = 20 }, entity =>
        {
            //replace character from text with nothing
            var cleanedInfo = ((dynamic)entity).Info.Replace(targetCharacters, "");
            ((dynamic)entity).Info = cleanedInfo;
            tableClient.UpsertEntity<T>(entity, TableUpdateMode.Replace);
            Console.WriteLine($"Cleaned entity with PartitionKey: {((dynamic)entity).PartitionKey}, Info: {((dynamic)entity).Info}");
        });

    }

    /// <summary>
    /// using an embedding model create embeddings vectors for all 15k famous ppl dataset,
    /// easy for cross-reference search with other databases where names will not match exact
    /// </summary>
    public static async Task<bool> FillPersonNameEmbeddings()
    {
        //get all person list
        //string filter = $"BirthTime eq '{emptyDetect}'";

        // Query the table for matching records
        Pageable<PersonListEntity> queryResults = personListClient_LocalEmulator.Query<PersonListEntity>();


        // Iterate through the query results in parallel and get embeddings for each and save them straight to db
        Parallel.ForEach(queryResults, new ParallelOptions { MaxDegreeOfParallelism = 10 }, ConvertPersonListToNameEmbeddings);


        return true;

    }

    private static void ConvertPersonListToNameEmbeddings(PersonListEntity entity)
    {
        try
        {
            Console.WriteLine("Embedding - " + entity.Name);

            //get embeddings for name only (LLM API call) 🤑
            var nameVectors = LLMEmbeddingManager.GetEmbeddingsForText_Ada002(entity.Name).Result;

            //make new row for name embeddings table and upload
            var newEmbedRow = new PersonNameEmbeddingsEntity() { PartitionKey = entity.Name, Embeddings = nameVectors, };

            //add to separate DB
            personNameEmbeddingsClient_LocalEmulator.UpsertEntityAsync(newEmbedRow);

            Console.WriteLine("Done - " + entity.Name);
        }
        catch (Exception e)
        {
            Console.WriteLine("!!FAILED TO EMBED -> " + entity.Name);

            Console.WriteLine(e);
            Console.WriteLine("UP AND ONWARD!!!!");
        }
    }


    /// <summary>
    /// given fuzzy person name, will return name found in record
    /// exp : DatasetFactory.FamousPersonNameLLMSearch("mrilyn monroe");
    /// </summary>
    public static string FamousPersonNameLLMSearch(string personName)
    {
        //#1 EMBED QUERY
        //get embeddings for fresh query meat
        var searchKeywordVector = LLMEmbeddingManager.GetEmbeddingsArrayForText_Ada002(personName).Result;

        //#2 GET ALL PROFILES
        var allDocsEmbeddings = personNameEmbeddingsClient_LocalEmulator.Query<PersonNameEmbeddingsEntity>()?.ToList();

        //#3 COSINE SIMILARITY (CPU power⚡)
        var similarity = PersonNameEmbeddingsGetSimilarity(searchKeywordVector, allDocsEmbeddings);

        // Take top results with minimum similarity score
        var minimumSimilarityScore = 0.85; //closest
        var topScoreMatch = similarity.Where(x => x.Key >= minimumSimilarityScore);

        //if top found person name, send back to caller
        if (topScoreMatch?.Any() ?? false)
        {
            var topPersonName = topScoreMatch.FirstOrDefault().Value.PartitionKey;
            return topPersonName;
        }

        //else empty to let caller know
        return "Empty";
    }

    public static Dictionary<double, PersonNameEmbeddingsEntity> PersonNameEmbeddingsGetSimilarity(double[] target, List<PersonNameEmbeddingsEntity> candidatesList)
    {
        //get score for each row and save it in dictionary
        var finalList = new Dictionary<double, PersonNameEmbeddingsEntity>() { };

        foreach (var candidate in candidatesList)
        {
            var similarityScore2 = NLPTools.CosineSimilarity(target, candidate.GetEmbeddingsArray());
            finalList.Add(similarityScore2, candidate);
        }

        // Sort dictionary into high score at top
        var sortedList = finalList.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

        return sortedList;
    }


    /// <summary>
    /// get's all 15.8k people entity from db
    /// </summary>
    /// <returns></returns>
    public static Pageable<PersonListEntity> AllFamousePeople15k() => personListClient_LocalEmulator.Query<PersonListEntity>();
}