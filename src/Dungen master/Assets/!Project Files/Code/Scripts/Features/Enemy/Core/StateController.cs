using Enemy.Components;
using Enemy.Components.Events;
using R3;
using UnityEngine;

namespace Enemy.Core
{
    [RequireComponent(typeof(EnemyMovement), typeof(EnemyAnimator), typeof(EnemyHealth))]
    [RequireComponent(typeof(AttackCooldownTracker))]
    [DisallowMultipleComponent]
    public class StateController : MonoBehaviour
    {
        [Tooltip("Start State (SO)")] public State currentState;

        [field: SerializeField] public ReactiveProperty<EnemyStatsSO> Stats { get; private set; }
        [field: SerializeField] public AttackPatternSO AttackPattern { get; private set; }

        public EnemyAnimator Animator { get; private set; }
        public EnemyAnimationEvents AnimationEvents { get; private set; }
        public EnemyHealth Health { get; private set; }
        public EnemyMovement Movement { get; private set; }
        public AttackCooldownTracker AttackCooldownTracker { get; private set; }
        public Transform Player { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<EnemyAnimator>();
            AnimationEvents = GetComponentInChildren<EnemyAnimationEvents>();

            Movement = GetComponent<EnemyMovement>();

            AttackCooldownTracker = GetComponent<AttackCooldownTracker>();

            Health = GetComponent<EnemyHealth>();
            Health.SetMaxHealth(Stats.CurrentValue.maxHealth);
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
}