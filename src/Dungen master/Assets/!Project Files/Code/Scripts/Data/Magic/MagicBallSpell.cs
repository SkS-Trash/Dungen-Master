using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Magic
{
    [CreateAssetMenu(menuName = "Data/Spells/MagicBallSpell")]
    public class MagicBallSpell : Spell
    {
        [SerializeField] private float speed = 1f;

        public override async void Cast(Transform castPoint)
        {
            var position = castPoint.position;
            var rotation = castPoint.rotation;
            var forward = castPoint.forward;
            
            var spellInstance = await Addressables.InstantiateAsync(SpellPrefab).Task;
            
            spellInstance.transform.position = position;
            spellInstance.transform.rotation = rotation;
            
            var rb = spellInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = forward * speed;
            }

            Destroy(spellInstance, 5f);
        }
    }
}