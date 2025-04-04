using StateMachines.TransitionMultiLayer;
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
}