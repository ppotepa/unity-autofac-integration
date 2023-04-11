using System;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace UnityIoC.IO.WSConsole
{
    public static class Program
    {
        const string WsServerUrl = "ws://127.0.0.1:2137/console";
        public static async Task Main(params string[] args)
        {
            WebSocket webSocket = new WebSocket("ws://127.0.0.1:2137/console");

            ThreadPool.QueueUserWorkItem((@object) =>
            {
                webSocket.OnOpen += (sender, errorArgs) => Console.WriteLine($"[{DateTime.Now}] Successfully connected to {WsServerUrl}");
                webSocket.OnError += (sender, errorArgs) => Console.WriteLine(errorArgs.Message);
                webSocket.OnMessage += (sender, e) =>
                {
                    Console.WriteLine(e.Data);
                };

            });

            int attempts = 1;
            while (webSocket.IsAlive == false)
            {
                Console.WriteLine($"[{DateTime.Now}] Attempting to connect to {WsServerUrl} - attempt {attempts++}");
                webSocket.Connect();
            }


            while (webSocket.Ping("ping"))
            {
                await Task.Delay(1);
            }

            Console.WriteLine($"[{DateTime.Now}] Connection closed.");
        }
    }
}

