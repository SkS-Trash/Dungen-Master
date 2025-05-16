using Enemy.Components;
using UnityEngine;

namespace Enemy.Core
{
    [RequireComponent(typeof(EnemyMovement), typeof(EnemyAnimator), typeof(EnemyHealth))]
    [RequireComponent(typeof(AttackCooldownTracker))]
    [DisallowMultipleComponent]
    public class StateController : MonoBehaviour
    {
        [Tooltip("Start State (SO)")] public State currentState;

        [field: SerializeField] public EnemyStatsSO Stats { get; private set; }
        [field: SerializeField] public AttackPatternSO AttackPattern { get; private set; }

        public EnemyAnimator Animator { get; private set; }
        public EnemyHealth Health { get; private set; }
        public EnemyMovement Movement { get; private set; }
        public Transform Player { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<EnemyMovement>();
            Animator = GetComponent<EnemyAnimator>();

            Health = GetComponent<EnemyHealth>();
            Health.SetMaxHealth(Stats.maxHealth);
        }

        private void Update() => currentState?.UpdateState(this);

        public void TransitionTo(State next)
        {
            if (!next || next == currentState) return;
            currentState?.Exit(this);
            currentState = next;
            currentState.Enter(this);
        }

        public void SetPlayerTransform(Transform playerTransform) =>
            Player = playerTransform;
    }

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