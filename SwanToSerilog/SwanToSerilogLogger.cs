using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace SwanToSerilog
{
    /// <summary>
    /// Transfers log messages from swan logger to serilog
    /// </summary>
    public class SwanToSerilogLogger : Swan.Logging.ILogger
    {
        private readonly ILogger _log;

        public SwanToSerilogLogger(ILogger log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //do nothing, no disposable parts
        }

        public void Log(Swan.Logging.LogMessageReceivedEventArgs logEvent)
        {
            var sourceName = logEvent.Source;
            var logLevel = ConvertLogLevel(logEvent.MessageType);
            var exception = logEvent.Exception;
            var msg = logEvent.Message;
            var log = _log;
            if (!string.IsNullOrWhiteSpace(sourceName))
            {
                log = _log.ForContext("SourceContext", sourceName);
            }
            log.Write(logLevel, exception, msg);
        }

        public static LogEventLevel ConvertLogLevel(Swan.Logging.LogLevel level)
        {
            var num = (int)level;
            return (LogEventLevel)Math.Max(num - 1, 0);
        }

        public Swan.Logging.LogLevel LogLevel { get; set; }
    }
}
