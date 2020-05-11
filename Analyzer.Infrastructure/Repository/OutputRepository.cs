using Analyzer.Core.Entities;
using Analyzer.Core.Interfaces.Repository;
using Analyzer.CrossCutting.Lib.Util;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Infrastructure.Repository
{
    public class OutputRepository : IRepository

    {  /// Save File
       /// </summary>
       /// <param name="Path"></param>
        public void Add(string FileName, string Content)
        {
            // Create a new file     
            using (FileStream fs = File.Create(Path.Combine(PathUtil.GetOutputPathMonitor(), FileName)))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(Content);
                fs.Write(title, 0, title.Length);
            }
        }

        public void AddFile(OutputFileContent content)
        {
            string FileName = content.Identifier.ToString() + ".txt";
            // Create a new file     
            using (StreamWriter sw = File.CreateText(Path.Combine(PathUtil.GetOutputPathMonitor(), FileName)))
            {
                sw.WriteLine("Sales report.");
                sw.WriteLine("Customer quantity: {0}", content.CustomerQuantity);
                sw.WriteLine("Sellers quantity: {0}", content.SellerQuantity);
                sw.WriteLine("Most expensive sale Id: {0}", content.IdMostExpansiveSale);
                sw.WriteLine("Worst seller: {0}", content.WorstSeller);
            }

        }

        /// <summary>
        /// Read files from input directory
        /// </summary>
        /// <returns></returns>
        public FileInfo[] GetAll()
        {
            //Get files from directory
            DirectoryInfo d = new DirectoryInfo(PathUtil.GetOutputPathMonitor());
            //Get Text Files
            FileInfo[] Files = d.GetFiles("*.txt");
            return Files;
        }

        /// <summary>
        /// Delete file from input directory
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public void DeleteFile(string FilePath)
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
        /// <summary>
        /// Move File
        /// </summary>
        /// <param name="FilePath"></param>
        public void BackupFile(string FilePath)
        {
            string FileName = Guid.NewGuid().ToString() + ".txt";

            string DestinationPath = Path.Combine(PathUtil.GetBackupPath(), FileName);

            File.Copy(FilePath, DestinationPath, true);
        }

        public bool HasFiles()
        {
            return Directory.EnumerateFileSystemEntries(PathUtil.GetOutputPathMonitor()).Any();
        }

        public Task RejectFile(FileInfo rejectedfile)
        {
            var t = Task.Run(() =>
            {
                string DestinationPath = Path.Combine(PathUtil.GetRejectedPathMonitor(), rejectedfile.Name);

                File.Move(rejectedfile.FullName, DestinationPath, true);
            });
            t.ConfigureAwait(false);

            return t;
        }
    }
}
