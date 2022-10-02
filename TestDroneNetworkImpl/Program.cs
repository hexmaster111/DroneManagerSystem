using System.Net;
using System.Net.Sockets;
using System.Text;
using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using GenericEventMapper;
using GenericMessaging;

namespace TestDroneNetworkImpl // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        private static ConsoleLog.ConsoleLog log = new ConsoleLog.ConsoleLog();

        private static IPAddress ServerIp;
        private static DroneId DroneId;

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
            ReceivingContractRegister.RegisterContracts(eventMapper, clientEndpointContract, log);
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