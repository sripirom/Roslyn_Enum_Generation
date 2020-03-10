using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using Sripirom.EnumGenerator.Services;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sripirom.EnumGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string applicationPath = Directory.GetCurrentDirectory();
            Console.WriteLine(applicationPath);

            //Console.WriteLine("Directory.GetCurrentDirectory() =" + Directory.GetCurrentDirectory());
            //string[] newArgs = { @"Domains\Enums"};
            //args = newArgs;
            
            string enumPath = Path.Combine(applicationPath, "Enums");
            Console.WriteLine(enumPath);
            if (args.Length > 0 && args?[0] != null)
            {
                Console.WriteLine(args[0]);
                enumPath = Path.Combine(applicationPath, args[0]);
                Console.WriteLine(enumPath);
                if (!Directory.Exists(enumPath))
                {
                    throw new IOException(enumPath);
                }
            }

            string connectionString = GetConnectionString(applicationPath,"");
            Console.WriteLine(connectionString);

       

           
            foreach(var enumFileName in Directory.GetFiles(enumPath).ToList().Where(a=> new FileInfo(a).Name.StartsWith("Enum")))
            {
                Console.WriteLine(enumFileName);
             
                GenerateSampleEnum(enumFileName, connectionString);
            }
        }

        public static string GetConnectionString(string rootPath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(rootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

            IConfigurationRoot configuration = builder.Build();
            string conString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(conString);
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
