using LoggerModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggerModule.Interfaces;
using System.IO;

namespace LoggerModule.File
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        public FileLogger(string filepath)
        {
            _filePath = filepath;
        }
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
            Log("WARN", message);
        }
        public void Error(string message)
        {
            Log("ERROR", message);
        }
        public void Fatal(string message)
        {
            Log("FATAL", message);
        }
        private void Log(string level, string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}" + $"[{level}] {message}";
            System.IO.File.AppendAllText(_filePath, logMessage + Environment.NewLine);
        }
    }
}
