using UnityEngine;

namespace Player.Components
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Sprint = Animator.StringToHash("IsSprint");
        private static readonly int AttackingMelee = Animator.StringToHash("AttackingMelee");
        private static readonly int AttackingMagic = Animator.StringToHash("AttackingMagic");

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

        public void SetSpeed(float speed)
        {
            if (animator)
                animator.SetFloat(Speed, speed);
        }

        public void StartSprint()
        {
            if (animator)
                animator.SetBool(Sprint, true);
        }

        public void EndSprint()
        {
            if (animator)
                animator.SetBool(Sprint, false);
        }

        public void MagicAttack()
        {
            if (animator)
                animator.SetTrigger(AttackingMagic);
        }

        public void MeleeAttack()
        {
            if (animator)
                animator.SetTrigger(AttackingMelee);
        }
    }
}