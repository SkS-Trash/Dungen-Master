using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ThirdPersonController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private float turnSmoothVelocity = 2f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [Space] [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [Space] [SerializeField] private PlayerAnimator animator;
        [Space] [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;

        private Vector2 _moveInput;
        private Vector3 _moveVelocity;
        private float _currentSpeed;

        public void Start()
        {
            _currentSpeed = moveSpeed;
        }

        public void Update()
        {
            _moveInput = moveAction.action.ReadValue<Vector2>();

            if (_moveInput.magnitude >= 0.1f)
            {
                Sprint();
                Rotate();
                Move();
            }
            else
            {
                animator.SetSpeed(0);
            }
        }

        private void Rotate()
        {
            var target = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * angle;
        }

        private void Move()
        {
            _moveVelocity = new Vector3(_moveInput.x, 0, _moveInput.y);
            _moveVelocity = Vector3.ClampMagnitude(_moveVelocity, 1f);
            _moveVelocity *= _currentSpeed * Time.deltaTime;
            _moveVelocity = characterController.transform.TransformDirection(_moveVelocity);
            characterController.Move(_moveVelocity);

            animator.SetSpeed(_moveInput.magnitude);
        }

        private void Sprint()
        {
            _currentSpeed = sprintAction.action.inProgress ? runSpeed : moveSpeed;

            if (sprintAction.action.inProgress)
            {
                animator.StartSprint();
            }
            else
            {
                animator.EndSprint();
            }
        }
    }
}