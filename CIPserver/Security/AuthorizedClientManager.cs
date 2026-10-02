using System.Collections.Concurrent;

namespace ZeroMqService
{
    public class AuthorizedClientManager
    {
        private readonly ConcurrentDictionary<string, DateTime> clients
            = new();
        public void Add(string username)
        {
            clients[username] = DateTime.UtcNow;
        }
        public void Remove(string username)
        {
            clients.TryRemove(username, out _);
        }
        public bool HasClients()
        {
            return clients.Count > 0;
        }
    }
}