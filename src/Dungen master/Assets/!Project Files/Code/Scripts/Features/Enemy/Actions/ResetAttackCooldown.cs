using Enemy.Components;
using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Reset Attack Cooldown")]
    public class ResetAttackCooldown : ActionSO
    {
        public override void Act(StateController c)
        {
            c.AttackCooldownTracker.ResetCooldown();
        }
    }
}