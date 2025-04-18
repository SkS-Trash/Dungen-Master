using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Magic
{
    [CreateAssetMenu(menuName = "Data/Spells/MagicBallSpell")]
    public class MagicBallSpell : Spell
    {
        public override async void Cast(Vector3 spawnPosition, Vector3 targetPosition)
        {
            var spellInstance = await Addressables.InstantiateAsync(SpellPrefab).Task;

            spellInstance.transform.position = spawnPosition;
            spellInstance.transform.rotation = Quaternion.LookRotation(targetPosition - spawnPosition);
            
            var spell = spellInstance.AddComponent<SpellBehaviour>();
            spell.SetSpell(this);
        }
    }
}