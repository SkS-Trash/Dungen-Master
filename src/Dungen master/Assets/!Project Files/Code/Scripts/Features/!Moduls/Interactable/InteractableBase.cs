using UnityEngine;

namespace Interactable
{
    public abstract class InteractableBase : MonoBehaviour
    {
        public virtual void OnInteract()
        {
            Debug.Log($"[InteractableBase] Interacted with {gameObject.name}");
        }

        public virtual void OnGainFocus()
        {
            Debug.Log($"[InteractableBase] Gaining focus");
        }

        public virtual void OnLoseFocus()
        {
            Debug.Log($"[InteractableBase] Losing focus");
        }
    }
}