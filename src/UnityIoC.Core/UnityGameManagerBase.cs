using Autofac;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityIoC.Core.Abstractions.Game;
using UnityIoC.Core.Abstractions.Game.Modules;
using UnityIoC.Core.IO.Network.WebSocket;

namespace UnityIoC.Core
{
    public class UnityGameManagerBase : MonoBehaviour
    {
        protected const string Manager = "Manager";
        protected Process ConsoleProcessHandle = default;
        protected GameObject ManagerObject = default;
        protected UnityWebSocketServer WebSocketServer = default;

        // ReSharper disable once InconsistentNaming
        private static readonly string WS_CONSOLE_EXECUTABLE_PATH
            = Path.Combine(new[] { Directory.GetCurrentDirectory(), "Assets", "Logic", "wsconsole.exe" });

        private readonly Process _cmdProcess = default;
        private ContainerBuilder _builder = default;
        private IContainer _container = default;
        private IServer _server = default;

        public UnityGameManagerBase()
        {
            WebSocketServer = new UnityWebSocketServer(this);
            ManagerObject ??= GameObject.Find(nameof(Manager));
        }

        ~UnityGameManagerBase()
        {

        }

        public bool IsRunning => ManagerObject != default;
        protected IContainer Container
        {
            get => _container = _container ?? Builder.Build();
            set => _container = value;
        }
        protected IServer Server => _server ??= Container.Resolve<IServer>();
        private ContainerBuilder Builder
        {
            get
            {
                if (_builder != null) return _builder;

                _builder = new ContainerBuilder();

                _builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                    .Where(type => type.IsSubclassOf(typeof(GameModule)))
                    .As<GameModule>()
                    .SingleInstance();

                _builder.RegisterType<Server>().As<IServer>().SingleInstance()
                    .OnActivated(args => args.Instance.SetInstance(args.Instance));

                ThreadPool.QueueUserWorkItem((_) => WebSocketServer.Initialize());

                ThreadPool.QueueUserWorkItem((@object) =>
                {

                    try
                    {
                        TaskCompletionSource<int> source = new();

                        ConsoleProcessHandle = new Process()
                        {
                            StartInfo =
                            {
                                FileName = WS_CONSOLE_EXECUTABLE_PATH,
                            },
                            EnableRaisingEvents = true,
                        };

                        ConsoleProcessHandle.Exited += (sender, args) =>
                        {
                            source.SetResult(ConsoleProcessHandle.ExitCode);
                            ConsoleProcessHandle.Dispose();
                        };


                        ConsoleProcessHandle.Start();
                    }
                    catch (Exception exception)
                    {
                        File.AppendAllText("ws_console.log", $"[{DateTime.Now}] \t {exception.Message}");
                    }
                });

                return _builder;
            }
        }

        protected IEnumerator Start()
        {
            yield return new WaitForSeconds(2.5f);
        }

        protected virtual void Update()
        {
            Server.Tick();
        }
    }
}