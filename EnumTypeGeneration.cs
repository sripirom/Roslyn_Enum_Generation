using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Text;
using System.Collections.Generic;

namespace RoslynCore
{
  public static class EnumTypeGeneration
  {
    public static SyntaxNode GenerateEnumType(SyntaxNode node)
    {
      IList<Tuple<int, string, string>> enumItems = new List<Tuple<int, string, string>>{
        new Tuple<int, string, string>(1, "AccountNormal", "Desc AccountNormal"),
        new Tuple<int, string, string>(2, "AccountNumber", "Desc AccountNumber"),
        new Tuple<int, string, string>(3, "AccountSpecial", "Desc AccountSpecial"),
        new Tuple<int, string, string>(4, "AccountSample", "Desc AccountSample"),
      };
      // Find the first class in the syntax node
      var enumNode = node.DescendantNodes().OfType<EnumDeclarationSyntax>().FirstOrDefault();

      if(enumNode!=null)
      {
        var codeIssues = node.GetDiagnostics();
        if(!codeIssues.Any())
        {
           // Get the name of the enum
          var enumName = enumNode.Identifier.Text;

          // Only for demo purposes, pluralizing an object is done by
          // simply adding the "s" letter. Consider proper algorithms
          StringBuilder newImplementation = new StringBuilder();
          newImplementation.Append($@"        
              public enum {enumName} {{");
          foreach(var item in enumItems){
            newImplementation.AppendLine($@" /// <summary>
    /// {item.Item2} = {item.Item1}
    /// </summary>
    [Description(""{item.Item3}"")]");
            newImplementation.AppendLine($"{item.Item2} = {item.Item1},");
          }

        newImplementation.Append("}}");

            var newEnumNode =
              SyntaxFactory.ParseSyntaxTree(newImplementation.ToString()).GetRoot()
              .DescendantNodes().OfType<EnumDeclarationSyntax>()
              .FirstOrDefault();

           
            if(!(enumNode.Parent is NamespaceDeclarationSyntax)) return null;

            var parentNamespace = (NamespaceDeclarationSyntax)enumNode.Parent;
            // Add the new class to the namespace
             var newParentNamespace = parentNamespace.RemoveNode(enumNode, SyntaxRemoveOptions.KeepEndOfLine);
            newParentNamespace = newParentNamespace.AddMembers(newEnumNode).NormalizeWhitespace();

            return newParentNamespace;
        }
        else
        {
          foreach(Diagnostic codeIssue in codeIssues)
          {
            string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: {codeIssue.Location.GetLineSpan()}, Severity: {codeIssue.Severity}";
            Console.WriteLine(issue);
          }
          return null;
        }
      }
      else
      {
        return null;
      }
    }
  }
}