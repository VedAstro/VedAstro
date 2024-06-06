
//█░█ ▒█▀▀▀ █▀▀█ █▀▀█ █░░ ░░ █░█ 　 █▀▀ █▀▀█ ░▀░ █▀▀▄ 　 █▀▄▀█ █░░█ 　 ▒█▀▄▀█ █░░█ █▀▀ █▀▀ 　 ▀▀█▀▀ █▀▀█ 
//░░░ ▒█▀▀▀ █░░█ █░░█ █░░ ▄▄ ░░░ 　 ▀▀█ █▄▄█ ▀█▀ █░░█ 　 █░▀░█ █▄▄█ 　 ▒█▒█▒█ █░░█ ▀▀█ █▀▀ 　 ░░█░░ █░░█ 
//░░░ ▒█░░░ ▀▀▀▀ ▀▀▀▀ ▀▀▀ ░█ ░░░ 　 ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀░ 　 ▀░░░▀ ▄▄▄█ 　 ▒█░░▒█ ░▀▀▀ ▀▀▀ ▀▀▀ 　 ░░▀░░ ▀▀▀▀ 

//█▀▄▀█ █▀▀ ░░ 　 █░█ █░░ █▀▀█ █▀▀█ █░█ 　 ░▀░ █▀▀▄ 　 ▀▀█▀▀ █░░█ █░░█ 　 █░░█ █▀▀ █▀▀█ █▀▀█ ▀▀█▀▀ 
//█░▀░█ █▀▀ ▄▄ 　 ░░░ █░░ █░░█ █░░█ █▀▄ 　 ▀█▀ █░░█ 　 ░░█░░ █▀▀█ █▄▄█ 　 █▀▀█ █▀▀ █▄▄█ █▄▄▀ ░░█░░ 
//▀░░░▀ ▀▀▀ ░█ 　 ░░░ ▀▀▀ ▀▀▀▀ ▀▀▀▀ ▀░▀ 　 ▀▀▀ ▀░░▀ 　 ░░▀░░ ▀░░▀ ▄▄▄█ 　 ▀░░▀ ▀▀▀ ▀░░▀ ▀░▀▀ ░░▀░░ 

//█▀▀█ █▀▀▄ █▀▀▄ 　 █░░░█ █▀▀█ ░▀░ ▀▀█▀▀ █▀▀ ░ █░█ 
//█▄▄█ █░░█ █░░█ 　 █▄█▄█ █▄▄▀ ▀█▀ ░░█░░ █▀▀ ▄ ░░░ 
//▀░░▀ ▀░░▀ ▀▀▀░ 　 ░▀░▀░ ▀░▀▀ ▀▀▀ ░░▀░░ ▀▀▀ █ ░░░
//-- Sir Philip Sidney

