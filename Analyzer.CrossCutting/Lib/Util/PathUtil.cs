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

            return Path.Combine(dir, "Analyzer/Analyzer.Infrastructure/Data/in");
        }

        public static string GetOutputPathMonitor()
        {
            var dir = GetBasePath();

            //Assuming Test is your Folder
            return Path.Combine(dir, "Analyzer/Analyzer.Infrastructure/Data/out");
        }

        private static string GetBasePath()
        {
            var enviroment = System.Environment.CurrentDirectory;
            return Directory.GetParent(enviroment).Parent.FullName;
        }
    }
}
