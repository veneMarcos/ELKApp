using System;
using Microsoft.Extensions.Logging;

namespace KibanaElasticApp.Logger
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddESLogger(this ILoggerFactory factory, IServiceProvider serviceProvider, string indexName = null)
        {
            factory.AddProvider(new ESLoggerProvider(serviceProvider, indexName));

            return factory;
        }
    }
}
