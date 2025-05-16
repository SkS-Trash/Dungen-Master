using Enemy.Core;
using UnityEngine;

namespace Enemy.Attacks
{
    [CreateAssetMenu(menuName = "AI/Attack/Melee")]
    public class MeleeAttackSO : EnemyAttackSO
    {
        public override void Execute(StateController controller)
        {
            controller.Animator.LaunchAttack();
        }
    }
}