using UnityEngine;

namespace Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Death = Animator.StringToHash("Death");

        [SerializeField] private Animator animator;

        public void SetIsWalk(bool value) => animator.SetBool(Walk, value);
        public void SetIsRun(bool value) => animator.SetBool(Run, value);
        public void LaunchHit() => animator.SetTrigger(Hit);
        public void LaunchAttack() => animator.SetTrigger(Attack);
        public void LaunchDeath() => animator.SetTrigger(Death);
    }
}