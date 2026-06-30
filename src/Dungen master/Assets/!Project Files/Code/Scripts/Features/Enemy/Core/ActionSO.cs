using UnityEngine;

namespace Enemy.Core
{
    public abstract class ActionSO : ScriptableObject
    {
        public abstract void Act(StateController c);
    }
}