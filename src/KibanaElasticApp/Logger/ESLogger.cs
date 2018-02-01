using System;
using KibanaElasticApp.ElasticSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KibanaElasticApp.Logger
{
    public class ESLogger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ElasticClientProvider _esClient;
        private readonly string _categoryName;
        private readonly LogLevel _logLevel;

        public ESLogger(ElasticClientProvider esClient, IHttpContextAccessor httpContextAccessor, string categoryName, LogLevel logLevel)
        {
            _esClient = esClient;
            _httpContextAccessor = httpContextAccessor;
            _categoryName = categoryName;
            _logLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            var entry = new LogEntry
            {
                EventId = eventId,
                DateTime = DateTime.UtcNow,
                Category = _categoryName,
                Message = message,
                Level = logLevel
            };

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                entry.TraceIdentifier = context.TraceIdentifier;
                entry.UserName = context.User.Identity.Name;
                var request = context.Request;
                entry.ContentLength = request.ContentLength;
                entry.ContentType = request.ContentType;
                entry.Host = request.Host.Value;
                entry.IsHttps = request.IsHttps;
                entry.Method = request.Method;
                entry.Path = request.Path;
                entry.PathBase = request.PathBase;
                entry.Protocol = request.Protocol;
                entry.QueryString = request.QueryString.Value;
                entry.Scheme = request.Scheme;

                entry.Cookies = request.Cookies;
                entry.Headers = request.Headers;
            }

            if (exception != null)
            {
                entry.Exception = exception.ToString();
                entry.ExceptionMessage = exception.Message;
                entry.ExceptionType = exception.GetType().Name;
                entry.StackTrace = exception.StackTrace;
            }

            // I do not using Nest, because conflict with Microsoft.Extensions.Logging
            _esClient.Client.Index(new Nest.IndexRequest<LogEntry>(entry));
        }
    }
}
