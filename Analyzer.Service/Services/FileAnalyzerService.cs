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
        Serilog.Core.Logger _logger;

        public FileAnalyzerService()
        {
            LoggerExtension logger = new LoggerExtension();
            _logger = logger.CreateLogger();
        }


        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
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
                        OutputFileContent outputFileContent = null;

                        try
                        {
                            //try parse files
                            outputFileContent = AnalyzeFile(item);
                        }
                        catch (Exception ex)
                        {
                            //Move to rejected folder if has error to parse
                            _InputRepository.RejectFile(item);
                            _logger.Error(ex, $"The file {item.Name} cannot be converted and has been moved to the rejected folder.");

                            //move to next interaction
                            return;
                        }

                        //log the processed information
                        _logger.Information($"Processed file {item.FullName} output: {Serializer.Serialize(outputFileContent)}");

                        //Save file in output diretory
                        _outputRepository.AddFile(outputFileContent);
                        //create backup
                        _InputRepository.BackupFile(item.FullName);
                        //delete from input folder
                        _InputRepository.DeleteFile(item.FullName);
                    });

                }
            }
            catch (IOException ex)
            {
                _logger.Error(ex, "An I/O error is ocurred");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error is ocurred");
            }
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
