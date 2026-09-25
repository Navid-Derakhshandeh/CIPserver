using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

namespace ModbusModule
{
    public class ModbusService
    {
        private readonly ModbusClient _client;

        public ModbusService(string ip, int port)
        {
            _client = new ModbusClient(ip, port);
        }
        public bool IsConnected => _client.Connected;

        public void Connect()
        {
            if(!_client.Connected)
            {
                _client.Connect();
                Console.WriteLine("Modbus Connected");
            }
        }
        public void Disconnect()
        {
            if (_client.Connected)
            {
                _client.Disconnect();
                Console.WriteLine("Modbus Disconnected");
            }
        }
        public int[] ReadHoldingRegisters(int startAddress, int quantity)
        {
            if(!_client.Connected)
                throw new InvalidOperationException("Modbus Is Not Connected");

            return _client.ReadHoldingRegisters(startAddress, quantity);
        }
        public void WriteMultipleRegisters(int address, int[] value)
        {
            if (!_client.Connected)
                throw new InvalidOperationException("Modbus Is Not Connected");
            _client.WriteMultipleRegisters(address, value);
        }
    }
}
