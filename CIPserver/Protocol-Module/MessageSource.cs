using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace protocol_module.Protocol
{
    public static class MessageSource
    {
        public const string Modbus = "modbus";
        public const string Profinet = "profinet";
        public const string Server = "server";
        public const string Client = "client";
    }
}
