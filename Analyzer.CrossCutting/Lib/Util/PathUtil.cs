using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace Analyzer.CrossCutting.Lib.Util
{
    public class PathUtil
    {
        public static string GetInputPathMonitor()
        {
            var dir = GetBasePath();
            dir = Path.Combine(dir, "Analyzer/Data/in");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        public static string GetOutputPathMonitor()
        {
            var dir = GetBasePath();
            dir = Path.Combine(dir, "Analyzer/Data/out");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        public static string GetRejectedPathMonitor()
        {
            var dir = GetBasePath();
            dir = Path.Combine(dir, "Analyzer/Data/rejected");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        public static string GetBackupPath()
        {
            var dir = GetBasePath();
            dir = Path.Combine(dir, "Analyzer/Analyzer.Infrastructure/Data/backup");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        private static string GetBasePath()
        {
            var enviroment = System.Environment.CurrentDirectory;
            return Directory.GetParent(enviroment).Parent.FullName;
        }


        public static string[] ExtractInformation(FileInfo file)
        {
            return System.IO.File.ReadAllLines(file.FullName);
        }
    }
}
