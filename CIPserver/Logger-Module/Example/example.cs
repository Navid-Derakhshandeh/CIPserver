using LoggerModule.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggerModule.Interfaces;

namespace Example
{
    public class Logger_Example
    {
        static void logger_example()
        {
            ILogger logger = new ConsoleLogger();
            logger.Debug("Debug Message");
            logger.Info("Application is started");
            logger.Warn("Temperature Is High");
            logger.Error("Connection Failed");
            logger.Fatal("Application cannot Continue");
        }
    }
}
