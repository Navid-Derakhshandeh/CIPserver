using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusModule;
using System.IO.Ports;

namespace Example
{
    public class Modbus_Example()
    {
        static void modbus_example()
        {
            var modbus = new ModbusService("192.168.1.101", 502);
            try
            {
                modbus.Connect();
                int[] registers = modbus.ReadHoldingRegisters(startAddress: 0, quantity: 10);

                Console.WriteLine("Registers:");
                for (int i = 0; i < registers.Length; i++)
                {
                    Console.WriteLine($"Registers{i}:{registers[i]}");
                }
                modbus.WriteMultipleRegisters(address: 9, value: [700]);
                Console.WriteLine(registers[9]);
                for (int i = 0; i < registers.Length; i++)
                {
                    Console.WriteLine($"Registers{i}:{registers[i]}");
                }
                modbus.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }
        }
       
    }
}
