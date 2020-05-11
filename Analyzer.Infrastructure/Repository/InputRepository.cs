using Analyzer.Core.Interfaces.Repository;
using Analyzer.CrossCutting.Lib.Util;
using Hangfire.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Infrastructure.Repository
{
    public class InputRepository : IRepository
    {
        /// <summary>
        /// Save File
        /// </summary>
        /// <param name="Path"></param>
        public void Add(string FileName, string Content)
        {
            // Create a new file     
            using (FileStream fs = File.Create(Path.Combine(PathUtil.GetInputPathMonitor(), FileName)))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(Content);
                fs.Write(title, 0, title.Length);
            }
        }
        /// <summary>
        /// Read files from input directory
        /// </summary>
        /// <returns></returns>
        public FileInfo[] GetAll()
        {
            //Get files from directory
            DirectoryInfo d = new DirectoryInfo(PathUtil.GetInputPathMonitor());
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
            return Directory.EnumerateFileSystemEntries(PathUtil.GetInputPathMonitor()).Any();
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