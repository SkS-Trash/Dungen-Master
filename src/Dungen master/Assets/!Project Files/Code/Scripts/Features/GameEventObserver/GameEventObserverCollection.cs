using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEventObserver
{
    [CreateAssetMenu(fileName = nameof(GameEventObserverCollection),
        menuName = "GameEventObserver/GameEventObserverCollection")]
    public class GameEventObserverCollection : ScriptableObject
    {
        [field: ListDrawerSettings(IsReadOnly = true)]
        [field: SerializeField]
        public List<ObserverItem> Observers { get; private set; }

        private void OnValidate()
        {
            AllEventsValidation();
            RemoveNoneValidation();
        }

        private void AllEventsValidation()
        {
            var events = Enum.GetValues(typeof(GameEventType)).Cast<GameEventType>().ToArray();
            foreach (var eventType in events)
            {
                if (eventType == GameEventType.None) continue;

                if (Observers.All(x => x.EventType != eventType))
                {
                    Observers.Add(new ObserverItem { EventType = eventType });
                }
            }
        }

        private void RemoveNoneValidation()
        {
            Observers = Observers.Where(x => x.EventType != GameEventType.None).ToList();
        }

        [Serializable]
        public class ObserverItem
        {
            [ReadOnly] public GameEventType EventType;
            public GameEventObserverBehaviour Observer;
        }
    }
}