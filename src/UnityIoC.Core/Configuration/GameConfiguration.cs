namespace UnityIoC.Core.Configuration
{
    public class GameConfiguration : IGameConfiguration
    {
        private double sunX;

        public double SunX { get => sunX; set => sunX = value; }
    }
}
