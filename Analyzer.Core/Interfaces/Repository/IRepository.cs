﻿using System.IO;
using System.Threading.Tasks;

namespace Analyzer.Core.Interfaces.Repository
{
    public interface IRepository
    {
        public void Add(string FileName, string Content);
        public FileInfo[] GetAll();
        public void DeleteFile(string FilePath);
        public void BackupFile(string FilePath);
        public Task RejectFile(FileInfo rejectedfile);

        public bool HasFiles();

    }
}
