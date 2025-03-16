public static class CanvasGroupExtensionMethod
{
    public static void SetVisibility(this UnityEngine.CanvasGroup canvasGroup, bool isVisible)
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.blocksRaycasts = isVisible;
        canvasGroup.interactable = isVisible;
    }
}