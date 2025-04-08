using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void OnInteract()
    {
        Debug.Log("Взаимодействие с " + gameObject.name);
    }

    public virtual void OnGainFocus()
    {
        Debug.Log("Объект в фокусе: " + gameObject.name);
    }

    public virtual void OnLoseFocus()
    {
        Debug.Log("Объект потерял фокус: " + gameObject.name);
    }
}