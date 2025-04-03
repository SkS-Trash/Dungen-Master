using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Sprint = Animator.StringToHash("IsSprint");
        private static readonly int AttackingMelee = Animator.StringToHash("AttackingMelee");
        private static readonly int AttackingMagic = Animator.StringToHash("AttackingMagic");

        [SerializeField] private Animator animator;

        public void SetSpeed(float speed) => animator.SetFloat(Speed, speed);

        public void StartSprint() => animator.SetBool(Sprint, true);
        public void EndSprint() => animator.SetBool(Sprint, false);

        public void MagicAttack() => animator.SetTrigger(AttackingMagic);
        public void MeleeAttack() => animator.SetTrigger(AttackingMelee);
    }
}