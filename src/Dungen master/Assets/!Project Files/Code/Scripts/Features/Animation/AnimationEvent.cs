using System;
using UnityEngine;

namespace Animation
{
    public abstract class AnimationEvent<TEventType> : MonoBehaviour where TEventType : Enum
    {
        public event Action<TEventType> OnAnimationEvent;
        
        // Этот метод вызывается из анимационного события в аниматоре.
        public void AnimationEventHandler(string eventName)
        {
            var eventType = Enum.Parse(typeof(TEventType), eventName);
            if (eventType is TEventType eventEnum)
            {
                Debug.Log(eventEnum.ToString());
                OnAnimationEvent?.Invoke(eventEnum);
                return;
            }

            Debug.LogError($"Имя события '{eventName}' не соответствует ни одному значению " +
                           $"перечисления типа '{typeof(TEventType)}'.");
        }
    }
}