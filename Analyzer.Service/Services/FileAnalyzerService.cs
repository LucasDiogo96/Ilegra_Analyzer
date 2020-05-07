using Analyzer.Core.Entities;
using Analyzer.Core.Enums;
using Analyzer.Core.Interfaces.Repository;
using Analyzer.Core.Interfaces.Service;
using Analyzer.CrossCutting.Lib.Extensions;
using Analyzer.CrossCutting.Lib.Util;
using Analyzer.Infrastructure.Repository;
using System;
using System.IO;
using System.Linq;

namespace Analyzer.Core.Services
{
    public class FileAnalyzerService : IFileAnalyzerService
    {

        //private readonly ILogger<FileAnalyzerService> _logger;
        //public FileAnalyzerService(ILogger<FileAnalyzerService> logger)
        //{
        //    _logger = logger;
        //}

        /// <summary>
        /// Hangfire service start point
        /// </summary>
        public void Start()
        {
            try
            {
                //init object
                IRepository _InputRepository = new InputRepository();
                //Get files from path
                FileInfo[] files = _InputRepository.GetAll();

                //Extract information by each one
                foreach (var item in files)
                {
                    AnalyzeFile(item);

                    _InputRepository.BackupFile(item.FullName);
                    _InputRepository.DeleteFile(item.FullName);
                }
            }
            catch (Exception e)
            {
                //_logger.LogError(e, e.Message);
                //throw to hangfire
                throw;
            }
        }

        /// <summary>
        /// Analyze file content
        /// </summary>
        /// <param name="file"></param>
        private static void AnalyzeFile(FileInfo file)
        {
            FileContent content = ParseToFileContent(file);

            OutputFileContent outputFileContent = new OutputFileContent(content);

            Save(outputFileContent, file);

        }

        /// <summary>
        /// Parte a file to object content
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static FileContent ParseToFileContent(FileInfo file)
        {
            string Delimiter = "ç";

            FileContent content = new FileContent();

            string[] lines = PathUtil.ExtractInformation(file);

            #region Populate file content based on text file

            foreach (string line in lines)
            {
                //Split line based on rules
                var keys = line.Split(Delimiter);
                //Get classification
                ClassificatorEnum Type = EnumExtensions.FromString<ClassificatorEnum>(keys.FirstOrDefault());
                //Populate object
                Classificate(ref content, Type, keys);
            }
            #endregion

            return content;
        }


        /// <summary>
        /// Classificate and parse from text information to c# object
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="keys"></param>
        private static void Classificate(ref FileContent content, ClassificatorEnum Type, string[] keys)
        {
            switch (Type)
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

        /// <summary>
        /// Save content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="Identifier"></param>
        /// <param name="file"></param>
        private static void Save(OutputFileContent content, FileInfo file)
        {
            IRepository _InputRepository = new OutputRepository();

            string key = String.Join("-", new String[]
                {
                    content.CustomerQuantity.ToString(),
                    content.IdMostExpansiveSale.ToString(),
                    content.SellerQuantity.ToString(),
                    content.WorstSeller
                });

            _InputRepository.Add(content.Identifier.ToString() + ".txt", key);
        }
    }
}
