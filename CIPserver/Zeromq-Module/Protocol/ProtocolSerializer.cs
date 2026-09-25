using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace protocol_module.Protocol
{
    public static class ProtocolSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        public static string Serialize(ProtocolMessage message)
        {
            return JsonSerializer.Serialize(message, Options);
        }
        public static ProtocolMessage Deserialize(string json)
        {
            ProtocolMessage? message = JsonSerializer.Deserialize<ProtocolMessage>(json, Options);
            if(message == null)
            {
                throw new InvalidOperationException("Invalid Protocol Message");
            }
            return message;
        }
    }
}
