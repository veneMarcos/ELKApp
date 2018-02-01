using System;
using KibanaElasticApp.ElasticSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace KibanaElasticApp.Logger
{
    public class ESLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ElasticClientProvider _esClient;
        private readonly string _indexName;

        public ESLoggerProvider(IServiceProvider serviceProvider, string indexName = null)
        {
            _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            _indexName = indexName;

            _esClient = serviceProvider.GetService<ElasticClientProvider>();
            _esClient.EnsureIndexWithMapping<LogEntry>(_indexName);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ESLogger(_esClient, _httpContextAccessor, categoryName, FindLevel(categoryName));
        }

        private LogLevel FindLevel(string categoryName)
        {
            var def = LogLevel.Warning;

            return def;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
