using Analyzer.Core.Entities;
using Analyzer.Core.Enums;
using Analyzer.Core.Interfaces.Repository;
using Analyzer.CrossCutting.Lib.Extensions;
using Analyzer.CrossCutting.Lib.Util;
using Analyzer.Infrastructure.Repository;
using Hangfire;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Analyzer.Core.Services
{
    public class FileAnalyzerService
    {
        /// <summary>
        /// Hangfire service start point
        /// </summary>
        /// 
        [DisableConcurrentExecution(5)]
        public void Execute()
        {
            try
            {
                //init object
                IRepository _InputRepository = new InputRepository();

                if (_InputRepository.HasFiles())
                {
                    OutputRepository _outputRepository = new OutputRepository();

                    //Get files from path
                    FileInfo[] files = _InputRepository.GetAll();

                    //Extract information by each one
                    Parallel.ForEach(files, item =>
                    {
                        OutputFileContent outputFileContent = AnalyzeFile(item);

                        //Save file in output diretory
                        _outputRepository.AddFile(outputFileContent);
                        //create backup
                        _InputRepository.BackupFile(item.FullName);
                        //delete from input folder
                        _InputRepository.DeleteFile(item.FullName);
                    });

                }
            }
            catch (IOException) { }
            catch (Exception) { }
        }

        /// <summary>
        /// Analyze file content
        /// </summary>
        /// <param name="file"></param>
        private static OutputFileContent AnalyzeFile(FileInfo file)
        {
            FileContent content = ParseToFileContent(file);

            OutputFileContent outputFileContent = new OutputFileContent(content);

            return outputFileContent;
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
    }
}
