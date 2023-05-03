using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Swan.Logging;
using SwanToSerilog;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //enricher allows to remove namespace from SourceContext
                .Enrich.With<SourceContextOnlyClassNameEnricher>()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
                .CreateLogger();
            Swan.Logging.Logger.UnregisterLogger<Swan.Logging.ConsoleLogger>();
            var swanToSerilog = new SwanToSerilogLogger(Log.Logger);
            Swan.Logging.Logger.RegisterLogger(swanToSerilog);

            "This is test string".Warn(nameof(Program));
            Console.ReadLine();

            (new Exception("Failed!")).Error(nameof(Program), "Problems!");
            Console.ReadLine();
        }
    }
}
