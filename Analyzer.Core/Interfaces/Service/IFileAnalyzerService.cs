using Hangfire.Server;

namespace Analyzer.Core.Interfaces.Service
{
    public interface IFileAnalyzerService
    {
        public void Start(BackgroundProcessContext context);
    }

}
