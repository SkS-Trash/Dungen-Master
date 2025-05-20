using Progress;
using Services.Progress;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Components
{
    public class ThirdPersonController : MonoBehaviour,
        ILevelProgressLoadEvent, ILevelProgressCollector
    {
        [SerializeField] private Transform camera;
        [SerializeField] private float turnSmoothVelocity = 2f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [Space]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [Space] 
        [SerializeField] private PlayerAnimator animator;
        [Space] 
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;

        private Vector2 _moveInput;
        private Vector3 _moveVelocity;
        private float _currentSpeed;

        public void Start()
        {
            _currentSpeed = moveSpeed;
        }

        private void OnEnable()
        {
            EventBus.Subscribe(this);
            LevelProgressSaveCollectorsProvider.Instance.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
            LevelProgressSaveCollectorsProvider.Instance.Unsubscribe(this);
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
            var target = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg + camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(camera.transform.eulerAngles.y, target, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * angle;
        }

        private void Move()
        {
            _moveVelocity = new Vector3(_moveInput.x, 0, _moveInput.y);
            _moveVelocity = Vector3.ClampMagnitude(_moveVelocity, 1f);
            _moveVelocity *= _currentSpeed * Time.deltaTime;
            _moveVelocity = camera.transform.TransformDirection(_moveVelocity);
            _moveVelocity.y = 0;
            
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

        public void OnProgressLoaded(LevelSaveData progress)
        {
            characterController.enabled = false;
            transform.position = progress.player.position;
            transform.rotation = progress.player.rotation;
            characterController.enabled = true;
        }

        public void Collect(LevelSaveData target)
        {
            target.player.position = transform.position;
            target.player.rotation = transform.rotation;
        }
    }
}