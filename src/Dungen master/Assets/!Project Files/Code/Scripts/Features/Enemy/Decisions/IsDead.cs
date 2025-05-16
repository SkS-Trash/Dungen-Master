using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Is Dead")]
    public class IsDead : DecisionSO
    {
        public override bool Decide(StateController c) => c.Health.CurrentHealth <= 0;
    }
}