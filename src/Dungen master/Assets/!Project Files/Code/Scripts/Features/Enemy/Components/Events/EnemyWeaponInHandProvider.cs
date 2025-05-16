using UnityEngine;
using Weapon;

namespace Enemy.Components.Events
{
    public class EnemyWeaponInHandProvider : MonoBehaviour
    {
        [SerializeField] private EnemyAnimationEvents animationEvents;
        [SerializeField] private WeaponInHandController weaponCollider;

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
                case EnemyAnimationEvents.AnimationEventType.EnablePhysicalAttack:
                    weaponCollider.EnableWeaponCollider();
                    break;
                case EnemyAnimationEvents.AnimationEventType.DisablePhysicalAttack:
                    weaponCollider.DisableWeaponCollider();
                    break;
                case EnemyAnimationEvents.AnimationEventType.AttackEnd:
                    weaponCollider.DisableWeaponCollider();
                    break;
            }
        }
    }
}