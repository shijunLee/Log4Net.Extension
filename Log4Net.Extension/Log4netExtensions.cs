using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using log4net.Repository;
using System.Collections;
using log4net.Config;
using log4net;

namespace Log4Net.Extension
{
    public static class Log4netExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            return AddLog4Net(factory, null);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Log4NetProviderOptions options)
        {
            
            if (options == null)
            {
                options = new Log4NetProviderOptions();
                options.RepositoryName = "OGLog";
            }
            ILoggerRepository rep = LogManager.CreateRepository(options.RepositoryName);
            var xmlDocument = Log4NetConfig.GetLog4NetConfig(options.Log4NetConfigXmlPath);
            ICollection configurationMessages = XmlConfigurator.Configure(rep, xmlDocument["log4net"]);
            using (var provider = new Log4NetLoggerProvider(options))
            {
                factory.AddProvider(provider);
            }
            return factory;
        }
    }
}
