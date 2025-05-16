using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Magic.Data
{
    public abstract class Spell : ScriptableObject
    {
        [field: SerializeField] public virtual string SpellName { get; protected set; }
        [field: SerializeField] public virtual AssetReference SpellPrefab { get; protected set; }
        [field: SerializeField] public virtual float Cooldown { get; protected set; }

        [field: SerializeField] public float Speed { get; protected set; } = 1f;

        public abstract void Cast(UnitType[] targetUnits, Vector3 spawnPosition, Vector3 targetPosition);
    }
}