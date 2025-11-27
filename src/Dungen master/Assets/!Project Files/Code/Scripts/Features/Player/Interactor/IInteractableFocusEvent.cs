using Interactable;

namespace Player.Interactor
{
    public interface IInteractableFocusEvent : IGlobalSubscriber
    {
        void OnInteractableFocus(InteractableBase interactable);
        void OnInteractableLoseFocus();
    }
}