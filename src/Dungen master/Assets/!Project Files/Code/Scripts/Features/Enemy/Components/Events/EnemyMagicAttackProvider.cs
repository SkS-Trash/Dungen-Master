using Magic;
using UnityEngine;

namespace Enemy.Components.Events
{
    public class EnemyMagicAttackProvider : MonoBehaviour
    {
        [SerializeField] private EnemyAnimationEvents animationEvents;
        [SerializeField] private MagicCastController magicCastController;

        private void Awake()
        {
            animationEvents ??= GetComponentInChildren<EnemyAnimationEvents>();
        }

        public void OnEnable()
        {
            if (animationEvents)
                animationEvents.OnAnimationEvent += AnimationEventHandler;
        }

        public void OnDisable()
        {
            if (animationEvents)
                animationEvents.OnAnimationEvent -= AnimationEventHandler;
        }

        private void AnimationEventHandler(EnemyAnimationEvents.AnimationEventType eventType)
        {
            switch (eventType)
            {
                case EnemyAnimationEvents.AnimationEventType.LaunchMagicAttack:
                    magicCastController.CastSpell();
                    break;
            }
        }
    }
}