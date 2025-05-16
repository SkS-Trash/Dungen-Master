using UnityEngine;

namespace Enemy.Core
{
    public abstract class EnemyAttackSO : ScriptableObject
    {
        public float cooldown;
        public float minDistance;
        public float maxDistance;

        public abstract void Execute(StateController controller);
    }
}