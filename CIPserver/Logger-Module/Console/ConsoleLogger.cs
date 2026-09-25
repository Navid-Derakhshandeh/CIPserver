using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggerModule.Interfaces;

namespace LoggerModule.Console
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            Log("DEBUG", message);
        }

        public void Info(string message)
        {
            Log("INFO", message);
        }
        public void Warn(string message)
        {
            Log("Warn", message);
        }
        public void Error(string message)
        {
            Log("Error", message);
        }
        public void Fatal(string message)
        {
            Log("Fatal", message);
        }
        private void Log(string level, string message)
        {
            System.Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}");
        }
    }
}
