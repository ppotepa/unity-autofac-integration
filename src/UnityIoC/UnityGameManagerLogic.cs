using System.Diagnostics;
using System.IO;
using UnityIoC.Core;

namespace UnityIoC
{
    public class UnityGameManagerLogic : UnityGameManagerBase
    {
        protected override void Update()
        {
            var path = Path.Combine(new[] { CurrentDirectory, "UnityIoC.Core.dll" });
            var path2 = Path.Combine(new[] { CurrentDirectory, "test.txt" });

            File.WriteAllText($"{CurrentDirectory}\\test.txt", path2);

            base.Update();
        }

        public Process ConsoleProcess { get; set; }
        private static string CurrentDirectory => Directory.GetCurrentDirectory();
    }

    
}