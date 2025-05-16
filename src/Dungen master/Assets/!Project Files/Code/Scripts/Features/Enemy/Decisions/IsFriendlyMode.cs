using Enemy.Core;
using Settings.AudioVideoOptionsMenu;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Is Friendly Mode")]
    public class IsFriendlyMode : DecisionSO
    {
        public override bool Decide(StateController c) => GameMode.IsFriendly;
    }
}