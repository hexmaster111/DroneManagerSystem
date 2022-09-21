using System.Net;
using System.Text;
using DroneManager.Interface.GenericTypes;
using DroneManager.Interface.GenericTypes.BaseTypes;
using DroneManager.Interface.RemoteConnection;
using DroneManager.Interface.ServerInterface;
using IConsoleLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestDroneNetworkImpl // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        private static ConsoleLog.ConsoleLog log = new ConsoleLog.ConsoleLog();

        static IRemoteStreamConnection connection = new RemoteStream();

        private static void Main(string[] args)
        {
            log.StartLogWriter();
            log.WriteLog(message: "Starting Drone Network Test");

            connection.Connect(null);
            connection.SendData(new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)));
            ConsoleLoop();
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


        public class RemoteStream : IRemoteStreamConnection
        {
            private Stream? _stream;


            public void Disconnect(ISendable? disconnectionArgs)
            {
                throw new NotImplementedException();
            }

            public void Connect(object? connectionArgs)
            {
                try
                {
                    var client = new System.Net.Sockets.TcpClient("127.0.0.1", 8000); // Create a new connection  
                    _stream = client.GetStream();
                    log.WriteLog(message: $"Connected to server at {client.Client.RemoteEndPoint}");

                    var clientThread = new Thread(ClientThread);
                    clientThread.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public void SendData(ISendable data)
            {
                var target = new SendableTarget("debug", data.ToJson());

                var send = target.ToJson();

                byte[] buffer = Encoding.ASCII.GetBytes(send.ToString());


                // // Convert the buffer back
                // var received = Encoding.ASCII.GetString(buffer);
                // log.WriteLog(message: "String Back: " + received, logLevel: LogLevel.Debug);
                // var a = JObject.Parse(received);
                // var b = a.ToObject<SendableTarget>();


                _stream.Write(buffer, 0, buffer.Length);
            }

            private void ClientThread()
            {
                while (running)
                {
                    var serializer = new JsonSerializer();
                    var reader = new StreamReader(_stream ?? throw new InvalidOperationException());
                    var jsonDeserializer = new JsonTextReader(reader);
                    var data = serializer.Deserialize<SendableTarget>(jsonDeserializer);

                    Thread.Sleep(1000);
                }
            }


            private bool running;

            public event Action<ISendable> DataReceived;
            public event Action<object>? DataSent;
            public event Action<ISendable> ConnectionStatusChanged;
            public ConnectionType ConnectionType { get; }
            public ConnectionStatus Status { get; }
        }
    }
}