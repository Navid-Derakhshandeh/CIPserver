
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zeromq_module.Interfaces
{
    public interface IMessageSender
    {
        Task SendAsync(string message, CancellationToken cancellationToken = default);
    }
}
