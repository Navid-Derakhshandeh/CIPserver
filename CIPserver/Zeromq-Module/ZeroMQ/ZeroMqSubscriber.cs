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
    public sealed class ZeroMqSubscriber : IMessageSubscriber, IDisposable
    {
        private readonly SubscriberSocket _socket;

        public ZeroMqSubscriber(string address, string topic)
        {
            _socket = new SubscriberSocket();

            _socket.Connect(address);
            _socket.Subscribe(topic);
        }

        public async Task<string> SubscribeAsync(
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_socket.TryReceiveFrameString(
                        TimeSpan.FromMilliseconds(100),
                        out string? topic))
                {
                    if (_socket.TryReceiveFrameString(
                            TimeSpan.FromMilliseconds(100),
                            out string? message))
                    {
                        return message;
                    }
                }

                await Task.Yield();
            }

            throw new OperationCanceledException(cancellationToken);
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }

}
