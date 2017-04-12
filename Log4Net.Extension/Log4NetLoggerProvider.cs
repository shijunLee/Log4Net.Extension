using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Log4Net.Extension
{
    public  class Log4NetLoggerProvider:ILoggerProvider
    {
        private Log4NetProviderOptions options;

        public Log4NetLoggerProvider()
        {
            options = new Log4NetProviderOptions();
            options.RepositoryName = "OGLog";
        }

        public Log4NetLoggerProvider(Log4NetProviderOptions _options)
        {
            options = _options;
        }

        public ILogger CreateLogger(string categoryName)
        {

            return new Log4NetLogger(categoryName, options);
        }

        public void Dispose()
        {
             
        }
    }
}
