using UnityEngine;

namespace GameEventObserver
{
    public abstract class GameEventObserverBehaviour : MonoBehaviour, IGameEventObserver
    {
        public abstract GameEventType EventType { get; }

        private event GameEvent OnEvent;

        protected virtual void OnEnable() => Subscribe();
        protected virtual void OnDisable() => Unsubscribe();

        public void Register(GameEvent @event)
        {
            if (@event == null) return;
            OnEvent += @event;
        }

        public void Unregister(GameEvent @event)
        {
            if (@event == null) return;
            OnEvent -= @event;
        }

        public void UnregisterAll()
        {
            OnEvent = null;
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        protected virtual void Notify()
        {
            OnEvent?.Invoke(this);
        }
    }
}