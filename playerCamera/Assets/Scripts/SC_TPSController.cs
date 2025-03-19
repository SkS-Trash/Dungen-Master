using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_TPSController : MonoBehaviour
{
    public float speed = 7.5f;
    public float runSpeed = 12.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public float cameraSmoothness = 5.0f; // Плавность вращения камеры
    public float cameraDistance = 5.0f; // Расстояние камеры от персонажа
    public LayerMask cameraCollisionMask; // Слой для проверки коллизий камеры

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;
    private float currentSpeed;
    private bool isRunning = false;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        currentSpeed = speed;
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }

            // Проверка на бег
            if (Input.GetKey(KeyCode.LeftShift) && canMove)
            {
                isRunning = true;
                currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * 5);
            }
            else
            {
                isRunning = false;
                currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 5);
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);

            // Плавное вращение камеры
            Quaternion targetRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            playerCameraParent.localRotation = Quaternion.Lerp(playerCameraParent.localRotation, targetRotation, cameraSmoothness * Time.deltaTime);

            // Ограничение на движение камеры за спиной персонажа
            Vector3 cameraOffset = -playerCameraParent.forward * cameraDistance;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, cameraOffset.normalized, out hit, cameraDistance, cameraCollisionMask))
            {
                playerCameraParent.position = hit.point;
            }
            else
            {
                playerCameraParent.localPosition = Vector3.Lerp(playerCameraParent.localPosition, cameraOffset, cameraSmoothness * Time.deltaTime);
            }

            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
}
