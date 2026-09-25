using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModule.Models
{
  public class ConnectionConfig
    {
        public int Id { get; set; }

        public string Protocol { get; set; } = string.Empty;

        public string Ip { get; set; } = string.Empty;

        public int Port { get; set; }

    }
}
