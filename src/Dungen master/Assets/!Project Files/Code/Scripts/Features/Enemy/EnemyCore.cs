using Sirenix.OdinInspector;
using StateMachines.TransitionMultiLayer;
using UI.Game;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyMovement), typeof(EnemyAnimator))]
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] private float multiplierOutOfRanges = 1.25f;
        [SerializeField] private float aggroRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;
        [Space] [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float chaseSpeed = 4f;
        [Space] [SerializeField] private HealthBar healthBar;
        [SerializeField] private EnemyCurrentStateDraw stateDraw;

        [Space] [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyHealth _health;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyMovement _movement;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyAnimator _animator;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyAnimationEvents _animationEvents;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private Transform _playerTransform;

        private IStateMachine _stateMachine;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _health = GetComponent<EnemyHealth>();
            _animator = GetComponent<EnemyAnimator>();
            _animationEvents = GetComponentInChildren<EnemyAnimationEvents>();
        }

        public void Initialize()
        {
            _stateMachine = new StateMachine();
            _stateMachine.OnStateChanged += state => stateDraw.SetCurrentState(state.ToString());

            // Create states
            var idleState = new IdleState(this);
            var patrolState = new PatrolState(this, _movement, _animator, patrolSpeed);
            var followState = new FollowState(this, _movement, _animator, chaseSpeed, _playerTransform);
            var attackState = new AttackState(this, _animator, _animationEvents, 1f);
            var damageState = new DamageState(this, _animator, _animationEvents, _health);
            var deathState = new DeathState(this, _animator, _animationEvents);

            // Есть баг с переходами между Idle - Follow - Attack - между ними
            
            // Set up transitions
            _stateMachine.AddTransition(idleState, attackState, false, IsPlayerInAttackRange, IsAttackReady);
            _stateMachine.AddTransition(idleState, followState, false, IsPlayerInRange, IsPlayerOutOfAttackRange);
            _stateMachine.AddTransition(idleState, patrolState, false, IsPlayerOutOfRange);

            _stateMachine.AddTransition(followState, patrolState, false, IsPlayerOutOfRange);
            _stateMachine.AddTransition(followState, attackState, false, IsPlayerInAttackRange, IsAttackReady);

            _stateMachine.AddTransition(patrolState, followState, false, IsPlayerInRange, IsPlayerOutOfAttackRange);

            _stateMachine.AddTransition(attackState, idleState, false, IsPlayerOutOfAttackRange);
            _stateMachine.AddTransition(attackState, idleState, false, IsAttackEnd);

            _stateMachine.AddTransition(damageState, idleState, false, IsDamageEnd);

            _stateMachine.AddGlobalTransition(damageState, IsDamage);
            _stateMachine.AddGlobalTransition(deathState, IsInDeathState);

            // Set initial state
            _stateMachine.SetInitialState(idleState);

            return;

            bool IsPlayerInRange() =>
                DistanceToPlayer() < aggroRange;

            bool IsPlayerOutOfRange() =>
                !_playerTransform || DistanceToPlayer() > aggroRange * multiplierOutOfRanges;

            bool IsPlayerInAttackRange() =>
                DistanceToPlayer() < attackRange;

            bool IsPlayerOutOfAttackRange() =>
                !_playerTransform || DistanceToPlayer() > attackRange * multiplierOutOfRanges;

            bool IsDamage() =>
                _health.WasDamaged;

            bool IsDamageEnd() =>
                damageState.IsGetHitEnd;

            bool IsInDeathState() =>
                _health.CurrentHealth <= 0;

            bool IsAttackReady() =>
                attackState.IsAttackReady;

            bool IsAttackEnd() =>
                attackState.IsAttackEnd;
        }

        private void OnEnable()
        {
            _health.OnHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= HandleHealthChanged;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        public EnemyCore SetPlayerTransform(Transform playerTransform)
        {
            _playerTransform = playerTransform;
            return this;
        }

        private void HandleHealthChanged(int currentHealth)
        {
            healthBar.SetHealthPercentage((float)currentHealth / _health.MaxHealth);

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        private float DistanceToPlayer()
        {
            if (_playerTransform == null)
                return float.PositiveInfinity;

            return Vector3.Distance(transform.position, _playerTransform.position);
        }
    }
}