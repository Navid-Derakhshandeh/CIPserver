using Microsoft.Extensions.Hosting;
using protocol_module.Protocol;
using protocol_module;
using zeromq_module.ZeroMQ;
using ModbusModule;


namespace ZeroMqService
{
    public class ZeroMqBackgroundService
        : BackgroundService
    {
        private readonly ModbusService _modbus;
        private readonly AuthorizedClientManager _clients;
        public ZeroMqBackgroundService(
            ModbusService modbus,
            AuthorizedClientManager clients)
        {
            _modbus = modbus;
            _clients = clients;
        }
        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            using var publisher =
                new ZeroMqPublisher(
                    "tcp://*:6003");
            Console.WriteLine(
                "ZeroMQ started");
            _modbus.Connect();
            while (!stoppingToken.IsCancellationRequested)
            {
                // No authorized users
                if (!_clients.HasClients())
                {
                    await Task.Delay(1000);
                    continue;
                }
                int[] registers;
                try
                {
                    registers =
                        _modbus.ReadHoldingRegisters(
                            0,
                            10);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Modbus read failed: {ex.Message}");
                    try
                    {
                        _modbus.Disconnect();
                        await Task.Delay(2000);
                        _modbus.Connect();
                        Console.WriteLine(
                            "Modbus reconnected");
                    }
                    catch (Exception reconnectError)
                    {
                        Console.WriteLine(
                            $"Reconnect failed: {reconnectError.Message}");
                    }
                    continue;
                }
                var message =
                new ProtocolMessage
                {
                    Version = 1,
                    Type = MessageType.Telemetry,
                    Source = MessageSource.Modbus,
                    DeviceId = "modbus-01",
                    TimeStamp =
                        DateTime.UtcNow,
                    Data = new ProtocolData
                    {
                        Operation =
                        "read_holding_registers",
                        Address = 0,
                        Quantity =
                            registers.Length,
                        Values = registers
                    }
                };
                string json =
                    ProtocolSerializer.Serialize(
                        message);
                await publisher.PublishAsync(
                    "telemetry",
                    json);
                Console.WriteLine(
                    "Telemetry sent");
                await Task.Delay(
                    1000,
                    stoppingToken);
            }
            _modbus.Disconnect();
        }
    }
}