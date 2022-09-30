using System.Net.Sockets;
using System.Text;
using Contracts;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.ServerInterface;
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

        private static ServerEndpointContract serverEndpointContract = new();
        private static ClientEndpointContract _clientEndpointContract = new();

        private static void _AssignTargets()
        {
            _clientEndpointContract.HandShake.Action += OnHandshake;
            ReceivingContractRegister.RegisterContracts(ref eventMapper, _clientEndpointContract, RefreshContractFunc, log);
        }


        private static void OnHandshake(HandShakeMessage obj)
        {
            log.WriteLog(message: $"Handshake received from {obj.Id} send at {obj.TimeStamp}");
        }
        
        public static Func<bool> RefreshContractFunc = RefreshContract;

        private static bool RefreshContract()
        {
            SendingContractRegister.RegisterSendingContract(serverEndpointContract, new object[] { writer }, log);
            return true;    
        }

        private static void Connect()
        {
            eventMapper = new EventMapper(log);
            _AssignTargets();
            var client = new TcpClient();
            client.Connect("127.0.0.1", 5000);
            var stream = client.GetStream();
            reader = new GenericReader(stream);
            reader.OnMessageReceived += eventMapper.HandleEvent;
            writer = new GenericWriter(stream);

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
                        _clientEndpointContract.HandShake.Send(
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