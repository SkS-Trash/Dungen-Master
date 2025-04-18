using UnityEngine;

namespace Player
{
    public abstract class Interactable : MonoBehaviour
    {
        public virtual void OnInteract()
        {
            Debug.Log("�������������� � " + gameObject.name);
        }

        public virtual void OnGainFocus()
        {
            Debug.Log("������ � ������: " + gameObject.name);
        }

        public virtual void OnLoseFocus()
        {
            Debug.Log("������ ������� �����: " + gameObject.name);
        }
    }
}