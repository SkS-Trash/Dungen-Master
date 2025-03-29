using Observers.Input;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int IsAttackingMelee = Animator.StringToHash("IsAttackingMelee");
        private static readonly int IsAttackingMagic = Animator.StringToHash("IsAttackingMagic");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        [SerializeField] private InputActionReader inputActionReader;

        private Animator _animator;
        private CharacterController _characterController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _animator.SetFloat(Speed, inputActionReader.MoveValue.magnitude);

            _animator.SetBool(IsJumping, inputActionReader.IsJumping && _characterController.isGrounded);

            if (inputActionReader.IsSprinting)
            {
                _animator.SetFloat(Speed, inputActionReader.MoveValue.magnitude * 2);
            }

            _animator.SetBool(IsAttackingMelee, Input.GetKeyDown(KeyCode.Mouse0));

            _animator.SetBool(IsAttackingMagic, Input.GetKeyDown(KeyCode.Mouse1));
        }
    }
}