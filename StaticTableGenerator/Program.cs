
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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace StaticTableGenerator
{
    internal class Program
    {

        /// <summary>
        /// dynamically fill in the place the location of the files, somewhat
        /// </summary>
        static string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        static string CalculatorCodeFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Logic\Calculate\Calculate.cs");
        static string MetadataStaticTableFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Data\OpenAPIStaticTable.cs");
        static string PythonCalculateStubFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro.Python\VedAstro\Library.pyi");


        static void Main(string[] args)
        {
            //get all open api calcs summary comments and use as description with name
            var calcDescriptionList = ExtractSummaries(CalculatorCodeFile);

            var allApiCalculatorsMethodInfo = Tools.GetAllApiCalculatorsMethodInfo();

            var returnList = new List<OpenAPIMetadata>();
            foreach (var openApiCalc in allApiCalculatorsMethodInfo)
            {
                //get special signature to find the correct description from list
                var signature = openApiCalc.GetMethodSignature();
                calcDescriptionList.TryGetValue(signature, out var description);
                var exampleOut = GetExampleOutputJson(openApiCalc);
                returnList.Add(new OpenAPIMetadata(signature, description ?? "NO DESC FOUND!! ERROR", exampleOut, openApiCalc));
            }

            //------
            //generate the new code
            var classAsText = GenerateStaticTableClass(returnList);
            //wrap with namespace
            var finalClassFile = $$"""
                          using System.Collections.Generic;
                          namespace VedAstro.Library
                          {
                          
                              /// <summary>
                              /// This list is auto generated from code when running StaticTableGenerator,
                              /// so that Open API methods have a metadata. Regenerate when files Calculate.cs gets updated
                              /// </summary>
                              {{classAsText}}
                          }

                          """;
            //writes new static table class
            File.WriteAllText(MetadataStaticTableFile, finalClassFile);

            //-----
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

            Console.WriteLine("Done!");
            Console.ReadLine();
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
                sb.AppendLine($"        new( \"{metadata.Signature}\",\"{metadata.Description}\",\"{metadata.ExampleOutput}\"),");
            }
            sb.AppendLine("    };");
            sb.AppendLine("}");
            return sb.ToString();
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
        public static Dictionary<string, string> ExtractSummaries(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            var tree = CSharpSyntaxTree.ParseText(fileContent);
            var root = tree.GetRoot();

            // Create a compilation that contains this tree
            var compilation = CSharpCompilation.Create("VedAstro.Library", new[] { tree });
            var semanticModel = compilation.GetSemanticModel(tree);

            Dictionary<string, string> summaries = new Dictionary<string, string>();
            //loop through only comments above methods
            foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                var trivia = method.GetLeadingTrivia().Where(x => x.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
                var xmlComments = trivia.Select(x => x.GetStructure()).OfType<DocumentationCommentTriviaSyntax>();
                var summary = xmlComments.SelectMany(x => x.ChildNodes()).OfType<XmlElementSyntax>()
                    .FirstOrDefault(x => x.StartTag.Name.ToString() == "summary");

                //unique ID of the method in text
                var methodSignature = method.GetMethodSignature(semanticModel);

                if (summary != null)
                {
                    var rawComments = summary.Content.ToString().Trim();

                    //clean comments, since it can be wild
                    //will remove all non-alphanumeric characters except for space, underscore, and dot.
                    //It will also replace all new lines and multiple spaces with a single space. 
                    string safeDescription = Regex.Replace(rawComments, "[^a-zA-Z0-9 _.]+", "");

                    //clean double white space or more
                    safeDescription = Regex.Replace(safeDescription, @"\s{2,}", " ");

                    //add to return list
                    summaries.Add(methodSignature, safeDescription);
                }
                else
                {

                    summaries.Add(methodSignature, "Empty sample text");
                }
            }
            return summaries;
        }

    }
}
