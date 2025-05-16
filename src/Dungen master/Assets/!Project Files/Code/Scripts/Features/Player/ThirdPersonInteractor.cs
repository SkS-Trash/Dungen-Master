using Interactable;
using Observers.Input;
using UnityEngine;

namespace Player
{
    public interface IInteractableFocusEvent : IGlobalSubscriber
    {
        void OnInteractableFocus(InteractableBase interactable);
        void OnInteractableLoseFocus();
    }

    public class ThirdPersonInteractor : MonoBehaviour
    {
        [SerializeField] private float interactionRange = 2f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private Transform interactionPoint;
        [Space] [SerializeField] private InputActionReader inputActionReader;

        private InteractableBase _currentInteractableBase;

        private void OnEnable()
        {
            inputActionReader.OnInteractChanged += OnInteractionInput;
        }

        private void OnDisable()
        {
            inputActionReader.OnInteractChanged -= OnInteractionInput;
        }

        private void Update()
        {
            CheckForInteractables();
        }

        private void CheckForInteractables()
        {
            var ray = new Ray(interactionPoint.position, interactionPoint.forward);

            if (Physics.Raycast(ray, out var hit, interactionRange, interactableLayer) &&
                hit.collider.TryGetComponent<InteractableBase>(out var interactableBase))
            {
                if (interactableBase == _currentInteractableBase) return;

                ClearCurrentInteractable();

                _currentInteractableBase = interactableBase;

                interactableBase.OnGainFocus();

                EventBus.RaiseEvent<IInteractableFocusEvent>(x =>
                    x.OnInteractableFocus(_currentInteractableBase));

                return;
            }

            ClearCurrentInteractable();
        }

        private void OnInteractionInput(bool isActive)
        {
            if (!isActive)
                return;

            if (_currentInteractableBase)
                _currentInteractableBase.OnInteract();
        }

        private void ClearCurrentInteractable()
        {
            if (!_currentInteractableBase) return;

            _currentInteractableBase.OnLoseFocus();
            _currentInteractableBase = null;

            EventBus.RaiseEvent<IInteractableFocusEvent>(x => x.OnInteractableLoseFocus());
        }

        private void OnDrawGizmosSelected()
        {
            if (!interactionPoint)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(interactionPoint.position,
                interactionPoint.position + interactionPoint.forward * interactionRange);
        }
    }
}