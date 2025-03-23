using UnityEngine;

public class SC_CameraCollision : MonoBehaviour
{
    public Transform referenceTransform;
    public float collisionOffset = 0.3f;
    public float cameraSpeed = 15f;

    Vector3 defaultPos;
    Vector3 directionNormalized;
    Transform parentTransform;
    float defaultDistance;

    void Start()
    {
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        Vector3 currentPos = defaultPos;
        RaycastHit hit;
        Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance))
        {
            currentPos = (directionNormalized * (hit.distance - collisionOffset));

            transform.localPosition = currentPos;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * cameraSpeed);
        }
    }
}
