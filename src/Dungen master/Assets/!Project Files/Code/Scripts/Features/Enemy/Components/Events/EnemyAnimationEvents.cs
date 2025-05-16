using Animation;

namespace Enemy.Components.Events
{
    public class EnemyAnimationEvents : AnimationEvent<EnemyAnimationEvents.AnimationEventType>
    {
        public enum AnimationEventType
        {
            EnablePhysicalAttack,
            DisablePhysicalAttack,
            LaunchMagicAttack,

            GetHitEnd,
            DeathEnd,
            AttackEnd,
        }
    }
}