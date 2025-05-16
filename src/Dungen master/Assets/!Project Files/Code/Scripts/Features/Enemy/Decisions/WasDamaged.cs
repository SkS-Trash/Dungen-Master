using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Was Damaged")]
    public class WasDamaged : DecisionSO
    {
        public override bool Decide(StateController c) => c.Health.WasDamaged;
    }
}