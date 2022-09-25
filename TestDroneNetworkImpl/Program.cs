using System.Net.Sockets;
using System.Text;
using ConsoleLogging;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.ServerInterface;
using GenericEventMapper;
using GenericMessaging;

namespace TestDroneNetworkImpl // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        private static ConsoleLog log = new ConsoleLogging.ConsoleLog();

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


        private static void _AssignTargets()
        {
        }

        private static void Connect()
        {
            var client = new TcpClient();
            client.Connect("172.0.0.1", 5000);
            var stream = client.GetStream();
            reader = new GenericReader(stream);
            writer = new GenericWriter(stream);
            eventMapper = new EventMapper(log);
            _AssignTargets();
            reader.OnMessageReceived += eventMapper.HandleEvent;


            stream.Close();
            client.Close();
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
                        writer.SendData(new SendableTarget(new HandShakeMessage(new DroneId(DroneType.Experimental, 1)),
                            "TestHandshake"));
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