using System;
using System.Collections;
using StateMachines.TransitionMultiLayer;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyMovement))]
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] private float aggroRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;

        private IStateMachine _stateMachine;

        private EnemyHealth _health;
        private EnemyMovement _movement;
        // private EnemyAnimator _animator;

        private Transform _playerTransform;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _health = GetComponent<EnemyHealth>();
        }

        private void Start()
        {
            _stateMachine = new StateMachine();

            // Create states
            var idleState = new IdleState(this, _movement);
            var patrolState = new PatrolState(this, _movement);
            var followState = new FollowState(this, _movement, _playerTransform);
            var attackState = new AttackState(this, _movement, 1f);
            var damageState = new DamageState(this, _movement, _health);
            var deathState = new DeathState(this, _movement);

            // Create
            Func<bool> isPlayerInRange = () => DistanceToPlayer() < aggroRange;
            Func<bool> isPlayerInAttackRange = () => DistanceToPlayer() < attackRange;
            Func<bool> isPlayerOutOfRange = () => _playerTransform == null || DistanceToPlayer() > aggroRange;
            Func<bool> isPlayerInDamageState = () => damageState.IsGetHitEnd;
            Func<bool> isInDeathState = () => _health.CurrentHealth <= 0;

            // Set up transitions
            _stateMachine.AddTransition(idleState, followState, false, isPlayerInRange);
            _stateMachine.AddTransition(idleState, patrolState, false, isPlayerOutOfRange);

            _stateMachine.AddTransition(followState, idleState, false, isPlayerOutOfRange);
            _stateMachine.AddTransition(followState, attackState, false, isPlayerInAttackRange);

            _stateMachine.AddTransition(attackState, followState, false, isPlayerOutOfRange);
            _stateMachine.AddTransition(attackState, damageState, false, isPlayerInDamageState);

            _stateMachine.AddTransition(damageState, followState, false, isPlayerInDamageState);

            _stateMachine.AddGlobalTransition(deathState, isInDeathState);

            // Set initial state
            _stateMachine.SetInitialState(idleState);
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
        
        public void SetPlayerTransform(Transform playerTransform) => _playerTransform = playerTransform;

        private void HandleHealthChanged(int currentHealth)
        {
            // TODO: Update health UI

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

        public void StartDeathCoroutine()
        {
            StartCoroutine(DestroyAfterDelay(2f));
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}