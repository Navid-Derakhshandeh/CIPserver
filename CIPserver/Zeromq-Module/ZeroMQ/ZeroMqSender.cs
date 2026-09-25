using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using zeromq_module.Interfaces;

namespace zeromq_module.ZeroMQ
{
    public sealed class ZeroMqSender : IMessageSender, IDisposable
    {
        private readonly PushSocket _socket;
        public ZeroMqSender(string address)
        {
            _socket = new PushSocket();
            _socket.Connect(address);
        }
        public Task SendAsync(string message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _socket.SendFrame(message);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _socket.Dispose();
        }
    }
}
