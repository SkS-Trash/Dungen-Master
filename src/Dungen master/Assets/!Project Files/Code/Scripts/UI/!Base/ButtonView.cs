using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonView : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }
        protected Button Button { get; private set; }

        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
            RectTransform = GetComponent<RectTransform>();
        }

        protected virtual void OnEnable() => Button.onClick.AddListener(OnClick);
        protected virtual void OnDisable() => Button.onClick.RemoveListener(OnClick);

        public void InteractableOn() => Button.interactable = true;
        public void InteractableOff() => Button.interactable = false;

        protected abstract void OnClick();
    }
}