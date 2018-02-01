using System;
using KibanaElasticApp.ElasticSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KibanaElasticApp.Logger
{
    public class ESLogger : ILogger
    {
        public ESLogger(ElasticClientProvider esClient, IHttpContextAccessor )
        {

        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
