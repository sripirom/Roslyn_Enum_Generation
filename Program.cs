using System;
using EnumGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EnumGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = GetConnectionString("");
            Console.WriteLine(connectionString);

            string enumPath = Path.Combine(Directory.GetCurrentDirectory(), "Enums");
            foreach(var enumFileName in Directory.GetFiles(enumPath).ToList().Where(a=> new FileInfo(a).Name.StartsWith("Enum")))
            {
                Console.WriteLine(enumFileName);
             
                GenerateSampleEnum(enumFileName, connectionString);
            }
        }

        public static string GetConnectionString(string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

            IConfigurationRoot configuration = builder.Build();
            string conString = Microsoft
            .Extensions
            .Configuration
            .ConfigurationExtensions
            .GetConnectionString(configuration, "DefaultConnection");

            return conString;
        }

        static void GenerateSampleEnum(string enumFileName, string connectionString)
        {
            string enumContent = File.ReadAllText(enumFileName);
            var node = CSharpSyntaxTree.ParseText(enumContent).GetRoot();
            var enumModel = EnumTypeGeneration.GenerateEnumType(node, connectionString);

            if(enumModel!=null){
                Console.WriteLine(enumModel.ToFullString());
                File.WriteAllText(enumFileName, enumModel.ToFullString());
            }
        }
    }
}
