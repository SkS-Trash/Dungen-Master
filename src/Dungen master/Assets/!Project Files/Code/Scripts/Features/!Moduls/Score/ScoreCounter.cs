using UnityEngine;

namespace Score
{
    public class ScoreCounter : MonoBehaviour,
        IBossDeathEvent,
        IEnemyDeathEvent
    {
        private int _currentScoreCount = 0;

        private void Start() => Notify();
        private void OnEnable() => EventBus.Subscribe(this);
        private void OnDisable() => EventBus.Unsubscribe(this);

        public void OnBossDeath()
        {
            _currentScoreCount += Random.Range(5, 10);

            Notify();
        }

        public void OnEnemyDeath()
        {
            _currentScoreCount += Random.Range(1, 3);

            Notify();
        }

        private void Notify()
        {
            EventBus.RaiseEvent<IScoreChangedEvent>(e => e.OnScoreChanged(_currentScoreCount));
        }
    }
}