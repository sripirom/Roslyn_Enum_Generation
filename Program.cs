using System;
using RoslynCore;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;

namespace RoslynCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string enumPath = Path.Combine(Directory.GetCurrentDirectory(), "Enums");
            foreach(var enumFileName in Directory.GetFiles(enumPath))
            {
                Console.WriteLine(enumFileName);
                string enumContent = File.ReadAllText(enumFileName);
                
                GenerateSampleEnum(enumContent);
            }
          
        }

        static void GenerateSampleEnum(string models)
        {

            var node = CSharpSyntaxTree.ParseText(models).GetRoot();
            var viewModel = EnumTypeGeneration.GenerateEnumType(node);
            if(viewModel!=null)
                Console.WriteLine(viewModel.ToFullString());

            Console.ReadLine();
        }

        static void GenerateSampleViewModel()
        {
            const string models = @"namespace Models
{
  public class Item
  {
    public string ItemName { get; set; }
  }
}
";
        var node = CSharpSyntaxTree.ParseText(models).GetRoot();
        var viewModel = ViewModelGeneration.GenerateViewModel(node);
        if(viewModel!=null)
            Console.WriteLine(viewModel.ToFullString());

        Console.ReadLine();
        }
    }

}
