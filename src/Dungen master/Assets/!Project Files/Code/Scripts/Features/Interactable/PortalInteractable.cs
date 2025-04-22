using UnityEngine;

namespace Interactable
{
    public class PortalInteractable : InteractableBase
    {
        public override void OnInteract()
        {
            Debug.Log($"[PortalInteractable] Interacted with {gameObject.name}");
        }
    }
}