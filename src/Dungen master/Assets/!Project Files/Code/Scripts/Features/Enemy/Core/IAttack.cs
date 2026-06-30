using UnityEngine;

namespace Enemy.Core
{
    public interface IAttack
    {
        void Execute(GameObject attacker, GameObject target);
    }
}