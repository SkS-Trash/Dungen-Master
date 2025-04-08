using UnityEngine;
using System.Collections;

public class ThirdPersonInteractor : MonoBehaviour
{
    [Header("Настройки взаимодействия")]
    public float interactionRange = 2f; 
    public KeyCode interactionKey = KeyCode.E; 
    public LayerMask interactableLayer; 
    public Transform interactionPoint; 

    [Header("Визуализация")]
    public GameObject interactionUI; 

    private Interactable currentInteractable; 

    void Update()
    {
        CheckForInteractables();
        HandleInteractionInput();
    }

    void CheckForInteractables()
    {
        Ray ray = new Ray(interactionPoint.position, interactionPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    if (currentInteractable != null)
                        currentInteractable.OnLoseFocus();

                    currentInteractable = interactable;
                    interactable.OnGainFocus();
                }

                
                if (interactionUI != null)
                    interactionUI.SetActive(true);
            }
            else
            {
                ClearCurrentInteractable();
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }

    void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            currentInteractable.OnInteract();
        }
    }

    void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }

        
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (interactionPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(interactionPoint.position, interactionPoint.position + interactionPoint.forward * interactionRange);
        }
    }
}