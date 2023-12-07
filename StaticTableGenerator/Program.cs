
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
		static string[] CalculatorCodeFile = new[]
		{
			Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Logic\Calculate\Calculate.cs") ,
			Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Logic\Calculate\CalculateKP.cs")
		};
		static string MetadataStaticTableFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro\Library\Data\OpenAPIStaticTable.cs");
		static string PythonCalculateStubFile = Path.Combine(userFolderPath, @"Desktop\Projects\VedAstro.Python\VedAstro\Library.pyi");


		static void Main(string[] args)
		{
			//get all open api calcs summary comments and use as description with name
			//var calcDescriptionList = ExtractSummaries(CalculatorCodeFile);
			var calcDescriptionList2 = ExtractSummaries2(CalculatorCodeFile);

			var allApiCalculatorsMethodInfo = Tools.GetAllApiCalculatorsMethodInfo();

			var returnList = new List<OpenAPIMetadata>();
			foreach (var openApiCalc in allApiCalculatorsMethodInfo)
			{
				//get special signature to find the correct description from list
				var signature = openApiCalc.GetMethodSignature();

				//get example output in json
				var exampleOut = GetExampleOutputJson(openApiCalc);

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

			//------
			//based on created meta data make new C# code file 
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
				sb.AppendLine($"        new( \"{metadata.Signature}\",\"{metadata.LineNumber}\",\"{metadata.ParameterDescription}\",\"{metadata.Description}\",\"{metadata.ExampleOutput}\"),");
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
		public static Dictionary<string, string> ExtractSummaries(string[] filePaths)
		{
			// Initialize a dictionary to hold the method signatures and their corresponding summaries
			Dictionary<string, string> summaries = new Dictionary<string, string>();
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
					// Get the unique ID of the method
					var methodSignature = method.GetMethodSignature(semanticModel);
					if (summary != null)
					{
						// Get the raw content of the summary
						var rawComments = summary.Content.ToString().Trim();
						// Clean the comments by removing all non-alphanumeric characters except for space, underscore, and dot
						string safeDescription = Regex.Replace(rawComments, "[^a-zA-Z0-9 _.]+", "");
						// Replace all new lines and multiple spaces with a single space
						safeDescription = Regex.Replace(safeDescription, @"\s{2,}", " ");
						// Add the method signature and cleaned summary to the dictionary
						summaries.Add(methodSignature, safeDescription);
					}
					else
					{
						// If there is no summary, add the method signature with a default text to the dictionary
						summaries.Add(methodSignature, "Empty sample text");
					}
				}
			}
			// Return the dictionary containing the method signatures and their summaries
			return summaries;
		}

		public static List<MethodDocumentation> ExtractSummaries2(string[] filePaths)
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
