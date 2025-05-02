using UnityEngine;

namespace GameEventObserver
{
    public abstract class GameEventObserverBehaviour : MonoBehaviour, IGameEventObserver
    {
        protected virtual void OnEnable() => Subscribe();
        protected virtual void OnDisable() => Unsubscribe();

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
    }
}