using Analyzer.Core.Entities;
using Analyzer.Core.Enums;
using Analyzer.CrossCutting.Lib.Extensions;
using Analyzer.Infrastructure.Commands;
using System;
using System.IO;
using System.Linq;

namespace Analyzer.Core.Services
{
    public class FileAnalyzerService
    {
        /// <summary>
        /// Hangfire service start point
        /// </summary>
        public static void Init()
        {
            //init object
            FileAnalyzerData fileAnalyzer = new FileAnalyzerData();
            //Get files from path
            FileInfo[] files = fileAnalyzer.GetInputFiles();
            //Extract information by each one
            foreach (var item in files) AnalyzeFile(item);
        }

        private static void AnalyzeFile(FileInfo file)
        {
            FileContent content = GetInputData(file);
            OutputFileContent outputFileContent = new OutputFileContent(content);
            SetOutputData(outputFileContent, content.Identifier, file);
        }

        private static void Classificate(ref FileContent content, string type, string[] keys)
        {
            ClassificatorEnum classificator = EnumExtensions.FromString<ClassificatorEnum>(type);

            switch (classificator)
            {
                case ClassificatorEnum.Seller:
                    //add seller
                    content.Sellers.Add(new Seller(keys[1], keys[2], keys[3]));
                    break;
                case ClassificatorEnum.Customer:
                    //add customer
                    content.Customers.Add(new Customer(keys[1], keys[2], keys[3]));
                    break;
                case ClassificatorEnum.Sale:
                    //Add sale
                    content.Sales.Add(new Sale(keys[1], keys[2], keys[3]));
                    break;
                default:
                    break;
            }
        }


        private static FileContent GetInputData(FileInfo file)
        {
            FileContent content = new FileContent();

            string[] lines = System.IO.File.ReadAllLines(file.FullName);

            #region Populate file content based on text file
            foreach (string line in lines)
            {
                //split line based on rules
                var keys = line.Split("ç");
                //get classification
                string type = keys.FirstOrDefault();
                //Populate object
                Classificate(ref content, type, keys);
            }
            #endregion

            return content;
        }

        private static void SetOutputData(OutputFileContent content, Guid Identifier,FileInfo file)
        {  //init object
            FileAnalyzerData fileAnalyzer = new FileAnalyzerData();
            fileAnalyzer.SetOutputFile(content, Identifier,file.FullName);
        }
    }
}
