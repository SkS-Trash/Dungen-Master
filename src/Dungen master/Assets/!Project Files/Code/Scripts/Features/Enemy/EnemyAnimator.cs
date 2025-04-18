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

        private void Awake()
        {
            animator ??= GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("Компонент аниматора не найден на GameObject.", this);
                enabled = false;
            }
        }

        public void SetIsWalk(bool value)
        {
            if (animator)
                animator.SetBool(Walk, value);
        }

        public void SetIsRun(bool value)
        {
            if (animator)
                animator.SetBool(Run, value);
        }

        public void LaunchHit()
        {
            if (animator)
                animator.SetTrigger(Hit);
        }

        public void LaunchAttack()
        {
            if (animator)
                animator.SetTrigger(Attack);
        }

        public void LaunchDeath()
        {
            if (animator)
                animator.SetTrigger(Death);
        }
    }
}