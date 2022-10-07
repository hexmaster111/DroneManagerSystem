using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using GenericEventMapper;
using GenericMessaging;
using IConsoleLog;
using RegisterSimulator;

namespace TestDroneNetworkImpl // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        private static ConsoleLog.ConsoleLog log = new ConsoleLog.ConsoleLog();

        private static IPAddress ServerIp;
        private static DroneId DroneId;
        private static RegSimulator _regSimulator;

        private static void Main(string[] args)
        {
            log.StartLogWriter();
            log.WriteLog(message: "Starting Drone Network Test");

            if (args.Length != 2)
            {
                log.WriteLog(message: "Invalid number of arguments. Expected 2, got " + args.Length);
                log.WriteLog(message: "Usage: TestDroneNetworkImpl.exe <server ip> <drone id>");
                log.WriteLog(message: "Starting with defualt values");
                ServerIp = IPAddress.Parse("127.0.0.1");
                DroneId = new DroneId(DroneType.Experimental, 5050);
            }
            else
            {
                ServerIp = IPAddress.Parse(args[1]);

                int id = int.Parse(args[0]);
                DroneId = new DroneId(DroneType.Experimental, id);
            }


            Connect();
            ConsoleLoop();
        }


        private static GenericReader reader;
        private static GenericWriter writer;

        private static EventMapper eventMapper;

        private static ServerEndpointContract serverEndpointContract;
        private static ClientEndpointContract clientEndpointContract;

        private static void _AssignTargets()
        {
            clientEndpointContract.HandShake.Action += OnHandshake;
            clientEndpointContract.BroadcastChatMessage.Action += OnBroadcastChatMessage;
            clientEndpointContract.HardwareUpdateRequest.Action += OnHardwareUpdateRequest;
            clientEndpointContract.SetRegister.Action += OnSetRegister;
            ReceivingContractRegister.RegisterContracts(eventMapper, clientEndpointContract, log);
        }

        private static void OnSetRegister(SetRegisterMessage obj)
        {
            log.WriteLog(message: "Received SetRegisterMessage");
            log.WriteLog(message: "Register: " + obj.RegisterName);
            log.WriteLog(message: "Value: " + obj.Value);
            try
            {
                _regSimulator.SetRegisterValue(obj.RegisterName, obj.Value);
            }
            catch (Exception e)
            {
                log.WriteLog(message: "Error: " + e.Message, LogLevel.Error);
            }
        }

        private static HardwareInfoUpdateMessage _buildHardwareInfoUpdateMessage()
        {
            var data = new List<HardwareInfoUpdateMessage.RemoteRegisterData>();

            foreach (var register in _regSimulator.Registers)
            {
                data.Add(new HardwareInfoUpdateMessage.RemoteRegisterData(register.Name, register.DataType,
                    register.Value));
            }

            return new HardwareInfoUpdateMessage(data.ToArray());
        }

        private static void OnHardwareUpdateRequest(BlankRequest a)
        {
            log.WriteLog(message: "Received HardwareUpdateRequest");
            serverEndpointContract.HardwareInfoUpdate.Send(_buildHardwareInfoUpdateMessage());
        }

        private static void OnBroadcastChatMessage(ChatMessage obj)
        {
            log.WriteLog(message: $"[{obj.Sender}] \"{obj.Message}\"");
        }


        private static void OnHandshake(HandShakeMessage obj)
        {
            log.WriteLog(message: $"Handshake received from {obj.Id} send at {obj.TimeStamp}");
        }

        private static bool RefreshContract()
        {
            SendingContractRegister.RegisterSendingContract(serverEndpointContract, new object[] { writer }, log);
            return true;
        }

        private static void Connect()
        {
            var client = new TcpClient();
            client.Connect(ServerIp, 5000);
            var stream = client.GetStream();
            reader = new GenericReader(stream);
            writer = new GenericWriter(stream);
            eventMapper = new EventMapper(log);

            serverEndpointContract = new ServerEndpointContractImpl(eventMapper);
            clientEndpointContract = new ClientEndpointContractImpl();

            _regSimulator = new RegSimulator(16);

            reader.OnMessageReceived += eventMapper.HandleEvent;
            _AssignTargets();
            RefreshContract();


            reader.StartReading();
            Thread.Sleep(Random.Shared.Next(0, 1000));
            serverEndpointContract.InitialConnectionHandShake.Send(
                new HandShakeMessage(DroneId));
        }


        static bool running = true;

        private static void ConsoleLoop()
        {
            while (running)
            {
                var command = Console.ReadLine();
                if (command == null) continue;
                //basic cli parcing
                var commandParts = command.Split(' ');
                switch (commandParts[0])
                {
                    case "exit":
                        running = false;
                        break;

                    case "hb":


                        var loc = new Location()
                        {
                            Latitude = 69,
                            Longitude = 420,
                            Speed = 42,
                            LocationAddress = "Hello address",
                            LocationName = "Hello name",
                            LocationProvider = "Epic provider",
                            TimeStamp = DateTime.Now
                        };

                        var hb = new HeartBeatSuperMessage(new LocationMessage(loc), _buildHardwareInfoUpdateMessage(),
                            new VitalsUpdateMessage(69, 420, 42));

                        serverEndpointContract.HeartBeat.Send(hb);
                        break;

                    case "test":
                        serverEndpointContract.InitialConnectionHandShake.Send(
                            new HandShakeMessage(DroneId));
                        break;

                    case "test2":
                        serverEndpointContract.VitalsUpdate.Send(new VitalsUpdateMessage(69, 420, 42));
                        break;

                    case "test3":

                        var location = new Location()
                        {
                            Latitude = 69,
                            Longitude = 420,
                            Speed = 42,
                            LocationAddress = "Hello address",
                            LocationName = "Hello name",
                            LocationProvider = "Epic provider",
                            TimeStamp = DateTime.Now
                        };

                        serverEndpointContract.LocationUpdate.Send(new LocationMessage(location));
                        break;

                    case "help":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("exit - exit the program");
                        Console.WriteLine("help - show this message");
                        break;
                    default:
                        Console.WriteLine("Unknown command. Type 'help' for list of available commands.");
                        break;
                }
            }
        }
    }
}