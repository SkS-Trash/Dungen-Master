using Enemy.Components;
using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/AttackFromPatternAction ")]
    public class AttackFromPattern : ActionSO
    {
        public override void Act(StateController c)
        {
            var pattern = c.AttackPattern;
            if (!pattern || !c.Player) return;

            var attack = pattern.PickNext(c.transform.position, c.Player.position);

            if (!c.AttackCooldownTracker.IsReady(attack)) return;

            attack?.Execute(c);

            c.AttackCooldownTracker.SetCooldown(attack);
        }
    }
}