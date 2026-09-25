using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace protocol_module.Protocol
{
    public sealed class ProtocolData
    {
        public string Operation { get; set; } = string.Empty;
        public int Address { get; set; }
        public int Quantity { get; set; }
        public int[] Values {get; set;} = Array.Empty<int>();
    }
}
