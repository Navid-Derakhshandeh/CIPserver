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
    public sealed class ZeroMqReceiver : IMessageReceiver, IDisposable
    {
        private readonly PullSocket  _socket;
        public ZeroMqReceiver(string address)
        {
            _socket = new PullSocket();
            _socket.Bind(address);
        }
        public async Task<string> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                if(_socket.TryReceiveFrameString(TimeSpan.FromMicroseconds(100), out string? message))
                {
                    return message;
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
