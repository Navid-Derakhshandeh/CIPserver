using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zeromq_module.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string topic, string message, CancellationToken cancellationToken = default);
    }
}
