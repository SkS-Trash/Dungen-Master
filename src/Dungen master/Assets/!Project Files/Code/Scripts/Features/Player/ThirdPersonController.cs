using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ThirdPersonController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private float turnSmoothVelocity = 2f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 5f;

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

            Jump();
            Sprint();

            if (_moveInput.magnitude >= 0.1f)
            {
                Rotate();
                Move();
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
            var move = new Vector3(_moveInput.x, 0, _moveInput.y);
            move = Vector3.ClampMagnitude(move, 1f);
            move *= _currentSpeed * Time.deltaTime;
            move = characterController.transform.TransformDirection(move);
            characterController.Move(move);
        }

        private void Sprint()
        {
            _currentSpeed = sprintAction.action.inProgress
                ? moveSpeed * 2
                : moveSpeed;
        }

        private void Jump()
        {
            if (jumpAction.action.triggered)
            {
                characterController.Move(Vector3.up * 5f);
            }
        }
    }
}