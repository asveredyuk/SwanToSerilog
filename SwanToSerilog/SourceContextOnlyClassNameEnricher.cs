using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;

namespace SwanToSerilog
{
    public class SourceContextOnlyClassNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var property))
            {
                var scalarValue = property as ScalarValue;
                var value = scalarValue?.Value as string;
                if (value == null)
                {
                    return;
                }
                var lastElement = value.Split('.').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(lastElement))
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("SourceContext", new ScalarValue(lastElement)));
                }

            }
        }
    }
}
