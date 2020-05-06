using Analyzer.Core.Entities;
using Analyzer.CrossCutting.Lib.Util;
using System;
using System.IO;
using System.Text;

namespace Analyzer.Infrastructure.Commands
{
    /// <summary>
    /// Running in background
    /// </summary>
    public class FileAnalyzerData
    {

        public FileInfo[] GetInputFiles()
        {
            //Get files from directory
            DirectoryInfo d = new DirectoryInfo(PathUtil.GetInputPathMonitor());
            //Get Text Files
            FileInfo[] Files = d.GetFiles("*.txt");
            return Files;
        }

        public void SetOutputFile(OutputFileContent content, Guid Identifier)
        {

            string path = Path.Combine(PathUtil.GetOutputPathMonitor(), Identifier.ToString() + ".txt");

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(path))
                File.Delete(path);

            // Create a new file     
            using (FileStream fs = File.Create(path))
            {

                string key = String.Join("-", new String[]
                { 
                    content.CustomerQuantity.ToString(), 
                    content.IdMostExpansiveSale.ToString(), 
                    content.SellerQuantity.ToString(), 
                    content.WorstSeller 
                });
 
                Byte[] title = new UTF8Encoding(true).GetBytes(key);
                fs.Write(title, 0, title.Length);
            }
        }
    }
}
