using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Text;
using System.Collections.Generic;

namespace EnumGenerator
{
  public static class EnumTypeGeneration
  {
    public static SyntaxNode GenerateEnumType(SyntaxNode node, string stringConnection)
    {
      // Find the first class in the syntax node
      var enumNode = node.DescendantNodes().OfType<EnumDeclarationSyntax>().FirstOrDefault();

      if(enumNode!=null)
      {
        var codeIssues = node.GetDiagnostics();
        if(!codeIssues.Any())
        {
           // Get the name of the enum
          var enumName = enumNode.Identifier.Text;
          var tableNameAttribute = enumNode.AttributeLists.FirstOrDefault();
          string tableName = tableNameAttribute.Attributes.FirstOrDefault().ArgumentList.Arguments[0].ToString().Replace('"', ' ').Trim();
          string columnId = tableNameAttribute.Attributes.FirstOrDefault().ArgumentList.Arguments[1].ToString().Replace('"', ' ').Trim();
          IEnumerable<Tuple<int, string, string>> enumItems = new DataTableLoader(stringConnection).Load(columnId, tableName);
          // Only for demo purposes, pluralizing an object is done by
          // simply adding the "s" letter. Consider proper algorithms
          StringBuilder newImplementation = new StringBuilder();
          newImplementation.Append($@" 
            using System.ComponentModel;

              {tableNameAttribute.ToString()}       
              public enum {enumName} {{");

          foreach(var item in enumItems)
          {
            newImplementation.AppendLine($@" /// <summary>
    /// {item.Item2} = {item.Item1}
    /// </summary>
    [Description(""{item.Item3}"")]");
            newImplementation.AppendLine($"{item.Item2} = {item.Item1},");
          }

        newImplementation.Append("}");

            var newEnumNode =
              SyntaxFactory.ParseSyntaxTree(newImplementation.ToString()).GetRoot()
              .DescendantNodes().OfType<EnumDeclarationSyntax>()
              .FirstOrDefault();

           
            if(!(enumNode.Parent is NamespaceDeclarationSyntax)) return null;

            var parentNamespace = (NamespaceDeclarationSyntax)enumNode.Parent;

            var qualifiedName = SyntaxFactory.ParseName("System.ComponentModel");
            var usingDirective = SyntaxFactory.UsingDirective(qualifiedName);
            // Add the new class to the namespace
            var newParentNamespace = parentNamespace.RemoveNode(enumNode, SyntaxRemoveOptions.KeepEndOfLine)
                                                    .AddMembers(newEnumNode).NormalizeWhitespace();
            if (!newParentNamespace.Usings.Select(d => d.Name.ToString()).Any(u => u == qualifiedName.ToString()))
            {
                newParentNamespace = newParentNamespace.AddUsings(usingDirective).NormalizeWhitespace();
            }                                 
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