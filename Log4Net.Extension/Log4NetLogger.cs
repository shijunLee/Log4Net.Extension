using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Log4Net.Extension
{
    internal class Log4NetLogger : ILogger
    {

        private readonly log4net.ILog _logger;
        private readonly Log4NetProviderOptions _options;

        public Log4NetLogger(string categoryName, Log4NetProviderOptions options)
        {

            _logger = log4net.LogManager.GetLogger(options.RepositoryName, categoryName);
            _options = options;
        }

        public IDisposable BeginScope<TState>(TState state)
        { 
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return IsEnabled(ConvertLogLevel(logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log4NetLevel = ConvertLogLevel(logLevel);
            if (IsEnabled(log4NetLevel))
            {
                if (formatter == null)
                {
                    throw new ArgumentNullException(nameof(formatter));
                } 
                var message = formatter(state, exception); 

                if (!string.IsNullOrEmpty(message))
                {
                    var logMessage = $"EventName:{eventId.Name},EventId:{eventId.Id},Message:{message}";
                    switch (logLevel)
                    { 
                        case LogLevel.Debug:
                            _logger.Debug(logMessage, exception);
                            break;
                        case LogLevel.Information:
                            _logger.Info(logMessage, exception);
                            break;
                        case LogLevel.Warning:
                            _logger.Warn(logMessage, exception);
                            break;
                        case LogLevel.Error:
                            _logger.Error(logMessage, exception);
                            break;
                        case LogLevel.Critical:
                            _logger.Fatal(logMessage, exception);
                            break;
                        default:
                            _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                            _logger.Info(logMessage, exception);
                            break;
                    } 
                }
            }
        }

        /// <summary>
        /// get log4net enabled level
        /// </summary>
        /// <param name="level">log4net</param>
        /// <returns></returns>
        private bool IsEnabled(log4net.Core.Level level)
        { 
            switch (level.Name)
            {
                case "ERROR":
                    return _logger.IsErrorEnabled;
                case "WARN":
                    return _logger.IsWarnEnabled;
                case "INFO":
                    return _logger.IsInfoEnabled;
                case "DEBUG":
                    return _logger.IsDebugEnabled;
                case "TRACE":
                    return _logger.IsInfoEnabled;
                case "FATAL":
                    return _logger.IsFatalEnabled;
                case "OFF":
                    return true;
                default:
                    return _logger.IsDebugEnabled;
            }
        }

        /// <summary>
        /// Convert loglevel to Log4Net.Level variant.
        /// </summary>
        /// <param name="logLevel">level to be converted.</param>
        /// <returns></returns>
        private static log4net.Core.Level ConvertLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return log4net.Core.Level.Trace;
                case LogLevel.Debug:
                    return log4net.Core.Level.Debug;
                case LogLevel.Information:
                    return log4net.Core.Level.Info;
                case LogLevel.Warning:
                    return log4net.Core.Level.Warn;
                case LogLevel.Error:
                    return log4net.Core.Level.Error;
                case LogLevel.Critical:
                    return log4net.Core.Level.Fatal;
                case LogLevel.None:
                    return log4net.Core.Level.Off;
                default:
                    return log4net.Core.Level.Debug;
            }
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}
