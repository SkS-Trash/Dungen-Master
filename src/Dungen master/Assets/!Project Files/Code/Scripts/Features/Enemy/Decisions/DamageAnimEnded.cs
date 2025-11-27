using Enemy.Components.Events;
using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Damage Anim Ended")]
    public class DamageAnimEnded : DecisionSO
    {
        public override bool Decide(StateController c) =>
            c.AnimationEvents.LastEvent == EnemyAnimationEvents.AnimationEventType.GetHitEnd;
    }
}