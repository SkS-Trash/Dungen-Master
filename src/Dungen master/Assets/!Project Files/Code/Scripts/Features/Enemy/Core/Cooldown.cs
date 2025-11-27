using UnityEngine;

namespace Enemy.Core
{
    public class Cooldown
    {
        private float _cooldownDuration;
        private float _nextReadyTime;

        public Cooldown(float duration)
        {
            _cooldownDuration = duration;
            _nextReadyTime = -Mathf.Infinity;
        }

        public bool IsReady => Time.time >= _nextReadyTime;

        public void Trigger()
        {
            _nextReadyTime = Time.time + _cooldownDuration;
        }

        public float NormalizedRemaining()
        {
            if (IsReady) return 0f;
            return Mathf.Clamp01((_nextReadyTime - Time.time) / _cooldownDuration);
        }
    }
}