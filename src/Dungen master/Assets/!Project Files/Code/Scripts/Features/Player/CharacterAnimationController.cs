using UnityEngine;

namespace Player
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Sprint = Animator.StringToHash("IsSprint");

        [SerializeField] private ThirdPersonController controller;

        [SerializeField] private Animator animator;

        public void LateUpdate()
        {
            animator.SetFloat(Speed, controller.MoveSpeed.magnitude);
            animator.SetBool(Sprint, controller.IsSprinting);
        }
    }
}