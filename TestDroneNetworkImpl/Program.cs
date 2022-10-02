using System.Net.Sockets;
using System.Text;
using Contracts;
using Contracts.ContractDTOs;
using DroneManager.Interface.GenericTypes;
using GenericEventMapper;
using GenericMessaging;

namespace TestDroneNetworkImpl // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        private static ConsoleLog.ConsoleLog log = new ConsoleLog.ConsoleLog();

        private static void Main(string[] args)
        {
            log.StartLogWriter();
            log.WriteLog(message: "Starting Drone Network Test");

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
            ReceivingContractRegister.RegisterContracts(eventMapper, clientEndpointContract, log);
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
            client.Connect("127.0.0.1", 5000);
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
                    {
                        serverEndpointContract.InitialConnectionHandShake.Send(
                            new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)));
                    }
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