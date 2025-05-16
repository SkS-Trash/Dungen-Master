using UnityEngine;

namespace Enemy.Core
{
    public abstract class DecisionSO : ScriptableObject
    {
        public abstract bool Decide(StateController c);
    }
}