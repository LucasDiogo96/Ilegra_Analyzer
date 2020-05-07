using Analyzer.Core.Interfaces.Service;
using Analyzer.Core.Services;
using Hangfire;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //add Hangfire monitor service with mongodb
            services.AddHangfire(config =>
            {
                var migrationOptions = new MongoMigrationOptions
                {
                    Strategy = MongoMigrationStrategy.Drop,
                    BackupStrategy = MongoBackupStrategy.Collections
                };
                var storageOptions = new MongoStorageOptions
                {
                    MigrationOptions = migrationOptions
                };

                config.UseMongoStorage(Configuration.GetConnectionString("HangfireDatabase"), storageOptions);

                services.AddSingleton<IFileAnalyzerService, FileAnalyzerService>();
            });

            services.AddHangfireServer();

            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            #region custom pipeline configuration

            app.UseHangfireDashboard();


            IFileAnalyzerService analyzerService = new FileAnalyzerService();

            //Start Hanfire background service

            //10 seg interval to call the recurrent job
            RecurringJob.AddOrUpdate(
                () => analyzerService.Start(),
              "*/5 * * * * *");

            app.UseSerilogRequestLogging();

            #endregion

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
