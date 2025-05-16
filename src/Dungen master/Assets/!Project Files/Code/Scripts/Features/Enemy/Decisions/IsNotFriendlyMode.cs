using Enemy.Core;
using Settings.AudioVideoOptionsMenu;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Is Not Friendly Mode")]
    public class IsNotFriendlyMode : DecisionSO
    {
        public override bool Decide(StateController c) => !GameMode.IsFriendly;
    }
}