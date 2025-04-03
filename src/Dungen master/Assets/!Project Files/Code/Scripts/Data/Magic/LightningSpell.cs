using UnityEngine;

namespace Magic
{
    [CreateAssetMenu(menuName = "Data/Spells/LightningSpell")]
    public class LightningSpell : Spell
    {
        public float radius = 5f;
        public GameObject lightningEffectPrefab;

        public override void Cast(Transform castPoint)
        {
            // Для молнии можно, например, создать эффект в заданном радиусе
            var effectInstance = Instantiate(lightningEffectPrefab, castPoint.position, Quaternion.identity);
            // Реализуйте логику поиска врагов и нанесения урона в радиусе
        }
    }
}