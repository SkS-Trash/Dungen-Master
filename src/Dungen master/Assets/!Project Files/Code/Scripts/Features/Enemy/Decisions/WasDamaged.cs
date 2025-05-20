using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Was Damaged")]
    public class WasDamaged : DecisionSO
    {
        public override bool Decide(StateController c)
        {
            if (c.Health.WasDamaged)
            {
                c.Health.WasDamaged = false;
                return true;
            }

            return false;
        }
    }
}