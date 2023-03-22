namespace UnityIoC.Core.Abstractions.Game
{

    public class Game : IGame
    {
        public static Game Instance = default;
        public void SetInstance(Game instance) => Instance = instance;
    }
}