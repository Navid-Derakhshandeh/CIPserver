using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zeromq_module.Interfaces;
using zeromq_module.ZeroMQ;
using System.IO.Ports;
using ModbusModule;
using NetMQ;
using NetMQ.Sockets;
using protocol_module;
using protocol_module.Protocol;

namespace ServerExample
{
    public class ServerZeromq_Example
    {
        public static  async Task serverzeromq_example()
        {
            var modbus = new ModbusService("127.0.0.1", 502);
            using var publisher = new ZeroMqPublisher("tcp://*:6003");
            using var receiver = new ZeroMqReceiver("tcp://*:6002");
            Console.WriteLine("Server Started.");
            try
            {
                modbus.Connect();
                while (true)
                {
                    // -----------------------------------------
                    // 1. Read REAL data from Modbus
                    // -----------------------------------------
                    int[] registers = modbus.ReadHoldingRegisters(startAddress: 0, quantity: 10);
                    Console.WriteLine();
                    Console.WriteLine("Modbus registers:");
                    for (int i = 0; i < registers.Length; i++)
                    {
                        Console.WriteLine($"Register{i} : {registers[i]}");
                    }
                    // -----------------------------------------
                    // 2. Put REAL Modbus data into protocol
                    // -----------------------------------------
                    var message = new ProtocolMessage
                    {
                        Version = 1,
                        Type = MessageType.Telemetry,
                        Source = MessageSource.Modbus,
                        DeviceId = "modbus-01",
                        TimeStamp = DateTime.UtcNow,
                        Data = new ProtocolData
                        {
                            Operation = "read_holding_registers",
                            Address = 0,
                            Quantity = registers.Length,
                            Values = registers
                        }
                    };
                    // -----------------------------------------
                    // 3. Protocol object -> JSON
                    // -----------------------------------------
                    string json = ProtocolSerializer.Serialize(message);
                    Console.WriteLine();
                    Console.WriteLine("Sending:");
                    Console.WriteLine(json);
                    // -----------------------------------------
                    // 4. JSON -> ZeroMQ
                    // -----------------------------------------
                    await publisher.PublishAsync("telemetry", json);
                    // Read every second
                    await Task.Delay(1000);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
            finally
            {
                modbus.Disconnect();
            }
        }
    }
}
