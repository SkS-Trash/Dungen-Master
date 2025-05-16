using UnityEngine;

namespace Enemy.Core
{
    [CreateAssetMenu(menuName = "AI/Enemy Stats")]
    public class EnemyStatsSO : ScriptableObject
    {
        public int maxHealth = 100;
        public float moveSpeed = 3;
        public float aggroRadius = 10;
        public float attackRadius = 2;
    }
}