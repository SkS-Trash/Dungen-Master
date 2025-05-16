using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Magic.Data
{
    [CreateAssetMenu(menuName = "Data/Spells/MagicBallSpell")]
    public class MagicBallSpell : Spell
    {
        [SerializeField] private int damage = 10;

        public override async void Cast(UnitType[] targetUnits, Vector3 spawnPosition, Vector3 targetPosition)
        {
            var spellInstance = await Addressables.InstantiateAsync(SpellPrefab).Task;

            spellInstance.transform.position = spawnPosition;
            spellInstance.transform.rotation = Quaternion.LookRotation(targetPosition - spawnPosition);

            var spell = spellInstance.AddComponent<SpellBehaviour>();
            spell.SetSpell(this);

            var damageMarker = spellInstance.AddComponent<SpellDamageMarker>();
            damageMarker.SetDamage(damage);
            damageMarker.SetUnitType(targetUnits);
        }
    }
}