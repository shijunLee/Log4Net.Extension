
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Log4Net.Extension
{
    public class Log4NetConfig
    {
        public static XmlDocument GetLog4NetConfig(string path)
        {
            XmlDocument document = new XmlDocument();
            const string xmlstr = @"
                    <?xml version=""1.0"" encoding=""utf-8"" ?>
                        <log4net>
                           <appender name = ""RollingLogFileAppender"" type = ""log4net.Appender.RollingFileAppender"" >
                                <file value = ""./log/"" /> 
                                <appendToFile value = ""true"" />
                                <rollingStyle value = ""Date"" />
                                <datePattern value = ""yyyy/yyyyMM/yyyyMMdd'.txt'"" />
                                <staticLogFileName value = ""false"" />
                                <param name = ""MaxSizeRollBackups"" value = ""100"" />
                                <layout type = ""log4net.Layout.PatternLayout"" > 
                                    <conversionPattern value = ""%newline %n记录时间：%date %n线程ID:[%thread] %n日志级别：  %-5level %n出错类：%logger property: [%property{NDC}] - %n错误描述：%message%newline %n"" />
                                    <lockingModel type=""log4net.Appender.FileAppender + MinimalLock"" />   
                                </layout> 
                            </appender> 
                            <root> 
                                <level value =""INFO""/>
                                <appender-ref ref= ""RollingLogFileAppender"" />
                                <appender-ref ref= ""ConsoleAppender"" />
                            </root> 
                        </log4net> ";
            try
            {
                if (path == null)
                {
                    document.LoadXml(xmlstr);
                }
                else
                {
                    using (Stream stream = File.Open(path, FileMode.Open))
                    {
                        document.Load(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return document;
        }
    }
}
