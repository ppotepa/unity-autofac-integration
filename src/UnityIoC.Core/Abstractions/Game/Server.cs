
using UnityEngine;
using UnityIoC.Core.IO.Network.WebSocket;


namespace UnityIoC.Core.Abstractions.Game
{

    public static class GameObjectExtensions
    {
        public static GameObject GetSun()
        {
            return GameObject.Find("Sun");
        }
    }

    public class Server : IServer
    {
        public static Server Instance = default;
        private const int SIGNALR_PORT = 86489;
       

        public Server()
        {
        
        }

        public void SetInstance(Server instance) => Instance = instance;
        public void Tick()
        {
            GameObject.Find("Sun").transform.Rotate(0.1f, 0.1f, 0.1f);
        }
    }
}