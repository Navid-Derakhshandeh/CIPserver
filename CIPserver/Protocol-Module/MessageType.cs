using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace protocol_module.Protocol
{
    public static class MessageType
    {
        public const string Telemetry = "telemetry";
        public const string Command = "command";
        public const string Status = "status";
        public const string Response = "response";
        public const string Error = "error";
    }
}
