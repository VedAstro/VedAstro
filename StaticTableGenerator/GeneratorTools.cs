using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace StaticTableGenerator
{
    public static class GeneratorTools
    {
        /// <summary>
        /// EXAMPLE: VedAstro.Library.Calculate.List<Avasta> PlanetAvasta(PlanetName planetName, Time time)
        /// </summary>
        public static string GetMethodSignature(this MethodDeclarationSyntax methodDeclaration, SemanticModel semanticModel)
        {
            var returnTypeSymbol = semanticModel.GetTypeInfo(methodDeclaration.ReturnType).Type;
            var returnType = returnTypeSymbol?.ToDisplayString() ?? methodDeclaration.ReturnType.ToString();
            var parameters = methodDeclaration.ParameterList.Parameters;
            var parameterDescriptions = parameters.Select(param =>
                $"{param.Type.ToString()} {param.Identifier.Text}");
            var declaringType = semanticModel.GetDeclaredSymbol(methodDeclaration.Parent)?.ToDisplayString();

            return $"{declaringType}.{returnType} {methodDeclaration.Identifier.Text}({string.Join(", ", parameterDescriptions)})";
        }

    }
}
