using log4net;
using log4net.Core;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server.Logger
{
    public class LoggerHelper
    {

        public    LoggerHelper()
        {

        }
        private ILog _log=null;
        public ILog log
        {
            get
            {


                if (_log == null)
                {
                    log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(Application.StartupPath, "log4net.xml")));
                    //从配置文件中读取Logger对象  
                    //WebLogger 里面的配置信息是用来将日志录入到数据库的
                    //做为扩展 做判断来确定日志的记录形式，数据库也好，txt文档也好，控制台程序也好。
                    _log = log4net.LogManager.GetLogger("myLogger"); //log4net.LogManager.GetLogger("WebLogger");
                }
                return _log;
            }
        }
        /// <summary>
        /// 调试
        /// </summary>
        public   void debug()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }


        /// <summary>
        /// 错误
        /// </summary>
        public   void error()
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// 严重错误
        /// </summary>
        public   void fatal()
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }

        /// <summary>
        /// 记录一般日志
        /// </summary>
        public   void info()
        {
            if (log.IsInfoEnabled)
            {
                //log.Info("Jerry");
                log.Info(message);
            }
        }


        /// <summary>
        /// 记录警告
        /// </summary>
        public   void warn()
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }
        LogMessage message = null;

        /// <summary>  
        /// 需要写日志的地方调用此方法  
        /// </summary>  
        /// <param name="level">自定义级别</param>  
        public   void SaveMessage(LogMessage logMessage, int level)
        {
            message = logMessage;

            switch (level)
            {
                case 1:
                    info();
                    break;

                case 2:
                    warn();
                    break;

                case 3:
                    error();
                    break;

                case 4:
                    fatal();
                    break;

                default: break;
            }
        }
    }

    public class LogMessage
    {
        public string SN { get; set; }
        public string MAC { get; set; }
        public string EndPoint { get; set; }
        public string CodeName { get; set; }
        public string ExceptionString { get; set; }
    }
    public class SNCustomLayout : log4net.Layout.PatternLayout
    {
        public SNCustomLayout()
        {
            this.AddConverter("SN", typeof(SNPatternConverter));
        }
    }
    public class MACCustomLayout : log4net.Layout.PatternLayout
    {
        public MACCustomLayout()
        {
            this.AddConverter("MAC", typeof(MACPatternConverter));
        }
    }
    public class EndPointCustomLayout : log4net.Layout.PatternLayout
    {
        public EndPointCustomLayout()
        {
            this.AddConverter("EndPoint", typeof(EndPointPatternConverter));
        }
    }
    public class CodeNameCustomLayout : log4net.Layout.PatternLayout
    {
        public CodeNameCustomLayout()
        {
            this.AddConverter("CodeName", typeof(CodeNamePatternConverter));
        }
    }
    public class ExceptionStringCustomLayout : log4net.Layout.PatternLayout
    {
        public ExceptionStringCustomLayout()
        {
            this.AddConverter("ExceptionString", typeof(ExceptionStringPatternConverter));
        }
    }
    internal sealed class SNPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.SN);

            }
        }
    }
    internal sealed class MACPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.MAC);

            }
        }
    }
    internal sealed class EndPointPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.EndPoint);

            }
        }
    }
    internal sealed class CodeNamePatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.CodeName);

            }
        }
    }
    internal sealed class ExceptionStringPatternConverter : PatternLayoutConverter
    {
        override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
            {
                writer.Write(logMessage.ExceptionString);

            }
        }
    }
}
