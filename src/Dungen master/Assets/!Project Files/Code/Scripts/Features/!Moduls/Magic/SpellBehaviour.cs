using UnityEngine;

namespace Magic
{
    public class SpellBehaviour : MonoBehaviour
    {
        private Spell _spell;
        private bool _isMoving;

        public void SetSpell(Spell spell)
        {
            _spell = spell;
            _isMoving = spell.Speed != 0;
        }

        private void Update()
        {
            if (_isMoving)
            {
                transform.Translate(Vector3.forward * (_spell.Speed * Time.deltaTime));
            }
        }
    }
}