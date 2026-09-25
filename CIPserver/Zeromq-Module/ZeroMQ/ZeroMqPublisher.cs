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
    public sealed class ZeroMqPublisher : IMessagePublisher, IDisposable
    {
        private readonly PublisherSocket _socket;

        public ZeroMqPublisher(string address)
        {
            _socket = new PublisherSocket();
            _socket.Bind(address);
        }

        public Task PublishAsync(
            string topic,
            string message,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _socket
                .SendMoreFrame(topic)
                .SendFrame(message);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }

}
