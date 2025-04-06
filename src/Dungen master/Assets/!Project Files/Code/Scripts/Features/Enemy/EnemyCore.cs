using StateMachines.TransitionMultiLayer;
using StateMachines.TransitionMultiLayer.ForState;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyMovement))]
    public class EnemyCore : MonoBehaviour
    {
        private EnemyHealth _enemyHealth;
        private EnemyMovement _enemyMovement;
        private IStateMachine _stateMachine;

        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _enemyMovement = GetComponent<EnemyMovement>();
        }

        private void Start()
        {
            _stateMachine = new StateMachine();

            // Create states
            var idleState = new IdleState(this);

            // Add transitions

            // Set initial state
            _stateMachine.SetInitialState(idleState);
        }

        private void OnEnable()
        {
            _enemyHealth.OnHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            _enemyHealth.OnHealthChanged -= HandleHealthChanged;
        }

        private void HandleHealthChanged(int currentHealth)
        {
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public class IdleState : IState
    {
        private readonly EnemyCore _enemyCore;

        public IdleState(EnemyCore enemyCore)
        {
            _enemyCore = enemyCore;
        }

        public void OnEnter()
        {
        }

        public void OnExecute()
        {
        }

        public void OnExit()
        {
        }
    }
}