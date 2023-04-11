using System.Threading.Tasks;
using UnityIoC.Core.Tools;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace UnityIoC.Core.IO.Network.WebSocket
{
    public class BasicWebSocketBehavior : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs eventArgs)
        {
            string message = eventArgs.Data == "BALUS"
                ? "I've been balused already..."
                : "I'm not available now.";

            Send(message);
        }
    }

    public class UnityWebSocketServer : Disposable
    {
        private readonly UnityGameManagerBase unityGameManagerBase;
        private WebSocketServer server = default;

        public UnityWebSocketServer(UnityGameManagerBase unityGameManagerBase)
        {
            
        }

        private bool IsRunning => server.IsListening;
        public async void Initialize()
        {
            server = new WebSocketServer("ws://127.0.0.1:2137");
            server.AddWebSocketService<BasicWebSocketBehavior>("/console");
            server.Start();

            while (unityGameManagerBase.IsRunning && server.IsListening)
            {
                await Task.Delay(1);
            }

            server.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && server is not null)
            {
                server.Stop();
            }
            
            base.Dispose(disposing);
        }
    }
}
