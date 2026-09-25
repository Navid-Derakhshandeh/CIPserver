using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using ModbusModule;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ModbusGrpc
{
    [Authorize]
    public class ModbusGrpcService :
        ModbusService.ModbusServiceBase
    {
        private readonly ModbusModule.ModbusService _modbus;


        public ModbusGrpcService(
            ModbusModule.ModbusService modbus)
        {
            _modbus = modbus;
        }


        public override Task<ConnectResponse> Connect(
            ConnectRequest request,
            ServerCallContext context)
        {
            try
            {
                Console.WriteLine(
                    $"Modbus Connect request from: {context.GetHttpContext().User.Identity?.Name}");


                _modbus.Connect();


                return Task.FromResult(
                    new ConnectResponse
                    {
                        Connected = _modbus.IsConnected,
                        Message = "Modbus connected"
                    });
            }
            catch (Exception ex)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.Internal,
                        ex.Message));
            }
        }


        public override Task<DisconnectResponse> Disconnect(
            DisconnectRequest request,
            ServerCallContext context)
        {
            try
            {
                _modbus.Disconnect();


                return Task.FromResult(
                    new DisconnectResponse
                    {
                        Connected = _modbus.IsConnected,
                        Message = "Modbus disconnected"
                    });
            }
            catch (Exception ex)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.Internal,
                        ex.Message));
            }
        }


        public override Task<StatusResponse> GetStatus(
            StatusRequest request,
            ServerCallContext context)
        {
            return Task.FromResult(
                new StatusResponse
                {
                    Connected = _modbus.IsConnected
                });
        }


        public override Task<ReadHoldingRegistersResponse>
            ReadHoldingRegisters(
                ReadHoldingRegistersRequest request,
                ServerCallContext context)
        {
            try
            {
                int[] registers =
                    _modbus.ReadHoldingRegisters(
                        request.StartAddress,
                        request.Quantity);


                var response =
                    new ReadHoldingRegistersResponse();


                response.Registers.AddRange(registers);


                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.Internal,
                        ex.Message));
            }
        }


        public override Task<WriteMultipleRegistersResponse>
            WriteMultipleRegisters(
                WriteMultipleRegistersRequest request,
                ServerCallContext context)
        {
            try
            {
                int[] values =
                    request.Values.ToArray();


                _modbus.WriteMultipleRegisters(
                    request.Address,
                    values);


                return Task.FromResult(
                    new WriteMultipleRegistersResponse
                    {
                        Success = true,
                        Message =
                            "Registers written successfully"
                    });
            }
            catch (Exception ex)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.Internal,
                        ex.Message));
            }
        }
    }
}