using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Reset Damage Flag")]
    public class ResetDamageFlag : ActionSO
    {
        public override void Act(StateController c) => c.Health.WasDamaged = false;
    }
}