using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ThirdPersonController : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }
        public Vector3 MoveSpeed { get; private set; }

        [SerializeField] private Transform camera;
        [SerializeField] private float turnSmoothVelocity = 2f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;

        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference jumpAction;

        private Vector2 _moveInput;
        private float _currentSpeed;

        public void Start()
        {
            _currentSpeed = moveSpeed;
        }

        public void Update()
        {
            _moveInput = moveAction.action.ReadValue<Vector2>();

            if (!(_moveInput.magnitude >= 0.1f))
            {
                MoveSpeed = Vector3.zero;
                return;
            }

            Sprint();

            Rotate();
            Move();
        }

        private void Rotate()
        {
            var target = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * angle;
        }

        private void Move()
        {
            MoveSpeed = new Vector3(_moveInput.x, 0, _moveInput.y);
            MoveSpeed = Vector3.ClampMagnitude(MoveSpeed, 1f);
            MoveSpeed *= _currentSpeed * Time.deltaTime;
            MoveSpeed = characterController.transform.TransformDirection(MoveSpeed);
            characterController.Move(MoveSpeed);
        }

        private void Sprint()
        {
            IsSprinting = sprintAction.action.inProgress;

            _currentSpeed = IsSprinting ? runSpeed : moveSpeed;
        }
    }
}