using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseUI : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            _canvasGroup.SetVisibility(true);
        }

        public virtual void Hide()
        {
            _canvasGroup.SetVisibility(false);
        }
    }
}