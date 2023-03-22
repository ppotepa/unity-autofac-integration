using Autofac;
using System;
using System.Collections;
using UnityEngine;
using UnityIoC.Core.Abstractions.Game;
using UnityIoC.Core.Abstractions.Game.Modules;

namespace UnityIoC.Core
{
    public class UnityGameManagerBase : MonoBehaviour
    {
        protected const string Manager = "Manager";
        protected IContainer _container = default;
        protected IGame _game = default;
        
        private GameObject _gameManagerObject = default;
        private ContainerBuilder _builder = default;

        protected IContainer Container
        {
            get
            {
                if (_container is null)
                {
                    _container = Builder.Build();
                }
                return _container;
            }
            set => _container = value;
        }

        protected IGame Game
        {
            get
            {
                if (_game is null)
                {
                    _game = Container.Resolve<IGame>();
                }
                return _game;
            }
        }

        protected GameObject ManagerObject
        {
            get
            {
                _gameManagerObject ??= GameObject.Find(nameof(Manager));
                return _gameManagerObject;
            }
        }

        private ContainerBuilder Builder
        {
            get
            {
                if (_builder is null)
                {
                    _builder = new ContainerBuilder();
                    //_builder.RegisterInstance(GameConfiguration).AsImplementedInterfaces().SingleInstance();

                    _builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                         .Where(type => type.IsSubclassOf(typeof(GameModule)))
                         .As<GameModule>()
                         .SingleInstance();

                    _builder.RegisterType<Game>().As<IGame>().SingleInstance().OnActivated(args => args.Instance.SetInstance(args.Instance));
                }

                return _builder;
            }
            set => _builder = value;
        }

        protected IEnumerator Start()
        {
            yield return new WaitForSeconds(2.5f);
        }

        protected virtual void Update()
        {
            ManagerObject.gameObject.transform.Rotate(0.1f, 0.1f, 0.1f);
        }
    }
}