using Observers.Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class TPSController : MonoBehaviour
    {
        [SerializeField] private InputActionReader inputActionReader;
        [Space] 
        [SerializeField] private float speed = 7.5f;
        [SerializeField] private float runSpeed = 12.0f;
        [Space] 
        [SerializeField] private float jumpSpeed = 8.0f;
        [SerializeField] private float gravity = 20.0f;
        [Space] 
        [SerializeField] private Transform playerCameraParent;
        [SerializeField] private float lookSpeed = 2.0f;
        [SerializeField] private float lookXLimit = 60.0f;
        [SerializeField] private float cameraSmoothness = 5.0f;
        [SerializeField] private float cameraDistance = 5.0f;
        [SerializeField] private LayerMask cameraCollisionMask;

        private CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _rotation = Vector2.zero;

        private float _currentSpeed;

        private bool _isRunning = false;
        private bool _canMove = true;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _rotation.y = transform.eulerAngles.y;
            _currentSpeed = speed;
        }

        private void Update()
        {
            if (_characterController.isGrounded)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                float curSpeedX = _canMove ? _currentSpeed * inputActionReader.MoveValue.y : 0;
                float curSpeedY = _canMove ? _currentSpeed * inputActionReader.MoveValue.x : 0;
                _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                if (inputActionReader.IsJumping && _canMove)
                {
                    _moveDirection.y = jumpSpeed;
                }

                if (inputActionReader.IsSprinting && _canMove)
                {
                    _isRunning = true;
                    _currentSpeed = Mathf.Lerp(_currentSpeed, runSpeed, Time.deltaTime * 5);
                }
                else
                {
                    _isRunning = false;
                    _currentSpeed = Mathf.Lerp(_currentSpeed, speed, Time.deltaTime * 5);
                }
            }

            _moveDirection.y -= gravity * Time.deltaTime;

            _characterController.Move(_moveDirection * Time.deltaTime);

            if (_canMove)
            {
                _rotation.y += inputActionReader.LookValue.x * lookSpeed;
                _rotation.x += -inputActionReader.LookValue.y * lookSpeed;
                _rotation.x = Mathf.Clamp(_rotation.x, -lookXLimit, lookXLimit);

                Quaternion targetRotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
                playerCameraParent.localRotation = Quaternion.Lerp(playerCameraParent.localRotation, targetRotation,
                    cameraSmoothness * Time.deltaTime);

                Vector3 cameraOffset = -playerCameraParent.forward * cameraDistance;
                if (Physics.Raycast(transform.position, cameraOffset.normalized, out var hit, cameraDistance,
                        cameraCollisionMask))
                {
                    playerCameraParent.position = hit.point;
                }
                else
                {
                    playerCameraParent.localPosition = Vector3.Lerp(playerCameraParent.localPosition, cameraOffset,
                        cameraSmoothness * Time.deltaTime);
                }

                transform.eulerAngles = new Vector2(0, _rotation.y);
            }
        }

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
        }
    }
}