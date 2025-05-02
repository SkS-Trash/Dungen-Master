using System;
using System.Linq;
using GameEventObserver;
using Providers.Data;
using VContainer;
using Object = UnityEngine.Object;

namespace Factories.GameEvent
{
    public class GameEventFactory : IGameEventFactory
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IObjectResolver _objectResolver;

        public GameEventFactory(
            IStaticDataProvider staticDataProvider,
            IObjectResolver objectResolver
        )
        {
            _staticDataProvider = staticDataProvider;
            _objectResolver = objectResolver;
        }

        public GameEventObserverBehaviour CreateGameEvent(GameEventType eventType, object data = null)
        {
            var prefab = _staticDataProvider
                .GetGameEventObserverCollection()
                .Observers
                .FirstOrDefault(x => x.EventType == eventType)?
                .Observer;

            if (prefab == null)
            {
                throw new ArgumentException($"No observer found for event type: {eventType}");
            }

            var prefabActive = prefab.gameObject.activeSelf;

            prefab.gameObject.SetActive(false);

            var instantiate = InstantiateAndInject(prefab);

            prefab.gameObject.SetActive(prefabActive);

            return instantiate;
        }

        private T InstantiateAndInject<T>(T prefab) where T : GameEventObserverBehaviour
        {
            var instance = Object.Instantiate(prefab);
            _objectResolver.Inject(instance);
            instance.gameObject.SetActive(true);
            return instance;
        }
    }
}