using System.Text;
using VedAstro.Library;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Reflection.Metadata;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace StaticTableGenerator
{
    internal class Program
    {

        /// <summary>
        /// dynamically fill in the place the location of the files, somewhat
        /// </summary>
        static string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        static string[] CalculatorCodeFile = new[]
        {
            Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Logic\Calculate\Calculate.cs")
        };
        static string MetadataStaticTableFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Data\OpenAPIStaticTable.cs");
        static string PythonCalculateStubFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro.Python\VedAstro\Library.pyi");
        static string EventDataListFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Website\wwwroot\data\EventDataList.xml");
        static string EventDataListStaticTableFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Data\EventDataListStatic.cs");
        static string HoroscopeDataListFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Website\wwwroot\data\HoroscopeDataList.xml");
        static string HoroscopeDataListStaticTableFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Data\HoroscopeDataListStatic.cs");
        static string HoroscopeNameEnumFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Data\Enum\HoroscopeName.cs");
        static string AIPredictionCSSafetyCacheFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\StaticTableGenerator\AI-MADE-DATA\AI-prediction-cs-safety-cache.csv");


        static void Main(string[] args)
        {
            //get all open api calcs summary comments and use as description with name
            var calcDescriptionList2 = ExtractSummaries(CalculatorCodeFile);

            var allApiCalculatorsMethodInfo = Tools.GetAllApiCalculatorsMethodInfo();

            var returnList = new List<OpenAPIMetadata>();
            foreach (var openApiCalc in allApiCalculatorsMethodInfo)
            {
                //get special signature to find the correct description from list
                var signature = openApiCalc.GetMethodSignature();

                //get example output in json
                //todo disabled because some methods not fully done causing errors
                //var exampleOut = GetExampleOutputJson(openApiCalc);
                var exampleOut = "";

                //get the description
                var description = calcDescriptionList2.Where(x => x.Signature == signature).Select(x => x.Description).FirstOrDefault();

                //get the line number of the method signature
                var lineNumber = calcDescriptionList2.Where(x => x.Signature == signature).Select(x => x.LineNumber).FirstOrDefault().ToString();

                //get all the params and their comments stacked in 1 string
                var paramsComments = calcDescriptionList2.Where(x => x.Signature == signature).Select(x => x.Params);

                //join all the params and their comments into 1 string in format like this: "param1: comment1, param2: comment2"
                var paramsCommentsString = string.Join(", ", paramsComments.SelectMany(x => x).Select(x => $"{x.Key}: {x.Value}"));

                returnList.Add(new OpenAPIMetadata(signature, lineNumber, paramsCommentsString, description ?? "NO DESC FOUND!! ERROR", exampleOut, openApiCalc));
            }


            //------ TASK 1
            WriteMetadataStaticTableClass(returnList);

            //------ TASK 2
            WritePythonCalculateStubFile(returnList);

            //------ TASK 3
            WriteEventDataListStaticTableClass();

            //------ TASK 4
            WriteHoroscopeDataListStaticTableClass();


            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static void WritePythonCalculateStubFile(List<OpenAPIMetadata> returnList)
        {
            var pythonStubFile = GeneratePythonStubFile(returnList);
            //wrap with namespace
            var finalPythonStubFile = $$"""
                                        # AUTO GENERATED ON {{DateTime.Now.ToString(Time.DateTimeFormat)}}
                                        # DO NOT EDIT DIRECTLY, USE STATIC TABLE GENERATOR IN MAIN REPO

                                        from typing import Any

                                        class Tarabala:
                                            pass
                                        class Karana:
                                            pass
                                        class NithyaYoga:
                                            pass
                                        class House:
                                            pass
                                        class DayOfWeek:
                                            pass
                                        class LunarMonth:
                                            pass
                                        class Object:
                                            pass
                                        class Type:
                                            pass
                                        class DateTimeOffset:
                                            pass
                                        class DateTime:
                                            pass
                                        class Boolean:
                                            pass
                                        class Int32:
                                            pass
                                        class TimeSpan:
                                            pass
                                        class Double:
                                            pass
                                        class String:
                                            pass
                                        class Time:
                                            pass
                                        class Angle:
                                            pass
                                        class ZodiacSign:
                                            pass
                                        class ZodiacName:
                                            pass
                                        class ConstellationName:
                                            pass
                                        class ConstellationAnimal:
                                            pass
                                        class PlanetToSignRelationship:
                                            pass
                                        class PlanetToPlanetRelationship:
                                            pass
                                        class HouseSubStrength:
                                            pass
                                        class PlanetName:
                                            pass
                                        class PlanetConstellation:
                                            pass
                                        class HouseName:
                                            pass
                                        class GeoLocation:
                                            pass
                                        class Person:
                                            pass
                                        class PanchakaName:
                                            pass
                                        class EventNature:
                                            pass
                                        class Varna:
                                            pass
                                        class PlanetMotion:
                                            pass
                                        class Shashtiamsa:
                                            pass
                                        class Dasas:
                                            pass
                                        class Tools:
                                            pass
                                        class LunarDay:
                                            pass


                                        {{pythonStubFile}}

                                        """;
            //writes new static stub file
            File.WriteAllText(PythonCalculateStubFile, finalPythonStubFile);
        }

        private static void WriteMetadataStaticTableClass(List<OpenAPIMetadata> returnList)
        {
            //based on created meta data make new C# code file 
            var classAsText = GenerateStaticTableClass(returnList);
            //wrap with namespace
            var finalClassFile = $$"""
                                   using System.Collections.Generic;
                                   namespace VedAstro.Library
                                   {
                                   
                                       /// <summary>
                                       /// Auto generated code by StaticTableGenerator, so that Open API methods have a metadata.
                                       /// Regenerate when files Calculate.cs gets updated. ✝️Amen for automation!
                                       /// </summary>
                                       {{classAsText}}
                                   }

                                   """;
            //writes new static table class
            File.WriteAllText(MetadataStaticTableFile, finalClassFile);
        }

        private static void WriteEventDataListStaticTableClass()
        {
            // Read the content of the XML event data file
            string fileContent = File.ReadAllText(EventDataListFile);

            //based on created metadata make new C# code file 
            var classAsText = GenerateEventDataStaticTableClass(fileContent);


            //writes new static table class
            File.WriteAllText(EventDataListStaticTableFile, classAsText);
        }

        private static void WriteHoroscopeDataListStaticTableClass()
        {
            // Read the content of the XML event data file
            string fileContent = File.ReadAllText(HoroscopeDataListFile);

            //based on created metadata make new C# code file 
            var classAsText = GenerateHoroscopeDataStaticTableClass(fileContent);
            var horoscopeNameEnumAsText = GenerateHoroscopeNameEnumAsText(fileContent);

            //writes new static table class
            File.WriteAllText(HoroscopeDataListStaticTableFile, classAsText);
            File.WriteAllText(HoroscopeNameEnumFile, horoscopeNameEnumAsText);
        }

        private static string GetExampleOutputJson(MethodInfo openApiCalc)
        {
            try
            {
                //prepare sample list
                var sampleInput = openApiCalc.GetInitializedSampleParameters();

                var jsonData = AutoCalculator.ExecuteFunctionsJSON(openApiCalc, sampleInput.ToArray());

                var jsonRaw = jsonData.ToString();

                //clean comments, since it can be wild
                //will remove all non-alphanumeric characters except for space, underscore, and dot.
                //It will also replace all new lines and multiple spaces with a single space. 
                string safeOutputJson = Regex.Replace(jsonRaw, @"\r\n?|\n", "");

                //double quotes to single quotes, to make storable in C#
                safeOutputJson = Regex.Replace(safeOutputJson, "\"", "'");

                //clean double white space or more
                safeOutputJson = Regex.Replace(safeOutputJson, @"\s{2,}", " ");

                return safeOutputJson;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error No sample for {openApiCalc.Name}");

                //no sample output can be made
                return "";
            }

        }

        /// <summary>
        /// class that implements this has a sample initialization data for demo/sample uses
        /// used to show Open API method's output without running code, dynamic documentation
        /// </summary>
        public static string GenerateStaticTableClass(List<OpenAPIMetadata> metadataList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public static class OpenAPIStaticTable");
            sb.AppendLine("{");
            sb.AppendLine("    public static List<OpenAPIMetadata> Rows = new List<OpenAPIMetadata>");
            sb.AppendLine("    {");
            foreach (var metadata in metadataList)
            {
                sb.AppendLine($"        new( \"{metadata.Signature}\",\"{metadata.LineNumber}\",\"{metadata.ParameterDescription}\",\"{metadata.Description}\",\"{metadata.ExampleOutput}\"),");
            }
            sb.AppendLine("    };");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// This is where the final massive CS for EvenData is made with LLM use! real pro 😁
        /// </summary>
        public static string GenerateEventDataStaticTableClass(string xmlString)
        {
            var document = XDocument.Parse(xmlString);
            var allList = document.Root.Elements().ToList();

            var compiledCode = new StringBuilder();
            string indent = "            "; // Adjust this to your desired indentation
            foreach (var eachEventXml in allList)
            {
                var yy = EventData.FromXml(eachEventXml);
                var xtagsString = "";
                foreach (var tag in yy.EventTags)
                {
                    xtagsString += $"EventTag.{tag.ToString()},";
                }

                var yTags = $"[{xtagsString}]";

                //use LLM to get create specialized summary (CACHED to save money 🤑)
                var specialSummaryCsCode = GenerateCSCodeAsync(yy.Description, useCache: true).Result;

                //add new event data code
                compiledCode.AppendLine($"{indent}new(EventName.{yy.Name}, EventNature.{yy.Nature}, {specialSummaryCsCode}, @\"{yy.Description}\", {yTags}, EventManager.GetEventCalculatorMethod(EventName.{yy.Name})),");
            }

            //make new line for next guy
            Console.WriteLine("\n~~~~~LLM work done~~~~~");

            //remove indentation at start of compiled lines
            var compiledCode2 = compiledCode.ToString().TrimStart();

            //NOTE: leave below code as is, to get perfect indentation
            var newClassFile =
                $@"using System.Collections.Generic;

namespace VedAstro.Library
{{
    /// <summary>
    /// Auto generated code by StaticTableGenerator, so that Open API methods have a metadata.
    /// Regenerate when files Calculate.cs gets updated. ✝️Amen for automation!
    /// </summary>
    public static class EventDataListStatic
    {{
        public static List<EventData> Rows = new List<EventData>
        {{
            {compiledCode2}
        }};
    }}
}}";


            // Replace all line endings with \r\n (CR LF)
            newClassFile = newClassFile.Replace("\r\n", "\n").Replace("\n", "\r\n");

            return newClassFile;
        }

        /// <summary>
        /// A place to track counts, for debugging, info print-outs
        /// </summary>
        public static int GenerateCount = 0;

        private static async Task<string> GenerateCSCodeAsync(string predictionText, bool useCache)
        {
            string rawOut;
            string sha256PredictionHash;

            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(predictionText);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                sha256PredictionHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }

            if (useCache)
            {
                // Check cache first
                if (File.Exists(AIPredictionCSSafetyCacheFile))
                {

                    string[] lines = await File.ReadAllLinesAsync(AIPredictionCSSafetyCacheFile);
                    foreach (string line in lines)
                    {
                        int index = line.IndexOf(',');


                        string hash = "";
                        string prediction = "";
                        if (index >= 0) // Ensure the comma exists
                        {
                            hash = line.Substring(0, index);
                            prediction = line.Substring(index + 1);
                        }

                        //hash matched exactly, what are the odds!
                        // If found a match, return the cached result
                        if (hash.Equals(sha256PredictionHash))
                        {
                            GenerateCount++;
                            Console.Write($"\rCache used no :{GenerateCount}"); // for easy debug
                            return prediction;
                        }
                    }
                }
            }

            // No cache hit or 'useCache' was false, so do risky generation (AI prediction)
            rawOut = await Task.Run(() => AISummarizePredictionText(predictionText));

            if (!Directory.Exists(Path.GetDirectoryName(AIPredictionCSSafetyCacheFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AIPredictionCSSafetyCacheFile));
            }

            // Add to csv file immediately by hash as ID
            await File.AppendAllLinesAsync(AIPredictionCSSafetyCacheFile, new[] { $"{sha256PredictionHash},{rawOut}" });

            return rawOut;
        }


        /// <summary>
        /// Given raw text sends to LLM and makes it into summarized c# code
        /// </summary>
        /// <param name="predictionText"></param>
        /// <returns></returns>
        private static async Task<string> AISummarizePredictionText(string predictionText)
        {

            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://vedastrocontainer.delightfulground-a2445e4b.westus2.azurecontainerapps.io/SummarizePrediction");

                //set settings and package data for LLM server
                var jsonContent = new JObject
                {
                    ["input_text"] = predictionText,
                    ["temperature"] = 0.1,
                    ["instruction_text"] = @"Analyze the context text for its relevance to the keywords {Mind, Studies, Family, Money, Love, Body} and determine the overall sentiment of the text only as either one of this tags {Good, Bad, Neutral}",
                    ["password"] = "empire"
                };

                //send request to LLM server and get response as raw JSON text
                request.Content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                //make data readable for conversion
                var rawJson = await response.Content.ReadAsStringAsync();
                var parsedJson = JObject.Parse(rawJson);

                //convert JSON to c# code
                //Expected OUT
                //new() {
                //     Studies = new(EventNature.Neutral),
                //     Family = new(EventNature.Neutral),
                // }
                StringBuilder sb = new StringBuilder();
                sb.Append("new() {");
                JProperty[] properties = parsedJson.Properties().ToArray();
                for (int i = 0; i < properties.Length; i++)
                {
                    JProperty property = properties[i];
                    sb.Append($"{property.Name} = new(EventNature.{property.Value})");

                    //only add comma if on between, not last 
                    if (i < properties.Length - 1) { sb.Append(","); }
                }
                sb.Append("}");
                var finalCSharpCode = sb.ToString();

                return finalCSharpCode;
            }
            catch (Exception e)
            {
                //set empty
                return "SpecializedSummary.Empty";
            }
        }

        public static string GenerateHoroscopeDataStaticTableClass(string xmlString)
        {
            var document = XDocument.Parse(xmlString);
            var allList = document.Root.Elements().ToList();

            var compiledCode = new StringBuilder();
            string indent = "            "; // Adjust this to your desired indentation
            foreach (var eachEventXml in allList)
            {
                var yy = HoroscopeData.FromXml(eachEventXml);
                var tagsAsText = "";
                foreach (var tag in yy.EventTags)
                {
                    tagsAsText += $"EventTag.{tag.ToString()},";
                }

                var yTags = $"[{tagsAsText}]";
                //add new event data code
                var cleanedDescription = Tools.CleanText(yy.Description);

                //raw horoscope name because not yet compiled
                var rawHoroscopeName = eachEventXml.Element("Name")?.Value;

                compiledCode.AppendLine($"{indent}new(HoroscopeName.{rawHoroscopeName}, EventNature.{yy.Nature}, SpecializedSummary.Empty, @\"{cleanedDescription}\", {yTags}, EventManager.GetHoroscopeCalculatorMethod(HoroscopeName.{rawHoroscopeName})),");
            }

            //remove indentation at start of compiled lines
            var compiledCode2 = compiledCode.ToString().TrimStart();

            //NOTE: leave below code as is, to get perfect indentation
            var newClassFile =
                $@"using System.Collections.Generic;

namespace VedAstro.Library
{{
    /// <summary>
    /// Auto generated code by StaticTableGenerator, so that Open API methods have a metadata.
    /// Regenerate when files Calculate.cs gets updated. ✝️Amen for automation!
    /// </summary>
    public static class HoroscopeDataListStatic
    {{
        public static List<HoroscopeData> Rows = new List<HoroscopeData>
        {{
            {compiledCode2}
        }};
    }}
}}";

            // Replace all line endings with \r\n (CR LF)
            newClassFile = newClassFile.Replace("\r\n", "\n").Replace("\n", "\r\n");

            return newClassFile;
        }
        
        public static string GenerateHoroscopeNameEnumAsText(string xmlString)
        {
            var document = XDocument.Parse(xmlString);
            var allList = document.Root.Elements().ToList();

            var compiledCode = new StringBuilder();
            string indent = "            "; // Adjust this to your desired indentation
            foreach (var eachEventXml in allList)
            {
                //raw horoscope name because not yet compiled
                var rawHoroscopeName = eachEventXml.Element("Name")?.Value;

                compiledCode.AppendLine($"{indent}{rawHoroscopeName},");
            }

            //remove indentation at start of compiled lines
            var compiledCode2 = compiledCode.ToString().TrimStart();

            //NOTE: leave below code as is, to get perfect indentation
            var newClassFile =
                $@"
namespace VedAstro.Library
{{
    /// <summary>
    /// Hard coded names of Horoscope calculators, this is the glue between LOGIC and DATA
    /// Auto generated code by StaticTableGenerator, so that Open API methods have a metadata.
    /// Regenerate when files Calculate.cs gets updated. ✝️Amen for automation!
    /// </summary>
    public enum HoroscopeName
    {{
        Empty = 0,
        {compiledCode2}
    }}
}}";

            // Replace all line endings with \r\n (CR LF)
            newClassFile = newClassFile.Replace("\r\n", "\n").Replace("\n", "\r\n");

            return newClassFile;
        }

        public static string GeneratePythonStubFile(List<OpenAPIMetadata> metadataList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("class Calculate:");
            foreach (var metadata in metadataList)
            {
                sb.AppendLine($"    {metadata.ToPythonMethodNameStub()}");
                sb.AppendLine($"        {metadata.ToPythonMethodDescStub()}");
                sb.AppendLine("        ...");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Given a path to a CS class file, will parse it and extract method name and comments above and return as list
        /// used to get metadata for Open API calculators
        /// </summary>
        public static List<MethodDocumentation> ExtractSummaries(string[] filePaths)
        {
            // Dictionary to hold the method signatures and their documentation
            List<MethodDocumentation> summaries = new List<MethodDocumentation>();
            // Loop through each file path provided
            foreach (var filePath in filePaths)
            {
                // Read the content of the file
                string fileContent = File.ReadAllText(filePath);
                // Parse the file content into a syntax tree
                var tree = CSharpSyntaxTree.ParseText(fileContent);
                // Get the root of the syntax tree
                var root = tree.GetRoot();
                // Create a compilation that contains this tree
                var compilation = CSharpCompilation.Create("VedAstro.Library", new[] { tree });
                // Get the semantic model of the tree
                var semanticModel = compilation.GetSemanticModel(tree);
                // Loop through all the methods in the syntax tree
                foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
                {
                    // Get the leading trivia (comments) of the method
                    var trivia = method.GetLeadingTrivia().Where(x => x.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
                    // Parse the trivia into XML comments
                    var xmlComments = trivia.Select(x => x.GetStructure()).OfType<DocumentationCommentTriviaSyntax>();
                    // Get the summary tag from the XML comments
                    var summary = xmlComments.SelectMany(x => x.ChildNodes()).OfType<XmlElementSyntax>()
                        .FirstOrDefault(x => x.StartTag.Name.ToString() == "summary");

                    // Get the param tags from the XML comments
                    var paramsComments = xmlComments.SelectMany(x => x.ChildNodes()).OfType<XmlElementSyntax>()
                        .Where(x => x.StartTag.Name.ToString() == "param")
                        .ToDictionary(x =>
                        {
                            var paramName = x.StartTag.Attributes.OfType<XmlNameAttributeSyntax>().First().Identifier.ToString();
                            return paramName;
                        }, x =>
                        {
                            var commentsRaw = x.Content.ToString().Trim();
                            var cleanedComments = CleanCodeCommentsText(commentsRaw);
                            return cleanedComments;
                        });

                    // Get the unique ID of the method
                    var methodSignature = method.GetMethodSignature(semanticModel);
                    // Get the line number of the method signature (plus 1 to compensate for 0 index)
                    var lineNumber = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    // Check if the summary tag exists
                    if (summary != null)
                    {
                        // Get the raw content of the summary
                        var rawComments = summary.Content.ToString().Trim();
                        var safeDescription = CleanCodeCommentsText(rawComments);
                        // Add the method signature and cleaned summary to the dictionary
                        summaries.Add(new MethodDocumentation { Signature = methodSignature, Description = safeDescription, Params = paramsComments, LineNumber = lineNumber });
                    }
                    else
                    {
                        // If there is no summary, add the method signature with a default text to the dictionary
                        summaries.Add(new MethodDocumentation { Signature = methodSignature, Description = "Empty sample text", Params = paramsComments, LineNumber = lineNumber });
                    }
                }
            }
            // Return the dictionary containing the method signatures and their summaries
            return summaries;

            //--------LOCAL FUNCTIONS--------

            string CleanCodeCommentsText(string rawComments)
            {
                // Clean the comments by removing all non-alphanumeric characters except for space, underscore, and dot
                string safeDescription = Regex.Replace(rawComments, "[^a-zA-Z0-9 _.]+", "");
                // Replace all new lines and multiple spaces with a single space
                safeDescription = Regex.Replace(safeDescription, @"\s{2,}", " ");
                return safeDescription;
            }
        }
    }
}
