using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyCore : MonoBehaviour
    {
        private EnemyHealth _enemyHealth;
        
        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
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