using Analyzer.Core.Interfaces.Service;
using Analyzer.Core.Services;
using Analyzer.CrossCutting.Lib.Util;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Analyzer.Test
{
    public class Tests
    {
        Logger log;
        FileAnalyzerService _service;

        [SetUp]
        public void Setup()
        {
            // create new service instance
            _service = new FileAnalyzerService();

            ///setting enviroment to test project
            var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            System.Environment.CurrentDirectory = projectPath;
            //setting logger
            log = new LoggerConfiguration()
           .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
           .CreateLogger();
            //initialize service
        }

        /// <summary>
        /// Load 1 files 
        /// </summary>
        [Test]
        public void LoadSingleFile()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            log.Information("Loading single file test");

            using (StreamWriter sw = File.CreateText(Path.Combine(PathUtil.GetInputPathMonitor(), "TestFile" + ".txt")))
            {
                sw.WriteLine("001�1234567891234�Pedro�20000");
                sw.WriteLine("001�3245678865434�Paulo�40000.99");
                sw.WriteLine("001�3245678865434�Lucas�40000.99");
                sw.WriteLine("002�2345675434544345�Jose da Silva�Rural");
                sw.WriteLine("002�2345675433444345�Eduardo Pereira�Rural");
                sw.WriteLine("003�08�[1-34-10,2-33-1.50,3-40-0.10]�Paulo");
                sw.WriteLine("003�08�[1-34-10,2-33-1.50,3-40-0.10]�Pedro");

            }

            try
            {
                log.Information("File inserted");
                _service.Execute();
            }
            catch (Exception e)
            {
                log.Information(e, "An error is ocurred!");
                Assert.Fail();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            log.Information("Exec process time: {0}", TimeSpan.FromMilliseconds(elapsedMs).TotalMinutes);
        }

        /// <summary>
        /// Load 1000 files assync 
        /// </summary>
        [Test]
        public void StressTest()
        {
            log.Information("Loading 1000 file test");

            var watch = System.Diagnostics.Stopwatch.StartNew();

            Parallel.For(0, 1000, i =>
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(PathUtil.GetInputPathMonitor(), "TestFile" + i + ".txt")))
                {
                    for (int e = 0; e < 500; e++)
                    {
                        sw.WriteLine("001�1234567891234�Pedro�50000");
                        sw.WriteLine("001�3245678865434�Paulo�40000.99");
                        sw.WriteLine("002�2345675434544345�Jose da Silva�Rural");
                        sw.WriteLine("002�2345675433444345�Eduardo Pereira�Rural");
                        sw.WriteLine("003�08�[1-34-10,2-33-1.50,3-40-0.10]�Paulo");
                    }
                }
            });

            try
            {
                log.Information("Files inserted");
                _service.Execute();

            }
            catch (Exception e)
            {
                log.Information(e, "An error is ocurred!");
                Assert.Fail();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            log.Information("Exec process time: {0}", TimeSpan.FromMilliseconds(elapsedMs).TotalMinutes);
        }
    }
}