using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using protocol_module;
using protocol_module.Protocol;

namespace Protocol_Example
{
    public class protocol_example
    {
        static void main()
        {
            var message = new ProtocolMessage
            {
                Version = 1,
                Type = MessageType.Telemetry,
                DeviceId = "modbus-01",
                Data = new ProtocolData
                {
                    Operation = "read_holding_registers",
                    Address = 0,
                    Quantity = 5,
                    Values = new[]
                  {
                        100,
                        200,
                        300,
                        400,
                        500
                    }
                }
            };
            Console.WriteLine("Original Object:");
            Console.WriteLine();
            Console.WriteLine($"Type:{message.Type}");
            Console.WriteLine($"Source: {message.Source}");
            Console.WriteLine($"Device: {message.DeviceId}");
            Console.WriteLine($"Operation: {message.Data.Operation}");
            Console.WriteLine($"Address: {message.Data.Address}");
            Console.WriteLine($"Quantity: {message.Data.Quantity}");
            Console.WriteLine();
            Console.WriteLine("Values:");
            foreach (int value in message.Data.Values)
            {
                Console.WriteLine(value);
            }
            // Convert ProtocolMessage -> Json
            string json = ProtocolSerializer.Serialize(message);
            Console.WriteLine();
            Console.WriteLine("Deserialized Object:");
            Console.WriteLine();

            // Convert JSON -> ProtocolMessage
            var received = ProtocolSerializer.Deserialize(json);
            Console.WriteLine();
            Console.WriteLine("Deserialized object:");
            Console.WriteLine();
            Console.WriteLine(
                $"Type: {received.Type}");

            Console.WriteLine(
                $"Source: {received.Source}");

            Console.WriteLine(
                $"Device: {received.DeviceId}");

            Console.WriteLine(
                $"Operation: {received.Data.Operation}");

            Console.WriteLine(
                $"Address: {received.Data.Address}");

            Console.WriteLine(
                $"Quantity: {received.Data.Quantity}");

            Console.WriteLine();
            Console.WriteLine("Values:");

            foreach (int value in received.Data.Values)
            {
                Console.WriteLine(value);
            }
        }
    }
}
