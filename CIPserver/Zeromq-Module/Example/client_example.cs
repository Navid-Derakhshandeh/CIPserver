using NetMQ;
using NetMQ.Sockets;
using protocol_module;
using protocol_module.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using zeromq_module.Interfaces;
using zeromq_module.ZeroMQ;

namespace Client_Example
{
    public class ClientZeromq_Example
    {
        static async Task clientzeromq_example()
        {
            using var subscriber = new ZeroMqSubscriber("tcp://localhost:6003", "telemetry");
            Console.WriteLine("Client Started");
            Console.WriteLine("waiting for telemetry...");
            while (true)
            {
                // -----------------------------------------
                // 1. Receive JSON from ZeroMQ
                // -----------------------------------------
                string json = await subscriber.SubscribeAsync();
                Console.WriteLine();
                Console.WriteLine("Received JSON:");
                Console.WriteLine(json);
                // -----------------------------------------
                // 2. JSON -> ProtocolMessage
                // -----------------------------------------
                ProtocolMessage message = ProtocolSerializer.Deserialize(json);
                // -----------------------------------------
                // 3. Read protocol data
                // -----------------------------------------
                Console.WriteLine();
                Console.WriteLine("Parsed Data:");
                Console.WriteLine($"Type: {message.Type}");
                Console.WriteLine($"Source: {message.Source}");
                Console.WriteLine($"Device: {message.DeviceId}");
                Console.WriteLine($"Operation: {message.Data.Operation}");
                Console.WriteLine($"Address: {message.Data.Address}");
                Console.WriteLine($"Quantity: {message.Data.Quantity}");
                // -----------------------------------------
                // 4. Client gets the REAL Modbus values
                // -----------------------------------------
                int[] registers = message.Data.Values;
                for (int i = 0; i < registers.Length; i++)
                {
                    Console.WriteLine($"Registers:{registers[i]}");
                }
            }
        }
    }
}
