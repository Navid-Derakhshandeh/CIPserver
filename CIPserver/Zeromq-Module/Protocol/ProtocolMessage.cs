using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace protocol_module.Protocol
{
    public sealed class ProtocolMessage
    {
        public int Version { get; set; } = 1;
        public string MessageId { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public ProtocolData Data { get; set; } = new();

    }
}
