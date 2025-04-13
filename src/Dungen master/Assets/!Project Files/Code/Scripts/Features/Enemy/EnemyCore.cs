using System;
using System.Collections;
using Sirenix.OdinInspector;
using StateMachines.TransitionMultiLayer;
using UI.Game;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyMovement))]
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] private float multiplierOutOfRanges = 1.25f;
        [SerializeField] private float aggroRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;
        [Space]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private EnemyCurrentStateDraw stateDraw;
        
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyHealth _health;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyMovement _movement;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private EnemyAnimator _animator;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private Transform _playerTransform;

        private IStateMachine _stateMachine;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _health = GetComponent<EnemyHealth>();
        }

        public void Initialize()
        {
            _stateMachine = new StateMachine();
            _stateMachine.OnStateChanged += state => stateDraw.SetCurrentState(state.ToString());

            // Create states
            var idleState = new IdleState(this, _movement);
            var patrolState = new PatrolState(this, _movement);
            var followState = new FollowState(this, _movement, _playerTransform);
            var attackState = new AttackState(this, _movement, 1f);
            var damageState = new DamageState(this, _movement, _health);
            var deathState = new DeathState(this, _movement);

            // Set up transitions
            _stateMachine.AddTransition(idleState, followState, false, IsPlayerInRange);
            _stateMachine.AddTransition(idleState, patrolState, false, IsPlayerOutOfRange);

            _stateMachine.AddTransition(followState, idleState, false, IsPlayerOutOfRange);
            _stateMachine.AddTransition(followState, attackState, false, IsPlayerInAttackRange);

            _stateMachine.AddTransition(patrolState, followState, false, IsPlayerInRange);

            _stateMachine.AddTransition(attackState, followState, false, IsPlayerOutOfAttackRange);

            _stateMachine.AddTransition(damageState, idleState, false, IsDamageEnd);

            _stateMachine.AddGlobalTransition(damageState, IsDamage);
            _stateMachine.AddGlobalTransition(deathState, IsInDeathState);

            // Set initial state
            _stateMachine.SetInitialState(idleState);

            return;

            bool IsPlayerInRange() =>
                DistanceToPlayer() < aggroRange;

            bool IsPlayerOutOfRange() =>
                _playerTransform == null || DistanceToPlayer() > aggroRange * multiplierOutOfRanges;

            bool IsPlayerInAttackRange() =>
                DistanceToPlayer() < attackRange;

            bool IsPlayerOutOfAttackRange() =>
                _playerTransform == null || DistanceToPlayer() > attackRange * multiplierOutOfRanges;

            bool IsDamage() =>
                _health.WasDamaged;

            bool IsDamageEnd() =>
                damageState.IsGetHitEnd;

            bool IsInDeathState() =>
                _health.CurrentHealth <= 0;
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