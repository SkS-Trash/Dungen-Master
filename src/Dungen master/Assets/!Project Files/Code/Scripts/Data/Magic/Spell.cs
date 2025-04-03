using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Magic
{
    public abstract class Spell : ScriptableObject
    {
        [field: SerializeField] public virtual string SpellName { get; protected set; }
        [field: SerializeField] public virtual AssetReference SpellPrefab { get; protected set; }
        [field: SerializeField] public virtual float Cooldown { get; protected set; }

        public abstract void Cast(Transform castPoint);
    }
}