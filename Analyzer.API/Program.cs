using Analyzer.CrossCutting.Lib.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Serilog configuration
            LoggerExtension logger = new LoggerExtension();
            logger.CreateLogger();

            CreateHostBuilder(args).Build().Run();
            #endregion
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
